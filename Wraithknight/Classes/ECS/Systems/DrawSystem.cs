﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class DrawSystem : System
    {

        private readonly Dictionary<Texture2D, HashSet<DrawComponent>> _sortedDrawComponents = new Dictionary<Texture2D, HashSet<DrawComponent>>();
        private readonly HashSet<DrawComponent> _animatedDrawComponents = new HashSet<DrawComponent>();
        private readonly Camera2D _camera;
        private readonly DrawAnimationSubsystem _animationSubsystem = new DrawAnimationSubsystem();

        public DrawSystem(ECS ecs, Camera2D camera) : base(ecs)
        {
            _ecs = ecs;
            _camera = camera;
        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.Components.TryGetValue(typeof(DrawComponent), out var component))
                {
                    DrawComponent drawComponent = component as DrawComponent;

                    if (drawComponent.Bindings.TryGetValue(typeof(AnimationComponent), out var animationComponent))
                    {
                        drawComponent.IsAnimated = true;
                        _animationSubsystem.RegisterAnimationComponent(animationComponent as AnimationComponent);
                        _animatedDrawComponents.Add(drawComponent);
                    }
                    else
                    {
                        InsertIntoBatch(drawComponent);
                    }
                    component.Activate();
                }
            }
        }

        public override void Update(GameTime gameTime)  
        {

        }

        public void Draw()
        {
            DrawBatches();
            DrawAnimatedComponents();
        }

        private void DrawBatches()
        {
            foreach (var batch in _sortedDrawComponents.Values)
            {
                foreach (var component in batch)
                {
                    if (component.Inactive) continue;
                    if (component.DrawRec.Intersects(_camera.CullRec)) //isVisible
                    {
                        UpdatePosition(component);
                        Functions_Draw.Draw(component);
                    }
                }
            }
        }

        private void DrawAnimatedComponents()
        {
            foreach (var component in _animatedDrawComponents)
            {
                if (component.Inactive) continue;
                if (component.DrawRec.Intersects(_camera.CullRec)) //isVisible
                {
                    UpdatePosition(component);
                    Functions_Draw.Draw(component);
                }
            }
        }

        private void ReorganizeIntoBatch(DrawComponent component)
        {
            RemoveFromBatch(component);
            InsertIntoBatch(component);
        }

        private void InsertIntoBatch(DrawComponent component)
        {
            if (!_sortedDrawComponents.ContainsKey(component.Texture)) _sortedDrawComponents.Add(component.Texture, new HashSet<DrawComponent>());
            _sortedDrawComponents[component.Texture].Add(component);
        }

        private void RemoveFromBatch(DrawComponent component)
        {
            if (_sortedDrawComponents.ContainsKey(component.Texture)) _sortedDrawComponents[component.Texture].Remove(component);
        }

        private void UpdatePosition(DrawComponent component)
        {
            if(component.SourcePos != null) component.DrawRec.Center = component.SourcePos + component.Offset;
        }

        public override void Reset()
        {
            _sortedDrawComponents.Clear();
            _animatedDrawComponents.Clear();
        }

        
    }
}
