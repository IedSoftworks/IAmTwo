using IAmTwo.Game;
using IAmTwo.Shaders;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.LevelEditor
{
    public class LevelEditorGrid : GameBackground
    {
        public static float GridSize = 10;
        public static float VisualGridSize = 20;

        private DrawObject2D _obj;

        public LevelEditorGrid(Camera cam): base(cam)
        {
            _obj = new DrawObject2D
            {
                Material =
                {
                    CustomShader = ShaderCollection.EditorShader["GridBackground"].GetShader(),
                    Blending = true
                }
            };
        }

        protected override void DrawContext(ref SM.Base.Window.DrawContext context)
        {
            base.DrawContext(ref context);
            _obj.Draw(context);
        }
    }
}