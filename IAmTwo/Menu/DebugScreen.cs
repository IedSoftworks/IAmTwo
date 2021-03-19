using IAmTwo.Resources;
using SM.Base.Utility;
using SM.Base.Window;
using SM2D.Drawing;
using SM2D.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmTwo.Menu
{
    class DebugScreen : ItemCollection
    {
        public static DebugScreen Screen = new DebugScreen();

        DrawText _text;

        private DebugScreen()
        {
            _text = new DrawText(Fonts.Text, "0ms");
            _text.Transform.Position.Set(-400, 200);
            Add(_text);
        }

        public override void Draw(DrawContext context)
        {
            base.Draw(context);

            _text.Text = Math.Floor(Deltatime.RenderDelta * 1000) + "ms";
        }
    }
}
