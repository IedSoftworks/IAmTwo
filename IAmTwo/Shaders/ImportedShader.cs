using System;
using SM.Base.Drawing;
using SM.Base.Windows;
using SM.OGL.Shaders;
using SM.Utility;

namespace IAmTwo.Shaders
{
    public class ImportedShader
    {
        private SimpleShader _shader;

        public string VertexPreset;
        public string VertexExtension = null;
        public string Fragment;
        public Action<UniformCollection, DrawContext> Uniform;

        public SimpleShader GetShader()
        {
            if (_shader != null) return _shader;

            return _shader = string.IsNullOrEmpty(VertexExtension) ? new SimpleShader(VertexPreset, AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL."+Fragment), Uniform) : new SimpleShader(VertexPreset, AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL." + VertexExtension), AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL." + Fragment), Uniform);
        }
    }
}