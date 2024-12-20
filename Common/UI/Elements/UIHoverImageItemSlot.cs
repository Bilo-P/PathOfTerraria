using ReLogic.Content;
using Terraria.GameContent;
using Terraria.UI;

namespace PathOfTerraria.Common.UI.Elements;

/// <summary>
///     Provides an item slot wrapper as a <see cref="UIElement" /> that contains hover transform effects.
/// </summary>
/// <remarks>
///     <inheritdoc cref="UIImageItemSlot" />
/// </remarks>
public class UIHoverImageItemSlot : UIImageItemSlot
{
	/// <summary>
	///     The target rotation for this image when it's being hovered by the mouse.
	/// </summary>
	/// <remarks>
	///     Defaults to <c>0f</c>.
	/// </remarks>
	public float ActiveRotation = 0f;

	/// <summary>
	///     The target scale for this image when it's being hovered by the mouse.
	/// </summary>
	/// <remarks>
	///     Defaults to <c>1f</c>.
	/// </remarks>
	public float ActiveScale = 1f;

	/// <summary>
	///     The target rotation for this image when it's not being hovered by the mouse.
	/// </summary>
	/// <remarks>
	///     Defaults to <c>0f</c>.
	/// </remarks>
	public float InactiveRotation = 0f;

	/// <summary>
	///     The target scale for this image when it's not being hovered by the mouse.
	/// </summary>
	/// <remarks>
	///     Defaults to <c>1f</c>.
	/// </remarks>
	public float InactiveScale = 1f;

	/// <summary>
	///     The smoothness used to perform scale/rotation interpolations.
	/// </summary>
	/// <remarks>
	///     Defaults to <c>0.3f</c>. Ranges from <c>0f</c> - <c>1f</c>.
	/// </remarks>
	public float Smoothness = 0.3f;

	public UIHoverImageItemSlot(
		Asset<Texture2D> backgroundTexture,
		Asset<Texture2D> iconTexture,
		ref Item[]? inventory,
		int slot,
		int context = ItemSlot.Context.InventoryItem
	) : base(backgroundTexture, iconTexture, ref inventory, slot, context)
	{
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);

		if (Background == null)
		{
			return;
		}
		
		Background.Rotation = MathHelper.SmoothStep(Background.Rotation,
			IsMouseHovering ? ActiveRotation : InactiveRotation, Smoothness);
		Background.ImageScale = MathHelper.SmoothStep(Background.ImageScale,
			IsMouseHovering ? ActiveScale : InactiveScale, Smoothness);
	}

	protected override void UpdateIcon()
	{
		if (Item == null)
		{
			return;
		}

		if (!Item.IsAir)
		{
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			Rectangle frame = Main.itemAnimations[Item.type] == null
				? texture.Frame()
				: Main.itemAnimations[Item.type].GetFrame(texture);

			ItemSlot.DrawItem_GetColorAndScale(Item, Item.scale, ref Icon.Color, 24f, ref frame, out _,
				out float finalDrawScale);

			Icon.ImageScale = MathHelper.SmoothStep(Icon.ImageScale,
				finalDrawScale * (IsMouseHovering ? ActiveScale : InactiveScale), Smoothness);
		}
		else
		{
			Icon.Rotation = MathHelper.SmoothStep(Icon.Rotation, IsMouseHovering ? ActiveRotation : InactiveRotation,
				Smoothness);
			Icon.ImageScale = MathHelper.SmoothStep(Icon.ImageScale, IsMouseHovering ? ActiveScale : InactiveScale,
				Smoothness);
		}

		Icon.SetImage(Item.IsAir ? IconTexture : TextureAssets.Item[Item.type]);
	}
}