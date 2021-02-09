using OpenTK;
using OpenTK.Graphics;
using SM.Base.Drawing;
using SM.Base.Scene;
using SM.Base.Windows;
using SM.Utility;
using SM2D.Drawing;

namespace IAmTwo.Game
{
    public class GameObject : PhysicsObject
    {
        public GameObject()
        {
            Mass = 100;
            ChecksGrounded = true;
        }

        protected override void DrawContext(ref DrawContext context)
        {
            GetMaterialReference().ShaderArguments["ColorScale"] = 1f;
            base.DrawContext(ref context);
        }

        public override void Collided(PhysicsObject obj, Vector2 mtv)
        {
            base.Collided(obj, mtv);
        }
    }
}