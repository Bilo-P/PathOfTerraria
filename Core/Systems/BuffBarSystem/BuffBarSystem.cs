﻿using Terraria.DataStructures;

public class BuffBarSystem : GlobalBuff
{
	/// <summary>
	///	Overrides the existing buff bar with an offset of 20 to account for the custom hotbar
	/// </summary>
	public override bool PreDraw(SpriteBatch spriteBatch, int type, int buffIndex, ref BuffDrawParams drawParams)
	{
		int buffPositionOffsetY = 20;
		drawParams.Position = new Vector2(drawParams.Position.X, drawParams.Position.Y + buffPositionOffsetY);
		return true;
	}
}