using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class FSuppData : Form
    {
        public FSuppData()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }


        private void button1_Click(object sender, EventArgs e)
        {

            // Determines whether the String includes a given character

            if ( (textBox1.Text.Replace(" ", "") == "-")|(textBox2.Text.Replace(" ", "") == "-"))
            {
                MessageBox.Show("Invalid number");
                return;
            }


            if (textBox1.Text.Replace(" ", "") == "") textBox1.Text = "0";
            Form1.SPRING_Constant = double.Parse(textBox1.Text.Replace(" ", ""));

            if (Form1.SPRING_Constant == 0 & radioButton6.Checked == true)
            {
                MessageBox.Show(" Spring constant must not be 0 !!!");
                return;
            }

            
            if (textBox2.Text.Replace(" ", "") == "") textBox2.Text = "0";
            Form1.Inc_Supp_angle_base_AND_GX = double.Parse(textBox2.Text);

            if (Form1.Inc_Supp_angle_base_AND_GX == 0 & radioButton7.Checked == true)
            {
                MessageBox.Show(" Angle must not be 0 !!!");
                return;
            }

            if (radioButton1.Checked == true)
            {
                Form1.bool_Fixed_support = true;
                Form1.bool_Pinned_support = false;
                Form1.bool_Roller_Supp_Ydir = false;
                Form1.bool_Guided_Supp_Ydir = false;
                Form1.bool_Delete_support = false;

                Form1.bool_SPRING_Supp_Ydir = false;
                Form1.bool_Inclied_Supp = false;
            }
            if (radioButton2.Checked == true)
            {
                Form1.bool_Fixed_support = false;
                Form1.bool_Pinned_support = true;
                Form1.bool_Roller_Supp_Ydir = false;
                Form1.bool_Guided_Supp_Ydir = false;
                Form1.bool_Delete_support = false;

                Form1.bool_SPRING_Supp_Ydir = false;
                Form1.bool_Inclied_Supp = false;
            }
            if (radioButton3.Checked == true)
            {
                Form1.bool_Fixed_support = false;
                Form1.bool_Pinned_support = false;
                Form1.bool_Roller_Supp_Ydir = true;
                Form1.bool_Guided_Supp_Ydir = false;
                Form1.bool_Delete_support = false;

                Form1.bool_SPRING_Supp_Ydir = false;
                Form1.bool_Inclied_Supp = false;
            }


            if (radioButton5.Checked == true)
            {
                Form1.bool_Fixed_support = false;
                Form1.bool_Pinned_support = false;
                Form1.bool_Roller_Supp_Ydir = false;
                Form1.bool_Guided_Supp_Ydir = true;
                Form1.bool_Delete_support = false;

                Form1.bool_SPRING_Supp_Ydir = false;
                Form1.bool_Inclied_Supp = false;

            }
            if (radioButton6.Checked == true)
            {
                if (Form1.Find_bool_Support_Displacement() == true)
                {
                    MessageBox.Show("Support Displacement with Spring support not allowed!!!");
                    return;
                }
                Form1.bool_SPRING_Supp_Ydir = true;

                Form1.bool_Fixed_support = false;
                Form1.bool_Pinned_support = false;
                Form1.bool_Roller_Supp_Ydir = false;
                Form1.bool_Guided_Supp_Ydir = false;
                Form1.bool_Delete_support = false;

                Form1.bool_SPRING_Supp_Ydir = true;
                Form1.bool_Inclied_Supp = false;


            }
            if (radioButton7.Checked == true)
            {
                if (Form1.Find_bool_Support_Displacement() == true)
                {
                    MessageBox.Show("Support Displacement with Inclined support not allowed!!!");
                    return;
                }


                Form1.bool_Fixed_support = false;
                Form1.bool_Pinned_support = false;
                Form1.bool_Roller_Supp_Ydir = false;
                Form1.bool_Guided_Supp_Ydir = false;
                Form1.bool_Delete_support = false;

                Form1.bool_SPRING_Supp_Ydir = false;
                Form1.bool_Inclied_Supp = true;


            }
            if (radioButton10.Checked == true)
            {
                Form1.bool_Fixed_support = false;
                Form1.bool_Pinned_support = false;
                Form1.bool_Roller_Supp_Ydir = false;
                Form1.bool_Guided_Supp_Ydir = false;
                Form1.bool_Delete_support = false;

                Form1.bool_SPRING_Supp_Ydir = false;
                Form1.bool_Inclied_Supp = false;
                Form1.bool_Delete_support = true;
            }
            if (Form1.bool_Fixed_support == true & Form1.mLines.Count > 0 & Form1.nodenumer_Selected > -1)
            {

                Form1.bool_SUPPORT_from_Read = false;
                Form1.bool_SUPPORT_from_Input = true;
                Form1.Sup_Node_Number_Type2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + "1";
                Debug.Print(" bool_Fixed_support" + Form1.nodenumer_Selected.ToString() + "," + "1");


            }

            if (Form1.bool_Pinned_support == true & Form1.mLines.Count > 0 & Form1.nodenumer_Selected > -1)
            {

                Form1.bool_SUPPORT_from_Read = false;
                Form1.bool_SUPPORT_from_Input = true;
                Form1.Sup_Node_Number_Type2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + "2";
                Debug.Print(" bool_Pinned_support" + Form1.nodenumer_Selected.ToString() + "," + "2");

            }
            if (Form1.bool_Roller_Supp_Ydir == true & Form1.mLines.Count > 0 & Form1.nodenumer_Selected > -1)
            {

                Form1.bool_SUPPORT_from_Read = false;
                Form1.bool_SUPPORT_from_Input = true;
                Form1.Sup_Node_Number_Type2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + "3";
                Debug.Print(" Form1.bool_Roller_Supp_Ydir" + Form1.nodenumer_Selected.ToString() + "," + "3");

            }


            if (Form1.bool_Guided_Supp_Ydir == true & Form1.mLines.Count > 0 & Form1.nodenumer_Selected > -1)
            {

                Form1.bool_SUPPORT_from_Read = false;
                Form1.bool_SUPPORT_from_Input = true;



                Form1.Sup_Node_Number_Type2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + "5";
                Debug.Print(" Form1.bool_Guided_Supp_Ydir" + Form1.nodenumer_Selected.ToString() + "," + "5");

            }
            if (Form1.bool_SPRING_Supp_Ydir == true & Form1.mLines.Count > 0 & Form1.nodenumer_Selected > -1)
            {

                Form1.bool_SUPPORT_from_Read = false;
                Form1.bool_SUPPORT_from_Input = true;



                Form1.Sup_Node_Number_Type2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + "6" + "," + Form1.SPRING_Constant.ToString();
                Debug.Print(" Form1.bool_SPRING_Supp_Ydir" + Form1.nodenumer_Selected.ToString() + "," + "6" + "," + Form1.SPRING_Constant.ToString());

            }

            if (Form1.bool_Inclied_Supp == true & Form1.mLines.Count > 0 & Form1.nodenumer_Selected > -1)
            {

                Debug.Print("textBox2.Text=" + textBox2.Text + "," + Form1.Inc_Supp_angle_base_AND_GX);
                Form1.bool_SUPPORT_from_Read = false;
                Form1.bool_SUPPORT_from_Input = true;

                Form1.bool_SPRING_Supp_Ydir = false;
                Form1.bool_Inclied_Supp = true;














































                if (Form1.Inclied_Supp_Node_num < 1)
                {
                    Form1.Sup_Node_Number_Type2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + "7" + "," + Form1.Inc_Supp_angle_base_AND_GX.ToString();
                    Debug.Print(Form1.Inc_Supp_angle_base_AND_GX + " DDDDDDDDDDDDDDDDDD INC RY support" + Form1.nodenumer_Selected.ToString() + "," + "7" + "," + Form1.Inc_Supp_angle_base_AND_GX.ToString());

                }
                else
                {
                    MessageBox.Show("One inclined Roller Support is allowable !!!");

                    return;
                }
            }


            if (Form1.bool_Delete_support == true & Form1.mLines.Count > 0 & Form1.nodenumer_Selected > -1)
            {

                Form1.bool_SUPPORT_from_Read = false;
                Form1.bool_SUPPORT_from_Input = true;
                Form1.Sup_Node_Number_Type2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + "0";
                Debug.Print("Form1.bool_Delete_support DDDDDDDDDDDDDDDDDD Form1.Sup_Node_Number_Type2[Form1.nodenumer_Selected]= " + Form1.nodenumer_Selected.ToString() + "," + "0");

            }




            Form1.Invoke_Unselect();
            Form1.nodenumer_Selected = -1;
            Form1.selpt = false;





            this.Hide();

            Form1 frm = new Form1();
            frm.TopMost = true;
            frm.BringToFront();
            frm.WindowState = FormWindowState.Normal;


            this.Close();

        }











        private void FSuppData_Load(object sender, EventArgs e)
        {










            Debug.Print(" private void FSuppData_Load_Load Form1.nodenumer_Selected = "
               + Form1.nodenumer_Selected + " , ");
            label5.Text = " Selected Node number= " + Form1.nodenumer_Selected;


        }

        private void FSuppData_FormClosing(object sender, FormClosingEventArgs e)
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

        private void radioButton2_MouseDown(object sender, MouseEventArgs e)
        {




        }

        private void radioButton1_MouseDown(object sender, MouseEventArgs e)
        {


        }

        private void radioButton3_MouseDown(object sender, MouseEventArgs e)
        {


        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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



    }
}
