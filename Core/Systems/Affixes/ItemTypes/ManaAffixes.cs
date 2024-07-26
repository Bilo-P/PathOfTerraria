﻿namespace PathOfTerraria.Core.Systems.Affixes.ItemTypes;

internal class ManaAffix : ItemAffix
{
	public override void ApplyAffix(EntityModifier modifier, Item item)
	{
		modifier.MaximumMana.Base += Value;
	}
}

internal class ManaRegenAffix : ItemAffix
{
	public override void ApplyAffix(EntityModifier modifier, Item item)
	{
		modifier.ManaRegen.Base += Value;
	}
}

internal class ManaPotionPowerAffix : ItemAffix
{
	public override void ApplyAffix(EntityModifier modifier, Item item)
	{
		modifier.PotionManaPower.Base += Value;
	}
}

internal class ManaPotionCapAffix : ItemAffix
{
	public override void ApplyAffix(EntityModifier modifier, Item item)
	{
		modifier.MaxManaPotions.Base += Value;
	}
}

internal class ManaPotionCooldownAffix : ItemAffix
{
	public override void ApplyAffix(EntityModifier modifier, Item item)
	{
		modifier.PotionManaDelay.Base -= Value;
	}
}