using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Estilingue.objects
{
    public abstract class Scene
    {
        internal readonly Game game;
        internal Camera activeCamera;

        private Vector3[] vertdata;
        private Vector3[] coldata;
        private Vector2[] texcoorddata;
        private int[] indicedata;
        private int ibo_elements = 5;

        public List<Volume> objectsObj;
        internal List<Volume> objects;
        private Dictionary<string, ShaderProgram> shaders = new();
        internal Dictionary<string, int> textures = new();
        private string activeShader = "defaulti";

        public Scene(Game game)
        {
            this.game = game;
            objects = new();
            objectsObj = new();
        }

        public virtual void Init()
        {
            this.activeCamera = game.camera;
            GL.GenBuffers(1, out ibo_elements);
            shaders.Add("default", new ShaderProgram("shader/vs.glsl", "shader/fs.glsl", true));
            shaders.Add("textured", new ShaderProgram("shader/vs_tex.glsl", "shader/fs_tex.glsl", true));
            activeShader = "textured";

            // Loading Textures
            textures.Add("opentksquare.png", Game.LoadImage("content/opentksquare.png"));
            textures.Add("opentksquare2.png", Game.LoadImage("content/opentksquare2.png"));
            textures.Add("opentksquare3.png", Game.LoadImage("content/opentksquare3.png"));
            textures.Add("player.png", Game.LoadImage("content/player.png"));


            Initialize();
        }


        public virtual void Update()
        {
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
                v.ViewProjectionMatrix = activeCamera.GetViewMatrix() * activeCamera.CreateFieldOfView();
                v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix;
            }

            GL.UseProgram(shaders[activeShader].programID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);


            UpdateProcess();
        }


        public virtual void Render()
        {
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

            RenderProcess();
        }

        protected abstract void Initialize();
        protected abstract void UpdateProcess();
        protected abstract void RenderProcess();
    }
}