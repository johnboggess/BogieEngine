#version 330 core

out vec4 FragColor;
in vec2 texCoord;
uniform sampler2D diffuse;
uniform sampler2D mask;

void main()
{
    FragColor = texture(diffuse, texCoord)*texture(mask, texCoord);//mix(texture(diffuse, texCoord), texture(test, texCoord), .5);
}