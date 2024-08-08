﻿using System.IO;
using PathOfTerraria.Common.Systems.Networking.Handlers;
using PathOfTerraria.Common.Systems.Networking.Handlers.MapDevice;
using Terraria.ID;

namespace PathOfTerraria.Common.Systems.Networking;

internal static class Networking
{
	public enum Message : byte
	{
		/// <summary>
		/// Spawns experience. Signature:<br/>
		/// <c>byte target, int xpValue, Vector2 position, Vector2 velocity, bool spawnLocally = false</c>
		/// </summary>
		SpawnExperience,

		/// <summary>
		/// Syncs the usage of hotbar potions. Signature:<br/>
		/// <c>byte playerWhoAmI, bool isHealingPotion, byte newValue, bool runLocally = false</c>
		/// </summary>
		SetHotbarPotionUse,

		/// <summary>
		/// Syncs the rings of the Guardian Angel's hits. Signature:<br/>
		/// <c>byte playerWhoAmI, short npcWho, bool runLocally = false</c>
		/// </summary>
		SyncGuardianAngelHit,

		/// <summary>
		/// Syncs placing an item in a map device. Signature:<br/>
		/// <c>byte fromWho, short itemId, Point16 entityKey</c>
		/// </summary>
		SyncMapDevicePlaceMap,

		ConsumeMapOffOfDevice,
	}

	internal static void HandlePacket(BinaryReader reader)
	{
		var message = (Message)reader.ReadByte();

		switch (message)
		{
			case Message.SpawnExperience:
				if (Main.netMode == NetmodeID.Server)
				{
					ExperienceHandler.ServerRecieve(reader);
				}
				else
				{
					ExperienceHandler.ServerRecieve(reader);
				}

				break;

			case Message.SetHotbarPotionUse:
				if (Main.netMode == NetmodeID.Server)
				{
					HotbarPotionHandler.ServerRecieve(reader);
				}
				else
				{
					HotbarPotionHandler.ClientRecieve(reader);
				}

				break;

			case Message.SyncGuardianAngelHit:
				if (Main.netMode == NetmodeID.Server)
				{
					SyncGuardianAngelHandler.ServerRecieve(reader);
				}
				else
				{
					SyncGuardianAngelHandler.ClientRecieve(reader);
				}

				break;

			case Message.SyncMapDevicePlaceMap:
				if (Main.netMode == NetmodeID.Server)
				{
					PlaceMapInDeviceHandler.ServerRecieve(reader);
				}
				else
				{
					PlaceMapInDeviceHandler.ClientRecieve(reader);
				}

				break;

			case Message.ConsumeMapOffOfDevice:
				if (Main.netMode == NetmodeID.Server)
				{
					ConsumeMapDeviceHandler.ServerRecieve(reader);
				}
				else
				{
					ConsumeMapDeviceHandler.ClientRecieve(reader);
				}

				break;

			default:
				throw null;
		}
	}

	internal static ModPacket GetPacket(Message type)
	{
		ModPacket packet = PoTMod.Instance.GetPacket();
		packet.Write((byte)type);
		return packet;
	}
}
