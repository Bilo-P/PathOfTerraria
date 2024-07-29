﻿using Terraria.ID;

namespace PathOfTerraria.Content.Items.Gear.VanillaItems.Clones.Bows;

internal class PulseBow : VanillaClone
{
	protected override short VanillaItemId => ItemID.PulseBow;

	public override void Defaults()
	{
		ItemType = Core.ItemType.Ranged;
		base.Defaults();
	}

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		type = ProjectileID.PulseBolt;
	}
}
