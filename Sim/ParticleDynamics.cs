using ParticleLife.Game;

namespace ParticleLife.Sim
{
    public static class ParticleDynamics
    {
        private static Random random = new Random();
        public static float[,] AttractionFactor = new float[10, 10];
        public static float MaxRadius = 100f;
        public static readonly float beta = 0.3f;
        public static float ForceMultiplier = 50f;

        public static void Init(int GroupCount)
        {
            AttractionFactor = new float[GroupCount, GroupCount];
            for (int i = 0; i < GroupCount; i++)
            {
                for (int j = 0; j < GroupCount; j++)
                {
                    AttractionFactor[i, j] = ((float)random.NextDouble() * 2f) - 1f;
                    Konsole.Log("Attraction Factor: " + AttractionFactor[i, j]);
                }
            }
        }

        public static float Force(float r, float a)
        {
            if (r < beta)
            {
                return r / beta - 1;
            }
            else if (beta < r && r < 1)
            {
                return a * (1 - Math.Abs(2 * r - 1 - beta) / (1 - beta));
            }
            else { return 0; }
        }
    }
}
