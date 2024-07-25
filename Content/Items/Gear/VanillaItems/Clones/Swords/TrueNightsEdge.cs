﻿using Terraria.DataStructures;
using Terraria.ID;

namespace PathOfTerraria.Content.Items.Gear.VanillaItems.Clones.Swords;

internal class TrueNightsEdge : VanillaClone
{
	protected override short VanillaItemId => ItemID.TrueNightsEdge;

	public override void Defaults()
	{
		ItemType = Core.ItemType.Melee;
		base.Defaults();
	}

	public override bool Shoot(Player plr, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		float scale = plr.GetAdjustedItemScale(Item);
		int dir = plr.direction;
		Projectile.NewProjectile(source, position, new Vector2(dir, 0f), 972, damage, knockback, plr.whoAmI, dir * plr.gravDir, plr.itemAnimationMax, scale);
		Projectile.NewProjectile(source, position, velocity, type, damage / 2, knockback, Main.myPlayer, dir * plr.gravDir, 32f, scale);
		NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, plr.whoAmI);
		return true;
	}

	public override void MeleeEffects(Player player, Rectangle box)
	{
		if (Main.rand.NextBool(5))
		{
			Dust.NewDust(new Vector2(box.X, box.Y), box.Width, box.Height, DustID.Demonite, player.direction * 2, 0f, 150, default, 1.4f);
		}

		int dust = Dust.NewDust(new Vector2(box.X, box.Y), box.Width, box.Height, DustID.Shadowflame, player.velocity.X * 0.2f + player.direction * 3, 
			player.velocity.Y * 0.2f, 100, default, 1.2f);

		Main.dust[dust].noGravity = true;
		Main.dust[dust].velocity.X /= 2f;
		Main.dust[dust].velocity.Y /= 2f;
	}
}