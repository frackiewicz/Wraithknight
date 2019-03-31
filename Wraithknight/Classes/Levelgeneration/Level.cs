using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    class Level
    {
        public bool[,] Walls;
        public EntityType[,] SpawnData;
        public int TileWidth;
        public int TileHeight;

        public Level(int x, int y)
        {
            Walls = new bool[x, y];
            SpawnData = new EntityType[x, y];
            TileWidth = 32;
            TileHeight = 32;
        }
    }
}
    