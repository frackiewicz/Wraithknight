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
        TestingRoom,
        Generation
    }

    public enum Direction
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }

    public enum Allegiance
    {
        Friendly,
        Enemy,
        Neutral
    }
}
