using OpenTK;
using System;

namespace Estilingue.objects
{
    public class PlayerScene : Scene
    {
        public Player player;
        public TexturedCube weapon;
        public TexturedCube ground;
        //public ObjVolume ted;

        public PlayerScene(Game game)
            : base(game)
        {
            player = new(Vector3.Zero, Vector3.Zero, new Vector3(1,2,1), this.game);
            ground = new(Vector3.Zero, Vector3.Zero, new Vector3(10, 0, 10));
            weapon = new(player.Position, player.Rotation, new Vector3(1f, 0.3f, 0.3f));
            //ted = ObjVolume.LoadFromFile("d8.obj");

        }

        protected override void Initialize()
        {
            player.TextureID = textures["player.png"];
            weapon.TextureID = textures["opentksquare.png"];
            ground.TextureID = textures["opentksquare.png"];
            //ted.TextureID = textures["opentksquare.png"];

            objects.Add(weapon);
            objects.Add(player);
            objects.Add(ground);
            //objects.Add(ted);

            player.Rotation = Vector3.UnitY * MathF.PI;
        }

        protected override void UpdateProcess()
        {
            player.Update(game.delta, game.mouseSensitivity);

            Vector3 offset = Vector3.Zero;

            offset.X = MathF.Cos(player.Rotation.Y - (MathF.PI / 2));
            offset.Z = MathF.Sin(player.Rotation.Y - (MathF.PI / 2));


            weapon.Position = new(
                player.Position.X + offset.X,
                player.Position.Y,
                player.Position.Z - offset.Z
                );
            weapon.Rotation = player.Rotation;
        }

        protected override void RenderProcess()
        {
        }
    }
}