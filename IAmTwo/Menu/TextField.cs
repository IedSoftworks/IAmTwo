using System;
using IAmTwo.Game;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using SM.Base.Drawing.Text;
using SM.Base.Window;
using SM2D.Controls;
using SM2D.Drawing;
using SM2D.Object;
using SM2D.Scene;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo.Menu
{
    public class TextField : ItemCollection
    {
        public static PolyLine BorderMesh = new PolyLine(new Vector2[]
        {
            new Vector2(0, .5f), 
            new Vector2(1, .5f),
            new Vector2(0, -.5f), 
            new Vector2(1, -.5f), 
        });

        private Camera _lastCam;
        private string _text;
        private bool _readOnly;

        public DrawObject2D Background;
        public DrawObject2D Border;
        public DrawText TextObj;
        public string Text => _text;

        public event Action Changed;
        
        public TextField(Font font, bool readOnly = false, float width = 250, string startText = "Hover to type")
        {
            float height = font.FontSize * 1.6f;
            _readOnly = readOnly;

            Background = new DrawObject2D();
            Background.Color = ColorPallete.DarkBackground;
            Background.Transform.Size.Set(width, height);
            Background.Transform.Position.Set(width / 2, 0);

            Border = new DrawObject2D();
            Border.Mesh = BorderMesh;
            Border.Color = _readOnly ? Color4.Green : Color4.Blue;
            Border.Transform.Size.Set(width, height);
            Border.ShaderArguments["ColorScale"] = 1.5f;

            TextObj = new DrawText(font, startText);
            TextObj.Transform.Size.Set(.9f);
            TextObj.Transform.Position.Set(font.FontSize / 2, 0);

            Add(Background, Border, TextObj);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);

            if (_lastCam == null || _readOnly) return;

            if (Mouse2D.MouseOver(Mouse2D.InWorld(_lastCam), Background))
            {
                Border.Color = Color4.LightGreen;
                if (Keyboard.AreSpecificKeysPressed(83, 119, out Key[] keys, true))
                {
                    foreach (Key pressedKey in keys)
                    {
                        string adding = pressedKey.ToString();
                        if (adding.StartsWith("Number")) adding = adding.Remove(0, 6);

                        _text += Keyboard.IsUp(Key.ShiftLeft) && Keyboard.IsUp(Key.ShiftRight) ? adding.ToLower() : adding;
                    }

                    TextObj.Text = _text;
                    Changed?.Invoke();
                }

                if (Keyboard.IsDown(Key.Space, true))
                {
                    _text += " ";
                    TextObj.Text = _text;
                    Changed?.Invoke();
                }

                if (Keyboard.IsDown(Key.BackSpace, true) && _text.Length > 0)
                {
                    _text = _text.Remove(_text.Length - 1);

                    TextObj.Text = _text;
                    Changed?.Invoke();
                }
            }
            else
            {
                Border.Color = Color4.Blue;
            }
        }

        public void SetText(string text, bool force = false)
        {
            if (!string.IsNullOrEmpty(_text) || force) TextObj.Text = _text = text;
        }

        public override void Draw(DrawContext context)
        {
            _lastCam = context.UseCamera as Camera;

            base.Draw(context);
        }
    }
}