using System.Drawing.Drawing2D;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class FconcP : Form
    {
        public FconcP()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }
        private PointF PS, PE;

        private bool bool_Inclided_Member = false;
        private string Pload2_Direction = "LY";


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
                g.SmoothingMode = SmoothingMode.AntiAlias;
                int i = Form1.Last_index_line_selected;

                int SN = Find_Node_Num_From_Coordinate(Form1.mLines[i].StartPoint);
                int EN = Find_Node_Num_From_Coordinate(Form1.mLines[i].EndPoint);

                PS = new PointF(300, 20);
                PE = new PointF(PS.X + 300, PS.Y);


                g.DrawLine(new Pen(Color.Gray, 1.0F), PS, PE);

                g.DrawString(("BeamNO= " + (i + 1)).ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), (PS.X + PE.X) / 2 - 40, (PS.Y));

                g.DrawString(("Member Lenght= " + Form1.L_n(Form1.Last_index_line_selected + 1) + "m").ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), (PS.X + PE.X) / 2 - 40, (PS.Y + 20));



                if (Math.Cos(Form1.Theta(i + 1) * Math.PI / 180) >= 0)
                {
                    g.DrawString(("Member StartNode= " + SN).ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Magenta), PS.X - 140, PS.Y, new StringFormat(StringFormatFlags.NoClip));
                    g.DrawString(("Member EndNode= " + EN).ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Magenta), PE.X, PE.Y, new StringFormat(StringFormatFlags.NoClip));

                }
                else
                {
                    g.DrawString(("Member EndNode= " + EN).ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Magenta), PS.X - 140, PS.Y, new StringFormat(StringFormatFlags.NoClip));
                    g.DrawString(("Member StartNode= " + SN).ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Magenta), PE.X, PE.Y, new StringFormat(StringFormatFlags.NoClip));
                }

            }
        }

        private void FconcP_Load(object sender, EventArgs e)
        {



            if (!string.IsNullOrEmpty(Form1.Pload2[Form1.Last_index_line_selected + 1]))
            {

                if (Form1.Find_All_Zero_Ploady_Sring(Form1.Pload2[Form1.Last_index_line_selected + 1]) == true)
                {
                    if ((Form1.mLines[Form1.Last_index_line_selected].StartPoint.X == Form1.mLines[Form1.Last_index_line_selected].EndPoint.X) | (Form1.mLines[Form1.Last_index_line_selected].StartPoint.Y == Form1.mLines[Form1.Last_index_line_selected].EndPoint.Y))
                    {
                        bool_Inclided_Member = false;
                        Pload2_Direction = "LY";
                        radioButton1.Checked = true;
                        radioButton2.Enabled = false;

                    }
                    if ((Form1.mLines[Form1.Last_index_line_selected].StartPoint.X != Form1.mLines[Form1.Last_index_line_selected].EndPoint.X) & (Form1.mLines[Form1.Last_index_line_selected].StartPoint.Y != Form1.mLines[Form1.Last_index_line_selected].EndPoint.Y))
                    {

                        bool_Inclided_Member = true;
                        radioButton1.Enabled = true;
                        radioButton2.Enabled = true;
                        Debug.Print(" FconcP_Load=333333333333333");
                        Debug.Print(" Form1.Pload2[Form1.Last_index_line_selected + 1]333= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);
                    }
                }


                if (Form1.Find_All_Zero_Ploady_Sring(Form1.Pload2[Form1.Last_index_line_selected + 1]) == false)
                {
                    Debug.Print(" Find_k_Index_Ploady_Sring, 13= " + Form1.Find_k_Index_Ploady_Sring(Form1.Pload2[Form1.Last_index_line_selected + 1], 13));
                    if (Form1.Find_k_Index_Ploady_Sring(Form1.Pload2[Form1.Last_index_line_selected + 1], 13) == "GY")
                    {
                        radioButton1.Checked = false;
                        radioButton1.Enabled = true;
                        radioButton2.Enabled = false;
                        radioButton2.Checked = true;
                        bool_Inclided_Member = true;
                        Pload2_Direction = "GY";
                    }
                    if (Form1.Find_k_Index_Ploady_Sring(Form1.Pload2[Form1.Last_index_line_selected + 1], 13) == "LY")
                    {
                        radioButton1.Checked = true;
                        radioButton2.Enabled = false;

                        bool_Inclided_Member = false;
                        Pload2_Direction = "LY";
                    }
                }

            }


            string[] test = new string[] { };

            try
            {


                if (!string.IsNullOrEmpty(Form1.Pload2[Form1.Last_index_line_selected + 1]))
                {

                    test = Form1.Pload2[Form1.Last_index_line_selected + 1].Split(new char[] { ',' });
                    for (int i = 0; i < test.Length; i++)
                    {


                    }


                    textBox1.Text = (double.Parse(test[1].Replace(" ", ""))).ToString();
                    textBox2.Text = test[2];
                    textBox3.Text = (double.Parse(test[3].Replace(" ", ""))).ToString();
                    textBox4.Text = test[4];
                    textBox5.Text = (double.Parse(test[5].Replace(" ", ""))).ToString();
                    textBox6.Text = test[6];
                }

            }
            catch (Exception)
            {

            }



            if (Form1.Find_All_Zero_Ploady_Sring(Form1.Pload2[Form1.Last_index_line_selected + 1]) == true)
            {
                textBox1.Text = "-800";
                textBox2.Text = "1";
            }


            string res = null;
            res = Form1.Find_REPLACE_k_Index_Ploady_Sring("2,5,1,30,2,0,0,0,0,0,0,0,0,LY", 3, "23.5");






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


                    k = i;
                }
            }


            return k;
        }

        private void button1_Click(object sender, EventArgs e)
        {
try
{
   
            if (textBox1.Text.Replace(" ", "") == "") textBox1.Text = "0";
            if (textBox2.Text.Replace(" ", "") == "") textBox2.Text = "0";
            if (textBox3.Text.Replace(" ", "") == "") textBox3.Text = "0";
            if (textBox4.Text.Replace(" ", "") == "") textBox4.Text = "0";
            if (textBox5.Text.Replace(" ", "") == "") textBox5.Text = "0";
            if (textBox6.Text.Replace(" ", "") == "") textBox6.Text = "0";

            string t1 = "";
            string t2 = "";
            string t3 = "";
            string t4 = "";
            string t5 = "";
            string t6 = "";
            t1 = textBox1.Text.Replace(" ", "");
            t2 = textBox2.Text.Replace(" ", "");
            t3 = textBox3.Text.Replace(" ", "");
            t4 = textBox4.Text.Replace(" ", "");
            t5 = textBox5.Text.Replace(" ", "");
            t6 = textBox6.Text.Replace(" ", "");
            double d = 0;

            d = Form1.L_n(Form1.Last_index_line_selected + 1);

            if (double.Parse(t1) == 0)
            {
                MessageBox.Show(" CONC LOAD P1 must be GRATER THAN ZERO !!!");
                return;
            }

            if (double.Parse(t1) != 0 & (double.Parse(t2) >= d | double.Parse(t2) == 0))
            {
                MessageBox.Show(" Distance must be MORE than ZERO or LESS than BEAM LENGTH!!!");
                return;
            }
            if (double.Parse(t3) != 0 & (double.Parse(t4) >= d | double.Parse(t4) == 0))
            {
                MessageBox.Show(" Distance must be MORE than ZERO or LESS than BEAM LENGTH!!!");
                return;
            }
            if (double.Parse(t5) != 0 & (double.Parse(t6) >= d | double.Parse(t6) == 0))
            {
                MessageBox.Show(" Distance must be MORE than ZERO or LESS than BEAM LENGTH!!!");
                return;
            }
            if (double.Parse(t2) != 0 & double.Parse(t4) != 0 & double.Parse(t6) != 0)
            {
                if ((double.Parse(t2) != double.Parse(t4)) & (double.Parse(t4) != double.Parse(t6)) & (double.Parse(t6) != double.Parse(t2)))
                {


                }
                else
                {
                    Debug.Print("11" + (double.Parse(t2) != double.Parse(t4)));
                    Debug.Print("12" + (double.Parse(t4) != double.Parse(t6)));
                    Debug.Print("13" + (double.Parse(t6) != double.Parse(t2)));
                    MessageBox.Show(" CONC LOAD must be NOT ON SAME POINT!!!");
                    return;
                }
            }




            if (radioButton1.Checked == true)
            {
                bool_Inclided_Member = false;
                Pload2_Direction = "LY";
            }
            if (radioButton2.Checked == true)
            {
                bool_Inclided_Member = true;
                Pload2_Direction = "GY";


            }


            int fac = 1;
            if (Math.Cos(Form1.Theta(Form1.Last_index_line_selected + 1) * Math.PI / 180) >= 0 & (Pload2_Direction != "GY")) fac = 1;
            if (Math.Cos(Form1.Theta(Form1.Last_index_line_selected + 1) * Math.PI / 180) < 0 & (Pload2_Direction != "GY")) fac = -1;



            string[] test = new string[] { };
            if (!string.IsNullOrEmpty(Form1.Pload2[Form1.Last_index_line_selected + 1])) 
            {

                test = Form1.Pload2[Form1.Last_index_line_selected + 1].Split(new char[] { ',' });
                for (int i = 0; i < test.Length; i++)
                {

                }
                Debug.Print("FconcP Form1.Pload2[Form1.Last_index_line_selected + 1]= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);



                test[0] = (Form1.Last_index_line_selected + 1).ToString();
                test[1] = (fac * double.Parse(t1.Replace(" ", ""))).ToString();
                test[2] = t2;
                test[3] = (fac * double.Parse(t3.Replace(" ", ""))).ToString();
                test[4] = t4;
                test[5] = (fac * double.Parse(t5.Replace(" ", ""))).ToString();
                test[6] = t6;
                test[13] = Pload2_Direction;

                string changed = string.Join(",", test);

                Debug.Print("Form1.Last_index_line_selected + 1" + Form1.Last_index_line_selected + 1);
                int k = Form1.Last_index_line_selected + 1;
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 0, test[0]);
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 1, test[1]);
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 2, test[2]);
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 3, test[3]);
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 4, test[4]);
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 5, test[5]);
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 6, test[6]);
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 13, Pload2_Direction);
                Debug.Print("Pconc Form1.Pload2[k] Form1.Find_REPLACE_k_Index_Ploady_Sring, k= " + k + " , " + Form1.Pload2[k]);

            }






}
catch (FormatException)
{
    MessageBox.Show("Number Format error");
    return ;
}

            Debug.Print("FconcP Form1.Pload2= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);

            Form1.bool_Mem_Load_Pload_from_Read = false;
            Form1.bool_Mem_Load_Pload_from_Input = true;

            Form1.Invoke_draw_Mem_load();

            Form1.Invoke_Unselect();
            Form1.Last_index_line_selected = -1;
            Form1.selln = false;

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

        private void FconcP_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {


            string[] test = new string[] { };
            if (!string.IsNullOrEmpty(Form1.Pload2[Form1.Last_index_line_selected + 1]))
            {

                test = Form1.Pload2[Form1.Last_index_line_selected + 1].Split(new char[] { ',' });
                for (int i = 0; i < test.Length; i++)
                {

                }
                Debug.Print(" Form1.Pload2[Form1.Last_index_line_selected + 1]= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);

                test[0] = (Form1.Last_index_line_selected + 1).ToString();
                test[1] = "0";
                test[2] = "0";
                test[3] = "0";
                test[4] = "0";
                test[5] = "0";
                test[6] = "0";

                string changed = string.Join(",", test);
                Form1.Pload2[Form1.Last_index_line_selected + 1] = changed;



                Debug.Print(" Form1.Pload2[Form1.Last_index_line_selected + 1]= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);



            }
            Form1.Invoke_Unselect();
            Form1.Last_index_line_selected = -1;
            Form1.selln = false;

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
            
            if ((sender as TextBox).SelectedText.Length > 0)
            {
                (sender as TextBox).Text = (sender as TextBox).Text.Replace((sender as TextBox).Text.Substring((sender as TextBox).SelectionStart, (sender as TextBox).SelectionLength), "");
            }
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar))
                    && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;

            // only allow minus sign at the beginning
            if (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0)
                e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
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

        private void button3_Click(object sender, EventArgs e)
        {

            string[] test = new string[] { };
            if (!string.IsNullOrEmpty(Form1.Pload2[Form1.Last_index_line_selected + 1]))
            {

                test = Form1.Pload2[Form1.Last_index_line_selected + 1].Split(new char[] { ',' });
                for (int i = 0; i < test.Length; i++)
                {

                }
                Debug.Print(" Form1.Pload2[Form1.Last_index_line_selected + 1]= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);

                test[0] = (Form1.Last_index_line_selected + 1).ToString();
                test[1] = "0";
                test[2] = "0";
                test[3] = "0";
                test[4] = "0";
                test[5] = "0";
                test[6] = "0";

                string changed = string.Join(",", test);
                Form1.Pload2[Form1.Last_index_line_selected + 1] = changed;



                Debug.Print(" Form1.Pload2[Form1.Last_index_line_selected + 1]= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);



            }
            Form1.Invoke_Unselect();
            Form1.Last_index_line_selected = -1;
            Form1.selln = false;

            this.Hide();
            //+++++++++++++++++++++++++++++++
            Form1 frm = new Form1();
            frm.TopMost = true;
            frm.BringToFront();
            frm.WindowState = FormWindowState.Normal;
            //+++++++++++++++++++++++++++++++

            this.Close();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {

            
            if ((sender as TextBox).SelectedText.Length > 0)
            {
                (sender as TextBox).Text = (sender as TextBox).Text.Replace((sender as TextBox).Text.Substring((sender as TextBox).SelectionStart, (sender as TextBox).SelectionLength), "");
            }
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar))
                    && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;

            // only allow minus sign at the beginning
            if (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0)
                e.Handled = true;
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {

           
            if ((sender as TextBox).SelectedText.Length > 0)
            {
                (sender as TextBox).Text = (sender as TextBox).Text.Replace((sender as TextBox).Text.Substring((sender as TextBox).SelectionStart, (sender as TextBox).SelectionLength), "");
            }
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar))
                    && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;

            // only allow minus sign at the beginning
            if (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0)
                e.Handled = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {

            
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

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            
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





    }
}
