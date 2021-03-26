using OpenTK;

namespace Estilingue
{
    public abstract class Volume
    {
        private Vector3 position = Vector3.Zero;
        private Vector3 rotation = Vector3.Zero;
        private Vector3 scale = Vector3.One;
        private Vector3 color = new(0.8f, 0.8f, 0.8f);

        private int vertCount;
        private int indiceCount;
        private int colorDataCount;
        private Matrix4 modelMatrix = Matrix4.Identity;
        private Matrix4 viewProjectionMatrix = Matrix4.Identity;
        public Matrix4 modelViewProjectionMatrix = Matrix4.Identity;

        public Vector3 Position { get => position; set => position = value; }
        public Vector3 Rotation { get => rotation; set => rotation = value; }
        public Vector3 Scale { get => scale; set => scale = value; }
        public int VertCount { get => vertCount; set => vertCount = value; }
        public int IndiceCount { get => indiceCount; set => indiceCount = value; }
        public int ColorDataCount { get => colorDataCount; set => colorDataCount = value; }
        public Matrix4 ModelMatrix { get => modelMatrix; set => modelMatrix = value; }
        public Matrix4 ViewProjectionMatrix { get => viewProjectionMatrix; set => viewProjectionMatrix = value; }
        public Matrix4 ModelViewProjectionMatrix { get => modelViewProjectionMatrix; set => modelViewProjectionMatrix = value; }

        protected Volume(Vector3 position, Vector3 rotation, Vector3 scale, Vector3 color)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.color = color;
        }

        public abstract Vector3[] GetVerts();

        public abstract int[] GetIndices(int offset = 0);

        public abstract Vector3[] GetColorData();

        public abstract void CalculateModelMatrix();
    }
}