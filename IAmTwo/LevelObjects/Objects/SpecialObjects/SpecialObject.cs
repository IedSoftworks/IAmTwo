using System.Collections.Generic;
using IAmTwo.Game;
using OpenTK;

namespace IAmTwo.LevelObjects.Objects.SpecialObjects
{
    public abstract class SpecialObject : PhysicsObject, IPlaceableObject
    {
        private List<Player> _enteredPlayers;

        protected SpecialObject()
        {
            ChecksGrounded = false;
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

        public ScaleArgs AllowedScaling { get; protected set; } = ScaleArgs.Default;
        public float AllowedRotationSteps { get; protected set; } = 90;
        public float? TriggerRotation { get; } = null;
        public string Category { get; protected set; } = "Special";
        public Vector2 StartSize { get; protected set; } = new Vector2(100, 33);
    }
}