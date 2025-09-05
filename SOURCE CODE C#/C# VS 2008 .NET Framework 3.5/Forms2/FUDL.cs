using System.Drawing.Drawing2D;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;


namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class FUDL : Form
    {
        public FUDL()
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


        private void FUDL_Load(object sender, EventArgs e)
        {
            Debug.Print(" Form1.L_n(Form1.Last_index_line_selected + 1)=" + Form1.L_n(Form1.Last_index_line_selected + 1));

            double udl1, d1, udl2, d2;
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
                        Debug.Print(" FconcP_Load=222222222222222222222222");
                        Debug.Print(" Form1.Pload2[Form1.Last_index_line_selected + 1]222= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);

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





            string res = null;
            res = Form1.Find_REPLACE_k_Index_Ploady_Sring("2,5,1,30,2,0,0,0,0,0,0,0,0,LY", 3, "23.5");
            Debug.Print(" UDL Find_REPLACE_k_Index_Ploady_Sring, Form1.Last_index_line_selected + 1= " + (Form1.Last_index_line_selected + 1) + " , " + res);











            Debug.Print(" private void FUDL_Load QQQQQQQQQQQQQQQQQQQQQQQQQQQQ  = "
                + Form1.Last_index_line_selected
                 + " , " + Form1.Pload2[Form1.Last_index_line_selected + 1]
                 + " , ");





            string[] test = new string[] { };

            try
            {


                if (!string.IsNullOrEmpty(Form1.Pload2[Form1.Last_index_line_selected + 1]))
                {

                    test = Form1.Pload2[Form1.Last_index_line_selected + 1].Split(new char[] { ',' });
                    for (int i = 0; i < test.Length; i++)
                    {


                    }
                    udl1 = double.Parse(test[7].Replace(" ", ""));
                    udl2 = double.Parse(test[8].Replace(" ", ""));
                    d1 = double.Parse(test[9].Replace(" ", ""));
                    d2 = double.Parse(test[10].Replace(" ", ""));


                    textBox1.Text = (udl1).ToString();
                    textBox3.Text = (udl2).ToString();
                    textBox2.Text = (d1).ToString();
                    textBox4.Text = (d2).ToString();

                }

            }
            catch (Exception)
            {

            }




            if (Form1.Find_All_Zero_Ploady_Sring(Form1.Pload2[Form1.Last_index_line_selected + 1]) == true)
            {
                textBox1.Text = "70";
                textBox3.Text = "50";
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


                    k = i;
                }
            }


            return k;
        }

        private void button1_Click(object sender, EventArgs e)
        {



            if (radioButton1.Checked == false & radioButton2.Checked == false)
            {
                MessageBox.Show("One RadioButton i.e. one Direction of LOAD must be selected !!!");
                return;
            }
            
                // Determines whether the String includes a given character

            if ( (textBox1.Text.Replace(" ", "") == "-")|(textBox3.Text.Replace(" ", "") == "-"))
            {
                MessageBox.Show("Invalid number");
                return;
            }



            double udl1, d1, udl2, d2;

            if (textBox1.Text.Replace(" ", "") == "") textBox1.Text = "0";
            if (textBox2.Text.Replace(" ", "") == "") textBox2.Text = "0";
            
            if (textBox3.Text.Replace(" ", "") == "") textBox3.Text = "0";
            if (textBox4.Text.Replace(" ", "") == "") textBox4.Text = "0";

            udl1 = double.Parse(textBox1.Text.Replace(" ", ""));
            udl2 = double.Parse(textBox3.Text.Replace(" ", ""));

            if (udl1 != 0 & udl2 != 0 & (Math.Abs(udl1) / udl1) * (Math.Abs(udl2) / udl2) == -1)
            {
                MessageBox.Show(" UDL/Trap/Triangula LOAD W1 and W2 both must have same SIGN !!!");
                return;
            }

            if (double.Parse(textBox1.Text.Replace(" ", "")) == 0 & double.Parse(textBox3.Text.Replace(" ", "")) == 0)
            {
                MessageBox.Show(" UDL/Trap/Triangula LOAD W1 and W2 both must not be ZERO !!!");
                return;
            }


            


            if (double.Parse(textBox2.Text.Replace(" ", "")) >= Form1.L_n(Form1.Last_index_line_selected + 1))
            {
                double c = Form1.L_n(Form1.Last_index_line_selected + 1);
                MessageBox.Show(" Distance d1 of W1 from startNode must be less than BEAM LENGTH " + c + " m!!!");

                return;
            }









            if (double.Parse(textBox4.Text.Replace(" ", "")) > Form1.L_n(Form1.Last_index_line_selected + 1))
            {
                double c = Form1.L_n(Form1.Last_index_line_selected + 1);
                MessageBox.Show(" Distance d2 of W2 from startNode must not exceed  BEAM LENGTH " + c + " m!!!");

                return;
            }

            if (double.Parse(textBox2.Text.Replace(" ", "")) >= double.Parse(textBox4.Text.Replace(" ", "")))
            {
                MessageBox.Show(" Distance d2 of W2 must be greater than Distance d1 of W1 from startNode!!!");
                return;
            }



            if (radioButton1.Checked == true)
            {
                Pload2_Direction = "LY";
            }
            if (radioButton2.Checked == true)
            {
                if (bool_Inclided_Member == true)
                {
                    Pload2_Direction = "GY";
                    radioButton2.Checked = true;
                    radioButton1.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Applicable for inclined beam !!!");
                    Pload2_Direction = "LY";
                    radioButton1.Checked = true;
                    return;
                }
            }

            string[] test = new string[] { };
            if (string.IsNullOrEmpty(Form1.Pload2[Form1.Last_index_line_selected + 1]) == false)
            {

                test = Form1.Pload2[Form1.Last_index_line_selected + 1].Split(new char[] { ',' });
                for (int i = 0; i < test.Length; i++)
                {

                }
                Debug.Print(" Form1.Pload2[Form1.Last_index_line_selected + 1]= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);

                udl1 = double.Parse(textBox1.Text.Replace(" ", ""));
                udl2 = double.Parse(textBox3.Text.Replace(" ", ""));
                d1 = double.Parse(textBox2.Text.Replace(" ", ""));
                d2 = double.Parse(textBox4.Text.Replace(" ", ""));








                int fac = 1;
                if (Math.Cos(Form1.Theta(Form1.Last_index_line_selected + 1) * Math.PI / 180) >= 0 & (Pload2_Direction != "GY")) fac = 1;
                if (Math.Cos(Form1.Theta(Form1.Last_index_line_selected + 1) * Math.PI / 180) < 0 & (Pload2_Direction != "GY")) fac = -1;






                test[7] = (fac * udl1).ToString();
                test[9] = d1.ToString();
                test[8] = (fac * udl2).ToString();
                test[10] = d2.ToString();
                test[13] = Pload2_Direction;

                string changed = string.Join(",", test);






                Debug.Print("Form1.Last_index_line_selected + 1" + Form1.Last_index_line_selected + 1);
                int k = Form1.Last_index_line_selected + 1;
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 0, k.ToString());
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 7, test[7]);
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 8, test[8]);
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 9, test[9]);
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 10, test[10]);
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 13, Pload2_Direction);

                Debug.Print("UDL Form1.Pload2[k] Form1.Find_REPLACE_k_Index_Ploady_Sring, k= " + k + " , " + Form1.Pload2[k]);































            }



            Debug.Print(" Form1.Pload2= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);










            Form1.bool_Mem_Load_Pload_from_Read = false;
            Form1.bool_Mem_Load_Pload_from_Input = true;



            Form1.Invoke_Unselect();
            Form1.Last_index_line_selected = -1;
            Form1.selln = false;





            this.Hide();

            Form1 frm = new Form1();
            frm.TopMost = true;
            frm.BringToFront();
            frm.WindowState = FormWindowState.Normal;


            this.Close();
        }

        private void FUDL_FormClosing(object sender, FormClosingEventArgs e)
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
                test[11] = "0";
                test[12] = "0";



                string changed = string.Join(",", test);
                Form1.Pload2[Form1.Last_index_line_selected + 1] = changed;
            }


            Debug.Print(" Form1.Pload2[Form1.Last_index_line_selected + 1]= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);


            Form1.Invoke_Unselect();
            Form1.Last_index_line_selected = -1;
            Form1.selln = false;

            this.Hide();

            Form1 frm = new Form1();
            frm.TopMost = true;
            frm.BringToFront();
            frm.WindowState = FormWindowState.Normal;


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


            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;


            if (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0)
                e.Handled = true;
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


            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;


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


            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
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
                test[7] = "0";
                test[8] = "0";
                test[9] = "0";
                test[10] = "0";


                string changed = string.Join(",", test);
                Form1.Pload2[Form1.Last_index_line_selected + 1] = changed;



                Debug.Print(" Form1.Pload2[Form1.Last_index_line_selected + 1]= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);



            }



            this.Hide();

            Form1 frm = new Form1();
            frm.TopMost = true;
            frm.BringToFront();
            frm.WindowState = FormWindowState.Normal;


            this.Close();
        }









    }
}

