using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    class BlinkComponent : BindableComponent
    {
        public bool BlinkTrigger;
        public InputComponent Input;

        public int MaxCharges;
        public int Charges;

        public TimerComponent Cooldown = new TimerComponent();
        public double CooldownMilliseconds;

        public BlinkComponent(double cooldown)
        {
            CooldownMilliseconds = cooldown;
        }
    }
}
