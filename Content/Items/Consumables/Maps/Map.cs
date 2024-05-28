﻿using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace PathOfTerraria.Content.Items.Consumables.Maps;

internal abstract class Map : ModItem
{
	public override string Texture => $"{PathOfTerraria.ModName}/Assets/Items/Consumables/Maps/Map";
	protected int Tier = 0;
	private List<MapAffix> _affixes = [];
	private string _name;

	public override void SetDefaults() {
		Item.width = 32;
		Item.height = 32;
		Item.useStyle = ItemUseStyleID.DrinkLiquid;
		Item.useAnimation = 15;
		Item.useTime = 15;
		Item.useTurn = true;
		Item.UseSound = SoundID.Item3;
		Item.maxStack = 1;
		Item.consumable = true;
		Item.rare = ItemRarityID.Green;
	}
	
	/// <summary>
	/// Allows you to customize what this item's name can be
	/// </summary>
	public virtual string GenerateName()
	{
		return "Unnamed Item";
	}
	
	public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
	{
		if (line.Mod == Mod.Name && line.Name == "Name")
		{
			yOffset = -2;
			line.BaseScale = Vector2.One * 1.1f;
			return true;
		}

		if (line.Mod == Mod.Name && line.Name == "Map")
		{
			yOffset = -8;
			line.BaseScale = Vector2.One * 0.8f;
			return true;
		}

		if (line.Mod == Mod.Name && line.Name == "Tier")
		{
			yOffset = 2;
			line.BaseScale = Vector2.One * 0.8f;
			return true;
		}
		
		return true;
	}
	
	public override void ModifyTooltips(List<TooltipLine> tooltips)
	{
		tooltips.Clear();
		var nameLine = new TooltipLine(Mod, "Name", Name);
		tooltips.Add(nameLine);
		
		var mapLine = new TooltipLine(Mod, "Map", "Map");
		tooltips.Add(mapLine);

		var rareLine = new TooltipLine(Mod, "Tier", "Tier: " + Tier);
		tooltips.Add(rareLine);

		foreach (MapAffix affix in _affixes)
		{
			string text = $"[i:{ItemID.MusketBall}] " + affix.GetTooltip(this);

			var affixLine = new TooltipLine(Mod, $"Affix{affix.GetHashCode()}", text);
			tooltips.Add(affixLine);
		}
	}
	
	public static void SpawnItem(Vector2 pos)
	{
		SpawnMap<LowTierMap>(pos);
	}
	
	public static void SpawnMap<T>(Vector2 pos) where T : Map
	{
		var item = new Item();
		item.SetDefaults(ModContent.ItemType<T>());
		if (item.ModItem is T map)
		{
			map.Tier = 1;
			map._name = map.GenerateName();
		}

		Item.NewItem(null, pos, Vector2.Zero, item);
	}
	
	public override void SaveData(TagCompound tag)
	{
		tag["tier"] = Tier;

		List<TagCompound> affixTags = [];
		foreach (MapAffix affix in _affixes)
		{
			var newTag = new TagCompound();
			affix.Save(newTag);
			affixTags.Add(newTag);
		}

		tag["affixes"] = affixTags;
	}

	public override void LoadData(TagCompound tag)
	{
		Tier = tag.GetInt("tier");

		IList<TagCompound> affixTags = tag.GetList<TagCompound>("affixes");

		foreach (TagCompound newTag in affixTags)
		{
			_affixes.Add(MapAffix.FromTag(newTag));
		}
	}
}