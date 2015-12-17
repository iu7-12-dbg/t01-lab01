using System;
using System.Drawing;
using System.Windows.Forms;

namespace AStarDemo
{
    internal static class Root
    {
        public static readonly Scene Scene;
        public static readonly Renderer Renderer;

        static Root()
        {
            Scene = new Scene();
            var background = new SceneObjects.Background(Color.White);
            Scene.Objects.Add(background);
            Renderer = new Renderer(() => { return Scene; });
            Renderer.AntialiasingEnabled = true;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            
            Application.Run(new MainDialog());
            Renderer.Dispose();
        }
    }
}
