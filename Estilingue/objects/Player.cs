using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace Estilingue.objects
{
    public class Player : TexturedCube
    {
        public GameWindow game;

        public Vector3 inputVector = Vector3.Zero;

        public Vector3 velocity = Vector3.Zero;
        private const float maxVelocity = 1;
        public const float maxVelocityMod = 1.6f;

        private readonly float acceleration = 10;
        private readonly float friction = 10;

        public float mouseSensitivity = 0.002f;

        public float facinAngle = (7 * MathF.PI) / 4f;

        public Player(Vector3 position, Vector3 rotation, Vector3 scale, GameWindow game)
            : base(position, rotation, scale)
        {

        }

        public void Update(float delta, float mouseSensitivity)
        {
            this.mouseSensitivity = mouseSensitivity;
            ProcessInput();

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

            if (velocity.LengthFast < 0.00001) velocity = Vector3.Zero;


            if (Position.Y <= 1)
            {
                velocity.Y = 0;
                Position = new Vector3(Position.X, 1, Position.Z);
            }

            Position += velocity;

            if (velocity.Normalized().Z >= 0)
            {
                Rotation = new Vector3(Rotation.X, -Vector3.CalculateAngle(Vector3.UnitX, velocity), Rotation.Z);
            }
            else if (velocity.Normalized().Z < 0)
            {
                Rotation = new Vector3(Rotation.X, Vector3.CalculateAngle(Vector3.UnitX, velocity), Rotation.Z);
            }

        }

        private void ProcessInput()
        {
            inputVector = Vector3.Zero;

            Action func;

            var keysList = new Dictionary<string, Action>()
            {
                {"W", func = delegate () { inputVector.Z--;} },
                {"S", func = delegate () { inputVector.Z++;} },
                {"A", func = delegate () { inputVector.X--;} },
                {"D", func = delegate () { inputVector.X++;} },
                //{"Space", func = delegate () { inputVector.Y = maxVelocity;} },
                {"Space", func = delegate () { inputVector.Y++;} },
                {"ShiftLeft", func = delegate () { inputVector.Y--;} }
            };

            foreach (Key key in Input.KeysDown)
            {
                string keyString = key.ToString();

                if (keysList.ContainsKey(keyString))
                {
                    keysList[keyString]();
                }
            }

            if (Input.MouseDown(MouseButton.Left))
            {
                //Rotation -= new Vector3(0f, Input.DeltaMovement().X * game.mouseSensitivity, 0f);
            }

            Vector3 offset = Vector3.Zero;

            Vector3 forward = new(MathF.Sin(facinAngle), 0, MathF.Cos(facinAngle));
            Vector3 right = new(-forward.Z, 0, forward.X);

            offset += inputVector.X * right;
            offset += -inputVector.Z * forward;
            offset.Y += inputVector.Y;

            inputVector = offset;

            inputVector.NormalizeFast();
        }
    }
}