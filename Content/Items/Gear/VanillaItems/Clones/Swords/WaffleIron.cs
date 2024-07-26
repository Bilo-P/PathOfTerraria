﻿using Terraria.ID;

namespace PathOfTerraria.Content.Items.Gear.VanillaItems.Clones.Swords;

internal class WaffleIron : VanillaClone
{
	protected override short VanillaItemId => ItemID.WaffleIron;

	public override void SetDefaults()
	{
		ItemType = Core.ItemType.Melee;
	}

	public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (Main.rand.NextBool(3))
		{
			target.AddBuff(BuffID.OnFire3, 5 * 60);
		}
	}
}
