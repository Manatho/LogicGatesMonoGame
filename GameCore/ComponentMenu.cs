using GameEngine.UI;
using GameEngine.UI.Control;
using GameEngine.UI.Control.Event;
using GameEngine.UI.Control.Texture;
using LogicGateFront;
using LogicGates;
using LogicGates.DefaultGates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogicGateFront
{
    internal class ComponentMenu : InputContainer
    {
        private static List<Type> gateTypes = new List<Type>() { typeof(AndGate), typeof(OrGate), typeof(NotGate), typeof(XorGate), typeof(Switch) };
        private static Dictionary<string, Type> nameToType = init();

        private static Dictionary<string, Type> init()
        {
            var temp = new Dictionary<string, Type>();
            foreach (Type type in gateTypes)
                temp.Add(type.FullName, type);
            return temp;
        }

        public bool Selected { get { return group.SelectedIndex != -1; } set { if (value) Disable(); }  }
        public Rectangle Bounds => new Rectangle(X, Y, Width, Height);

        private readonly int X = 700;
        private readonly int Width = 100;
        private readonly int Height = 600;
        private readonly int Y = 0;

        private readonly int Padding = 5;


        private int CurrentY;
        Dictionary<string, InputContainer> Containers = new Dictionary<string, InputContainer>();
        Dictionary<InputContainer, List<Iinput>> Inputs = new Dictionary<InputContainer, List<Iinput>>();
        ToggleButtons group = new ToggleButtons();

        public ComponentMenu (GameMenu gameMenu)
        {
            Add(new TextureField(new Rectangle(X - 3, Y, 3, Height), new Color(50, 50, 50)));
            Add(new TextureField(new Rectangle(X, Y, Width, Height), new Color(220, 220, 220)));

            CreateGateButtons(this, gameMenu);
            CurrentY += Padding;
            Add(new TextureField(new Rectangle(X, CurrentY, Width, 2), new Color(50, 50, 50)));
            CurrentY += Padding;
            CreateGateSettings(this);
        }

        private readonly int BPadding = 5;
        private readonly int ButtonPrRow = 3;
        private void CreateGateButtons(InputContainer componentMenu, GameMenu gameMenu)
        {
            
            List<TextureField> icons = new List<TextureField>();

            ButtonTexture bt = new ButtonTexture(
                InputController.DefaultTexture,
                    new Color(50, 50, 50),
                    new Color(100, 100, 100),
                    new Color(75, 75, 75),
                    new Color(95, 115, 255)
                );
            int buttonSize = (Width - (ButtonPrRow + 1) * BPadding) / ButtonPrRow;
            int offsetY = 0;
            for (int i = 0; i < gateTypes.Count; i++)
            {
                Type gateType = gateTypes[i];
                int offsetX = (i % ButtonPrRow);
                offsetX = offsetX * buttonSize + offsetX * BPadding;
                offsetY = (i / ButtonPrRow);
                offsetY = offsetY * buttonSize + offsetY * BPadding;

                Rectangle bounds = new Rectangle(X + BPadding + offsetX, Y + BPadding + offsetY, buttonSize, buttonSize);
                Rectangle iconBounds = bounds;
                iconBounds.Inflate(-4, -4);

                Texture2D texture;
                if (gameMenu.GateTextureMap.ContainsKey(gateType.FullName))
                    texture = gameMenu.GateTextureMap[gateType.FullName];
                else
                    texture = InputController.DefaultTexture;

                TextureField icon = new TextureField(iconBounds, texture , Color.White);
                Button temp = new Button(bounds, bt, new Text("", Color.White));
                temp.ID = gateType.FullName;

                icons.Add(icon);
                group.Add(temp, false);
            }

            

            group.BeforeSelectedButtonChange += (object sender, ButtonEventArgs e) =>
            {
                if(group.SelectedButton != null)
                {
                    Containers[group.SelectedButton.ID].Active = false;
                    Containers[group.SelectedButton.ID].DeFocus();
                }

                if (!Selected)
                {
                    OnActive?.Invoke();
                }
            };

            group.AfterSelectedButtonChange += (object sender, ButtonEventArgs e) =>
            {

                Containers[e.Button.ID].Active = true;
                Containers[e.Button.ID].Focus();
            };

            componentMenu.Add(group);
            componentMenu.Add(icons);
            CurrentY = offsetY + buttonSize + BPadding;
        }



        private void CreateGateSettings(InputContainer componentMenu)
        {
            foreach (Type t in gateTypes)
            {
                ParameterInfo[] paras = { };
                foreach (var cs in t.GetConstructors())
                    if (paras.Length < cs.GetParameters().Length)
                        paras = cs.GetParameters();

                InputContainer temp = new InputContainer();
                List<Iinput> inputs = new List<Iinput>();
                int oldY = CurrentY;
                foreach (var para in paras)
                {
                    if (typeof(int).IsAssignableFrom(para.ParameterType))
                    {
                        Point start = new Point(X + Padding, CurrentY);
                        temp.Add(new Label(new Text(para.Name, Color.Black), start));

                        int textLength = (int)InputController.DefaultFont.MeasureString(para.Name).X;
                        int textHeight = (int)InputController.DefaultFont.MeasureString(para.Name).Y;
                        int x = start.X + Padding + textLength;
                        int width = X + Width - x - Padding;
                        var input = new NumberSelector(new Rectangle(x, start.Y, width, textHeight), 1, 99, textHeight, 10);
                        temp.Add(input);
                        inputs.Add(input);

                        CurrentY += textHeight + Padding;

                        temp.Active = false;
                        temp.DeFocus();
                    }
                }
                CurrentY = oldY;
                Containers[t.FullName] = temp;
                Inputs.Add(temp, inputs);
            }
            componentMenu.Add(Containers.Values);
        }

        public IGate NewGate()
        {
            IGate g = null;

            if (!Selected)
                return g;

            Type t = nameToType[group.SelectedButton.ID];
            List<Iinput> inputs = Inputs[Containers[group.SelectedButton.ID]];

            int length = 0;
            ConstructorInfo ctor = null;
            foreach (var cs in t.GetConstructors())
                if (length <= cs.GetParameters().Length)
                {
                    length = cs.GetParameters().Length;
                    ctor = cs;
                }

            object[] param = new object[inputs.Count];

            for (int i = 0; i < inputs.Count; i++)
                param[i] = GetInputValue(inputs[i]);

            g = ctor.Invoke(param) as IGate;

            return g;
        }

        private object GetInputValue(Iinput iinput)
        {
            if(iinput is NumberSelector)
            {
                return (iinput as NumberSelector).Value;
            }
            return null;
        }

        public override void Focus()
        {
            base.Focus();

            InputController.KeyboardManager.KeyUp += KeyboardManager_KeyUp;
        }

        public override void DeFocus()
        {
            base.DeFocus();
            InputController.KeyboardManager.KeyUp -= KeyboardManager_KeyUp;
        }

        private void KeyboardManager_KeyUp(object sender, KeyboardEventArgs e)
        {
            if(e.Key == Keys.Escape && group.SelectedButton != null)
            {
                Disable();
            }
        }


        private void Disable()
        {
            Containers[group.SelectedButton.ID].Active = false;
            Containers[group.SelectedButton.ID].DeFocus();
            group.UnlockSelected();
            OnDisable?.Invoke();
        }

        public event Action OnDisable;
        public event Action OnActive;

    }


}