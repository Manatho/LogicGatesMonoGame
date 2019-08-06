using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.UI
{
	public class MouseManager
	{
		public Point Position { get { return _mouseState.Position; } }
		public MouseState MouseState { get { return _mouseState; } }

		private MouseState _mouseState;
		private ButtonState _leftButton;
		private ButtonState _rightButton;

		private Point _moved;
		private int _scroll;

		public void Update()
		{
			_mouseState = Mouse.GetState();

			//ScrollWheel
			if (_mouseState.ScrollWheelValue < _scroll)
				OnScrollDown();
			else if (_mouseState.ScrollWheelValue > _scroll)
				OnScrolledUp();

			//Left
			if (_mouseState.LeftButton == ButtonState.Pressed)
			{
				if (_leftButton == ButtonState.Released)
					OnLeftButtonDown();

				OnLeftButtonPressed();
			}
			else if (_leftButton == ButtonState.Pressed)
				OnLeftButtonUp();


			//Right
			if (_mouseState.RightButton == ButtonState.Pressed)
			{
				if (_rightButton == ButtonState.Released)
					OnRightButtonDown();

				OnRightButtonPressed();
			}
			else if (_rightButton == ButtonState.Pressed)
				OnRightButtonUp();

			//Move
			if (_moved != _mouseState.Position)
			{
				_moved = _mouseState.Position;
				OnMouseMoved();
			}

			_rightButton = _mouseState.RightButton;
			_leftButton = _mouseState.LeftButton;
			_scroll = _mouseState.ScrollWheelValue;

		}

		//Scroll
		public event EventHandler<MouseEventArgs> Scrolled;
		public event EventHandler<MouseEventArgs> ScrolledDown;
		private void OnScrollDown()
		{
			if (ScrolledDown != null)
				ScrolledDown(this, new MouseEventArgs(_mouseState));

			if (Scrolled != null)
				Scrolled(this, new MouseEventArgs(_mouseState));
		}

		public event EventHandler<MouseEventArgs> ScrolledUp;
		private void OnScrolledUp()
		{
			if (ScrolledUp != null)
				ScrolledUp(this, new MouseEventArgs(_mouseState));

			if (Scrolled != null)
				Scrolled(this, new MouseEventArgs(_mouseState));
		}

		//Moved
		public event EventHandler<MouseEventArgs> MouseMoved;
		private void OnMouseMoved()
		{
			if (MouseMoved != null)
				MouseMoved(this, new MouseEventArgs(_mouseState));
		}

		//Left
		public event EventHandler<MouseEventArgs> LeftButtonDown;
		private void OnLeftButtonDown()
		{
			//Console.WriteLine("Test");

			if (LeftButtonDown != null)
				LeftButtonDown(this, new MouseEventArgs(_mouseState));
		}

		public event EventHandler<MouseEventArgs> LeftButtonPressed;
		private void OnLeftButtonPressed()
		{
			if (LeftButtonPressed != null)
				LeftButtonPressed(this, new MouseEventArgs(_mouseState));
		}

		public event EventHandler<MouseEventArgs> LeftButtonUp;
		private void OnLeftButtonUp()
		{
			if (LeftButtonUp != null)
				LeftButtonUp(this, new MouseEventArgs(_mouseState));
		}

		//Right
		public event EventHandler<MouseEventArgs> RightButtonDown;
		private void OnRightButtonDown()
		{
			if (RightButtonDown != null)
				RightButtonDown(this, new MouseEventArgs(_mouseState));
		}

		public event EventHandler<MouseEventArgs> RightButtonPressed;
		private void OnRightButtonPressed()
		{
			if (RightButtonPressed != null)
				RightButtonPressed(this, new MouseEventArgs(_mouseState));
		}

		public event EventHandler<MouseEventArgs> RightButtonUp;
		private void OnRightButtonUp()
		{
			if (RightButtonUp != null)
				RightButtonUp(this, new MouseEventArgs(_mouseState));
		}

		public void UnsubscribeAll()
		{
			Scrolled = null;
			ScrolledDown  = null;
			ScrolledUp = null;
			MouseMoved = null;
			LeftButtonDown = null;
			LeftButtonPressed = null;
			LeftButtonUp = null;
			RightButtonDown = null;
			RightButtonPressed = null;
			RightButtonUp = null;
		}
	}

	public class MouseEventArgs : EventArgs
	{
		public MouseState MouseState;

		public MouseEventArgs(MouseState mouseState)
		{
			MouseState = mouseState;
		}


	}
}
