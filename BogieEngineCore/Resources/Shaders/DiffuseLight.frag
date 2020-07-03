#version 330 core

out vec4 FragColor;
in vec2 texCoord;
in vec3 fragPos;
in vec3 normal;

uniform sampler2D diffuse;
uniform sampler2D test;
uniform vec3 LightPosition;

void main()
{
    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(LightPosition - fragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * vec3(0,1,1);
    FragColor = vec4(diffuse, 1.0);
}