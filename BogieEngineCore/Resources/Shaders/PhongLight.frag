﻿#version 330 core

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};

struct Light {
    vec3 position;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
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
uniform Light light;

void main()
{
    vec3 norm = normalize(normal);
    
    vec3 lightDir = normalize(light.position - fragPos);
    vec3 viewDir = normalize(ViewPosition - fragPos);
    vec3 reflectDir = reflect(-lightDir, norm);

    float diff = max(dot(norm, lightDir), 0.0);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, texCoord));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, texCoord));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, texCoord));

    vec3 result = (ambient+diffuse+specular);

    FragColor = vec4(result, 1.0);
}