#version 330

in vec2 v_TexCoords;
in vec2 vE_ScaledPos;

uniform sampler2D Texture;
uniform vec4 Tint;
uniform float GridSize;

layout(location = 0) out vec4 color;

//# import SM_base_fragment_textureGamma

vec4 texture2DGamma(sampler2D s, vec2 P);

void main() {
	vec2 grid = mod(vE_ScaledPos, GridSize);
	float s = grid.x > GridSize - 2 || grid.y > GridSize - 2 ? 1 : 0;

	vec4 result = vec4((1 - s) + s) * .025;
	color = result;
}