using Estilingue.objects;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
namespace Estilingue
{
    public class Game : GameWindow
    {

        public float mouseSensitivity = 0.002f;

        private bool pause = false;

        public float FPS;
        public float delta;

        public PlayerScene playerScene;
        public FCamera camera;

        public Game()
            : base(1280, 720, new GraphicsMode(32, 24, 0, 2), "Bem-Vindo!") { }

        private void InitProgram()
        {
            playerScene = new(this);
            camera = new(TypeOfView.Orthographic, playerScene.player, this);
            
            playerScene.Init();
            camera.Init();
            Input.Initialize(this);
        }

        protected override void OnLoad(EventArgs _)
        {
            Log();

            GL.ClearColor(Color.SteelBlue);
            GL.PointSize(size: 5);
            InitProgram();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            playerScene.Render();
            //worldScene.Render();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            delta = (float)e.Time;

            FPS++;

            if (FPS >= 60)
            {
                Title = string.Format("FPS: " + (int)(1d / e.Time));
                FPS = 0f;
            }

            if (pause) { }
            else
            {
                UpdatesProcess((float)e.Time);
            }

            if (Input.KeyPress("Escape") && CursorVisible)
            {
                Close();
            }
            else if (Input.KeyPress("Escape") && !CursorVisible)
            {
                CursorVisible = true;
                pause = true;
                Input.MouseLock = false;
                Log();
            }
            if (Input.MousePress("Left") && CursorVisible && pause)
            {
                CursorVisible = false;
                pause = false;
                Input.MouseLock = true;
                Log();
            }

            Input.Update();
        }

        private void UpdatesProcess(float delta)
        {
            playerScene.Update();
            camera.Update();
        }

        private void Log()
        {
            Console.Clear();
            string PAUSE = string.Format("\n\n                    P A U S E D\n\n    CLICK EM QUALQUER LUGAR DA TELA PARA RESUMIR");
            string CONTROLS = string.Format("                 W\n{0}{1}{2}{3}{4}{5}{6}",
                                                " USE:           ASD            PARA SE MOVIMENTAR.\n",
                                                "        LShift       ESPAÇO\n",
                                                "   MOVA O MOUSE PARA OLHAR AO REDOR\n\n",
                                                "   SEGURE O BOTÃO ESQUERDO DO MOUSE\n",
                                                "          PARA A FREE VIEW\n\n",
                                                "     UTILIZE A RODA DO MOUSE PARA\n",
                                                "            ALTERAR O ZOOM\n");
            if (pause)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(PAUSE);
                Title += "  P A U S E D";
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(CONTROLS);
                Title += "  R E S U M E D";
            }
        }

        public static int LoadImage(Bitmap image)
        {
            int textID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, textID);
            BitmapData data = image.LockBits(
                new(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(
                TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba,
                data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0);
            image.UnlockBits(data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return textID;
        }

        public static int LoadImage(string filename)
        {
            try
            {
                Bitmap file = new(filename);
                return LoadImage(file);
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
        }
    }
}