using IAmTwo.Game.Objects.SpecialObjects;
using OpenTK;
using SM.Base.Windows;

namespace IAmTwo.Game
{
    public class SpecialActor : PhysicsObject
    {
        private SpecialObject _lastSpecialObject;

        public override void Update(UpdateContext context)
        {
            base.Update(context);

            if (!CollidedWith.Contains(_lastSpecialObject))
            {
                _lastSpecialObject?.EndCollision(this, Vector2.Zero);
                _lastSpecialObject = null;
            }
        }

        public void HandleCollision(SpecialObject special, Vector2 mtv)
        {
            if (special != _lastSpecialObject)
            {
                special.BeganCollision(this, mtv);
                _lastSpecialObject = special;
            }

            special.ColliedWithPlayer(this, mtv);
        }
    }
}