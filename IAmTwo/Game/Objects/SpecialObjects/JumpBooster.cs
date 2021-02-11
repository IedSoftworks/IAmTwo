using IAmTwo.Resources;
using OpenTK;
using SM.Base.Windows;

namespace IAmTwo.Game.Objects.SpecialObjects
{
    public class JumpBooster : SpecialObject
    {
        public static float Multiplier = 40;

        public JumpBooster()
        {
            Color = ColorPallete.Up;
            Texture = Resource.RequestTexture(@".\Resources\jump_arrow.png");
            TextureTransform.Rotation = 180;
            _Material.Blending = true;
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);
            TextureTransform.Offset.Add(0, -context.Deltatime);
        }
        public override void BeganCollision(Player p, Vector2 mtv)
        {
            base.BeganCollision(p, mtv);

            p.JumpMultiplier = Multiplier;
        }

        public override void EndCollision(Player p, Vector2 mtv)
        {
            base.EndCollision(p, mtv);

            p.JumpMultiplier = Player.DefaultJumpMultiplier;
        }

        protected override void DrawContext(ref DrawContext context)
        {
            GetMaterialReference().ShaderArguments["ColorScale"] = 1f;
            base.DrawContext(ref context);
        }
    }
}