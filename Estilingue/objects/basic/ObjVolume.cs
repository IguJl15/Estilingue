using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Estilingue.objects
{
    public class ObjVolume : Volume
    {
        private Vector3[] vertices;
        private Vector3[] colors;
        private Vector2[] textCoords;

        private List<Tuple<int, int, int>> faces = new();

        public ObjVolume(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
        {
        }

        public ObjVolume() : base(Vector3.Zero, Vector3.Zero, Vector3.One)
        {
        }

        public override int VertCount { get { return vertices.Length; } }
        public override int IndiceCount { get { return faces.Count * 3; } }
        public override int ColorDataCount { get { return colors.Length; } }

        /// <summary>
        /// Get vertices for this object
        /// </summary>
        /// <returns></returns>
        public override Vector3[] GetVerts()
        {
            return vertices;
        }

        /// <summary>
        /// Get indices to draw this object
        /// </summary>
        /// <param name="offset">Number of vertices buffered before this object</param>
        /// <returns>Array of indices with offset applied</returns>
        public override int[] GetIndices(int offset = 0)
        {
            List<int> temp = new();

            foreach (var face in faces)
            {
                temp.Add(face.Item1 + offset);
                temp.Add(face.Item2 + offset);
                temp.Add(face.Item3 + offset);
            }

            return temp.ToArray();
        }

        /// <summary>
        /// Get color data.
        /// </summary>
        /// <returns></returns>
        public override Vector3[] GetColorData()
        {
            return colors;
        }

        /// <summary>
        /// Get texture coordinates
        /// </summary>
        /// <returns></returns>
        public override Vector2[] GetTextureCoords()
        {
            return textCoords;
        }

        /// <summary>
        /// Calculates the model matrix from transforms
        /// </summary>
        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale) *
                Matrix4.CreateRotationX(Rotation.X) *
                Matrix4.CreateRotationY(Rotation.Y) *
                Matrix4.CreateRotationZ(Rotation.Z) *
                Matrix4.CreateTranslation(Position);
        }

        public static ObjVolume LoadFromFile(string filename)
        {
            ObjVolume obj = new();
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    obj = LoadFromString(reader.ReadToEnd());
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found: {0}", filename);
            }
            catch (Exception)
            {
                Console.WriteLine("Error loading file: {0}", filename);
            }

            return obj;
        }

        public static ObjVolume LoadFromString(string obj)
        {
            // Seperate lines from the file
            List<String> lines = new(obj.Split('\n'));

            // Lists to hold model data
            List<Vector3> verts = new();
            List<Vector3> colors = new();
            List<Vector2> texs = new();
            List<Tuple<int, int, int>> faces = new();

            // Read file line by line
            foreach (String line in lines)
            {
                if (line.StartsWith("v ")) // Vertex definition
                {
                    // Cut off beginning of line
                    String temp = line[2..];

                    Vector3 vec = new();

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a vertex
                    {
                        String[] vertparts = temp.Split(' ');

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0], out vec.X);
                        success &= float.TryParse(vertparts[1], out vec.Y);
                        success &= float.TryParse(vertparts[2], out vec.Z);

                        // Dummy color/texture coordinates for now
                        colors.Add(new Vector3(MathF.Sin(vec.Z), MathF.Sin(vec.Z), MathF.Sin(vec.Z)));
                        texs.Add(new Vector2(MathF.Sin(vec.Z), MathF.Sin(vec.Z)));

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing vertex: {0}", line);
                        }
                    }

                    verts.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Tuple<int, int, int> face = new(0, 0, 0);

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a face
                    {
                        String[] faceparts = temp.Split(' ');

                        int i1, i2, i3;

                        // Attempt to parse each part of the face
                        bool success = int.TryParse(faceparts[0], out i1);
                        success &= int.TryParse(faceparts[1], out i2);
                        success &= int.TryParse(faceparts[2], out i3);

                        // If any of the parses failed, report the error
                        if (success)
                        {
                            // Decrement to get zero-based vertex numbers
                            face = new(i1 - 1, i2 - 1, i3 - 1);
                            faces.Add(face);
                        }
                        else
                        {
                            Console.WriteLine("Error parsing face: {0}", line);
                        }
                    }
                }
            }

            // Create the ObjVolume
            ObjVolume vol = new();
            vol.vertices = verts.ToArray();
            vol.faces = new(faces);
            vol.colors = colors.ToArray();
            vol.textCoords = texs.ToArray();

            return vol;
        }
    }
}