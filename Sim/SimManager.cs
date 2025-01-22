using ComputeSharp;
using ParticleLife.Game;

namespace ParticleLife.Sim
{
    public static partial class SimManager
    {

        public static float[] PX;
        public static float[] PY;
        public static float[] VX;
        public static float[] VY;
        public static int[] Group;

        public static int GroupCount = 8;

        public static float[] Bounds = new float[] { 0, 0, 5000f, 5000f };
        public static float dt = 0.0004f;
        public static float Friction = 0.65f;

        public static int StepCounter = 0;

        public static void Init(int count, int groupCount, ParticleDispersionOptions options = ParticleDispersionOptions.Random)
        {
            GroupCount = groupCount;
            PX = new float[count];
            PY = new float[count];
            VX = new float[count];
            VY = new float[count];
            Group = new int[count];
            DistributeParticles(count, groupCount, options);
            ParticleDynamics.Init(groupCount);
            SimRenderer.Init(groupCount);
        }

        private static float StepTimer = 0f;

        private static float[] Flatten2DArray(float[,] array, int width)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            if (cols != width)
            {
                throw new ArgumentException("The provided width does not match the second dimension of the array.");
            }

            float[] flattenedArray = new float[rows * cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    flattenedArray[i * cols + j] = array[i, j];
                }
            }

            return flattenedArray;
        }
        public static void Update()
        {
            float multiplier = ParticleDynamics.MaxRadius * ParticleDynamics.ForceMultiplier;
            float maxRadiusReciprocal = 1.0f / ParticleDynamics.MaxRadius;
            float width = Bounds[2] - Bounds[0];
            float height = Bounds[3] - Bounds[1];

            using ReadWriteBuffer<float> PXBuffer = GraphicsDevice.GetDefault().AllocateReadWriteBuffer<float>(PX);
            using ReadWriteBuffer<float> PYBuffer = GraphicsDevice.GetDefault().AllocateReadWriteBuffer<float>(PY);
            using ReadWriteBuffer<float> VXBuffer = GraphicsDevice.GetDefault().AllocateReadWriteBuffer<float>(VX);
            using ReadWriteBuffer<float> VYBuffer = GraphicsDevice.GetDefault().AllocateReadWriteBuffer<float>(VY);
            using ReadOnlyBuffer<int> GroupBuffer = GraphicsDevice.GetDefault().AllocateReadOnlyBuffer(Group);
            using ReadOnlyBuffer<float> AttractorsBuffer = GraphicsDevice.GetDefault().AllocateReadOnlyBuffer(Flatten2DArray(ParticleDynamics.AttractionMatrix, ParticleDynamics.AttractionMatrix.GetLength(0)));

            GraphicsDevice.GetDefault().For(PX.Length, new ParticleUpdate(PXBuffer, PYBuffer, VXBuffer, VYBuffer, GroupBuffer, ParticleDynamics.MaxRadius, ParticleDynamics.ForceMultiplier, width, width / 2, height, height / 2, maxRadiusReciprocal, ParticleDynamics.MaxRadius * ParticleDynamics.MaxRadius, multiplier, AttractorsBuffer, ParticleDynamics.AttractionMatrix.GetLength(0)));
            //GraphicsDevice.GetDefault().For(PX.Length, new ParticlePositionUpdate(PXBuffer, PYBuffer, VXBuffer, VYBuffer, dt, width, height, Bounds[2]));
            PXBuffer.CopyTo(PX);
            PYBuffer.CopyTo(PY);
            VXBuffer.CopyTo(VX);
            VYBuffer.CopyTo(VY);


            // Update particle velocities
            if (false)
            {
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
                                float f = ParticleDynamics.Force(r * maxRadiusReciprocal, ParticleDynamics.AttractionMatrix[Group[i], Group[j]]);
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



            }
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
        [ThreadGroupSize(DefaultThreadGroupSizes.X)]
        [GeneratedComputeShaderDescriptor]
        public readonly partial struct ParticleUpdate(
            ReadWriteBuffer<float> PXBuffer,
            ReadWriteBuffer<float> PYBuffer,
            ReadWriteBuffer<float> VXBuffer,
            ReadWriteBuffer<float> VYBuffer,
            ReadOnlyBuffer<int> groups,
            float MaxRadius,
            float ForceMultiplier,
            float width,
            float hwidth,
            float height,
            float hheight,
            float MRR,
            float MRSquared,
            float multiplier,
            ReadOnlyBuffer<float> Attractors,
            int attractorsArrayWidth
        ) : IComputeShader
        {

            public void Execute()
            {

                int i = ThreadIds.X;
                float ax = 0;
                float ay = 0;
                for (int j = 0; j < PXBuffer.Length; j++)
                {
                    if (i != j)
                    {
                        // Calculate the wrapped distance between particles
                        float dx = PXBuffer[j] - PXBuffer[i];
                        float dy = PYBuffer[j] - PYBuffer[i];
                        // Wrap distances to consider the toroidal space
                        if (dx > hwidth)
                        {
                            dx -= width;   // Wrap around right to left
                        }
                        if (dx < -hheight)
                        {
                            dx += width;  // Wrap around left to right
                        }
                        if (dy > hwidth)
                        {
                            dy -= height; // Wrap around top to bottom
                        }
                        if (dy < -hheight)
                        {
                            dy += height; // Wrap around bottom to top
                        }

                        float rSquared = dx * dx + dy * dy;

                        if (rSquared > 0 && rSquared < ParticleDynamics.MaxRadius * ParticleDynamics.MaxRadius)
                        {
                            float r = Hlsl.Length(new ComputeSharp.Float2(dx, dy));
                            float a = Attractors[groups[i] * attractorsArrayWidth + groups[j]];
                            float f = ParticleDynamics.GForce(r * MRR, a);
                            ax += f * dx / r;
                            ay += f * dy / r;
                        }
                    }
                }

                ax *= multiplier;
                ay *= multiplier;
                VXBuffer[i] *= Friction;
                VYBuffer[i] *= Friction;
                VXBuffer[i] += ax * dt;
                VYBuffer[i] += ay * dt;
            }
        }

        public static void DistributeParticles(int count, int groupCount, ParticleDispersionOptions options)
        {
            switch (options)
            {
                case ParticleDispersionOptions.Random:
                    RandomizeParticles(count, groupCount);
                    break;

                case ParticleDispersionOptions.Distributed:
                    int sideCount = (int)Math.Sqrt(count);
                    for (int i = 0; i < count; i++)
                    {
                        PX[i] = (i % sideCount) * Bounds[2] / sideCount;
                        PY[i] = (i / sideCount) * Bounds[3] / sideCount;
                        VX[i] = 0f;
                        VY[i] = 0f;
                        Group[i] = i % groupCount;
                    }

                    break;

                //Pie chart like distribution
                case ParticleDispersionOptions.Pie:
                    DistributePie(count, groupCount);
                    break;


                //Distribute large squares of each group side-by-side
                case ParticleDispersionOptions.Grid:
                    DistributeGrid(count, groupCount);
                    break;

                case ParticleDispersionOptions.Line:
                    int halfCount = count / 2;
                    for (int i = 0; i < halfCount; i++)
                    {
                        PX[i] = Bounds[2] / 2;
                        PY[i] = Bounds[3] / 2 + i * Bounds[3] / halfCount;
                        VX[i] = 0f;
                        VY[i] = 0f;
                        Group[i] = i % groupCount;
                    }
                    for (int i = halfCount; i < count; i++)
                    {
                        PX[i] = Bounds[3] / 2 + i * Bounds[3] / halfCount;
                        PY[i] = Bounds[2] / 2;
                        VX[i] = 0f;
                        VY[i] = 0f;
                        Group[i] = i % groupCount;
                    }
                    break;

                case ParticleDispersionOptions.Circle:
                    for (int i = 0; i < count; i++)
                    {
                        PX[i] = Bounds[2] / 2 + (float)Math.Cos(i * Math.PI * 2 / count) * Bounds[2] / 4;
                        PY[i] = Bounds[3] / 2 + (float)Math.Sin(i * Math.PI * 2 / count) * Bounds[3] / 4;
                        VX[i] = 0f;
                        VY[i] = 0f;
                        Group[i] = i % groupCount;
                    }
                    break;
            }

        }

        public static void RandomizeParticles(int count, int groupCount)
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
        public static void DistributeGrid(int count, int groupCount)
        {
            int GroupSize = count / groupCount;

            int CellGridSize = (int)Math.Ceiling(Math.Sqrt(groupCount));
            int ParticleGridSize = (int)Math.Ceiling(Math.Sqrt(GroupSize));
            int groupCounter = 0;
            int CellIndex = 0;
            float CellWidth = Bounds[2] / CellGridSize;
            float CellHeight = Bounds[3] / CellGridSize;
            float particleWidth = CellWidth / ParticleGridSize;

            float getCellX(int index)
            {
                return (index % CellGridSize) * CellWidth;
            }
            float getCellY(int index)
            {
                return (index / CellGridSize) * CellHeight;
            }

            float GetParticleX(int index)
            {
                return (index % ParticleGridSize) * particleWidth;
            }

            float GetParticleY(int index)
            {

                return (index / ParticleGridSize) * particleWidth;
            }

            for (int i = 0; i < count; i++)
            {
                int groupIndex = i - (CellIndex * GroupSize);


                PX[i] = getCellX(CellIndex) + GetParticleX(groupCounter);
                PY[i] = getCellY(CellIndex) + GetParticleY(groupCounter);
                Group[i] = CellIndex;
                groupCounter++;
                if (groupCounter >= GroupSize)
                {
                    groupCounter = 0;
                    CellIndex++;
                    if (CellIndex >= groupCount)
                    {
                        CellIndex = 0;
                    }
                }
            }
        }
        public static void DistributePie(int count, int groupCount)
        {
            if (count <= 0 || groupCount <= 0)
            {
                throw new ArgumentException("Count and groupCount must be greater than 0.");
            }

            Random random = new Random();
            int index = 0;

            for (int g = 0; g < groupCount; g++)
            {
                float startAngle = g * (2.0f * (float)Math.PI / groupCount);
                float endAngle = (g + 1) * (2.0f * (float)Math.PI / groupCount);

                int pointsInGroup = count / groupCount + (g < count % groupCount ? 1 : 0);

                for (int p = 0; p < pointsInGroup; p++)
                {
                    // Generate a random radius (square root ensures uniform distribution in area)
                    float radius = (float)Math.Sqrt(random.NextDouble()) * 0.5f; // Scale to radius of 0.5

                    // Generate a random angle within the group's slice
                    float angle = (float)(startAngle + random.NextDouble() * (endAngle - startAngle));

                    // Convert polar coordinates to Cartesian coordinates (shift to center at 0.5, 0.5)
                    PX[index] = (0.5f + radius * (float)Math.Cos(angle)) * (Bounds[2] / 2) + (Bounds[2] / 4);
                    PY[index] = (0.5f + radius * (float)Math.Sin(angle)) * (Bounds[2] / 2) + (Bounds[2] / 4);

                    // Assign the group index
                    Group[index] = g;

                    index++;
                }
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
