using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.UI.Control.Texture
{
	public struct ButtonTexture
	{
		private int index;


		private Texture2D[] textures;
		private Color[] textureColors;

		public Texture2D CurrentTexture
		{
			get
			{
				if (textures.Length > 1)
					return index == 3 ? textures[2] : textures[index];
				else
					return textures[0];
			}
		}
		public Color CurrentColor { get { return textureColors[index]; } }


		public ButtonTexture(Texture2D idle, Texture2D hover, Texture2D pressed)
		{
			this.textures = new Texture2D[] { idle, hover, pressed };
			this.textureColors = new Color[] { Color.White, Color.White, Color.White, Color.White };
			index = 0;
		}

		public ButtonTexture(Texture2D common, Color idle, Color hover, Color pressed, Color disabled)
		{
			this.textures = new Texture2D[] { common };
			this.textureColors = new Color[] { idle, hover, pressed, disabled };
			this.index = 0;
		}


		public void DisabledColor(Color disableColor)
		{
			Color[] temp = textureColors;
			textureColors = new Color[textureColors.Length];
			Array.Copy(temp, textureColors, temp.Length);

			this.textureColors[3] = disableColor;
		}


		public void Hover()
		{
			index = 1;
		}

		public void Idle()
		{
			index = 0;
		}

		public void Pressed()
		{
			index = 2;
		}

		public void Disabled()
		{
			index = 3;
		}
		

	}
}
