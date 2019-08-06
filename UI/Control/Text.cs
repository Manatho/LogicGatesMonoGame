using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.UI
{

    /// <summary>
    /// Container for text to be drawn, defines Color and Font in addition to the line.
    /// </summary>
	public class Text
	{
		public Color Color;
		public string Line;
		public SpriteFont Font;

		public Point Size { get { return Font.MeasureString(Line).ToPoint();} }
		public int Height { get { return (int)Font.MeasureString(Line).Y; } }
		public int Width { get { return (int)Font.MeasureString(Line).X; } }

		public Text(string lines, Color color, SpriteFont font)
		{
			Color = color;
			Line = lines;
			Font = font;

		}

        public Text(string lines, Color color)
            : this(lines, color, InputController.DefaultFont)
        {

        }

        public Point MeasureString(int length)
        {
            return Font.MeasureString(Line.Substring(0, length)).ToPoint();
        }

		public virtual void Draw(SpriteBatch spriteBatch, Point position)
		{
			spriteBatch.DrawString(Font, Line, position.ToVector2(), Color);
			
		}	

		public virtual Text Copy()
		{
			return new Text(Line, Color, Font);
		}

		public override string ToString()
		{
			return Line;
		}

	}


    /// <summary>
    /// Spezialitation of 'Text' that renders it differently
    /// </summary>
	public class OutLinedText : Text
	{
		public Color OutLineColor;

		public OutLinedText(string lines, Color color, Color outlineColor, SpriteFont font)
			: base(lines, color, font)
		{
			OutLineColor = outlineColor;
		}

		public override void Draw(SpriteBatch spriteBatch, Point position)
		{


			spriteBatch.DrawString(Font, Line, new Vector2(position.X, position.Y + 1), OutLineColor);
			spriteBatch.DrawString(Font, Line, new Vector2(position.X - 1, position.Y), OutLineColor);
			spriteBatch.DrawString(Font, Line, new Vector2(position.X, position.Y - 1), OutLineColor);
			spriteBatch.DrawString(Font, Line, new Vector2(position.X + 1, position.Y), OutLineColor);

			spriteBatch.DrawString(Font, Line, position.ToVector2(), OutLineColor);
			base.Draw(spriteBatch, position);
		}

		public override Text Copy()
		{
			return new OutLinedText(Line, Color, OutLineColor, Font);
		}

	}

	[Flags]
	public enum Alignment { Center = 0, Left = 1, Right = 2, Top = 4, Bottom = 8 }

	public class Label : Iinput
	{
		public Text Text;
		public Point Position;
		public Alignment Alignment = Alignment.Bottom | Alignment.Right;

		public Label(Text text, Point position)
		{
			Text = text;
			Position = position;
		}

		public Label(string text, Color color, SpriteFont spritefont, Point position)
			: this(new Text(text, color, spritefont), position)
		{
				
		}

		public void Move(Point amountToMove)
		{
			Position += amountToMove;
		}

		public void Update()
		{

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Point temp = Position;

			if(Alignment == Alignment.Center)
			{
				temp.X -= Text.Width / 2;
				temp.Y -= Text.Height / 2;
			}
			else
			{
				if (Alignment.HasFlag(Alignment.Left))
					temp.X -= Text.Width;


				if (Alignment.HasFlag(Alignment.Top))
					temp.Y -= Text.Height;
			}

			Text.Draw(spriteBatch, temp);
		}

		public void Focus()
		{

		}

		public void DeFocus()
		{

		}
	}

	public class LabelBox : Iinput
	{
		public Text Text;
		public TextureField BackDrop;

        public bool Hover = false;

        protected Point origin;

		public Alignment Alignment = Alignment.Center;
		public Rectangle Padding = Rectangle.Empty;

		public LabelBox(Text text, TextureField backDrop)
		{
			Text = text;
			BackDrop = backDrop;
		}

		public LabelBox(string text, Color color, SpriteFont spritefont, Rectangle bounds)
			: this(new Text(text, color, spritefont), new TextureField(bounds, null, Color.White))
		{
				
		}

		public LabelBox(Text text, Rectangle bounds)
			: this(text, new TextureField(bounds, null, Color.White))
		{

		}

		public void Move(Point amountToMove)
		{
			BackDrop.Move(amountToMove);
		}

		public virtual void Update()
		{

		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			Rectangle bounds = BackDrop.Bounds;

			Point size = Text.Size;
			origin = bounds.Center - (size.ToVector2() / 2).ToPoint();

			if (Alignment.HasFlag(Alignment.Right))
				origin.X += bounds.Width / 2 - size.X / 2 - Padding.Width;

			if (Alignment.HasFlag(Alignment.Left))
				origin.X -= bounds.Width / 2 - size.X / 2 + Padding.X;

			if (Alignment.HasFlag(Alignment.Bottom))
				origin.Y += bounds.Height / 2 - size.Y / 2 - Padding.Height;

			if (Alignment.HasFlag(Alignment.Top))
				origin.Y -= bounds.Height / 2 - size.Y / 2 + Padding.Y;

			BackDrop.Draw(spriteBatch);


			Text.Draw(spriteBatch, origin);
		}

        public event EventHandler BoundsCrossed;
        private void OnHover()
        {
            if (BoundsCrossed != null)
                BoundsCrossed(this, EventArgs.Empty);
        }

        private void MouseManager_MouseMoved(object sender, MouseEventArgs e)
        {
            bool temp = BackDrop.Bounds.Contains(e.MouseState.Position);

            if(temp != Hover)
            {
                Hover = temp;
                OnHover();
            }


        }

        public virtual void Focus()
		{
            InputController.MouseManager.MouseMoved += MouseManager_MouseMoved;
		}



        public virtual void DeFocus()
		{
            InputController.MouseManager.MouseMoved -= MouseManager_MouseMoved;
        }
	}


    /// <summary>
    /// Textbox for typing in inputs
    /// </summary>
	public class TextBox : Iinput
	{
		public bool IgnoreHeight = false;

		private Rectangle _padding;
		public Rectangle Padding { get { return _padding; } set { _padding = value; MakeLines(); Align(); } }

		public Rectangle Bounds { get { return _backDrop.Bounds; } set { _backDrop.Bounds = value; MakeLines(); Align(); } }
		public Rectangle PaddedBounds { get { return new Rectangle(Bounds.X + Padding.X, Bounds.Y + Padding.Y, Bounds.Width - Padding.Width - Padding.X, Bounds.Height - Padding.Height - Padding.Y); } }

		public Alignment Alignment = Alignment.Left;
		public Alignment LineAlignment = Alignment.Left;

		public string Text { get { return _text.Line; } set { _text.Line = value; MakeLines(); Align(); } }
		public Color Color { get { return _text.Color; } set { _text.Color = value; UpdateFont(); } }
		

		private int _lineHeight;
		private Text _text;
		private List<LabelBox> _labelTexts;
		private TextureField _backDrop;
		
		public TextBox(Text text, TextureField backDrop)
		{
			_padding = new Rectangle(5, 5, 5, 5);
			_text = text;
			_backDrop = backDrop;
			

			MakeLines();
			Align();
		}

		public TextBox(string text, Color color, SpriteFont spritefont, Rectangle bounds)
			: this(new Text(text, color, spritefont), new TextureField(bounds, null, Color.White))
		{

		}

		public TextBox(Text text, Rectangle bounds)
			:this(text,  new TextureField(bounds, null, Color.White))
		{

		}

		private void MakeLines()
		{


			_labelTexts = new List<LabelBox>();
			string[] newlines = _text.Line.Split('\n');
			int height = 0;

			foreach (string s in newlines)
			{
				float overshot = (float)_text.Font.MeasureString(s).X / (PaddedBounds.Width);

				if (overshot > 1)
				{
					List<string> stringSplit = new List<string>();
					int length = (int)(s.Length / overshot);
					int offset = 0;
					string remaining = s;

					if (length == 0)
						break;

					while (true)
					{
						int temp = (stringSplit.Count + 1) * length + offset;
						if (temp < remaining.Length)
							stringSplit.Add(remaining.Substring(stringSplit.Count * length + offset, length));
						else if (temp - length < remaining.Length)
							stringSplit.Add(remaining.Substring(stringSplit.Count * length + offset));
						else
							break;


						if(stringSplit.Count > 1)
						{
							int index1 = stringSplit.Count - 1;
							int index2 = stringSplit.Count - 2;

							if (stringSplit[index1][0] != ' ' && stringSplit[index2][stringSplit[index2].Length - 1] != ' ')
							{
								List<string> tempStrings = stringSplit[index2].Split(' ').ToList();
								string move = tempStrings[tempStrings.Count - 1];

								string trying = move + stringSplit[index1];
								tempStrings = trying.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
								if (tempStrings[0].Length < length)
								{
									stringSplit[index2] = stringSplit[index2].Remove(stringSplit[index2].Length - move.Length);
									stringSplit[index1] = stringSplit[index1].Insert(0, move);
								
									if (stringSplit[index1].Length > length)
									{
										move = "";
										tempStrings = stringSplit[index1].Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries).ToList();


										while (stringSplit[index1].Length > length && tempStrings.Count > 1)
										{
											move = tempStrings[tempStrings.Count - 1];
											tempStrings.RemoveAt(tempStrings.Count - 1);
											stringSplit[index1] = stringSplit[index1].Remove(stringSplit[index1].Length - move.Length);
											offset -= move.Length;
										}
									}
								}
							}
						}
					}



					for (int i = 0; i < stringSplit.Count; i++)
						if (!makeline(i, height, stringSplit[i].Trim(), out height))
							break;
				}
				else
					if (!makeline(_labelTexts.Count, height, s.Trim(), out height))
						break;
			}

			_lineHeight = height;
		}

		private bool makeline(int index, int height, string s, out int newHeight)
		{

			Text temp = _text.Copy();
			temp.Line = s;



			if (height + temp.Height <= PaddedBounds.Height || IgnoreHeight)
			{
				_labelTexts.Add(new LabelBox(temp, new Rectangle(PaddedBounds.X, PaddedBounds.Y + height, PaddedBounds.Width, temp.Height)));
				height += _labelTexts[index].Text.Height;
				newHeight = height;
				return true;
			}
			else
			{
				if(_labelTexts.Count > 0)
				{
					string test = _labelTexts[_labelTexts.Count - 1].Text.Line;
					if (test.Length < 3)
						_labelTexts[_labelTexts.Count - 1].Text.Line = "...";
					else
						_labelTexts[_labelTexts.Count - 1].Text.Line = test.Replace(test.Substring(test.Length - 3), "...");

				}

				newHeight = height;
				return false;
			}
		}

		private void Align()
		{
			foreach (LabelBox l in _labelTexts)
				l.Alignment = LineAlignment;


			if(Alignment == Alignment.Center)
			{
				int temp = (PaddedBounds.Height - _lineHeight) / 2;

				foreach (LabelBox l in _labelTexts)
					l.Move(new Point(0, temp));
				
			}
			else
			{

			}
		}

		private void UpdateFont()
		{
			foreach (LabelBox l in _labelTexts)
				l.Text.Color = Color;
		}


		
		public void Move(Point amountToMove)
		{
			_backDrop.Move(amountToMove);

			foreach (LabelBox l in _labelTexts)
				l.Move(amountToMove);
		}

		public void Update()
		{

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			_backDrop.Draw(spriteBatch);

			foreach (LabelBox l in _labelTexts)
				l.Draw(spriteBatch);
		}

		public virtual void Focus()
		{

		}

		public virtual void DeFocus()
		{

		}

		
	}




}
