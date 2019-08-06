using System;
using System.Collections.Generic;
using System.Text;

namespace LogicGates
{
    public class Connection
    {
        public static Connection EmptyConnection => new Connection();
        public bool Connected => Inputee == null || Outputee == null;

        public IGate Inputee;
        public int InputPin;

        public IGate Outputee;
        public int OutputPin;
        public bool OutPutState => Outputee.Outputs[OutputPin];

        public Connection() { }

        public Connection(IGate outputee, int outputPin, IGate inputee, int inputPin)
        {
            Outputee = outputee;
            Inputee = inputee;
            OutputPin = outputPin;
            InputPin = inputPin;
        }

        public void Disconnect()
        {
            Outputee = null;
            Inputee = null;
        }
    }


}
