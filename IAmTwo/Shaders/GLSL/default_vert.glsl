#version 330
#define maxInstances 32
struct Instance {
    mat4 ModelMatrix;
    mat3 TextureMatrix;
};
layout(location = 0) in vec3 a_Position;
layout(location = 1) in vec2 a_Texture;
layout(location = 3) in vec4 a_Color;

uniform mat4 MVP;

uniform mat3 MasterTextureMatrix;
uniform Instance[maxInstances] Instances;
uniform bool HasVColor;

out vec2 v_RealPos;
out vec3 v_VertexPosition;
out vec2 v_TexCoords;
out vec4 v_Color;

void main() {
    v_Color = vec4(1);
    if (HasVColor) v_Color = a_Color;

    v_TexCoords = vec2(MasterTextureMatrix * Instances[gl_InstanceID].TextureMatrix * vec3(a_Texture, 1));

    v_VertexPosition = a_Position;
    gl_Position = MVP * Instances[gl_InstanceID].ModelMatrix * vec4(a_Position, 1);
}