using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    static class AnimationStructures //TODO Breunig talk about this
    {
        /*
         * maybe also add an Identifier library for states?
         */
        public static List<Animation> GetAnimationList(EntityType type)
        {
            List<Animation> animationList = new List<Animation>();
            Point frameSize;

            switch (type)
            {
                case EntityType.Forest_Wolf:
                {
                    frameSize = new Point(64,32);
                    animationList.Add(new Animation(Assets.GetTexture("wolfRunning"), "running", "running", frameSize, EntityState.Moving).CreateAnimationFrames(0, 6, 100));
                    animationList.Add(new Animation(Assets.GetTexture("wolf"), "idle", "idle", frameSize, EntityState.Idle).CreateAnimationFrames(0, 1, 1000));
                } break;

            }

            return animationList;
        }

    }
}
