#version 330 core
layout (location = 0) in vec3 Position;
layout (location = 1) in vec2 UV;
layout (location = 2) in vec3 Normal;

uniform mat4 Projection;
uniform mat4 View;
uniform mat4 Model;

out vec2 texCoord;
out vec3 fragPos;
out vec3 normal;

void main()
{
    gl_Position = Projection*View*Model*vec4(Position, 1.0);

	texCoord = UV;
	fragPos = vec3(Model * vec4(Position, 1.0));
	normal = mat3(transpose(inverse(Model))) * Normal;
}