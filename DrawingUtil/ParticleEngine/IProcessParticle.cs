
namespace GameEngine.DrawingUtil.ParticleEngine
{
    public interface IProcessParticle
    {
        void Process(Particle particle, float deltaTime);
    }


    public interface ISpawnParticle
    {
        int MaxActiveParticles { get; }

        void Spawn(Particle deadParticle);
        int SpawnCount();
    }


}
