using System.Drawing.Drawing2D;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;


namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class FconcM : Form
    {
        public FconcM()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true); // this is to avoid visual artifacts

        }



        private PointF PS, PE;

        //private bool bool_Inclided_Member = false;
        //private string Pload2_Direction = "LY";


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
                //PS = new PointF(300, 160);
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
            // Determines whether the String includes a given character

            if ( (textBox1.Text.Replace(" ", "") == "-"))
            {
                MessageBox.Show("Invalid number");
                return;
            }

            
            if (textBox1.Text.Replace(" ", "") == "") textBox1.Text = "0";
            if (textBox2.Text.Replace(" ", "") == "") textBox2.Text = "0";


            if (double.Parse(textBox1.Text.Replace(" ", "")) == 0)
            {
                MessageBox.Show(" CONC MOMENT Z M must not be ZERO !!!");
                return;
            }

            if (double.Parse(textBox2.Text.Replace(" ", "")) <= 0)
            {
                MessageBox.Show(" Distance d1 of M from startNode must not be ZERO or NEGATIVE !!!");
                return;
            }


            if (double.Parse(textBox2.Text.Replace(" ", "")) >= Form1.L_n(Form1.Last_index_line_selected + 1))
            {
                double c = Form1.L_n(Form1.Last_index_line_selected + 1);
                MessageBox.Show(" Distance d1 of M from startNode must be less than BEAM LENGTH " + c + " m!!!");

                return;
            }

            string[] test = new string[] { };
            if (string.IsNullOrEmpty(Form1.Pload2[Form1.Last_index_line_selected + 1]) == false) // 21 June 21 to avoid nul exception
            {

                test = Form1.Pload2[Form1.Last_index_line_selected + 1].Split(new char[] { ',' });
                for (int i = 0; i < test.Length; i++)
                {

                }
                Debug.Print(" Form1.Pload2[Form1.Last_index_line_selected + 1]= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);
                double M, d1;
                M = double.Parse(textBox1.Text.Replace(" ", ""));
                d1 = double.Parse(textBox2.Text.Replace(" ", ""));
                Debug.Print(" MMMMMMMMMMMMMMMMMMMM =" + (-1 * M) + ", " + d1);//it will be opp sign

                test[11] = (M).ToString();
                test[12] = d1.ToString();


                string changed = string.Join(",", test);



                Debug.Print("Form1.Last_index_line_selected + 1" + Form1.Last_index_line_selected + 1);
                int k = Form1.Last_index_line_selected + 1;
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 0, k.ToString());
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 11, test[11]);
                Form1.Pload2[k] = Form1.Find_REPLACE_k_Index_Ploady_Sring(Form1.Pload2[k], 12, test[12]);


                Debug.Print("Mcon Form1.Pload2[k] Form1.Find_REPLACE_k_Index_Ploady_Sring, k= " + k + " , " + Form1.Pload2[k]);


            }



            Debug.Print("Mcon Form1.Pload2= " + Form1.Pload2[Form1.Last_index_line_selected + 1]);





            Form1.bool_Mem_Load_Pload_from_Read = false;
            Form1.bool_Mem_Load_Pload_from_Input = true;



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



        private void FconcM_Load(object sender, EventArgs e)
        {
            string res = null;
            res = Form1.Find_REPLACE_k_Index_Ploady_Sring("2,5,1,30,2,0,0,0,0,0,0,0,0,LY", 3, "23.5");
            Debug.Print(" Mcon Find_REPLACE_k_Index_Ploady_Sring, Form1.Last_index_line_selected + 1= " + (Form1.Last_index_line_selected + 1) + " , " + res);






            string[] test = new string[] { };

            try
            {


                if (!string.IsNullOrEmpty(Form1.Pload2[Form1.Last_index_line_selected + 1])) // 21 June 21 to avoid nul exception
                {

                    test = Form1.Pload2[Form1.Last_index_line_selected + 1].Split(new char[] { ',' });
                    for (int i = 0; i < test.Length; i++)
                    {


                    }

                    textBox1.Text = (double.Parse(test[11].Replace(" ", ""))).ToString();
                    textBox2.Text = test[12];
                }

            }
            catch (Exception)
            {

            }








        }

        private void button2_Click(object sender, EventArgs e)
        {


            string[] test = new string[] { };
            if (!string.IsNullOrEmpty(Form1.Pload2[Form1.Last_index_line_selected + 1])) // 21 June 21 to avoid nul exception
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
            //+++++++++++++++++++++++++++++++
            Form1 frm = new Form1();
            frm.TopMost = true;
            frm.BringToFront();
            frm.WindowState = FormWindowState.Normal;
            //+++++++++++++++++++++++++++++++

            this.Close();
        }

        private void FconcM_FormClosing(object sender, FormClosingEventArgs e)
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            ////+/-
            ////Delete Selected Text from Textbox and Enter New Char in C#.NET
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

        private void button3_Click(object sender, EventArgs e)
        {

            string[] test = new string[] { };
            if (!string.IsNullOrEmpty(Form1.Pload2[Form1.Last_index_line_selected + 1])) // 21 June 21 to avoid nul exception
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
            //+++++++++++++++++++++++++++++++
            Form1 frm = new Form1();
            frm.TopMost = true;
            frm.BringToFront();
            frm.WindowState = FormWindowState.Normal;
            //+++++++++++++++++++++++++++++++

            this.Close();
        }



    }
}

