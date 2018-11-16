using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    struct Level
    {
        public bool[][] Walls;

        public Level(int x, int y)
        {
            Walls = new bool[x][];
            for (int i = 0; i < Walls.Length; i++)
            {
                Walls[i] = new bool[y];
            }
        }
    }
}
    