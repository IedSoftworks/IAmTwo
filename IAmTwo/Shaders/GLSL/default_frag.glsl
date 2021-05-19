#version 330

in vec2 v_RealPos;
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

uniform bool HasMenuRect;
uniform vec4 MenuRectangle;

layout(location = 0) out vec4 color;

//# import SM_base_fragment_textureGamma

vec4 texture2DGamma(sampler2D s, vec2 P);

void main() {
	if (HasMenuRect)
	{
		color = vec4(v_RealPos, 0, 1);
	
		return;
	}


	color = v_Color * Tint;
	if (HasTexture) {
		color *= texture2DGamma(Texture, v_TexCoords);
	}
	color *= Scale;
	
	if (HasEmission) {
		vec4 emission = texture2DGamma(Emission, v_TexCoords);
		emission.a = 0;

		color += emission * EmissionTint * EmissionStrength;
	}
}