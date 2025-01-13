using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTK_console_sample01
{
    class SimpleWindow : GameWindow
    {
        private float rotationAngle = 0.0f; // Unghiul de rotație al triunghiului
        private float mouseOffsetX = 0.0f;  // Deplasarea triunghiului pe axa X

        public SimpleWindow() : base(800, 600)
        {
            KeyDown += Keyboard_KeyDown;  // Gestionare apăsări de taste
            MouseMove += Mouse_Move;      // Gestionare mișcări de mouse
        }

        void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Exit();

            if (e.Key == Key.F11)
                WindowState = (WindowState == WindowState.Fullscreen) ? WindowState.Normal : WindowState.Fullscreen;

            if (e.Key == Key.Left) rotationAngle -= 5.0f;   // Rotește triunghiul la stânga
            if (e.Key == Key.Right) rotationAngle += 5.0f;  // Rotește triunghiul la dreapta
        }

        void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            mouseOffsetX = (float)e.X / Width * 2.0f - 1.0f; // Deplasare triunghi pe axa X
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.MidnightBlue); // Setare culoare de fundal
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height); // Ajustare viewport
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0); // Setare proiecție ortografică
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit); // Curățare ecran

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(mouseOffsetX, 0.0f, 0.0f); // Deplasare pe baza mouse-ului
            GL.Rotate(rotationAngle, 0.0f, 0.0f, 1.0f); // Rotație pe baza tastelor

            GL.Begin(PrimitiveType.Triangles); // Desenează triunghi mov
            GL.Color3(Color.Purple);
            GL.Vertex2(-0.5f, -0.5f);
            GL.Vertex2(0.5f, -0.5f);
            GL.Vertex2(0.0f, 0.5f);
            GL.End();

            SwapBuffers(); 
        }

        [STAThread]
        static void Main(string[] args)
        {
            using (SimpleWindow example = new SimpleWindow())
            {
                example.Run(30.0, 0.0);
            }
        }
    }
}
