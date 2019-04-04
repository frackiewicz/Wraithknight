using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    public static class Flags
    {
        // ****************************************************************** //
        public static Boolean Release = false;
        // ****************************************************************** //
        // Descriptors
        public static float Version = 0.01f;
        public static BootRoutine BootRoutine = BootRoutine.TestingRoom;


        // Dev flags
        public static Boolean ShowDrawRecs = false;
        public static Boolean ShowCollisionRecs = false;
        public static Boolean ShowDebuggingText = false;
        public static Boolean ShowMovementCenters = false;

        public static Boolean Debug
        {
            get => ShowDrawRecs || ShowCollisionRecs || ShowDebuggingText || ShowMovementCenters;
        }
        
        // Logic Flags
        public static bool FpsBelowThreshold;

        // Option flags

        // Cheat flags

    }
}
