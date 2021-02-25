#version 330

in vec3 v_VertexPosition;
in vec2 v_TexCoords;

uniform vec4 ObjColor;
uniform vec2 Movement;

layout(location = 0) out vec4 Color;

//# import SM_base_fragment_noise
float ClassicPerlinNoise(vec2 P);

void main() {
    float outerRing = clamp(length(v_VertexPosition.xy * 2) - .25, 0,1);
    float innerRing = 1 - outerRing;

    float noise = ClassicPerlinNoise((Movement) * (innerRing * 10 + 15)) * ClassicPerlinNoise(v_VertexPosition.xy * 20);

    float result = noise * 2 + outerRing * 1.5;
    vec3 resultingCol = vec3(result, result, result) * ObjColor.rgb;

    Color = vec4(resultingCol, 1);
}