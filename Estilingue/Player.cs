using OpenTK;
using OpenTK.Input;

namespace Estilingue
{
    internal class Player : Cube
    {
        public Vector2 mouse = Vector2.Zero;
        public Vector3 inputVector = Vector3.Zero;
        public Vector3 velocity = Vector3.Zero;
        public Vector3 gravity = new Vector3(0f, 0.1f, 0f);
        private readonly float friction = 10;
        private float maxVelocity = 5;
        private readonly float acceleration = 1;
        public float mouseSensitivity = 0.03f;
        public Player(Vector3 position, Vector3 rotation, Vector3 color) : base(position, rotation, Vector3.One, color)
        {
        }

        public void Update(float delta)
        {
            inputVector = Vector3.Zero;
           
            if (Input.KeyDown(Key.Q))
            {
                inputVector.Y--;
            }
            if (Input.KeyDown(Key.E))
            {
                inputVector.Y++;
            }
            if (Input.KeyDown(Key.D))
            {
                inputVector.X++;
            }
            if (Input.KeyDown(Key.A))
            {
                inputVector.X--;
            }
            if (Input.KeyDown(Key.S))
            {
                inputVector.Z++;
            }
            if (Input.KeyDown(Key.W))
            {
                inputVector.Z--;
            }
            if (Input.KeyPress(Key.ShiftLeft))
            {
                maxVelocity *= 1.6f;
            }
            if (Input.KeyRelease(Key.ShiftLeft))
            {
                maxVelocity /= 1.6f;
            }

            inputVector.NormalizeFast();

            if (inputVector != new Vector3(0f, 0f, 0f))
            {
                velocity = Vector3.Lerp(velocity, //from
                                        inputVector * maxVelocity / 10, //from
                                        delta * acceleration); //step %
            }
            else
            {
                velocity = Vector3.Lerp(velocity, // from
                                        Vector3.Zero, // to
                                        delta * friction); // step %
            }
            
            if (!Input.MouseDown(MouseButton.Left))
            {
                Rotation = new(0f, Rotation.Y + Input.DeltaMovement().X * mouseSensitivity, 0);
            }
            //if (Position.Y > 0.0f)
            //{
            //    velocity -= gravity;
            //}

            Position += new Vector3(velocity.X, velocity.Y, velocity.Z);
        }
    }
}