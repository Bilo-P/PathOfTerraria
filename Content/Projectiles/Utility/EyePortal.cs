﻿using PathOfTerraria.Common.Subworlds.BossDomains;
using PathOfTerraria.Common.UI;
using SubworldLibrary;
using Terraria.GameContent;
using Terraria.ID;

namespace PathOfTerraria.Content.Projectiles.Utility;

internal class EyePortal : ModProjectile
{
	private ref float Timer => ref Projectile.ai[0];

	public override void SetDefaults()
	{
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.timeLeft = 2;
		Projectile.tileCollide = false;
		Projectile.Size = new Vector2(80, 80);
		Projectile.Opacity = 0.5f;
		Projectile.netImportant = true;
	}

	public override bool? CanDamage()
	{
		return false;
	}

	public override void AI()
	{
		Projectile.timeLeft++;
		Projectile.rotation += 0.15f;
		Projectile.Opacity = MathHelper.Lerp(Projectile.Opacity, 1f, 0.05f);
		Projectile.velocity *= 0.94f;

		if (Main.rand.NextBool(14))
		{
			Dust.NewDust(Projectile.position + new Vector2(8), Projectile.width - 16, Projectile.height - 16, DustID.Firework_Red);
		}

		Lighting.AddLight(Projectile.Center, TorchID.Red);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = TextureAssets.Projectile[Type].Value;

		for (int i = 0; i < 3; ++i)
		{
			float rotation = Projectile.rotation * (i % 2 == 0 ? -1 : 1);
			Vector2 position = Projectile.Center - Main.screenPosition;
			Color color = lightColor * ((3 - i) * 0.2f) * Projectile.Opacity;
			Main.spriteBatch.Draw(tex, position, null, color, rotation, tex.Size() / 2f, 1f - i * 0.2f, SpriteEffects.None, 0);
		}

		return false;
	}

	public class ClickEyePortalPlayer : ModPlayer
	{
		public override void PostUpdate()
		{
			if (Main.myPlayer == Player.whoAmI)
			{
				foreach (Projectile projectile in Main.ActiveProjectiles)
				{
					if (projectile.ModProjectile is EyePortal teleportal && projectile.Hitbox.Contains(Main.MouseWorld.ToPoint()))
					{
						if (Main.mouseRight && Main.mouseRightRelease)
						{
							SubworldSystem.Enter<EyeDomain>();
						}

						Tooltip.SetName("Enter");
					}
				}
			}
		}
	}
}
