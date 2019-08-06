using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicGates
{
    public static class Gate
    {
        public static void Connect(IGate output, int outputPin, IGate input, int inputPin)
        {
            Connection connection = new Connection(output, outputPin, input, inputPin);
            output.SetOutputConnection(connection);
            input.SetInputConnection(connection);
        }

        public static void ConnectToInput(this IGate output, int outputPin, IGate input, int inputPin)
        {
            Connection connection = new Connection(output, outputPin, input, inputPin);
            output.SetOutputConnection(connection);
            input.SetInputConnection(connection);
        }

        public static void Update(IGate gate, Action updated)
        {
            //Initialize
            int maxRepeat = 19;
            Dictionary<IGate, int> repeatMap = new Dictionary<IGate, int>();
            UniqueQueue<IGate> newGates = new UniqueQueue<IGate>();

            gate.Update();
            updated();
            repeatMap.Add(gate, 1);
            newGates.EnqueueAll(gate.OutputConnections
                .Where(connection => !connection.Connected)
                .Select(connection => connection.Inputee));

            //Loop through gates
            while (newGates.Count > 0)
            {
                gate = newGates.Dequeue();

                //Prevent looping forever
                if (!repeatMap.ContainsKey(gate))
                    repeatMap.Add(gate, 0);
                repeatMap[gate]++;

                if (repeatMap[gate] > maxRepeat)
                    break;

                gate.Update();
                updated();
                var candidates = gate.OutputConnections
                    .Where(connection => !connection.Connected)
                    .Select(connection => connection.Inputee);

                newGates.EnqueueAll(candidates);
            }

        }

        public static void EnqueueAll<T>(this UniqueQueue<T> queue, IEnumerable<T> enu)
        {
            foreach (T obj in enu)
                queue.Enqueue(obj);
        }
    }

}
