﻿using PathOfTerraria.Common.Systems;
using PathOfTerraria.Common.Systems.DisableBuilding;
using PathOfTerraria.Common.World.Generation;
using PathOfTerraria.Content.Tiles.BossDomain;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.WorldBuilding;

namespace PathOfTerraria.Common.Subworlds.BossDomains;

public class WallOfFleshDomain : BossDomainSubworld
{
	public override int Width => 1800;
	public override int Height => 250;
	public override int[] WhitelistedCutTiles => [TileID.Pots, TileID.CrimsonThorns, ModContent.TileType<FrayedRope>()];
	public override int[] WhitelistedMiningTiles => [ModContent.TileType<FrayedRope>()];
	public override int DropItemLevel => 30;

	public bool BossSpawned = false;
	public bool ReadyToExit = false;
	public bool LeftBlocked = true;

	public override List<GenPass> Tasks => [new PassLegacy("Reset", ResetStep), new	PassLegacy("Base Terrain", Terrain),
		new PassLegacy("Arenas", SpawnArenas), new PassLegacy("Settle Liquids", SettleLiquids)];

	private void SpawnArenas(GenerationProgress progress, GameConfiguration configuration)
	{
		int minX = 0;
		int maxX = Main.spawnTileX;

		if (LeftBlocked)
		{
			minX = Main.spawnTileX;
			maxX = Width;
		}

		int sixth = (maxX - minX) / 6;

		for (int i = 0; i < 4; ++i)
		{
			int x = minX + sixth * (i + 1);

			PlaceArena(x);
		}
	}

	private void PlaceArena(int x)
	{
		int id = 0;

		Point16 size = new();
		StructureHelper.Generator.GetDimensions("Assets/Structures/WoFDomain/Arena_" + id, Mod, ref size);

		x -= size.X / 2;

		for (int i = x; i < x + size.X; ++i)
		{
			for (int j = 0; j < Height; ++j)
			{
				Tile tile = Main.tile[i, j];
				tile.HasTile = true;
				tile.TileType = TileID.ObsidianBrick;
			}
		}

		StructureTools.PlaceByOrigin("Assets/Structures/WoFDomain/Arena_" + id, new Point16(x, (int)(Height * 0.35f)), Vector2.Zero, null, false);
	}

	/// <summary>
	/// Copied from vanilla's Settle Liquids generation step.
	/// </summary>
	/// <param name="progress"></param>
	/// <param name="configuration"></param>
	private void SettleLiquids(GenerationProgress progress, GameConfiguration configuration)
	{
		progress.Message = Lang.gen[27].Value;
		
		Liquid.worldGenTilesIgnoreWater(ignoreSolids: true);
		Liquid.QuickWater(3);
		WorldGen.WaterCheck();
		Liquid.quickSettle = true;

		int repeats = 0;
		int maxRepeats = 10;

		while (repeats < maxRepeats)
		{
			int liquidAmount = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
			repeats++;
			double currentSteps = 0.0;
			int forcedStop = liquidAmount * 5;
			while (Liquid.numLiquid > 0)
			{
				forcedStop--;
				if (forcedStop < 0)
				{
					break;
				}

				double stepsRemaining = (double)(liquidAmount - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / liquidAmount;

				if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > liquidAmount)
				{
					liquidAmount = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
				}

				if (stepsRemaining > currentSteps)
				{
					currentSteps = stepsRemaining;
				}
				else
				{
					stepsRemaining = currentSteps;
				}

				if (repeats == 1)
				{
					progress.Set(stepsRemaining / 3.0 + 0.33);
				}

				Liquid.UpdateLiquid();
			}

			WorldGen.WaterCheck();
			progress.Set(repeats * 0.1 / 3.0 + 0.66);
		}

		Liquid.quickSettle = false;
		Liquid.worldGenTilesIgnoreWater(ignoreSolids: false);
		Main.tileSolid[484] = false;

		AddCrucible();
	}

	public override void OnEnter()
	{
		base.OnEnter();

		SubworldSystem.hideUnderworld = false;
	}

	private void Terrain(GenerationProgress progress, GameConfiguration configuration)
	{
		progress.Message = Language.GetTextValue($"Mods.{PoTMod.ModName}.Generation.Terrain");

		Main.worldSurface = 2;
		Main.rockLayer = 6;
		Main.spawnTileY += 15;

		GetNoises(out FastNoiseLite noise, out FastNoiseLite softNoise, out FastNoiseLite wallNoise,
			out FastNoiseLite smallWallNoise, out FastNoiseLite wallTypeNoise);

		HashSet<Point> hellStonePoints = [];
		HashSet<Point> houseLocations = [];
		HashSet<Point> lavaLocations = [];
		HashSet<Point> fleshLocations = [];

		LeftBlocked = WorldGen.genRand.NextBool();
		BossSpawned = false;
		ReadyToExit = false;

		for (int i = 0; i < Width; ++i)
		{
			int minY = Main.maxTilesY - 160 + (int)(noise.GetNoise(i, 0) * 40);
			int maxY = Main.maxTilesY - (int)(noise.GetNoise(i, 1200) * 80) - 120;
			int offsetY = (int)(wallTypeNoise.GetNoise(i, -200) * 18f);

			minY = Math.Max(Main.maxTilesY - 150 + (int)(softNoise.GetNoise(i, 0) * 60), minY) + offsetY;
			maxY = Math.Min(Main.maxTilesY - (int)(softNoise.GetNoise(i, 1200) * 80) - 120, maxY) + (int)(wallTypeNoise.GetNoise(i, 200) * 18f);

			progress.Value = (float)i / Width - 1;

			for (int j = 0; j < Height; ++j)
			{
				Tile tile = Main.tile[i, j];

				if (IsWallX(i, LeftBlocked))
				{
					fleshLocations.Add(new Point(i, j));
				}

				if (j < minY || j > maxY)
				{
					tile.HasTile = true;
					tile.TileType = TileID.Ash;

					if (IsWallX(i, LeftBlocked))
					{
						float mul;
						int absOffset = (int)(Math.Abs(j - (Main.maxTilesY - 130)) * 0.65f);

						if (absOffset < 20)
						{
							mul = 0;
						}
						else
						{
							mul = MathHelper.Clamp(1 - (absOffset - 20) / 50f, 0, 1);
						}

						if (mul > WorldGen.genRand.NextFloat())
						{
							fleshLocations.Add(new Point(i, j));
						}
						else
						{
							fleshLocations.Remove(new Point(i, j));
						}
					}

					if (j > maxY)
					{
						if (WorldGen.genRand.NextBool(250))
						{
							hellStonePoints.Add(new Point(i, j));
						}
						else if (WorldGen.genRand.NextBool(200) && OpenExtensions.GetOpenings(i, j).HasFlag(OpenFlags.Above) &&
							NoNearbyHouses(i, j, houseLocations) && i > 40 && j < Height - 40 && i < Width - 40)
						{
							houseLocations.Add(new Point(i, j));
						}
					}
				}
				else
				{
					if (j > Height - 70)
					{
						tile.LiquidType = LiquidID.Lava;
						tile.LiquidAmount = 255;
					}

					if (WorldGen.genRand.NextBool(3000))
					{
						lavaLocations.Add(new Point(i, j));
					}
				}

				if (j + offsetY < 10 || j + offsetY >= Height - 10)
				{
					continue;
				}

				tile = Main.tile[i, j + offsetY];

				if (FadeNoise(wallNoise, i, j + offsetY) <= -0.35f || FadeNoise(smallWallNoise, i, j + offsetY) <= -0.25f)
				{
					tile.WallType = (ushort)(WallID.LavaUnsafe1 + GetWallType(wallTypeNoise, i, j));
				}
				else
				{
					tile.WallType = WallID.None;
				}
			}
		}

		foreach (Point pos in lavaLocations)
		{
			WorldGen.digTunnel(pos.X, pos.Y, 0, 0, 5, 12, true);
		}

		foreach (Point pos in hellStonePoints)
		{
			WorldGen.TileRunner(pos.X, pos.Y, WorldGen.genRand.NextFloat(4, 12), WorldGen.genRand.Next(2, 40), TileID.Hellstone);
		}

		foreach (Point pos in houseLocations)
		{
			WorldGen.HellHouse(pos.X, pos.Y, (byte)TileID.ObsidianBrick, (byte)WallID.ObsidianBrickUnsafe);
		}

		foreach (Point pos in fleshLocations)
		{
			Tile tile = Main.tile[pos];
			tile.HasTile = true;
			tile.TileType = TileID.FleshBlock;
		}

		for (int i = 0; i < Width; ++i)
		{
			for (int j = 0; j < Height; ++j)
			{
				Tile tile = Main.tile[i, j];

				if (tile.TileType == TileID.Platforms) // WorldGen.HellHouse doesn't do this for its own platforms for some reason
				{
					tile.TileFrameY = 234;
				}

				if (tile.LiquidType == LiquidID.Water)
				{
					tile.LiquidType = LiquidID.Lava;
				}
			}
		}

		StructureTools.PlaceByOrigin("Assets/Structures/WoFDomain/Spawn", new Point16(Main.spawnTileX, Main.spawnTileY), new Vector2(0.5f, 0), null, false);
		return;

		static int GetWallType(FastNoiseLite wallTypeNoise, int i, int j)
		{
			int type = (ushort)(wallTypeNoise.GetNoise(i, j) * 100f) / 25;

			return type switch
			{
				0 => 0,
				1 => 1,
				2 => 2,
				_ => 3
			};
		}

		float FadeNoise(FastNoiseLite wallNoise, int i, int j)
		{
			int absOffset = Math.Abs(j - (Main.maxTilesY - 130));
			float noise = wallNoise.GetNoise(i, j + 200);

			if (!IsBehindWall(i, LeftBlocked))
			{
				float mul;

				if (absOffset < 20)
				{
					return 0;
				}
				else
				{
					mul = MathHelper.Clamp(1 - (absOffset - 20) / 50f, 0, 1);
				}

				return MathHelper.Lerp(noise, 0, mul);
			}
			else
			{
				return noise;
			}
		}
	}

	private void AddCrucible()
	{
		int crucibleX = LeftBlocked ? Width - 70 : 70;
		int crucibleY = Main.spawnTileY;

		while (!WorldGen.SolidTile(crucibleX, crucibleY) && Main.tile[crucibleX, crucibleY].LiquidAmount <= 50)
		{
			crucibleY++;
		}

		crucibleY -= 10;
		StructureTools.PlaceByOrigin("Assets/Structures/WoFDomain/Crucible", new Point16(crucibleX, crucibleY), new Vector2(0.5f, 0.5f), null, false);

		crucibleX--;
		crucibleY -= 5;
		int count = 0;

		while (!WorldGen.SolidTile(crucibleX, crucibleY))
		{
			if (count == 0)
			{
				for (int i = 0; i < 2; ++i)
				{
					Tile tile = Main.tile[crucibleX, crucibleY + i];
					tile.TileType = (ushort)ModContent.TileType<VoodooRope>();
					tile.HasTile = true;
				}
			}
			else
			{
				Tile tile = Main.tile[crucibleX, crucibleY];
				tile.TileType = (ushort)(count == 16 ? ModContent.TileType<FrayedRope>() : TileID.Rope);
				tile.HasTile = true;
			}

			crucibleY--;
			count++;
		}
	}

	private static bool IsWallX(int i, bool leftBlocked)
	{
		const int OuterEdge = 76;
		const int InnerEdge = 64;

		if (leftBlocked)
		{
			return i > Main.spawnTileX - OuterEdge && i < Main.spawnTileX - InnerEdge;
		}

		return i > Main.spawnTileX + InnerEdge && i < Main.spawnTileX + OuterEdge;
	}

	private static bool IsBehindWall(int i, bool leftBlocked)
	{
		if (leftBlocked)
		{
			return i < Main.spawnTileX - 65;
		}

		return i > Main.spawnTileX + 65;
	}

	private static bool NoNearbyHouses(int i, int j, HashSet<Point> houseLocations)
	{
		foreach (Point pos in houseLocations)
		{
			if (pos.ToVector2().Distance(new Vector2(i, j)) < 100)
			{
				return true;
			}
		}

		return true;
	}

	private static void GetNoises(out FastNoiseLite noise, out FastNoiseLite softNoise, out FastNoiseLite wallNoise, out FastNoiseLite smallWallNoise, out FastNoiseLite wallTypeNoise)
	{
		noise = new FastNoiseLite(WorldGen._genRandSeed);
		noise.SetFrequency(0.03f);
		noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);

		softNoise = new FastNoiseLite(WorldGen._genRandSeed);
		softNoise.SetFrequency(0.08f);
		softNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);

		wallNoise = new FastNoiseLite(WorldGen._genRandSeed);
		wallNoise.SetFrequency(0.04f);
		wallNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

		smallWallNoise = new FastNoiseLite(WorldGen._genRandSeed + 1);
		smallWallNoise.SetFrequency(0.1f);
		smallWallNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

		wallTypeNoise = new FastNoiseLite(WorldGen._genRandSeed + 2);
		wallTypeNoise.SetFrequency(0.01f);
		wallTypeNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
	}

	public override void Update()
	{
		Liquid.UpdateLiquid();

		Main.dayTime = false;
		Main.time = Main.nightLength / 2;
		Main.moonPhase = (int)MoonPhase.Full;

		foreach (Player player in Main.ActivePlayers)
		{
			player.GetModPlayer<StopBuildingPlayer>().ConstantStopBuilding = true;
		}

		if (NPC.AnyNPCs(NPCID.WallofFlesh))
		{
			if (!BossSpawned) // Remove all flesh blocks
			{
				for (int i = Main.spawnTileX - 100; i < Main.spawnTileX + 100; i++)
				{
					for (int j = 0; j < Height; ++j)
					{
						Tile tile = Main.tile[i, j];

						if (tile.TileType == TileID.FleshBlock)
						{
							tile.HasTile = false;

							if (Main.netMode == NetmodeID.Server)
							{
								NetMessage.SendTileSquare(-1, i, j);
							}

							WorldGen.SquareTileFrame(i, j, true);
						}
					}
				}
			}
			
			BossSpawned = true;
		}

		if (BossSpawned && !NPC.AnyNPCs(NPCID.WallofFlesh) && !ReadyToExit)
		{
			BossTracker.CachedBossesDowned.Add(NPCID.WallofFlesh);
			ReadyToExit = true;
		}
	}

	/// <summary>
	/// Used to clamp camera to hide the other side of the domain for a "wow" factor.
	/// </summary>
	public class WoFDomainSystem : ModSystem
	{
		public override void ModifyScreenPosition()
		{
			if (SubworldSystem.Current is WallOfFleshDomain domain && !domain.BossSpawned)
			{
				if (domain.LeftBlocked)
				{
					if (Main.screenPosition.X / 16f < Main.spawnTileX - 70)
					{
						Main.screenPosition.X = (Main.spawnTileX - 70) * 16;
					}
				}
				else
				{
					if ((Main.screenPosition.X + Main.screenWidth) / 16f > Main.spawnTileX + 70)
					{
						Main.screenPosition.X = (Main.spawnTileX + 70) * 16 - Main.screenWidth;
					}
				}
			}
		}
	}
}