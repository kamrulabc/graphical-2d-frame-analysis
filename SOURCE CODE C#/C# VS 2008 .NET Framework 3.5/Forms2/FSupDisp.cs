using System;
using System.Diagnostics;
using System.Windows.Forms;


namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class FSupDisp : Form
    {
        public FSupDisp()
        {
            InitializeComponent();
        }

        private void FSupDisp_Load(object sender, EventArgs e)
        {
            //textBox1.Text = "0.1";
            //textBox2.Text = "0.15";


            Array.Resize(ref   Form1.Support_Displacement_S, Form1.totalnode + 1);

            Debug.Print(" private void FSuppData_Load_Load Form1.nodenumer_Selected = "
                          + Form1.nodenumer_Selected);
            label5.Text = " Selected Node number= " + Form1.nodenumer_Selected;



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

        private void button1_Click(object sender, EventArgs e)
        {
            // Determines whether the String includes a given character

            if ( (textBox1.Text.Replace(" ", "") == "-")|(textBox2.Text.Replace(" ", "") == "-"))
            {
                MessageBox.Show("Invalid number");
                return;
            }

            
            if (textBox1.Text.Replace(" ", "") == "") textBox1.Text = "0";
            if (textBox2.Text.Replace(" ", "") == "") textBox2.Text = "0";
            if (Math.Abs(double.Parse(textBox1.Text.Replace(" ", ""))) > 0.3)
            {
                MessageBox.Show("Support_Displacement must not exceed 0.3M !!!");
                return;
            }
            if (Math.Abs(double.Parse(textBox2.Text.Replace(" ", ""))) > 0.3)
            {
                MessageBox.Show("Support_Displacement must not exceed 0.3M !!!");
                return;
            }
            if (Form1.Inc_Supp_angle_base_AND_GX != 0 | Form1.SPRING_Constant > 0)
            {
                Debug.Print("Form1.Inc_Supp_angle_base_AND_GX != 0 | Form1.SPRING_Constant > 0 " + Form1.Inc_Supp_angle_base_AND_GX + "xxxx" + Form1.SPRING_Constant);
                MessageBox.Show("Support Displacement with Spring or Inclined Support not allowed!!!");
                return;
            }

            Form1.Support_Displacement_S[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + (double.Parse(textBox1.Text.Replace(" ", ""))).ToString() + "," + (double.Parse(textBox2.Text.Replace(" ", ""))).ToString();
            Debug.Print(" Form1.Support_Displacement_S[Form1.nodenumer_Selected]= " + Form1.Support_Displacement_S[Form1.nodenumer_Selected]);


            Debug.Print("Form1.Inc_Supp_angle_base_AND_GX != 0 | Form1.SPRING_Constant > 0 " + Form1.Inc_Supp_angle_base_AND_GX + " , " + Form1.SPRING_Constant);

            Form1.Invoke_Unselect();

            Form1.nodenumer_Selected = -1;




            this.Hide();

            Form1 frm = new Form1();
            frm.TopMost = true;
            frm.BringToFront();
            frm.WindowState = FormWindowState.Normal;


            this.Close();

        }

        private void FSupDisp_FormClosing(object sender, FormClosingEventArgs e)
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





































    }
}
