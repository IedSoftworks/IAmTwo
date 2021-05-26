using IAmTwo.LevelObjects.Objects.SpecialObjects;

namespace IAmTwo.LevelObjects.Objects
{
    public class ButtonConnector : Connector
    {
        public ButtonConnector(PressableButton start, IButtonTarget end) : base(start, end)
        {

        }
    }
}