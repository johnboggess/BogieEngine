#version 330 core

out vec4 FragColor;
in vec2 texCoord;
in vec3 fragPos;
in vec3 normal;

uniform sampler2D diffuse;
uniform sampler2D test;
uniform vec3 LightPosition;
uniform vec3 ViewPosition;

void main()
{
    float specularStrength = 0.5;

    vec3 lightDir = normalize(LightPosition - fragPos);
    vec3 viewDir = normalize(ViewPosition - fragPos);

    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 256);
    vec3 specular = specularStrength * spec * vec3(0,1,1);
    FragColor = vec4(specular, 1.0);
}