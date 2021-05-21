using System;
using IAmTwo.LevelObjects.Objects.SpecialObjects;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using SM.Base.Drawing;
using SM.Base.Textures;
using SM.Base.Utility;
using SM.Base.Window;
using SM.Base.Window.Contexts;
using SM.Utils.Controls;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.Game
{
    public class Player : SpecialActor
    {
        public static float DefaultJumpMultiplier = 35;
        public static float PlayerSize = 100f;
        
        private float _speed = 40;

        private ItemCollection _visual;

        public DrawObject2D _head;
        private DrawObject2D _body;
        private DrawObject2D _arm;

        public float JumpMultiplier = DefaultJumpMultiplier;
        public bool Mirror;
        public bool React;


        public Player(bool mirror)
        {
            Passive = false;
            
            Color = mirror ? ColorPallete.Mirror : ColorPallete.Player;
            Mirror = mirror;
            HitboxNoRotation = true;

            const float darkening = .3f;

            Material mat = new Material()
            {
                Blending = true,
                Texture = Resource.RequestTexture(@".\Resources\player_d.png"),
                ShaderArguments =
                {
                    {"EmissionTex", Resource.RequestTexture(@".\Resources\player_e.png")},
                    {"EmissionTint", Color},
                    {"EmissionStrength", 2.5f}
                },
                Tint = new Color4(darkening, darkening, darkening, 1)
            };

            _visual = new ItemCollection {Transform = Transform};
            _visual.Transform.ZIndex.Set(50);

            HitboxChangeMatrix = Matrix4.CreateScale(.5f, 1, 1);

            _body = new DrawObject2D()
            {
                Material = mat
            };
            _body.TextureTransform.SetRectangleRelative((Texture)mat.Texture, new Vector2(0), new Vector2(234, 512));
            _body.Transform.Size.Set(_body.TextureTransform.Scale);

            _head = new DrawObject2D()
            {
                Material = mat
            };
            _head.TextureTransform.SetRectangleRelative((Texture)mat.Texture, new Vector2(234, 0), new Vector2(221, 150));
            _head.Transform.Size.Set(_head.TextureTransform.Scale);
            _head.Transform.Position.Set(.025f, .5f);

            _visual.Add(_body, _head);
            _visual.Transform.Size.Set(200);
        }

        public override void Update(UpdateContext context)
        {
            if (!React) return;

            const float rot = 5;

            float xDir = Controller.Actor.Get<float>("p_move") * (Mirror ? -1 : 1);
            if (Math.Sign(xDir) != 0)
            {
                Transform.VerticalFlip = Math.Sign(xDir) < 0;
                Transform.Rotation.Set(rot * (Transform.VerticalFlip ? 1 : -1));
                _head.Transform.Rotation.Set(rot);
            }
            else
            {
                Transform.Rotation.Set(0);
                _head.Transform.Rotation.Set(0);
            }

            Force.X = xDir * _speed;

            bool jump = Controller.Actor.Get<bool>("p_jump");
            if (jump && Grounded)
            {
                Force.Y += Gravity * JumpMultiplier;
            }

            _visual.Update(context);
            base.Update(context);
        }

        protected override void DrawContext(ref DrawContext context)
        {
            _visual.Draw(context);
        }

        public override void FixedUpdate(FixedUpdateContext context)
        {
            if (!React) return;

            base.FixedUpdate(context);
        }

        public override void Collided(PhysicsObject obj, Vector2 mtv)
        {
            base.Collided(obj, mtv);

            if (obj is SpecialObject special)
            {
                HandleCollision(special, mtv);
                return;
            }
            
            DefaultCollisionResolvement(obj, mtv);
        }
    }
}