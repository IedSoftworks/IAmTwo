#version 330

in vec2 vE_FogTexCoords;
in vec2 v_TexCoords;

uniform float Time;
uniform sampler2D FogTex;

uniform vec4 Tint;

layout(location = 0) out vec4 color;

//# import SM_base_fragment_textureGamma
vec4 texture2DGamma(sampler2D s, vec2 P);

//# import SM_base_fragment_noise
float ClassicPerlinNoise(vec2 P);

float st(float value, float start, float width) {
	return smoothstep(start, start + width, value);
}

void main() {
	float texX = fract(v_TexCoords.x);
	float texY = fract(v_TexCoords.y);

	const float width = .025f;
	float x = st(texX, .025 + width, -width) + st(texX, .975 - width, width);
	float y = st(texY, .025 + width, -width) + st(texY, .975 - width, width);

	float border = min(x+y, 1);
	float content = (1 - border) * ClassicPerlinNoise(vec2(texX, texY) * 5);

	float tile = border + content;

	vec3 tileColor = vec3(border) + mix(vec3(.25), vec3(.1), content);

	color = vec4(tileColor * Tint.rgb * .1, 1);

	color += texture2DGamma(FogTex, vE_FogTexCoords) * .005;
}