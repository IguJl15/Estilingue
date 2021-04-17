using OpenTK;

namespace Estilingue.objects
{
    public class Cube : Volume
    {
        public Vector3 color;

        public Cube(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
        {
            VertCount = 8;
            IndiceCount = 36; // Two triangles (3 vertices) per face for all the six faces. 2 * 3 * 6
            ColorDataCount = 8;
        }

        public override Vector3[] GetVerts()
        {
            return new[]{new(-0.5f, -0.5f,  -0.5f),
                new Vector3(0.5f, -0.5f,  -0.5f),
                new Vector3(0.5f, 0.5f,  -0.5f),
                new Vector3(-0.5f, 0.5f,  -0.5f), 
                new Vector3(-0.5f, -0.5f,  0.5f),
                new Vector3(0.5f, -0.5f,  0.5f),
                new Vector3(0.5f, 0.5f,  0.5f),
                new Vector3(-0.5f, 0.5f,  0.5f),
            };
        }

        public override int[] GetIndices(int offset)
        {
            int[] inds = new[]{
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
            return new[]{
                new Vector3(1.0f, 0.65f, 0.42f),
                new Vector3(0.9f, 0.65f, 0.3f),
                new Vector3(1.0f, 0.65f, 0.42f),
                new Vector3(1.0f, 0.55f, 0.3f),
                new Vector3(0.9f, 0.55f, 0.3f),
                new Vector3(1.0f, 0.65f, 0.42f),
                new Vector3(1.0f, 0.65f, 0.3f),
                new Vector3(0.9f, 0.55f, 0.42f)};
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

        public override Vector2[] GetTextureCoords()
        {
            return System.Array.Empty<Vector2>();
        }
    }
}