using GameEngine.DrawingUtil;
using LogicGates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicGateFront.GameCore
{
    public class ConnectionObject : Connection
    {
        public static int Name = 0;
        private int id = 0;

        public ConnectionObject(GateObject outputee, int outputPin, GateObject inputee, int inputPin)
            : base(outputee, outputPin, inputee, inputPin)
        {
            Name++;
            id = Name;
        }

        public void Connect()
        {
            Connection existingOutput = Outputee.OutputConnections[OutputPin];
            existingOutput.Inputee = null;
            existingOutput.Outputee = null;

            Connection existingInput = Inputee.InputConnections[InputPin];
            existingInput.Inputee = null;
            existingInput.Outputee = null;


            Outputee.SetOutputConnection(this);
            Inputee.SetInputConnection(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GateObject @in = Inputee as GateObject;
            GateObject @out = Outputee as GateObject;

            if (!Connected)
                spriteBatch.Line(@in.GetInputPin(InputPin), @out.GetOutputPin(OutputPin), 2, Color.Green);
        }
    }
}
