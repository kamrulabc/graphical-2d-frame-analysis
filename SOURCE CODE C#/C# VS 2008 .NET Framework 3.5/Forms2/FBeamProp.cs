using System.Drawing.Drawing2D;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class FBeamProp : Form
    {
        public FBeamProp()
        {
            InitializeComponent();
        }
        private PointF PS, PE;




        private void FBeamProp_FormClosing(object sender, FormClosingEventArgs e)
        {



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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox1.Text = "0.0225";
                textBox2.Text = "4.21875e-5";
                textBox3.Text = "2e8";


            }
            if (checkBox1.Checked == false)
            {
                textBox1.Text = "0";
                textBox2.Text = "0";
                textBox3.Text = "0";

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (textBox1.Text.Replace(" ", "") == "") textBox1.Text = "0";
            if (textBox2.Text.Replace(" ", "") == "") textBox2.Text = "0";
            if (textBox3.Text.Replace(" ", "") == "") textBox3.Text = "0";
            if (double.Parse(textBox1.Text.Replace(" ", "")) == 0)
            {
                MessageBox.Show(" Area must not be ZERO !!!");
                return;
            }
            if (double.Parse(textBox2.Text.Replace(" ", "")) == 0)
            {
                MessageBox.Show(" Iz must not be ZERO !!!");
                return;
            }
            if (double.Parse(textBox3.Text.Replace(" ", "")) == 0)
            {
                MessageBox.Show(" E must not be ZERO !!!");
                return;
            }



            double t1 = double.Parse(textBox1.Text.Replace(" ", ""));
            double t2 = double.Parse(textBox2.Text.Replace(" ", ""));
            double t3 = double.Parse(textBox3.Text.Replace(" ", ""));
            Form1.Mproperty_A_I_E[Form1.Last_index_line_selected + 1] = (Form1.Last_index_line_selected + 1).ToString() + "," + t1 + "," + t2 + "," + t3;

            for (int k = 1; k <= Form1.totalmember; k++)
            {


                Debug.Print("FBeamProp......Mproperty_A_I_E[k]= " + k.ToString() + "," + Form1.Mproperty_A_I_E[k]);



            }


            string[] test = new string[] { };
            Array.Resize(ref Form1.Iz, Form1.totalmember + 1);
            Array.Resize(ref Form1.Ayz, Form1.totalmember + 1);
            Array.Resize(ref Form1.Ey, Form1.totalmember + 1);

            Array.Resize(ref Form1.Mproperty_A_I_E, Form1.totalmember + 1);
            for (int i = 1; i <= Form1.totalmember; i++)
            {

                if (!string.IsNullOrEmpty(Form1.Mproperty_A_I_E[i])) // 21 June 21 to avoid nul exception
                {
                    test = Form1.Mproperty_A_I_E[i].Split(new char[] { ',' });
                    for (int j = 0; j < test.Length; j++)
                    {

                    }

                    Form1.Ayz[i] = double.Parse(test[1]);
                    Form1.Iz[i] = double.Parse(test[2]);
                    Form1.Ey[i] = double.Parse(test[3]);


                }

                Debug.Print(i + "FBeamProp RRRRRRR Ayz[i], Iz[i],  Ey[i]" + ", " + Form1.Ayz[i] + " , " + Form1.Iz[i] + " , " + Form1.Ey[i] + " , " + Form1.Mproperty_A_I_E[i]);
            }
            //======================================
            Form1.Invoke_Unselect();

            Form1.nodenumer_Selected = -1;
            Form1.selpt = false;
            //====================================



            this.Hide();
            //+++++++++++++++++++++++++++++++
            Form1 frm = new Form1();
            frm.TopMost = true;
            frm.BringToFront();
            frm.WindowState = FormWindowState.Normal;
            //+++++++++++++++++++++++++++++++

            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            ////+ve 
            ////only Delete Selected Text from Textbox and Enter New Char in C#.NET
            if ((sender as TextBox).SelectedText.Length > 0)
            {
                (sender as TextBox).Text = (sender as TextBox).Text.Replace((sender as TextBox).Text.Substring((sender as TextBox).SelectionStart, (sender as TextBox).SelectionLength), "");
            }
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar))
                    && (e.KeyChar != '.'))
                e.Handled = true;

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;



        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            ////+ve 
            ////only Delete Selected Text from Textbox and Enter New Char in C#.NET
            if ((sender as TextBox).SelectedText.Length > 0)
            {
                (sender as TextBox).Text = (sender as TextBox).Text.Replace((sender as TextBox).Text.Substring((sender as TextBox).SelectionStart, (sender as TextBox).SelectionLength), "");
            }
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar))
                    && (e.KeyChar != '.'))
                e.Handled = true;

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;


        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            ////+ve 
            ////only Delete Selected Text from Textbox and Enter New Char in C#.NET
            if ((sender as TextBox).SelectedText.Length > 0)
            {
                (sender as TextBox).Text = (sender as TextBox).Text.Replace((sender as TextBox).Text.Substring((sender as TextBox).SelectionStart, (sender as TextBox).SelectionLength), "");
            }
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar))
                    && (e.KeyChar != '.'))
                e.Handled = true;

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;


        }

        private void button2_Click(object sender, EventArgs e)
        {




            if (textBox1.Text.Replace(" ", "") == "") textBox1.Text = "0";
            if (textBox2.Text.Replace(" ", "") == "") textBox2.Text = "0";
            if (textBox3.Text.Replace(" ", "") == "") textBox3.Text = "0";
            if (double.Parse(textBox1.Text.Replace(" ", "")) == 0)
            {
                MessageBox.Show(" Area must not be ZERO !!!");
                return;
            }
            if (double.Parse(textBox2.Text.Replace(" ", "")) == 0)
            {
                MessageBox.Show(" Iz must not be ZERO !!!");
                return;
            }
            if (double.Parse(textBox3.Text.Replace(" ", "")) == 0)
            {
                MessageBox.Show(" E must not be ZERO !!!");
                return;
            }



            double t1 = double.Parse(textBox1.Text.Replace(" ", ""));
            double t2 = double.Parse(textBox2.Text.Replace(" ", ""));
            double t3 = double.Parse(textBox3.Text.Replace(" ", ""));

            for (int k = 1; k <= Form1.totalmember; k++)
            {

                Form1.Mproperty_A_I_E[k] = k.ToString() + "," + t1 + "," + t2 + "," + t3;

                Debug.Print("FBeamProp......Mproperty_A_I_E[k]= " + k.ToString() + "," + Form1.Mproperty_A_I_E[k]);



            }
            //====================================



            this.Hide();
            //+++++++++++++++++++++++++++++++
            Form1 frm = new Form1();
            frm.TopMost = true;
            frm.BringToFront();
            frm.WindowState = FormWindowState.Normal;
            //+++++++++++++++++++++++++++++++

            this.Close();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (Form1.totalnode >= 2)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                int i = Form1.Last_index_line_selected;

                draw_Member_ON_FORM(i + 1, e.Graphics);
                if (Math.Cos(Form1.Theta(i + 1) * Math.PI / 180) >= 0)
                {
                    Form1.DrawArrow(new Pen(Color.Gray, 1.0F), PS.X, PS.Y, (PS.X + PE.X) / 2, (PS.Y + PE.Y) / 2, 30, e.Graphics);
                }
                else
                {
                    Form1.DrawArrow(new Pen(Color.Gray, 1.0F), PE.X, PE.Y, (PS.X + PE.X) / 2, (PS.Y + PE.Y) / 2, 30, e.Graphics);
                }
            }
        }



        private void draw_Member_ON_FORM(int Mem_ID, Graphics g)
        {

            if (Form1.totalnode >= 2)
            {
                double xx1 = 0;
                double xx2 = 0;
                double yy1 = 0;
                double yy2 = 0;
                xx1 = Form1.mLines[Form1.Last_index_line_selected].StartPoint.X / Form1.GridX;
                yy1 = Form1.mLines[Form1.Last_index_line_selected].StartPoint.Y / Form1.GridX;
                xx2 = Form1.mLines[Form1.Last_index_line_selected].EndPoint.X / Form1.GridX;
                yy2 = Form1.mLines[Form1.Last_index_line_selected].EndPoint.Y / Form1.GridX;

                yy1 = 16 - yy1;
                yy2 = 16 - yy2;



                g.SmoothingMode = SmoothingMode.AntiAlias;
                int i = Form1.Last_index_line_selected;

                int SN = Find_Node_Num_From_Coordinate(Form1.mLines[i].StartPoint);
                int EN = Find_Node_Num_From_Coordinate(Form1.mLines[i].EndPoint);

                PS = new PointF(20, 80);
                PE = new PointF(PS.X + 300, PS.Y);


                g.DrawLine(new Pen(Color.Gray, 1.0F), PS, PE);

                g.DrawString(("MemberNO= " + (i + 1)).ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), (PS.X + PE.X) / 2 - 40, (PS.Y));

                g.DrawString(("Member Lenght= " + Form1.L_n(Form1.Last_index_line_selected + 1) + "m").ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), (PS.X + PE.X) / 2 - 40, (PS.Y + 20));



                if (Math.Cos(Form1.Theta(i + 1) * Math.PI / 180) >= 0)
                {

                    g.DrawString(("Member StartNode= " + SN).ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Magenta), PS.X, PS.Y - 30, new StringFormat(StringFormatFlags.NoClip));
                    g.DrawString(("Member EndNode= " + EN).ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Magenta), PE.X, PE.Y, new StringFormat(StringFormatFlags.NoClip));

                }
                else
                {
                    g.DrawString(("Member EndNode= " + EN).ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Magenta), PS.X - 140, PS.Y, new StringFormat(StringFormatFlags.NoClip));
                    g.DrawString(("Member StartNode= " + SN).ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Magenta), PE.X, PE.Y, new StringFormat(StringFormatFlags.NoClip));
                }

            }
        }

        private int Find_Node_Num_From_Coordinate(PointF p)
        {

            int i = 0;
            int k = 0;
            i = -1;

            for (i = 1; i <= Form1.totalnode; i++)
            {
                if (Form1.nodecoordinate[i].X == p.X && Form1.nodecoordinate[i].Y == p.Y)
                {

                    // k = nodenumer[i];
                    k = i;
                }
            }


            return k;
        }

        private void FBeamProp_Load(object sender, EventArgs e)
        {



            string[] test = new string[] { };
            if (!string.IsNullOrEmpty(Form1.Mproperty_A_I_E[Form1.Last_index_line_selected + 1])) // 21 June 21 to avoid nul exception
            {
                test = Form1.Mproperty_A_I_E[Form1.Last_index_line_selected + 1].Split(new char[] { ',' });
                for (int j = 0; j < test.Length; j++)
                {
                    Debug.Print(j + " , " + test[0] + "  Mproperty_A_I_E[ID]" + "  ############################################## " + test[j]);
                }

                textBox1.Text = test[1].ToString();//Ayz
                textBox2.Text = test[2].ToString();//Iz
                textBox3.Text = test[3].ToString();//Ey

            }

            if (Form1.bool_EDIT == false)
            {
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
            }
            else
            {
                foreach (Control c in this.Controls)
                {
                    c.Enabled = true;
                }
            }
        }



    }
}
