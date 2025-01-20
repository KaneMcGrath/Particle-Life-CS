using Raylib_cs;
using System.Numerics;

namespace ParticleLife.Sim
{
    public static class SimManager
    {
        public static int ParticleCount = 10;
        // ParticleData: x, y, xVel, yVel
        public static float[,] ParticleData = new float[10, 4];
        public static int[] ParticleGroup = new int[10];
        public static Color[] ParticleColors = new Color[10];
        public static float FixedDeltaTime = 0.016f;

        public static int ParticleGroupCount = 0;

        public static float ParticleSize = 10f;


        public static float[] SimConstraints = new float[] { -5000, 5000, -5000, 5000 };

        public static void SetupSim(int particleCount, int GroupCount)
        {
            ParticleCount = particleCount;
            ParticleGroupCount = GroupCount;
            ParticleData = new float[particleCount, 4];
            ParticleGroup = new int[particleCount];
            ParticleColors = new Color[particleCount];
            ParticleDynamics.RandomizeAttraction(GroupCount);
            RandomizePositions();
            RandomizeParticleColors();
            SimulationGrid.SetupGrid();
        }

        public static void RandomizePositions()
        {
            Random random = new Random();
            for (int i = 0; i < ParticleData.GetLength(0); i++)
            {
                ParticleData[i, 0] = random.Next(0, 1280);
                ParticleData[i, 1] = random.Next(0, 720);
                ParticleData[i, 2] = random.Next(-1, 1);
                ParticleData[i, 3] = random.Next(-1, 1);
                ParticleGroup[i] = random.Next(0, ParticleGroupCount);
            }
        }

        public static void RandomizeParticleColors()
        {
            Random random = new Random();
            for (int i = 0; i < ParticleData.GetLength(0); i++)
            {
                ParticleColors[i] = new Color((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)255);
            }
        }

        public static void Update()
        {
            GridUpdate();
            Draw();
        }

        public static void Draw()
        {
            for (int i = 0; i < ParticleData.GetLength(0); i++)
            {
                //Raylib.DrawCircle((int)ParticleData[i, 0], (int)ParticleData[i, 1], ParticleSize, ParticleColors[ParticleGroup[i]]);
                Raylib.DrawCircleV(Surface.MainCamera.WorldToScreen(new Vector2(ParticleData[i, 0], ParticleData[i, 1])), Surface.MainCamera.Scale(ParticleSize), ParticleColors[ParticleGroup[i]]);
            }
        }

        public static void SimpleUpdate()
        {
            for (int i = 0; i < ParticleData.GetLength(0); i++)
            {
                for (int j = 0; j < ParticleData.GetLength(0); j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    float d = Distance(ParticleData[i, 0], ParticleData[i, 1], ParticleData[j, 0], ParticleData[j, 1]);
                    if (d < ParticleDynamics.TailDistance)
                    {
                        UpdateParticleForce(i, j, d);
                    }
                }
            }
            for (int i = 0; i < ParticleData.GetLength(0); i++)
            {
                UpdateParticlePhysics(i);
                DampenParticle(i);
                ConstrainParticle(i);
            }
        }

        public static void GridUpdate()
        {
            //GetParticlesInEachCell
            for (int i = 0; i < ParticleData.GetLength(0); i++)
            {
                float width = SimConstraints[1] - SimConstraints[0];
                float height = SimConstraints[3] - SimConstraints[2];
                int CellX = (int)((ParticleData[i, 0] - SimConstraints[0]) / width);
                int CellY = (int)((ParticleData[i, 1] - SimConstraints[2]) / height);
                SimulationGrid.ParticleData[CellX, CellY, SimulationGrid.ParticleIndex[CellX, CellY]] = i;
                SimulationGrid.ParticleIndex[CellX, CellY] += 1;
            }
            //UpdateParticleForces In Each Cell and Neighbors
            for (int i = 0; i < SimulationGrid.GridWidth; i++)
            {
                for (int j = 0; j < SimulationGrid.GridHeight; j++)
                {
                    int[] Particles = new int[ParticleCount];
                    int index = 0;
                    int minX = Math.Max(0, i - 1);
                    int maxX = Math.Min(SimulationGrid.GridWidth - 1, i + 1);
                    int minY = Math.Max(0, j - 1);
                    int maxY = Math.Min(SimulationGrid.GridHeight - 1, j + 1);
                    for (int x = minX; x <= maxX; x++)
                    {
                        for (int y = minY; y <= maxY; y++)
                        {
                            for (int b = 0; b < SimulationGrid.ParticleIndex[x, y]; b++)
                            {
                                Particles[index] = SimulationGrid.ParticleData[x, y, b];
                                index++;
                            }
                        }
                    }
                    for (int l = 0; l < index; l++)
                    {
                        for (int m = 0; m < index; m++)
                        {
                            if (l == m)
                            {
                                continue;
                            }
                            float d = Distance(ParticleData[Particles[l], 0], ParticleData[Particles[l], 1], ParticleData[Particles[m], 0], ParticleData[Particles[m], 1]);
                            if (d < ParticleDynamics.TailDistance)
                            {
                                UpdateParticleForce(Particles[l], Particles[m], d);
                            }
                        }

                    }
                    //UpdateParticlePhysics
                    for (int T = 0; T < ParticleData.GetLength(0); T++)
                    {
                        UpdateParticlePhysics(T);
                        DampenParticle(T);
                        ConstrainParticle(T);
                    }

                }
                SimulationGrid.ClearGrid();
            }
        }

        public static void UpdateParticleForce(int indexA, int indexB, float Distance)
        {
            //Add Attraction Force
            float Attraction = ParticleDynamics.GetAttractionForce(ParticleGroup[indexA], ParticleGroup[indexB], Distance);
            float xDiff = ParticleData[indexB, 0] - ParticleData[indexA, 0];
            float yDiff = ParticleData[indexB, 1] - ParticleData[indexA, 1];
            float xForce = xDiff * Attraction;
            float yForce = yDiff * Attraction;
            ParticleData[indexA, 2] += xForce * FixedDeltaTime;
            ParticleData[indexA, 3] += yForce * FixedDeltaTime;
        }

        public static void UpdateParticlePhysics(int index)
        {
            //Update Position by Velocity
            ParticleData[index, 0] += ParticleData[index, 2] * FixedDeltaTime;
            ParticleData[index, 1] += ParticleData[index, 3] * FixedDeltaTime;
        }

        public static void DampenParticle(int index)
        {
            ParticleData[index, 2] *= 0.99f;
            ParticleData[index, 3] *= 0.99f;
        }

        public static void ConstrainParticle(int index)
        {
            if (ParticleData[index, 0] < SimConstraints[0])
            {
                ParticleData[index, 0] = SimConstraints[0];
                ParticleData[index, 2] *= -1;
            }
            if (ParticleData[index, 0] > SimConstraints[1])
            {
                ParticleData[index, 0] = SimConstraints[1];
                ParticleData[index, 2] *= -1;
            }
            if (ParticleData[index, 1] < SimConstraints[2])
            {
                ParticleData[index, 1] = SimConstraints[2];
                ParticleData[index, 3] *= -1;
            }
            if (ParticleData[index, 1] > SimConstraints[3])
            {
                ParticleData[index, 1] = SimConstraints[3];
                ParticleData[index, 3] *= -1;
            }
        }

        private static float Distance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
