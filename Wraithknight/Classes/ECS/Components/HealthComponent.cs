using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    class HealthComponent : Component
    {
        public int MaxHealth;
        public int CurrentHealth;
        public int MinHealth; //whats the point? idonfukinknow

        public HealthComponent(int maxHealth = 0, int currentHealth = 0, int minHealth = 0)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
            MinHealth = minHealth;
        }
    }
}
