using System;
using System.Drawing;
using System.Windows.Forms;

namespace AStarDemo
{
    internal static class Root
    {
        public static readonly Scene Scene;
        public static readonly Renderer Renderer;
        public static readonly Core.GraphEngine<Core.GraphEdge> GraphEngine;
        public static readonly Core.Graph Graph;

        static Root()
        {
            Scene = new Scene();
            var background = new SceneObjects.Background(Color.White);
            Scene.Objects.Add(background.Id, background);
            Renderer = new Renderer(() => { return Scene; });
            Renderer.AntialiasingEnabled = true;
            GraphEngine = new Core.GraphEngine<Core.GraphEdge>(1024);
            Graph = new Core.Graph(1024);
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            
            Application.Run(new MainDialog());
            Scene.Dispose();
            Renderer.Dispose();
        }
    }
}
