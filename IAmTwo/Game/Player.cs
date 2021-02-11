using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms.VisualStyles;
using IAmTwo.Game.Objects;
using IAmTwo.Game.Objects.SpecialObjects;
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
        public static Vector2 PlayerSize = new Vector2(40,60);

        private GameKeybindActor _keybindActor;

        private List<PhysicsObject> _collidedWith = new List<PhysicsObject>();

        private float _speed = 2000;

        private bool _jumpCharging = false;
        private float _jumpMomentum;
        private float _jumpHeight;

        private SpecialObject _lastSpecialObject = null;

        public float JumpMultiplier = DefaultJumpMultiplier;
        public bool Mirror;
        public bool React;

        public Player(GameKeybindActor actor, bool mirror) : base(true)
        {
            Mass = 10;

            _Material.Blending = true;

            actor.ConnectHost(keybindHost);
            _keybindActor = actor;

            _ShaderArguments["ColorScale"] = 1f;

            Texture = Resource.RequestTexture(@".\Resources\player_spite.png");

            Color = mirror ? ColorPallete.Mirror : ColorPallete.Player;
            Mirror = mirror;

            Transform.ZIndex = mirror ? -1 : 1;
        }

        public override void Update(UpdateContext context)
        {
            if (!React) return;

            float speedMul = 1;

            float xDir = _keybindActor.Get<float>("move") * (Mirror ? -1 : 1);
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

                special.ColliedWithPlayer(this, mtv);
                return;
            }

            if (obj is PlayerSpawner) return;

            Force += mtv * 75;
            Transform.Position.Add(mtv);

        }
    }
}