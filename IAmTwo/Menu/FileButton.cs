using System;
using System.Net;
using System.Windows.Forms;
using IAmTwo.Game;
using IAmTwo.Resources;
using OpenTK.Graphics;
using OpenTK.Input;
using SM.Base.Scene;
using SM.Base.Windows;
using SM2D.Controls;
using SM2D.Drawing;
using SM2D.Scene;
using Mouse = SM.Base.Controls.Mouse;

namespace IAmTwo.Menu
{
    public class FileButton : ItemCollection
    {
        private Camera _lastCamera;
        private DrawObject2D _background;
        private FileBrowser _parent;

        private bool _folder;
        private string _path;

        public string VisibleText { get; private set; }
        public string Path => _path;
        public bool Clicked;
        public float Height;

        public FileButton(FileBrowser parent, float width, string path, bool folder)
        {
            _parent = parent;
            _folder = folder;
            _path = path;

            Height = Fonts.Button.FontSize * 1.6f;

            _background = new DrawObject2D()
            {
                Color = new Color4(0,0,0,0f),
                Material = {Blending = true}
            };
            _background.Transform.Size.Set(width, Height);
            _background.Transform.Position.Set(width / 2 - 10f, 0);

            float iconSize = Height * .9f;

            DrawObject2D fileIcon = new DrawObject2D()
            {
                Texture = Resource.RequestTexture(@".\Resources\fileIcon.png"),
                Material = {Blending = true}
            };
            fileIcon.TextureTransform.Scale.Set(1 / 3f, 1);
            fileIcon.TextureTransform.Offset.Set(fileIcon.TextureTransform.Scale.X * (folder ? 1 : 0), 0);
            fileIcon.Transform.Size.Set(iconSize);

            string[] pathParts = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            VisibleText = pathParts[pathParts.Length - 1];
            DrawText fileText = new DrawText(folder ? Fonts.Button : Fonts.Text, VisibleText);
            fileText.Transform.Position.Set(iconSize * 1.2f, 0);

            Add(_background, fileText, fileIcon);
        }

        public override void Update(UpdateContext context)
        {
            if (Clicked)
            {
                _background.RenderActive = true;
                _background.Color = ColorPallete.LightBackground;

                return;
            }

            if (_lastCamera != null)
            {
                bool check = Mouse2D.MouseOver(Mouse2D.InWorld(_lastCamera), _background);
                if (check)
                {
                    _background.RenderActive = true;
                    _background.Color = ColorPallete.LightBackground;

                    if (Mouse.IsDown(MouseButton.Left, true))
                    {
                        _parent.UpdateSelection(this);

                        if (_folder)
                        {
                            _parent.SetPath(_path);
                        }
                    }
                }
                else
                {
                    _background.RenderActive = false;
                }
            }
        }

        public override void Draw(DrawContext context)
        {
            _lastCamera = context.UseCamera as Camera;

            base.Draw(context);

            _background.RenderActive = false;
        }
    }
}