#version 330

in vec2 v_TexCoords;
in vec4 v_Color;

uniform sampler2D Texture;
uniform bool HasTexture;
uniform vec4 Tint;

uniform float Scale;

layout(location = 0) out vec4 color;

void main() {

	color = v_Color * Tint;
	if (HasTexture) color = texture(Texture, v_TexCoords) * Tint;
	color *= Scale;
}