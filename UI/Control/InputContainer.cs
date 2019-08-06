using GameEngine.UI.Control.Event;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.UI.Control
{
    public class InputContainer : Iinput
    {
        //Inputcontainer
        public List<Iinput> input { get; private set; }
        public List<MessagesBox> Messages { get; private set; }

        public Point origin { get; private set; }

        public bool Active { get; set; }

        public bool IsMessageActive { get; private set; }
        private MessagesBox ActiveMessage;

        //------------------------------------------------------------
        //------------------------Constructors------------------------
        //------------------------------------------------------------

        public InputContainer()
        {
            origin = Point.Zero;
            input = new List<Iinput>();
            IsMessageActive = false;
            Active = true;
            Focus();
        }

        public InputContainer(Point offset)
        {
            origin = offset;
            input = new List<Iinput>();
            IsMessageActive = false;
            Active = true;
            Focus();
        }

        //------------------------------------------------------
        //------------------------Methods-----------------------
        //------------------------------------------------------

        public void MoveTo(Point position)
        {
            Point temp = position - origin;
            Move(temp);
        }

        public void Move(Point amountToMove)
        {
            origin += amountToMove;

            foreach (Iinput i in input)
                i.Move(amountToMove);
        }

        public void Add(Iinput input)
        {
            Add(input, false, true);
        }

        public void Add(Iinput input, bool ignoreOffset, bool startFocused)
        {
            if (!ignoreOffset)
                input.Move(origin);

            this.input.Add(input);


            if (startFocused)
                input.Focus();
            else
                input.DeFocus();

        }

        public void Add(params Iinput[] inputs)
        {
            Add(inputs);
        }

        public void Add(IEnumerable<Iinput> inputs)
        {
            foreach (Iinput i in inputs)
                Add(i);
        }

        public void Add(bool startFocused, params Iinput[] inputs)
        {
            foreach (Iinput i in inputs)
                Add(i, false, startFocused);

        }


        public void Add(Iinput input, bool ignoreOffset)
        {
            Add(input, ignoreOffset, true);
        }

        public void Add(MessagesBox message)
        {
            message.Activated += OnMessage;
            message.Deactivated += OnMessage;

            if (message.Active)
            {
                ActiveMessage = message;
                IsMessageActive = true;
                DeFocus();
            }

        }

        public void Remove(Iinput Input)
        {
            input.Remove(Input);
        }
        public void Remove(MessagesBox Messages)
        {
            Messages.Activated -= OnMessage;
        }


        //------------------------------------------------------
        //------------------------Update------------------------
        //------------------------------------------------------

        public void Update()
        {
            if (Active)
            {
                if (!IsMessageActive)
                    for (int i = 0; i < input.Count; i++)
                        input[i].Update();
                else ActiveMessage.Update();
            }


        }

        //-------------------------------------------------------
        //------------------------Drawing------------------------
        //-------------------------------------------------------

        public void Draw(SpriteBatch SpriteBatch)
        {
            if (Active)
            {
                for (int i = 0; i < input.Count; i++)
                    input[i].Draw(SpriteBatch);

                if (IsMessageActive)
                    ActiveMessage.Draw(SpriteBatch);
            }
        }

        //------------------------------------------------------
        //------------------------EVENTS------------------------
        //------------------------------------------------------

        private void OnMessage(object sender, MessagesEventArgs eventArgs)
        {
            ActiveMessage = eventArgs.MessageBox;
            IsMessageActive = ActiveMessage.Active;
            State(!ActiveMessage.Active);
            OnMessageActivated();
        }

        private void State(bool state)
        {
            if (state)
                Focus();
            else
                DeFocus();
        }

        public virtual void Focus()
        {
            foreach (Iinput i in input)
                i.Focus();
        }

        public virtual void DeFocus()
        {
            foreach (Iinput i in input)
                i.DeFocus();
        }


        public event EventHandler MessageActivated;
        private void OnMessageActivated()
        {
            if (MessageActivated != null)
                MessageActivated(this, EventArgs.Empty);
        }
    }
}
