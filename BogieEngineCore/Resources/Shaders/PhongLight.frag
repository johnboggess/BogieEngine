#version 330 core

out vec4 FragColor;
in vec2 texCoord;
in vec3 fragPos;
in vec3 normal;

uniform sampler2D diffuseTexture;
uniform sampler2D test;
uniform vec3 LightPosition;
uniform vec3 ViewPosition;

void main()
{
    float ambientStrength = 0.1;
    float specularStrength = 0.5;
    
    vec3 lightDir = normalize(LightPosition - fragPos);
    vec3 viewDir = normalize(ViewPosition - fragPos);
    vec3 reflectDir = reflect(-lightDir, normal);

    vec3 norm = normalize(normal);
    float diff = max(dot(norm, lightDir), 0.0);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);

    vec3 diffuse = diff * vec3(0,1,1);
    vec3 ambient = ambientStrength * vec3(0,1,1);
    vec3 specular = specularStrength * spec * vec3(0,1,1);

    vec3 result = (ambient+diffuse+specular) * texture(diffuseTexture, texCoord).xyz;

    FragColor = vec4(result, 1.0);
}