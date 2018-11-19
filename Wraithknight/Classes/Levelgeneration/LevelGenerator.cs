using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    public enum LevelPreset
    {
        Test
    }

    class LevelGenerator
    {
        public int NoisePercent;
        public int BoundsNoisePercent;
        public int BoundsReach; //distancefrombounds

        public int AutomataCycles;
        public int StarvationNumber;
        public int BirthNumber;


        public Level GenerateLevel()
        {
            Level level = new Level(50, 50);
            return GenerateLevel(level);
        }

        public Level GenerateLevel(Level level) //For now Cellular Automata
        {
            FillLevelWithRandomNoise(level, 50);
            FillBoundsWithRandomNoise(level, 60, 5);
            for (int i = 0; i < 0; i++)
            {
                ApplySimpleCellularAutomata(level, StarvationNumber, BirthNumber);
            }
            return level;
        }

        public void ApplyPreset(LevelPreset preset)
        {
            if (preset == LevelPreset.Test)
            {
                NoisePercent = 50;
                BoundsNoisePercent = 60;
                BoundsReach = 5;

                AutomataCycles = 0;
                StarvationNumber = 3;
                BirthNumber = 5;
            }
        }

        private void FillLevelWithRandomNoise(Level level, int percentWalls)
        {
            Random random = new Random();
            for (int x = 0; x < level.Walls.GetLength(0); x++)
            {
                for (int y = 0; y < level.Walls.GetLength(1); y++)
                {
                    if (random.Next(0, 100) < percentWalls)
                    {
                        level.Walls[x,y] = true;
                    }
                }
            }
        }

        private void FillBoundsWithRandomNoise(Level level, int percentWalls, int distanceFromBounds)
        {
            Random random = new Random();
            for (int x = 0; x < level.Walls.GetLength(0); x++)
            {
                for (int y = 0; y < level.Walls.GetLength(1); y++)
                {
                    if(x <= distanceFromBounds || x >= (level.Walls.GetLength(0)) - distanceFromBounds || y <= distanceFromBounds || y >= (level.Walls.GetLength(1)) - distanceFromBounds)
                    if (random.Next(0, 100) < percentWalls)
                    {
                        level.Walls[x, y] = true;
                    }
                }
            }
        }

        public void ApplySimpleCellularAutomata(Level level, int starvationNumber, int birthNumber)
        {
            bool[,] newWalls = new bool[level.Walls.GetLength(0), level.Walls.GetLength(1)];
            for (int x = 0; x < level.Walls.GetLength(0); x++)
            {
                for (int y = 0; y < level.Walls.GetLength(1); y++)
                {
                    newWalls[x, y] = level.Walls[x, y];

                    int nearbyWalls = CountNearbyWalls(level, x, y, 1);

                    if (newWalls[x, y])
                    { //Cell is a wall
                        if (nearbyWalls <= starvationNumber) newWalls[x, y] = false;
                    }
                    else
                    { //Cell is not a wall
                        if(nearbyWalls >= birthNumber) newWalls[x, y] = true;
                    }
                }
            }

            //TODO Breunig why doesnt this work v
            // level.Walls = newWalls;

            for (int x = 0; x < level.Walls.GetLength(0); x++)
            {
                for (int y = 0; y < level.Walls.GetLength(1); y++)
                {
                    level.Walls[x, y] = newWalls[x, y];
                }
            }
        }

        private int CountNearbyWalls(Level level, int xSource, int ySource, int range)
        {
            int neighbors = 0;
            for (int x = xSource-range; x <= xSource+1; x++)
            {
                for (int y = ySource-range; y <= ySource+1; y++)
                {
                    if ((x == xSource && y == ySource))
                    {
                        continue;
                    }

                    if (x < 0 || x > level.Walls.GetLength(0) - 1 || y < 0 || y > level.Walls.GetLength(1) - 1)
                    { //out of bounds
                        neighbors++;
                    }
                    else if (level.Walls[x, y])
                    {
                        neighbors++;
                    }
                }
            }
            return neighbors;
        }
    }
}
