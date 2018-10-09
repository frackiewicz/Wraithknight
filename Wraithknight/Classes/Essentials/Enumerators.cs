using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    public enum BootRoutine
    {
        Game,
        TestingRoom
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum AiType
    {
        Simple
    }

    public enum ActorType //HeroSwordsman , ForestWolf   <-- seperate with prefixes
    {
        Empty,
        test
    }
}
