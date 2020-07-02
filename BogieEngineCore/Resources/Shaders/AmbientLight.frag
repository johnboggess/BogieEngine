#version 330 core

out vec4 FragColor;
in vec2 texCoord;
in vec3 normal;
uniform sampler2D diffuse;
uniform sampler2D test;

void main()
{
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * vec3(0,1,1);
    vec3 result = ambient;

    FragColor = vec4(result, 1.0);
}