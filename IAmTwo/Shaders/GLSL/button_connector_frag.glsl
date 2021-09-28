#version 330

in vec2 v_TexCoords;

uniform vec4 Tint;

layout(location = 0) out vec4 Color;

// From https://thebookofshaders.com/09/
float circle(in vec2 _st, in float _radius){
    vec2 l = _st-vec2(0.5);
    return 1.-smoothstep(_radius-(_radius*0.01),
                         _radius+(_radius*0.01),
                         dot(l,l)*4.0);
}

void main() {
    const float circleSize = .5;

    float c = circle(fract(v_TexCoords), circleSize);
    float b = circle(fract(v_TexCoords), circleSize + .3);
    float alpha = b;

    vec3 col = vec3(c) + b * vec3(.25);

	Color = vec4(col, alpha) * Tint * 3;
}