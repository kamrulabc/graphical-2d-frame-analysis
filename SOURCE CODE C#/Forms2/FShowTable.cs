using System.Drawing.Drawing2D;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class FShowTable : Form
    {
        public FShowTable()
        {
            InitializeComponent();
        }

        private double VxY_Y, VxY_Indx, MxY_Y, MxY_Indx, YxY_Y, YxY_Indx, AFY_Y, AFY_Indx, YxY_YR, YxYR_Indx;

        private static int comboBox1_Text = 10;
       
        private PointF Location_Move;
       
        private bool bool_max = false;
        private bool bool_moving = false;
        private  int mid = Form1.Last_index_line_selected + 1;
        private  int start = 292 - 80;
        private int dx;
        private double y1, y2, y4, y5, yR, lx = 0;
        private string s1, s2, s4, s5, sR, sx = "";



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                comboBox1.Visible = false;
                label4.Visible = false;
            }
            if (tabControl1.SelectedTab == tabPage2)
            {
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                comboBox1.Visible = true;
                
                label4.Visible = true;


                dataGridView1.ColumnCount = 7;

                dataGridView1.Columns[0].Name = " # ";
                dataGridView1.Columns[1].Name = "Distance mm";
                dataGridView1.Columns[2].Name = "Axial force kN";
                dataGridView1.Columns[3].Name = "Shear kN";
                dataGridView1.Columns[4].Name = "Moment kN-m";
                dataGridView1.Columns[5].Name = "Displacement from original position in local Y Direction in mm";
                dataGridView1.Columns[6].Name = "Relative Displacement from two ends of beam mm";



                int i = mid;

                comboBox1.Text="10";



               


            }

            if (tabControl1.SelectedTab == tabPage2)
            {
                dataGridView1.Visible = true;
                PictureBox1.Visible = false;
                Panel1.Visible = false;

            }

            if (tabControl1.SelectedTab == tabPage1)
            {
                Panel1.Visible = true;
                PictureBox1.Visible = true;
                dataGridView1.Visible = false;

            }
        }

        private int FindMAX_Index_Double_1D(double[] array1)
        {

            double[] result = new double[2];



            int[] index = new int[array1.Length];


            for (int k = array1.Length - 1; k >= 0; k--)
            {
                index[k] = k;


            }


            int tmp = 0;
            for (int i = 1; i <= array1.Length; i++)
            {
                for (int j = 0; j < index.Length - 1; j++)
                {
                    if ((array1[index[j]]) > (array1[index[j + 1]]))
                    {
                        tmp = index[j + 1];
                        index[j + 1] = index[j];
                        index[j] = tmp;

                    }

                }
            }



            result[0] = array1[index[array1.Length - 1]];
            result[1] = index[array1.Length - 1];
            return index[array1.Length - 1];

        }


        private void FShowTable_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("12");
            comboBox1.Items.Add("10");

            Panel1.BorderStyle = BorderStyle.FixedSingle;

            Panel1.AutoScroll = true;


            PictureBox1.Parent = Panel1;



            Size size = new Size(Convert.ToInt32(1200), Convert.ToInt32(800));


            PictureBox1.Size = size;
            PictureBox1.Invalidate();

            dataGridView1.Height = 400;
            dataGridView1.Width = 700;


            Cursor.Position = new Point(PictureBox1.Left + PictureBox1.Width / 2,
                                PictureBox1.Top + PictureBox1.Height / 2);
            bool_max = true;

            radioButton1.Checked = true;
            tabControl1.TabPages[0].Text = "Show Graph";
            tabControl1.TabPages[1].Text = "Show Table";


            double frac = Form1.L_n(mid) / Form1.segment2 * 1d;

            double[] q = new double[Form1.segment2 + 1];
            double[] q1 = new double[Form1.segment2 + 1];
            int ix;


            double[,] m = new double[Form1.totalmember + 1, 6 + 1];



            for (int j = 0; j <= Form1.segment2; j = j + 1) { q[j] = Math.Abs(Form1.BMD_ALL[mid, 5, j]); q1[j] = (Form1.BMD_ALL[mid, 5, j]); }
            ix = FindMAX_Index_Double_1D(q);
            Debug.WriteLine(ix + " =ABSOLUTE Max AF 5555555==" + q1[ix] + "," + (ix * frac).ToString("0.00"));
            AFY_Y = q1[ix];
            AFY_Indx = ix;


            for (int j = 0; j <= Form1.segment2; j = j + 1) { q[j] = Math.Abs(Form1.BMD_ALL[mid, 1, j]); q1[j] = (Form1.BMD_ALL[mid, 1, j]); }
            ix = FindMAX_Index_Double_1D(q);
            Debug.WriteLine(ix + " =ABSOLUTE Max VV 111111==" + q1[ix] + "," + (ix * frac).ToString("0.00"));
            VxY_Y = q1[ix];
            VxY_Indx = ix;


            for (int j = 0; j <= Form1.segment2; j = j + 1) { q[j] = Math.Abs(Form1.BMD_ALL[mid, 2, j]); q1[j] = (Form1.BMD_ALL[mid, 2, j]); }
            ix = FindMAX_Index_Double_1D(q);
            Debug.WriteLine(ix + " =ABSOLUTE Max MM 222222==" + q1[ix] + "," + (ix * frac).ToString("0.00"));
            MxY_Y = q1[ix];
            MxY_Indx = ix;


            for (int j = 0; j <= Form1.segment2; j = j + 1) { q[j] = Math.Abs(1000 * Form1.BMD_ALL[mid, 4, j]); q1[j] = (1000 * Form1.BMD_ALL[mid, 4, j]); }
            ix = FindMAX_Index_Double_1D(q);
            Debug.WriteLine(ix + " =ABSOLUTE Max YY 4444==" + q[ix] + "," + (ix * frac).ToString("0.00"));
            YxY_Y = q1[ix];
            YxY_Indx = ix;


            for (int j = 0; j <= Form1.segment2; j = j + 1) { q[j] = Math.Abs(Form1.Relative_Disp[mid, j]); q1[j] = Form1.Relative_Disp[mid, j]; }
            ix = FindMAX_Index_Double_1D(q);
            Debug.WriteLine(ix + " =ABSOLUTE Max Re RRRRRRRRRR==" + q1[ix] + "," + (ix * frac).ToString("0.00"));
            YxY_YR = q1[ix];
            YxYR_Indx = ix;




            Form1.selpt = false;
            Form1.selln = false;
            Form1.drawln = false;


            Form1.bool_FconcP = false;
            Form1.bool_FconcM = false;
            Form1.bool_FSupDisp = false;
            Form1.bool_FUDL = false;
            Form1.bool_FNodal = false;
            Form1.bool_FSuppData = false;
            Form1.bool_FDelSupp = false;
            Form1.bool_FBeamProp = false;
            Form1.bool_HINGE_Intermediate = false;
            Form1.bool_FShowTable = false;
            Form1.bool_FDirectSupp = false;






            //double qaz = Form1.Ayz[mid];
            //Iz[i] = double.Parse(test[2]);
            //Ey[i] = double.Parse(test[3]);

            label1.Text ="Member No.= "+mid+ ", START Node is : " + Find_Node_Num_From_Coordinate(Form1.mLines[mid - 1].StartPoint) + "," + "END Node is : " + Find_Node_Num_From_Coordinate(Form1.mLines[mid - 1].EndPoint) + "," + " Material Property : " + " A = "+Form1.Ayz[mid] + ","+ " I = "+Form1.Ayz[mid] + ","+" E = "+Form1.Ey[mid] ;


            tabControl1.SelectedIndex = 3;
            tabControl1.SelectedTab = tabPage1;


            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = this.Size;

        }

        private double[,] find(double[, ,] x, int n, int mult)
        {


            double[] V = new double[Form1.segment2 + 1];
            double[,] Y = new double[Form1.totalmember + 1, 6 + 1];
            if (n != 6)
            {
                for (int j = 0; j <= Form1.segment2; j = j + 1)
                {
                    V[j] = x[mid, n, j] * mult;

                }
            }
            if (n == 6)
            {
                for (int j = 0; j <= Form1.segment2; j = j + 1)
                {
                    V[j] = Form1.Relative_Disp[mid, j] * mult * (-1);

                }
            }


            Y[mid, 1] = V.Max();
            Y[mid, 2] = Array.IndexOf(V, V.Max());
            Y[mid, 3] = V.Min();
            Y[mid, 4] = Array.IndexOf(V, V.Min());

            return Y;
        }

        private double[,] find_Absoulute(double[, ,] x, int n, int mult)
        {


            double[] V = new double[Form1.segment2 + 1];
            double[,] Y = new double[Form1.totalmember + 1, 6 + 1];
            if (n != 6)
            {
                for (int j = 0; j <= Form1.segment2; j = j + 1)
                {
                    V[j] = Math.Abs(x[mid, n, j] * mult);

                }
            }
            if (n == 6)
            {
                for (int j = 0; j <= Form1.segment2; j = j + 1)
                {
                    V[j] = Math.Abs(Form1.Relative_Disp[mid, j] * mult * (-1));

                }
            }


            Y[mid, 1] = V.Max();
            Y[mid, 2] = Array.IndexOf(V, V.Max());
            Y[mid, 3] = V.Min();
            Y[mid, 4] = Array.IndexOf(V, V.Min());

            return Y;
        }






        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

            double fracx = Form1.L_n(mid) / Form1.segment2 * 1d;
            double lx1, lx2, lx4, lx5, lxR = 0;
            string sx1, sx2, sx4, sx5, sxR = null;

            try
            {
                draw_Horizontal(mid, Form1.Fact[mid, 5], 80, 5, e.Graphics);
                draw_Horizontal(mid, Form1.Fact[mid, 1], 180, 1, e.Graphics);
                draw_Horizontal(mid, Form1.Fact[mid, 2], 280, 2, e.Graphics);

                draw_Horizontal(mid, Form1.Fact[mid, 4], 380, 4, e.Graphics);
                draw_Horizontal_RelDisp(mid, Form1.Fact[mid, 4], 480, e.Graphics);



                double frac = 480d / Form1.segment2;



                float r = 2.5F;
                e.Graphics.DrawEllipse(Pens.Black, (float)(start + AFY_Indx * frac - r), (float)(80 - AFY_Y * Form1.Fact[mid, 5] - r), r * 2, r * 2);
                e.Graphics.DrawEllipse(Pens.Black, (float)(start + AFY_Indx * frac - r), (float)(80) - r, r * 2, r * 2);


                e.Graphics.DrawEllipse(Pens.Black, (float)(start + VxY_Indx * frac - r), (float)(180 - VxY_Y * Form1.Fact[mid, 1] - r), r * 2, r * 2);
                e.Graphics.DrawEllipse(Pens.Black, (float)(start + VxY_Indx * frac - r), (float)(180) - r, r * 2, r * 2);

                e.Graphics.DrawEllipse(Pens.Black, (float)(start + MxY_Indx * frac - r), (float)(280 - MxY_Y * Form1.Fact[mid, 2] - r), r * 2, r * 2);
                e.Graphics.DrawEllipse(Pens.Black, (float)(start + MxY_Indx * frac - r), (float)(280), r * 2, r * 2);

                e.Graphics.DrawEllipse(Pens.Black, (float)(start + YxY_Indx * frac - r), (float)(380 - YxY_Y * Form1.Fact[mid, 4] - r), r * 2, r * 2);
                e.Graphics.DrawEllipse(Pens.Black, (float)(start + YxY_Indx * frac - r), (float)(380) - r, r * 2, r * 2);

                e.Graphics.DrawEllipse(Pens.Black, (float)(start + YxYR_Indx * frac - r), (float)(480 + YxY_YR * Form1.Fact[mid, 4] - r), r * 2, r * 2);
                e.Graphics.DrawEllipse(Pens.Black, (float)(start + YxYR_Indx * frac - r), (float)(480) - r, r * 2, r * 2);





                e.Graphics.DrawString((("Axial Force kN:").ToString()), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (10), (80), new StringFormat(StringFormatFlags.NoClip));
                e.Graphics.DrawString((("Shear Force kN:").ToString()), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (10), (180 - 10), new StringFormat(StringFormatFlags.NoClip));
                e.Graphics.DrawString((("Bending Moment kN-m:").ToString()), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (10), (280 - 10), new StringFormat(StringFormatFlags.NoClip));
                e.Graphics.DrawString((("Displacement from" + Environment.NewLine + "original position to" + Environment.NewLine + "Loacal Y direction mm:").ToString()), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (10), (380 - 10), new StringFormat(StringFormatFlags.NoClip));
                e.Graphics.DrawString((("Displacement from" + Environment.NewLine + "Member Ends mm:").ToString()), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (10), (480 - 10), new StringFormat(StringFormatFlags.NoClip));







                if (bool_max == true)
                {
                    e.Graphics.DrawLine(new Pen(Color.Red, 2.0F), (float)(start + AFY_Indx * frac), (float)(80), (float)(start + AFY_Indx * frac), (float)(80 - AFY_Y * Form1.Fact[mid, 5]));
                    e.Graphics.DrawLine(new Pen(Color.Red, 2.0F), (float)(start + VxY_Indx * frac), (float)(180), (float)(start + VxY_Indx * frac), (float)(180 - VxY_Y * Form1.Fact[mid, 1]));
                    e.Graphics.DrawLine(new Pen(Color.Red, 2.0F), (float)(start + MxY_Indx * frac), (float)(280), (float)(start + MxY_Indx * frac), (float)(280 - MxY_Y * Form1.Fact[mid, 2]));
                    e.Graphics.DrawLine(new Pen(Color.Red, 2.0F), (float)(start + YxY_Indx * frac), (float)(380), (float)(start + YxY_Indx * frac), (float)(380 - YxY_Y * Form1.Fact[mid, 4]));
                    e.Graphics.DrawLine(new Pen(Color.Red, 2.0F), (float)(start + YxYR_Indx * frac), (float)(480), (float)(start + YxYR_Indx * frac), (float)(480 + YxY_YR * Form1.Fact[mid, 4]));


                    y1 = y2 = y4 = y5 = yR = 0;

                    y1 = VxY_Y;
                    if (Math.Abs(y1) > 1) { s1 = y1.ToString("0.00"); }
                    if (Math.Abs(y1) < 1) { s1 = y1.ToString("0.000"); }

                    y2 = MxY_Y;
                    if (Math.Abs(y2) > 1) { s2 = y2.ToString("0.00"); }
                    if (Math.Abs(y2) < 1) { s2 = y2.ToString("0.000"); }
                    y4 = YxY_Y;
                    if (Math.Abs(y4) > 1) { s4 = y4.ToString("0.00"); }
                    if (Math.Abs(y4) < 1) { s4 = y4.ToString("0.000"); }
                    y5 = AFY_Y;
                    if (Math.Abs(y5) > 1) { s5 = y5.ToString("0.00"); }
                    if (Math.Abs(y5) < 1) { s5 = y5.ToString("0.000"); }
                    yR = -YxY_YR;
                    if (Math.Abs(yR) > 1) { sR = yR.ToString("0.00"); }
                    if (Math.Abs(yR) < 1) { sR = yR.ToString("0.000"); }



                    lx1 = VxY_Indx * fracx;
                    sx1 = lx1.ToString("0.00");
                    lx2 = MxY_Indx * fracx;
                    sx2 = lx2.ToString("0.00");
                    lx4 = YxY_Indx * fracx;
                    sx4 = lx4.ToString("0.00");
                    lx5 = AFY_Indx * fracx;
                    sx5 = lx5.ToString("0.00");
                    lxR = YxYR_Indx * fracx;
                    sxR = lxR.ToString("0.00");




                    e.Graphics.DrawString((s5 + " kN ," + " at " + sx5 + " m"), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (start + 480 + 40), (80), new StringFormat(StringFormatFlags.NoClip));
                    e.Graphics.DrawString((s1 + " kN," + " at " + sx1 + " m"), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (start + 480 + 40), (180 - 10), new StringFormat(StringFormatFlags.NoClip));
                    e.Graphics.DrawString((s2 + " kN-m," + " at " + sx2 + " m"), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (start + 480 + 40), (280 - 10), new StringFormat(StringFormatFlags.NoClip));
                    e.Graphics.DrawString((s4 + " mm," + " at " + sx4 + " m"), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (start + 480 + 40), (380 - 10), new StringFormat(StringFormatFlags.NoClip));
                    e.Graphics.DrawString((sR + " mm," + " at " + sxR + " m"), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (start + 480 + 40), (480 - 10), new StringFormat(StringFormatFlags.NoClip));


                }



                if (bool_moving == true)
                {

                    if (Location_Move.X >= start - 10 & Location_Move.X <= 480 + start + 10)
                    {
                        e.Graphics.DrawLine(new Pen(Color.Gray, 1.50F), Location_Move.X, 20, Location_Move.X, 520);


                    }

                    label3.Text = (Location_Move.X - start).ToString();


                    if (Location_Move.X >= start & Location_Move.X <= start + 480)
                    {
                        y1 = y2 = y4 = y5 = yR = lx = dx = 0;

                        dx = (int)((Location_Move.X - start) / 4d);
                        y1 = Form1.BMD_ALL[mid, 1, dx];
                        if (Math.Abs(y1) > 1) { s1 = y1.ToString("0.00"); }
                        if (Math.Abs(y1) < 1) { s1 = y1.ToString("0.000"); }

                        y2 = Form1.BMD_ALL[mid, 2, dx];
                        if (Math.Abs(y2) > 1) { s2 = y2.ToString("0.00"); }
                        if (Math.Abs(y2) < 1) { s2 = y2.ToString("0.000"); }
                        y4 = 1000 * Form1.BMD_ALL[mid, 4, dx];
                        if (Math.Abs(y4) > 1) { s4 = y4.ToString("0.00"); }
                        if (Math.Abs(y4) < 1) { s4 = y4.ToString("0.000"); }
                        y5 = Form1.BMD_ALL[mid, 5, dx];
                        if (Math.Abs(y5) > 1) { s5 = y5.ToString("0.00"); }
                        if (Math.Abs(y5) < 1) { s5 = y5.ToString("0.000"); }
                        yR = -Form1.Relative_Disp[mid, dx];
                        if (Math.Abs(yR) > 1) { sR = yR.ToString("0.00"); }
                        if (Math.Abs(yR) < 1) { sR = yR.ToString("0.000"); }


                        lx = dx * Form1.L_n(mid) / Form1.segment2 * 1d;
                        sx = lx.ToString("0.00");

                    }
                    if (Location_Move.X >= start - 5 & Location_Move.X <= start + 480 + 5)
                    {

                        e.Graphics.DrawString((s5 + " kN ," + " at " + sx + " m"), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (start + 480 + 40), (80), new StringFormat(StringFormatFlags.NoClip));
                        e.Graphics.DrawString((s1 + " kN," + " at " + sx + " m"), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (start + 480 + 40), (180 - 10), new StringFormat(StringFormatFlags.NoClip));
                        e.Graphics.DrawString((s2 + " kN-m," + " at " + sx + " m"), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (start + 480 + 40), (280 - 10), new StringFormat(StringFormatFlags.NoClip));
                        e.Graphics.DrawString((s4 + " mm," + " at " + sx + " m"), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (start + 480 + 40), (380 - 10), new StringFormat(StringFormatFlags.NoClip));
                        e.Graphics.DrawString((sR + " mm," + " at " + sx + " m"), new Font("Segoe UI", 12, FontStyle.Regular), new SolidBrush(Color.Gray), (start + 480 + 40), (480 - 10), new StringFormat(StringFormatFlags.NoClip));

                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Print("Paint Exception caught.", ex);
            }

        }

        private void draw_Horizontal(int mid, double scalev, double y1, int nnn, Graphics gg)
        {
            gg.SmoothingMode = SmoothingMode.AntiAlias;
            int i = Form1.Last_index_line_selected + 1;

            double[,] arr2 = new double[Form1.totalmember + 1, Form1.segment2 + 1];

            PointF[] Gy = new PointF[Form1.segment2 + 1];
            PointF[] Gx = new PointF[Form1.segment2 + 1];


            int j = 0;

            double x1 = 0;

            double x2 = 0;
            double y2 = 0;

            double xx = 0;
            double[] dy = new double[Form1.segment2 + 1];
            string[] Str = new string[Form1.segment2 + 1];

            double frac = 480d / Form1.segment2;

            for (j = 0; j <= Form1.segment2; j++)
            {






                xx = j * frac;

                x1 = start;

                x2 = (x1 + xx);






                arr2[mid, j] = Form1.BMD_ALL[mid, nnn, j];
                if (nnn != 4)
                {
                    y2 = (y1 - arr2[mid, j] * scalev);
                }
                if (nnn == 4)
                {
                    y2 = (y1 - arr2[mid, j] * scalev * 1000);
                }


                Gx[j].X = (float)x2;
                Gx[j].Y = (float)y1;

                Gy[j].X = (float)x2;
                Gy[j].Y = (float)y2;
                gg.DrawLine(new Pen(Color.Gray, 0.0F), (float)x1, (float)y1, (float)x2, (float)y1);























            }







            PointF[] Foot_of_Ordinate = new PointF[Form1.segment2 + 1];
            PointF[] Ordinate = new PointF[Form1.segment2 + 1 + 2];
            Ordinate[0] = Gx[0];
            Ordinate[Form1.segment2 + 2] = Gx[Form1.segment2];


            for (j = 0; j <= Form1.segment2; j++)
            {
                Foot_of_Ordinate[j] = Gx[j];
                Ordinate[j + 1] = Gy[j];

            }
            gg.DrawLines(new Pen(Color.Gray, 0.0F), Ordinate);








            for (j = 0; j <= Form1.segment2; j += 12)
            {
                gg.DrawLine(new Pen(Color.Gray, 1.0F), Foot_of_Ordinate[j], Ordinate[j + 1]);
                if (nnn != 4)
                {
                    y2 = (y1 - arr2[mid, j] * scalev);
                    if (Math.Abs(arr2[mid, j] * scalev) > 1) gg.DrawString(((arr2[mid, j]).ToString("#.00")), new Font("Microsoft Sans Serif", 8, FontStyle.Regular), new SolidBrush(Color.Magenta), (Ordinate[j + 1].X), (Ordinate[j + 1].Y), new StringFormat(StringFormatFlags.NoClip));

                    if (Math.Abs(arr2[mid, j] * scalev) < 1) gg.DrawString(((arr2[mid, j]).ToString("0.000")), new Font("Microsoft Sans Serif", 8, FontStyle.Regular), new SolidBrush(Color.Magenta), (Ordinate[j + 1].X), (Ordinate[j + 1].Y), new StringFormat(StringFormatFlags.NoClip));
                }
                if (nnn == 4)
                {
                    y2 = (y1 - arr2[mid, j] * scalev * 1000);
                    if (Math.Abs(arr2[mid, j] * scalev * 1000) > 1) gg.DrawString(((arr2[mid, j] * 1000).ToString("#.00")), new Font("Microsoft Sans Serif", 8, FontStyle.Regular), new SolidBrush(Color.Magenta), (Ordinate[j + 1].X), (Ordinate[j + 1].Y), new StringFormat(StringFormatFlags.NoClip));
                    if (Math.Abs(arr2[mid, j] * scalev * 1000) < 1) gg.DrawString(((arr2[mid, j] * 1000).ToString("0.000")), new Font("Microsoft Sans Serif", 8, FontStyle.Regular), new SolidBrush(Color.Magenta), (Ordinate[j + 1].X), (Ordinate[j + 1].Y), new StringFormat(StringFormatFlags.NoClip));
                }

            }

        }



        private void draw_Horizontal_RelDisp(int mid, double scalev, double y1, Graphics gg)
        {
            gg.SmoothingMode = SmoothingMode.AntiAlias;
            int i = Form1.Last_index_line_selected + 1;



            PointF[] Gy = new PointF[Form1.segment2 + 1];
            PointF[] Gx = new PointF[Form1.segment2 + 1];


            int j = 0;

            double x1 = 0;

            double x2 = 0;
            double y2 = 0;

            double xx = 0;
            double[] dy = new double[Form1.segment2 + 1];
            string[] Str = new string[Form1.segment2 + 1];





            double frac = 480d / Form1.segment2;

            for (j = 0; j <= Form1.segment2; j++)
            {






                xx = j * frac;

                x1 = start;

                x2 = (x1 + xx);








                y2 = (y1 + Form1.Relative_Disp[mid, j] * scalev);



                Gx[j].X = (float)x2;
                Gx[j].Y = (float)y1;

                Gy[j].X = (float)x2;
                Gy[j].Y = (float)y2;
                gg.DrawLine(new Pen(Color.Gray, 0.0F), (float)x1, (float)y1, (float)x2, (float)y1);




















            }







            PointF[] Foot_of_Ordinate = new PointF[Form1.segment2 + 1];
            PointF[] Ordinate = new PointF[Form1.segment2 + 1 + 2];
            Ordinate[0] = Gx[0];
            Ordinate[Form1.segment2 + 2] = Gx[Form1.segment2];
            for (j = 0; j <= Form1.segment2; j++)
            {
                Foot_of_Ordinate[j] = Gx[j];
                Ordinate[j + 1] = Gy[j];
            }
            gg.DrawLines(new Pen(Color.Gray, 0.0F), Ordinate);
            for (j = 0; j <= Form1.segment2; j++)
            {







            }


            for (j = 0; j <= Form1.segment2; j += 12)
            {


            }

            int k = 0;
            if (Form1.segment2 == 120) k = 12;
            if (Form1.segment2 == 12) k = 1;
            if (Form1.segment2 == 10) k = 1;
            for (j = 0; j <= Form1.segment2; j += k)
            {
                gg.DrawLine(new Pen(Color.Gray, 1.0F), Foot_of_Ordinate[j], Ordinate[j + 1]);


                if (Math.Abs(Form1.Relative_Disp[mid, j]) > 1) gg.DrawString(((-Form1.Relative_Disp[mid, j]).ToString("#.00")), new Font("Microsoft Sans Serif", 8, FontStyle.Regular), new SolidBrush(Color.Magenta), (Ordinate[j + 1].X), (Ordinate[j + 1].Y), new StringFormat(StringFormatFlags.NoClip));
                if (Math.Abs(Form1.Relative_Disp[mid, j]) < 1) gg.DrawString(((-Form1.Relative_Disp[mid, j]).ToString("0.000")), new Font("Microsoft Sans Serif", 8, FontStyle.Regular), new SolidBrush(Color.Magenta), (Ordinate[j + 1].X), (Ordinate[j + 1].Y), new StringFormat(StringFormatFlags.NoClip));



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

        }

        private void FShowTable_FormClosing(object sender, FormClosingEventArgs e)
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



        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {






            int x = e.X;
            int y = e.Y;
            SnapToGrid(ref x, ref y);
            Location_Move = new Point(x, y);
            label2.Text = x + " , " + y;



            PictureBox1.Invalidate();
        }


        private void SnapToGrid(ref int x, ref int y)
        {
            double frac = 480d / Form1.segment2;




            int x1 = Convert.ToInt32(((double)(x) / 4d));
            int y1 = Convert.ToInt32(((double)(y) / 4d));

            x = Convert.ToInt32(x1 * 4d);
            y = Convert.ToInt32(y1 * 4d);

        }



        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                bool_max = true;
                bool_moving = false;
            }
            PictureBox1.Invalidate();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                bool_max = false;
                bool_moving = true;
                Cursor.Position = new Point(PictureBox1.Left + PictureBox1.Width / 2,
                               PictureBox1.Top + PictureBox1.Height / 2);
            }
            PictureBox1.Invalidate();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            int i = mid;
   
           
        int comboBoxValue = int.Parse(comboBox1.Text);
        dataGridView1.Rows.Clear();
       

        List<DataGridViewRow> rows = new List<DataGridViewRow>();

        string[] rowx = new string[7];
        
        int j = 0;
        int k = 0;
        int kk = 0;
        if (Form1.segment2 == 120) k = 120 / comboBoxValue;
        if (Form1.segment2 == 12) k = 1;
        if (Form1.segment2 == 10) k = 1;
        for (j = 0; j <= Form1.segment2; j += k)
        
        {
            kk = kk + 1;

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridView1); // Create cells matching the column count
            rowx[0] = (kk).ToString();
            rowx[1] = ((Form1.L_n(Form1.Last_index_line_selected + 1) * j / Form1.segment2 ).ToString("0.00"));
                    rowx[2] = Form1.BMD_ALL[i, 5, j].ToString("0,0.000");
                    rowx[3] = Form1.BMD_ALL[i, 1, j].ToString("0,0.000");
                    rowx[4] = Form1.BMD_ALL[i, 2, j].ToString("0,0.000");
                    rowx[5] = (Form1.BMD_ALL[i, 4, j] * 1000).ToString("0,0.000");
                    rowx[6] = (-Form1.Relative_Disp[i, j]).ToString("0,0.000");

                    dataGridView1.Rows.Add(rowx);
                    
                }
        dataGridView1.Rows.AddRange(rows.ToArray());




                                dataGridView1.Refresh();
       
        
        }

        //==================================================================

    }
   
}
        
  



    

