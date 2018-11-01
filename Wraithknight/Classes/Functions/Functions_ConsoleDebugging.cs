using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    static class Functions_ConsoleDebugging //maybe useful in the future?
    {
        public static void DumpEntityInfo(Entity entity)
        {
            Console.Write("Entity-" + entity.ID + " ");
        }
    }
}
