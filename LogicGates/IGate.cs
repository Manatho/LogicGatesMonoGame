using System.Collections.Generic;

namespace LogicGates
{
    public interface IGate
    {
        bool[] Inputs { get; }
        bool GetInput(int index);
        void SetInputConnection(Connection connection);
        void SetInputConnection(IGate outputee, int output, int input);

        bool[] Outputs { get; }
        bool GetOutput(int index);
        void SetOutputConnection(Connection connection);
        void SetOutputConnection(IGate outputee, int output, int input);

        void Update();

        Connection[] InputConnections { get; }
        Connection[] OutputConnections { get; }

        bool OutputChanged { get; }
    }
}