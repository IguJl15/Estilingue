using OpenTK;
using System;

namespace Estilingue.objects
{
    public class TCamera : Camera
    {
        public TCamera(Volume target, GameWindow game) : base(target, game) {}
        public TCamera(TypeOfView typeOfView, Volume target, GameWindow game) : base(typeOfView, target, game) {}
        public override void Init()
        {
        }
        public override void Update()
        {
            // CHANGE THE FIELD OF VIEW:
            //fieldOfView -= Input.DeltaWheel() * 0.1f;
            //if (fieldOfView < 0.2f) fieldOfView = 0.2f;
            //if (fieldOfView > 2) fieldOfView = 2;

            DistanceToTarget -= Input.DeltaWheel() * 0.1f;

            if (DistanceToTarget < 2) DistanceToTarget = 2;
            if (DistanceToTarget > 10) DistanceToTarget = 10;

            if (Input.MouseDown(OpenTK.Input.MouseButton.Left))
            {
                orientation.X -= Input.DeltaMovement().X * MouseSensitivity;
                orientation.Y -= Input.DeltaMovement().Y * MouseSensitivity;
                if (orientation.Y < -PI / 2) orientation.Y = -(PI / 2f);
            }
            else
            {
                Orientation = Vector2.Lerp(Orientation, new(Target.Rotation.Y - PI / 2, orientation.Y), 0.25f);
                orientation.Y -= Input.DeltaMovement().Y * MouseSensitivity;
                if (orientation.Y < 0) orientation.Y = 0;
            }

            if (orientation.Y > PI / 2) orientation.Y = PI / 2 - MouseSensitivity;
            //orientation.X %= 2 * PI;

            offSet.X = HorizontalDistance() * MathF.Sin(orientation.X + Target.Rotation.X);
            offSet.Z = HorizontalDistance() * MathF.Cos(orientation.X + Target.Rotation.X);

            position.X = Target.Position.X - offSet.X;
            position.Y = VerticalDistance();
            position.Z = Target.Position.Z - offSet.Z;
        }
        private float HorizontalDistance()
        {
            return DistanceToTarget * MathF.Cos(orientation.Y);
        }
        private float VerticalDistance()
        {
            return DistanceToTarget * MathF.Sin(orientation.Y);
        }

    }
}