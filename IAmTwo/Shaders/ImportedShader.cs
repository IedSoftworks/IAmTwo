using System;
using SM.Base.Drawing;
using SM.Base.Windows;
using SM.OGL.Shaders;
using SM.Utility;

namespace IAmTwo.Shaders
{
    public class ImportedShader
    {
        public static bool LowComplexity = false;

        private SimpleShader _highShader;

        public string VertexPreset;
        public string VertexExtension = null;

        public string HighFragment;
        public string LowFragment;
        
        public Action<UniformCollection, DrawContext> Uniform;

        public SimpleShader GetShader()
        {
            if (_highShader != null) return _highShader;

            return _highShader = string.IsNullOrEmpty(VertexExtension) ? new SimpleShader(VertexPreset, AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL."+ (LowComplexity ? LowFragment : HighFragment) ), Uniform) : new SimpleShader(VertexPreset, AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL." + VertexExtension), AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL." + (LowComplexity ? LowFragment : HighFragment)), Uniform);
        }
    }
}