#version 330

in vec2 v_TexCoords;
in vec4 v_Color;

uniform sampler2D Texture;

uniform sampler2D Emission;
uniform float EmissionStrength;

uniform bool HasTexture;
uniform bool HasEmission;
uniform vec4 Tint;
uniform vec4 EmissionTint;

uniform float Scale;

layout(location = 0) out vec4 color;

//# import SM_base_fragment_textureGamma

vec4 texture2DGamma(sampler2D s, vec2 P);

void main() {

	color = v_Color * Tint;
	if (HasTexture) {
		color *= texture2DGamma(Texture, v_TexCoords);
	}
	color *= Scale;
	
	if (HasEmission) {
		color += texture2DGamma(Emission, v_TexCoords) * EmissionTint * EmissionStrength;
	}
}