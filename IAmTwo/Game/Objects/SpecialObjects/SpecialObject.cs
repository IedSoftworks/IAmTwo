using System.Collections.Generic;
using OpenTK;

namespace IAmTwo.Game.Objects.SpecialObjects
{
    public abstract class SpecialObject : GameObject
    {
        private List<Player> _enteredPlayers;

        protected SpecialObject()
        {
            ChecksGrounded = false;
            Transform.Size.Set(100,33);
            TextureTransform.Scale.Set(3f, 1);
        }

        public virtual void ColliedWithPlayer(Player p, Vector2 mtv)
        {

        }

        public virtual void BeganCollision(Player p, Vector2 mtv)
        {

        }

        public virtual void EndCollision(Player p, Vector2 mtv)
        {

        }
    }
}