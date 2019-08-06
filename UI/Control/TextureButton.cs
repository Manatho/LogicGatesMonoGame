using GameEngine.UI.Control.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GameEngine.UI
{
	public class TextureButton : Button
	{
		private Vector2 TextureScale;
		private Texture2D lastTexture;


		//Textless button
		public TextureButton(Rectangle buttonBounds, ButtonTexture buttonTexture)
			: base(buttonBounds)
		{
			this.buttonTexture = buttonTexture;
		}


		//Textless button
		public TextureButton(Rectangle buttonBounds)
			: base(buttonBounds)
		{

		}
		

		protected override bool InBounds(MouseState mouseState)
		{

			if (base.InBounds(mouseState))
			{
				if (TextureScale == Vector2.Zero || lastTexture != buttonTexture.CurrentTexture)
				{
					TextureScale = new Vector2(ButtonBounds.Width / buttonTexture.CurrentTexture.Width, ButtonBounds.Height / buttonTexture.CurrentTexture.Height);
					lastTexture = buttonTexture.CurrentTexture;
				}

				// Get Mouse position relative to top left of Texture
				Vector2 pixelPosition = mouseState.Position.ToVector2() - ButtonBounds.Location.ToVector2();

				uint[] PixelData = new uint[1];
				
				buttonTexture.CurrentTexture.GetData<uint>(0, new Rectangle((int)(pixelPosition.X / TextureScale.X), (int)(pixelPosition.Y / TextureScale.Y), (1), (1)), PixelData, 0, 1);

				// Check if pixel in Array is non Alpha, give or take 20
				if (((PixelData[0] & 0xFF000000) >> 24) > 20)
				{
					return true;
				}
			}
			return false;

		}
	}
}
