using System.Linq;
using PathOfTerraria.Core.Systems.ModPlayers;
using PathOfTerraria.Helpers;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace PathOfTerraria.Core.Systems.Players;

/*
 * TODO: Implement gear sets:
 *		Gear sets should stop the player from wearing specific types of weapons and
 *		offhand accessories at the same time. For example, the player should not be
 *		able to hold a sword and equip a quiver, or hold a bow and equip a shield,
 *		etc.
 */
public sealed class GearSwapManager : ModPlayer
{
	/// <summary>
	///		The player's gear inventory, reserved for a weapon and an offhand accessory.
	/// </summary>
	public Item?[] Inventory = new[] { new Item(ItemID.None), new Item(ItemID.None) };
	
	public override void UpdateEquips()
	{
		if (!GearSwapKeybind.SwapKeybind.JustPressed)
		{
			return;
		}

		var previousWeapon = Player.inventory[0];
		var previousOffhand = Player.armor[6];

		Player.inventory[0] = Inventory[0];
		Player.armor[6] = Inventory[1];

		Inventory[0] = previousWeapon;
		Inventory[1] = previousOffhand;
		
		SoundEngine.PlaySound(
			SoundID.MenuTick with
			{
				Pitch = -0.25f,
				MaxInstances = 1
			}
		);
	}

	public override void SaveData(TagCompound tag)
	{
		tag["inventory"] = Inventory.Select(ItemIO.Save).ToArray();
	}

	public override void LoadData(TagCompound tag)
	{
		Inventory = tag.GetList<TagCompound>("inventory").Select(ItemIO.Load).ToArray();
	}
}