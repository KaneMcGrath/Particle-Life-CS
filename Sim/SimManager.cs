namespace ParticleLife.Sim
{
    public static class SimManager
    {

        public static float[] PX;
        public static float[] PY;
        public static float[] VX;
        public static float[] VY;
        public static int[] Group;

        public static int GroupCount = 8;

        public static float[] Bounds = new float[] { 0, 0, 5000f, 5000f };
        public static float dt = 0.001f;
        public static float Friction = 0.65f;

        public static void Init(int count, int groupCount)
        {
            GroupCount = groupCount;
            PX = new float[count];
            PY = new float[count];
            VX = new float[count];
            VY = new float[count];
            Group = new int[count];
            RandomizeParticles(count, groupCount);
            ParticleDynamics.Init(groupCount);
            SimRenderer.Init(groupCount);
        }

        public static void Update()
        {
            float multiplier = ParticleDynamics.MaxRadius * ParticleDynamics.ForceMultiplier;
            float maxRadiusReciprocal = 1.0f / ParticleDynamics.MaxRadius;
            float width = Bounds[2] - Bounds[0];
            float height = Bounds[3] - Bounds[1];

            // Update particle velocities
            Parallel.For(0, PX.Length, i =>
            {
                float ax = 0;
                float ay = 0;

                for (int j = 0; j < PX.Length; j++)
                {
                    if (i != j)
                    {
                        // Calculate the wrapped distance between particles
                        float dx = PX[j] - PX[i];
                        float dy = PY[j] - PY[i];

                        // Wrap distances to consider the toroidal space
                        if (dx > width / 2)
                        {
                            dx -= width;   // Wrap around right to left
                        }

                        if (dx < -width / 2)
                        {
                            dx += width;  // Wrap around left to right
                        }

                        if (dy > height / 2)
                        {
                            dy -= height; // Wrap around top to bottom
                        }

                        if (dy < -height / 2)
                        {
                            dy += height; // Wrap around bottom to top
                        }

                        float rSquared = dx * dx + dy * dy;

                        if (rSquared > 0 && rSquared < ParticleDynamics.MaxRadius * ParticleDynamics.MaxRadius)
                        {
                            float r = MathF.Sqrt(rSquared);
                            float f = ParticleDynamics.Force(r * maxRadiusReciprocal, ParticleDynamics.AttractionFactor[Group[i], Group[j]]);
                            ax += f * dx / r;
                            ay += f * dy / r;
                        }
                    }
                }

                ax *= multiplier;
                ay *= multiplier;

                VX[i] *= Friction;
                VY[i] *= Friction;
                VX[i] += ax * dt;
                VY[i] += ay * dt;
            });

            // Update particle positions
            for (int i = 0; i < PX.Length; i++)
            {
                PX[i] += VX[i] * dt;
                PY[i] += VY[i] * dt;

                // Wrap positions to keep particles within bounds
                if (PX[i] < Bounds[0])
                {
                    PX[i] += width;
                }

                if (PX[i] > Bounds[2])
                {
                    PX[i] -= width;
                }

                if (PY[i] < Bounds[1])
                {
                    PY[i] += height;
                }

                if (PY[i] > Bounds[3])
                {
                    PY[i] -= height;
                }
            }
        }



        private static void RandomizeParticles(int count, int groupCount)
        {
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                PX[i] = random.NextSingle() * Bounds[2];
                PY[i] = random.NextSingle() * Bounds[3];
                VX[i] = 0f;
                VY[i] = 0f;
                Group[i] = random.Next(0, groupCount);
            }
        }

        private static float Distance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static float FastDistance(float x1, float y1, float x2, float y2)
        {
            return MathF.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
        }
    }
}
