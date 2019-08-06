using GameEngine.UI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.MenuSystem
{
	public class MenuManager
	{
		private Dictionary<string, Menu> menus;
		private Menu activeMenu;

		public MenuManager()
		{
            if (!InputController.IsInitialized)
                throw new Exception("InputController not Initialized!");

			menus = new Dictionary<string, Menu>();
		}

		public void AddMenu(Menu menu)
		{
			menu.ChangedMenu += MenuChanged;
			menus.Add(menu.MenuID, menu);
		}

		public void RemoveMenu(Menu menu)
		{
			menu.ChangedMenu -= MenuChanged;
			menus.Remove(menu.MenuID);

			if (activeMenu == menu)
				activeMenu.UnloadContent();

			if (menus.Count == 0)
				activeMenu = null;
		}

		public void UnloadMenus()
		{
			foreach (Menu m in menus.Values)
				m.UnloadContent();

			menus.Clear();
				
		}

		//------------------------------

		public void Update()
		{
			if(activeMenu != null)
				activeMenu.Update();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (activeMenu != null)
				activeMenu.Draw(spriteBatch);
		}


		//-------------------------------

		public void SetMenu(string menuID)
		{
			ChangeMenu(menuID);
		}

		private void MenuChanged(object sender, MenuEventArgs e)
		{
			ChangeMenu(e.MenuName);
		}

		private void ChangeMenu(string MenuID)
		{
			if(activeMenu != null)
				activeMenu.UnloadContent();

			InputController.Reset();
			

			GC.Collect();

			activeMenu = menus[MenuID];
			activeMenu.LoadContent();
			activeMenu.Initialize();
		}


	}
}
