using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.UI
{
	public class KeyboardManager
	{
		private KeyboardState KeyboardState;

		private HashSet<Keys> KeysDown;

		public KeyboardManager()
		{
			KeysDown = new HashSet<Keys>();
		}


		public void Update()
		{
			KeyboardState = Keyboard.GetState();

			
			Keys[] currentKeys = KeyboardState.GetPressedKeys();

			if(currentKeys.Length > 0)
			{
				foreach (Keys k in currentKeys)
				{
					if (!KeysDown.Contains(k))
					{
						KeysDown.Add(k);
						OnKeyDown(k);
					}
				}

				OnKeyPressed(currentKeys);
			}

			if(KeysDown.Count > 0)
			{
				HashSet<Keys> hashedCurrentKeys = new HashSet<Keys>(currentKeys);
				KeysDown.RemoveWhere(k => !hashedCurrentKeys.Contains(k) && OnKeyUp(k));
			}
			

		}

		public event EventHandler<KeyboardEventArgs> KeyDown;
		private void OnKeyDown(Keys key)
		{
			if (KeyDown != null)
				KeyDown(this, new KeyboardEventArgs(key, KeysDown));
		}

		public event EventHandler<KeyboardEventArgs> KeyPressed;
		private void OnKeyPressed(Keys[] keys)
		{
			if (KeyPressed != null)
				KeyPressed(this, new KeyboardEventArgs(KeysDown));
		}

		public event EventHandler<KeyboardEventArgs> KeyUp;
		private bool OnKeyUp(Keys key)
		{

			if (KeyUp != null)
				KeyUp(this, new KeyboardEventArgs(key, KeysDown));


			return true;
		}

		public void UnsubscribeAll()
		{
			KeyDown = null;
			KeyUp = null;
			KeyPressed = null;
		}


	}

	public class KeyboardEventArgs : EventArgs
	{
		public Keys Key;
		public HashSet<Keys> Keys;

		public KeyboardEventArgs(Keys key, HashSet<Keys> keys)
		{
            
			Key = key;
			Keys = keys;
		}

		public KeyboardEventArgs(HashSet<Keys> keys)
		{
			Keys = keys;
			Key = default(Keys);
		}

		public bool HasKey(Keys key)
		{
			return Keys.Contains(key);
		}
	}


	
}
