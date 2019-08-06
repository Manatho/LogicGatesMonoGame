using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.UI.Control.Texture
{
	class ComboBoxTexture
	{
		public ScrollerTexture scrollTexture { get; private set; }

		public Texture2D selectionTexture;
		public Color selectionColor;

		public Texture2D selectedTexture;
		public Color selectedColor;

		public Texture2D itemBackgroundTexture;
		public Color itemBackgroundColor;

		public Texture2D currentItemTexture;
		public Color currentItemColor;


		public ComboBoxTexture(Texture2D selectionTexture, Texture2D selectedTexture, Texture2D itemBackgroundTexture, Texture2D currentItemTexture, ScrollerTexture scrollTexture)
		{
			this.currentItemTexture = currentItemTexture;
			this.scrollTexture = scrollTexture;
			this.selectionTexture = selectionTexture;
			this.selectedTexture = selectedTexture;
			this.itemBackgroundTexture = itemBackgroundTexture;

			selectedColor = Color.White;
			selectionColor = Color.White;
			itemBackgroundColor = Color.White;
		}

		public ComboBoxTexture(Texture2D commonTexture, Color selectionColor, Color selectedColor, Color itemBackgroundColor, Color currentItemColor, ScrollerTexture scrollTexture)
		{
			this.currentItemTexture = commonTexture;
			this.scrollTexture = scrollTexture;
			this.selectionTexture = commonTexture;
			this.selectedTexture = commonTexture;
			this.itemBackgroundTexture = commonTexture;

			this.selectedColor = selectedColor;
			this.selectionColor = selectionColor;
			this.itemBackgroundColor = itemBackgroundColor;
			this.currentItemColor = currentItemColor;
		}




	}
}
