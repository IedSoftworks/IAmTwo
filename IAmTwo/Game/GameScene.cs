using System;
using System.Runtime.Remoting;
using IAmTwo.Game.Objects;
using IAmTwo.Game.Objects.SpecialObjects;
using IAmTwo.Resources;
using IAmTwo.Shaders;
using OpenTK;
using SharpDX.Win32;
using SM.Base.Drawing.Text;
using SM.Base.Windows;
using SM.Game.Controls;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.Game
{
    public class GameScene : Level
    {


        public override void Initialization()
        {
            base.Initialization();

            Background = new GameBackground();

            CreateBorders(Objects);

            PlayerSpawner p = new PlayerSpawner(false);
            p.Transform.Position.Set(-500,0);

            PlayerSpawner mirror = new PlayerSpawner(true);
            mirror.Transform.Position.Set(500, 0);

            GameObject obj = new GameObject();
            obj.Transform.Size.Set(500, 20);

            PortalConnector portal = new PortalConnector(new Vector2(-250, -Camera.WorldScale.Y / 2 + 15 / 2 + 50), new Vector2(0, 20+25));
            
            Objects.Add(p, mirror, obj, portal);
            p.Spawn();
            mirror.Spawn();
        }
    }
}