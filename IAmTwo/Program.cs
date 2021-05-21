﻿using System;
using IAmTwo.Game;
using IAmTwo.LevelObjects;
using IAmTwo.Shaders;
using OpenTK;
using SM.Base.Window;
using SM2D;
using SM2D.Types;
using System.Linq;
using IAmTwo.LevelObjects.Objects;
using SM.Utils.Controls;

namespace IAmTwo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Any(a => a == "--controller"))
            {
                Controller.Actor = GameKeybindActor.CreateControllerActor(0);
                Controller.IsPS = args.Any(a => a == "--playstation");
            }
            else
            {
                Controller.Actor = GameKeybindActor.CreateKeyboardActor();
            }

            Transformation.ZIndexPercision = 50;

            LevelSet.Load();
            GameController.GlobalDeadband = .5f;

            GLWindow window = new GLWindow(1600,900, "I am two - DevBuild", WindowFlags.Window, VSyncMode.Off)
            { };
            window.ApplySetup(new Window2DSetup());
            window.UpdateFrame += Controller.MouseCursor;

            window.TargetUpdateFrequency = 60;
            window.SetRenderPipeline(new GameRenderPipeline());
            //window.SetScene(MainMenu.Menu);
            window.SetScene(new CreditsScene(LevelSet.LevelSets.First().Value[0]));
            //window.SetScene(new GameScene(new LevelConstructor() { Size = 650 }));
            window.RunFixedUpdate(100);
            window.Run();
        }
    }
}
