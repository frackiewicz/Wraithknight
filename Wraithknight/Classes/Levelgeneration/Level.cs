﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    enum LevelData
    {
        Nothing,
        HeroSpawn,
        EnemySpawn
    }
    class Level
    {
        public bool[,] Walls;
        public LevelData[,] Data;
        public int TileWidth;
        public int TileHeight;

        public Level(int x, int y)
        {
            Walls = new bool[x, y];
            Data = new LevelData[x, y];
            TileWidth = 16;
            TileHeight = 16;
        }
    }
}
    