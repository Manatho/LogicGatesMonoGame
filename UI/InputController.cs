using GameEngine.UI.Control.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameEngine.Logic;
using GameEngine.UI.Control;

namespace GameEngine.UI
{
    public class InputController
    {
        public static Rectangle ScreenSize;

        public static SpriteFont DefaultFont;
        public static Texture2D DefaultTexture;

        public static MouseManager MouseManager;
        public static KeyboardManager KeyboardManager;

        public static GraphicsDevice GraphicsDevice; //Probably need to be moved

        public static GameTime UiTime;
        public static double CurrentUiTime { get { return UiTime.TotalGameTime.TotalMilliseconds; } }

        public static bool IsInitialized {get; private set;}


		public static void Initialize(GraphicsDevice graphicsDevice, ContentManager content)
		{
			GraphicsDevice = graphicsDevice;

            IsInitialized = true;

            UiTime = new GameTime();

			DefaultTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
			DefaultTexture.SetData(new[] { Color.White });

			ScreenSize = graphicsDevice.PresentationParameters.Bounds;

			DefaultFont = content.Load<SpriteFont>("Fonts/DefaultFont");

			MouseManager = new MouseManager();
			KeyboardManager = new KeyboardManager();


			Button.StandardTexture = new ButtonTexture(DefaultTexture, new Color(40, 40, 45), new Color(50, 50, 55), new Color(100, 100, 110), new Color(130, 130, 130));
			Scroller.StandardTexture = new ScrollerTexture(Button.StandardTexture, new ButtonTexture(DefaultTexture, new Color(37,63,150), new Color(49, 83, 196), Color.DodgerBlue, Color.LightGray), DefaultTexture);
			ComboBox.StandardTexture = new ComboBoxTexture(DefaultTexture, Color.DarkGray, Color.Red, Color.LightGray, Color.White, Scroller.StandardTexture);

		}

		public static void Reset()
		{
			KeyboardManager.UnsubscribeAll(); //Remember to initialize the InputController
			MouseManager.UnsubscribeAll();
		}

		public static void Update(GameTime time)
		{
			UiTime = time;
			MouseManager.Update();
			KeyboardManager.Update();
		}

	}
}
