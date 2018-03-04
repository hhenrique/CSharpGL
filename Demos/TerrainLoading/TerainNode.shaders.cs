using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpGL;

namespace TerrainLoading
{
    partial class TerainNode
    {
        private const string vert = @"#version 330 core
  
//uniforms
uniform mat4 MVP;					//combined modelview projection matrix
uniform ivec2 TERRAIN_SIZE;	        //half terrain size
uniform sampler2D heightMapTexture;	//heightmap texture
uniform float scale;				//scale for the heightmap height

out vec3 color;

void main()
{   
    float u = float(gl_VertexID % TERRAIN_SIZE.x) / float(TERRAIN_SIZE.x - 1);
    float v = float(gl_VertexID / TERRAIN_SIZE.x) / float(TERRAIN_SIZE.y - 1);

    float r = texture(heightMapTexture, vec2(u, v)).r;
    float g = texture(heightMapTexture, vec2(u, v)).g;
    float b = texture(heightMapTexture, vec2(u, v)).b;
    float alpha = texture(heightMapTexture, vec2(u, v)).a;

    float x = (u - 0.5) * TERRAIN_SIZE.x;
    float z = (v - 0.5) * TERRAIN_SIZE.y;
	float height = (alpha - 0.5) * scale;

	gl_Position = MVP*vec4(x, height, z, 1);

    if (alpha == 0f)
    {
        color = vec3(1, 1, 1);
    }
    else
    {
        color = vec3(r, g, b);
    }
}
";

        private const string frag = @"#version 330 core

in vec3 color;
uniform bool renderWireframe = false;

layout (location=0) out vec4 vFragColor;	//fragment shader output
 
void main()
{
    //if (renderWireframe)
    //{
	//    vFragColor = vec4(1.0, 1.0, 1.0, 1.0);
    //}
    //else
    {
        vFragColor = vec4(color.r, color.g, color.b, 1);
    }
}
";
    }
}
