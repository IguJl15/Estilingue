using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.IO;

namespace Estilingue
{
    class Game : GameWindow
    {
        public Game()
            : base(1280, 720, new GraphicsMode(32, 24, 0, 4))
        {

        }

        /// <summary>
        /// ID of our program on the graphics card
        /// </summary>
        int pgmID;
        /// <summary>
        /// Address of the vertex shader
        /// </summary>
        int vsID;
        /// <summary>
        /// Address of the fragment shader
        /// </summary>
        int fsID;
        /// <summary>
        /// Address of the color parameter
        /// </summary>
        int attribute_vcol;
        /// <summary>
        /// Address of the position parameter
        /// </summary>
        int attribute_vpos;
        /// <summary>
        /// Address of the modelview matrix uniform
        /// </summary>
        int uniform_mview;
        /// <summary>
        /// Address of the Vertex Buffer Object for our position parameter
        /// </summary>
        int vbo_position;
        /// <summary>
        /// Address of the Vertex Buffer Object for our color parameter
        /// </summary>
        int vbo_color;
        /// <summary>
        /// Address of the Vertex Buffer Object for our modelview matrix
        /// </summary>
        int vbo_mview;
        /// <summary>
        /// Index Buffer Object
        /// </summary>
        int ibo_elements;
        /// <summary>
        /// Array of our vertex positions
        /// </summary>
        Vector3[] vertdata;
        /// <summary>
        /// Array of our vertex colors
        /// </summary>
        Vector3[] coldata;
        /// <summary>
        /// Array of our indices
        /// </summary>
        int[] indicedata;
        /// <summary>
        /// List of all the Volumes to be drawn
        /// </summary>
        List<Volume> objects = new List<Volume>();
        Player player;
        Camera camera;

        void initProgram()
        {
            objects.Add(new Cube(new Vector3(0.0f, -1f, 0f), Vector3.Zero, new Vector3(50f, 0.05f, 15f)));
            objects.Add(player);

            /** In this function, we'll start with a call to the GL.CreateProgram() function,
             * which returns the ID for a new program object, which we'll store in pgmID. */
            pgmID = GL.CreateProgram();

            loadShader("vs.glsl", ShaderType.VertexShader, pgmID, out vsID);
            loadShader("fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);

            /** Now that the shaders are added, the program needs to be linked.
             * Like C code, the code is first compiled, then linked, so that it goes
             * from human-readable code to the machine language needed. */
            GL.LinkProgram(pgmID);

            /** We have multiple inputs on our vertex shader, so we need to get
            * their addresses to give the shader position and color information for our vertices.
            * 
            * To get the addresses for each variable, we use the 
            * GL.GetAttribLocation and GL.GetUniformLocation functions.
            * Each takes the program's ID and the name of the variable in the shader. */
            attribute_vpos = GL.GetAttribLocation(pgmID, "vPosition");
            attribute_vcol = GL.GetAttribLocation(pgmID, "vColor");
            uniform_mview = GL.GetUniformLocation(pgmID, "modelview");

            /** Now our shaders and program are set up, but we need to give them something to draw.
             * To do this, we'll be using a Vertex Buffer Object (VBO).
             * When you use a VBO, first you need to have the graphics card create
             * one, then bind to it and send your information. 
             * Then, when the DrawArrays function is called, the information in
             * the buffers will be sent to the shaders and drawn to the screen. */
            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_mview);

            /** We'll need to get another buffer object to put our indice data into.  */
            GL.GenBuffers(1, out ibo_elements);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            player = new(new(0.0f, 0.0f, -5f), Vector3.Zero);
            camera = new();
            initProgram();

            Input.Initialize(this);
            camera.Initialize(this);

            Title = "Hello OpenTK!";
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
        }

        /// <summary>
        /// This creates a new shader (using a value from the ShaderType enum), loads code for it, compiles it, and adds it to our program.
        /// It also prints any errors it found to the console, which is really nice for when you make a mistake in a shader (it will also yell at you if you use deprecated code).
        /// </summary>
        /// <param name="filename">File to load the shader from</param>
        /// <param name="type">Type of shader to load</param>
        /// <param name="program">ID of the program to use the shader with</param>
        /// <param name="address">Address of the compiled shader</param>
        void loadShader(String filename, ShaderType type, int program, out int address)
        {

            address = GL.CreateShader(type);
            using (StreamReader sr = new(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }



        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.EnableVertexAttribArray(attribute_vpos);
            GL.EnableVertexAttribArray(attribute_vcol);

            int indiceat = 0;

            foreach (Volume v in objects)
            {
                GL.UniformMatrix4(uniform_mview, false, matrix: ref v.modelViewProjectionMatrix);
                GL.DrawElements(BeginMode.Triangles, v.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += v.IndiceCount;
            }

            GL.DisableVertexAttribArray(attribute_vpos);
            GL.DisableVertexAttribArray(attribute_vcol);

            GL.Flush();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            /** In this code, we gather up all the values for the data we need to send to the graphics card. */
            List<Vector3> verts = new List<Vector3>();
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();

            int vertcount = 0;
            foreach (Volume v in objects)
            {
                verts.AddRange(v.GetVerts().ToList());
                inds.AddRange(v.GetIndices(vertcount).ToList());
                colors.AddRange(v.GetColorData().ToList());
                vertcount += v.VertCount;
            }

            // Processar inputs e updates das classes
            player.Update((float)e.Time);
            camera.Update(this);

            if (Input.KeyPress(OpenTK.Input.Key.Escape))
            {
                if (CursorVisible)
                {
                    Exit();
                }
                else
                {
                    CursorVisible = true;
                }
            }
            if (Input.MousePress(OpenTK.Input.MouseButton.Left) && CursorVisible)
            {
                CursorVisible = false;
            }

            //fim
            vertdata = verts.ToArray();
            indicedata = inds.ToArray();
            coldata = colors.ToArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vcol, 3, VertexAttribPointerType.Float, true, 0, 0);

            foreach (Volume v in objects)
            {
                v.CalculateModelMatrix();
                v.ViewProjectionMatrix = camera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f);
                v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix;
            }

            GL.UseProgram(pgmID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);
            Input.Update();

        }
    }
}