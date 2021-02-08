using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms.VisualStyles;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SM.Base.Drawing;
using SM.Base.Scene;
using SM.Base.Windows;
using SM.Game.Controls;
using SM.Utility;
using SM2D.Drawing;

namespace IAmTwo.Game
{
    public class Player : PhysicsObject
    {
        static GameKeybindHost keybindHost = new GameKeybindHost(new GameKeybindList()
        {
            {"move", context => (float)(context.KeyboardState[Key.A] ? -1 : 0) + (context.KeyboardState[Key.D] ? 1 : 0), context => context.ControllerState?.Thumbs.Left.X },
            {"jump", context => context.KeyboardState[Key.Space], context => context.ControllerState?.Buttons.A}
        });

        private GameKeybindActor _keybindActor;
        private bool _mirror;

        private float _speed = 200;

        private bool _jumpCharging = false;
        private float _jumpMomentum;
        private float _jumpHeight;

        public Player(GameKeybindActor actor, bool mirror) : base(true)
        {
            Mass = 10;

            actor.ConnectHost(keybindHost);
            _keybindActor = actor;

            Transform.Size.Set(40,60);
            Texture = Resource.RequestTexture(@".\Resources\player_spite.png");

            Color = mirror ? Color4.Red : Color4.Blue;
            _mirror = mirror;

            Transform.ZIndex = mirror ? -1 : 1;
        }

        public override void Update(UpdateContext context)
        {
            float speedMul = 1;
            if (!Grounded) speedMul = .5f;

            float xDir = _keybindActor.Get<float>("move") * (_mirror ? -1 : 1);
            if (Math.Sign(xDir) != 0)
                Transform.VerticalFlip = Math.Sign(xDir) < 0;

            Force.X = xDir * _speed * speedMul;

            bool jump = _keybindActor.Get<bool>("jump");
            if (jump && Grounded)
            {
                Force.Y += CalculateGravity() * 25;
            }


            base.Update(context);

        }

        protected override void DrawContext(ref DrawContext context)
        {
            GetMaterialReference().ShaderArguments["ColorScale"] = 1f;

            base.DrawContext(ref context);
        }

        public override void Collided(PhysicsObject obj, Vector2 mtv)
        {
            base.Collided(obj, mtv);
            Force += mtv * 75;
            Transform.Position.Add(mtv);
        }
    }
}