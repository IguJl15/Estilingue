using OpenTK;
using System;

namespace Estilingue.objects
{
    public class FCamera : Camera
    {

        public FCamera(Volume target, GameWindow game) : base(target, game)
        {
        }
        public FCamera(TypeOfView typeOfView, Volume target, GameWindow game) : base(typeOfView, target, game)
        {
        }

        public override void Init()
        {
            this.Orientation = new(-PI / 4, MathF.Atan(1 / MathF.Sqrt(2)));
            this.DistanceToTarget = 10;
        }

        public override void Update()
        {
            


            DistanceToTarget -= Input.DeltaWheel() * 0.1f;

            if (DistanceToTarget < 2) DistanceToTarget = 2;
            if (DistanceToTarget > 10) DistanceToTarget = 10;

            position.X = Target.Position.X - DistanceToTarget * MathF.Cos(Orientation.Y) * MathF.Sin(Orientation.X);
            position.Y = DistanceToTarget * MathF.Sin(Orientation.Y);
            position.Z = Target.Position.Z - DistanceToTarget * MathF.Cos(Orientation.Y) * MathF.Cos(Orientation.X);

        }
    }
}