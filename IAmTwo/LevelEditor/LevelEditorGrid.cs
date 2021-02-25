﻿using IAmTwo.Game;
using IAmTwo.Shaders;
using SM.Base.Windows;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.LevelEditor
{
    public class LevelEditorGrid : GameBackground
    {
        public static float GridSize = 10;
        public static float VisualGridSize = 20;

        public LevelEditorGrid(Camera cam): base(cam)
        {
            Material.CustomShader = ShaderCollection.EditorShader["GridBackground"].GetShader();
        }

        protected override void DrawContext(ref DrawContext context)
        {
            
            base.DrawContext(ref context);
        }
    }
}