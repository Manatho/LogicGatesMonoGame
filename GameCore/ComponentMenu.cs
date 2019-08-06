using GameEngine.UI;
using GameEngine.UI.Control;
using GameEngine.UI.Control.Event;
using GameEngine.UI.Control.Texture;
using LogicGateFront;
using LogicGates.DefaultGates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogicGateFront
{
    internal class ComponentMenu
    {
        private static List<Type> gateTypes = new List<Type>() { typeof(AndGate), typeof(OrGate), typeof(NotGate), typeof(XorGate) };

        private static readonly int X = 700;
        private static readonly int Width = 100;
        private static readonly int Height = 600;
        private static readonly int Y = 0;

        private static readonly int Padding = 5;

        private static TemporaryState state;

        public static InputContainer Create(GameMenu gameMenu)
        {
            state = new TemporaryState();

            InputContainer componentMenu = new InputContainer();
            componentMenu.Add(new TextureField(new Rectangle(X - 3, Y, 3, Height), new Color(50, 50, 50)));
            componentMenu.Add(new TextureField(new Rectangle(X, Y, Width, Height), new Color(220, 220, 220)));

            CreateGateButtons(componentMenu, gameMenu);
            state.CurrentY += Padding;
            componentMenu.Add(new TextureField(new Rectangle(X, state.CurrentY, Width, 2), new Color(50, 50, 50)));
            state.CurrentY += Padding;
            CreateGateSettings(componentMenu);


            return componentMenu;
        }

        private static readonly int BPadding = 5;
        private static readonly int ButtonPrRow = 3;
        private static void CreateGateButtons(InputContainer componentMenu, GameMenu gameMenu)
        {
            ToggleButtons group = new ToggleButtons();
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

                TextureField icon = new TextureField(iconBounds, gameMenu.GateTextureMap[gateType.Name.Replace("Gate", "")], Color.White);
                Button temp = new Button(bounds, bt, new Text("", Color.White));
                temp.ID = gateType.FullName;

                icons.Add(icon);
                group.Add(temp, false);
            }

            

            group.BeforeSelectedButtonChange += (object sender, ButtonEventArgs e) =>
            {
                if(group.SelectedButton != null)
                {
                    state.Containers[group.SelectedButton.ID].Active = false;
                    state.Containers[group.SelectedButton.ID].DeFocus();
                }
            };

            group.AfterSelectedButtonChange += (object sender, ButtonEventArgs e) =>
            {
                state.Containers[e.Button.ID].Active = true;
                state.Containers[e.Button.ID].Focus();
            };

            componentMenu.Add(group);
            componentMenu.Add(icons);
            state.CurrentY = offsetY + buttonSize + BPadding;
        }



        private static void CreateGateSettings(InputContainer componentMenu)
        {
            foreach (Type t in gateTypes)
            {
                ParameterInfo[] paras = { };
                foreach (var cs in t.GetConstructors())
                    if (paras.Length < cs.GetParameters().Length)
                        paras = cs.GetParameters();

                InputContainer temp = new InputContainer();
                int oldY = state.CurrentY;
                foreach (var para in paras)
                {
                    if (typeof(int).IsAssignableFrom(para.ParameterType))
                    {
                        Point start = new Point(X + Padding, state.CurrentY);
                        temp.Add(new Label(new Text(para.Name, Color.Black), start));

                        int textLength = (int)InputController.DefaultFont.MeasureString(para.Name).X;
                        int textHeight = (int)InputController.DefaultFont.MeasureString(para.Name).Y;
                        int x = start.X + Padding + textLength;
                        int width = X + Width - x - Padding;
                        temp.Add(new NumberSelector(new Rectangle(x, start.Y, width, textHeight), 1, 99, textHeight, 10));

                        state.CurrentY += textHeight + Padding;

                        temp.Active = false;
                        temp.DeFocus();
                    }
                }
                state.CurrentY = oldY;
                state.Containers[t.FullName] = temp;
            }
            componentMenu.Add(state.Containers.Values);
        }

        private class TemporaryState
        {
            public int CurrentY = 0;
            public Dictionary<string, InputContainer> Containers = new Dictionary<string, InputContainer>();
        }
    }


}