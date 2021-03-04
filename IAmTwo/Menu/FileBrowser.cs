using System;
using System.Configuration.Assemblies;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
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
        private DrawText _pathViewer;
        private ItemCollection _contents;
        private ItemCollection _folderVisual;

        public override DrawObject2D Background { get; set; }
        public virtual Vector2 Size { get; set; }

        public FileBrowser(string startPath = @".\Levels\test")
        {
            _currentPath = startPath;
            float cornerSize = 10f;
            
            Vector2 halfSize = Size / 2;
            Polygon backgroundMesh = new Polygon(new Vector2[]
            {
                new Vector2(-halfSize.X, halfSize.Y - cornerSize), 
                new Vector2(-halfSize.X + cornerSize, halfSize.Y), 
                new Vector2(halfSize.X, halfSize.Y), 
                new Vector2(halfSize.X, -halfSize.Y + cornerSize),
                new Vector2(halfSize.X - cornerSize, -halfSize.Y),
                new Vector2(-halfSize.X, -halfSize.Y),
            });

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

            _contents = new ItemCollection();
            _contents.Transform.Position.Set(-halfSize.X + 5, halfSize.Y - cornerSize * 2);

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
            
            _contents.Add(pathViewer);

            Add(Background, border, _contents);


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
            _currentPath = path;

            _contents.Remove(_folderVisual);
            _folderVisual = GenerateFolderVisual();
            _contents.Add(_folderVisual);

            _pathViewer.Text = GenerateViewerString(path);
        }

        private ItemCollection GenerateFolderVisual()
        {
            ItemCollection col = new ItemCollection();

            return col;
        }

        public override bool Input()
        {
            return false;
        }
    }
}