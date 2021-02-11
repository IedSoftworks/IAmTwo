using System;
using System.Reflection;
using System.Windows.Forms.VisualStyles;
using IAmTwo.Resources;
using IAmTwo.Shaders;
using OpenTK;
using SM.Base.Drawing;
using SM.Base.Textures;
using SM.Base.Time;
using SM.Base.Types;
using SM.Base.Windows;
using SM.Game.Controls;
using SM.Utility;
using SM2D.Object;
using SM2D.Scene;

namespace IAmTwo.Game.Objects
{
    public class PlayerSpawner : GameObject
    {
        public static GameKeybindActor KeybindActor = GameKeybindActor.CreateKeyboardActor();

        public static Polygon Circle = Polygon.GenerateCircle();
        
        private bool _mirror;
        private Timer _spawnTimer;

        private Player _player;
        private CVector2 _shaderMove = new CVector2(0);

        public PlayerSpawner(bool mirror)
        {
            ApplyPolygon(Circle);

            _shaderMove = new Vector2(0, Randomize.GetFloat(-1, 1));
            _mirror = mirror;
            Color = mirror ? ColorPallete.Mirror : ColorPallete.Player;
            SetShader(ShaderCollection.PortalShader);
            
            _Material.Blending = true;

            _ShaderArguments["move"] = _shaderMove;

            _spawnTimer = new Timer(1);
            _spawnTimer.Tick += SpawnAnimation;
            _spawnTimer.End += (timer, context) => _player.React = true;
        }

        private void SpawnAnimation(Stopwatch arg1, UpdateContext arg2)
        {
            _player.Transform.Size.Set(Player.PlayerSize * MathHelper.Clamp(arg1.Elapsed, 0,1));
            _player.Transform.Rotation = 360 * MathHelper.Clamp(arg1.Elapsed, 0, 1) % 360;

            _shaderMove.X += Deltatime.RenderDelta * .1f * (_mirror ? -1 : 1);
        }

        protected override void DrawContext(ref DrawContext context)
        {
            base.DrawContext(ref context);
        }

        public void Spawn()
        {
            _player = new Player(KeybindActor, _mirror);
            _player.Transform.Size.Set(0);
            _player.Transform.Position.Set(Transform.Position);
            (Parent as ItemCollection).Add(_player);

            _spawnTimer.Start();
        }
    }
}