﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpGL;

namespace Normal
{
    public partial class NormalNode
    {
        private const string vertexShader = @"#version 150 core

in vec3 vPosition;
in vec2 vTexCoord;
in vec3 vNormal;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;
uniform mat4 normalMatrix;

out vec3 passPosition; // position in eye space.
out vec2 passTexCoord;
out vec3 passNormal; // normal in eye space.

void main(void)
{
    gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(vPosition, 1.0f);

    passPosition = (viewMatrix * modelMatrix * vec4(vPosition, 1.0f)).xyz;
    passTexCoord = vTexCoord;
    passNormal = (normalMatrix * vec4(vNormal, 0)).xyz;
}
";
        private const string fragmentShader = @"#version 150 core

uniform vec3 ambientColor = vec3(0.2, 0.2, 0.2);
uniform vec3 diffuseColor = vec3(1, 0.8431, 0);

const vec3 lightPosition = vec3(0, 0, 0); // flash light's position in eye space.

in vec3 passPosition;
in vec2 passTexCoord;
in vec3 passNormal;

uniform sampler2D tex;

out vec4 vFragColor;

void main(void)
{
	vec4 color = texture(tex, passTexCoord);

    vec3 L = normalize(lightPosition - passPosition);
    float diffuse = max(0, dot(L, normalize(passNormal)));
    
    //vFragColor = vec4(ambientColor + diffuse * diffuseColor, 1.0f);
    vFragColor = vec4(ambientColor + diffuse * color.xyz, 1.0f);
}
";

    }
}
