#version 330

struct Instance {
    mat4 ModelMatrix;
    mat3 TextureMatrix;
};

layout(location = 0) in vec3 a_Position;

uniform mat4 Model;
uniform Instance[32] Instances;

out vec2 v_RealPos;

void v_Extension() {
	v_RealPos = vec2(Model * Instances[0].ModelMatrix * vec4(a_Position,1));
}