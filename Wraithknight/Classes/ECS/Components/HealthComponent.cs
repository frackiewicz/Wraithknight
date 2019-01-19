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
        public int ProcessedHealth; //TODO Replace calculations with this here so that the health system has more power over health

        public double InvincibilityTimeMilliseconds;
        public double RemainingInvincibilityTimeMilliseconds; //TODO Logic missing

        public bool IsDead => CurrentHealth <= 0;

        public HealthComponent(int maxHealth, int currentHealth = -1, double invincibilityTimeMilliseconds = 0)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth == -1 ? MaxHealth : currentHealth;
            ProcessedHealth = CurrentHealth;
            InvincibilityTimeMilliseconds = invincibilityTimeMilliseconds;
        }
    }
}
