namespace Graphical_2D_Frame_Analysis_CSharp
{
    partial class Foutput
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sAVEASTEXTFILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oPENVIEWNOTEPADOUTPUTTEXTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(13, 77);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(199, 96);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sAVEASTEXTFILEToolStripMenuItem,
            this.oPENVIEWNOTEPADOUTPUTTEXTToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(458, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sAVEASTEXTFILEToolStripMenuItem
            // 
            this.sAVEASTEXTFILEToolStripMenuItem.Name = "sAVEASTEXTFILEToolStripMenuItem";
            this.sAVEASTEXTFILEToolStripMenuItem.Size = new System.Drawing.Size(116, 20);
            this.sAVEASTEXTFILEToolStripMenuItem.Text = "SAVE AS TEXT FILE";
            this.sAVEASTEXTFILEToolStripMenuItem.Click += new System.EventHandler(this.sAVEASTEXTFILEToolStripMenuItem_Click);
            // 
            // oPENVIEWNOTEPADOUTPUTTEXTToolStripMenuItem
            // 
            this.oPENVIEWNOTEPADOUTPUTTEXTToolStripMenuItem.Name = "oPENVIEWNOTEPADOUTPUTTEXTToolStripMenuItem";
            this.oPENVIEWNOTEPADOUTPUTTEXTToolStripMenuItem.Size = new System.Drawing.Size(242, 20);
            this.oPENVIEWNOTEPADOUTPUTTEXTToolStripMenuItem.Text = "OPEN AND VIEW NOTEPAD OUTPUT TEXT";
            this.oPENVIEWNOTEPADOUTPUTTEXTToolStripMenuItem.Click += new System.EventHandler(this.oPENVIEWNOTEPADOUTPUTTEXTToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(103, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Foutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 261);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Foutput";
            this.Text = "Foutput";
            this.Load += new System.EventHandler(this.output_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Foutput_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sAVEASTEXTFILEToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem oPENVIEWNOTEPADOUTPUTTEXTToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}