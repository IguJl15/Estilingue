using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace Estilingue
{
    internal class Player : TexturedCube
    {
        public Vector2 mouse = Vector2.Zero;
        public Vector3 inputVector = Vector3.Zero;
        public Vector3 velocity = Vector3.Zero;
        public Vector3 gravity = new Vector3(0f, 0.1f, 0f);
        private readonly float friction = 10;
        private float maxVelocity = 5;
        private readonly float acceleration = 1;
        public Player(Vector3 position, Vector3 rotation) : base(position, rotation, Vector3.One)
        {
        }

        public void Update(float delta, float mouseSensitivity)
        {
            processInput();

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
                Rotation = new(0f, Rotation.Y - Input.DeltaMovement().X * mouseSensitivity, 0);
            }
            //if (Position.Y > 0.0f)
            //{
            //    velocity -= gravity;
            //}


            Position += new Vector3(velocity.X, velocity.Y, velocity.Z);
        }



        private void processInput()
        {

            inputVector = Vector3.Zero;
            
            Action func;

            var keysList = new Dictionary<string, Action>()
            {
                {"W", func = delegate () { inputVector.Z--; }},
                {"S", func = delegate () { inputVector.Z++; }},
                {"A", func = delegate () { inputVector.X--; }},
                {"D", func = delegate () { inputVector.X++; }},
                {"Q", func = delegate () { inputVector.Y--; }},
                {"E", func = delegate () { inputVector.Y++; }}
            };

            foreach(Key key in Input.KeysDown)
            {
                string keyString = key.ToString();

                if (keysList.ContainsKey(keyString)) 
                {
                    keysList[keyString]();
                }
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

        }
    }
}