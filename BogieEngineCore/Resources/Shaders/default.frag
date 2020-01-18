#version 330 core

out vec4 FragColor;
in vec2 texCoord;
uniform sampler2D diffuse;
uniform sampler2D test;

void main()
{
    FragColor = texture(diffuse, texCoord);//mix(texture(diffuse, texCoord), texture(test, texCoord), .5);
}