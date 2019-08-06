using GameEngine.DrawingUtil;
using GameEngine.UI.Control;
using GameEngine.UI.Control.Event;
using GameEngine.UI.Control.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GameEngine.UI
{
	public class ToggleButton : Button, Iinput
	{
		public bool Active = false;


        public ToggleButton(Rectangle ButtonBounds, Text text, bool center = false)
            : base(ButtonBounds, StandardTexture, text, center)
        {
            this.buttonTexture = new ButtonTexture(buttonTexture.CurrentTexture, Color.Gray, Color.Red, Color.LightGray, Color.DimGray);
            this.LeftClicked += OnLeftClicked;
        }




        public ToggleButton(Rectangle ButtonBounds, string Text, SpriteFont SpriteFont, bool center = false)
			: this(ButtonBounds, Button.StandardTexture, new Text(Text, Color.Black, SpriteFont), center)
		{
		}

		public ToggleButton(Rectangle Bounds, ButtonTexture buttonTexture, Text text, bool center = false)
			: base(Bounds, buttonTexture, text, center)
		{
			this.LeftClicked += OnLeftClicked;
		}


		public override void Update()
		{
			if (Active && !Locked)
				buttonTexture.Pressed();
			
		}

		private void OnLeftClicked(object sender, EventArgs e)
		{
			if (Active) Deactivate();
			else Activate();
		}

		public void Activate()
		{
			Active = true;
		}

		public void Deactivate()
		{
			Active = false;
			
			
		}
	}

	//.......................................................
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||
	//'''''''''''''''''''''''''''''''''''''''''''''''''''''''

	public class ToggleButtons : Iinput
	{
		public List<Button> Buttons { get; private set; }
		public Button SelectedButton {get; private set;}
		public int SelectedIndex { get; private set; }

		public bool Disabled { get; private set; }

		//------------------------------------------------------------
		//------------------------Constructors------------------------
		//------------------------------------------------------------

		//Textless button
		public ToggleButtons()
		{
			SelectedIndex = -1;
			Buttons = new List<Button>();
			Disabled = false;
		}

		//------------------------------------------------------
		//------------------------Update------------------------
		//------------------------------------------------------

		public void Move(Point amountToMove)
		{
			foreach (Button b in Buttons)
			{
				b.Move(amountToMove);
			}

		}

		public void Update()
		{

			

		}


		//-------------------------------------------------------
		//------------------------Drawing------------------------
		//-------------------------------------------------------

		public void Draw(SpriteBatch Spritebatch)
		{
			foreach (Button b in Buttons)
			{
				b.Draw(Spritebatch);
			}
		}

		//-------------------------------------------------------
		//------------------------Methods------------------------
		//-------------------------------------------------------
		public void UpdateButtonStrings(string[] Text)
		{
			for (int i = 0; i < Text.Length && i < Buttons.Count; i++)
			{
				Buttons[i].Text = Text[i];
			}
		}
		public void UpdateButtonStrings()
		{
			for (int i = 0; i < Buttons.Count; i++)
			{
				Buttons[i].Text = "";
			}
		}

		private static int i = 0;
		public void Add(Button Button, bool setID = true)
		{
			if(i == 65 || i == 83)
			{

			}

            if(setID)
			    Button.ID = "B" + i;
			i++;
			Button.LeftClicked += LeftClicked;
			Buttons.Add(Button);
		}

		public void LeftClicked(object sender, ButtonEventArgs b)
		{
			OnBeforeSelectedButtonChange();
			Lock(Buttons.IndexOf(b.Button));
			OnAfterSelectedButtonChange();
		}

		public void Lock(int Index)
		{
			if (!Disabled)
				for (int i = 0; i < Buttons.Count; i++)
				{
					if (Buttons[i].Locked && Index != i)
						Buttons[i].Enable();
				}
			

			Buttons[Index].Disable();
			SelectedButton = Buttons[Index];
			SelectedIndex = Index;
			
		}

		
		public void Disable()
		{
			for (int i = 0; i < Buttons.Count; i++)
				Buttons[i].Disable();
			Disabled = true;
		}

		public void Enable()
		{
			Disabled = false;
			UnLock(true);
		}

		public void UnLock(bool lockSelected)
		{
			if(!Disabled)
			{
				for (int i = 0; i < Buttons.Count; i++)
				{
					if (Buttons[i].Locked)
						Buttons[i].Enable();
				}

                if(lockSelected && SelectedButton != null)
                    SelectedButton.Disable();
                else
                {
                    SelectedButton = null;
                    SelectedIndex = -1;
                }


			}
		}

		public void UnLock(int Index)
		{
			Buttons[Index].Enable();
			SelectedButton = null;
			SelectedIndex = -1;
		}

        public void UnlockSelected()
        {
            if(SelectedIndex != -1)
                UnLock(SelectedIndex);
        }

		//-------------------------------------------------------
		//------------------------Event--------------------------
		//-------------------------------------------------------

		public event EventHandler<ButtonEventArgs> AfterSelectedButtonChange;
		private void OnAfterSelectedButtonChange()
		{
			if (AfterSelectedButtonChange != null)
			{
                AfterSelectedButtonChange(this, new ButtonEventArgs() { Button = SelectedButton });
			}
		}

		public event EventHandler<ButtonEventArgs> BeforeSelectedButtonChange;
		private void OnBeforeSelectedButtonChange()
		{
			if (BeforeSelectedButtonChange != null)
			{
				BeforeSelectedButtonChange(this, new ButtonEventArgs() { Button = SelectedButton });
			}
		}

		public virtual void Focus()
		{
			foreach (Button b in Buttons)
				b.Focus();
			
		}

		public virtual void DeFocus()
		{
			foreach (Button b in Buttons)
				b.DeFocus();
			
		}
	}

	//.......................................................
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||
	//'''''''''''''''''''''''''''''''''''''''''''''''''''''''

	public class TextureField : Iinput
	{
		public Rectangle Bounds;


        public Color Color { get { return color; }
            set
            {
                if(InputController.DefaultTexture != null)
                {
                    if (Texture == null)
                    {
                        Texture = InputController.DefaultTexture;
                    }

                    EnableDrawing = true;
                    color = value;
                }
                
            }
        }
        private Color color;


		public Texture2D Texture;

		public bool stroke = false;
		public int Outlining = 1;

		public bool EnableDrawing { get { return _enableDrawing; } set { if (Texture != null) _enableDrawing = value; } }
		private bool _enableDrawing = true;


		public void Move(Point amountToMove)
		{
			Bounds = new Rectangle(Bounds.X + amountToMove.X, Bounds.Y + amountToMove.Y, Bounds.Width, Bounds.Height);
		}

		//------------------------------------------------------------
		//------------------------Constructors------------------------
		//------------------------------------------------------------
		//Button with text
		public TextureField(Rectangle bounds, Texture2D texture, Color color)
		{
			Texture = texture;
			Bounds = bounds;
			Color = color;

			if (texture == null)
				_enableDrawing = false;
		}

        public TextureField(Rectangle bounds, Color color)
            : this(bounds, InputController.DefaultTexture, color)
        {

        }

        public TextureField(Rectangle bounds)
            : this(bounds, null, default(Color))
        {

        }

		//------------------------------------------------------
		//------------------------Update------------------------
		//------------------------------------------------------


		public void Update()
		{

		}

		//-------------------------------------------------------
		//------------------------Drawing------------------------
		//-------------------------------------------------------

		public void Draw(SpriteBatch Spritebatch)
		{
			if (_enableDrawing)
			{
				if (stroke)
				{
                    Spritebatch.StrokeRect(Bounds, Outlining, Color, Texture);
				}
				else
				{
					Spritebatch.Draw(Texture, Bounds, Color);
				}
			}
			
		}

		public virtual void Focus()
		{

		}

		public virtual void DeFocus()
		{

		}
	}

	//.......................................................
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||
	//'''''''''''''''''''''''''''''''''''''''''''''''''''''''


	class ComboBoxes
	{
		private List<ComboBox> _comboxes;

		public ComboBoxes()
		{
			_comboxes = new List<ComboBox>();
		}

		public ComboBoxes(params ComboBox[] comboBoxes)
			: this()
		{
			Add(comboBoxes);
		}

		public void Add(params ComboBox[] comboBoxes)
		{
			foreach (ComboBox c in comboBoxes)
			{
				c.Opened += OnOpen;
				c.Closed += OnClose;

				_comboxes.Add(c);
			}
		}

		public void ResetAll()
		{
			foreach (ComboBox c in _comboxes)
			{
				c.Reset();
			}
		}

		public void ResetAll(ComboBox ignore)
		{
			foreach (ComboBox c in _comboxes)
			{
				if (c != ignore)
					c.Reset();
			}
		}


		private void OnOpen(object sender, EventArgs e)
		{
			foreach (ComboBox c in _comboxes)
			{
				if (c != sender)
				{
					c.Lock = true;
					c.DeFocus();
				}
					
			}
		}

		private void OnClose(object sender, EventArgs e)
		{
			foreach (ComboBox c in _comboxes)
			{
				if (c != sender)
				{
					c.Lock = false;
					c.Focus();
				}
					
			}
		}


	}

	class ComboBox : Iinput
	{
		public string InfoText { get; set; }

		//Items
		public List<string> items { get; protected set; }
		protected List<Rectangle> itemsBounds;

		//Index
		protected int selectedIndex = -1;
		public int SelectedIndex
		{
			get
			{
				return selectedIndex;
			}
			set
			{
				selectedIndex = value;
				OnIndexChange();
			}
		}
		public string SelectedItem { get; private set; }
		protected int tempIndex = -1;

		protected int showAmount;
		protected int offset;

		//Appearence
		public Rectangle Bounds { get; protected set; }
		protected SpriteFont spriteFont;

		//General- and button states
		protected bool collapsed = true;
		protected bool Collapsed { get { return collapsed; } 
			set 
			{ 
				collapsed = value;
			} 
		}

		public bool Lock { get; set; }


		protected bool collapsible = true;
		protected int scrollWheelWidth = 20;

		protected ComboBoxTexture comboBoxTexture;
		public static ComboBoxTexture StandardTexture;

		protected Scroller _scroller;
		private Button _dropDownButton;

		//------------------------------------------------------------
		//------------------------Constructors------------------------
		//------------------------------------------------------------

		public ComboBox(Point Position, int length, SpriteFont SpriteFont, int ShowAmount = -1)
			:this(Position, length, SpriteFont, StandardTexture, ShowAmount)
		{
			
		}



		public ComboBox(Point Position, int length, SpriteFont SpriteFont, ComboBoxTexture comboBoxTexture, int ShowAmount = -1)
		{
			Lock = false;
			InfoText = "";
			this.comboBoxTexture = comboBoxTexture;
			int height = Convert.ToInt32((int)SpriteFont.MeasureString("Tj").Y);
			spriteFont = SpriteFont;
			showAmount = ShowAmount;
			offset = 0;

			Bounds = new Rectangle(Position.X, Position.Y, length - scrollWheelWidth, height);

			if (showAmount != -1)
			{
				_scroller = new Scroller(new Rectangle(Bounds.Right, Bounds.Y, scrollWheelWidth, (ShowAmount + 1) * height), true);
				_scroller.ValueChanged += ScrolleValueChanged;
			}
				


			_dropDownButton = new Button(new Rectangle(Bounds.Right, Bounds.Y, scrollWheelWidth, height), "v", spriteFont, false);
			_dropDownButton.LeftClickedUp += OnDropDown;

			items = new List<string>();
			itemsBounds = new List<Rectangle>();		
				
		}

		
		private void ScrolleValueChanged(object sender, EventArgs e)
		{
			offset = _scroller.Value;
		}

		//------------------------------------------------------
		//------------------------Methods-----------------------
		//------------------------------------------------------

		public void Move(Point amountToMove)
		{
			Bounds = new Rectangle(Bounds.X + amountToMove.X, Bounds.Y + amountToMove.Y, Bounds.Width, Bounds.Height);

			_dropDownButton.Move(amountToMove);

			for (int i = 0; i < itemsBounds.Count; i++)
			{
				itemsBounds[i] = new Rectangle(itemsBounds[i].X + amountToMove.X, itemsBounds[i].Y + amountToMove.Y, itemsBounds[i].Width, itemsBounds[i].Height);
			}

			if(_scroller != null)
				_scroller.Move(amountToMove);

		}

		private void OnDropDown(object sender, EventArgs e)
		{
			Collapse();
		}



		public virtual void Add(string item)
		{	
			items.Add(item);
			itemsBounds.Add(Rectangle());
			if(_scroller != null)
			{
				if (showAmount < items.Count)
				_scroller.Add(1);
			}
			else 
				showAmount = items.Count;
		}

		public void Add(params string[] items)
		{
			foreach (string s in items)
				Add(s);
		}

		private void Collapse()
		{
			if (Collapsed)
			{
				Collapsed = false;
				OnOpened();
			}
			else
			{
				Collapsed = true;
				OnClosed();
			}
		}

		//------------------------------------------------------
		//------------------------Update------------------------
		//------------------------------------------------------

		public void Update()
		{

		}


		private void OnLeftClick(object sender, MouseEventArgs e)
		{
			if (Bounds.Contains(e.MouseState.Position) && collapsible)
				Collapse();

			if (!Collapsed && MouseInsideBounds())
			{
				selectedIndex = tempIndex;
				if (collapsible)
				{
					Collapsed = true;
					OnClosed();
				}

				OnIndexChange();
			}

			if (collapsible && (!MouseInsideBounds() && !Bounds.Contains(e.MouseState.Position)) && !Collapsed)
				Collapse();
			
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			if (!Collapsed && MouseInsideBounds())
				tempIndex = itemsBounds.FindIndex(x => x.Contains(e.MouseState.Position.X, e.MouseState.Position.Y + Bounds.Height * offset));
		}

		private void OnScrollUp(object sender, MouseEventArgs e)
		{
			_scroller.Value--;
		}

		private void OnScrollDown(object sender, MouseEventArgs e)
		{
			_scroller.Value++;
		}

		public void Reset()
		{
			selectedIndex = -1;
			SelectedItem = null;
		}

		public virtual bool MouseInsideBounds()
		{
			int temp = showAmount != -1 ? 0 : showAmount;
			return new Rectangle(Bounds.X, Bounds.Y + Bounds.Height, Bounds.Width, Bounds.Height * (itemsBounds.Count - temp)).Contains(Mouse.GetState().Position);
		}

		public virtual Rectangle Rectangle()
		{
			return new Rectangle(Bounds.X, Bounds.Y+(Bounds.Height*(itemsBounds.Count+1)), Bounds.Width, Bounds.Height);
		}




		public virtual void Focus()
		{

			InputController.MouseManager.ScrolledUp += OnScrollUp;
			InputController.MouseManager.ScrolledDown += OnScrollDown;

			InputController.MouseManager.LeftButtonDown += OnLeftClick;
			InputController.MouseManager.MouseMoved += OnMouseMove;

			_dropDownButton.Focus();
			
			if(_scroller != null)
				_scroller.Focus();
		}

		public virtual void DeFocus()
		{
			InputController.MouseManager.ScrolledUp -= OnScrollUp;
			InputController.MouseManager.ScrolledDown -= OnScrollDown;

			InputController.MouseManager.LeftButtonDown -= OnLeftClick;
			InputController.MouseManager.MouseMoved -= OnMouseMove;

			_dropDownButton.DeFocus();

			if(_scroller != null)
				_scroller.DeFocus();
		}
		

		//-------------------------------------------------------
		//------------------------Drawing------------------------
		//-------------------------------------------------------

		

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(comboBoxTexture.currentItemTexture, Bounds, comboBoxTexture.currentItemColor);
			if (selectedIndex > -1)
				spriteBatch.DrawString(spriteFont, " " + items[selectedIndex], new Vector2(Bounds.X, Bounds.Y), Color.Black);
			else
				spriteBatch.DrawString(spriteFont, InfoText, new Vector2(Bounds.X, Bounds.Y), Color.Black);

			_dropDownButton.Draw(spriteBatch);

			if (!Collapsed)
			{
				if(_scroller != null)
					_scroller.Draw(spriteBatch);

				for (int i = offset; i < showAmount + offset && i < items.Count; i++)
				{
					Rectangle temp = new Rectangle(itemsBounds[i].X, itemsBounds[i].Y - offset * Bounds.Height, itemsBounds[i].Width, itemsBounds[i].Height);


					if (selectedIndex == i)
						spriteBatch.Draw(comboBoxTexture.selectedTexture, temp, comboBoxTexture.selectedColor);
					else if (tempIndex == i)
						spriteBatch.Draw(comboBoxTexture.selectionTexture, temp, comboBoxTexture.selectionColor);
					else
						spriteBatch.Draw(comboBoxTexture.itemBackgroundTexture, temp, comboBoxTexture.itemBackgroundColor);
					
					spriteBatch.DrawString(spriteFont, " " + items[i], new Vector2(itemsBounds[i].X, itemsBounds[i].Y - offset * Bounds.Height), Color.Black);

				}
			}
		}

		//------------------------------------------------------
		//------------------------EVENTS------------------------
		//------------------------------------------------------

		public event EventHandler IndexChange;
		private void OnIndexChange()
		{
			if (selectedIndex != -1)
				SelectedItem = items[selectedIndex];

			if (IndexChange != null)
			{
				IndexChange(this, EventArgs.Empty);
			}
			
			
		}

		public event EventHandler Opened;
		private void OnOpened()
		{
			if (Opened != null)
				Opened(this, EventArgs.Empty);
		}

		public event EventHandler Closed;
		private void OnClosed()
		{
			if (Closed != null)
				Closed(this, EventArgs.Empty);
		}
	}

	//.......................................................
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||
	//'''''''''''''''''''''''''''''''''''''''''''''''''''''''

	class ListBox : ComboBox, Iinput
	{

		public ListBox(Point Position, int Length, int Height, SpriteFont SpriteFont, ComboBoxTexture comboBoxTexture)
			: base(Position, Length, SpriteFont, comboBoxTexture, Height-1)
		{
			initialize(Position, Length, Height, spriteFont);
		}

		public ListBox(Point Position, int Length, int Height, SpriteFont SpriteFont)
			: base(Position, Length, SpriteFont, Height-1)
		{
			initialize(Position, Length, Height, spriteFont);
		}

		private void initialize(Point Position, int Length, int Height, SpriteFont SpriteFont)
		{
			int height = Convert.ToInt32((int)SpriteFont.MeasureString("Tj").Y);
			Bounds = new Rectangle(Position.X, Position.Y, Length - scrollWheelWidth, height);
			items = new List<string>();
			itemsBounds = new List<Rectangle>();
			collapsible = false;
			Collapsed = false;
		}

		public override void Add(string item)
		{
			items.Add(item);
			itemsBounds.Add(Rectangle());
			if (_scroller != null)
			{
				if (showAmount < items.Count-1)
					_scroller.Add(1);
			}
			else
				showAmount = items.Count;

		}


		public override bool MouseInsideBounds()
		{
			return new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height * (itemsBounds.Count - (itemsBounds.Count - (showAmount+1)))).Contains(Mouse.GetState().Position);
		}

		public override Rectangle Rectangle()
		{
			return new Rectangle(Bounds.X, Bounds.Y + (Bounds.Height * (itemsBounds.Count)), Bounds.Width, Bounds.Height);
		}

		public override void Draw(SpriteBatch SpriteBatch)
		{
			_scroller.Draw(SpriteBatch);
			

			for (int i = offset; i <= showAmount + offset && i < items.Count; i++)
			{
				Rectangle temp = new Rectangle(itemsBounds[i].X, itemsBounds[i].Y - offset * Bounds.Height, itemsBounds[i].Width, itemsBounds[i].Height);

				SpriteBatch.Draw(comboBoxTexture.itemBackgroundTexture, temp, comboBoxTexture.itemBackgroundColor);

				if (selectedIndex == i)
					SpriteBatch.Draw(comboBoxTexture.selectedTexture, temp, comboBoxTexture.selectedColor);
				else if (tempIndex == i)
					SpriteBatch.Draw(comboBoxTexture.selectionTexture, temp, comboBoxTexture.selectionColor);
				

				SpriteBatch.DrawString(spriteFont, " " + items[i], new Vector2(itemsBounds[i].X, itemsBounds[i].Y - offset * Bounds.Height), Color.Black);

			}

		}

	}


	//.......................................................
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||
	//|||||||||||||||||||||||||||||||||||||||||||||||||||||||
	//'''''''''''''''''''''''''''''''''''''''''''''''''''''''

	public class MessagesBox
	{
		public static explicit operator MessagesBox(InputContainer a)
		{
			return new MessagesBox(a);
		}


		//Inputcontainer
		public InputContainer Inputs { get; private set; }


		public bool Active { get; private set; }


		//MessagesBox
		public List<MessagesBox> Messages { get; private set; }
		public bool IsMessageActive { get; private set; }

		public Rectangle Bounds { get; private set; }


		//------------------------------------------------------------
		//------------------------Constructors------------------------
		//------------------------------------------------------------

		public MessagesBox(TextureField BackgroundTexture)
		{
			Bounds = BackgroundTexture.Bounds;
			Inputs = new InputContainer(BackgroundTexture.Bounds.Location);
			Inputs.Add(BackgroundTexture, true);
		}

		public MessagesBox(InputContainer input)
		{
			this.Inputs = input;
		}

		//------------------------------------------------------
		//------------------------Methods-----------------------
		//------------------------------------------------------

		public void Add(Iinput Input)
		{
			Inputs.Add(Input, false, false);
		}

		public void Add(Iinput input, bool ignoreOffset)
		{
			Inputs.Add(input, ignoreOffset, false);
		}

		public void Add(params Iinput[] inputs  )
		{
			Inputs.Add(false, inputs);
		}

		public void Add(MessagesBox input)
		{
			this.Inputs.Add(input);
		}

		public void SetState(bool State)
		{
			if (State)
			{
				Activate();
			}
			else
			{
				Deactivate();
			}
			
		}

		protected virtual void Activate()
		{
			Active = true;
			Inputs.Focus();
			OnActivated();
		}

		protected virtual void Deactivate()
		{
			Active = false;
			Inputs.DeFocus();
			OnDeactivated();
		}

		public void Move(Point amountToMove)
		{
			Inputs.Move(amountToMove);
		}


		//------------------------------------------------------
		//------------------------Update------------------------
		//------------------------------------------------------

		public void Update()
		{
			if (!IsMessageActive)
			{
				Inputs.Update();
			}
		}

		//-------------------------------------------------------
		//------------------------Drawing------------------------
		//-------------------------------------------------------

		public void Draw(SpriteBatch spriteBatch)
		{
			Inputs.Draw(spriteBatch);

		}

		//------------------------------------------------------
		//------------------------EVENTS------------------------
		//------------------------------------------------------

		public event EventHandler<MessagesEventArgs> Activated;
		private void OnActivated()
		{
			if(Activated != null)
			{
				Activated(this, new MessagesEventArgs() { MessageBox = this });
			}
		}

		public event EventHandler<MessagesEventArgs> Deactivated;
		private void OnDeactivated()
		{
			if (Deactivated != null)
			{
				Deactivated(this, new MessagesEventArgs() { MessageBox = this });
			}
		}




	}


	public class ScrollerOld : Iinput
	{
		private ScrollerTexture scrollerTexture;
		public static ScrollerTexture StandardTexture;

		public List<Label> ScrollerLabels;
		private bool valueLabel;
		public bool ValueLabel
		{
			get { return valueLabel; }
			set
			{
				if (ScrollerLabels.Count > 0)
				{
					valueLabel = true;
				}
				else
				{
					valueLabel = false;
				}
			}
		}

		public string ValueLabelString = "";

		private int ButtonHeight = 10;

		public int MinimumCount = 0;
		private int itemCount;
		public int ItemCount
		{
			get
			{
				return itemCount;
			}
			set
			{

				Value = MinimumCount;
				itemCount = value;
				//ButtonHeight = ScrollerButtonHeight;
				if (Vertical)
				{
					if (ItemCount == 0)
					{
						ButtonHeight = ScrollBounds.Height;
					}
					else if (ButtonHeight < ScrollBounds.Height / (ItemCount - MinimumCount))
					{
						ButtonHeight = (int)(ScrollBounds.Height / (ItemCount - MinimumCount + 1) * 1.5f);
					}

					scrollButton.ButtonBounds = new Rectangle(scrollButton.ButtonBounds.X, scrollButton.ButtonBounds.Y, scrollButton.ButtonBounds.Width, ButtonHeight);

					
					if (ItemCount != 0)
					{
						Increment = (int)(1 / ((ScrollBounds.Height - ButtonHeight) / (float)itemCount));	
					}

				}
				else
				{

					if (ItemCount == 0)
					{
						ButtonHeight = ScrollBounds.Width;
					}
					else if (ButtonHeight < ScrollBounds.Width / (ItemCount - MinimumCount))
					{
						ButtonHeight = (int)(ScrollBounds.Width / (ItemCount - MinimumCount) * 1.5f);
					}
					else
					{
						ButtonHeight = ScrollBounds.Height;
					}
					//ScrollButton = new Rectangle(ScrollBounds.X, ScrollBounds.Y, ButtonHeight, ScrollBounds.Height);
					scrollButton.ButtonBounds = new Rectangle(scrollButton.ButtonBounds.X, scrollButton.ButtonBounds.Y, ButtonHeight, scrollButton.ButtonBounds.Height);

					if (ItemCount != 0)
					{
						Increment = (int)(1 / ((ScrollBounds.Width-ButtonHeight) / (float)itemCount));
					}

				}

				if (Increment == 0)
					Increment = 1;

				if (ItemCount <= 0)
					scrollButton.Disable();
				else
					scrollButton.Enable();

			}
		}
		int startPos = 0;
		int Increment = 1;
		float itemHeight = 0;

		public Color ScrollButtonColor = new Color(30, 30, 30);
		public Color BackGroundColor = Color.Silver;
		public Color UpDownButtonsColor = new Color(70, 70, 70);

		private Button upperButton;
		private Button lowerButton;
		private Button scrollButton;


		private Rectangle ScrollBounds;
        public Rectangle Bounds { get
            {
                return new Rectangle(upperButton.ButtonBounds.Location,
                  new Point(
                      upperButton.ButtonBounds.Width + lowerButton.ButtonBounds.Width + ScrollBounds.Width,
                      upperButton.ButtonBounds.Height));
            }
        }



		private bool scrolling = false;
		private bool Vertical;

		private int value;
		public int Value 
		{
			get
			{
				return value;
			} 
			
			
			set
			{
				if(value != Value)
				{
					if (value > itemCount)
						this.value = itemCount;
					else if (value < MinimumCount)
						this.value = MinimumCount;
					else
						this.value = value;

					OnValueChanged();
					updatePosition();
				}

				if(ScrollerLabels.Count > 0)
					ScrollerLabels[0].Text.Line = ValueLabelString + Value;
				
			}
		
		
		
		}

		

		//------------------------------------------------------
		//------------------------Constructors------------------
		//------------------------------------------------------

		public ScrollerOld(Rectangle Bounds,  ScrollerTexture scrollerTexture, bool Vertical = false)
		{
			this.scrollerTexture = scrollerTexture;
			initialize(Bounds, Vertical);

		}
		public ScrollerOld(Rectangle bounds, bool vertical = false)
		{
			initialize(bounds, vertical);

		}


		private void initialize(Rectangle Bounds, bool Vertical = false)
		{
			

			this.scrollerTexture = StandardTexture;

			ScrollerLabels = new List<Label>();
			this.Vertical = Vertical;

			if (Vertical)
			{
                ButtonHeight = Bounds.Width/2;
				upperButton = new Button(new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, ButtonHeight));
				lowerButton = new Button(new Rectangle(Bounds.X, Bounds.Bottom - ButtonHeight, Bounds.Width, ButtonHeight));
				ScrollBounds = new Rectangle(Bounds.X, Bounds.Y + ButtonHeight, Bounds.Width, Bounds.Height - 2 * ButtonHeight);
				scrollButton = new Button(ScrollBounds);
			}
			else
			{
                ButtonHeight = Bounds.Height/2;
				upperButton = new Button(new Rectangle(Bounds.X, Bounds.Y, ButtonHeight, Bounds.Height));
				lowerButton = new Button(new Rectangle(Bounds.Right - ButtonHeight, Bounds.Y, ButtonHeight, Bounds.Height));
				ScrollBounds = new Rectangle(Bounds.X + ButtonHeight, Bounds.Y, Bounds.Width - 2 * ButtonHeight, Bounds.Height);
				scrollButton = new Button(ScrollBounds);
			}

			upperButton.buttonTexture = scrollerTexture.upButton;
			lowerButton.buttonTexture = scrollerTexture.downButton;
			scrollButton.buttonTexture = scrollerTexture.scrollButton;


			upperButton.LeftClicked += delegate(object s, ButtonEventArgs e) { if (Value > 0) Value--; };
			lowerButton.LeftClicked += delegate(object s, ButtonEventArgs e) { if (Value < itemCount) Value++; };
			scrollButton.LeftClickedPressed += delegate(object s, ButtonEventArgs e)
			{
				if(!scrolling)
				{
					if (Vertical)
					{
						itemHeight = (((ScrollBounds.Height - ButtonHeight) * Increment) / (itemCount - MinimumCount));
						startPos = (int)((Mouse.GetState().Position.Y - (scrollButton.ButtonBounds.Y - ScrollBounds.Y)));
					}
					else
					{
						itemHeight = (((ScrollBounds.Width - ButtonHeight) * Increment) / (float)(itemCount - MinimumCount));
						startPos = (Mouse.GetState().Position.X - (scrollButton.ButtonBounds.X - ScrollBounds.X));
					}
						
				}

				scrolling = true;
			};

			scrollButton.Disable();
		}

        private void ScrollWheelChange(Point Position)
        {
            if (scrolling)
            {
                int oldValue = Value;
                if (Vertical)
                {
                    if (Value <= itemCount && Value >= MinimumCount)
                        Value = (int)((Position.Y - startPos) / itemHeight) * Increment + MinimumCount;
                }
                else
                {
                    if (Value <= itemCount && Value >= MinimumCount)
                        Value = (int)((Position.X - startPos) / itemHeight * Increment + MinimumCount);
                }
            }
        }

        private void OnLeftClickUp(object s, MouseEventArgs e)
		{
			scrolling = false;
			startPos = 0;
		}

		private void OnMouseMove(object s, MouseEventArgs e)
		{
			if (ItemCount != 0 && scrolling)
			{
				ScrollWheelChange(e.MouseState.Position);
			}
		}

		public void Move(Point amountToMove)
		{
			scrollButton.Move(amountToMove);
			lowerButton.Move(amountToMove);
			upperButton.Move(amountToMove);
			ScrollBounds = new Rectangle(ScrollBounds.X + amountToMove.X, ScrollBounds.Y + amountToMove.Y, ScrollBounds.Width, ScrollBounds.Height);

			foreach (Label l in ScrollerLabels)
			{
				l.Move(amountToMove);
			}
		}

		//------------------------------------------------------
		//------------------------Update------------------------
		//------------------------------------------------------


		public void Update()
		{

		}

		//------------------------------------------------------
		//------------------------Methods------------------------
		//------------------------------------------------------

		

		private void updatePosition()
		{
			if (Vertical)
			{
				int y = (int)((ScrollBounds.Y + ((ScrollBounds.Height - ButtonHeight) / (float)(itemCount - MinimumCount)) * (Value - MinimumCount)));
				scrollButton.ButtonBounds = new Rectangle(scrollButton.ButtonBounds.X, y, scrollButton.ButtonBounds.Width, scrollButton.ButtonBounds.Height);
			}
			else
			{
				int x = (int)((ScrollBounds.X + ((ScrollBounds.Width - ButtonHeight) / (float)(itemCount - MinimumCount)) * (Value - MinimumCount)));
				scrollButton.ButtonBounds = new Rectangle(x, scrollButton.ButtonBounds.Y, scrollButton.ButtonBounds.Width, scrollButton.ButtonBounds.Height);
			}
		}

		public void Add(int Amount)
		{
			ItemCount += Amount;
		}

		

		//-------------------------------------------------------
		//------------------------Drawing------------------------
		//-------------------------------------------------------

		public void Draw(SpriteBatch Spritebatch)
		{
			upperButton.Draw(Spritebatch);
			lowerButton.Draw(Spritebatch);

			Spritebatch.Draw(scrollerTexture.backgroundTexture, ScrollBounds, scrollerTexture.backgroundColor);

			scrollButton.Draw(Spritebatch);


			if (ScrollerLabels.Count > 0)
			{
				foreach (Label L in ScrollerLabels)
				{
					L.Draw(Spritebatch);
				}
			}
		}

		public virtual void Focus()
		{
			lowerButton.Focus();
			upperButton.Focus();
			scrollButton.Focus();

			InputController.MouseManager.LeftButtonUp += OnLeftClickUp;
			InputController.MouseManager.MouseMoved += OnMouseMove;
		}

		public virtual void DeFocus()
		{
			lowerButton.DeFocus();
			upperButton.DeFocus();
			scrollButton.DeFocus();

			InputController.MouseManager.LeftButtonUp -= OnLeftClickUp;
			InputController.MouseManager.MouseMoved -= OnMouseMove;
		}

		//------------------------------------------------------
		//------------------------Events------------------------
		//------------------------------------------------------

		public event EventHandler ValueChanged;
		private void OnValueChanged()
		{
			if (ValueChanged != null)
				ValueChanged(this, EventArgs.Empty);
		}

	}
}


