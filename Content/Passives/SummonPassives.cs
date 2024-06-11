﻿using PathOfTerraria.Core.Systems.TreeSystem;

namespace PathOfTerraria.Content.Passives;

internal class MinionPassive : Passive
{
	public override int Id => 10;
	public override string Name => "Empowered Horde";
	public override string Tooltip => "Increases your minions' damage by 10% per level";
}

internal class SentryPassive : Passive
{
	public override int Id => 11;
	public override string Name => "Steadfast Sentries";
	public override string Tooltip => "Increases your sentries' damage by 10% per level";
}