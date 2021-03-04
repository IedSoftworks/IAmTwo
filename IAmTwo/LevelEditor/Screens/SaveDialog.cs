using IAmTwo.Menu;
using OpenTK;

namespace IAmTwo.LevelEditor
{
    public class SaveDialog : FileBrowser
    {
        public override Vector2 Size { get; set; } = new Vector2(700, 400);

        public SaveDialog() : base()
        {

        }

        public void Show()
        {
            LevelEditor.CurrentEditor.CloseAllMenus(this);
            RenderActive = true;
        }

        public void Hide()
        {
            RenderActive = false;
        }
    }
}