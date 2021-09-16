using System;
using IAmTwo.Game;
using IAmTwo.Menu;
using IAmTwo.Menu.MainMenuParts;
using IAmTwo.Resources;
using INIPass;
using OpenTK;
using SM2D.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace IAmTwo.LevelEditor
{
    public class LevelSetEditor : PlayMenu
    {
        private const float OffsetFields = 30;

        public LevelSetEditor(Vector2 _sceneSize) : base(_sceneSize)
        {
            DrawText header = new DrawText(Fonts.Button, "<UNSET>");
            header.Transform.Position.Set(0, -NextPackPos * Offset);
            Packs.Add(header);
            NextPackPos++;

            foreach (string directory in Directory.EnumerateDirectories("Levels"))
            {
                if (LevelSet.LevelSets.Any(a => a.Value.Any(b => b.Folder == directory))) continue;

                Button btn = new Button(directory, 150, allowBorder: false);
                btn.Transform.Position.Set(0, -NextPackPos * Offset);
                btn.Click += () => Edit(directory);

                Packs.Add(btn);
                NextPackPos++;
            }
        }

        protected override void Close()
        {
            LevelEditorMainMenu.Scene.HidePackageManager();

            if (_openFile != null && _fileChanged)
            {
                File.WriteAllText(_openPath, _openFile.Compile());
            }
        }

        protected override void SetLevelSelect(LevelSet set)
        {
            Edit(set.Folder);
        }

        private string _openPath;
        private INIFile _openFile;
        private bool _fileChanged;

        protected void Edit(string directory)
        {
            if (_openFile != null && _fileChanged)
            {
                File.WriteAllText(_openPath, _openFile.Compile());
            }

            _fileChanged = false;
            _levelSelect.Clear();
            string path = Path.Combine(directory, "pack.ini");
            
            INIFile f = new INIFile();

            if (File.Exists(path))
            {
                f = INIFile.Load(path);
            }
            else
            {
                f.Add("General", new INISection()
                {
                    {"Name", ""},
                    {"Category", "Others~0"}
                });

                f.Add("Levels", new INISection());
            }

            _openPath = path;
            _openFile = f;

            INISection general = f["General"];

            DrawText nameHead = new DrawText(Fonts.Button, "Name:");
            TextField nameField = new TextField(Fonts.Button, width: 340);
            nameField.Transform.Position.Set(50, 0);
            nameField.Changed += () =>
            {
                general["Name"].FirstValue.Data = nameField.Text;
                _fileChanged = true;
            };

            DrawText categoryHead = new DrawText(Fonts.Button, "Category:");
            categoryHead.Transform.Position.Set(0, -OffsetFields);
            TextField categoryField = new TextField(Fonts.Button, width:300);
            categoryField.Transform.Position.Set(90, -OffsetFields);
            
            DrawText posHead = new DrawText(Fonts.Button, "Position:");
            posHead.Transform.Position.Set(0, -OffsetFields * 2);
            TextField posField = new TextField(Fonts.Button, width: 300)
            {
                KeyRangeMin = 109,
                KeyRangeMax = 118
            };
            posField.Transform.Position.Set(90, -OffsetFields * 2);

            Action setCategory = () =>
            {
                general["Category"].FirstValue.Data = $"{categoryField.Text}~{posField.Text}";
                _fileChanged = true;
            };

            categoryField.Changed += setCategory;
            posField.Changed += setCategory;
            
            _levelSelect.Add(nameHead, nameField, categoryHead, categoryField, posHead, posField);

            nameField.SetText(general["Name"], true);

            string[] splits = general["Category"].FirstString.Split('~');
            categoryField.SetText(splits[0], true);
            posField.SetText(splits[1], true);
        }
    }
}