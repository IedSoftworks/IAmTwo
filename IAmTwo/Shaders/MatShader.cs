using System;
using SM.Base.Shaders;
using SM.Base.Window;
using SM.OGL.Shaders;

namespace IAmTwo.Shaders
{
    public class MatShader : MaterialShader
    {
        public Action<UniformCollection, DrawContext> Uniform;
        public Action<UniformCollection, DrawContext> Vertex;

        public MatShader(string vertex, string fragment) : base(vertex, fragment)
        { }

        protected override void DrawProcess(DrawContext context)
        {
            Uniform.Invoke(Uniforms, context);

            Vertex.Invoke(Uniforms, context);
        }
    }
}