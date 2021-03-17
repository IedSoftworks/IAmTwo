using System;
using IAmTwo.LevelObjects.Objects.SpecialObjects;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Input;
using SM.Base.Utility;
using SM.Base.Window;
using SM.Game.Controls;

namespace IAmTwo.Game
{
    public class Player : SpecialActor
    {
        static GameKeybindHost keybindHost = new GameKeybindHost(new GameKeybindList()
        {
            {"move", context => (float)(context.KeyboardState[Key.A] ? -1 : 0) + (context.KeyboardState[Key.D] ? 1 : 0), context => context.ControllerState?.Thumbs.Left.X },
            {"jump", context => context.KeyboardState[Key.Space], context => context.ControllerState?.Buttons.A}
        });

        public static float DefaultJumpMultiplier = 25;
        public static Vector2 PlayerSize = new Vector2(50);


        private GameKeybindActor _keybindActor;

        private float _speed = 2000;
        
        public float JumpMultiplier = DefaultJumpMultiplier;
        public bool Mirror;
        public bool React;

        public Player(GameKeybindActor actor, bool mirror)
        {
            Mass = 10;
            Passive = false;

            Material.Blending = true;

            actor.ConnectHost(keybindHost);
            _keybindActor = actor;

            Transform.Size.Set(50);
            Texture = Resource.RequestTexture(@".\Resources\MovingBox_d.png");

            ShaderArguments["EmissionTex"] = Resource.RequestTexture(@".\Resources\MovingBox_e.png");
            ShaderArguments["EmissionStrength"] = 2f;
            
            Color = mirror ? ColorPallete.Mirror : ColorPallete.Player;
            Mirror = mirror;
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
                Force.Y += CalculateGravity(context.Deltatime) * JumpMultiplier;

            }

            base.Update(context);
        }

        public override void Collided(PhysicsObject obj, Vector2 mtv)
        {
            base.Collided(obj, mtv);

            if (obj is SpecialObject special)
            {
                HandleCollision(special, mtv);
                return;
            }
            
            DefaultCollisionResolvement(mtv);
        }
    }
}