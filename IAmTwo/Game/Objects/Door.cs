using IAmTwo.Game.Objects.SpecialObjects;
using OpenTK;

namespace IAmTwo.Game.Objects
{
    public class Door : GameObject, IButtonTarget
    {
        private Vector2 _oldSize;

        public Door()
        {
            Transform.Size.Changed += () => 
                _oldSize = Transform.Size;
        }

        public void Activation(PressableButton button, SpecialActor trigger)
        { }

        public void Collision(PressableButton button, SpecialActor trigger)
        {
            Transform.Size.Set(0, false);
            CanCollide = false;
        }

        public void Reset(PressableButton button, SpecialActor trigger)
        {
            Transform.Size.Set(_oldSize, false);
            CanCollide = true;
        }
    }
}