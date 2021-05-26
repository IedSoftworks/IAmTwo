using System;
using System.IO;
using SM.Base.Shaders;
using SM.Base.Utility;
using SM.Base.Window;
using SM.OGL.Shaders;

namespace IAmTwo.Shaders
{
    public class ImportedShader
    {
        public static bool LowComplexity = false;

        private MaterialShader _highShader;

        public string VertexPreset;

        public string VertexFile = null;

        public string HighFragment;
        public string LowFragment;
        
        public Action<UniformCollection, DrawContext> Uniform;

        public MaterialShader GetShader()
        {
            if (_highShader != null) return _highShader;

            string fragment = AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL." + HighFragment);

            if (string.IsNullOrEmpty(VertexFile)) _highShader = new SimpleShader(VertexPreset, fragment, Uniform);
            else
            {
                string vertex = AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL." + VertexFile);
                MatShader shader = (MatShader)(_highShader = new MatShader(vertex, fragment));
                shader.Vertex = SimpleShader.VertexFiles[VertexPreset].Item2;
                shader.Uniform = Uniform;
            }

            return _highShader;
        }
    }
}