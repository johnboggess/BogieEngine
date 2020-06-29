#version 330 core

out vec4 FragColor;
in vec2 texCoord;
uniform sampler2D diffuse;

void main()
{
    FragColor = texture(diffuse, texCoord*5);
}