using OpenTK;

namespace Estilingue
{
    public class Cube : Volume
    {
        public Cube(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
        {

            Position = position;
            Rotation = rotation;
            Scale = scale;


            VertCount = 8;
            IndiceCount = 36; // Two triangles (3 vertices) per face for all the six faces. 2 * 3 * 6
            ColorDataCount = 8;
        }
        public override Vector3[] GetVerts()
        {
            return new Vector3[] {new Vector3(-0.5f, -0.5f,  -0.5f),
                new Vector3(0.8f, -0.8f,  -0.8f),
                new Vector3(0.8f, 0.8f,  -0.8f),
                new Vector3(-0.8f, 0.8f,  -0.8f),
                new Vector3(-0.8f, -0.8f,  0.8f),
                new Vector3(0.8f, -0.8f,  0.8f),
                new Vector3(0.8f, 0.8f,  0.8f),
                new Vector3(-0.8f, 0.8f,  0.8f),
            };

        }
        public override int[] GetIndices(int offset)
        {
            int[] inds = new int[] {
                //left
                0, 2, 1,
                0, 3, 2,
                //back
                1, 2, 6,
                6, 5, 1,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //front
                0, 7, 3,
                0, 4, 7,
                //bottom
                0, 1, 5,
                0, 5, 4
            };

            if (offset != 0)
            {
                for (int i = 0; i < inds.Length; i++)
                {
                    inds[i] += offset;
                }
            }

            return inds;
        }

        public override Vector3[] GetColorData()
        {
            return new Vector3[]
            {
                new Vector3( 0.8f, 0.8f, 0.8f),
                new Vector3( 0.7f, 0.7f, 0.7f),
                new Vector3( 0.6f, 0.6f, 0.6f),
                new Vector3( 0.8f, 0.8f, 0.8f),
                new Vector3( 0.6f, 0.6f, 0.6f),
                new Vector3( 0.5f, 0.5f, 0.5f),
                new Vector3( 0.8f, 0.8f, 0.8f),
                new Vector3( 0.8f, 0.8f, 0.8f)
            };
        }

        public override void CalculateModelMatrix()
        {
            ModelMatrix =
                Matrix4.CreateScale(Scale) *
                Matrix4.CreateRotationX(Rotation.X) *
                Matrix4.CreateRotationY(Rotation.Y) *
                Matrix4.CreateRotationZ(Rotation.Z) *
                Matrix4.CreateTranslation(Position);
        }

    }
}
