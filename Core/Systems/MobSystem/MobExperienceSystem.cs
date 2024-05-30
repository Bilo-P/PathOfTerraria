﻿using PathOfTerraria.Content.Items.Gear;
using PathOfTerraria.Core.Systems.Experience;
using PathOfTerraria.Data;
using PathOfTerraria.Data.Models;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace PathOfTerraria.Core.Systems.MobSystem;

public class MobExperienceSystem : GlobalNPC
{
	/// <summary>
	/// Handles the experience gained from killing a mob
	/// </summary>
	/// <param name="npc"></param>
	public override void OnKill(NPC npc)
	{

		MobAPRGSystem npcSystem = npc.GetGlobalNPC<MobAPRGSystem>();
		int amount = npcSystem._experience ?? (int)Math.Max(1, npc.lifeMax * 0.25f);
		amount =
			npcSystem.Rarity
				switch //We will need to evaluate this as magic/rare natively get more HP. So we do even want this? Was just POC, maybe just change amount evaluation?
				{
					MobRarity.Rare => Convert.ToInt32(amount * 1.1) //Rare mobs give 10% increase xp
					,
					MobRarity.Magic => Convert.ToInt32(amount * 1.05) //Magic mobs give 5% increase xp
					,
					_ => amount
				};
		
		foreach (Player player in Main.player.Where(n =>
			         n.active && Vector2.DistanceSquared(n.Center, npc.Center) < Math.Pow(2000, 2)))
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				ExperienceTracker.SpawnExperience(amount, npc.Center, 6f, player.whoAmI);
			}
			else
			{
				Networking.Networking.SendSpawnExperienceOrbs(-1, player.whoAmI, amount, npc.Center, 6f);
			}
		}
	}
}