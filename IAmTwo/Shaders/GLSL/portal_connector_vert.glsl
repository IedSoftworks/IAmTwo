#version 330

layout(location = 0) in vec3 a_Position;

uniform vec2 Size;

out vec2 vE_ScaledPos;

void v_Extension() {
	vE_ScaledPos = vec2(a_Position.x, a_Position.y + .5f) * Size * 2;
}