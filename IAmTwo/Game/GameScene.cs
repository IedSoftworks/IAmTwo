using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using IAmTwo.LevelObjects;
using IAmTwo.LevelObjects.Objects;
using IAmTwo.LevelObjects.Objects.SpecialObjects;
using IAmTwo.Resources;
using IAmTwo.Shaders;
using OpenTK;
using SharpDX.Win32;
using SM.Base.Drawing.Text;
using SM.Base.Types;
using SM.Base.Windows;
using SM.Game.Controls;
using SM.Utility;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.Game
{
    public class GameScene : LevelScene
    {
        private DrawText _fpsCounter;


        public GameScene(LevelConstructor constructor) : base(constructor)
        { }

        public override void Initialization()
        {
            base.Initialization();

            PlayerSpawner p = new PlayerSpawner()
            {
                Mirror = false
            };
            p.Transform.Position.Set(-500,-50);

            PlayerSpawner mirror = new PlayerSpawner()
            {
                Mirror = false
            };
            mirror.Transform.Position.Set(500, 50);

            GameObject obj = new GameObject();
            obj.Transform.Size.Set(1300, 20);
            
            Elevator elevator = new Elevator();
            elevator.Transform.Size.Set(100, 300);
            elevator.Transform.Position.Set(300, -Camera.WorldScale.Y / 2 + 15 / 2 + 150);

            Elevator sideElevator = new Elevator();
            sideElevator.Transform.Size.Set(Camera.WorldScale.X, 100);
            sideElevator.Transform.Position.Set(0, 200);
            
            Cube cube = new Cube();
            cube.Transform.Position.Set(-100, -Camera.WorldScale.Y / 2 + 15 / 2 + 50);

            Door door = new Door();
            door.Transform.Position.Set(-250, -Camera.WorldScale.Y / 2 + 15 / 2 + 50);
            door.Transform.Size.Set(15, 100);

            PressableButton button = new PressableButton();
            button.Transform.Position.Set(0, button.Transform.Size.Y);
            button.ButtonActor = door;

            JumpModifier booster = new JumpModifier();
            booster.Transform.Position.Set(-500, -Camera.WorldScale.Y / 2 + 15 / 2 + 25);
            
            JumpModifier blocker = new JumpModifier();
            blocker.Transform.Position.Set(500, -Camera.WorldScale.Y / 2 + 15 / 2 + 25);
            
            Portal entrence = new Portal();
            entrence.Transform.Position.Set(0, -Camera.WorldScale.Y / 2 + 15 / 2 + 25);

            Portal exit = new Portal();
            exit.Transform.Position.Set(0, 25);


            _fpsCounter = new DrawText(Fonts.Button, "0ms");
            _fpsCounter.Transform.Position.Set(-Camera.WorldScale.X / 2 + 15, Camera.WorldScale.Y / 2 - 15);

            Objects.Add(p, mirror, obj, door, button,  elevator, sideElevator, cube, booster, blocker, exit, entrence);
            HUD.Add(_fpsCounter);
            
            p.Spawn();
            mirror.Spawn();
            entrence.Connect(exit);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);
        }

        public override void Draw(DrawContext context)
        {
            _fpsCounter.Text = $"{Math.Floor(Deltatime.RenderDelta * 1000)}ms";

            base.Draw(context);
        }
    }
}