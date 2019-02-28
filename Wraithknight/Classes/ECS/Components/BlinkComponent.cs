using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class BlinkComponent : BindableComponent
    {
        public InputComponent Input;

        public int MaxCharges;
        public int Charges;
        public bool HasChargeReady => Charges > 0;

        public TimerComponent Cooldown = new TimerComponent();
        public double CooldownMilliseconds;

        public int BlinkMovementSpeed;
        public double BlinkMovementDurationInMilliseconds;
        public TimerComponent MovementDurationTimer = new TimerComponent();
        public bool InvulnerableOnMovementBlink = true;
        public bool PreviousTimerOver;
        public Vector2 MovementDirection;
        public Coord2 MovementExitSpeed;

        //Extra damage, projectile speed etc

        public BlinkComponent(int maxCharges, double cooldown, int blinkMovementSpeed, double blinkMovementDurationInMilliseconds)
        {
            MaxCharges = maxCharges;
            Charges = MaxCharges;
            CooldownMilliseconds = cooldown;
            BlinkMovementSpeed = blinkMovementSpeed;
            BlinkMovementDurationInMilliseconds = blinkMovementDurationInMilliseconds;
        }

        public override void Activate()
        {
            if (Bindings.TryGetValue(typeof(InputComponent), out var binding))
            {
                Input = binding as InputComponent;
            }
        }
    }
}
