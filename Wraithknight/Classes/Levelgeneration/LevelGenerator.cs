using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    public enum LevelPreset
    {
        Test,
        Forest
    }

    struct EnemySpawnBudgetData
    {
        public EntityType Type;
        public int Cost;

        public EnemySpawnBudgetData(EntityType type, int cost)
        {
            Type = type;
            Cost = cost;
        }
    }

    class LevelGenerator //TODO got plenty of cleanup todo here
    {
        public LevelPreset CurrentPreset;

        public int Width;
        public int Height;

        public bool FinishAutomata;
        public bool DoCleanup;
        public bool RemoveEmptyRooms;

        public int NoisePercent;
        public int BoundsNoisePercent;
        public int BoundsReach; //distancefrombounds

        public int AutomataCycles;
        public int StarvationNumber;
        public int BirthNumber;

        public int MinRoomPercent;
        public int MaxRoomPercent;

        public int TotalEnemySpawnBudget;

        public readonly Dictionary<LevelPreset, List<EnemySpawnBudgetData>> EnemySpawnBudgetValues = new Dictionary<LevelPreset, List<EnemySpawnBudgetData>>
        {
            {
                LevelPreset.Forest, new List<EnemySpawnBudgetData>
                {
                    new EnemySpawnBudgetData(EntityType.Forest_Knight, 5),
                    new EnemySpawnBudgetData(EntityType.Forest_Wolf, 7)
                }
            }
        };


        public Level GenerateLevel()
        {
            Level level = new Level(Width, Height);
            return GenerateLevel(level);
        }

        public Level GenerateLevel(int width, int height)
        {
            Level level = new Level(width, height);
            return GenerateLevel(level);
        }

        public Level GenerateLevel(Level level) //For now Cellular Automata
        {
            FillLevelWithRandomNoise(level, NoisePercent);
            FillBoundsWithRandomNoise(level, BoundsNoisePercent, BoundsReach);

            if (FinishAutomata) AutomataCycles = 100;

            int loopCounter = 0;
            while (!ApplySimpleCellularAutomata(level) && loopCounter < AutomataCycles)
            {
                loopCounter++;
            }

            if (DoCleanup && !MapCleanup(level.Walls)) return GenerateLevel(level.Walls.GetLength(0), level.Walls.GetLength(1));

            SpawnEntities(level);


            return level;
        }

        public void ApplyPreset(LevelPreset preset)
        {
            if (preset == LevelPreset.Test)
            {
                CurrentPreset = LevelPreset.Test;

                Width = 100;
                Height = 100;

                FinishAutomata = false;
                DoCleanup = false;
                RemoveEmptyRooms = false;

                NoisePercent = 50;
                BoundsNoisePercent = 60;
                BoundsReach = 10;

                AutomataCycles = 1;
                StarvationNumber = 3;
                BirthNumber = 5;

                MinRoomPercent = 30;
                MaxRoomPercent = 50;
                TotalEnemySpawnBudget = 25;
            }

            if (preset == LevelPreset.Forest)
            {
                CurrentPreset = LevelPreset.Forest;

                Width = 100;
                Height = 100;

                FinishAutomata = true;
                DoCleanup = true;
                RemoveEmptyRooms = true;

                NoisePercent = 52;
                BoundsNoisePercent = 60;
                BoundsReach = 5;

                AutomataCycles = 0;
                StarvationNumber = 3;
                BirthNumber = 5;

                MinRoomPercent = 10;
                MaxRoomPercent = 15;
                TotalEnemySpawnBudget = 25;
            }
        }

        #region MapGeneration

        private static void FillLevelWithRandomNoise(Level level, int percentWalls)
        {
            Random random = new Random();
            for (int x = 0; x < level.Walls.GetLength(0); x++)
            {
                for (int y = 0; y < level.Walls.GetLength(1); y++)
                {
                    if (random.Next(0, 100) < percentWalls)
                    {
                        level.Walls[x, y] = true;
                    }
                }
            }
        }

        private static void FillBoundsWithRandomNoise(Level level, int percentWalls, int distanceFromBounds)
        {
            Random random = new Random();
            for (int x = 0; x < level.Walls.GetLength(0); x++)
            {
                for (int y = 0; y < level.Walls.GetLength(1); y++)
                {
                    if (x <= distanceFromBounds || x >= (level.Walls.GetLength(0)) - distanceFromBounds || y <= distanceFromBounds || y >= (level.Walls.GetLength(1)) - distanceFromBounds)
                        if (random.Next(0, 100) < percentWalls)
                        {
                            level.Walls[x, y] = true;
                        }
                }
            }
        }

        public bool ApplySimpleCellularAutomata(Level level)
        {
            bool[,] newWalls = new bool[level.Walls.GetLength(0), level.Walls.GetLength(1)];
            bool[,] oldWalls = level.Walls;
            for (int x = 0; x < oldWalls.GetLength(0); x++)
            {
                for (int y = 0; y < oldWalls.GetLength(1); y++)
                {
                    newWalls[x, y] = oldWalls[x, y];

                    ApplyRules(x, y, oldWalls, newWalls);
                }
            }

            bool generationFinished = AreMapsIdentical(newWalls, oldWalls);

            CopyWalls(newWalls, oldWalls);

            return generationFinished;
        }

        private void ApplyRules(int x, int y, bool[,] oldMap, bool[,] newMap)
        {
            int nearbyWalls = CountNearbyWalls(oldMap, x, y, 1);

            if (newMap[x, y])
            {
                if (nearbyWalls <= StarvationNumber) newMap[x, y] = false;
            }
            else
            {
                if (nearbyWalls >= BirthNumber) newMap[x, y] = true;
            }
        }

        private int CountNearbyWalls(Level level, int xSource, int ySource, int range)
        {
            return CountNearbyWalls(level.Walls, xSource, ySource, range);
        }

        private static int CountNearbyWalls(bool[,] map, int xSource, int ySource, int range)
        {
            int neighbors = 0;
            for (int x = xSource - range; x <= xSource + 1; x++)
            {
                for (int y = ySource - range; y <= ySource + 1; y++)
                {
                    if ((x == xSource && y == ySource))
                    {
                        continue;
                    }

                    if (x < 0 || x > map.GetLength(0) - 1 || y < 0 || y > map.GetLength(1) - 1)
                    {
                        //out of bounds
                        neighbors++;
                    }
                    else if (map[x, y])
                    {
                        neighbors++;
                    }
                }
            }

            return neighbors;
        }


        private static bool AreMapsIdentical(bool[,] a, bool[,] b)
        {
            if (a.Length != b.Length) throw new ArgumentException();

            for (int x = 0; x < a.GetLength(0); x++)
            {
                for (int y = 0; y < a.GetLength(1); y++)
                {
                    if (a[x, y] != b[x, y]) return false;
                }
            }

            return true;
        }

        private static void CopyWalls(bool[,] source, bool[,] target)
        {
            for (int x = 0; x < target.GetLength(0); x++)
            {
                for (int y = 0; y < target.GetLength(1); y++)
                {
                    target[x, y] = source[x, y];
                }
            }
        }

        #endregion

        #region MapCleanup

        private struct Floodling
        {
            public bool Value; //remember to count yourself
            public Point Source;
            public int Count;

            public Floodling(bool value, int x, int y)
            {
                Value = value;
                Source = new Point(x, y);
                Count = 0;
            }

            public Floodling(bool value, Point point)
            {
                Value = value;
                Source = point;
                Count = 0;
            }
        }

        private bool MapCleanup(bool[,] map)
        {
            if (RemoveEmptyRooms)
                if (!RemoveSmallerRooms(map))
                    return false;

            return true;
        }

        private bool RemoveSmallerRooms(bool[,] map)
        {
            bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)];
            List<Floodling> rooms = new List<Floodling>();

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (!map[x, y] && !visited[x, y])
                    {
                        FindSimilarCluster(x, y, map, visited, rooms);
                    }
                }
            }

            rooms.Sort((a, b) => b.Count.CompareTo(a.Count));

            float OpenPercentage = ((float) rooms[0].Count / map.Length) * 100;
            if (OpenPercentage < MinRoomPercent || OpenPercentage > MaxRoomPercent) return false;

            for (int i = 1; i < rooms.Count; i++)
            {
                FlipSimilarCluster(rooms[i], map, new bool[map.GetLength(0), map.GetLength(1)]);
            }

            return true;
        }

        private static int CountRooms(bool[,] map)
        {
            bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)];
            List<Floodling> rooms = new List<Floodling>();

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (!map[x, y] && !visited[x, y])
                    {
                        FindSimilarCluster(x, y, map, visited, rooms);
                    }
                }
            }

            return rooms.Count;
        }

        private static void FindSimilarCluster(int x, int y, bool[,] map, bool[,] visited, List<Floodling> floodlings)
        {
            Floodling floodling = new Floodling(map[x, y], x, y);
            Stack<Point> stack = new Stack<Point>();
            stack.Push(new Point(x, y));

            Point currentCell;
            while (stack.Count > 0)
            {
                currentCell = stack.Pop();

                if (currentCell.X < 0 || currentCell.X > map.GetLength(0) - 1 || currentCell.Y < 0 || currentCell.Y > map.GetLength(1) - 1) continue;

                if (map[currentCell.X, currentCell.Y] == floodling.Value && !visited[currentCell.X, currentCell.Y])
                {
                    floodling.Count++;
                    visited[currentCell.X, currentCell.Y] = true;

                    stack.Push(new Point(currentCell.X - 1, currentCell.Y));
                    stack.Push(new Point(currentCell.X + 1, currentCell.Y));
                    stack.Push(new Point(currentCell.X, currentCell.Y - 1));
                    stack.Push(new Point(currentCell.X, currentCell.Y + 1));
                }
            }

            floodlings.Add(floodling);
        }

        private static void FlipSimilarCluster(Floodling floodling, bool[,] map, bool[,] visited)
        {
            Stack<Point> stack = new Stack<Point>();
            stack.Push(new Point(floodling.Source.X, floodling.Source.Y));

            Point currentCell;
            while (stack.Count > 0)
            {
                currentCell = stack.Pop();

                if (currentCell.X < 0 || currentCell.X > map.GetLength(0) - 1 || currentCell.Y < 0 || currentCell.Y > map.GetLength(1) - 1) continue;

                if (map[currentCell.X, currentCell.Y] == floodling.Value && !visited[currentCell.X, currentCell.Y])
                {
                    map[currentCell.X, currentCell.Y] = !floodling.Value;
                    visited[currentCell.X, currentCell.Y] = true;

                    stack.Push(new Point(currentCell.X - 1, currentCell.Y));
                    stack.Push(new Point(currentCell.X + 1, currentCell.Y));
                    stack.Push(new Point(currentCell.X, currentCell.Y - 1));
                    stack.Push(new Point(currentCell.X, currentCell.Y + 1));
                }
            }
        }

        #endregion

        #region EntitySpawning

        private void SpawnEntities(Level level)
        {
            bool[,] walls = level.Walls;
            EntityType[,] data = level.Data;

            SpawnPathBlockers(walls, data);
            SpawnEnemies(walls, data);
            SpawnHero(walls, data);
        }

        private static void SpawnHero(bool[,] walls, EntityType[,] data)
        {
            Point point = GetRandomPoint(walls);
            while (walls[point.X, point.Y] || data[point.X, point.Y] != EntityType.Nothing)
            {
                point = GetRandomPoint(walls);
            }

            data[point.X, point.Y] = EntityType.Hero;
        }

        private void SpawnEnemies(bool[,] walls, EntityType[,] data)
        {
            int remainingEnemySpawnBudget = TotalEnemySpawnBudget;
            List<EnemySpawnBudgetData> allowedSpawns = EnemySpawnBudgetValues[CurrentPreset].OrderBy(o => o.Cost).ToList();

            Random random = new Random();
            while (remainingEnemySpawnBudget > 0)
            {
                #region TryRandomSpawns
                int randomIndex = random.Next(allowedSpawns.Count);
                if (allowedSpawns[randomIndex].Cost < remainingEnemySpawnBudget)
                {
                    SpawnEnemyAtRandomLocation(walls, data, allowedSpawns[randomIndex].Type);
                    remainingEnemySpawnBudget -= allowedSpawns[randomIndex].Cost;
                }
                #endregion

                #region TryCheapestSpawns
                else
                {
                    bool noEnemySpawned = true;

                    foreach (var enemy in allowedSpawns)
                    {
                        if (enemy.Cost < remainingEnemySpawnBudget)
                        {
                            SpawnEnemyAtRandomLocation(walls, data, enemy.Type);
                            remainingEnemySpawnBudget -= enemy.Cost;
                            noEnemySpawned = false;
                            break;
                        }
                    }

                    if (noEnemySpawned) break;
                }
                #endregion
            }
        }


        private static void SpawnEnemyAtRandomLocation(bool[,] walls, EntityType[,] data, EntityType spawnedEnemy)
        {
            Point point = GetRandomPoint(walls);
            while (walls[point.X, point.Y] || data[point.X, point.Y] != EntityType.Nothing)
            {
                point = GetRandomPoint(walls);
            }

            data[point.X, point.Y] = spawnedEnemy;
        }


        private static void SpawnDecorations(bool[,] walls, EntityType[,] data)
        {
            Point point = GetRandomPoint(walls);
            while (walls[point.X, point.Y] || data[point.X, point.Y] != EntityType.Nothing)
            {
                point = GetRandomPoint(walls);
            }
        }

        private static void SpawnPathBlockers(bool[,] walls, EntityType[,] data) //TODO THIS LATER
        {
            for (int i = 0; i < 5; i++) //TODO UPPER set by preset
            {
                Point point = GetRandomPoint(walls);
                while (walls[point.X, point.Y] || data[point.X, point.Y] != EntityType.Nothing)
                {
                    point = GetRandomPoint(walls);
                }

                data[point.X, point.Y] = EntityType.Treestump;
            }
        }

        private static Point GetRandomPoint<T>(T[,] array)
        {
            Random random = new Random();
            return new Point(random.Next(0, array.GetLength(0)), random.Next(0, array.GetLength(1)));
        }

        #endregion
    }
}
/*
 * for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    
                }
            }
 */