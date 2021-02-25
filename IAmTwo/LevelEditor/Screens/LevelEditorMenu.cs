using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.LevelEditor
{
    public abstract class LevelEditorMenu : ItemCollection
    {
        public abstract DrawObject2D Background { get; set; }

        public abstract bool Input();

        public virtual void Keybinds() {}

        public virtual void Open() {}
        public virtual void Close() {}
    }
}