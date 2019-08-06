using LogicGates;
using LogicGates.DefaultGates;

namespace LogicGates.DefaultGates
{
    class XorGate : AbstractGate
    {
        public XorGate()
        {
            Inputs = new bool[2];
            InputConnections = new Connection[2];

            Outputs = new bool[1];
            OutputConnections = new Connection[1];
            InitializeConnections();
            Update();
        }

        public override void Update()
        {
            bool previousState = Outputs[0];
            Outputs[0] = true;
            int ons = 0;
            bool pinState;
            for (int i = 0; i < InputConnections.Length; i++)
            {
                if (!InputConnections[i].Connected)
                {
                    pinState = InputConnections[i].OutPutState;
                    if (pinState)
                        ons++;
                }
                else
                    pinState = false;
                Inputs[i] = pinState;
            }

            Outputs[0] = ons == 1;
            OutputChanged = previousState != Outputs[0];
        }
    }
}
