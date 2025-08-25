using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class FABOUT : Form
    {
        public FABOUT()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            
        }

        private void FABOUT_Load(object sender, EventArgs e)
        {
            richTextBox1.Clear();


            richTextBox1.WordWrap = false;
            // Dock the RichTextBox to fill the entire form
            richTextBox1.Dock = DockStyle.Fill;


            richTextBox1.AutoSize = true;


            richTextBox1.Multiline = true;
            richTextBox1.MaxLength = int.MaxValue;


            this.WindowState = FormWindowState.Maximized;

            richTextBox1.Font = new Font("Consolas", 11, FontStyle.Regular);


            richTextBox1.Text = richTextBox1.Text.Replace(Environment.NewLine, Environment.NewLine + Environment.NewLine);

            richTextBox1.Text = @"Graphical 2D Frame Analysis Graphical 2D Frame Analysis C Sharp
-Fully open source
Author
Md. Kamrul Hassan
•	B.Sc. in Civil Engineering, Bangladesh University of Engineering and Technology (BUET)
•	Email:    kamrulabc@gmail.com
•	GitHub:   github.com/kamrulabc
•	LinkedIn: linkedin.com/in/md-k-hassan-8a996a378
•   Youtube:  https://www.youtube.com/watch?v=INi5fiNNsJ8

It is designed for:
- Civil & Structural Engineering students and teachers  
 - Developers interested in computational mechanics  

## Overview
-This project is a C# implementation of the Matrix Method for graphical structural analyzing many different types of 2D frame structures.  
";


        }
    }
}
