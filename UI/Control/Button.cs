using GameEngine.UI.Control.Event;
using GameEngine.UI.Control.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GameEngine.UI
{

	public class Button : Iinput
	{

		//Appererance
		public Rectangle ButtonBounds { get; set; }
		public string ID;

		//Text
		public TextBox TextBox;
		public string Text{ get { return TextBox.Text;} set{TextBox.Text = value;}}
		public Color TextColor { get { return TextBox.Color; } set { TextBox.Color = value; } }

		//Button- and inputstates
	
		public bool Locked { get; private set; }
		public bool Hide = false;
		public bool EnableRightClick { get; private set; }
		public bool MouseInBounds { get; private set; }

		//Texture
		public ButtonTexture buttonTexture;
		public static ButtonTexture StandardTexture;

		//------------------------------------------------------------
		//------------------------Constructors------------------------
		//------------------------------------------------------------

		public static Button EmptyButton { get { return new Button(Rectangle.Empty); } }

		//Textless button
		public Button(Rectangle buttonBounds)
		{
			buttonTexture = StandardTexture;
			ButtonBounds = buttonBounds;
			TextBox = new TextBox(new Text("", Color.Black, InputController.DefaultFont), buttonBounds);
		}

		//Button with text
		public Button(Rectangle buttonBounds, ButtonTexture buttonTexture, Text text, bool center = false)
		{
			if (center)
				ButtonBounds = new Rectangle(buttonBounds.X - (buttonBounds.Width / 2), buttonBounds.Y - (buttonBounds.Height / 2), buttonBounds.Width, buttonBounds.Height);
			else
				ButtonBounds = new Rectangle(buttonBounds.X, buttonBounds.Y, buttonBounds.Width, buttonBounds.Height);

			TextBox = new TextBox(text, ButtonBounds);
			TextBox.LineAlignment = Alignment.Center;
			TextBox.Alignment = Alignment.Center;
			TextBox.IgnoreHeight = true;
			TextBox.Padding = new Rectangle(0, 0, 0, 0);

			this.buttonTexture = buttonTexture;



		}

		public Button(Rectangle buttonBounds, string Text, SpriteFont SpriteFont, ButtonTexture buttonTexture, bool center = false)
			: this(buttonBounds, buttonTexture, new Text(Text, Color.Black, SpriteFont), center) {}

		public Button(Rectangle buttonBounds, string Text, SpriteFont SpriteFont, bool center = false)
			: this(buttonBounds, StandardTexture, new Text(Text, Color.Black, SpriteFont), center){}

		public Button(Rectangle buttonBounds, Text text, bool center = false)
			: this(buttonBounds, StandardTexture, text, center){}




		public void Move(Point amountToMove)
		{
			ButtonBounds = new Rectangle(ButtonBounds.X + amountToMove.X, ButtonBounds.Y + amountToMove.Y, ButtonBounds.Width, ButtonBounds.Height);
			TextBox.Move(amountToMove);
		}


		//------------------------------------------------------
		//------------------------Update------------------------
		//------------------------------------------------------

		


		public virtual void Update()
		{
		
		}

		//-------------------------------------------------------
		//------------------------Drawing------------------------
		//-------------------------------------------------------

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!Hide)
			{
				spriteBatch.Draw(buttonTexture.CurrentTexture, ButtonBounds, buttonTexture.CurrentColor);
				TextBox.Draw(spriteBatch);
			}
		}

		//-------------------------------------------------------
		//------------------------Methods------------------------
		//-------------------------------------------------------

		protected virtual bool InBounds(MouseState mouseState)
		{
			return ButtonBounds.Contains(mouseState.Position);
		}


		public void Disable(bool EnableRightClick)
		{
			Locked = true;
			buttonTexture.Disabled();
			this.EnableRightClick = EnableRightClick;
		}
		public void Disable()
		{
			Disable(false);

		}

		public void Enable(bool EnableRightClick)
		{
			Locked = false;
			this.EnableRightClick = EnableRightClick;
			CheckState();
            OnEnabled();


        }
		public void Enable()
		{
			Enable(true);
		}

		//------------------------------------------------------
		//------------------------Events------------------------
		//------------------------------------------------------


		private bool _downOnButton = false;
		public event EventHandler<ButtonEventArgs> LeftClicked;
		public event EventHandler<ButtonEventArgs> LeftClickedUp;
		private void OnLeftClickedUp(object s, MouseEventArgs e)
		{
			if (!Locked && !Hide && InBounds(e.MouseState))
			{
				if (LeftClickedUp != null)
				{
					LeftClickedUp(this, new ButtonEventArgs() { Button = this });
				}

				if(_downOnButton)
				{
					if (LeftClicked != null)
						LeftClicked(this, new ButtonEventArgs() { Button = this });
				}
			}
			_downOnButton = false;
			if (!Locked && !Hide && !_downOnButton)
				CheckState();
		}

		public event EventHandler<ButtonEventArgs> LeftClickedDown;
		private void OnLeftClickedDown(object s, MouseEventArgs e)
		{
			if (!Locked && !Hide && InBounds(e.MouseState))
			{
				if (LeftClickedDown != null)
					LeftClickedDown(this, new ButtonEventArgs() { Button = this });
				
				_downOnButton = true;
			}
			else
				_downOnButton = false;
		}

		public event EventHandler<ButtonEventArgs> LeftClickedPressed;
		private void OnLeftClickedPressed(object s, MouseEventArgs e)
		{
			if (!Locked && !Hide && InBounds(e.MouseState))
			{
				if (LeftClickedPressed != null)
					LeftClickedPressed(this, new ButtonEventArgs() { Button = this, MousePos = e.MouseState.Position });

				buttonTexture.Pressed();
			}
				
			
		}

		//----------------------------------------

		public event EventHandler<ButtonEventArgs> RightClickDown;
		private void OnRightClickDown(object sender, MouseEventArgs e)
		{
			if ((!Locked || EnableRightClick) && !Hide && InBounds(e.MouseState))
				if (RightClickDown != null)
					RightClickDown(this, new ButtonEventArgs() { Button = this });
		}

		public event EventHandler<ButtonEventArgs> RightClickPressed;
		private void OnRightClickPressed(object sender, MouseEventArgs e)
		{
			if ((!Locked || EnableRightClick) && !Hide && InBounds(e.MouseState))
				if (RightClickPressed != null)
					RightClickPressed(this, new ButtonEventArgs() { Button = this });
		}

		public event EventHandler<ButtonEventArgs> RightClickUp;
		private void OnRightClickUp(object sender, MouseEventArgs e)
		{
			if (RightClickUp != null)
				if ((!Locked || EnableRightClick) && !Hide && InBounds(e.MouseState))
					RightClickUp(this, new ButtonEventArgs() { Button = this });
		}

		//--------------------------

		public event EventHandler<ButtonEventArgs> LeftBounds;
		private void OnLeftBounds()
		{
			if (LeftBounds != null)
			{
				LeftBounds(this, new ButtonEventArgs() { Button = this });
			}
		}

        //--------------------------

        public event EventHandler<ButtonEventArgs> EnteredBounds;
        private void OnEnteredBounds()
        {
            if (EnteredBounds != null)
            {
                EnteredBounds(this, new ButtonEventArgs() { Button = this });
            }
        }

        //--------------------------

        public event EventHandler<ButtonEventArgs> Enabled;
        private void OnEnabled()
        {
            if (Enabled != null)
            {
                Enabled(this, new ButtonEventArgs() { Button = this });
            }
        }

        private void MouseMoved(object s, MouseEventArgs e)
		{

			if (!Locked && !Hide && !_downOnButton)
				CheckState();

            if (InBounds(e.MouseState))
            {
                if (!MouseInBounds)
                    OnEnteredBounds();
                MouseInBounds = true;
            }
            else
            {
                if (MouseInBounds)
                    OnLeftBounds();
                MouseInBounds = false;
            }

                


		}


		private void CheckState()
		{
			if (InBounds(InputController.MouseManager.MouseState))
			{
				buttonTexture.Hover();
			}
			else
				buttonTexture.Idle();
		}



		private bool _focused = false;
		public virtual void Focus()
		{
			if(!_focused)
			{
				InputController.MouseManager.MouseMoved += MouseMoved;

				InputController.MouseManager.LeftButtonPressed += OnLeftClickedPressed;
				InputController.MouseManager.LeftButtonDown += OnLeftClickedDown;
				InputController.MouseManager.LeftButtonUp += OnLeftClickedUp;

				InputController.MouseManager.RightButtonDown += OnRightClickDown;
				InputController.MouseManager.RightButtonUp += OnRightClickUp;
				InputController.MouseManager.RightButtonPressed += OnRightClickPressed;
				_focused = true;
			}


			if (!Locked && !Hide)
				CheckState();
			
		}

		public virtual void DeFocus()
		{
			if(_focused)
			{
				InputController.MouseManager.MouseMoved -= MouseMoved;

				InputController.MouseManager.LeftButtonPressed -= OnLeftClickedPressed;
				InputController.MouseManager.LeftButtonDown -= OnLeftClickedDown;
				InputController.MouseManager.LeftButtonUp -= OnLeftClickedUp;

				InputController.MouseManager.RightButtonDown -= OnRightClickDown;
				InputController.MouseManager.RightButtonUp -= OnRightClickUp;
				InputController.MouseManager.RightButtonPressed -= OnRightClickPressed;
				_focused = false;
			}
			
		}

	}


}

