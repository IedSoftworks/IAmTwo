#version 330

struct PortaledActor {
    float YPos;
    bool Reverse;
    vec4 Color;
};

in vec2 vE_ScaledPos;

uniform vec2 Size;
uniform PortaledActor[32] Actors;
uniform int ActorAmount;
uniform vec4 PortalColor;

layout(location = 0) out vec4 Color;

void main() {
    vec4 middle = PortalColor * vec4((abs(vE_ScaledPos.x) < 2.5 ? 1 : 0)) * 2;

    vec4 yColor = vec4(0);
    for(int i = 0; i < ActorAmount; i++) {
        PortaledActor Actor = Actors[i];

        float targetY = Actor.YPos;
        if (Actor.Reverse) targetY = Size.y * 2 - targetY;

        yColor += Actor.Color * (1 - clamp(distance(vec2(0, targetY), vE_ScaledPos) - 20, 0, 1));
    }
    vec4 result = middle + vec4(vec3(yColor) * 2, 0);


    Color = result;
}