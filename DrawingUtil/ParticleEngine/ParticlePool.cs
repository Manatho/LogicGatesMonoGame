using GameEngine.Logic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.DrawingUtil.ParticleEngine
{
    class ParticlePool
    {
        public Particle this[int i] { get { return particles[i]; } }


        private Particle[] particles;
        public int Index { get; private set; }


        public ParticlePool(int size)
        {
            Index = 0;

            particles = new Particle[size];
            for (int i = 0; i < particles.Length; i++)
                particles[i] = new Particle();
        }

        

        public IEnumerable<Particle> Spawn()
        {
            for (int i = Index; i < particles.Length; i++)
            {
                Index++;
                yield return particles[i];
                
            }
        }

        public IEnumerable<Particle> Alive()
        {
            for (int i = 0; i < Index; i++)
            {
                if (!particles[i].Alive)
                {
                    Particle temp = particles[i];
                    particles[i] = particles[Index - 1];
                    particles[Index - 1] = temp;
                    i--;
                    Index--;
                }

                if(i != -1)
                    yield return particles[i];
            }
        }
    }
}
