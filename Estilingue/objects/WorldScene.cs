using OpenTK;

namespace Estilingue.objects
{


    public class WorldScene : Scene
    {

        internal Cube ground;
        internal Cube cubo;

        public WorldScene(Game game) 
        : base(game)
        {

            ground = new(Vector3.Zero, Vector3.Zero, new(50, 1, 50));
            cubo = new(Vector3.Zero, Vector3.Zero, Vector3.One);
            
        }

        protected override void Initialize()
        {
            ground.TextureID = textures["opentksquare.png"];
            cubo.TextureID = textures["opentksquare.png"];
        }

        protected override void RenderProcess()
        {
            objects.Add(cubo);
            objects.Add(ground);
        }

        protected override void UpdateProcess()
        {

        }
    }
}
