#version 330

in vec2 vE_ScaledPos;
in vec3 v_VertexPosition;

uniform float move;
uniform vec4 ObjColor;

//# import SM_base_fragment_noise
float SimplexNoise(vec2 P);

layout(location = 0) out vec4 Color;

void main() {
	float noise = SimplexNoise(abs(vE_ScaledPos) * vec2(1, .1) - vec2(0, move));
	
	float allowedArea = clamp(abs(v_VertexPosition.y) * 2 - 0.5, 0, 1) * 2;

	float result = smoothstep(0, 1, 1 - allowedArea) * noise;
	Color = vec4(result, result, result, result) * ObjColor;
}