namespace ParticleLife.Sim
{
    public static class SimulationGrid
    {
        public static int GridWidth = 100;
        public static int GridHeight = 100;
        public static int[,,] ParticleData = new int[0, 0, 0];
        public static int[,] ParticleIndex = new int[0, 0];

        public static void SetupGrid()
        {
            //Partition the grid based on the ParticleDynamics and SimConstraints
            float width = SimManager.SimConstraints[1] - SimManager.SimConstraints[0];
            float height = SimManager.SimConstraints[3] - SimManager.SimConstraints[2];
            GridWidth = (int)(Math.Ceiling(width / ParticleDynamics.TailDistance));
            GridHeight = (int)(Math.Ceiling(height / ParticleDynamics.TailDistance));
            ParticleData = new int[GridWidth, GridHeight, SimManager.ParticleCount];
            ParticleIndex = new int[GridWidth, GridHeight];
        }

        public static void ClearGrid()
        {
            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    //for (int d = 0; d < SimManager.ParticleCount; d++)
                    //{
                    //    ParticleData[i, j, d] = 0;
                    //
                    //}
                    ParticleIndex[i, j] = 0;
                }
            }
        }
    }
}
