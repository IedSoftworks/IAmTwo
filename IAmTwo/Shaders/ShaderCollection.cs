using System.Collections.Generic;
using IAmTwo.Game;
using IAmTwo.LevelEditor;
using IAmTwo.LevelObjects.Objects;
using OpenTK;
using SM.Base.Drawing;
using SM.Base.PostEffects;
using SM.Base.Textures;
using SM.OGL.Shaders;
using SM2D.Scene;

namespace IAmTwo.Shaders
{
    public class ShaderCollection
    {
        public static Dictionary<string, ImportedShader> Shaders = new Dictionary<string, ImportedShader>()
        {
            {"Default", new ImportedShader()
            {
                VertexPreset = "instanced",
                HighFragment = "default_frag.glsl",
                Uniform = (u, c) =>
                {
                    u["Model"].SetMatrix4(c.ModelMatrix);

                    u["HasMenuRect"].SetUniform1(c.Material.ShaderArguments.ContainsKey("MenuRect"));
                    u["MenuRectangle"].SetUniform4(c.Material.ShaderArguments.Get("MenuRect", Vector4.Zero));

                    u["Texture"].SetTexture(c.Material.Texture, u["HasTexture"]);

                    u["Gamma"].SetUniform1(PostProcessUtility.Gamma);

                    u["Emission"].SetTexture(c.Material.ShaderArguments.Get<Texture>("EmissionTex"), u["HasEmission"]);
                    u["EmissionStrength"].SetUniform1(c.Material.ShaderArguments.Get("EmissionStrength", 1f));
                    u["EmissionTint"].SetUniform4(c.Material.ShaderArguments.Get("EmissionTint", c.Material.Tint));

                    u["Tint"].SetUniform4(c.Material.Tint);
                    u["Scale"].SetUniform1(c.Material.ShaderArguments.Get("ColorScale", 1f));
                }
            }},
            {"Background", new ImportedShader()
            {
                VertexPreset = "basic",
                VertexFile = "background_vert.glsl",
                HighFragment = "background_frag.glsl",
                Uniform = (u, c) =>
                {
                    u["Texture"].SetTexture(c.Material.Texture, u["HasTexture"]);

                    u["Gamma"].SetUniform1(PostProcessUtility.Gamma);
                    u["Tint"].SetUniform4(GameBackground.Color);

                    u["FogTextureMatrix"].SetMatrix3(GameRenderPipeline.BloomAmountTransform.GetMatrix());
                    u["FogTex"].SetTexture(GameRenderPipeline.AmountTex);
                }
            }},
            {"Portal", new ImportedShader()
            {
                VertexPreset = "basic",
                HighFragment = "spawner_frag.glsl",
                Uniform = (u, c) =>
                {
                    u["ObjColor"].SetUniform4(c.Material.Tint);
                    u["Rot"].SetUniform1(c.Material.ShaderArguments.Get("Rot", 0f));
                    u["RingLoc"].SetUniform1(c.Material.ShaderArguments.Get("RingLoc",   0f));
                }
            }},
            {"PortalConnector", new ImportedShader()
            {
                VertexPreset = "basic",
                VertexFile = "portal_connector_vert.glsl",
                HighFragment = "portal_connector_frag.glsl",
                Uniform = (u, c) =>
                {
                    u["Size"].SetUniform2(c.Material.ShaderArguments.Get("ConnectorLength", Vector2.Zero));

                    u["Motion"].SetUniform1(c.Material.ShaderArguments.Get("shaderMotion", 0f));

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
                }
            }},
            {"ButtonConnector", new ImportedShader()
            {
                VertexPreset = "basic",
                HighFragment = "button_connector_frag.glsl",
                Uniform = (u, c) =>
                {
                    u["Tint"].SetUniform4(c.Material.Tint);
                }
            }},
            { "Door", new ImportedShader()
                {
                    VertexPreset = "basic",
                    VertexFile = "portal_connector_vert.glsl",
                    HighFragment = "door_frag.glsl",
                    Uniform = (u, c) =>
                    {
                        u["Size"].SetUniform2(c.Material.ShaderArguments.Get("Size", Vector2.One));

                        u["move"].SetUniform1(c.Material.ShaderArguments.Get("Move", 0f));
                        u["ObjColor"].SetUniform4(c.Material.Tint);
                    }
                }
            },
            {"GameObject", new ImportedShader()
            {
                VertexPreset = "basic",
                HighFragment = "gameobject_frag.glsl",
                Uniform = (collection, context) =>
                {
                    collection["xTexScale"].SetUniform1(context.Material.ShaderArguments.Get("xTex", 1f));
                    collection["Glow"].SetUniform1(GameObject.Glow.X);
                    collection["Gamma"].SetUniform1(PostProcessUtility.Gamma);
                }
            }
            },
            {"Goal", new ImportedShader()
            {
                VertexPreset = "basic",
                HighFragment = "goal_frag.glsl",
                Uniform = (u, c) =>
                {
                    u["yPos"].SetUniform1(c.Material.ShaderArguments.Get("yPos", 0f));
                    u["Color"].SetUniform4(c.Material.Tint);

                    u["Brightness"].SetUniform1(c.Material.ShaderArguments.Get("brightness", 1f));
                }
            }}
        };

        public static Dictionary<string, ImportedShader> EditorShader = new Dictionary<string, ImportedShader>
        {
            {"GridBackground", new ImportedShader()
            {
                VertexPreset = "basic",
                VertexFile = "Editor.grid_vert.glsl",
                HighFragment = "Editor.grid_frag.glsl",
                Uniform = (u, c) =>
                {
                    u["Size"].SetUniform2((c.UseCamera as Camera).WorldScale.X, (c.UseCamera as Camera).WorldScale.Y);
                    u["GridSize"].SetUniform1(LevelEditorGrid.VisualGridSize);
                } 
            }}
        };
    }
}