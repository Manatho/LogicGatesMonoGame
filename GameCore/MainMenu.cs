using GameEngine.DrawingUtil;
using GameEngine.MenuSystem;
using GameEngine.UI;
using GameEngine.UI.Control;
using LogicGateFront.GameCore;
using LogicGates;
using LogicGates.DefaultGates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LogicGateFront
{
    public class GameMenu : Menu
    {
        public List<GateObject> Gates = new List<GateObject>();
        public List<ConnectionObject> Connections = new List<ConnectionObject>();

        GateMouseAndKeyboardControl gateControl;
        ComponentMenu componentMenu;

        public Texture2D AndGateTexture;
        public Texture2D OrGateTexture;
        public Texture2D XorGateTexture;
        public Texture2D NotGateTexture;
        public Dictionary<string, Texture2D> GateTextureMap;

    public SpriteFont SmallFont;


        public GameMenu(ContentManager content) : base(content, "GameMenu")
        {
            gateControl = new GateMouseAndKeyboardControl(this);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            AndGateTexture = content.Load<Texture2D>("Gates/AND");
            OrGateTexture = content.Load<Texture2D>("Gates/OR");
            XorGateTexture = content.Load<Texture2D>("Gates/XOR");
            NotGateTexture = content.Load<Texture2D>("Gates/LED");

            SmallFont = content.Load<SpriteFont>("Fonts/small");

            GateTextureMap = new Dictionary<string, Texture2D>()
            {
                {typeof(AndGate).FullName, AndGateTexture },
                {typeof(OrGate).FullName, OrGateTexture },
                {typeof(XorGate).FullName, XorGateTexture },
                {typeof(NotGate).FullName, NotGateTexture }
            };

        }

        public override void Initialize()
        {
            base.Initialize();

            componentMenu = new ComponentMenu(this);
            componentMenu.OnActive += Cm_OnActive;
            componentMenu.OnDisable += Cm_OnDisable;
            inputContainer.Add(componentMenu);

            /*Gates.Add(new GateObject(new XorGate(), XorGateTexture, new Point(50, 100)));
            Gates.Add(new GateObject(new OrGate(2), OrGateTexture, new Point(150, 100)));
            Gates.Add(new GateObject(new NotGate(1), NotGateTexture, new Point(250, 100)));
            Gates.Add(new GateObject(new NotGate(1), NotGateTexture, new Point(250, 100)));
            Gates.Add(new GateObject(new NotGate(1), NotGateTexture, new Point(250, 100)));
            Gates.Add(new GateObject(new NotGate(1), NotGateTexture, new Point(250, 100)));

            Gates.Add(new SwitchObject(1, new Point(150, 70)));
            Gates.Add(new SwitchObject(1, new Point(150, 70)));
            Gates.Add(new SwitchObject(1, new Point(150, 50)));
            Gates.Add(new SwitchObject(1, new Point(150, 50)));
            Gates.Add(new GateObject(new AndGate(2, 1), AndGateTexture, new Point(200, 50)));*/
            //gates.Add(new GateObject(new AndGate(4), InputController.DefaultTexture, new Point(300, 50)));
            //gates.Add(new GateObject(new AndGate(4), InputController.DefaultTexture, new Point(400, 50)));

            gateControl.Initialize();

            InputController.MouseManager.LeftButtonDown += MouseManager_LeftButtonDown;

            thread = new Thread(new ThreadStart(Test));
        }

        private void MouseManager_LeftButtonDown(object sender, MouseEventArgs e)
        {
            if (componentMenu.Selected && !componentMenu.Bounds.Contains(e.MouseState.Position))
            {
                IGate gate = componentMenu.NewGate();
                GateObject go;
                if (gate is Switch)
                    go = new SwitchObject(gate.Outputs.Length, e.MouseState.Position);
                else
                    go = new GateObject(gate, GateTextureMap[gate.GetType().FullName], e.MouseState.Position);

                go.Defcous();
                Gates.Add(go);
                gateControl.AddClick(go);
                Console.WriteLine(gate);
            }
        }

        private void Cm_OnDisable()
        {
            foreach (GateObject gateObject in Gates)
                gateObject.Focus();
        }

        private void Cm_OnActive()
        {
            foreach (GateObject gateObject in Gates)
                gateObject.Defcous();
        }

        Thread thread;

        private void Test()
        {
            Gate.Update(Gates[0], Nothing);
            thread = new Thread(new ThreadStart(Test));
        }

        public override void Update()
        {
            base.Update();
        }

        public void Nothing()
        {
            Thread.Sleep(200);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (GateObject gateObject in Gates)
                gateObject.Draw(spriteBatch);

            foreach (ConnectionObject connection in Connections)
                connection.Draw(spriteBatch);

            gateControl.Draw(spriteBatch);
        }

    }
}