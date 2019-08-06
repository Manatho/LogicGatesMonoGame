using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LogicGates.DefaultGates
{
    public abstract class AbstractGate : IGate
    {
        public bool[] Inputs { get; protected set; }
        public bool[] Outputs { get; protected set; }

        public Connection[] InputConnections { get; protected set; }
        public Connection[] OutputConnections { get; protected set; }

        public bool OutputChanged { get; protected set; }

        protected void InitializeConnections()
        {
            for (int i = 0; i < InputConnections.Length; i++)
                InputConnections[i] = Connection.EmptyConnection;

            for (int i = 0; i < OutputConnections.Length; i++)
                OutputConnections[i] = Connection.EmptyConnection;
        }

        public bool GetInput(int index)
        {
            return Inputs[index];
        }

        public bool GetOutput(int index)
        {
            return Outputs[index];
        }

        public void SetInputConnection(Connection connection)
        {
            InputConnections[connection.InputPin] = connection;
        }

        public void SetInputConnection(IGate outputee, int output, int input)
        {
            SetInputConnection(new Connection { Outputee = outputee, OutputPin = output, Inputee = this, InputPin = input });
        }

        public void SetOutputConnection(Connection connection)
        {
            OutputConnections[connection.OutputPin] = connection;
        }

        public void SetOutputConnection(IGate outputee, int output, int input)
        {
            SetOutputConnection(new Connection { Outputee = outputee, OutputPin = output, Inputee = this, InputPin = input });
        }

        public abstract void Update();

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(GetType().Name);
            builder.Append(" -> Input: [");
            builder.Append(string.Join(",", Inputs));
            builder.Append("] Output: [");
            builder.Append(string.Join(",", Outputs));
            builder.Append("]");
            return builder.ToString();
        }




    }
}
