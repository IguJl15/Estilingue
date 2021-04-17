using OpenTK;
using System;

namespace Estilingue.objects
{
    public abstract class Camera
    {
        internal Volume target;
        internal GameWindow game;

        internal TypeOfView typeOfView;

        internal Vector3 position;
        internal Vector3 offSet;
        internal Vector2 orientation;
        private float distanceToTarget;
        private float fieldOfView;

        internal float mouseSensitivity = 0.0025f;

        public const float PI = MathF.PI;

        public Volume Target => target;

        public GameWindow Game => game;

        public TypeOfView TypeOfView { get => typeOfView; set => typeOfView = value; }
        public Vector3 Position { get => position; set => position = value; }
        public Vector2 Orientation { get => orientation; set => orientation = value; }
        public Vector3 OffSet { get => offSet; set => offSet = value; }
        public float DistanceToTarget { get => distanceToTarget; set => distanceToTarget = value; }
        public float FieldOfView { get => fieldOfView; set => fieldOfView = value; }
        public float MouseSensitivity { get => mouseSensitivity; set => mouseSensitivity = value; }

        public Camera(Volume target, GameWindow game)
        {
            this.target = target;
            this.game = game;
        }
        public Camera(TypeOfView typeOfView, Volume target, GameWindow game)
        {
            this.TypeOfView = typeOfView;
            this.target = target;
            this.game = game;
        }

        public virtual Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Target.Position, Vector3.UnitY);
        }

        public virtual Matrix4 CreateFieldOfView()
        {
            Matrix4 matrix = new();
            switch (TypeOfView)
            {
                case TypeOfView.Projection:
                    matrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, (float)Game.Width / (float)Game.Height, 0.1f, 80.0f);
                    break;
                
                case TypeOfView.Orthographic:
                    matrix = Matrix4.CreateOrthographic((float)Game.Width / 80, (float)Game.Height / 80, 0.1f, 80.0f);
                    break;
                default: // projection view
                    matrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, (float)Game.Width / (float)Game.Height, 0.1f, 80.0f);
                    break;
            }

            return matrix;
        }

        public abstract void Init();
        public abstract void Update();
    }
    
    public enum TypeOfView
    {
        Projection = 0,
        Orthographic = 1
    }
}
