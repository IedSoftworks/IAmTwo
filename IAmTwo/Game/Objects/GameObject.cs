using OpenTK;
using SM.Base.Windows;

namespace IAmTwo.Game.Objects
{
    public class GameObject : PhysicsObject
    {
        public GameObject()
        {
            Mass = 1;
            ChecksGrounded = true;
        }
    }
}