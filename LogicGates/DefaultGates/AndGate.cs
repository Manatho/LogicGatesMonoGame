using System.Collections.Generic;

namespace LogicGates.DefaultGates
{
    public class AndGate : AbstractGate
    {
        public AndGate(int inputs, int outputs)
        {
            Inputs = new bool[inputs];
            InputConnections = new Connection[inputs];

            Outputs = new bool[1];
            OutputConnections = new Connection[1];
            InitializeConnections();
            Update();
        }
    
        public override void Update()
        {
            bool previousState = Outputs[0];
            Outputs[0] = true;
            for (int i = 0; i < InputConnections.Length; i++)
            {
                if (!InputConnections[i].Connected)
                {
                    bool pinState = InputConnections[i].OutPutState;
                    Inputs[i] = pinState;

                    if (!pinState)
                    {
                        Outputs[0] = false;
                    }
                }
                else
                {
                    Outputs[0] = false;
                }
            }
            OutputChanged = previousState != Outputs[0];
        }
    }
}