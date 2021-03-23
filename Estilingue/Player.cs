using OpenTK;
using OpenTK.Input;
using System;

namespace Estilingue
{
    class Player : Cube
    {

        public Vector2 mouse = Vector2.Zero;
        public Vector3 inputVector = Vector3.Zero;
        public Vector3 velocity = Vector3.Zero;
        public Vector3 gravity = new Vector3(0f, 0.1f, 0f);
        readonly float friction = 10;
        readonly float maxVelocity = 5;
        readonly float acceleration = 1;

        public Player(Vector3 position,Vector3 rotation) : base(position, rotation, Vector3.One)
        {
        }

        public void Update(float delta)
        {
            inputVector = Vector3.Zero;
            if (Input.KeyPress(Key.E))
            {
                velocity.Y = maxVelocity;
            }
            if (Input.KeyDown(Key.D) | Input.KeyDown(Key.Right))
            {
                inputVector.X++;
            }
            if (Input.KeyDown(Key.A) | Input.KeyDown(Key.Left))
            {
                inputVector.X--;
            }
            if (Input.KeyDown(Key.S) | Input.KeyDown(Key.Down))
            {
                inputVector.Z++;
            }
            if (Input.KeyDown(Key.W) | Input.KeyDown(Key.Up))
            {
                inputVector.Z--;
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
            if (Position.Y > 0.0f)
            {
                velocity -= gravity;
            }
            Console.WriteLine(velocity);

            Position += new Vector3(velocity.X, velocity.Y, velocity.Z);
        }
    }
}
