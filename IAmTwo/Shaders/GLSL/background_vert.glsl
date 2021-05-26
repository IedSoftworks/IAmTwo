#version 330

layout(location = 0) in vec3 a_Position;
layout(location = 1) in vec2 a_Texture;

uniform mat4 MVP;
uniform mat3 MasterTextureMatrix;

uniform mat4 ModelMatrix;
uniform mat3 FogTextureMatrix;

out vec2 v_TexCoords;
out vec2 vE_FogTexCoords;

void main() {
    v_TexCoords = vec2(MasterTextureMatrix * vec3(a_Texture, 1));
    
    gl_Position = MVP * vec4(a_Position, 1);

	vE_FogTexCoords = vec2( FogTextureMatrix * vec3(a_Texture, 1) );
}