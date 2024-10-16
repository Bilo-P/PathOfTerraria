﻿using System.Collections.Generic;
using PathOfTerraria.Common.Enums;
using PathOfTerraria.Content.Items.Gear.Weapons.Bow;
using PathOfTerraria.Content.Items.Gear.Weapons.Sword;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace PathOfTerraria.Common.Systems.ModPlayers;

public class ClassModPlayer : ModPlayer
{
	public PlayerClass SelectedClass;
	
	/// <summary>
	/// Overrides the vanilla starting inventory to clear it
	/// </summary>
	/// <param name="itemsByMod"></param>
	/// <param name="mediumCoreDeath"></param>
	public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
	{
		if (!itemsByMod.TryGetValue("Terraria", out List<Item> vanillaItems))
		{
			return;
		}

		vanillaItems.Clear();

		// We want the first slot to be empty so the chosen class' weapon can
		// be placed there.
		var item = new Item();
		item.TurnToAir();
		vanillaItems.Add(item);

		item = new Item(ModContent.ItemType<MagicWoodenSword>());
		vanillaItems.Add(item);
		
		item = new Item(ItemID.CopperPickaxe);
		vanillaItems.Add(item);

		item = new Item(ModContent.ItemType<MagicWoodenBow>());
		vanillaItems.Add(item);

		item = new Item(ItemID.WoodenArrow, 500);
		vanillaItems.Add(item);
	}

	public override void Initialize()
	{
		SelectedClass = PlayerClass.None;
	}

	public override void SaveData(TagCompound tag)
	{
		tag["selectedClass"] = (int) SelectedClass;
	}

	public override void LoadData(TagCompound tag)
	{
		int selectedClassInt = tag.GetInt("selectedClass");
		SelectedClass = (PlayerClass) selectedClassInt;
	} 

	public bool HasSelectedClass()
	{
		return SelectedClass != PlayerClass.None;
	}
}