using OpenTK;
using OpenTK.Input;
using System;

namespace Estilingue
{
    class Camera
    {
        public Vector3 position = Vector3.Zero;
        public Vector2 orientation = new((float)Math.PI,
                                        0f);
        public Vector3 velocity = Vector3.Zero;
        public float moveSpeed = 0.2f;
        public float mouseSensitivity = 0.0025f;
        public Vector2 lastMouse = new();
        public Vector3 offSet = new();

        public Vector3 distanceToTarget = new(0f, 1f, (float)(2 * Math.Cos(Math.PI / 6)));

        public Matrix4 GetThirdPersonViewMatrix(Vector3 target)
        {
            Vector3 lookAt = new();

            lookAt.X = (float)(Math.Sin((float)orientation.X) * Math.Cos((float)orientation.Y));
            lookAt.Y = (float)Math.Sin((float)orientation.Y);
            lookAt.Z = (float)(Math.Cos((float)orientation.X) * Math.Cos((float)orientation.Y));

            //position = target + distanceToTarget;

            return Matrix4.LookAt(position, target, Vector3.UnitY);

        }
        public Matrix4 GetFirstPersonViewMatrix()
        {
            Vector3 lookAt = new();

            lookAt.X = (float)(Math.Sin((float)orientation.X) * Math.Cos((float)orientation.Y));
            lookAt.Y = (float)Math.Sin((float)orientation.Y);
            lookAt.Z = (float)(Math.Cos((float)orientation.X) * Math.Cos((float)orientation.Y));
            return Matrix4.LookAt(position, position + lookAt, Vector3.UnitY);
        }

        public void Move(float x, float y, float z)
        {
            offSet = new();

            Vector3 forward =   new((float)Math.Sin((float)orientation.X),
                                0,
                                (float)Math.Cos((float)orientation.X));
            Vector3 right = new(-forward.Z, 0, forward.X);

            offSet += x * right;
            offSet += z * forward;
            offSet.Y += y;
            offSet.NormalizeFast();
            offSet = Vector3.Multiply(offSet, moveSpeed);
            position += offSet;
            //Console.WriteLine("OffSet: " + offSet);
        }

        public void AddRotation(float x, float y)
        {
            x = x * mouseSensitivity;
            y = y * mouseSensitivity;

            orientation.X = (orientation.X + x) % ((float)Math.PI * 2.0f);
            orientation.Y = Math.Max(Math.Min(orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
        }

        public void Initialize(GameWindow game)
        {
            lastMouse = Input.MousePosition();
            game.CursorVisible = false;
            game.FocusedChanged += Game_FocusedChanged;
        }

        private void Game_FocusedChanged(object sender, EventArgs e)
        {
            lastMouse = Input.MousePosition();
        }

        public void Update(GameWindow game)
        {


            /** Controls to FPS mode:
            if (Input.KeyDown(Key.W))
            {
                this.Move(0f, 0.0f, 0.1f);
            }

            if (Input.KeyDown(Key.S))
            {
                this.Move(0f, 0f, -0.1f);
            }

            if (Input.KeyDown(Key.A))
            {
                this.Move(-0.1f, 0f, 0f);
            }

            if (Input.KeyDown(Key.D))
            {
                this.Move(0.1f, 0f, 0f);
            }

            if (Input.KeyDown(Key.Q))
            {
                this.Move(0f, -0.1f, 0f);
            }

            if (Input.KeyDown(Key.E))
            {
                this.Move(0f, 0.1f, 0f);
            }

            if (game.Focused)
            {
                Vector2 delta =  lastMouse - Input.MousePosition();
                AddRotation(delta.X, delta.Y);
                lastMouse = Input.MousePosition();
            }
            */

            if (orientation.X > 2* Math.PI)
            {
                orientation.X = 0;
            }
            if (orientation.Y > 2 * Math.PI)
            {
                orientation.Y = 0;
            }

            //Console.WriteLine("Visão\nX: {0}\nY: {1}", orientation.X * 180/Math.PI, orientation.Y * 180 / Math.PI);

        }
    }
}
