using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IAmTwo.Game;
using IAmTwo.LevelEditor;
using IAmTwo.LevelObjects;
using IAmTwo.LevelObjects.Objects;
using IAmTwo.Shaders;
using OpenTK;
using SM.Base.Windows;
using SM.Game.Controls;
using SM2D;
using SM2D.Pipelines;

namespace IAmTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            GLWindow window = new GLWindow(1600,900, "I am two - DevBuild", GameWindowFlags.Default, VSyncMode.Off);
            window.ApplySetup(new Window2DSetup()
            {
                WorldScale = new Vector2(0, 700)
            });
            //window.CursorVisible = false;

            window.TargetUpdateFrequency = 60;
            window.SetRenderPipeline(new GameRenderPipeline());
            window.SetScene(new LevelEditor.LevelEditor(new LevelConstructor()));
            //window.SetScene(new GameScene(new LevelConstructor() { Size = 650 }));
            window.Run();
        }
    }
}
