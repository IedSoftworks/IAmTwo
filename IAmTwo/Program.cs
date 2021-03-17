using System;
using IAmTwo.LevelObjects;
using IAmTwo.Shaders;
using OpenTK;
using SM.Base.Window;
using SM2D;

namespace IAmTwo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            GLWindow window = new GLWindow(1600,900, "I am two - DevBuild", WindowFlags.Window, VSyncMode.Off);
            window.ApplySetup(new Window2DSetup()
            {
                WorldScale = new Vector2(0, 700)
            });
            window.CursorVisible = false;

            window.TargetUpdateFrequency = 60;
            window.SetRenderPipeline(new GameRenderPipeline());
            window.SetScene(new LevelEditor.LevelEditor(new LevelConstructor()));
            //window.SetScene(new GameScene(new LevelConstructor() { Size = 650 }));
            window.RunFixedUpdate(60);
            window.Run();
        }
    }
}
