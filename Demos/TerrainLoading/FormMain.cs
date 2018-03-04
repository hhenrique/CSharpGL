using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSharpGL;

namespace TerrainLoading
{
    public partial class FormMain : Form
    {
        private Scene scene;
        private ActionList actionList;

        private TerainNode terrainNode;

        public FormMain()
        {
            InitializeComponent();

            this.Load += FormMain_Load;
            this.winGLCanvas1.OpenGLDraw += winGLCanvas1_OpenGLDraw;
            this.winGLCanvas1.Resize += winGLCanvas1_Resize;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            var rootElement = GetTree();

            //var position = new vec3(5, 3, 4) * 0.2f;
            var position = new vec3(0, 400, -2000);
            var center = new vec3(0, 0, 0);
            var up = new vec3(0, 1, 0);
            var camera = new Camera(position, center, up, CameraType.Perspecitive, this.winGLCanvas1.Width, this.winGLCanvas1.Height);
            this.scene = new Scene(camera)

            {
                RootNode = rootElement,
                ClearColor = Color.SkyBlue.ToVec4(),
            };

            var list = new ActionList();
            var transformAction = new TransformAction(scene.RootNode);
            list.Add(transformAction);
            var renderAction = new RenderAction(scene);
            list.Add(renderAction);
            this.actionList = list;

            var manipulater = new FirstPerspectiveManipulater();
            //var manipulater = new SatelliteManipulater();
            //manipulater.StepLength = 0.1f;
            manipulater.Bind(camera, this.winGLCanvas1);

            SynchronizeModelAndUI();
        }

        private SceneNodeBase GetTree()
        {
            this.terrainNode = TerainNode.Create();
            return this.terrainNode;
        }

        private void SynchronizeModelAndUI()
        {
            this.wirefameCheckBox.Checked = this.terrainNode.RenderAsWireframe;
        }

        private void winGLCanvas1_OpenGLDraw(object sender, PaintEventArgs e)
        {
            ActionList list = this.actionList;
            if (list != null)
            {
                vec4 clearColor = this.scene.ClearColor;
                GL.Instance.ClearColor(clearColor.x, clearColor.y, clearColor.z, clearColor.w);
                GL.Instance.Clear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);

                list.Act(new ActionParams(Viewport.GetCurrent()));
            }
        }

        void winGLCanvas1_Resize(object sender, EventArgs e)
        {
            this.scene.Camera.AspectRatio = ((float)this.winGLCanvas1.Width) / ((float)this.winGLCanvas1.Height);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.rotateCheckBox.Checked)
            {
                var node = this.scene.RootNode;
                if (node != null)
                {
                    node.RotationAngle += 1.3f;
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openDlg.FileName;
                Bitmap bmp = new Bitmap(filename);
                var node = this.scene.RootNode as TerainNode;
                node.UpdateHeightmap(bmp);
            }
        }

        private void wirefameCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.terrainNode.RenderAsWireframe = this.wirefameCheckBox.Checked;
        }
    }

}
