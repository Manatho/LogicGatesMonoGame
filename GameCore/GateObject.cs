using GameEngine.DrawingUtil;
using GameEngine.UI;
using LogicGateFront.PointExtensions;
using LogicGates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LogicGateFront.GameCore
{
    public class GateObject : IGate
    {
        protected IGate gate;
        protected Texture2D gateTexture;

        public Point Position
        {
            get { return _area.Location; }
            set
            {
                var diffB = _boundingBox.Location - _area.Location;
                var diffO = _originalPosition - _area.Location;
                _area.Location = value;
                _boundingBox.Location = value + diffB;
                _originalPosition = value + diffO;
            }
        }

        protected Point _originalPosition;
        protected Rectangle _area;
        protected Rectangle _boundingBox;

        private int size = 10;
        private int padding = 5;
        private float pinsize = 4;
        private float rotation = 0;

        private List<Vector2> InputPins;
        public Vector2 GetInputPin(int index) => InputPins[index] + Position.ToVector2();
        private List<Vector2> OutputPins;
        public Vector2 GetOutputPin(int index) => OutputPins[index] + Position.ToVector2();

        public SelectedPin SelectedPin { get; private set; }

        public bool BodySelected { get; private set; } = false;
        public bool Selected { get; set; } = false;

        public Action<GateObject> OnLeftClickDown { get; set; }
        public Action<GateObject> OnLeftClickUp { get; set; }


        public GateObject(IGate gate, Texture2D gateTexture, Point position)
        {
            this.gate = gate;
            SelectedPin = new SelectedPin();

            int width = Math.Max(gate.Inputs.Length, gate.Outputs.Length);
            _area = new Rectangle(position, new Point(size + padding * 2, size * width + padding * 2));
            _originalPosition = _area.Location;
            this.gateTexture = createGateTexture(gateTexture);


            _boundingBox = _area;
            _boundingBox.Inflate(padding * 3, 0);

            InputPins = CreatePinPosition(gate.Inputs.Length, -padding * 2, _area.Height);
            OutputPins = CreatePinPosition(gate.Outputs.Length, padding * 2 + _area.Width, _area.Height);

            Focus();
        }

        private Texture2D createGateTexture(Texture2D gateTexture)
        {
            return Drawing.GenerateTexture(InputController.GraphicsDevice, _area.Width, _area.Height, (SpriteBatch spriteBatch) =>
            {
                Rectangle first = new Rectangle(0, 0, _area.Width, _area.Width / 2);
                Rectangle second = new Rectangle(0, _area.Width / 2, _area.Width, _area.Height - _area.Width);
                Rectangle third = new Rectangle(0, _area.Height - _area.Width / 2, _area.Width, _area.Width / 2);

                spriteBatch.Draw(gateTexture, first, new Rectangle(0, 0, 200, 99), Color.White);
                spriteBatch.Draw(gateTexture, second, new Rectangle(0, 99, 200, 2), Color.White);
                spriteBatch.Draw(gateTexture, third, new Rectangle(0, 101, 200, 99), Color.White);
            });
        }

        private List<Vector2> CreatePinPosition(int pins, int offset, int length)
        {
            List<Vector2> points = new List<Vector2>();

            if (pins == 0)
                return points;

            float spacing = (length / (float)pins) / (float)2;
            float current = 0;

            for (int i = 0; i < pins; i++)
            {
                current += spacing;
                points.Add(new Vector2(offset, current));
                current += spacing;
            }
            return points;
        }

        public void Rotate90()
        {

            var oldPosition = Position.ToVector2();
            var oldCenter = _area.Center.ToVector2();

            _area = RotateRectangle(_area, 1);
            _boundingBox = RotateRectangle(_boundingBox, 1);

            for (int i = 0; i < InputPins.Count; i++)
            {
                InputPins[i] = (InputPins[i] + oldPosition).Rotate(oldCenter, (float)Math.PI / 2) - _area.Location.ToVector2();
            }

            for (int i = 0; i < OutputPins.Count; i++)
            {
                OutputPins[i] = (OutputPins[i] + oldPosition).Rotate(oldCenter, (float)Math.PI / 2) - _area.Location.ToVector2();
            }

            rotation += (float)Math.PI / 2f;
            //rotation += 0.1f;

        }

        private Rectangle RotateRectangle(Rectangle rect, int a)
        {
            Vector2 centerOfRotation = rect.Center.ToVector2();
            Vector2 newLocation = rect.Location.ToVector2().Rotate(centerOfRotation, ((float)Math.PI / 2) * a);
            Vector2 newDiminsions = (rect.Location.ToVector2() + new Vector2(rect.Width, rect.Height)).Rotate(centerOfRotation, ((float)Math.PI / 2) * a);

            int x = 0;
            int width = 0;
            int y = 0;
            int height = 0;
            if (newLocation.X > newDiminsions.X)
            {
                x = (int)newDiminsions.X;
                width = (int)(newLocation.X - newDiminsions.X);
            }
            else
            {
                x = (int)newLocation.X;
                width = (int)(newDiminsions.X - newLocation.X);
            }

            if (newLocation.Y > newDiminsions.Y)
            {
                y = (int)newDiminsions.Y;
                height = (int)(newLocation.Y - newDiminsions.Y);
            }
            else
            {
                y = (int)newLocation.Y;
                height = (int)(newDiminsions.Y - newLocation.Y);
            }
            return new Rectangle(x, y, width, height);

        }

        public virtual void Focus()
        {
            InputController.MouseManager.MouseMoved += MouseManager_MouseMoved;

            InputController.MouseManager.LeftButtonDown += OnLeftClickedDown;
            InputController.MouseManager.LeftButtonUp += OnLeftClickedUp;

        }

        public virtual void Defcous()
        {
            InputController.MouseManager.MouseMoved -= MouseManager_MouseMoved;

            InputController.MouseManager.LeftButtonDown -= OnLeftClickedDown;
            InputController.MouseManager.LeftButtonUp -= OnLeftClickedUp;
        }

        private void MouseManager_MouseMoved(object sender, MouseEventArgs e)
        {
            BodySelected = false;
            SelectedPin = new SelectedPin();
            if (_boundingBox.Contains(e.MouseState.Position))
            {
                if (_area.Contains(e.MouseState.Position))
                {
                    BodySelected = true;
                }
                else
                {
                    for (int i = 0; i < InputPins.Count; i++)
                    {
                        Vector2 v = (InputPins[i] + Position.ToVector2());
                        if (Vector2.Distance(v, e.MouseState.Position.ToVector2()) <= pinsize)
                            SelectedPin = new SelectedPin(i + 1, InputPins, OutputPins, Position.ToVector2()); ; // selectpin == 0: null, < 0: Output, > 0: Input


                    }

                    for (int i = 0; i < OutputPins.Count; i++)
                    {
                        Vector2 v = (OutputPins[i] + Position.ToVector2());
                        if (Vector2.Distance(v, e.MouseState.Position.ToVector2()) <= pinsize)
                            SelectedPin = new SelectedPin(-i - 1, InputPins, OutputPins, Position.ToVector2()); // selectpin == 0: null, < 0: Output, > 0: Input
                    }
                }
            }
        }

        private void OnLeftClickedUp(object sender, MouseEventArgs e)
        {
            if (_boundingBox.Contains(e.MouseState.Position))
            {
                if (OnLeftClickUp != null)
                    OnLeftClickUp(this);
            }
        }

        private void OnLeftClickedDown(object sender, MouseEventArgs e)
        {
            if (SelectedPin.IsSet || BodySelected)
            {
                if (OnLeftClickDown != null)
                    OnLeftClickDown(this);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (SelectedPin.IsSet)
            {
                spriteBatch.Circle(SelectedPin.PinPosition, pinsize + 1, Color.Yellow);
            }

            foreach (Vector2 point in InputPins)
            {
                Vector2 p = point + Position.ToVector2();
                spriteBatch.Line(p, _area.Center.ToVector2(), 2, Color.White);
                spriteBatch.Circle(p, pinsize, Color.LightBlue);
            }

            foreach (Vector2 point in OutputPins)
            {
                Vector2 p = Position.ToVector2() + point;
                spriteBatch.Line(p, _area.Center.ToVector2(), 2, Color.White);
                spriteBatch.Circle(p, pinsize, Color.PaleVioletRed);
            }

            if (BodySelected || Selected)
            {
                Rectangle rect = _area;
                rect.Inflate(1, 1);
                spriteBatch.StrokeRect(rect, 2, Color.Red);
            }

            Point test = _originalPosition;
            test.X += gateTexture.Width / 2;
            test.Y += gateTexture.Height / 2;

            Rectangle desp = new Rectangle(test, gateTexture.Bounds.Size);

            if (Outputs[0])
                spriteBatch.Draw(gateTexture, desp, null, Color.LightGreen, rotation, gateTexture.Bounds.Size.ToVector2() / 2, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(gateTexture, desp, null, Color.White, rotation, gateTexture.Bounds.Size.ToVector2()/2, SpriteEffects.None, 0);
        

            //spriteBatch.StrokeRect(_boundingBox, 2, Color.Red);
        }

        #region Pass through to the gate
        public bool[] Inputs => gate.Inputs;
        public bool[] Outputs => gate.Outputs;
        public Connection[] InputConnections => gate.InputConnections;
        public Connection[] OutputConnections => gate.OutputConnections;
        public bool OutputChanged => gate.OutputChanged;

        public bool GetInput(int index)
        {
            return gate.GetInput(index);
        }

        public bool GetOutput(int index)
        {
            return gate.GetOutput(index);
        }

        public void SetInputConnection(Connection connection)
        {
            gate.SetInputConnection(connection);
        }

        public void SetInputConnection(IGate outputee, int output, int input)
        {
            gate.SetInputConnection(outputee, output, input);
        }

        public void SetOutputConnection(Connection connection)
        {
            gate.SetOutputConnection(connection);
        }

        public void SetOutputConnection(IGate outputee, int output, int input)
        {
            gate.SetOutputConnection(outputee, output, input);
        }

        public void Update()
        {
            gate.Update();
        }
        #endregion
    }

    public class SelectedPin
    {
        private int selectedPin;

        public bool IsInput => selectedPin > 0;
        public bool IsOutput => selectedPin < 0;
        public bool IsSet => selectedPin != 0;

        public Vector2 PinPosition { get; private set; }
        public int PinIndex { get; private set; }

        public SelectedPin(int selectedPin, List<Vector2> inputPins, List<Vector2> outputPins, Vector2 offset)
        {
            this.selectedPin = selectedPin;
            this.PinPosition = IsSet ? (IsInput ? inputPins[selectedPin - 1] + offset : outputPins[-1 * selectedPin - 1] + offset) : Vector2.Zero;
            this.PinIndex = IsSet ? (IsInput ? selectedPin - 1 : -1 * selectedPin - 1) : -1;
        }

        public SelectedPin()
        {
            this.selectedPin = 0;
            this.PinPosition = Vector2.Zero;
        }
    }
}
