using GameEngine.DrawingUtil;
using GameEngine.UI;
using LogicGateFront.GameCore;
using LogicGates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LogicGateFront
{
    public class GateMouseAndKeyboardControl
    {
        private GameMenu gameMenu;
        private GateObject selectedGate;

        private Point cursorOffset;
        private bool dragGate;

        private ConnectionObject currentConnection;
        private Vector2 connectionStart;


        public GateMouseAndKeyboardControl(GameMenu gameMenu)
        {
            this.gameMenu = gameMenu;
        }

        internal void Initialize()
        {
            InputController.MouseManager.MouseMoved += MouseManager_MouseMoved;
            InputController.MouseManager.LeftButtonUp += MouseManager_LeftButtonUp;

            InputController.KeyboardManager.KeyUp += KeyboardManager_KeyUp;
            gameMenu.Gates.ForEach(g => g.OnLeftClickDown = SelectGate);
        }

        public void AddClick(GateObject gate)
        {
            gate.OnLeftClickDown = SelectGate;
        }

        private void MouseManager_LeftButtonUp(object sender, MouseEventArgs e)
        {
            dragGate = false;
            if (selectedGate != null && selectedGate.BodySelected)
            {
                selectedGate.Selected = true;
            }
            else
            {
                if (selectedGate != null)
                    selectedGate.Selected = false;
                selectedGate = null;
            }
        }

        private void MouseManager_MouseMoved(object sender, MouseEventArgs e)
        {
            if (selectedGate != null && dragGate)
            {
                selectedGate.Position = e.MouseState.Position + cursorOffset;
            }
        }

        private void KeyboardManager_KeyUp(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.R)
            {
                if (selectedGate != null)
                {
                    selectedGate.Rotate90();
                }           
            }
            if (e.Key == Keys.Delete)
            {
                if (selectedGate != null)
                {
                    foreach (Connection c in selectedGate.InputConnections)
                        c.Disconnect();

                    foreach (Connection c in selectedGate.OutputConnections)
                        c.Disconnect();

                    gameMenu.Gates.Remove(selectedGate);
                }
            }
        }

        private void SelectGate(GateObject g)
        {
            if (g.BodySelected)
            {
                if (selectedGate != null)
                    selectedGate.Selected = false;
                selectedGate = g;
                dragGate = true;
                cursorOffset = g.Position - InputController.MouseManager.Position;
                currentConnection = null;
            }
            else if (g.SelectedPin.IsSet)
            {
                if (currentConnection == null)
                {
                    if (g.SelectedPin.IsInput)
                        currentConnection = new ConnectionObject(null, 0, g, g.SelectedPin.PinIndex);
                    else
                        currentConnection = new ConnectionObject(g, g.SelectedPin.PinIndex, null, 0);

                    connectionStart = g.SelectedPin.PinPosition;
                }
                else
                {
                    if (g.SelectedPin.IsInput)
                    {
                        currentConnection.Inputee = g;
                        currentConnection.InputPin = g.SelectedPin.PinIndex;
                        currentConnection.Connect();
                    }
                    else
                    {
                        currentConnection.Outputee = g;
                        currentConnection.OutputPin = g.SelectedPin.PinIndex;
                        currentConnection.Connect();
                    }
                    Gate.Update(currentConnection.Inputee, () => { });
                    gameMenu.Connections.Add(currentConnection);
                    gameMenu.Connections.RemoveAll(c => c.Connected);
                    currentConnection = null;
                }
            }
            else
            {
                currentConnection = null;
            }

        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (currentConnection != null)
            {
                spriteBatch.Line(connectionStart, InputController.MouseManager.Position.ToVector2(), 2, Color.Red);
            }
        }
    }
}