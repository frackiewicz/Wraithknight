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

                #region Actors

                case EntityType.ForestKnight:
                {
                    frameSize = new Point(128, 64);
                    animationList.Add(new Animation(Assets.GetTexture("forestKnightRunning"), "running", "running", frameSize, EntityState.Moving).CreateAnimationFrames(0, 9, 150));
                    animationList.Add(new Animation(Assets.GetTexture("forestKnightIdle"), "idle", "idle", frameSize, EntityState.Idle).CreateAnimationFrames(0, 5, 500));
                    animationList.Add(new Animation(Assets.GetTexture("forestKnightAttack"), "attacking", "idle", frameSize, EntityState.Attacking).CreateAnimationFrames(0, 8, 100));
                    animationList.Add(new Animation(Assets.GetTexture("forestKnightDying"), "dying", "dying", frameSize, EntityState.Dying).CreateAnimationFrames(0, 17, 100));
                    break;
                }

                case EntityType.ForestWolf:
                {
                    frameSize = new Point(64, 48);
                    animationList.Add(new Animation(Assets.GetTexture("forestWolfRunning"), "running", "running", frameSize, EntityState.Moving).CreateAnimationFrames(0, 6, 100));
                    animationList.Add(new Animation(Assets.GetTexture("forestWolfIdle"), "idle", "idle", frameSize, EntityState.Idle).CreateAnimationFrames(0, 1, 1000));
                    animationList.Add(new Animation(Assets.GetTexture("forestWolfAttack"), "attacking", "idle", frameSize, EntityState.Attacking).CreateAnimationFrames(0, 6, 100));
                    animationList.Add(new Animation(Assets.GetTexture("forestWolfDying"), "dying", "dying", frameSize, EntityState.Dying).CreateAnimationFrames(0, 7, 100));
                    break;
                }

                case EntityType.ForestArcher:
                {
                    frameSize = new Point(96, 64);
                    animationList.Add(new Animation(Assets.GetTexture("forestArcherRunning"), "running", "running", frameSize, EntityState.Moving).CreateAnimationFrames(0, 10, 100));
                    animationList.Add(new Animation(Assets.GetTexture("forestArcherAttack"), "idle", "idle", frameSize, EntityState.Idle).CreateAnimationFrames(0, 1, 500));
                    animationList.Add(new Animation(Assets.GetTexture("forestArcherAttack"), "attacking", "idle", frameSize, EntityState.Attacking).CreateAnimationFrames(0, 22, 100));
                    animationList.Add(new Animation(Assets.GetTexture("forestArcherDying"), "dying", "dying", frameSize, EntityState.Dying).CreateAnimationFrames(0, 13, 100));
                    break;
                }

                case EntityType.ForestBoss:
                {
                    frameSize = new Point(132, 96);
                    animationList.Add(new Animation(Assets.GetTexture("forestBossRunning"), "running", "running", frameSize, EntityState.Moving).CreateAnimationFrames(0, 8, 150));
                    animationList.Add(new Animation(Assets.GetTexture("forestBossDying"), "idle", "idle", frameSize, EntityState.Idle).CreateAnimationFrames(0, 1, 500));
                    animationList.Add(new Animation(Assets.GetTexture("forestBossAttack"), "attacking", "idle", frameSize, EntityState.Attacking).CreateAnimationFrames(0, 10, 100));
                    animationList.Add(new Animation(Assets.GetTexture("forestBossDying"), "dying", "dying", frameSize, EntityState.Dying).CreateAnimationFrames(0, 18, 100));
                    break;
                }
                #endregion

                #region Objects

                #endregion

                #region Projectiles

                case EntityType.HeroKnightSlashWeak:
                {
                    frameSize = new Point(64, 64);
                    animationList.Add(new Animation(Assets.GetTexture("heroslashweak"), "moving", "moving", frameSize, EntityState.Moving, false).CreateAnimationFrames(0, 1, 100));
                    animationList.Add(new Animation(Assets.GetTexture("heroslashweakdying"), "dying", "dying", frameSize, EntityState.Dying, false).CreateAnimationFrames(0, 9, 50));
                    break;
                }
                case EntityType.HeroKnightThrowingDagger:
                {
                    frameSize = new Point(16, 16);
                    animationList.Add(new Animation(Assets.GetTexture("herothrowingdagger"), "moving", "moving", frameSize, EntityState.Moving, false).CreateAnimationFrames(0, 6, 50));
                    break;
                }

                case EntityType.ForestKnightSlash:
                {
                    frameSize = new Point(64, 64);
                    animationList.Add(new Animation(Assets.GetTexture("heroslashweak"), "moving", "moving", frameSize, EntityState.Moving, false).CreateAnimationFrames(0, 1, 100));
                    animationList.Add(new Animation(Assets.GetTexture("heroslashweakdying"), "dying", "dying", frameSize, EntityState.Dying, false).CreateAnimationFrames(0, 9, 50));
                    break;
                }
                case EntityType.ForestArcherArrow:
                {
                    frameSize = new Point(16, 16);
                    animationList.Add(new Animation(Assets.GetTexture("herothrowingdagger"), "moving", "moving", frameSize, EntityState.Moving, false).CreateAnimationFrames(0, 6, 50));
                    break;
                }
                case EntityType.ForestWolfBite:
                {
                    frameSize = new Point(32, 48);
                    animationList.Add(new Animation(Assets.GetTexture("forestWolfBite"), "moving", "moving", frameSize, EntityState.Moving, false).CreateAnimationFrames(0, 6, 100));
                    animationList.Add(new Animation(Assets.GetTexture("forestWolfBite"), "dying", "dying", frameSize, EntityState.Moving, false).CreateAnimationFrames(0, 1, 500, 7));
                    break;
                }
                #endregion

            }

            return animationList;
        }

    }
}
