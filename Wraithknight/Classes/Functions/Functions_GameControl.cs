using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    static class Functions_GameControl
    {
        private static GameBase _gameBase;

        public static void ConnectGameBase(GameBase gameBase)
        {
            _gameBase = gameBase;
        }

        public static void ExitGame()
        {
           _gameBase.Exit();
        }

        public static void SetFixedTimeStep(bool value)
        {
            _gameBase.IsFixedTimeStep = value;
        }
    }
}
