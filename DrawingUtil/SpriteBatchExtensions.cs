using GameEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameEngine.DrawingUtil
{
    public static class SpriteBatchExtensions
	{
		public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, TexturePlacement texturePlacement, Color color)
		{
			spriteBatch.Draw
			(
				texture: texture,
				position: texturePlacement.Position,
				sourceRectangle: texturePlacement.SourceRectangle,
				color: color,
				scale: texturePlacement.Scale
			);
		}

    }

    [Flags]
    public enum Alignment { Center = 0, Left = 1, Right = 2, Top = 4, Bottom = 8 }
    public static class SpriteBatchStringExtentions
    {


        public static void OutlineText(this SpriteBatch spriteBatch, SpriteFont font, string Text, float x, float y, Color textColor, Color outlining)
        {
            OutlineText(spriteBatch, font, Text, new Vector2(x, y), textColor, outlining);
        }
        public static void OutlineText(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color textColor, Color outlining)
        {
           spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y + 1), outlining);
           spriteBatch.DrawString(font, text, new Vector2(position.X - 1, position.Y), outlining);
           spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y - 1), outlining);
           spriteBatch.DrawString(font, text, new Vector2(position.X + 1, position.Y), outlining);
           spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y), textColor);
        }

        public static void DrawString(SpriteBatch spriteBatch, SpriteFont font, string text, Rectangle bounds, Alignment align, Color color)
        {
            Vector2 size = font.MeasureString(text);
            Vector2 pos = new Vector2(bounds.Center.X, bounds.Center.Y);
            Vector2 origin = size * 0.5f;

            if (align.HasFlag(Alignment.Left))
                origin.X += bounds.Width / 2 - size.X / 2;

            if (align.HasFlag(Alignment.Right))
                origin.X -= bounds.Width / 2 - size.X / 2;

            if (align.HasFlag(Alignment.Top))
                origin.Y += bounds.Height / 2 - size.Y / 2;

            if (align.HasFlag(Alignment.Bottom))
                origin.Y -= bounds.Height / 2 - size.Y / 2;

            spriteBatch.DrawString(font, text, new Vector2((int)pos.X, (int)pos.Y), color, 0, new Vector2((int)origin.X, (int)origin.Y), 1, SpriteEffects.None, 0);
        }

        public static void DrawString(this SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(InputController.DefaultFont, text, position, color);
        }
    }

    public static class SpriteBatchGeometricExtensions
    {

        public static void Line(this SpriteBatch spriteBatch, Point start, Point end, Color color)
        {
            Line(spriteBatch, InputController.DefaultTexture, color, start, end);
        }

        public static void Line(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, int lineWidth, Color color)
        {
            Line(spriteBatch, InputController.DefaultTexture, lineWidth, color, start, end);
        }

        public static void Line(this SpriteBatch spriteBatch, Point start, Point end, int lineWidth, Color color)
        {
            Line(spriteBatch, InputController.DefaultTexture, lineWidth, color, start.ToVector2(), end.ToVector2());
        }

        public static void Line(this SpriteBatch spriteBatch, Texture2D texture, Color Color, Point Start, Point End)
        {
            Vector2 start = new Vector2(Start.X, Start.Y);
            Vector2 end = new Vector2(End.X, End.Y);

            Line(spriteBatch, texture, 1, Color, start, end);
        }

        public static void Line(this SpriteBatch spriteBatch, Texture2D texture, Color Color, Vector2 Start, Vector2 End)
        {
            Line(spriteBatch, texture, 1, Color, Start, End);
        }

        public static void Line(this SpriteBatch spriteBatch, Texture2D texture, int LineWidth, Color Color, Vector2 Start, Vector2 End)
        {
            float angle = (float)Math.Atan2(Start.Y - End.Y, Start.X - End.X);
            int length = (int)Vector2.Distance(Start, End);

            //Draw line
         

            spriteBatch.Draw(texture,End, new Rectangle(0, 0, length, LineWidth), Color, angle, new Vector2(0, LineWidth/2),1f, SpriteEffects.None, 1);
        }

        public static void StrokeRect(this SpriteBatch spriteBatch, Rectangle rectangleToDraw, int thicknessOfBorder, Color color, Texture2D texture)
        {
            // Draw top line
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), color);

            // Draw left line
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), color);

            // Draw right line
            spriteBatch.Draw(texture, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), color);
            // Draw bottom line
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width, thicknessOfBorder), color);
        }

        public static void StrokeRect(this SpriteBatch spriteBatch, Rectangle rectangleToDraw, int thicknessOfBorder, Color color)
        {
            StrokeRect(spriteBatch, rectangleToDraw, thicknessOfBorder, color, InputController.DefaultTexture);
        }



        private static Texture2D circle = null;
        public static void Circle(this SpriteBatch spriteBatch, Vector2 position, float radius, Color color)
        {


            if(circle != null && circle.Bounds.Size.X >= radius*2)
                spriteBatch.Draw(circle, position: position - new Vector2(radius), scale: new Vector2(radius / (circle.Bounds.Size.X/2)), color: color);
            else
            {
                if (circle != null)
                    circle.Dispose();

                if (InputController.DefaultTexture == null)
                    throw new Exception("InputController not initialized, no default Texture");

                if (radius < 25)
                    radius = 25;


                circle = Drawing.GenerateTexture(spriteBatch.GraphicsDevice, (int)radius * 2, (int)radius * 2, 
                delegate (SpriteBatch sb) 
                {
                    for (int i = 0; i < (int)radius*2; i++)
                    {
                        for (int j = 0; j < (int)radius*2; j++)
                        {
                            Vector2 temp = new Vector2(j, i);
                            if (Vector2.Distance(temp, new Vector2(radius, radius)) <= radius)
                            {
                                sb.Draw(InputController.DefaultTexture, new Rectangle(temp.ToPoint(), new Point(1)), Color.White);
                            }
                        }
                    }
                });
            }
        }


        public static void Rectangle(this SpriteBatch spriteBatch, Rectangle bounds, Color color)
        {
            if (InputController.DefaultTexture == null)
                throw new Exception("InputController not initialized, no default Texture");

            spriteBatch.Draw(InputController.DefaultTexture, bounds, color);
        }

        public static void Rectangle(this SpriteBatch spriteBatch, Vector2 position, Vector2 scale, Color color)
        {
            if (InputController.DefaultTexture == null)
                throw new Exception("InputController not initialized, no default Texture");

            spriteBatch.Draw(InputController.DefaultTexture, position, null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }



    }
}
