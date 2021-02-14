using IAmTwo.Resources;
using OpenTK;
using SM.Base.Windows;

namespace IAmTwo.Game.Objects.SpecialObjects
{
    public class Elevator : SpecialObject
    {
        private bool _reverse;
        private bool _sideways;

        public float Speed = 1.5f;

        public bool Reverse
        {
            get => _reverse;
            set
            {
                _reverse = value;
                ReverseAction();
            }
        }

        public Elevator(bool sideways)
        {
            _sideways = sideways;
            Reverse = false;

            Texture = Resource.RequestTexture(@".\Resources\jumparrow_d.png");
            _ShaderArguments["EmissionTex"] = Resource.RequestTexture(@".\Resources\jumparrow_e.png");
            _ShaderArguments["EmissionStrength"] = 1f;

            _Material.Blending = true;
            Transform.Size.Changed += SizeChanged;
        }
        public override void Update(UpdateContext context)
        {
            base.Update(context);
            TextureTransform.Offset.Add(0, -context.Deltatime * .5f);
        }
        private void SizeChanged()
        {
            float size = 3;

            if (_sideways)
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

        private void ReverseAction()
        {
            TextureTransform.Rotation = (_sideways ? -90 : 0) + (_reverse ? 0 : 180);
            _ShaderArguments["EmissionTint"] = _reverse ? ColorPallete.Down : ColorPallete.Up;
        }

        public override void ColliedWithPlayer(SpecialActor p, Vector2 mtv)
        {
            base.Collided(p, mtv);
            float sped = Gravity * p.Mass * Speed * (_sideways ? 20 :1) * (_reverse ? -1 : 1);
            if (_sideways) p.Force.X += sped;
            else p.Force.Y += sped;
        }
    }
}