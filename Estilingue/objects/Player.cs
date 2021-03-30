using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace Estilingue
{
    internal class Player : TexturedCube
    {
        GameWindow game;

        public Vector2 mouse = Vector2.Zero;
        public Vector3 inputVector = Vector3.Zero;

        public Vector3 velocity = Vector3.Zero;
        private float maxVelocity = 1;
        public float maxVelocityMod = 1.6f;

        private readonly float acceleration = 10;
        private readonly float friction = 10;

        public float mouseSensitivity = 0.002f;
        public Vector2 orientation = new(0f, 0f);


        public Player(Vector3 position, Vector3 rotation) : base(position, rotation, Vector3.One) { }

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
            Position += velocity;
        }
        private void processInput()
        {

            inputVector = Vector3.Zero;
            
            Action func;

            var keysList = new Dictionary<string, Action>()
            {
                {"W", func = delegate () { inputVector.Z--;} },
                {"S", func = delegate () { inputVector.Z++;} },
                {"A", func = delegate () { inputVector.X--;} },
                {"D", func = delegate () { inputVector.X++;} },
                {"ShiftLeft", func = delegate () { inputVector.Y--;} },
                {"Space", func = delegate () { inputVector.Y++;} }
            };

            foreach(Key key in Input.KeysDown)
            {
                string keyString = key.ToString();

                if (keysList.ContainsKey(keyString)) 
                {
                    keysList[keyString]();
                }
            }

            if (!Input.MouseDown(MouseButton.Left))
            {
                orientation = new(orientation.X - Input.DeltaMovement().X * mouseSensitivity, 0);
            }

            //orientation.X %= 2f * MathF.PI;
            Console.WriteLine(orientation.X);

            Rotation = new(0, orientation.X + MathF.PI / 2, 0);

            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3(MathF.Sin(orientation.X), 0, MathF.Cos(orientation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += inputVector.X * right;
            offset += -inputVector.Z * forward;
            offset.Y += inputVector.Y;

            inputVector = offset;

            inputVector.NormalizeFast();

        }
    }
}