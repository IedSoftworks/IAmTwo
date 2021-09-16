using System;
using IAmTwo.Game;
using IAmTwo.Resources;
using OpenTK;
using SM.Base.Utility;
using SM.Base.Window;

namespace IAmTwo.LevelObjects.Objects.SpecialObjects
{
    public class Elevator : SpecialObject
    {
        private bool _reverse;
        private bool _sideways;

        public float Speed = 1;
        
        public Elevator()
        {
            Name = "Elevator";

            StartSize = new Vector2(100, 300);

            Color = ColorPallete.Up;
            Texture = Resource.RequestTexture(@".\Resources\jumparrow_e.png");
            ShaderArguments["ColorScale"] = 1f;

            Material.Blending = true;
            Transform.Size.Changed += SizeChanged;
            Transform.Rotation.Changed += RotationOnChanged;

            Transform.Rotation.Set(180);
        }

        private void RotationOnChanged()
        {
            float rot = Transform.Rotation;

            _sideways = Math.Abs(rot - 90) < 1f || Math.Abs(rot - 270) < 1f;
            _reverse = rot == 0 || Math.Abs(rot - 270) < 1f;

            Color = _reverse ? ColorPallete.Down : ColorPallete.Up;
        }
        private void SizeChanged()
        {
            float size = 3;


            if (Transform.Size.X > Transform.Size.Y)
            {
                float aspect = Transform.Size.X / Transform.Size.Y;

                TextureTransform.Scale.Set(aspect * size, size);
            }
            else
            {
                float aspect = Transform.Size.Y / Transform.Size.X;

                TextureTransform.Scale.Set(size,  aspect * size);
            }
        }

        public override void ColliedWithPlayer(SpecialActor p, Vector2 mtv)
        {
            base.Collided(p, mtv);
            if (p.CollidedWith.Count > 1) return;

            float sped = -Gravity * Speed * (_sideways ? 1 : 10) * (_reverse ? -1 : 1) * Deltatime.FixedUpdateDelta;
            if (_sideways) p.Velocity.X += sped;
            else p.Velocity.Y += sped;
        }

        protected override void DrawContext(ref DrawContext context)
        {

            TextureTransform.Offset.Add(0, -Deltatime.RenderDelta * .1f);
            base.DrawContext(ref context);
        }
    }
}