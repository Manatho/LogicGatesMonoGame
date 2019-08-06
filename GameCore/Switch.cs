using GameEngine.UI;
using LogicGates;
using LogicGates.DefaultGates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicGateFront.GameCore
{
    public class SwitchObject : GateObject
    {
        private Rectangle onArea;

        public SwitchObject(int outputs, Point position) : base(new Switch(outputs), InputController.DefaultTexture, position)
        {

        }


        public override void Focus()
        {
            base.Focus();
            InputController.MouseManager.RightButtonDown += MouseManager_RightButtonDown;
        }

        public override void Defcous()
        {
            base.Defcous();
            InputController.MouseManager.RightButtonDown -= MouseManager_RightButtonDown;
        }

        private void MouseManager_RightButtonDown(object sender, MouseEventArgs e)
        {
            if (_area.Contains(e.MouseState.Position))
            {
                Switch temp = gate as Switch;
                temp.State = !temp.State;
                Gate.Update(this, () => { });
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            onArea = _area;
            onArea.Inflate(-2, -2);

            if (Outputs[0])
            {
                spriteBatch.Draw(gateTexture, onArea, Color.Green);
            }
            else
            {
                spriteBatch.Draw(gateTexture, onArea, Color.Red);
            }
        }
    }

    class Switch : AbstractGate
    {
        public bool State { get; set; }

        public Switch(int outputs)
        {
            Inputs = new bool[0];
            InputConnections = new Connection[0];

            Outputs = new bool[outputs];
            OutputConnections = new Connection[outputs];
            InitializeConnections();
            Update();
        }

        public override void Update()
        {
            bool[] previousState = Outputs;

            for (int i = 0; i < Outputs.Length; i++)
            {
                Outputs[i] = State;
            }

            OutputChanged = Enumerable.SequenceEqual(previousState, Outputs);

        }
    }
}
