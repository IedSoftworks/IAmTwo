using System.Collections.Generic;
using IAmTwo.Game.Objects;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Drawing;
using SM.Base.PostEffects;
using SM.Base.Textures;
using SM.Base.Types;
using SM.OGL.Shaders;
using SM.Utility;

namespace IAmTwo.Shaders
{
    public class ShaderCollection
    {
        public static SimpleShader DefaultShader = new SimpleShader("instanced", AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL.default_frag.glsl"),
            (u, c) =>
            {
                u["Texture"].SetTexture(c.Material.Texture, u["HasTexture"]);

                u["Gamma"].SetUniform1(PostProcessFinals.Gamma);

                u["Emission"].SetTexture(c.Material.ShaderArguments.Get<Texture>("EmissionTex"), u["HasEmission"]);
                u["EmissionStrength"].SetUniform1(c.Material.ShaderArguments.Get("EmissionStrength", 1f));
                u["EmissionTint"].SetUniform4(c.Material.ShaderArguments.Get("EmissionTint", c.Material.Tint));

                u["Tint"].SetUniform4(c.Material.Tint);
                u["Scale"].SetUniform1(c.Material.ShaderArguments.Get("ColorScale", 1f));
            });

        public static SimpleShader PortalShader = new SimpleShader("basic", AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL.spawner_frag.glsl"),
        (u, c) =>
        {
            u["ObjColor"].SetUniform4(c.Material.Tint);
            u["Movement"].SetUniform2(c.Material.ShaderArguments.Get("move", Vector2.Zero));
        });

        public static SimpleShader PortalConnectorShader = new SimpleShader("basic", AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL.portal_connector_vert.glsl"), AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL.portal_connector_frag.glsl"),
            (u, c) =>
            {
                u["Size"].SetUniform2(c.Material.ShaderArguments.Get("ConnectorLength", Vector2.One));

                List<PortalTraveler> currentTravelers = c.Material.ShaderArguments.Get<List<PortalTraveler>>("Actors");
                UniformArray uniformArray = u.GetArray("Actors");

                u["ActorAmount"].SetUniform1(currentTravelers.Count);
                for (int i = 0; i < currentTravelers.Count; i++)
                {
                    Dictionary<string, Uniform> s = uniformArray.Get(i);
                    s["YPos"].SetUniform1(currentTravelers[i].CurrentY);
                    s["Reverse"].SetUniform1(currentTravelers[i].Reverse);
                    s["Color"].SetUniform4(currentTravelers[i].Color);
                }

                u["PortalColor"].SetUniform4(c.Material.Tint);
            });
    }
}