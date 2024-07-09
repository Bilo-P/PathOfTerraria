﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathOfTerraria.Content.Buffs;
using Terraria.ID;

namespace PathOfTerraria.Core.Systems.ModPlayers;
/// <summary>
/// This is for either buffs or debuffs, as its the same in terraria; but mainly debuffs.
/// </summary>
public class OnHitDeBuffer : EntityModifierSegment, IEnumerable<KeyValuePair<int, Dictionary<int, StatModifier>>>
{
	private readonly Dictionary<int, Dictionary<int, StatModifier>> Buffs = [];
	
	public override Dictionary<string, StatModifier> Modifiers => Buffs
		.SelectMany(outer => outer.Value
			.Select(inner => new KeyValuePair<string, StatModifier>("+" + Lang.GetBuffName(outer.Key) + " (" + MathF.Round(inner.Key / 60f, 2) + " s)", inner.Value)))
		.ToDictionary(v => v.Key, v => v.Value);

	public void Add(int id, int duration, float val)
	{
		if (!Buffs.ContainsKey(id))
		{
			Buffs.Add(id, new Dictionary<int, StatModifier>());
		}

		if (!Buffs[id].ContainsKey(duration))
		{
			Buffs[id].Add(duration, new StatModifier());
		}

		Buffs[id][duration] += val;
	}

	public IEnumerator<KeyValuePair<int, Dictionary<int, StatModifier>>> GetEnumerator()
	{
		foreach (KeyValuePair<int, Dictionary<int, StatModifier>> val in Buffs)
		{
			yield return val;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
