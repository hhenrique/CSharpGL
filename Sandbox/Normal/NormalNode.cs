﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpGL;
using System.Drawing;

namespace Normal
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NormalNode : PickableNode, IRenderable
    {
        private const string vPosition = "vPosition";
        private const string vTexCoord = "vTexCoord";
        private const string vNormal = "vNormal";
        private const string projectionMatrix = "projectionMatrix";
        private const string viewMatrix = "viewMatrix";
        private const string modelMatrix = "modelMatrix";
        private const string normalMatrix = "normalMatrix";
        private const string diffuseColor = "diffuseColor";
        private const string vertexColor = "vertexColor";
        private const string pointerColor = "pointerColor";
        private const string tex = "tex";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static NormalNode Create(IBufferSource model, string position, string texCoord, string normal, vec3 size)
        {
            var builders = new RenderMethodBuilder[2];
            {
                // render model
                var vs = new VertexShader(vertexShader);
                var fs = new FragmentShader(fragmentShader);
                var provider = new ShaderArray(vs, fs);
                var map = new AttributeMap();
                map.Add(vPosition, position);
                map.Add(vTexCoord, texCoord);
                map.Add(vNormal, normal);
                builders[0] = new RenderMethodBuilder(provider, map);
            }
            {
                // render normal
                var vs = new VertexShader(normalVertex);
                var gs = new GeometryShader(normalGeometry);
                var fs = new FragmentShader(normalFragment);
                var provider = new ShaderArray(vs, gs, fs);
                var map = new AttributeMap();
                map.Add(vPosition, position);
                map.Add(vNormal, normal);
                builders[1] = new RenderMethodBuilder(provider, map);
            }

            var node = new NormalNode(model, position, texCoord, builders);
            string folder = System.Windows.Forms.Application.StartupPath;
            node.TextureSource = new TextureSource(System.IO.Path.Combine(folder, @"superficie.png"));
            node.ModelSize = size;

            node.Initialize();

            return node;
        }

        private NormalNode(IBufferSource model, string positionNameInIBufferSource, string texCoordNameInIBufferSource, params RenderMethodBuilder[] builders)
            : base(model, positionNameInIBufferSource, builders)
        {
            this.RenderModel = true;
            this.RenderNormal = false;
        }

        private ThreeFlags enableRendering = ThreeFlags.BeforeChildren | ThreeFlags.Children | ThreeFlags.AfterChildren;
        /// <summary>
        /// Render before/after children? Render children? 
        /// RenderAction cares about this property. Other actions, maybe, maybe not, your choice.
        /// </summary>
        public ThreeFlags EnableRendering
        {
            get { return this.enableRendering; }
            set { this.enableRendering = value; }
        }

        public ITextureSource TextureSource { get; set; }

        public void RenderBeforeChildren(RenderEventArgs arg)
        {
            ICamera camera = arg.Camera;
            mat4 projection = camera.GetProjectionMatrix();
            mat4 view = camera.GetViewMatrix();
            mat4 model = this.GetModelMatrix();
            mat4 normal = glm.transpose(glm.inverse(view * model));

            if (this.RenderModel)
            {
                RenderMethod method = this.RenderUnit.Methods[0];
                ShaderProgram program = method.Program;

                var source = this.TextureSource;
                if (source != null)
                {
                    program.SetUniform(tex, source.BindingTexture);
                }

                program.SetUniform(projectionMatrix, projection);
                program.SetUniform(viewMatrix, view);
                program.SetUniform(modelMatrix, model);
                program.SetUniform(normalMatrix, normal);

                method.Render();
            }

            if (this.RenderNormal)
            {
                RenderMethod method = this.RenderUnit.Methods[1];
                ShaderProgram program = method.Program;
                program.SetUniform(projectionMatrix, projection);
                program.SetUniform(viewMatrix, view);
                program.SetUniform(modelMatrix, model);

                method.Render();
            }
        }

        public void RenderAfterChildren(RenderEventArgs arg)
        {
        }

        public vec3 DiffuseColor
        {
            get
            {
                vec3 value = new vec3();
                if (this.RenderUnit != null && this.RenderUnit.Methods.Length > 0)
                {
                    RenderMethod method = this.RenderUnit.Methods[0];
                    ShaderProgram program = method.Program;
                    program.GetUniformValue(diffuseColor, out value);
                }

                return value;
            }
            set
            {
                if (this.RenderUnit != null && this.RenderUnit.Methods.Length > 0)
                {
                    RenderMethod method = this.RenderUnit.Methods[0];
                    ShaderProgram program = method.Program;
                    program.SetUniform(diffuseColor, value);
                }
            }
        }

        public vec3 VertexColor
        {
            get
            {
                vec3 value = new vec3();
                if (this.RenderUnit != null && this.RenderUnit.Methods.Length > 0)
                {
                    RenderMethod method = this.RenderUnit.Methods[1];
                    ShaderProgram program = method.Program;
                    program.GetUniformValue(vertexColor, out value);
                }

                return value;
            }
            set
            {
                if (this.RenderUnit != null && this.RenderUnit.Methods.Length > 0)
                {
                    RenderMethod method = this.RenderUnit.Methods[1];
                    ShaderProgram program = method.Program;
                    program.SetUniform(vertexColor, value);
                }
            }
        }

        public vec3 PointerColor
        {
            get
            {
                vec3 value = new vec3();
                if (this.RenderUnit != null && this.RenderUnit.Methods.Length > 0)
                {
                    RenderMethod method = this.RenderUnit.Methods[1];
                    ShaderProgram program = method.Program;
                    program.GetUniformValue(pointerColor, out value);
                }

                return value;
            }
            set
            {
                if (this.RenderUnit != null && this.RenderUnit.Methods.Length > 0)
                {
                    RenderMethod method = this.RenderUnit.Methods[1];
                    ShaderProgram program = method.Program;
                    program.SetUniform(pointerColor, value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool RenderModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RenderNormal { get; set; }
    }
}
