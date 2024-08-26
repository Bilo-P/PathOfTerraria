﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using PathOfTerraria.Common.Mechanics;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;

namespace PathOfTerraria.Common.Systems.ModPlayers;

internal class SkillCombatPlayer : ModPlayer
{
	public static ModKeybind Skill1Keybind;
	public static ModKeybind Skill2Keybind;
	public static ModKeybind Skill3Keybind;

	public readonly Skill[] HotbarSkills = new Skill[3];
	public Skill[] UnlockedSkills = [];
	
	public int Points;

	public override void Load()
	{
		if (Main.dedServ)
		{
			return;
		}

		Skill1Keybind = KeybindLoader.RegisterKeybind(Mod, "UseSkill1", Keys.D3);
		Skill2Keybind = KeybindLoader.RegisterKeybind(Mod, "UseSkill2", Keys.D4);
		Skill3Keybind = KeybindLoader.RegisterKeybind(Mod, "UseSkill3", Keys.D5);

#if DEBUG
		// This code only runs in debug as we don't need to do this in production; keys would already be generated by this point.
		// This code autogenerates the keys for both the name and description of a skill.

		IEnumerable<Type> skills = AssemblyManager.GetLoadableTypes(Mod.Code).Where(x => typeof(Skill).IsAssignableFrom(x) && !x.IsAbstract);

		foreach (Type skillType in skills)
		{
			var skill = Skill.GetAndPrepareSkill(skillType);

			Language.GetOrRegister("Mods.PathOfTerraria.Skills." + skill.Name + ".Name", () => "");
			Language.GetOrRegister("Mods.PathOfTerraria.Skills." + skill.Name + ".Description", () => "");
		}
#endif
	}

	public override void ProcessTriggers(TriggersSet triggersSet)
	{
		if (Skill1Keybind.JustPressed && HotbarSkills[0] != null && HotbarSkills[0].CanUseSkill(Player))
		{
			HotbarSkills[0]?.UseSkill(Player);
		}

		if (Skill2Keybind.JustPressed && HotbarSkills[1] != null && HotbarSkills[1].CanUseSkill(Player))
		{
			HotbarSkills[1]?.UseSkill(Player);
		}

		if (Skill3Keybind.JustPressed && HotbarSkills[2] != null && HotbarSkills[2].CanUseSkill(Player))
		{
			HotbarSkills[2]?.UseSkill(Player);
		}
	}

	public override void ResetEffects()
	{
		if (HotbarSkills == null || HotbarSkills.Length == 0)
		{
			return;
		}

		foreach (Skill skill in HotbarSkills)
		{
			if (skill is { Timer: > 0 })
			{
				skill.Timer--;
			}
		}
	}

	public override void PostUpdate()
	{
		for (int i = 0; i < HotbarSkills.Length; i++)
		{
			Skill skill = HotbarSkills[i];

			if (skill is not null)
			{
				//skill.UpdateEquipped(Player);

				if (!skill.CanEquipSkill(Player))
				{
					HotbarSkills[i] = null;
					continue;
				}
			}
		}
	}

	public override void SaveData(TagCompound tag)
	{
		for (int i = 0; i < HotbarSkills.Length; i++)
		{
			Skill skill = HotbarSkills[i];

			if (skill is null)
			{
				return;
			}

			TagCompound skillTag = new()
			{
				{ "type", skill.GetType().AssemblyQualifiedName }
			};

			skill.SaveData(skillTag);
			tag.Add("skill" + i, skillTag);
		}
		
		tag["points"] = Points;
	}

	public override void LoadData(TagCompound tag)
	{
		for (int i = 0; i < HotbarSkills.Length; ++i)
		{
			if (!tag.ContainsKey("skill" + i))
			{
				return;
			}

			TagCompound data = tag.GetCompound("skill" + i);
			string type = data.GetString("type");

			var skill = Skill.GetAndPrepareSkill(Type.GetType(type));
			skill.LoadData(data);
			skill.LevelTo(skill.Level);

			HotbarSkills[i] = skill;
		}
	}

	public bool TryGetSkill<T>(out Skill skill) where T : Skill
	{
		skill = null;

		for (int i = 0; i < HotbarSkills.Length; i++)
		{
			if (HotbarSkills[i] is not null && HotbarSkills[i].GetType() == typeof(T))
			{
				skill = HotbarSkills[i];
				return true;
			}
		}

		return false;
	}

	public bool HasSkill(string name)
	{
		for (int i = 0; i < HotbarSkills.Length; i++)
		{
			if (HotbarSkills[i] is not null && HotbarSkills[i].Name == name)
			{
				return true;
			}
		}

		return false;
	}

	public bool TryAddSkill(Skill skill)
	{
		if (!skill.CanEquipSkill(Player))
		{
			Main.NewText("Skill cannot be added.");
			return false;
		}

		for (int i = 0; i < HotbarSkills.Length; ++i)
		{
			if (HotbarSkills[i] != null && HotbarSkills[i].Name == skill.Name)
			{
				Main.NewText("Skill already added.");
				return false;
			}
		}

		for (int i = 0; i < HotbarSkills.Length; i++)
		{
			if (HotbarSkills[i] == null)
			{
				HotbarSkills[i] = skill;
				HotbarSkills[i].LevelTo(HotbarSkills[i].Level);
				Main.NewText("Skill added successfully.");
				return true;
			}
		}

		Main.NewText("No available space to add the skill.");
		return false;
	}

	public bool TryRemoveSkill(Skill skill)
	{
		for (int i = 0; i < HotbarSkills.Length; i++)
		{
			if (HotbarSkills[i] != null && HotbarSkills[i].GetType() == skill.GetType())
			{
				HotbarSkills[i] = null;
				Main.NewText("Skill removed successfully.");
				return true;
			}
		}

		Main.NewText("Skill not found in the current skill set.");
		return false;
	}
}