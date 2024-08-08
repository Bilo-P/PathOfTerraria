﻿using PathOfTerraria.Common.World.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace PathOfTerraria.Common.Subworlds.Passes;

public class FlatWorldPass(int floorY = 500, bool spawnWalls = false, FastNoiseLite surfaceNoise = null) : GenPass("Terrain", 1)
{
	public readonly int FloorY = floorY;
	public readonly bool SpawnWalls = spawnWalls;
	public readonly FastNoiseLite Noise = surfaceNoise;

	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
	{
		progress.Message = "ENTERING TIER X MAP"; // Sets the text displayed for this pass
		Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
		Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds
		for (int x = 0; x < Main.maxTilesX; x++)
		{
			for (int y = 0; y < Main.maxTilesY; y++)
			{
				progress.Set((y + x * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY)); // Controls the progress bar, should only be set between 0f and 1f
				Tile tile = Main.tile[x, y];
				int floorY = (int)(FloorY + (Noise is null ? 0 : Noise.GetNoise(x, 0) * 4f));

				if (y <= floorY)
				{
					continue; // Stop tiles from being placed above the floor
				}

				tile.HasTile = true;
				tile.TileType = TileID.Stone;

				if (SpawnWalls && y > floorY + 1)
				{
					tile.WallType = WallID.Stone;
				}
			}
		}
	}
}