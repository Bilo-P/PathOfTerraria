﻿using PathOfTerraria.Core.Systems;

namespace PathOfTerraria.Core.Items;

public interface ISwapItemModifiersItem
{
	void SwapItemModifiers(Item item, EntityModifier SwapItemModifier);
}
