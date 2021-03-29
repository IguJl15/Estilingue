using OpenTK;
using System;

namespace Estilingue
{
    internal class TCamera
    {

        private readonly Player player;

        public Vector3 position = Vector3.Zero;
        public Vector2 orientation = new((3 * MathF.PI) / 2, 0f);
        public Vector3 offSet = new();
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

        public void Update(float mouseSensitivity)
        {
            distanceToTarget -= Input.DeltaWheel() * 0.1f;

            if (Input.MouseDown(OpenTK.Input.MouseButton.Left))
            {
                orientation.X -= Input.DeltaMovement
                    
                    
                    
         ().X * mouseSensitivity;
                orientation.Y += Input.DeltaMovement().Y * mouseSensitivity;
            }
            

            offSet.X = HorizontalDistance() * MathF.Sin(orientation.X + player.Rotation.X);
            offSet.Z = HorizontalDistance() * MathF.Cos(orientation.X + player.Rotation.X);

            position.X = player.Position.X - offSet.X;
            position.Y = player.Position.Y + VerticalDistance();
            position.Z = player.Position.Z - offSet.Z;
        }
    }
}