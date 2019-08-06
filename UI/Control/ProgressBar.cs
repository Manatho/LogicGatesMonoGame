using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.DrawingUtil;

namespace GameEngine.UI.Control
{
    public class Progressbar : Iinput
    {
        public TextureField Background;
        public Rectangle Bounds { get { return Background.Bounds; } }

        public float Progress { get { return progress; }
            set
            {
                if (value > 1) progress = 1;
                else if (value < 0) progress = 0;
                else progress = value;
                UpdateProgress();
            }
        }
        private float progress;
        private Rectangle progressRectangle;

        public Color BackgroundColor;
        public Color ForeGroundColor;

        public Progressbar(Rectangle bounds, Color background, Color foreground)
        {
            Background = new TextureField(bounds, background);
            ForeGroundColor = foreground;

            Progress = 0;
        }

        private void UpdateProgress()
        {
            progressRectangle = new Rectangle(Background.Bounds.Location, new Point((int)(Background.Bounds.Width * progress), Background.Bounds.Height));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            spriteBatch.Rectangle(progressRectangle, ForeGroundColor);
        }



        public void Move(Point amount)
        {
            Background.Move(amount);
        }

        public void Update()
        {
            
        }

        public void Focus()
        {
        }

        public void DeFocus()
        {
        }
    }
}
