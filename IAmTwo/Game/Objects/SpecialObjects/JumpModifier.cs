using IAmTwo.Resources;
using OpenTK;
using SM.Base.Windows;

namespace IAmTwo.Game.Objects.SpecialObjects
{
    public class JumpModifier : SpecialObject
    {
        private float _multiplier;
        
        public float Multiplier
        {
            get => _multiplier;
            set
            {
                _multiplier = value;
                HandleMultiplierChange();
            }
        }

        public JumpModifier(float multiplier)
        {
            Multiplier = multiplier;

            Texture = Resource.RequestTexture(@".\Resources\jumparrow_d.png");
            _ShaderArguments["EmissionTex"] = Resource.RequestTexture(@".\Resources\jumparrow_e.png");
            _ShaderArguments["EmissionStrength"] = 1f;
            _Material.Blending = true;
        }

        private void HandleMultiplierChange()
        {
            bool up = _multiplier / Player.DefaultJumpMultiplier > 1;

            _ShaderArguments["EmissionTint"] = up ? ColorPallete.Up : ColorPallete.Down;
            TextureTransform.Rotation = up ? 180 : 0;
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);
            TextureTransform.Offset.Add(0, -context.Deltatime*.5f);
        }
        public override void BeganCollision(SpecialActor a, Vector2 mtv)
        {
            base.BeganCollision(a, mtv);

            if (a is Player p) p.JumpMultiplier = Multiplier;
        }

        public override void EndCollision(SpecialActor a, Vector2 mtv)
        {
            base.EndCollision(a, mtv);

            if (a is Player p) p.JumpMultiplier = Player.DefaultJumpMultiplier;
        }

        protected override void DrawContext(ref DrawContext context)
        {
            GetMaterialReference().ShaderArguments["ColorScale"] = 1f;
            base.DrawContext(ref context);
        }
    }
}