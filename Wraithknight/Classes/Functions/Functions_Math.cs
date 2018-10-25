using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    public static class Functions_Math
    {

        public static float Sqrt(float number)
        {
            throw new NotImplementedException();
        }

        public static float Pow2(float basenumber)
        {
            return basenumber * basenumber;
        }

        public static float PowPositive(float basenumber, float exponent)
        {
            if(exponent == 0)
            {
                return 1;
            }
            float result = basenumber;
            for(int i = 1; i<exponent; i++)
            {
                result *= basenumber;
            }
            return result;
        }

        public static float DegreeToRadian(float degree)
        {
            return (float) Math.PI * degree / 180;
        }
        public static double DegreeToRadian(double degree)
        {
            return Math.PI * degree / 180;
        }

        public static float RadianToDegree(float radian)
        {
            return  radian * (float) (180 / Math.PI);
        }
        public static double RadianToDegree(double radian)
        {
            return radian * (180 / Math.PI);
        }
    }
}
