using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    class LevelGenerator
    {
        public Level GenerateLevel(Level level) //For now Cellular Automata
        {
            FillLevelWithRandomNoise(level, 50);

            return level;
        }

        private void FillLevelWithRandomNoise(Level level, int percentWalls)
        {
            Random random = new Random();
            for (int x = 0; x < level.Walls.Length; x++)
            {
                for (int y = 0; y < level.Walls[x].Length; y++)
                {
                    if (random.Next(0, 100) < percentWalls)
                    {
                        level.Walls[x][y] = true;
                    }
                }
            }
        }

        private void ApplySimpleCellularAutomata(Level level)
        {

        }
    }
}
