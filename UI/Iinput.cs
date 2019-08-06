using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.UI
{
	//Menu items such as buttons and the like
	public interface Iinput
	{
		void Move(Point amountToMove);
		void Update();
		void Draw(SpriteBatch spriteBatch);

		void Focus();
		void DeFocus();
	}
}
