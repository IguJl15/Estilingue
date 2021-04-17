using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace Estilingue.objects
{
    internal static class Input
    {
        private static List<Key> keysDown;
        private static List<Key> keysDownLast;
        private static List<MouseButton> buttonsDown;
        private static List<MouseButton> buttonsDownLast;
        private static Vector2 mousePosition;
        private static Vector2 mousePositionLast;
        private static float wheelCount;
        private static float deltaWheel;
        private static GameWindow game;
        private static bool mouseLock;
        public static List<Key> KeysDown { get => keysDown; set => keysDown = value; }
        public static GameWindow Game { set => game = value; }
        public static bool MouseLock { get => mouseLock; set => mouseLock = value; }

        public static void Initialize(GameWindow game)
        {
            Game = game;
            keysDown = new();
            keysDownLast = new();
            buttonsDown = new();
            buttonsDownLast = new();
            mousePositionLast = new();
            mouseLock = false;
            SetMousePosition(Vector2.Zero);
            Update();

            game.KeyDown += Game_KeyDown;
            game.KeyUp += Game_KeyUp;
            game.MouseDown += Game_MouseDown;
            game.MouseUp += Game_MouseUp;
            game.MouseWheel += Game_MouseWheel;
            game.FocusedChanged += Game_FocusedChanged;
            game.MouseMove += Game_MouseMove;
        }

        private static void Game_MouseMove(object sender, MouseMoveEventArgs e)
        {
            mousePosition = new(e.Position.X - game.ClientSize.Width / 2, -(e.Position.Y - game.ClientSize.Height / 2));
        }

        private static void Game_FocusedChanged(object sender, EventArgs e)
        {
            mousePositionLast = new(Mouse.GetState().X, Mouse.GetState().Y);
        }

        private static void Game_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            wheelCount = e.ValuePrecise;
            deltaWheel = e.DeltaPrecise;
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
        }

        public static void Update()
        {
            keysDownLast = new(keysDown);
            buttonsDownLast = new(buttonsDown);
            deltaWheel = 0.0f;

            if (game.Focused)
            {
                mousePositionLast = MousePosition();

                if (mouseLock)
                {
                    SetMousePosition(new(0f, 0f));
                }
            }
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

        public static bool KeyPress(String key)
        {
            Key lKey = Enum.Parse<Key>(key);
            return (keysDown.Contains(lKey) && !keysDownLast.Contains(lKey));
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

        public static bool MousePress(string button)
        {
            MouseButton lButton = Enum.Parse<MouseButton>(button);
            return (buttonsDown.Contains(lButton) && !buttonsDownLast.Contains(lButton));
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
            return mousePosition;
        }

        public static float WheelCount()
        {
            return wheelCount;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>Retorna quantas vezes a roda do mouse foi movida desde o ultimo frame update.</returns>
        public static float DeltaWheel()
        {
            return deltaWheel;
        }

        public static Vector2 DeltaMovement()
        {
            if (mouseLock)
            {
                return mousePosition;
            }
            else
            {
                return mousePosition - mousePositionLast;
            }
        }

        public static void SetMousePosition(Vector2 location)
        {
            Mouse.SetPosition(location.X + game.Location.X + game.ClientSize.Width / 2, location.Y + game.Location.Y + game.ClientSize.Height / 2);
        }
    }
}