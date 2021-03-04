using System;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;
using IAmTwo.Game;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using SM.Base.Drawing.Text;
using SM.Base.Windows;
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

        public DrawObject2D Background;
        public DrawObject2D Border;
        public DrawText TextObj;
        
        public TextField(Font font, float width = 250)
        {
            float height = font.FontSize * 1.6f;

            Background = new DrawObject2D();
            Background.Color = ColorPallete.DarkBackground;
            Background.Transform.Size.Set(width, height);
            Background.Transform.Position.Set(width / 2, 0);

            Border = new DrawObject2D();
            Border.Mesh = BorderMesh;
            Border.Color = Color4.Blue;
            Border.Transform.Size.Set(width, height);
            Border.ShaderArguments["ColorScale"] = 1.5f;

            TextObj = new DrawText(font, "Hover to type");
            TextObj.Transform.Size.Set(.9f);
            TextObj.Transform.Position.Set(font.FontSize / 2, 0);

            Add(Background, Border, TextObj);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);

            if (_lastCam == null) return;

            if (Mouse2D.MouseOver(Mouse2D.InWorld(_lastCam), Background))
            {
                Border.Color = Color4.LightGreen;
                if (Keyboard.AreSpecificKeysPressed(83, 108, out Key[] keys, true))
                {
                    foreach (Key pressedKey in keys)
                    {
                        string adding = pressedKey.ToString();
                        _text += !Keyboard.IsDown(Key.ControlLeft) ? adding.ToLower() : adding;
                    }

                    TextObj.Text = _text;
                }

                if (Keyboard.IsDown(Key.BackSpace, true) && _text.Length > 0)
                {
                    _text = _text.Remove(_text.Length - 1);
                    TextObj.Text = _text;
                }
            }
            else
            {
                Border.Color = Color4.Blue;
            }
        }

        public override void Draw(DrawContext context)
        {
            _lastCam = context.UseCamera as Camera;

            base.Draw(context);
        }
    }
}