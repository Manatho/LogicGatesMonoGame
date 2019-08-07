using LogicGates.DefaultGates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicGates.DefaultGates
{
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
