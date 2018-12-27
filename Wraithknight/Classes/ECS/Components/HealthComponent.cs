using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    class HealthComponent : BindableComponent
    {
        public int MaxHealth;
        public int CurrentHealth;

        public bool IsDead => CurrentHealth <= 0;

        public HealthComponent(int maxHealth, int currentHealth = -1)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth == -1 ? MaxHealth : currentHealth;
        }
    }
}
