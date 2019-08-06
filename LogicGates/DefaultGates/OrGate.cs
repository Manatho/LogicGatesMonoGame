using LogicGates;
using LogicGates.DefaultGates;

namespace LogicGates.DefaultGates
{
    class OrGate : AbstractGate
    {
        public OrGate(int inputs)
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
            Outputs[0] = false;
            for (int i = 0; i < InputConnections.Length; i++)
            {
                if (!InputConnections[i].Connected)
                {
                    bool pinState = InputConnections[i].OutPutState;
                    Inputs[i] = pinState;

                    if (pinState)
                        Outputs[0] = true;
                }
            }
            OutputChanged = previousState != Outputs[0];
        }
    }
}
