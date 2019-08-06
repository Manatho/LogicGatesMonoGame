using GameEngine.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.DrawingUtil.ParticleEngine
{
    public class Emitter
    {
        private ParticlePool pool;

        private List<IProcessParticle> particleProcessors;
        private List<ISpawnParticle> particleSpawners;

        public bool Spawning { get; private set; }
        private float spawnDuration = -1;
        private double startedSpawning;

        public Emitter(ICollection<IProcessParticle> processors, ICollection<ISpawnParticle> spawners, int count)
        {
            pool = new ParticlePool(count);

            particleSpawners = new List<ISpawnParticle>();
            particleSpawners.AddRange(spawners);

            particleProcessors = new List<IProcessParticle>();
            particleProcessors.AddRange(processors);
            particleProcessors.Add(new ParticleLifeTime());
        }

        public void Spawn()
        {
            Spawning = true;
            spawnDuration = -1;
            startedSpawning = LogicController.CurrentMillisecond;
        }

        public void Spawn(float duration)
        {
            Spawn();
            spawnDuration = duration;
        }

        public void Stop()
        {
            Spawning = false;
        }

        public void Update()
        {
            if(Spawning)
            {
                SpawnParticles();
                if (spawnDuration != -1 && LogicController.CurrentMillisecond - startedSpawning > spawnDuration * 1000)
                    Spawning = false;
            }
                
            foreach (Particle p in pool.Alive())
            {
                foreach (IProcessParticle process in particleProcessors)
                    process.Process(p, LogicController.TimeDelta);
            }
        }

        private void SpawnParticles()
        {
            IEnumerator<Particle> enumerator = pool.Spawn().GetEnumerator();

            foreach (ISpawnParticle s in particleSpawners)
            {
                for (int i = s.SpawnCount(); i > 0; i--)
                {
                    if(enumerator.MoveNext())
                        s.Spawn(enumerator.Current);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in pool.Alive())
                spriteBatch.Circle(p.Position, p.Size, p.Color);
                //spriteBatch.Rectangle(new Rectangle(p.Position.ToPoint(), new Point((int)p.Size)), p.Color);
        }


        public T GetParticleProcessor<T>()
        {
            foreach (IProcessParticle p in particleProcessors)
                if(p is T)
                    return (T)p;
            return default(T);
        }

        public T GetParticleSpawner<T>()
        {
            foreach (ISpawnParticle p in particleSpawners)
                if (p is T)
                    return (T)p;
            return default(T);
        }
    }


    public static class RandomExtension
    {
        public static float NextFloat(this Random r)
        {
            return (float)r.NextDouble();
        }
    }

    public class ParticleLifeTime : IProcessParticle
    {
        void IProcessParticle.Process(Particle particle, float deltaTime)
        {
            particle.Life -= deltaTime;
        }
    }

    public class ParticleMover : IProcessParticle
    {
        void IProcessParticle.Process(Particle particle, float deltaTime)
        {
            if (particle.Alive)
            {
                particle.Velocity = particle.InitialVelocity * (particle.LifeTimeRatio* particle.LifeTimeRatio * 10);
                particle.Position += particle.Velocity * deltaTime;
            }
                
        }
    }

    public class ParticleShrinker : IProcessParticle
    {
        void IProcessParticle.Process(Particle particle, float deltaTime)
        {
            //if(particle.LifeTimeRatio > 0.75)
                particle.Size = (particle.LifeTimeRatio * particle.InitialSize);
            //else
               // particle.Size = ((1.50f-particle.LifeTimeRatio) + (5 * (0.75f - particle.LifeTimeRatio)) * particle.InitialSize);
        }
    }


    public class ColorShift : IProcessParticle
    {
        SortedList<float, Color> colors;
        float key;
        float previousKey;

        public Color Start { get { return colors[1]; } set { colors[1] = value; } }
        public Color End { get { return colors[0]; } set { colors[0] = value; } }

        public ColorShift(Color startColor, Color endColor)
        {
            colors = new SortedList<float, Color>();
            colors.Add(1, startColor);
            colors.Add(0, endColor);
        }


        void IProcessParticle.Process(Particle particle, float deltaTime)
        {



            for(int i = colors.Keys.Count-2; i >= 0; i--)
                if (colors.Keys[i] <= particle.LifeTimeRatio)
                {
                    previousKey = colors.Keys[i+1];
                    key = colors.Keys[i];
                    break;
                }

            particle.Color = Color.Lerp(colors[previousKey], colors[key], Map(particle.LifeTimeRatio, previousKey, key ));
           // Console.WriteLine(Map(particle.LifeTimeRatio, previousKey, key) + " " + particle.LifeTimeRatio);
        }


        private float Map(float value, float start, float stop)
        {
            return (value - start) / (stop - start);
        }


        public void Add(float startTime, Color color)
        {
            colors.Add(startTime, color);
        }
    }

    public class DefaultSpawner : ISpawnParticle
    {
        public Vector2 Position;
        public float Cone = 6.28f;
        public float Angle = 0;
        public Color Color = Color.Red;
        public float Speed = 25;
        public float SpeedVariance = 25;

        public int MaxActiveParticles { get; private set; }

        public int EmissionRate = -1;
        private double last;

        public DefaultSpawner(Vector2 position, int amount)
        {
            this.Position = position;
            this.MaxActiveParticles = amount;
        }

        public DefaultSpawner(Vector2 position, int amount, int emisionRate)
            : this(position, amount)
        {
            EmissionRate = emisionRate;
        }

        Random r = new Random(10);
        void ISpawnParticle.Spawn(Particle deadParticle)
        {
            float temp = (float)r.NextDouble();
            float test = (float)r.NextDouble();
            deadParticle.Reuse(Position, 1f, Angle - Cone / 2 + Cone * r.NextFloat(), SpeedVariance * test + Speed - SpeedVariance/2, 3, Color);
        }

        public int SpawnCount()
        {
            if (EmissionRate == -1)
                return MaxActiveParticles;

            int amount = (int)((LogicController.CurrentMillisecond - last) / (1000f/ EmissionRate));

            if (amount != 0)
                last = LogicController.CurrentMillisecond;


            return amount;
        }


    }


}
