#version 330 core
layout (location = 0) in vec3 Position;
layout (location = 1) in vec2 UV;
layout (location = 2) in vec3 Normal;
layout (location = 3) in vec3 Tangent;
layout (location = 4) in vec3 BiTangent;

uniform mat4 Projection;
uniform mat4 View;
uniform mat4 Model;

out vec2 texCoord;
out vec3 fragPos;
out mat3 TBN;

void main()
{
    gl_Position = Projection*View*Model*vec4(Position, 1.0);

	texCoord = UV;
	fragPos = vec3(Model * vec4(Position, 1.0));

	vec3 T = normalize(vec3(Model * vec4(Tangent,	0.0)));
	vec3 B = normalize(vec3(Model * vec4(BiTangent, 0.0)));
	vec3 N = normalize(vec3(Model * vec4(Normal,	0.0)));

	TBN = mat3(T, B, N);
}