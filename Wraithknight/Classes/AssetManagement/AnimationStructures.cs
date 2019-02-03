using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    static class AnimationStructures 
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
                case EntityType.Forest_Knight:
                {
                    frameSize = new Point(64, 64);
                    animationList.Add(new Animation(Assets.GetTexture("forestknightRunning"), "running", "running", frameSize, EntityState.Moving).CreateAnimationFrames(0, 8, 150));
                    animationList.Add(new Animation(Assets.GetTexture("forestknightIdle"), "idle", "idle", frameSize, EntityState.Idle).CreateAnimationFrames(0, 4, 500));
                    break;
                }
                case EntityType.Forest_Wolf:
                {
                    frameSize = new Point(64,32);
                    animationList.Add(new Animation(Assets.GetTexture("wolfRunning"), "running", "running", frameSize, EntityState.Moving).CreateAnimationFrames(0, 6, 100));
                    animationList.Add(new Animation(Assets.GetTexture("wolf"), "idle", "idle", frameSize, EntityState.Idle).CreateAnimationFrames(0, 1, 1000));
                    break;
                }
                case EntityType.HeroKnightSlashWeak:
                {
                    frameSize = new Point(64, 64);
                    animationList.Add(new Animation(Assets.GetTexture("heroslashweak"), "moving", "moving", frameSize, EntityState.Moving, false).CreateAnimationFrames(0, 1, 100));
                    animationList.Add(new Animation(Assets.GetTexture("heroslashweakdying"), "dying", "dying", frameSize, EntityState.Dying, false).CreateAnimationFrames(0, 9, 50));
                    break;
                }

            }

            return animationList;
        }

    }
}
