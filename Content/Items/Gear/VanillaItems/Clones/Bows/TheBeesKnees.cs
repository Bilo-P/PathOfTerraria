﻿using PathOfTerraria.Core.Items;
using Terraria.ID;

namespace PathOfTerraria.Content.Items.Gear.VanillaItems.Clones.Bows;

internal class TheBeesKnees : VanillaClone
{
	protected override short VanillaItemId => ItemID.BeesKnees;

	public override void SetDefaults()
	{
		PoTInstanceItemData data = this.GetInstanceData();
		data.ItemType = Core.ItemType.Ranged;
	}

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		if (type == ProjectileID.WoodenArrowFriendly)
		{
			type = ProjectileID.BeeArrow;
		}
	}
}
