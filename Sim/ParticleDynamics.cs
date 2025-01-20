namespace ParticleLife.Sim
{
    public static class ParticleDynamics
    {
        private static Random random = new Random();
        public static float[,] AttractionFactor = new float[10, 10];

        public static void RandomizeAttraction(int GroupCount)
        {
            AttractionFactor = new float[GroupCount, GroupCount];
            for (int i = 0; i < GroupCount; i++)
            {
                for (int j = 0; j < GroupCount; j++)
                {
                    AttractionFactor[i, j] = (random.NextSingle() * 2) - 0.5f;
                }
            }
        }

        public static readonly float PeakDistance = 80f;
        public static readonly float TailDistance = 150f;
        public static readonly float RepulsionEnd = 20f;
        public static readonly float AttractorMultiplyer = 100f;
        public static readonly float RepulsionMultiplyer = 20f;

        //Force curve more closely modelling the example video
        public static float GetAttraction(int GroupA, int GroupB, float Distance)
        {
            if (Distance < RepulsionEnd)
            {
                return -(RepulsionMultiplyer / RepulsionEnd) * Distance + RepulsionMultiplyer;
            }
            else if (Distance < PeakDistance)
            {
                return (AttractionFactor[GroupA, GroupB] / (PeakDistance - RepulsionEnd)) * (Distance - RepulsionEnd);
            }
            else
            {
                return AttractionFactor[GroupA, GroupB] * ((TailDistance - Distance) / (TailDistance - PeakDistance));
            }
        }

        public static float GetAttractionForce(int GroupA, int GroupB, float Distance)
        {
            return GetAttraction(GroupA, GroupB, Distance) * AttractorMultiplyer;
        }

        public static void Update()
        {

        }
    }
}
