using SM2D.Drawing;

namespace IAmTwo.Game
{
    public class BaseGameObject : DrawObject2D
    {
        public int ID { get; set; } = -1;

        public override string ToString()
        {
            return $"{Name} ({ID})";
        }
    }
}