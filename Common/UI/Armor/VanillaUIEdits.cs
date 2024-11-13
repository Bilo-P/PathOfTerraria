﻿using Mono.Cecil.Cil;
using MonoMod.Cil;
using PathOfTerraria.Core.UI;
using System.Linq;
using System.Reflection;

namespace PathOfTerraria.Common.UI.Armor;

internal class VanillaUIEdits : ModSystem
{
	public override void Load()
	{
		IL_Main.DrawPVPIcons += ModifyPvpIconLocations;
	}

	private void ModifyPvpIconLocations(ILContext il)
	{
		ILCursor c = new(il);

		MethodInfo method = typeof(SpriteBatch).GetMethod(nameof(SpriteBatch.Draw), BindingFlags.Public | BindingFlags.Instance,
			[typeof(Texture2D), typeof(Vector2), typeof(Rectangle?), typeof(Color), typeof(float), typeof(Vector2), typeof(Vector2), typeof(SpriteEffects), typeof(float)]);

		if (!c.TryGotoNext(x => x.MatchCallvirt(method)))
		{
			return;
		}

		if (!c.TryGotoPrev(x => x.MatchLdsfld<Main>(nameof(Main.spriteBatch))))
		{
			return;
		}

		c.Emit(OpCodes.Ldloca_S, (byte)1);
		c.Emit(OpCodes.Ldloca_S, (byte)2);
		c.EmitDelegate(ModifyToggleLocation);
	}

	private static void ModifyToggleLocation(ref int x, ref int y)
	{
		int pixels = (int)(UIManager.Data.First(x => x.Identifier == $"{PoTMod.ModName}:Inventory").Value as UIGearInventory).Root.Left.Pixels;
		float off = pixels - Main.screenWidth + UIGearInventory.ArmorPageWidth + UIGearInventory.Margin;
		x += (int)off + 20;
		y += 140;
	}
}