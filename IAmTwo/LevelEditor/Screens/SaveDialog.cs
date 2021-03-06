using System.IO;
using IAmTwo.Menu;
using OpenTK;

namespace IAmTwo.LevelEditor
{
    public class SaveDialog : FileBrowser
    {
        public override Vector2 Size { get; set; } = new Vector2(700, 400);

        public SaveDialog() : base(true)
        {

        }

        public override void Proceed()
        {
            string file = !FilePath.EndsWith(".iatl") ? FilePath + ".iatl" : FilePath;

            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.CreateNew);
            LevelEditor.CurrentEditor.Constructor.Store(stream, LevelEditor.CurrentEditor.Objects);
            stream.Close();

            base.Proceed();
        }

        public void Show()
        {
            LevelEditor.CurrentEditor.CloseAllMenus(this);
            Active = true;
        }

        public void Hide()
        {
            Active = false;
        }
    }
}