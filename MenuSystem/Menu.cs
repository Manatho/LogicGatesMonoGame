
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameEngine.UI;
using GameEngine.UI.Control;

namespace GameEngine.MenuSystem
{

	public abstract class Menu
	{
		//InputVariables see Menu.Input.cs for more
		protected InputContainer inputContainer;
		protected ContentManager content;

		public string MenuID { get; private set; }


		//------------------------Constructors------------------------
		public Menu(ContentManager content, string menuID)
		{
			this.content = new ContentManager(content.ServiceProvider, content.RootDirectory);
			this.MenuID = menuID;
			this.inputContainer = new InputContainer();
		}

		public virtual void LoadContent() { } //Not abstract, is optional
		public virtual void Initialize() 
		{ 
			inputContainer = new InputContainer(); 
		}

		public virtual void UnloadContent() 
		{
			inputContainer = null;
			content.Unload();
		}

		//------------------------Update------------------------
		public virtual void Update()
		{
			inputContainer.Update();
		}

		//------------------------Draw--------------------------
		public virtual void Draw(SpriteBatch spriteBatch)
		{
			inputContainer.Draw(spriteBatch);
		}

		public event EventHandler<MenuEventArgs> ChangedMenu;
		protected void OnChangedMenu(string changeToMenu)
		{
			if (ChangedMenu != null)
				ChangedMenu(this, new MenuEventArgs(changeToMenu));
		}

	}
}
