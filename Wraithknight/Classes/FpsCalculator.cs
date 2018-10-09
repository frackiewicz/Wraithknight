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

        /// <summary>
        /// Call this Method on every Drawcall
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _lastUpdateTimestamp = gameTime.TotalGameTime.TotalSeconds;
            _fpsForCalculation.Add((int)(1/gameTime.ElapsedGameTime.TotalSeconds));
        }

        /// <summary>
        /// Use this to output the calculated FramesPerSecond
        /// </summary>
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
