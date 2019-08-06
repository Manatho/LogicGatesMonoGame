using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;




namespace GameEngine.UI
{
    using Keys = Microsoft.Xna.Framework.Input.Keys;
    using Microsoft.Xna.Framework.Graphics;

    public class InputBox : LabelBox
	{


		private bool _showCursor = false;

        private int cursorOffset { get { return _cursorOffset; }
            set
            {
                _cursorOffset = value;
                if (_cursorOffset < 0)
                    _cursorOffset = 0;
                else if (_cursorOffset > Text.Line.Length)
                    _cursorOffset = Text.Line.Length;
                }
        }

        private int _cursorOffset = 0;

		private double time;
		
		public bool InFocus;
		private bool _active = false;

		public InputBox(Text text, Rectangle bounds)
			:base(text, bounds)
		{
			Alignment = Alignment.Left;
		}

		public InputBox(Text text, TextureField textureField)
			:base(text, textureField)
		{
			Alignment = Alignment.Left;
		}

		private void ChangeText(object s, KeyboardEventArgs e)
		{
			if(InFocus)
			{
                string value = "";
                switch (e.Key)
                {
                    case Keys.OemMinus:
                        value = "-";
                        break;
                    case Keys.OemPeriod:
                        value = ".";
                        break;
                    case Keys.Back:
                        if(Text.Line.Length > 0)
                            Text.Line = Text.Line.Remove(Text.Line.Length - 1 - cursorOffset,1);
                        break;
                    case Keys.Enter:
                        InFocus = false;
                        OnLostFocus();
                        break;
                    case Keys.Left:
                        cursorOffset++;
                        break;
                    case Keys.Right:
                        cursorOffset--;
                        break;

                    default:
                        if (e.Key >= Keys.A && e.Key <= Keys.Z)
                        {

                            if (e.Keys.Contains(Keys.LeftShift) || e.Keys.Contains(Keys.RightShift))
                                value = e.Key.ToString();
                            else
                                value = e.Key.ToString().ToLower();

                        }

                        if (e.Key >= Keys.D0 && e.Key <= Keys.D9)
                        {
                            value += e.Key.ToString()[1];
                        }
                        break;
                }

                Text.Line = Text.Line.Insert(Text.Line.Length - cursorOffset, value);
			}
			
		}

		public override void Update()
		{
			double temp = InputController.UiTime.TotalGameTime.TotalMilliseconds;

			if(InFocus)
			{
				if (temp - time > 600)
				{
					time = temp;
					if (_showCursor)
					    _showCursor = false;
					else
                        _showCursor = true;
                }
			}
		}

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (_showCursor)
                spriteBatch.DrawString(Text.Font, "|", (origin + new Point(Text.MeasureString(Text.Line.Length - cursorOffset).X - 1,0)).ToVector2(), Color.Red);
        }

		public override string ToString()
		{
			return Text.Line;
		}



		private void CheckCollision(object s, MouseEventArgs e)
		{
			if (BackDrop.Bounds.Contains(e.MouseState.Position))
			{
				OnHasFocus();
				InFocus = true;	
			}
			else if(InFocus)
			{
				InFocus = false;
				OnLostFocus();
			}
				
		}

		public event EventHandler HasFocus;
		private void OnHasFocus()
		{
			if (HasFocus != null)
				HasFocus(this, EventArgs.Empty);
		}

		public event EventHandler LostFocus;
		private void OnLostFocus()
		{
			if (LostFocus != null)
				LostFocus(this, EventArgs.Empty);

            OnValueChanged();
            _showCursor = false;
        }

        public event EventHandler ValueChanged;
        private void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }

		public override void Focus()
		{
			if(!_active)
			{
				InputController.MouseManager.LeftButtonDown += CheckCollision;
				InputController.KeyboardManager.KeyDown += ChangeText;
				_active = true;
			}
		}

		public override void DeFocus()
		{
			if(_active)
			{
				InputController.MouseManager.LeftButtonDown -= CheckCollision;
				InputController.KeyboardManager.KeyDown -= ChangeText;
				_active = false;
			}
			
		}

	}


 

}
