using System;
using System.Runtime.Remoting;
using IAmTwo.Game.SpecialObjects;
using IAmTwo.Resources;
using IAmTwo.Shaders;
using SharpDX.Win32;
using SM.Base.Drawing.Text;
using SM.Base.Windows;
using SM.Game.Controls;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.Game
{
    public class GameScene : Scene
    {
        public override void Initialization()
        {
            base.Initialization();

            Background = new GameBackground();

            CreateBorders(Objects);
            GameKeybindActor actor = GameKeybindActor.CreateKeyboardActor();

            Player p = new Player(actor, false);
            p.Transform.Position.Set(-500,0);

            Player mirror = new Player(actor, true);
            mirror.Transform.Position.Set(500, 0);

            GameObject obj = new GameObject();
            obj.Transform.Size.Set(500, 20);
            JumpBooster booster = new JumpBooster();
            booster.Transform.Position.Set(250, -Camera.WorldScale.Y / 2 + 15 /2 + 33);
            JumpBlocker blocker = new JumpBlocker();
            blocker.Transform.Position.Set(-250, -Camera.WorldScale.Y / 2 + 15 /2 + 33);

            Objects.Add(p, mirror, obj, booster, blocker);
        }

        public static void CreateBorders(ItemCollection collection)
        {
            int size = 15;

            for (int i = 0; i < 4; i++)
            {
                GameObject wall = new GameObject();
                
                bool hoz = Math.Floor(i / 2d) == 0;
                bool second = i % 2 == 1;

                switch (hoz)
                {
                    case true:
                        wall.Transform.Size.Set(size, Camera.WorldScale.Y);
                        wall.Transform.Position.Set((Camera.WorldScale.X / 2 - size / 2) * (second ? -1 : 1), 0);
                        break;
                    case false:
                        wall.Transform.Size.Set(Camera.WorldScale.X, size);
                        wall.Transform.Position.Set(0, (Camera.WorldScale.Y / 2 - size / 2) * (second ? -1 : 1));
                        break;
                }
                collection.Add(wall);
            }
        }

        public override void Update(UpdateContext context)
        {
            foreach (PhysicsObject collider in PhysicsObject.Colliders)
            {
                collider.UpdateHitbox();
            }

            base.Update(context);
        }
    }
}