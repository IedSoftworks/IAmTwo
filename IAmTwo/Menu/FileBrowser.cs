using System;
using System.Configuration.Assemblies;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Windows.Forms.VisualStyles;
using IAmTwo.Game;
using IAmTwo.LevelEditor;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SM2D.Drawing;
using SM2D.Object;
using SM2D.Scene;

namespace IAmTwo.Menu
{
    public abstract class FileBrowser : LevelEditorMenu
    {
        private string _currentPath;
        private string _startPath;
        private string _fileMask;

        private bool _editable;

        private DrawText _pathViewer;
        private ItemCollection _folderContainer;
        private ItemCollection _folderVisual;

        private FileButton _selection;
        private TextField _fileField;

        protected string FilePath => _editable ? Path.Combine(_currentPath, _fileField.Text) : _selection.Path;

        public override DrawObject2D Background { get; set; }
        public virtual Vector2 Size { get; set; }

        public FileBrowser(bool editable, string startPath = @".\Levels", string fileMask = "*.*")
        {
            _startPath = _currentPath = startPath;
            _fileMask = fileMask;
            _editable = editable;
            
            float cornerSize = 10f;
            
            Vector2 halfSize = Size / 2;
            Polygon backgroundMesh = Models.CreateBackgroundPolygon(Size, cornerSize);

            Background = new DrawObject2D();
            Background.Mesh = backgroundMesh;
            Background.Color = ColorPallete.Background;
            Background.Transform.Size.Set(1);

            DrawObject2D border = new DrawObject2D()
            {
                Mesh = backgroundMesh,
                ForcedMeshType = PrimitiveType.LineLoop,
                Color = Color4.Blue,
                ShaderArguments = { ["LineWidth"] = 2f, ["ColorScale"] = 1.2f }
            };
            border.Transform.Size.Set(1);


            ItemCollection contents = new ItemCollection(); 
            contents.Transform.Position.Set(-halfSize.X + 5, halfSize.Y - cornerSize * 2);


            ItemCollection pathViewer = new ItemCollection();
            pathViewer.Transform.Position.Set(10, 0);

            DrawObject2D pathBackground = new DrawObject2D()
            {
                Color = ColorPallete.DarkBackground
            };
            pathBackground.Transform.Size.Set(Size.X * .97f, Fonts.Text.FontSize * 1.6f);
            pathBackground.Transform.Position.Set(pathBackground.Transform.Size.X / 2, 0);

            _pathViewer = new DrawText(Fonts.Text, GenerateViewerString(_currentPath));
            _pathViewer.Transform.Position.Set(10,0);

            pathViewer.Add(pathBackground, _pathViewer);


            if (editable)
            {
                ImageButton newFolderButton = new ImageButton(Resource.RequestTexture(@".\Resources\fileIcon.png"),
                    17.5f, 17.5f, 5f);
                newFolderButton.TextureTransformation.Scale.Set(1 / 3f, 1);
                newFolderButton.TextureTransformation.Offset.Set(1 / 3f * 2, 0);
                newFolderButton.Transform.Position.Set(20, -30f);
                
                ItemCollection folderCreator = new ItemCollection()
                {
                    Active = false
                };
                folderCreator.Transform.Position.Set(40, -30);

                TextField field = new TextField(Fonts.Button, startText:"Hover to enter folder name", width:500f);

                Button createButton = new Button("Create Folder", 150);
                createButton.Transform.Position.Set(520, 0);
                createButton.Transform.Size.Set(.8f);
                createButton.Click += () =>
                {
                    folderCreator.Active = false;

                    string createPath = _currentPath + "\\" + field.Text;
                    if (Directory.Exists(createPath))
                    {
                        field.Border.Color = Color4.Red;
                    }
                    else
                    {
                        Directory.CreateDirectory(createPath);
                        SetPath(createPath);
                    }
                };

                folderCreator.Add(field, createButton);

                newFolderButton.Click += () => folderCreator.Active = true;

                contents.Add(newFolderButton, folderCreator);
            }


            // Folder Preparations;
            _folderContainer = new ItemCollection();
            _folderContainer.Transform.Position.Set(20, -45f);

            Vector2 backgroundSize = new Vector2(Size.X - 50, Size.Y * .65f);
            DrawObject2D folderBackground = new DrawObject2D()
            {
                Mesh = Models.CreateBackgroundPolygon(backgroundSize, cornerSize),
                Color = ColorPallete.DarkBackground
            };
            folderBackground.Transform.Size.Set(1);
            folderBackground.Transform.Position.Set(backgroundSize.X / 2, -backgroundSize.Y / 2);

            _folderContainer.Add(folderBackground);


            _fileField = new TextField(Fonts.Text, !editable, backgroundSize.X, !editable ? "" : "Hover to enter filename");
            _fileField.Transform.Position.Set(20, -backgroundSize.Y - 65);


            ItemCollection buttonCollection = new ItemCollection();
            buttonCollection.Transform.Size.Set(.8f);
            buttonCollection.Transform.Position.Set(450, -backgroundSize.Y - 95);

            Button proceed = new Button("Proceed", 100);
            proceed.Click += Proceed;

            Button cancel = new Button("Cancel", 90);
            cancel.Transform.Position.Set(150, 0);
            cancel.Click += Cancel;


            buttonCollection.Add(cancel, proceed);


            contents.Add(pathViewer, _folderContainer, _fileField, buttonCollection);

            Add(Background, border, contents);

            SetPath(_currentPath);
        }

        private string GenerateViewerString(string path)
        {
            string[] pathParts = path.Split(new []{'\\'}, StringSplitOptions.RemoveEmptyEntries);
            string finishedString = "";

            foreach (string part in pathParts)
            {
                if (part == ".") continue;
                finishedString += part + ">";
            }

            return finishedString;
        }

        public void SetPath(string path)
        {
            string[] pathParts = path.Split(new[] {"\\"}, StringSplitOptions.RemoveEmptyEntries);
            string newPath = path;
            if (pathParts[pathParts.Length - 1] == "..")
            {
                newPath = "";
                for (var i = 0; i < pathParts.Length - 2; i++)
                {
                    newPath += pathParts[i]+ "\\";
                }


                if (!newPath.Contains(_startPath)) newPath = _startPath;
            }

            _currentPath = newPath;

            if (_folderContainer.Contains(_folderVisual)) _folderContainer.Remove(_folderVisual);
            _folderVisual = GenerateFolderVisual();
            _folderContainer.Add(_folderVisual);

            _pathViewer.Text = GenerateViewerString(newPath);
        }

        private ItemCollection GenerateFolderVisual()
        {
            float offset = 10f;
            _fileField.SetText("");

            ItemCollection col = new ItemCollection();
            col.Transform.Position.Set(10, 0);
            col.Transform.Size.Set(.75f);

            float y = 20;

            FileButton button;
            if (_currentPath != _startPath)
            {
                button = new FileButton(this, (Size.X - 50) * 1.25f, _currentPath + "\\..", true);
                button.Transform.Position.Set(0, -y);
                col.Add(button);

                y += button.Height;
            }

            foreach (string directory in Directory.EnumerateDirectories(_currentPath))
            {
                button = new FileButton(this, (Size.X - 50) * 1.25f, directory, true);
                button.Transform.Position.Set(0, -y);
                col.Add(button);

                y += button.Height;
            }

            foreach (string file in Directory.EnumerateFiles(_currentPath, _fileMask))
            {
                button = new FileButton(this, (Size.X - 50) * 1.25f, file, false);
                button.Transform.Position.Set(0, -y);
                col.Add(button);

                y += button.Height;
            }

            return col;
        }

        public void UpdateSelection(FileButton button)
        {
            if(_selection != null) _selection.Clicked = false;
            _selection = button;
            button.Clicked = true;
            
            _fileField.SetText(button.VisibleText);
        }

        private void Clear()
        {
            Active = false;
            SetPath(_startPath);
        }

        public virtual void Cancel()
        {
            Clear();
        }

        public virtual void Proceed()
        {
            Clear();
        }

        public override bool Input()
        {
            return false;
        }
    }
}