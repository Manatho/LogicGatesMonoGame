using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.UI.Control.Event
{
	public class ButtonEventArgs : EventArgs
	{
		public Button Button { get; set; }
        public Point MousePos { get; set; }
	}


	public class MessagesEventArgs : EventArgs
	{
		public MessagesBox MessageBox { get; set; }
	}


}
