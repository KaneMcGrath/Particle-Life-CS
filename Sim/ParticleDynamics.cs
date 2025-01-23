using ComputeSharp;

namespace ParticleLife.Sim
{
    public static class ParticleDynamics
    {
        private static Random random = new Random();
        public static float[,] AttractionMatrix = new float[10, 10];
        public static float MaxRadius = 1000f;
        public static readonly float beta = 0.3f;
        public static float ForceMultiplier = 250f;

        public static void Init(int GroupCount)
        {
            AttractionMatrix = new float[GroupCount, GroupCount];
            for (int i = 0; i < GroupCount; i++)
            {
                for (int j = 0; j < GroupCount; j++)
                {
                    AttractionMatrix[i, j] = ((float)random.NextDouble() * 2f) - 1f;
                    ;
                }
            }
            SimRenderer.Init(GroupCount);
        }

        //When the attraction matrix is too large, downsample it
        public static void DownSampleAttractionMatrix(int GroupCount)
        {
            float[,] matrixCopy = new float[GroupCount, GroupCount];
            for (int i = 0; i < GroupCount; i++)
            {
                for (int j = 0; j < GroupCount; j++)
                {
                    if (i < AttractionMatrix.GetLength(0) && j < AttractionMatrix.GetLength(1))
                    {
                        matrixCopy[i, j] = AttractionMatrix[i, j];
                    }
                    else
                    {
                        matrixCopy[i, j] = ((float)random.NextDouble() * 2f) - 1f;
                    }
                }
            }
            AttractionMatrix = matrixCopy;
            //particles with valid groups are ignored
            //invalid groups are randomized
            for (int i = 0; i < SimManager.PX.Length; i++)
            {
                if (SimManager.Group[i] >= GroupCount)
                {
                    SimManager.Group[i] = random.Next(0, GroupCount);
                }
            }
        }

        public static void RandomizeAttractionMatrix()
        {
            for (int i = 0; i < AttractionMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < AttractionMatrix.GetLength(1); j++)
                {
                    AttractionMatrix[i, j] = ((float)random.NextDouble() * 2f) - 1f;
                }
            }
        }



        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static float Force(float r, float a)
        {
            if (r < beta)
            {
                return r / beta - 1;
            }
            else if (r < 1) // Combining "beta < r && r < 1"
            {
                return a * (1 - MathF.Abs(2 * (r - 0.5f) - beta) / (1 - beta));
            }
            else
            {
                return 0;
            }
        }

        public static float GForce(float r, float a)
        {
            if (r < beta)
            {
                return r / beta - 1;
            }
            else if (r < 1) // Combining "beta < r && r < 1"
            {
                return a * (1 - Hlsl.Abs(2 * (r - 0.5f) - beta) / (1 - beta));
            }
            else
            {
                return 0;
            }
        }
    }
}
