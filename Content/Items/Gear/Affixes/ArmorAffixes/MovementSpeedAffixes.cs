﻿﻿using PathOfTerraria.Core.Systems;

 namespace PathOfTerraria.Content.Items.Gear.Affixes.ArmorAffixes;

 internal class MovementSpeed : Affix
{
	public override GearType PossibleTypes => GearType.Leggings;
	public override ModifierType ModifierType => ModifierType.Added;
	public override bool IsFlat => false;
	public override string Tooltip => "# Movement Speed";
	protected override float internalModifierCalculation(Gear gear)
	{
		return gear.ItemLevel / 10f * (0.6f + 0.4f * Value); // ranges from 60% to 100%
	}

	public override void BuffPassive(Player player, Gear gear)
	{
		player.moveSpeed += GetModifierValue(gear) / 100f;
	}
}