using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class FNodal : Form
    {
        public FNodal()
        {
            InitializeComponent();
        }



        private void FNodal_Load(object sender, EventArgs e)
        {












            Debug.Print(" private void FSuppData_Load_Load Form1.nodenumer_Selected = "
                          + Form1.nodenumer_Selected + " , ");
            label5.Text = " Selected Node number= " + Form1.nodenumer_Selected;
            string[] test = new string[] { };
            int k = 0;

            if (!string.IsNullOrEmpty(Form1.PJoint[Form1.nodenumer_Selected]))
            {
                test = Form1.PJoint[Form1.nodenumer_Selected].Split(new char[] { ',' });
                for (k = 0; k < test.Length; k++)
                {

                }
                k = int.Parse(test[0]);
                textBox1.Text = (test[1]);
                textBox2.Text = (test[2]);
                textBox3.Text = (test[3]);
            }

            else
            {
                textBox1.Text = "-80";
                textBox2.Text = "-70";
                textBox3.Text = "-90";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox1.Text.Replace(" ", "") == "") textBox1.Text = "0";
            if (textBox2.Text.Replace(" ", "") == "") textBox2.Text = "0";
            if (textBox3.Text.Replace(" ", "") == "") textBox3.Text = "0";
            Form1.PJoint2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + textBox1.Text
                + "," + textBox2.Text + "," + textBox3.Text;


            Form1.Invoke_Unselect();

            Form1.nodenumer_Selected = -1;




            this.Hide();

            Form1 frm = new Form1();
            frm.TopMost = true;
            frm.BringToFront();
            frm.WindowState = FormWindowState.Normal;


            this.Close();

        }

        private void FNodal_FormClosing(object sender, FormClosingEventArgs e)
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
        private void FNodal_KeyPress(object sender, KeyPressEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            // Determines whether the String includes a given character

            if ((textBox1.Text.Replace(" ", "") == "-") | (textBox2.Text.Replace(" ", "") == "-") | (textBox3.Text.Replace(" ", "") == "-"))
            {
                MessageBox.Show("Invalid number");
                return;
            }

            if (textBox1.Text.Replace(" ", "") == "") textBox1.Text = "0";
            if (textBox2.Text.Replace(" ", "") == "") textBox2.Text = "0";
            if (textBox3.Text.Replace(" ", "") == "") textBox3.Text = "0";
            Form1.PJoint2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + textBox1.Text
                + "," + textBox2.Text + "," + textBox3.Text;


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
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
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





    }
}
