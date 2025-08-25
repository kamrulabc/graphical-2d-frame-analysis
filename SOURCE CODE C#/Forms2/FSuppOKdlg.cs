using System;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class FDirectSupp : Form
    {
        public FDirectSupp()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Form1.bool_Fixed_support == true & Form1.mLines.Count > 0 & Form1.nodenumer_Selected > -1)
            {
                Form1.bool_Pinned_support = false;
                Form1.bool_Delete_support = false;
                Form1.bool_SUPPORT_from_Read = false;
                Form1.bool_SUPPORT_from_Input = true;
                Form1.Sup_Node_Number_Type2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + "1";



            }

            if (Form1.bool_Pinned_support == true & Form1.mLines.Count > 0 & Form1.nodenumer_Selected > -1)
            {
                Form1.bool_Fixed_support = false;
                Form1.bool_Delete_support = false;
                Form1.bool_SUPPORT_from_Read = false;
                Form1.bool_SUPPORT_from_Input = true;
                Form1.Sup_Node_Number_Type2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + "2";


            }
            if (Form1.bool_Delete_support == true & Form1.mLines.Count > 0 & Form1.nodenumer_Selected > -1)
            {
                Form1.bool_Fixed_support = false;
                Form1.bool_Pinned_support = false;
                Form1.bool_SUPPORT_from_Read = false;
                Form1.bool_SUPPORT_from_Input = true;
                Form1.Sup_Node_Number_Type2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + "0";


            }

            if (Form1.bool_Roller_Supp_Ydir == true & Form1.mLines.Count > 0 & Form1.nodenumer_Selected > -1)
            {

                Form1.bool_SUPPORT_from_Read = false;
                Form1.bool_SUPPORT_from_Input = true;
                Form1.Sup_Node_Number_Type2[Form1.nodenumer_Selected] = Form1.nodenumer_Selected.ToString() + "," + "3";


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

        private void FDirectSupp_FormClosing(object sender, FormClosingEventArgs e)
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
