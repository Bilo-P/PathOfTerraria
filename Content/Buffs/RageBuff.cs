﻿namespace PathOfTerraria.Content.Buffs;

public sealed class RageBuff : ModBuff
{
	private sealed class GlobalNPCImpl : GlobalNPC
	{
		public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
		{
			base.ModifyHitPlayer(npc, target, ref modifiers);

			if (!target.HasBuff<RageBuff>())
			{
				return;
			}

			modifiers.FinalDamage += 5;
		}
	}

	public override void Update(Player player, ref int buffIndex)
	{ 
		player.GetDamage(DamageClass.Generic) += 1.5f;
	}
}