using OpenTK;
using System.Collections.Generic;
using OpenTK.Input;
using System;
namespace Estilingue
{
    static class Input
    {
        private static List<Key> keysDown;
        private static List<Key> keysDownLast;
        private static List<MouseButton> buttonsDown;
        private static List<MouseButton> buttonsDownLast;

        public static void Initialize(GameWindow game)
        {
            keysDown = new();
            keysDownLast = new();
            buttonsDown = new();
            buttonsDownLast = new();

            game.KeyDown += Game_KeyDown;
            game.KeyUp += Game_KeyUp;
            game.MouseDown += Game_MouseDown;
            game.MouseUp += Game_MouseUp;
            game.MouseWheel += Game_MouseWheel;

        }

        private static void Game_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Console.WriteLine(e.DeltaPrecise); // vezes rolado
            Console.WriteLine(e.Value); // total desde que o programa foi aberto
        }

        private static void Game_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            while (keysDown.Contains(e.Key))
            {
                keysDown.Remove(e.Key);
            }
        }
        private static void Game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            while (!keysDown.Contains(e.Key))
            {
                keysDown.Add(e.Key);
            }
        }
        private static void Game_MouseUp(object sender, MouseButtonEventArgs e)
        {
            while (buttonsDown.Contains(e.Button))
            {
                buttonsDown.Remove(e.Button);
            }
        }
        private static void Game_MouseDown(object sender, MouseButtonEventArgs e)
        {
            while (!buttonsDown.Contains(e.Button))
            {
                buttonsDown.Add(e.Button);
            }
            Console.WriteLine("Button: " + e.Button);
        }

        public static void Update()
        {
            keysDownLast = new(keysDown);
            buttonsDownLast = new(buttonsDown);
        }

        /// <summary>
        /// Indica se a Tecla acaba de ser pressionada. Indicates whether the key has just been pressed
        /// </summary>
        /// <param name="key"> Uma tecla para verficar. A Key to check.</param>
        /// <returns> True se a tecla acaba de ser pressionada; caso contrário, False</returns>
        public static bool KeyPress(Key key)
        {
            return (keysDown.Contains(key) && !keysDownLast.Contains(key));
        }
        /// <summary>
        /// Indica se a Tecla acaba de ser solta. Indicates whether the key has just been released.        /// </summary>
        /// <param name="key">Uma tecla para verficar. A Key to check.</param>
        /// <returns></returns>
        public static bool KeyRelease(Key key)
        {
            return (!keysDown.Contains(key) && keysDownLast.Contains(key));
        }
        /// <summary>
        /// Indica se a tecla está atualmente pressionada.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyDown(Key key)
        {
            return (keysDown.Contains(key));
        }

        /// <summary>
        /// Indica se o Botão acaba de ser pressionada. Indicates whether the key has just been pressed
        /// </summary>
        /// <param name="key"> Um Botão para verficar. A Button to check.</param>
        /// <returns> True se a tecla acaba de ser pressionada; caso contrário, False</returns>
        public static bool MousePress(MouseButton button)
        {
            return (buttonsDown.Contains(button) && !buttonsDownLast.Contains(button));
        }
        /// <summary>
        /// Indica se um Botão acaba de ser solto.    
        /// </summary>
        /// <param name="button"></param>
        /// <returns>True se o botão acaba de ser solto</returns>
        public static bool MouseRelease(MouseButton button)
        {
            return (!buttonsDown.Contains(button) && buttonsDownLast.Contains(button));
        }
        /// <summary>
        /// Indica se um Botão esta atualmente pressionado.    
        /// </summary>
        /// <param name="button"></param>
        /// <returns>True se o botão estiver pressionado</returns>
        public static bool MouseDown(MouseButton button)
        {
            return (buttonsDown.Contains(button));
        }
        /// <summary>
        /// indica a posição do mouse em relação a tela OpenTk
        /// </summary>
        /// <returns></returns>
        public static Vector2 MousePosition()
        {
            return new(Mouse.GetState().X, Mouse.GetState().Y);
        }
    }

}
