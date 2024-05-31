using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.ConsoleMenu
{
    class ConsoleMenu
    {
        public static float _oldSpeed   = 0;
        public static float _oldHealth  = 0;
        public static float _oldStamina = 0;

        public static float _newSpeed = 10;

        public static bool _increaseSpeed   = false;
        public static bool _infiniteHealth  = false;
        public static bool _infiniteStamina = false;

        public static void ParseCommand(string[] command)
        {
            if (command[0] == "speed")
            {
                var speed = float.Parse(command[1]);
                SetSpeed(speed);
            }
            else if (command[0] == "stamina")
            {
                var stamina = bool.Parse(command[1]);
                ToggleInfiniteStamina(stamina);
            }
            else if (command[0] == "health")
            {
                var health = bool.Parse(command[1]);
                ToggleInfiniteHealth(health);
            }
        }

        private static void SetSpeed(float speed)
        {
            _newSpeed = speed;
        }

        private static void ToggleInfiniteHealth(bool health)
        {
            _infiniteHealth = health;
        }

        private static void ToggleInfiniteStamina(bool stamina)
        {
            _infiniteStamina = stamina;
        }
    }
}