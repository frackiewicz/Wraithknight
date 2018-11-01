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

        public HealthComponent(int maxHealth = 0, int currentHealth = 0)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
        }
    }
}
