using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.UI.Control.Texture
{
	public struct ScrollerTexture
	{
		public ButtonTexture upButton;
		public ButtonTexture downButton;
		public ButtonTexture scrollButton;
		public Texture2D backgroundTexture;
		public Color backgroundColor;


		public ScrollerTexture(ButtonTexture Buttons, ButtonTexture ScrollButton, Texture2D BackgroundTexture)
		{
			upButton = Buttons;
			downButton = Buttons;
			scrollButton = ScrollButton;
			backgroundTexture = BackgroundTexture;
			backgroundColor = Color.White;
		}
	}
}
