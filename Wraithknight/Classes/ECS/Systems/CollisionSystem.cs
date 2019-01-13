using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class CollisionSystem : System
    {
        public struct Pair
        {
            public readonly CollisionComponent Collision;
            public readonly MovementComponent Movement;

            public Pair(CollisionComponent collision, MovementComponent movement)
            {
                Collision = collision;
                Movement = movement;
            }
        }

        private readonly ECS _ecs;

        private readonly HashSet<CollisionComponent> _collisionComponents = new HashSet<CollisionComponent>();
        private readonly HashSet<Pair> _moveableCollisionComponents = new HashSet<Pair>();

        private Level _level;
        private CollisionComponent[,] _wallCollisions; //can handle only 1 component per tile

        private readonly CollisionLogicSubsystem _logicSubsystem; //TODO Breunig, maybe also move physical into a subsystem to clean up this mess?
        private readonly CollisionPhysicalSubsystem _physicalSubsystem;

        #region General System Stuff

        public CollisionSystem(ECS ecs)
        {
            _ecs = ecs;
            _logicSubsystem = new CollisionLogicSubsystem(ecs);
            _physicalSubsystem = new CollisionPhysicalSubsystem();
        }


        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponent(_collisionComponents, entities);
            BindMovementComponents();
            if(_level != null) MapWalls();
        }

        private void BindMovementComponents()
        {
            foreach (var component in _collisionComponents)
            {
                if (component.Bindings.TryGetValue(typeof(MovementComponent), out var bind))
                {
                    _moveableCollisionComponents.Add(new Pair(component, (MovementComponent)bind));
                }
            }
        }

        public override void Reset()
        {
            _moveableCollisionComponents.Clear();
            _collisionComponents.Clear();
            _logicSubsystem.ResetSystem();
            _physicalSubsystem.ResetSystem();
        }

        public HashSet<CollisionComponent> GetCollisionComponents()
        {
            return _collisionComponents;
        }

        private void AlignAllPairsWithMovement()
        {
            foreach (var pair in _moveableCollisionComponents)
            {
                AlignPairWithMovement(pair);
            }
        }

        private static void AlignPairWithMovement(Pair pair)
        {
            pair.Collision.CollisionRectangle.SetCenter(pair.Movement.Position);
        }

        public void DrawMinkowski()
        {
            foreach (var actor in _moveableCollisionComponents)
            {
                if (actor.Collision.Inactive) continue;
                foreach (var target in _collisionComponents)
                {
                    if (target.Inactive) continue;
                    if (actor.Movement.IsMoving && !actor.Collision.Equals(target))
                    {
                        Functions_Draw.DrawDebug(CollisionPhysicalSubsystem.CalculateMinkowskiDifference(actor.Collision.CollisionRectangle, target.CollisionRectangle));
                    }
                }
            }
        }

        #endregion 

        public override void Update(GameTime gameTime)
        {
            AlignAllPairsWithMovement();

            foreach (var actor in _moveableCollisionComponents)
            {
                if (actor.Collision.Inactive) continue;
                foreach (var target in _collisionComponents)
                {
                    if (target.Inactive) continue;
                    if (actor.Movement.IsMoving && !actor.Collision.Equals(target))
                    {
                        if (actor.Collision.IsPhysical && target.IsPhysical) _physicalSubsystem.HandlePhysicalCollision(actor, target, gameTime, false);
                        else _logicSubsystem.HandleCollision(actor.Collision, target, gameTime);
                    }
                }
                //check here for map
                if (actor.Collision.IsPhysical) HandleWallCollision(actor, gameTime, false);
            }
        }

        #region Level Blackmagic

        public void RegisterLevel(Level level)
        {
            _level = level;
            _wallCollisions = new CollisionComponent[level.Walls.GetLength(0),level.Walls.GetLength(1)];

            //Filter _collisionComponents for walls and put them into the array
            MapWalls();
        }

        private void MapWalls()
        {
            List<CollisionComponent> newCollisionComponents = new List<CollisionComponent>();
            foreach (var collisionComponent in _collisionComponents)
            {
                if (collisionComponent.IsWall)
                {
                    _wallCollisions[(int)(collisionComponent.CollisionRectangle.X / _level.TileWidth), (int)(collisionComponent.CollisionRectangle.Y / _level.TileHeight)] = collisionComponent;
                }
                else
                {
                    newCollisionComponents.Add(collisionComponent);
                }
            }
            _collisionComponents.Clear();
            foreach (var collisionComponent in newCollisionComponents)
            {
                _collisionComponents.Add(collisionComponent);
            }
        }

        private void HandleWallCollision(Pair actor, GameTime gameTime, bool swept)
        {
            int xSource = (int)actor.Movement.Position.X/_level.TileWidth;
            int ySource = (int)actor.Movement.Position.Y/_level.TileHeight;
            int range;

            range = (int)(1 + (swept ? actor.Movement.Speed.Cartesian.Length() : 1 * gameTime.ElapsedGameTime.TotalSeconds + Math.Sqrt(Math.Pow(actor.Collision.CollisionRectangle.Extents.X, 2) + Math.Pow(actor.Collision.CollisionRectangle.Extents.Y, 2))) / Math.Max(_level.TileWidth, _level.TileHeight));
            //minrange + Movedistance + circle around rectangle / cellsize    

            //check for close walls
            for (int x = xSource - range; x <= xSource + 1; x++)
            {
                for (int y = ySource - range; y <= ySource + 1; y++)
                {
                    if (x < 0 || x > _wallCollisions.GetLength(0) - 1 || y < 0 || y > _wallCollisions.GetLength(1) - 1)
                    {
                        //out of bounds
                    }

                    else if (_wallCollisions[x, y] != null)
                    {
                        _physicalSubsystem.HandlePhysicalCollision(actor, _wallCollisions[x, y], gameTime, swept);
                    }
                }
            }
        }

        #endregion


    }
}