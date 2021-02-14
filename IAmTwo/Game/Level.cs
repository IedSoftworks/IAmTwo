using System;
using IAmTwo.Game.Objects;
using SM.Base.Drawing.Text;
using SM.Base.Windows;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.Game
{
    public class Level : Scene
    {
        public int CompleteCount = 0;
        
        public override void Initialization()
        {
            base.Initialization();

            Background = new GameBackground();
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