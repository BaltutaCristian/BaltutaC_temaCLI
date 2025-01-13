using System;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GraphicsApp
{
    class Cube
    {
        private Vector3[] vertices;
        private Color[] faceColors;
        private int[][] faceIndices = new int[][]
        {
            new int[] { 0, 1, 5, 4 }, // Fața 1
            new int[] { 1, 2, 6, 5 }, // Fața 2
            new int[] { 2, 3, 7, 6 }, // Fața 3
            new int[] { 3, 0, 4, 7 }, // Fața 4
            new int[] { 0, 1, 2, 3 }, // Fața de sus
            new int[] { 4, 5, 6, 7 }  // Fața de jos
        };

        public Cube(string filePath)
        {
            LoadVertices(filePath);
            InitializeColors();
        }

        private void LoadVertices(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length != 8) throw new Exception("Fișierul trebuie să conțină exact 8 rânduri.");

                vertices = new Vector3[8];
                for (int i = 0; i < 8; i++)
                {
                    string[] parts = lines[i].Split(',');
                    if (parts.Length != 3) throw new Exception($"Linia {i + 1} din fișier este invalidă.");

                    vertices[i] = new Vector3(
                        float.Parse(parts[0]),
                        float.Parse(parts[1]),
                        float.Parse(parts[2])
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la încărcarea coordonatelor cubului: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private void InitializeColors()
        {
            faceColors = new Color[6];
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                faceColors[i] = Color.FromArgb(255, random.Next(256), random.Next(256), random.Next(256));
            }
        }

        public void ChangeFaceColor(int faceIndex, int rDelta, int gDelta, int bDelta, int aDelta)
        {
            var color = faceColors[faceIndex];
            int r = Math.Max(0, Math.Min(255, color.R + rDelta));
            int g = Math.Max(0, Math.Min(255, color.G + gDelta));
            int b = Math.Max(0, Math.Min(255, color.B + bDelta));
            int a = Math.Max(0, Math.Min(255, color.A + aDelta));

            faceColors[faceIndex] = Color.FromArgb(a, r, g, b);
        }

        public void Draw()
        {
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < 6; i++)
            {
                GL.Color4(faceColors[i]);
                foreach (var index in faceIndices[i])
                {
                    GL.Vertex3(vertices[index]);
                }
            }
            GL.End();
        }
    }

    class Triangle
    {
        private Vector3[] vertices;
        private Color[] vertexColors;

        public Triangle()
        {
            vertices = new Vector3[3]
            {
                new Vector3(-0.5f, -0.5f, 0.0f),
                new Vector3(0.5f, -0.5f, 0.0f),
                new Vector3(0.0f, 0.5f, 0.0f)
            };

            vertexColors = new Color[3]
            {
                Color.Red,
                Color.Green,
                Color.Blue
            };
        }

        public void ChangeVertexColor(int vertexIndex, int rDelta, int gDelta, int bDelta)
        {
            var color = vertexColors[vertexIndex];
            int r = Math.Max(0, Math.Min(255, color.R + rDelta));
            int g = Math.Max(0, Math.Min(255, color.G + gDelta));
            int b = Math.Max(0, Math.Min(255, color.B + bDelta));

            vertexColors[vertexIndex] = Color.FromArgb(255, r, g, b);

            Console.WriteLine($"Vertex {vertexIndex + 1} - R: {r}, G: {g}, B: {b}");
        }

        public void Draw()
        {
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < 3; i++)
            {
                GL.Color4(vertexColors[i]);
                GL.Vertex3(vertices[i]);
            }
            GL.End();
        }
    }

    class GraphicsWindow : GameWindow
    {
        private Cube cube;
        private Triangle triangle;
        private int selectedFace = 0;
        private int selectedVertex = 0;

        public GraphicsWindow() : base(800, 600)
        {
            cube = new Cube("cub.txt");
            triangle = new Triangle();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 0.1f, 100.0f);
            GL.LoadMatrix(ref perspective);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var keyboard = Keyboard.GetState();

            // Modificare culoare cub
            if (keyboard[Key.Number1]) selectedFace = 0;
            if (keyboard[Key.Number2]) selectedFace = 1;
            if (keyboard[Key.Number3]) selectedFace = 2;
            if (keyboard[Key.Number4]) selectedFace = 3;
            if (keyboard[Key.Number5]) selectedFace = 4;
            if (keyboard[Key.Number6]) selectedFace = 5;

            if (keyboard[Key.R]) cube.ChangeFaceColor(selectedFace, 10, 0, 0, 0);
            if (keyboard[Key.G]) cube.ChangeFaceColor(selectedFace, 0, 10, 0, 0);
            if (keyboard[Key.B]) cube.ChangeFaceColor(selectedFace, 0, 0, 10, 0);
            if (keyboard[Key.A]) cube.ChangeFaceColor(selectedFace, 0, 0, 0, -10);

            // Modificare culoare triunghi
            if (keyboard[Key.F1]) selectedVertex = 0;
            if (keyboard[Key.F2]) selectedVertex = 1;
            if (keyboard[Key.F3]) selectedVertex = 2;

            if (keyboard[Key.R]) triangle.ChangeVertexColor(selectedVertex, 10, 0, 0);
            if (keyboard[Key.G]) triangle.ChangeVertexColor(selectedVertex, 0, 10, 0);
            if (keyboard[Key.B]) triangle.ChangeVertexColor(selectedVertex, 0, 0, 10);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0.0f, 0.0f, -5.0f);

            cube.Draw();

            GL.Translate(0.0f, -2.0f, 0.0f);
            triangle.Draw();

            SwapBuffers();
        }

        [STAThread]
        static void Main(string[] args)
        {
            using (GraphicsWindow window = new GraphicsWindow())
            {
                window.Run(60.0);
            }
        }
    }
}
