using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    public abstract class Screen
    {
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void HandleInput(GameTime gameTime) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
    }
}
