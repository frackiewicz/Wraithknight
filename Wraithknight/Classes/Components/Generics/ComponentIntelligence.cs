using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    public class ComponentIntelligence
    {
        public AiType AiType;
        public Boolean SkipIntervall = false; //If true, skip interval checking : Application in tracking?

        public GameTime LastUpdate;
        public GameTime UpdateIntervallInMs;

        public Rectangle SightRectangle;
        public Vector2 MoveTarget;
        public Vector2 AttackTarget; //chasing?

    }
}
