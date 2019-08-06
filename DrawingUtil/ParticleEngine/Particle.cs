using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.DrawingUtil.ParticleEngine
{
    public class Particle
    {
        public Vector2 Position;

        public Vector2 Velocity;
        public Vector2 InitialVelocity { get; private set; }

        public float Size;
        public float InitialSize { get; private set; }

        public Color Color;

        private float life;
        public float Life { get { return life; } set { life = value; LifeTimeRatio = Life / startLife; Alive = life > 0; } }
        public float LifeTimeRatio { get; private set; }

        public bool Alive { get; private set; }

        private float startLife;

        public Particle()
        {

        }

        public Particle(Vector2 position, float life, float angle, float speed, float size, Color color)
        {
            Reuse(position, life, angle, speed, size, color);

        }

        public void Reuse(Vector2 position, float life, float angle, float speed, float size, Color color)
        {
            Position = position;
            Life = life;
            Size = size;
            Color = color;
            startLife = life;
            Alive = true;
            LifeTimeRatio = 1;
            InitialVelocity = new Vector2(speed * (float)Math.Cos(angle), speed * (float)Math.Sin(angle));
            Velocity = InitialVelocity;
            InitialSize = Size;

        }

    }
}
