﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    public enum CollisionBehavior
    {
        Block,
        Disappear,
        DisappearOnWall,
        Bounce,
        Stick,
        Pass //useful for sticky floors, event triggers etc
    }

    class CollisionComponent : BindableComponent
    {
        public CollisionBehavior Behavior;
        public AABB CollisionRectangle;
        public Vector2 Offset;

        public bool IsWall; //Should use coordinate efficiency
        public bool IsPhysical;
        public bool IsPhasing; //Used for Blinking? maybe deal damage to self if stuck in enemy to discourage it


        public CollisionComponent(CollisionBehavior behavior = CollisionBehavior.Block, AABB collisionRectangle = new AABB(), Vector2 offset = new Vector2(), bool isWall = false, bool isPhysical = false)
        {
            Behavior = behavior;
            SetIsPhysical();
            CollisionRectangle = collisionRectangle;
            Offset = offset;
            IsWall = isWall;
            IsPhysical = isPhysical;
        }

        private void SetIsPhysical()
        {
            IsPhysical = Behavior == CollisionBehavior.Block || Behavior == CollisionBehavior.Bounce;
        }
    }
}

/*
 * How about you turn this component into a list of collision boxes
 * those boxes than have their specific behaviors assigned
 * components which have this bound to them will then ask for the behaviortype that the require
 *
 * thoughts??
 * you need to do this before finishing daggers
 * TODO Breunig?
 */