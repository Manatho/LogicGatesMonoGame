using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.DrawingUtil
{
	public class Drawing
	{


		//Standard metoder til at tegne simpel geometri
			

			public delegate void DrawDelegate(SpriteBatch spriteBatch);



			public static Texture2D GenerateTexture(GraphicsDevice GraphicsDevice, int Width, int Height, DrawDelegate ThingToDraw)
			{
				using (SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice))
				{
					RenderTarget2D renderTarget = new RenderTarget2D(GraphicsDevice, Width, Height, false, SurfaceFormat.Rgba64, DepthFormat.None, 16, RenderTargetUsage.DiscardContents);
					GraphicsDevice.SetRenderTarget(renderTarget);
					GraphicsDevice.Clear(Color.Transparent);

					spriteBatch.Begin();
					ThingToDraw(spriteBatch);
					spriteBatch.End();

					GraphicsDevice.SetRenderTarget(null);
					return (Texture2D)renderTarget;
				}
			}
	}


	public class TexturePlacement
	{
		public Rectangle SourceRectangle { get; set; }
		public Rectangle DestinationRectangle { get; set; }

		public Vector2 Position { get; set; }

		public Vector2 Scale { get; set; }

		public bool Draw { get { return !DestinationRectangle.IsEmpty; } }

		public TexturePlacement(Rectangle destination, Rectangle source)
		{
			SourceRectangle = source;
			DestinationRectangle = destination;
		}

		public TexturePlacement(Rectangle source, Vector2 position, Vector2 scale)
		{
			SourceRectangle = source;
			Position = position;
			Scale = scale;
		}


		public void SetScale(Point textureSize, Point itemSize)
		{
			Scale = new Vector2(itemSize.X / (float)textureSize.X, itemSize.Y / (float)textureSize.Y);

		}

	}
}
