using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    public static class Functions_Operators
    {
        public static T CastComponent<T>(Component component)
        {
            try
            {
                return (T)Convert.ChangeType(component, typeof(T));
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Wrong cast: " + component.GetType() + " to " + typeof(T));
                return default(T);
            }
        }

        public static T CastSystem<T>(System system)
        {
            try
            {
                return (T)Convert.ChangeType(system, typeof(T));
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Wrong cast: " + system.GetType() + " to " + typeof(T));
                return default(T);
            }
        }
    }
}
