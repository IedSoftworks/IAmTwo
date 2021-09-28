using IAmTwo.LevelObjects.Objects.SpecialObjects;
using OpenTK;
using SM.Base.Scene;
using SM.Base.Window;

namespace IAmTwo.Game
{
    public class SpecialActor : PhysicsObject, IScriptable
    {
        protected SpecialObject _lastSpecialObject;

        public bool UpdateActive { get; set; } = true;
        public virtual void Update(UpdateContext context)
        {
            if (!CanCollide) return;
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