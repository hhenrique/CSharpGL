namespace TerrainLoading
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.winGLCanvas1 = new CSharpGL.WinGLCanvas();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblState = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnOpen = new System.Windows.Forms.Button();
            this.openDlg = new System.Windows.Forms.OpenFileDialog();
            this.rotateCheckBox = new System.Windows.Forms.CheckBox();
            this.wirefameCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.winGLCanvas1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // winGLCanvas1
            // 
            this.winGLCanvas1.AccumAlphaBits = ((byte)(0));
            this.winGLCanvas1.AccumBits = ((byte)(0));
            this.winGLCanvas1.AccumBlueBits = ((byte)(0));
            this.winGLCanvas1.AccumGreenBits = ((byte)(0));
            this.winGLCanvas1.AccumRedBits = ((byte)(0));
            this.winGLCanvas1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.winGLCanvas1.Location = new System.Drawing.Point(12, 44);
            this.winGLCanvas1.Name = "winGLCanvas1";
            this.winGLCanvas1.RenderTrigger = CSharpGL.RenderTrigger.TimerBased;
            this.winGLCanvas1.Size = new System.Drawing.Size(1115, 589);
            this.winGLCanvas1.StencilBits = ((byte)(0));
            this.winGLCanvas1.TabIndex = 0;
            this.winGLCanvas1.TimerTriggerInterval = 40;
            this.winGLCanvas1.UpdateContextVersion = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 639);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1139, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblState
            // 
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(56, 17);
            this.lblState.Text = "state info";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 30;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 13);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 25);
            this.btnOpen.TabIndex = 4;
            this.btnOpen.Text = "&Open ...";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // rotateCheckBox
            // 
            this.rotateCheckBox.AutoSize = true;
            this.rotateCheckBox.Location = new System.Drawing.Point(112, 20);
            this.rotateCheckBox.Name = "rotateCheckBox";
            this.rotateCheckBox.Size = new System.Drawing.Size(58, 17);
            this.rotateCheckBox.TabIndex = 5;
            this.rotateCheckBox.Text = "&Rotate";
            this.rotateCheckBox.UseVisualStyleBackColor = true;
            // 
            // wirefameCheckBox
            // 
            this.wirefameCheckBox.AutoSize = true;
            this.wirefameCheckBox.Location = new System.Drawing.Point(177, 20);
            this.wirefameCheckBox.Name = "wirefameCheckBox";
            this.wirefameCheckBox.Size = new System.Drawing.Size(96, 17);
            this.wirefameCheckBox.TabIndex = 6;
            this.wirefameCheckBox.Text = "&Wireframe only";
            this.wirefameCheckBox.UseVisualStyleBackColor = true;
            this.wirefameCheckBox.CheckedChanged += new System.EventHandler(this.wirefameCheckBox_CheckedChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 661);
            this.Controls.Add(this.wirefameCheckBox);
            this.Controls.Add(this.rotateCheckBox);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.winGLCanvas1);
            this.Name = "FormMain";
            this.Text = "TerrainLoading - CSharpGL";
            ((System.ComponentModel.ISupportInitialize)(this.winGLCanvas1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CSharpGL.WinGLCanvas winGLCanvas1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblState;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.OpenFileDialog openDlg;
        private System.Windows.Forms.CheckBox rotateCheckBox;
        private System.Windows.Forms.CheckBox wirefameCheckBox;
    }
}