﻿using PathOfTerraria.Content.Items.Gear;

namespace PathOfTerraria.Core.Systems.Affixes.Affixes.GearTypes.WeaponAffixes;

public class ModifyHitAffixes
{
	internal class PiercingGearAffix : GearAffix
	{
		public override GearType PossibleTypes => GearType.Melee;
		public override void ApplyAffix(EntityModifier modifier, Gear gear)
		{
			modifier.ArmorPenetration.Base += Value * 5 + gear.ItemLevel / 50;
		}
	}

	internal class AddedKnockbackGearAffix : GearAffix
	{
		public override GearType PossibleTypes => GearType.Weapon;
		public override void ApplyAffix(EntityModifier modifier, Gear gear)
		{
			modifier.Knockback += (Value * 10 + gear.ItemLevel / 20)/100f;
		}
	}

	internal class IncreasedKnockbackGearAffix : GearAffix
	{
		public override GearType PossibleTypes => GearType.Weapon;
		public override void ApplyAffix(EntityModifier modifier, Gear gear)
		{
			modifier.Knockback *= 1f + (Value * 10 + gear.ItemLevel / 20) / 100f;
		}
	}

	internal class BaseKnockbackGearAffix : GearAffix
	{
		public override GearType PossibleTypes => GearType.Weapon;
		public override void ApplyAffix(EntityModifier modifier, Gear gear)
		{
			modifier.Knockback.Base += Value * gear.ItemLevel / 100;
		}
	}

	internal class FlatKnockbackGearAffix : GearAffix
	{
		public override GearType PossibleTypes => GearType.Weapon;
		public override void ApplyAffix(EntityModifier modifier, Gear gear)
		{
			modifier.Knockback.Flat += Value * gear.ItemLevel / 60;
		}
	}
}