﻿using Terraria.DataStructures;

namespace PathOfTerraria.Common.Systems.RealtimeGen;

/// <summary>
/// Helper methods for realtime generation.
/// </summary>
public static class RealtimeSteps
{
	/// <summary>
	/// Wraps around <see cref="WorldGen.PlaceTile(int, int, int, bool, bool, int, int)"/>. Uses default return value
	/// for PlaceTile to determine what the step returns.
	/// </summary>
	/// <param name="x">X position.</param>
	/// <param name="y">Y position.</param>
	/// <param name="type">Tile type to place.</param>
	/// <returns>A realtime step that places a tile.</returns>
	public static RealtimeStep PlaceTile(int x, int y, int type)
	{
		return new RealtimeStep((i, j) => WorldGen.PlaceTile(i, j, type), new Point16(x, y));
	}

	/// <summary>
	/// Wraps around <see cref="WorldGen.KillTile(int, int, bool, bool, bool)"/>.<br/>
	/// If there is a tile at <paramref name="x"/>, <paramref name="y"/>, the step returns true.
	/// </summary>
	/// <param name="x">X position.</param>
	/// <param name="y">Y position.</param>
	/// <returns>A realtime step that kills a tile.</returns>
	public static RealtimeStep KillTile(int x, int y)
	{
		return new RealtimeStep((i, j) =>
		{
			WorldGen.KillTile(i, j);
			return !Main.tile[i, j].HasTile;
		}, new Point16(x, y));
	}

	/// <summary>
	/// Wraps around <see cref="Tile.SmoothSlope(int, int, bool, bool)"/>.<br/>
	/// Returns true by default, unless <paramref name="quickSkip"/> is true.
	/// </summary>
	/// <param name="x">X position.</param>
	/// <param name="y">Y position.</param>
	/// <param name="quickSkip">If true, this step will return false for quicker placement.</param>
	/// <returns>A realtime step that slopes a tile.</returns>
	public static RealtimeStep SmoothSlope(int x, int y, bool quickSkip = false)
	{
		return new RealtimeStep((i, j) =>
		{
			Tile.SmoothSlope(i, j);
			return !quickSkip;
		}, new Point16(x, y));
	}

	/// <summary>
	/// Wraps around <see cref="WorldGen.PlaceWall(int, int, int, bool)"/>.<br/>
	/// Returns if the tile 
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="wall"></param>
	/// <param name="quickSkip"></param>
	/// <returns></returns>
	public static RealtimeStep PlaceWall(int x, int y, int wall, bool quickSkip = false)
	{
		return new RealtimeStep((i, j) =>
		{
			WorldGen.PlaceWall(i, j, wall);
			return !quickSkip || Main.tile[i, j].WallType == wall;
		}, new Point16(x, y));
	}
}
