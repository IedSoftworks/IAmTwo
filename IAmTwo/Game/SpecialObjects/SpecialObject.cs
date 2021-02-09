using System.Collections.Generic;
using System.Security.AccessControl;
using OpenTK;
using SM.Base.Windows;

namespace IAmTwo.Game.SpecialObjects
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

        public virtual void BeganCollision(Player p, Vector2 mtv)
        {

        }

        public virtual void EndCollision(Player p, Vector2 mtv)
        {

        }
    }
}