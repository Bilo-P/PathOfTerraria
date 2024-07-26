﻿using PathOfTerraria.Core.Items;
using Terraria.GameContent.Drawing;
using Terraria.ID;

namespace PathOfTerraria.Content.Items.Gear.VanillaItems.Clones.Swords;

internal class Keybrand : VanillaClone
{
	protected override short VanillaItemId => ItemID.Keybrand;

	public override void SetDefaults()
	{
		PoTInstanceItemData data = this.GetInstanceData();
		data.ItemType = Core.ItemType.Melee;
	}

	public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
	{
		float lifeFactor = target.life / (float)target.lifeMax;
		float bonusDamage = Utils.GetLerpValue(1f, 0.1f, lifeFactor, clamped: true);
		modifiers.FinalDamage += bonusDamage;
	}

	public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
	{
		Rectangle itemRectangle = SwordCommon.GetItemRectangle(player, Item);
		var point = itemRectangle.Center.ToVector2();
		Vector2 positionInWorld = target.Hitbox.ClosestPointInRect(point);

		ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings
		{
			PositionInWorld = positionInWorld
		}, player.whoAmI);
	}
}