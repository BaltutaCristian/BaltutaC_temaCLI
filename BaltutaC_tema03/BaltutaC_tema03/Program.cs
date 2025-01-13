using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace TriangleApp
{
    class TriangleWindow : GameWindow
    {
        private Vector2[] vertices = new Vector2[3]; // Coordonatele triunghiului
        private Color[] vertexColors = new Color[3]; // Culorile vârfurilor
        private Color[] initialColors = new Color[3]; // Culorile inițiale
        private float cameraAngle = 0.0f; // Unghiul camerei
        private HashSet<Key> pressedKeys = new HashSet<Key>(); // Tastele apăsate

        private int currentVertex = 0; // Vertexul selectat

        public TriangleWindow() : base(800, 600)
        {
            // Citire coordonate și culori din fișier
            LoadTriangleData("triunghi.txt");

            // Evenimente pentru apăsare și eliberare taste
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }

        private void LoadTriangleData(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                for (int i = 0; i < 3; i++)
                {
                    string[] parts = lines[i].Split(',');
                    vertices[i] = new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
                    var color = Color.FromArgb(255, int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4]));
                    vertexColors[i] = color;
                    initialColors[i] = color; // Salvăm culorile inițiale
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la citirea fisierului: {ex.Message}");
                Exit();
            }
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            pressedKeys.Add(e.Key);

            if (e.Key == Key.Number1) currentVertex = 0;
            if (e.Key == Key.Number2) currentVertex = 1;
            if (e.Key == Key.Number3) currentVertex = 2;

            if (e.Key == Key.Space) ResetColors(); // Resetăm culorile la cele inițiale
            else ChangeColor(currentVertex);
        }

        private void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            pressedKeys.Remove(e.Key);
        }

        private void ResetColors()
        {
            for (int i = 0; i < 3; i++)
            {
                vertexColors[i] = initialColors[i];
            }
            Console.WriteLine("Culorile au fost resetate la valorile initiale.");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.Black); // Fundal negru
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0); // Proiecție ortografică
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            // Modificare unghi cameră
            if (pressedKeys.Contains(Key.Left)) cameraAngle -= 2.0f;
            if (pressedKeys.Contains(Key.Right)) cameraAngle += 2.0f;
        }

        private void ChangeColor(int vertexIndex)
        {
            int r = vertexColors[vertexIndex].R;
            int g = vertexColors[vertexIndex].G;
            int b = vertexColors[vertexIndex].B;

            if (pressedKeys.Contains(Key.R)) r = Math.Min(r + 10, 255);
            if (pressedKeys.Contains(Key.G)) g = Math.Min(g + 10, 255);
            if (pressedKeys.Contains(Key.B)) b = Math.Min(b + 10, 255);

            vertexColors[vertexIndex] = Color.FromArgb(255, r, g, b);

            // Afișează noile valori RGB în consolă
            Console.WriteLine($"Vertex {vertexIndex + 1} - R: {r}, G: {g}, B: {b}");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Rotate(cameraAngle, 0.0f, 0.0f, 1.0f); // Rotație pe baza camerei

            // Desenarea triunghiului
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < 3; i++)
            {
                GL.Color4(vertexColors[i]);
                GL.Vertex2(vertices[i]);
            }
            GL.End();

            SwapBuffers();
        }

        [STAThread]
        static void Main(string[] args)
        {
            using (TriangleWindow window = new TriangleWindow())
            {
                window.Run(60.0);
            }
        }
    }
}
