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
    public class NumberSelector : Iinput
    {
        private Button _increaseSize;
        private Button _decreaseSize;
        private LabelBox _sizeLabel;

        private int _buttonHeight = 20;
        private int _buttonWidth = 20;

        public Rectangle Bounds { get; private set; }
        public int Value { get; private set; }
        public int Step { get; private set; }

        public int Max { get; private set; }
        public int Min { get; private set; }

        private Repeater _repeater;
        private bool _increase;

        public NumberSelector(Rectangle bounds, int minimum, int maximum) : this(bounds, minimum, maximum, 20, 20) { }

        public NumberSelector(Rectangle bounds, int minimum, int maximum, int buttonHeight, int buttonWidth)
        {
            Bounds = bounds;
            _increase = false;

            Value = 1;
            Step = 1;
            Min = minimum;
            Max = maximum;

            _buttonHeight = buttonHeight;
            _buttonWidth = buttonWidth;

            _repeater = new Repeater(20);
            _repeater.Repeat += Repeater;


            _decreaseSize = new Button(new Rectangle(bounds.X, bounds.Y, _buttonWidth, _buttonHeight), new Text("<", Color.Black, InputController.DefaultFont), false);
            _decreaseSize.LeftClickedPressed += RepeaterUpdate;
            _decreaseSize.LeftClickedDown += Decrease;

            _sizeLabel = new LabelBox(new Text("1", Color.Black, InputController.DefaultFont), new TextureField(
                new Rectangle(_decreaseSize.ButtonBounds.Right, _decreaseSize.ButtonBounds.Y, bounds.Width - _decreaseSize.ButtonBounds.Width * 2, _buttonHeight),
                InputController.DefaultTexture, Color.White));

            _increaseSize = new Button(new Rectangle(_sizeLabel.BackDrop.Bounds.Right, bounds.Y, _buttonWidth, _buttonHeight), new Text(">", Color.Black, InputController.DefaultFont), false);
            _increaseSize.LeftClickedDown += Increase;
            _increaseSize.LeftClickedPressed += RepeaterUpdate;
        }

        public void Move(Point amountToMove)
        {
            _increaseSize.Move(amountToMove);
            _decreaseSize.Move(amountToMove);
            _sizeLabel.Move(amountToMove);
        }


        public virtual void Update()
        {
            _increaseSize.Update();
            _decreaseSize.Update();
            _sizeLabel.Update();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            _increaseSize.Draw(spriteBatch);
            _decreaseSize.Draw(spriteBatch);
            _sizeLabel.Draw(spriteBatch);
        }

        private void Decrease(object sender, EventArgs e)
        {
            ChangeValue(-Step);
            _repeater.Start(500);
            _increase = false;
        }

        private void Increase(object sender, EventArgs e)
        {

            ChangeValue(Step);
            _repeater.Start(500);
            Console.WriteLine("speed");
            _increase = true;
        }

        private void ChangeValue(int changeBy)
        {
            Value += changeBy;

            if (Value > Max)
                Value = Max;
            else if (Value < Min)
                Value = Min;

            OnValueChanged();
        }

        private void RepeaterUpdate(object sender, EventArgs e)
        {
            _repeater.Update();
        }

        private void Repeater(object sender, EventArgs e)
        {
            if (_increase)
                ChangeValue(Step);
            else
                ChangeValue(-Step);
        }



        public event EventHandler ValueChanged;
        private void OnValueChanged()
        {
            _sizeLabel.Text.Line = "" + Value;

            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }

        public virtual void Focus()
        {
            _decreaseSize.Focus();
            _increaseSize.Focus();
            _sizeLabel.Focus();
        }

        public virtual void DeFocus()
        {
            _decreaseSize.DeFocus();
            _increaseSize.DeFocus();
            _sizeLabel.DeFocus();
        }

    }

}
