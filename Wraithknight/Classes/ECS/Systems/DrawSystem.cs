using System;
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

        public DrawSystem(Camera2D camera)
        {
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
                    DrawComponent(component);
                }
            }
        }

        private void DrawAnimatedComponents()
        {
            foreach (var component in _animatedDrawComponents)
            {
                DrawComponent(component);
            }
        }

        private void DrawComponent(DrawComponent component)
        {
            if (component.Inactive) return;

            UpdatePosition(component);
            if (component.DrawRec.Intersects(_camera.CullRec)) //TODO can confirm, this causes the pop ins
            {
                if (component.GetRotationFromMovementVector) RotateFromMovementVector(component);
                Functions_Draw.Draw(component);
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
            if(component.BoundPos != null) component.DrawRec.Center = component.BoundPos + component.Offset;
        }

        private static void RotateFromMovementVector(DrawComponent component)
        {
            if (component.Bindings.TryGetValue(typeof(MovementComponent), out var binding))
            {
                MovementComponent movement = binding as MovementComponent;
                if (!movement.IsMoving) return;
                component.Rotation = (float)movement.Speed.Polar.Angle;
            }
        }

        public override void Reset()
        {
            _sortedDrawComponents.Clear();
            _animatedDrawComponents.Clear();
        }

        
    }
}
