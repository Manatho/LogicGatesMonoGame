using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.MenuSystem
{
	public class MenuEventArgs : EventArgs
	{
		public string MenuName { get; set; }

		public MenuEventArgs(string menuName)
		{
			MenuName = menuName;
		}
	}
}
