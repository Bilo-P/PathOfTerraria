﻿﻿using PathOfTerraria.Core.Systems;

 namespace PathOfTerraria.Content.Items.Gear.Affixes.ArmorAffixes;

 internal class ManaGearAffix : GearAffix
 {
	 public ManaGearAffix()
	 {
		 PossibleTypes = GearType.Armor;
	 }
		
	 public override float GetModifierValue(Gear gear)
	 {
		 return 5 + (int)(Value * 20) + gear.ItemLevel / 20;
	 }

	 public override string GetTooltip(Player player, Gear gear)
	 {
		 return $"+{GetModifierValue(gear)} Maximum Mana";
	 }

	 public override void BuffPassive(Player player, Gear gear)
	 {
		 player.statManaMax2 += 5 + (int)(Value * 20) + gear.ItemLevel / 20;
	 }
 }

 internal class ManaRegenGearAffix : GearAffix
 {
	 public ManaRegenGearAffix()
	 {
		 PossibleTypes = GearType.Armor;
	 }
		
	 public override float GetModifierValue(Gear gear)
	 {
		 return 1 + (int)(Value * 4) + gear.ItemLevel / 40;
	 }

	 public override string GetTooltip(Player player, Gear gear)
	 {
		 return $"+{GetModifierValue(gear)} Mana Regeneration";
	 }

	 public override void BuffPassive(Player player, Gear gear)
	 {
		 player.manaRegen += 1 + (int)(Value * 4) + gear.ItemLevel / 40;
	 }
 }

 internal class ManaPotionPowerGearAffix : GearAffix
 {
	 public ManaPotionPowerGearAffix()
	 {
		 PossibleTypes = GearType.Armor;
	 }
		
	 public override float GetModifierValue(Gear gear)
	 {
		 return 10 + (int)(Value * 10) + gear.ItemLevel / 20;
	 }

	 public override string GetTooltip(Player player, Gear gear)
	 {
		 return $"Mana potions restore {GetModifierValue(gear)} more Mana";
	 }

	 public override void BuffPassive(Player player, Gear gear)
	 {
		 player.GetModPlayer<PotionSystem>().ManaPower += 10 + (int)(Value * 10) + gear.ItemLevel / 20;
	 }
 }

 internal class ManaPotionCapGearAffix : GearAffix
 {
	 public ManaPotionCapGearAffix()
	 {
		 PossibleTypes = GearType.Armor;
	 }
		
	 public override float GetModifierValue(Gear gear)
	 {
		 return 1 + (int)Math.Round(Value, MidpointRounding.ToEven) + gear.ItemLevel / 100;
	 }

	 public override string GetTooltip(Player player, Gear gear)
	 {
		 return $"You can hold {GetModifierValue(gear)} additional mana potions";
	 }

	 public override void BuffPassive(Player player, Gear gear)
	 {
		 player.GetModPlayer<PotionSystem>().MaxMana += 1 + (int)Math.Round(Value, MidpointRounding.ToEven) + gear.ItemLevel / 100;
	 }
 }

 internal class ManaPotionCooldownGearAffix : GearAffix
 {
	 public ManaPotionCooldownGearAffix()
	 {
		 PossibleTypes = GearType.Armor;
		 RequiredInfluence = GearInfluence.Solar;
	 }
		
	 public override float GetModifierValue(Gear gear)
	 {
		 return 0.5f + Value * 0.5f;
	 }

	 public override string GetTooltip(Player player, Gear gear)
	 {
		 return $"Mana potions are ready {GetModifierValue(gear)} seconds sooner";
	 }

	 public override void BuffPassive(Player player, Gear gear)
	 {
		 player.GetModPlayer<PotionSystem>().ManaDelay -= (int)(60 * (0.5f + Value * 0.5f));
	 }
 }