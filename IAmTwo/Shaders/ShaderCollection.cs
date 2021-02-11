using OpenTK;
using SM.Base.Drawing;
using SM.Base.Types;
using SM.Utility;

namespace IAmTwo.Shaders
{
    public class ShaderCollection
    {
        public static SimpleShader DefaultShader = new SimpleShader("instanced", AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL.default_frag.glsl"),
            (u, c) =>
            {
                u["Texture"].SetTexture(c.Material.Texture, u["HasTexture"]);
                u["Tint"].SetUniform4(c.Material.Tint);
                u["Scale"].SetUniform1(c.Material.ShaderArguments.Get("ColorScale", 1f));
            });

        public static SimpleShader PortalShader = new SimpleShader("basic", AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL.spawner_frag.glsl"),
        (u, c) =>
        {
            u["ObjColor"].SetUniform4(c.Material.Tint);

            CVector2 move = c.Material.ShaderArguments.Get<CVector2>("move");
            Vector2 movement;
            if (move != null) movement = (Vector2) move;
            else movement = Vector2.Zero;

            u["Movement"].SetUniform2(movement);
        });
    }
}