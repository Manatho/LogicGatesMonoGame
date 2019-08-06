using GameEngine.UI.Control.Event;
using GameEngine.UI.Control.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.UI.Control
{
    public class Scroller : Iinput
    {
        private ScrollerTexture scrollerTexture;
        public static ScrollerTexture StandardTexture;

        private int ButtonSize = 10;
        public int MinimumSize = 10;

        public int MinimumCount = 0;
        private int itemCount;
        public int ItemCount{ get{return itemCount;} set { UpdateItemCount(value);}}

        int startValue;
        float startPos = 0;
        int Increment = 1;
        float itemHeight = 0; //Pixels between each
        private bool scrolling = false;
        private bool Vertical;

        private int buttonUp;
        private int buttonDown;
        private int scrollBounds;
        private Rectangle normalizedBounds;

        private Button upperButton;
        private Button lowerButton;
        private Button scrollButton;

        private Rectangle ScrollBounds;
        public Rectangle Bounds { get { return new Rectangle(upperButton.ButtonBounds.Location, 
            new Point( upperButton.ButtonBounds.Width + lowerButton.ButtonBounds.Width + ScrollBounds.Width, upperButton.ButtonBounds.Height));}}

        private int value;
        public int Value {  get { return value; }
            set
            {
                if (value != Value)
                {
                    if (value > itemCount)
                        this.value = itemCount;
                    else if (value < MinimumCount)
                        this.value = MinimumCount;
                    else
                        this.value = value;

                    OnValueChanged();
                    UpdateScrollButton();
                }
            }
        }

        //------------------------------------------------------
        //------------------------Constructors------------------
        //------------------------------------------------------

        public Scroller(int x, int y, int height, int thickness, ScrollerTexture scrollerTexture, bool Vertical = false)
        {
            this.scrollerTexture = scrollerTexture;
            Initialize(x,y,height,thickness, Vertical);

        }
        public Scroller(int x, int y, int height, int thickness, bool vertical = false)
        {
            Initialize(x, y, height, thickness, vertical);
        }

        public void ChangeButtonSize(int top, int bottom)
        {
            buttonDown = bottom;
            buttonUp = top;
            scrollBounds = normalizedBounds.Height - buttonUp - buttonDown;
            SetButtonSizes();
            UpdateScrollButton();
        }

        [Obsolete("Use integer based constructors")]
        public Scroller(Rectangle bounds, bool vertical = false)
        {
            Initialize(bounds.X, bounds.Y, bounds.Width, bounds.Height, vertical);
        }

        private void Initialize(int x, int y, int height, int thickness, bool Vertical = false)
        {
            scrollerTexture = StandardTexture;
            this.Vertical = Vertical;

            buttonUp = ButtonSize;
            buttonDown = ButtonSize;
            scrollBounds = height - buttonUp - buttonDown;

            normalizedBounds = new Rectangle(x, y, thickness, height);

            upperButton = new Button(new Rectangle());
            lowerButton = new Button(new Rectangle());
            scrollButton = new Button(new Rectangle());
            SetButtonSizes();

            upperButton.buttonTexture = scrollerTexture.upButton;
            lowerButton.buttonTexture = scrollerTexture.downButton;
            scrollButton.buttonTexture = scrollerTexture.scrollButton;

            scrollButton.Disable();

            upperButton.LeftClicked += delegate (object s, ButtonEventArgs e) { if (Value > 0) Value--; };
            lowerButton.LeftClicked += delegate (object s, ButtonEventArgs e) { if (Value < itemCount) Value++; };
            scrollButton.LeftClickedPressed += delegate (object s, ButtonEventArgs e) { ScrollPress(Vertical ? e.MousePos.Y : e.MousePos.X); };
        }

        //------------------------------------------------------------
        //------------------------ActualButtons-----------------------
        //------------------------------------------------------------

        private void SetButtonSizes()
        {
            if (Vertical)
            {
                upperButton.ButtonBounds = (new Rectangle(normalizedBounds.X, normalizedBounds.Y, normalizedBounds.Width, buttonUp));
                lowerButton.ButtonBounds = (new Rectangle(normalizedBounds.X, normalizedBounds.Bottom - buttonDown, normalizedBounds.Width, buttonDown));
                ScrollBounds = new Rectangle(normalizedBounds.X, normalizedBounds.Y + buttonUp, normalizedBounds.Width, scrollBounds);
                scrollButton.ButtonBounds = (ScrollBounds);
            }
            else
            {
                upperButton.ButtonBounds = (new Rectangle(normalizedBounds.X, normalizedBounds.Y, buttonUp, normalizedBounds.Width));
                lowerButton.ButtonBounds = (new Rectangle(normalizedBounds.X + normalizedBounds.Height -buttonDown, normalizedBounds.Y, buttonDown, normalizedBounds.Width));
                ScrollBounds = new Rectangle(normalizedBounds.X + buttonUp, normalizedBounds.Y, scrollBounds, normalizedBounds.Width);
                scrollButton.ButtonBounds = (ScrollBounds);
            }
        }

        private void UpdateScrollButton()
        {
            int temp = (int)(((scrollBounds - ButtonSize) / (float)(itemCount - MinimumCount)) * (Value - MinimumCount));

            if (Vertical)
                scrollButton.ButtonBounds = new Rectangle(scrollButton.ButtonBounds.X, ScrollBounds.Y + temp, scrollButton.ButtonBounds.Width, ButtonSize);
            else
                scrollButton.ButtonBounds = new Rectangle(ScrollBounds.X + temp, scrollButton.ButtonBounds.Y, ButtonSize, scrollButton.ButtonBounds.Height);
        }

        private void ScrollPress(int offset)
        {
            if (!scrolling)
            {
                itemHeight = (((scrollBounds - ButtonSize) * (float)Increment) / (float)(itemCount - MinimumCount));
                startPos = offset;
                startValue = Value;
            }
            scrolling = true;
        }

        private void ScrollWheelChange(int Position)
        {
            if (scrolling)
            {
                int oldValue = Value;

                if (Value <= itemCount && Value >= MinimumCount)
                    Value = (int)(MinimumCount + startValue + (Increment * Position - Increment * startPos) / itemHeight + Increment * oldValue - Increment * startValue);
                    //Value = (int)(((Position - startPos + oldValue * itemHeight) / itemHeight) * Increment + MinimumCount- ((ItemCount*(Increment-1))*startValue/(float)ItemCount));
                    //Commented bit is the old calculation, above is a "simplified" version. The last bit of the above (after minimumCount) Is magic. The first bit works when
                    // the startValue is 0, but fails if higher, (Item*(Increment-1) fixes the issue when startValue = ItemCount, (StartValue/ItemCount - scales this)

                if (oldValue != value)
                    startPos += ((value - oldValue) * itemHeight);
            }
        }

        private void UpdateItemCount(int value)
        {
            Value = MinimumCount;
            itemCount = value;

            if (ItemCount == 0)
                ButtonSize = scrollBounds; //Fill The scrollbar
            else if (MinimumSize < scrollBounds / (ItemCount - MinimumCount))
                ButtonSize = (int)(scrollBounds / (ItemCount - MinimumCount) * 1.5f); //Scale button, 1.5f is for making it look better
            else
                    ButtonSize = MinimumSize;

            if (ItemCount != 0)
                Increment = (int)(1 / ((scrollBounds - ButtonSize) / (float)itemCount)); //Due to the minimumsize scrolling is not 1 to 1 and requires scaling.

            if (Increment == 0)
                Increment = 1;

            if (ItemCount <= 0)
                scrollButton.Disable();
            else
                scrollButton.Enable();

            UpdateScrollButton();
        }

        public void Add(int Amount)
        {
            ItemCount += Amount;
        }


        public event EventHandler ValueChanged;
        private void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }

        public void Move(Point amountToMove)
        {
            scrollButton.Move(amountToMove);
            lowerButton.Move(amountToMove);
            upperButton.Move(amountToMove);
            ScrollBounds = new Rectangle(ScrollBounds.X + amountToMove.X, ScrollBounds.Y + amountToMove.Y, ScrollBounds.Width, ScrollBounds.Height);

        }

        //------------------------------------------------------
        //------------------------Update------------------------
        //------------------------------------------------------

        public void Update() { }

        public void Draw(SpriteBatch Spritebatch)
        {
            upperButton.Draw(Spritebatch);
            lowerButton.Draw(Spritebatch);

            Spritebatch.Draw(scrollerTexture.backgroundTexture, ScrollBounds, scrollerTexture.backgroundColor);

            scrollButton.Draw(Spritebatch);
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

        private void OnLeftClickUp(object s, MouseEventArgs e)
        {
            scrolling = false;
            startPos = 0;
        }

        private void OnMouseMove(object s, MouseEventArgs e)
        {
            if (ItemCount != 0 && scrolling)
                ScrollWheelChange(Vertical ? e.MouseState.Y : e.MouseState.X);
        }

        

    }
}
