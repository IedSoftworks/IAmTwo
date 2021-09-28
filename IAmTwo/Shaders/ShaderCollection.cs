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
                    u["Texture"].SetTexture(c.Material.Texture, u["HasTexture"]);

                    u["Gamma"].SetFloat(PostProcessUtility.Gamma);

                    u["Emission"].SetTexture(c.Material.ShaderArguments.Get<Texture>("EmissionTex"), u["HasEmission"]);
                    u["EmissionStrength"].SetFloat(c.Material.ShaderArguments.Get("EmissionStrength", 1f));
                    u["EmissionTint"].SetColor(c.Material.ShaderArguments.Get("EmissionTint", c.Material.Tint));

                    u["Tint"].SetColor(c.Material.Tint);
                    u["Scale"].SetFloat(c.Material.ShaderArguments.Get("ColorScale", 1f));
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

                    u["Gamma"].SetFloat(PostProcessUtility.Gamma);
                    u["Tint"].SetColor(GameBackground.Color);

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
                    u["ObjColor"].SetColor(c.Material.Tint);
                    u["Rot"].SetFloat(c.Material.ShaderArguments.Get("Rot", 0f));
                    u["RingLoc"].SetFloat(c.Material.ShaderArguments.Get("RingLoc",   0f));
                }
            }},
            {"PortalConnector", new ImportedShader()
            {
                VertexPreset = "basic",
                VertexFile = "portal_connector_vert.glsl",
                HighFragment = "portal_connector_frag.glsl",
                Uniform = (u, c) =>
                {
                    u["Size"].SetVector2(c.Material.ShaderArguments.Get("ConnectorLength", Vector2.Zero));

                    u["Motion"].SetFloat(c.Material.ShaderArguments.Get("shaderMotion", 0f));

                    List<PortalTraveler> currentTravelers = c.Material.ShaderArguments.Get<List<PortalTraveler>>("Actors");
                    UniformArray uniformArray = u.GetArray("Actors");

                    u["ActorAmount"].SetInt(currentTravelers.Count);
                    for (int i = 0; i < currentTravelers.Count; i++)
                    {
                        Dictionary<string, Uniform> s = uniformArray.Get(i);
                        s["YPos"].SetFloat(currentTravelers[i].CurrentY);
                        s["Reverse"].SetBool(currentTravelers[i].Reverse);
                        s["Color"].SetColor(currentTravelers[i].Color);
                    }

                    u["PortalColor"].SetColor(c.Material.Tint);
                }
            }},
            {"ButtonConnector", new ImportedShader()
            {
                VertexPreset = "basic",
                HighFragment = "button_connector_frag.glsl",
                Uniform = (u, c) =>
                {
                    u["Tint"].SetColor(c.Material.Tint);
                }
            }},
            { "Door", new ImportedShader()
                {
                    VertexPreset = "basic",
                    VertexFile = "portal_connector_vert.glsl",
                    HighFragment = "door_frag.glsl",
                    Uniform = (u, c) =>
                    {
                        u["Size"].SetVector2(c.Material.ShaderArguments.Get("Size", Vector2.One));

                        u["move"].SetFloat(c.Material.ShaderArguments.Get("Move", 0f));
                        u["ObjColor"].SetColor(c.Material.Tint);
                    }
                }
            },
            {"GameObject", new ImportedShader()
            {
                VertexPreset = "basic",
                HighFragment = "gameobject_frag.glsl",
                Uniform = (collection, context) =>
                {
                    collection["xTexScale"].SetFloat(context.Material.ShaderArguments.Get("xTex", 1f));
                    collection["Glow"].SetFloat(GameObject.Glow.X);
                    collection["Gamma"].SetFloat(PostProcessUtility.Gamma);
                }
            }
            },
            {"Goal", new ImportedShader()
            {
                VertexPreset = "basic",
                HighFragment = "goal_frag.glsl",
                Uniform = (u, c) =>
                {
                    u["yPos"].SetFloat(c.Material.ShaderArguments.Get("yPos", 0f));
                    u["Color"].SetColor(c.Material.Tint);

                    u["Brightness"].SetFloat(c.Material.ShaderArguments.Get("brightness", 1f));
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
                    u["Size"].SetVector2((c.UseCamera as Camera).WorldScale.X, (c.UseCamera as Camera).WorldScale.Y);
                    u["GridSize"].SetFloat(LevelEditorGrid.VisualGridSize);
                } 
            }}
        };
    }
}