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
        public virtual Screen LoadContent() { return this; }
        public virtual Screen UnloadContent() { return this; }
        public virtual Screen HandleInput(GameTime gameTime) { return this; }
        public virtual Screen Update(GameTime gameTime) { return this; }
        public virtual Screen Draw(GameTime gameTime) { return this; }
    }
}
