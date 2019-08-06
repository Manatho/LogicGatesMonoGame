using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicGates.DefaultGates
{
    public class NotGate : AbstractGate
    {
        public NotGate(int outputs): base()
        {
            Inputs = new bool[1];
            InputConnections = new Connection[1];

            Outputs = new bool[outputs];
            OutputConnections = new Connection[outputs];
            InitializeConnections();
            Update();
        }

        public override void Update()
        {
            bool[] previousState = Outputs;

            if (!InputConnections[0].Connected)
            {
                Inputs[0] = InputConnections[0].OutPutState;

                for (int i = 0; i < Outputs.Length; i++)
                    Outputs[i] = !Inputs[0];
                
            }
            else
                for (int i = 0; i < Outputs.Length; i++)
                    Outputs[i] = !Inputs[0];

            OutputChanged = Enumerable.SequenceEqual(previousState, Outputs);
        }
    }
}
