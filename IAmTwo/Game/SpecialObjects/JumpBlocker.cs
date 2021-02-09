using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Windows;

namespace IAmTwo.Game.SpecialObjects
{
    public class JumpBlocker : SpecialObject
    {
        public static float Multiplier = 40;

        public JumpBlocker()
        {
            Color = Color4.Red;
            Texture = Resource.RequestTexture(@".\Resources\jump_arrow.png");
            _material.Blending = true;
        }

        public override void BeganCollision(Player p, Vector2 mtv)
        {
            base.BeganCollision(p, mtv);

            p.JumpMultiplier = 0;
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