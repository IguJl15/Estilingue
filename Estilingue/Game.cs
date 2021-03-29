using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Estilingue
{
    public class Game : GameWindow
    {
        
        private Vector3[] vertdata;
        private Vector3[] coldata;
        private int[] indicedata;
        private int ibo_elements;
        public float mouseSensitivity = 0.0025f;
        Vector2[] texcoorddata;
        string activeShader = "default";
#pragma warning disable IDE0044
        Dictionary<string, ShaderProgram> shaders = new();
        Dictionary<string, int> textures = new();
        private List<Volume> objects = new();
#pragma warning restore IDE0044

        private Player player;
        private TCamera camera;

        public float FPS;

        public Game()
            : base(1440,900, new GraphicsMode(32,24,0,2)){}
        
        private void InitProgram()
        {


            Input.Initialize(this);

            /** We'll need to get another buffer object to put our indice data into.  */
            GL.GenBuffers(1, out ibo_elements);

            shaders.Add("default", new ShaderProgram("shader/vs.glsl", "shader/fs.glsl", true));
            shaders.Add("textured", new ShaderProgram("shader/vs_tex.glsl", "shader/fs_tex.glsl", true));
            activeShader = "textured";

            textures.Add("opentksquare.png", LoadImage("content/opentksquare.png"));
            textures.Add("opentksquare2.png", LoadImage("content/opentksquare2.png"));
            textures.Add("player.png", LoadImage("content/player.png"));

            TexturedCube tc = new(new(5.0f, 1f, 0f), Vector3.Zero, Vector3.One);
            tc.TextureID = textures["opentksquare.png"];
            objects.Add(tc);

            TexturedCube tc2 = new(new(-5.0f, 1f, 0f), Vector3.Zero, Vector3.One);
            tc2.Position += new Vector3(1f, 1f, 1f);
            tc2.TextureID = textures["opentksquare2.png"];
            objects.Add(tc2);

            TexturedCube tc3 = new(new Vector3(0.0f, -1f, 0f), Vector3.Zero, new Vector3(50f, 0.05f, 15f));
            tc3.Position += new Vector3(1f, 1f, 1f);
            tc3.TextureID = textures["opentksquare2.png"];
            objects.Add(tc3);

            player = new(new(0.0f, 0.0f, -5f), Vector3.Zero);
            player.TextureID = textures["player.png"];
            objects.Add(player);

            camera = new(player);



        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Title = "Hello OpenTK!";
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
            InitProgram();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);


            shaders[activeShader].EnableVertexAttribArrays();

            int indiceat = 0;

            foreach (Volume v in objects)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, v.TextureID);

                Matrix4 mdv1 = v.ModelViewProjectionMatrix;

                GL.UniformMatrix4(shaders[activeShader].GetUniform("modelview"), false, ref mdv1);

                if (shaders[activeShader].GetUniform("maintexture") != -1)
                {
                    GL.Uniform1(shaders[activeShader].GetUniform("maintexture"), 0);
                }

                GL.DrawElements(BeginMode.Triangles, v.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += v.IndiceCount;
            }

            shaders[activeShader].DisableVertexAttribArrays();

            GL.Flush();
            SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            UpdatesProcess((float)e.Time);

            // In this code, we gather up all the values for the data we need to send to the graphics card.
            List<Vector3> verts = new();
            List<int> inds = new();
            List<Vector3> colors = new();
            List<Vector2> texcoords = new();

            int vertcount = 0;

            foreach (Volume v in objects)
            {
                verts.AddRange(v.GetVerts().ToList());
                inds.AddRange(v.GetIndices(vertcount).ToList());
                colors.AddRange(v.GetColorData().ToList());
                texcoords.AddRange(v.GetTextureCoords());
                vertcount += v.VertCount;
            }

            vertdata = verts.ToArray();

            indicedata = inds.ToArray();
            coldata = colors.ToArray();
            texcoorddata = texcoords.ToArray();



            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vPosition"));

            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            // Buffer vertex color if shader supports it
            if (shaders[activeShader].GetAttribute("vColor") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vColor"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }
            

            // Buffer texture coordinates if shader supports it
            if (shaders[activeShader].GetAttribute("texcoord") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("texcoord"));
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texcoorddata.Length * Vector2.SizeInBytes), texcoorddata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            foreach (Volume v in objects)
            {
                v.CalculateModelMatrix();
                v.ViewProjectionMatrix = camera.GetThirdPersonViewMatrix() *
                    Matrix4.CreatePerspectiveFieldOfView(1.3f, (float)ClientSize.Width / (float)ClientSize.Height, 1.0f, 80.0f); 
                v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix;
            }

            GL.UseProgram(shaders[activeShader].programID);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);

            Input.Update();
        }

        private void UpdatesProcess(float delta)
        {
            player.Update(delta, mouseSensitivity);
            camera.Update(mouseSensitivity);
            Console.WriteLine(player.ModelMatrix.ExtractTranslation());

            if (Input.KeyPress("Escape") && CursorVisible)
            {
                Close();
            }
            else if(Input.KeyPress("Escape") && !CursorVisible)
            {
                CursorVisible = true;
            }
            if (Input.MousePress("Left") && CursorVisible)
            {
                CursorVisible = false;
            }
            if (Input.KeyPress(OpenTK.Input.Key.C))
            {
                if (Input.MouseLock)
                {
                    Input.MouseLock = false;
                }
                if (!Input.MouseLock)
                {
                    Input.MouseLock = true;
                }
            }

        }
        static int LoadImage(Bitmap image)
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
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0);
            image.UnlockBits(data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return textID;
        }
        static int LoadImage(string filename)
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