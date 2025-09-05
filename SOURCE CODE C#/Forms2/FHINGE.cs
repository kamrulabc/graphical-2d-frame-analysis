using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class FHINGE : Form
    {
        public FHINGE()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void HINGE_Load(object sender, EventArgs e)
        {

        }

        private void HINGE_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {


            int NODENUM = 0;
            int NODENUM2 = 0;
            int MEMNUM = 0;

            int mem_index = 0;
            MEMNUM = Form1.Last_index_line_selected + 1;
            if (radioButton1.Checked == false & radioButton2.Checked == false)
            {
                MessageBox.Show("Please select START OR END OF MEMBER !!! ");
                return;
            }
            if (radioButton1.Checked == true)
            {
                mem_index = 1;
            }
            if (radioButton2.Checked == true)
            {

                mem_index = 2;
            }


            NODENUM2 = Form1.Connectivity[MEMNUM, mem_index];
            if (mem_index == 1) NODENUM = Find_Node_Num_From_Coordinate(Form1.mLines[Form1.Last_index_line_selected].StartPoint);
            if (mem_index == 2) NODENUM = Find_Node_Num_From_Coordinate(Form1.mLines[Form1.Last_index_line_selected].EndPoint);
            Debug.Print("add HHHHHHHHHHHH NODENUM2 , NODENUM HINGE Form.Form1.HINGE_String2= " + NODENUM2 + "," + NODENUM);






            Form1.HINGE_String2[NODENUM] = NODENUM2.ToString() + "," + MEMNUM.ToString() + "," + mem_index.ToString();

            Debug.Print("add HHHHHHHHHHHH QQQQQQ HINGE Form.Form1.HINGE_String2= " + Form1.HINGE_String2[NODENUM]);





            Debug.Print("add HHHHHHHHHHHHHHHHHHHHHHHHHHHH HINGE_String.Length= " + Form1.HINGE_String2.Length);
            for (int k = 1; k < Form1.totalnode_From_Lines; k++)
            {
                Debug.Print(k + "  add nodenum,HINGE_MemID[k], HINGE_String.Length, HINGE_String[k]= " + MEMNUM + " , " + Form1.HINGE_String2.Length + " , " + Form1.HINGE_String2[k]);
            }











            Form1.bool_Hinge_from_Read = false;
            Form1.bool_Hinge_from_Input = true;



            Form1.bool_Mem_Load_Pload_from_Read = false;
            Form1.bool_Mem_Load_Pload_from_Input = true;

            Form1.Invoke_draw_Mem_load();

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



        private void button2_Click(object sender, EventArgs e)
        {
            int NODENUM = 0;
            int NODENUM2 = 0;
            int MEMNUM = 0;

            int mem_index = 0;
            MEMNUM = Form1.Last_index_line_selected + 1;
            if (radioButton1.Checked == false & radioButton2.Checked == false)
            {
                MessageBox.Show("Please select START OR END OF MEMBER !!! ");
                return;
            }
            if (radioButton1.Checked == true)
            {
                mem_index = 1;
            }
            if (radioButton2.Checked == true)
            {

                mem_index = 2;
            }


            NODENUM2 = Form1.Connectivity[MEMNUM, mem_index];
            if (mem_index == 1) NODENUM = Find_Node_Num_From_Coordinate(Form1.mLines[Form1.Last_index_line_selected].StartPoint);
            if (mem_index == 2) NODENUM = Find_Node_Num_From_Coordinate(Form1.mLines[Form1.Last_index_line_selected].EndPoint);
            Debug.Print("delete HHHHHHHHHHHH  NODENUM2 , NODENUM HINGE Form.Form1.HINGE_String2= " + NODENUM2 + "," + NODENUM);








            Form1.HINGE_String2[NODENUM] = NODENUM.ToString() + "," + "0" + "," + "0";

            Debug.Print("delete  HHHHHHHHHHHH QQQQQQ HINGE Form.Form1.HINGE_String= " + Form1.HINGE_String2[NODENUM]);




            Debug.Print("delete HHHHHHHHHHHHHHHHHHHHHHHHHHHH HINGE_String.Length= " + Form1.HINGE_String2.Length);
            for (int k = 1; k < Form1.totalnode_From_Lines; k++)
            {
                Debug.Print(k + "  delete nodenum,HINGE_MemID[k], HINGE_String.Length, HINGE_String[k]= " + MEMNUM + " , " + Form1.HINGE_String2.Length + " , " + Form1.HINGE_String2[k]);
            }











            Form1.bool_Hinge_from_Read = false;
            Form1.bool_Hinge_from_Input = true;



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






    }
}

