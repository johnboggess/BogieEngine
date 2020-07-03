#version 330 core

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
}; 

out vec4 FragColor;
in vec2 texCoord;
in vec3 fragPos;
in vec3 normal;

uniform sampler2D diffuseTexture;
uniform sampler2D test;
uniform vec3 LightPosition;
uniform vec3 ViewPosition;

uniform Material material;

void main()
{
    vec3 norm = normalize(normal);
    
    vec3 lightDir = normalize(LightPosition - fragPos);
    vec3 viewDir = normalize(ViewPosition - fragPos);
    vec3 reflectDir = reflect(-lightDir, norm);

    float diff = max(dot(norm, lightDir), 0.0);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    
    vec3 ambient = vec3(1,1,1) * material.ambient;
    vec3 diffuse = vec3(1,1,1) * (diff * material.diffuse);
    vec3 specular = vec3(1,1,1) * (spec * material.specular); 

    vec3 result = (ambient+diffuse+specular) * texture(diffuseTexture, texCoord).xyz;

    FragColor = vec4(result, 1.0);
}