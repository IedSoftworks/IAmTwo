using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Windows.Forms;
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

        public virtual void ColliedWithPlayer(SpecialActor a, Vector2 mtv)
        {

        }

        public virtual void BeganCollision(SpecialActor a, Vector2 mtv)
        {

        }

        public virtual void EndCollision(SpecialActor a, Vector2 mtv)
        {

        }
    }
}