using OpenTK;

namespace Estilingue
{
    public abstract class Volume
    {
        private Vector3 position = Vector3.Zero;
        private Vector3 rotation = Vector3.Zero;
        private Vector3 scale = Vector3.One;

        private int vertCount;
        private int indiceCount;
        private int colorDataCount;
        private int textureCoordsCount;
        private Matrix4 modelMatrix = Matrix4.Identity;
        private Matrix4 viewProjectionMatrix = Matrix4.Identity;
        public Matrix4 modelViewProjectionMatrix = Matrix4.Identity;
        private bool isTextured = false;
        private int textureID;

        public Vector3 Position { get => position; set => position = value; }
        public Vector3 Rotation { get => rotation; set => rotation = value; }
        public Vector3 Scale { get => scale; set => scale = value; }
        public int VertCount { get => vertCount; set => vertCount = value; }
        public int IndiceCount { get => indiceCount; set => indiceCount = value; }
        public int ColorDataCount { get => colorDataCount; set => colorDataCount = value; }
        public int TextureCoordsCount { get => textureCoordsCount; set => textureCoordsCount = value; }
        public Matrix4 ModelMatrix { get => modelMatrix; set => modelMatrix = value; }
        public Matrix4 ViewProjectionMatrix { get => viewProjectionMatrix; set => viewProjectionMatrix = value; }
        public Matrix4 ModelViewProjectionMatrix { get => modelViewProjectionMatrix; set => modelViewProjectionMatrix = value; }
        public bool IsTextured { get => isTextured; set => isTextured = value; }
        public int TextureID { get => textureID; set => textureID = value; }

        protected Volume(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public abstract Vector3[] GetVerts();

        public abstract int[] GetIndices(int offset = 0);

        public abstract Vector3[] GetColorData();

        public abstract Vector2[] GetTextureCoords();

        public abstract void CalculateModelMatrix();
    }
}