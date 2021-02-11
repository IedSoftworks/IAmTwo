using OpenTK;
using SM.Base.Windows;

namespace IAmTwo.Game.Objects
{
    public class GameObject : PhysicsObject
    {
        public GameObject()
        {
            Mass = 100;
            ChecksGrounded = true;

            _ShaderArguments["ColorScale"] = 1f;
        }

        protected override void DrawContext(ref DrawContext context)
        {
            base.DrawContext(ref context);
        }

        public override void Collided(PhysicsObject obj, Vector2 mtv)
        {
            base.Collided(obj, mtv);
        }
    }
}