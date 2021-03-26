using OpenTK;
using System;

namespace Estilingue
{
    internal class TCamera
    {

        private Player player;

        public Vector3 position = Vector3.Zero;
        public Vector2 orientation = new(MathF.PI, 0f);
        public Vector3 offSet = new();
        public float mouseSensitivity = 0.0025f;
        public float distanceToTarget = 5;

        public TCamera(Player player)
        {
            this.player = player;
        }

        public Matrix4 GetThirdPersonViewMatrix()
        {
            return Matrix4.LookAt(position, player.Position, Vector3.UnitY);
        }
        private float HorizontalDistance()
        {
            return distanceToTarget * MathF.Cos(orientation.Y);
        }
        private float VerticalDistance()
        {
            return distanceToTarget * MathF.Sin(orientation.Y);
        }

        public void Update(GameWindow game)
        {
            distanceToTarget = distanceToTarget - Input.DeltaWheel() * 0.1f;

            if (Input.MouseDown(OpenTK.Input.MouseButton.Left))
            {
                orientation.X -= Input.DeltaMovement().X * mouseSensitivity;
                orientation.Y += Input.DeltaMovement().Y * mouseSensitivity;
            }
            

            offSet.X = HorizontalDistance() * MathF.Sin(orientation.X + player.Rotation.X);
            offSet.Z = HorizontalDistance() * MathF.Cos(orientation.X + player.Rotation.X);

            position.X = player.Position.X - offSet.X;
            position.Y = player.Position.Y + VerticalDistance();
            position.Z = player.Position.Z - offSet.Z;
            //Console.WriteLine(Input.DeltaMovement());
            //Console.WriteLine(orientation);

            /**
            * if (Input.KeyDown(Key.Up))
            * {
            *     this.Move(0f, 0.0f, 0.1f);
            * }
            *
            * else if (Input.KeyDown(Key.Down))
            * {
            *     this.Move(0f, 0f, -0.1f);
            * }
            *
            * else if (Input.KeyDown(Key.Left))
            * {
            *     this.Move(-0.1f, 0f, 0f);
            * }
            *
            * else if (Input.KeyDown(Key.Right))
            * {
            *     this.Move(0.1f, 0f, 0f);
            * }
            * else
            * {
            *     this.Move(0f, 0f, 0f);
            * }
            * if (Input.KeyDown(Key.Q))
            * {
            *     this.Move(0f, -0.1f, 0f);
            * }
            *
            * if (Input.KeyDown(Key.E))
            * {
            *     this.Move(0f, 0.1f, 0f);
            * }
            * if (game.Focused)
            * {
            *     Vector2 delta =  lastMouse - Input.MousePosition();
            *     AddRotation(delta.X, delta.Y);
            *     lastMouse = Input.MousePosition();
            * }

            if (orientation.X > 2 * Math.PI)
            {
                orientation.X = 0;
            }
            if (orientation.Y > 2 * Math.PI)
            {
                orientation.Y = 0;
            }

            * */
        }
    }
}