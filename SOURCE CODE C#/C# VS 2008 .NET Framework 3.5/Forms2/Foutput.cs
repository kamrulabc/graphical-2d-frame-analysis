using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;


namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class Foutput : Form
    {
        public Foutput()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            // Assign the existing ContextMenuStrip to the RichTextBox
            richTextBox1.ContextMenuStrip = contextMenuStrip1;
        }
        

        private void output_Load(object sender, EventArgs e)
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

            richTextBox1.Text = Form1.stringBuilderALL.ToString();
            
        }


        private void LoadTextFileToRichTextBox(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                richTextBox1.LoadFile(filePath, RichTextBoxStreamType.PlainText);
            }
            else
            {
                MessageBox.Show("File not found.");
            }
        }

        private void sAVEASTEXTFILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Text Files|*.txt",
                Title = "Open a Text File"
            };


            
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Ensure the file has a .txt extension
                    string fileName = saveFileDialog1.FileName;
                    if (!fileName.ToLower().EndsWith(".txt"))
                    {
                        fileName += ".txt";
                    }

                    richTextBox1.SaveFile(fileName, RichTextBoxStreamType.PlainText);
                }
                catch (Exception ex)
                {
                   Debug.Print("Error saving file: " + ex.ToString());
                }
            }

        }

       

        private void oPENVIEWNOTEPADOUTPUTTEXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Text Files|*.txt",//"Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Open a Text File"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = openFileDialog1.FileName;
                    string fileContent = File.ReadAllText(filePath);
                    richTextBox1.Text = fileContent;
                }
                catch (Exception ex)
                {
                    Debug.Print("Error opening file: " + ex.ToString());
                }
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (this.richTextBox1.SelectionLength > 0)
            if (!string.IsNullOrEmpty(richTextBox1.SelectedText))

            {
                Clipboard.SetText(this.richTextBox1.SelectedText);
            }
        }

        private void Foutput_FormClosing(object sender, FormClosingEventArgs e)
        {

            Form1.selpt = false;
            Form1.selln = false;
            Form1.drawln = false;


            Form1.selpt = false;
            Form1.selln = false;
            Form1.drawln = false;


            Form1.bool_FconcP = false;//1
            Form1.bool_FconcM = false;//2
            Form1.bool_FSupDisp = false;//3
            Form1.bool_FUDL = false;//4
            Form1.bool_FNodal = false;//5
            Form1.bool_FSuppData = false;//6
            Form1.bool_FDelSupp = false;//7
            Form1.bool_FBeamProp = false;//8
            Form1.bool_HINGE_Intermediate = false;//9
            Form1.bool_FShowTable = false;//10
            Form1.bool_FDirectSupp = false;//11





            Form1.Invoke_Unselect();
            Form1.nodenumer_Selected = -1;
            Form1.Last_index_line_selected = -1;
        }

        //##################################################    

        //############################################################
    }
}
