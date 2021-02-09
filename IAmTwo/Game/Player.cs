using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms.VisualStyles;
using IAmTwo.Game.SpecialObjects;
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

        public static float DefaultJumpMultiplier = 25;

        private GameKeybindActor _keybindActor;
        private bool _mirror;

        private List<PhysicsObject> _collidedWith = new List<PhysicsObject>();

        private float _speed = 1000;

        private bool _jumpCharging = false;
        private float _jumpMomentum;
        private float _jumpHeight;

        private SpecialObject _lastSpecialObject = null;

        public float JumpMultiplier = DefaultJumpMultiplier;

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

            float xDir = _keybindActor.Get<float>("move") * (_mirror ? -1 : 1);
            if (Math.Sign(xDir) != 0)
                Transform.VerticalFlip = Math.Sign(xDir) < 0;

            Force.X = xDir * _speed * speedMul;

            bool jump = _keybindActor.Get<bool>("jump");
            if (jump && Grounded)
            {
                Force.Y += CalculateGravity() * JumpMultiplier;

            }

            _collidedWith.Clear();
            base.Update(context);

            if (!_collidedWith.Contains(_lastSpecialObject))
            {
                _lastSpecialObject?.EndCollision(this, Vector2.Zero);
                _lastSpecialObject = null;
            }
        }

        protected override void DrawContext(ref DrawContext context)
        {
            GetMaterialReference().ShaderArguments["ColorScale"] = 1f;

            base.DrawContext(ref context);
        }

        public override void Collided(PhysicsObject obj, Vector2 mtv)
        {
            base.Collided(obj, mtv);
            _collidedWith.Add(obj);

            if (obj is SpecialObject special)
            {
                if (obj != _lastSpecialObject)
                {
                    special.BeganCollision(this, mtv);
                    _lastSpecialObject = special;
                }
                return;
            }

            Force += mtv * 75;
            Transform.Position.Add(mtv);

        }
    }
}