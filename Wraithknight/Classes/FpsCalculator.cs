using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class FpsCalculator //TODO Cleanup? Optimization?
    {
        //TODO Ringbuffer
        /*
         * Explanation:
         * CCreate a self repeating list,
         * you calculate the sum in a fixed variable
         * adding an element adds to the sum, removing the object at the iterator and substracting from the total sum
         */
        private readonly List<int> _fpsForCalculation;
        private readonly double _outputIntervallSeconds;
        private double _lastDisplayTimestamp;
        private int _lastDisplayfps;
        private double _lastUpdateTimestamp;

        public FpsCalculator(double intervall)
        {
            _fpsForCalculation = new List<int>();
            _outputIntervallSeconds = intervall;
            _lastDisplayTimestamp = 0;
        }

        public void Update(GameTime gameTime)
        {
            _lastUpdateTimestamp = gameTime.TotalGameTime.TotalSeconds;
            _fpsForCalculation.Add((int)(1/gameTime.ElapsedGameTime.TotalSeconds));
        }

        public int Getfps()
        {
            if (ReadyForOutput())
            {
                UpdateVariables();
            }
            return _lastDisplayfps;
        }

        private bool ReadyForOutput()
        {
            return _lastUpdateTimestamp - _lastDisplayTimestamp >= _outputIntervallSeconds;
        }

        private void UpdateVariables()
        {
            _lastDisplayTimestamp = _lastUpdateTimestamp;
            _lastDisplayfps = GetAverageFps();
        }

        private int GetAverageFps()
        {
            int result = 0;
            foreach (int value in _fpsForCalculation)
            {
                result += value;
            }
            result /= _fpsForCalculation.Count;
            _fpsForCalculation.Clear();
            return result;
        }
    }
}
