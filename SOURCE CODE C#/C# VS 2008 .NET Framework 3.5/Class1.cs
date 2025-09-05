namespace Graphical_2D_Frame_Analysis_CSharp
{
    class Class1
    {
        
        
        
        
        
        ////#########################################################################################
        //if (Test_PictureBox1_Paint == true)
        //    {
        //        gr.Clear(PictureBox1.BackColor);
        //        gr.SmoothingMode = SmoothingMode.AntiAlias;
        //        ////ggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg

        //        gr.Clear(PictureBox1.BackColor);
        //        gr.SmoothingMode = SmoothingMode.AntiAlias;
        //        ////ggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg
        //        draw_tSupp_SRING_Vert(100, 115, gr);
        //        draw_tSupp_Guided_vertical(100, 100, gr);
        //        //draw_traiangle(200, 200, gr);
        //        //gr.DrawEllipse(new Pen( Color.Gray, 3.0F), 200, 200, 5, 5);
        //        draw_tSupp_SRING_Horz(215, 200, gr);
        //        draw_tSupp_Guided_horizontal(200, 200, gr);
        //        //  //kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk
        //        //  //kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk
        //        // // draw_rectangle(225, 90, gr);
        //        // // draw_triangle_Pinned_Support(200, 100, 90, gr);
        //        // // draw_triangle_Pinned_Support(200, 100, 180, gr);
        //        // // draw_triangle_Pinned_Support(200, 100, 270, gr);

        //        draw_triangle_Roller_support(300, 200, 0, gr);
        //        draw_triangle_Roller_support(400, 200, 50, gr);
        //        draw_triangle_Roller_support(500, 200, 90, gr);
        //        draw_triangle_Roller_support(600, 200, 180, gr);
        //        draw_triangle_Roller_support(700, 200, 270, gr);
        //        /////==================================================================

        //        //////   //Graphics g= PictureBox1.CreateGraphics();
        //        //////   //drawbigline(gr);

        //        //////   /////float th = -45;// rotation anticlocwise+ve
        //        //////   //X1after trans = X + tx;Y1 after trans = Y + ty
        //        //////   //x1 after rotation anticlocwise+ve = x cos th — y sin th
        //        //////   //y1 after rotation anticlocwise+ve = x sin th + y cos th



        //        //////kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk



        //        //  PointF p0 = new PointF(0, 0);

        //        //  ///// Create points that define curve.
        //        //  PointF point1 = new PointF(50.0F, 50.0F);
        //        //  PointF point2 = new PointF(100.0F, 75.0F);
        //        //  PointF point3 = new PointF(150.0F, 100.0F);
        //        //  PointF point4 = new PointF(200.0F, 110.0F);
        //        //  PointF point5 = new PointF(300.0F, 100.0F);
        //        //  PointF point6 = new PointF(350.0F, 75.0F);
        //        //  PointF point7 = new PointF(400.0F, 50.0F);
        //        //  PointF point8 = new PointF(500.0F, 100.0F);
        //        //  PointF[] curvePoints = { point1, point2, point3, point4, point5, point6, point7 };
        //        //  PointF[] Points2 = { point1, point7};
        //        //  gr.DrawLine(new Pen( Color.Gray, 1.0F), point1, point7);
        //        //  gr.DrawLine(new Pen(Color.Magenta, 3.0F), point1, point7);
        //        //  ////// Draw lines between original points to screen.
        //        //  gr.DrawCurve(new Pen(Color.Red, 1), curvePoints);
        //        //  PointF[] curvePoints1 = Translate_Array_PointF(curvePoints, 100, 100);
        //        //  gr.DrawCurve(new Pen(Color.Green, 1), curvePoints1);
        //        //  gr.DrawLine(new Pen(Color.Green, 1), curvePoints1[0], curvePoints1[6]);
        //        //  double L_1 = Math.Sqrt(Math.Pow(point1.X - point8.X, 2) + Math.Pow(point1.Y - point8.Y, 2));

        //        //  double angle = (Math.Atan2((point1.Y - point8.Y), (point1.X - point8.X))) * 180 / Math.PI; // radian to degree
        //        //  angle = (Math.Atan2((point8.Y - point1.Y), L_1)) * 180 / Math.PI;
        //        //  if (angle < 0)
        //        //  {
        //        //      angle = 360 + angle;
        //        //  }
        //        //  Debug.Print("angle= " + angle);
        //        //  //angle = -1 * (float)angle;//ok
        //        //  //angle = 60f;//ok
        //        //  gr.DrawLine(new Pen(Color.Black, 1.0F), point1, rotate_PointF(point7, point1, (float)angle));

        //        //  PointF[] curvePoints_Rotate_aboutOrigin = rotate_Array_PointF(curvePoints, p0, (float)angle);
        //        //  PointF[] Points2_Rotate_aboutOrigin = rotate_Array_PointF(Points2, p0, (float)angle);
        //        //  PointF[] curvePoints3 = Translate_Array_PointF(curvePoints_Rotate_aboutOrigin, p0.X, p0.Y);
        //        //  PointF[] Points2_Rotate_aboutOrigin2 = Translate_Array_PointF(Points2, p0.X, p0.Y);
        //        // gr.DrawCurve(new Pen(Color.Cyan, 2.0F), Translate_Array_PointF(curvePoints_Rotate_aboutOrigin, p0.X, p0.Y));//at origin
        //        // gr.DrawCurve(new Pen(Color.Cyan, 2.0F), Translate_Array_PointF(Points2_Rotate_aboutOrigin, p0.X, p0.Y));//at origin
        //        // gr.DrawCurve(new Pen(Color.Black, 2.0F), Translate_Array_PointF(curvePoints3, point1.X, point1.Y));// shifted to point1
        //        // gr.DrawCurve(new Pen(Color.Black, 2.0F), Translate_Array_PointF(Points2_Rotate_aboutOrigin2, point1.X, point1.Y));// shifted to point1

        //        //  //////kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk
        //        //  //// ///////Draw lines to screen.

        //        // //////line rotate ex11111 ggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg
        //        // PointF p00 = new PointF(0, 0);
        //        // PointF p1x = new PointF(200, 100);
        //        // PointF p2x = new PointF(250, 150);
        //        // gr.DrawLine(new Pen( Color.Gray, 1), p1x, p2x);
        //        // gr.DrawLine(new Pen(Color.Red, 1), p1x, rotate_PointF(p2x, p1x, 30F));
        //        // gr.DrawLine(new Pen(Color.Red, 1), p1x, rotate_PointF(p2x, p1x, 30F));
        //        // gr.DrawLine(new Pen(Color.Red, 1), rotate_PointF(p1x, p2x, 60F), p2x);
        //        // //////line rotate ex2222 ggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg
        //        // float th = -45;//+ve clockwise,-ve rotation anticlockwise+ve

        //        // PointF p1 = new PointF(150, 100);
        //        // PointF p2 = new PointF(200, 180);
        //        // PointF[] points5 = { p1, p2 };
        //        // gr.DrawLines(new Pen( Color.Gray, 10.0F), Translate_Array_PointF(points5, 300, 100));

        //        // gr.DrawLine(new Pen(Color.Magenta, 10.0F), p1, p2);
        //        //  gr.DrawLine(new Pen(Color.Black, 10.0F), Translate_PointF(p1,0,0), Translate_PointF(p2,0,0));
        //        // gr.DrawLine(new Pen(Color.Green, 1.0F), rotate_PointF(p1, p1, th), rotate_PointF(p2, p1, th));
        //        // gr.DrawLine(new Pen(Color.Green, 10.0F), p1, rotate_PointF(p2, p1, th));//=gr.DrawLine(new Pen(Color.Green, 1.0F), rotate_PointF(p1, p1, th), rotate_PointF(p2, p1, th));
        //        // gr.DrawLine(new Pen(Color.Cyan, 10.0F), p2, rotate_PointF(p1, p2, th));
        //        //  //////END line rotate  ggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg
        //        //==============================================
        //        draw_Cursor(250, 100, -140F, gr);
        //        draw_Cursor_Node(200, 100, -140F, gr);



        //    }////end if (Test_PictureBox1_Paint == false)

        /////end test ####################################################################################################

        ///////@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        ////private void test_draw_example2(Graphics g)
        ////{
        ////    g.Clear(PictureBox1.BackColor);
        ////    g.SmoothingMode = SmoothingMode.AntiAlias;

        ////    PointF[] points2 =
        ////     {
                
        ////         new PointF(200.0F,  150.0F),
        ////         new PointF(250.0F, 150.0F)
        ////     };

        ////    float th = -45;
        ////    PointF p0 = new PointF(0, 0);
        ////    PointF p1 = new PointF(100, 100);
        ////    PointF p2 = new PointF(200, 100);
        ////    PointF[] points5 = { p1, p2 };


        ////    Pen redPen = new Pen(Color.Red, 1);
        ////    Pen greenPen = new Pen(Color.Green, 3);


        ////    PointF point1 = new PointF(50.0F, 50.0F);
        ////    PointF point2 = new PointF(100.0F, 75.0F);
        ////    PointF point3 = new PointF(150.0F, 100.0F);
        ////    PointF point4 = new PointF(200.0F, 110.0F);
        ////    PointF point5 = new PointF(300.0F, 100.0F);
        ////    PointF point6 = new PointF(350.0F, 75.0F);
        ////    PointF point7 = new PointF(400.0F, 50.0F);
        ////    PointF point8 = new PointF(400.0F, 100.0F);
        ////    PointF[] curvePoints = { point1, point2, point3, point4, point5, point6, point7 };
        ////    PointF[] Points2 = { point1, point8 };
        ////    g.DrawLine(new Pen(Color.Gray, 1.0F), point1, point7);
        ////    g.DrawLine(new Pen(Color.Magenta, 1.0F), point1, point8);
        ////    // Draw lines between original points to screen.
        ////    g.DrawCurve(new Pen(Color.Red, 1), curvePoints);
        ////    PointF[] curvePoints1 = Translate_Array_PointF(curvePoints, 100, 100);
        ////    g.DrawCurve(new Pen(Color.Green, 1), curvePoints1);
        ////    g.DrawLine(new Pen(Color.Green, 1), curvePoints1[0], curvePoints1[6]);
        ////    double L_1 = Math.Sqrt(Math.Pow(point1.X - point8.X, 2) + Math.Pow(point1.Y - point8.Y, 2));

        ////    double angle = (Math.Atan2((point1.Y - point8.Y), (point1.X - point8.X))) * 180 / Math.PI; // radian to degree
        ////    angle = (Math.Atan2((point8.Y - point1.Y), L_1)) * 180 / Math.PI;
        ////    if (angle < 0)
        ////    {
        ////        angle = 360 + angle;
        ////    }

        ////    PointF[] curvePoints2 = rotate_Array_PointF(curvePoints, p0, (float)angle);
        ////    PointF[] Points22 = rotate_Array_PointF(Points2, p0, (float)angle);
        ////    PointF[] curvePoints3 = Translate_Array_PointF(curvePoints2, p0.X, p0.Y);
        ////    PointF[] Points222 = Translate_Array_PointF(Points2, p0.X, p0.Y);

        ////    g.DrawCurve(new Pen(Color.Black, 1.0F), Translate_Array_PointF(curvePoints3, point1.X, point1.Y));
        ////    g.DrawCurve(new Pen(Color.Black, 1.0F), Translate_Array_PointF(Points222, point1.X, point1.Y));

        ////}



       //private void button3_Click(object sender, EventArgs e)
       // {





       //     ////////=================================================================
       //     ////double[] result = new double[2];
       //     ////double[] array1 = { 200, -50, 60.57, 30, 200.1, 10 };

       //     //////int[] index = { 0, 1, 2, 3, 4, 5 };
       //     ////int[] index = new int[array1.Length];
       //     ////Debug.WriteLine("array1.Length= " + array1.Length);
       //     ////for (int k = 1; k <= array1.Length - 1; k++)
       //     ////{
       //     ////    index[k] = k;
       //     ////    Debug.WriteLine(k + "index[k]= " + index[k]);

       //     ////}
       //     ////Debug.WriteLine(MakeDisplayable_1D_j_0_INT(index));
       //     ////int tmp = 0;
       //     ////for (int i = 1; i <= array1.Length; i++)
       //     ////{
       //     ////    for (int j = 0; j < index.Length - 1; j++)
       //     ////    {
       //     ////        if (array1[index[j]] > array1[index[j + 1]])
       //     ////        {
       //     ////            tmp = index[j + 1];
       //     ////            index[j + 1] = index[j];
       //     ////            index[j] = tmp;
       //     ////            //Debug.WriteLine("i, j, index[j + 1], array1[index[j + 1]]= " + i + " , " + j + " , " + index[j + 1] + " , " + array1[index[j + 1]]);
       //     ////        }
       //     ////        Debug.WriteLine("i, index[j + 1], array1[index[j + 1]]= " + i + " , " + index[j + 1] + " , " + array1[index[j + 1]]);
       //     ////    }
       //     ////}

       //     ////// Display month number and customer amount for month with highest amount of customers
       //     ////Debug.WriteLine(index[array1.Length - 1] + " , " + array1[index[array1.Length - 1]]);
       //     ////result[0] = array1[index[array1.Length - 1]];
       //     ////result[1] = index[array1.Length - 1];



       //     //////Debug.Print("Find_bool_Support_Displacement yyxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx =" + Find_bool_Support_Displacement());










       //     //            double d = 3D;
       //     //            Debug.Print("ddd  =" + d);

       //     //double[,] cmp1 = K_2_G_Element_i_1_j_1(1);//K_Global_Element_Matrix_i_0_j_0(1);//K_Local_Element_Matrix_i_0_j_0(1);//K_G_Element_ij_11(1);

       //     //for (int i = 0; i <= cmp1.GetUpperBound(0); i++)
       //     //            {
       //     //                for (int j = 0; j <= cmp1.GetUpperBound(1); j++)
       //     //                {
       //     //                    Debug.Print("test  =" + cmp1[i, j]);
       //     //}
       //     //}

       //     //System.Text.StringBuilder str = Read_Red_Data_as_array_of_line();
       //     //Debug.Print(" Read_Red_Data_as_array_of_line()= " + Environment.NewLine + str);

       //     //System.Text.StringBuilder str = Read_Input_Data_as_array_of_line();
       //     //Debug.Print(" Read_Input_Data_as_array_of_line()= " + Environment.NewLine + str);


       //     //string[] test = new string[] { };
       //     //string[] sss = new string[1];
       //     //sss[0] = "KKKKK";
       //     //Array.Resize(ref sss, 5);
       //     ////sss[0] = "KKKKK";
       //     //for (int i = 0; i < sss.Length; i++)
       //     //{
       //     //    if (!string.IsNullOrEmpty(sss[i])) // 21 June 21 to avoid nul exception
       //     //    {

       //     //        test = sss[i].Split(new char[] { ',' });

       //     //        for (int j = 0; j < test.Length; j++)
       //     //        {

       //     //            //Debug.Print(i + "test yyxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx =" + test[i]);
       //     //        }
       //     //        //Debug.Print(i + "," + sss[i]);
       //     //    }
       //     //}

       //     //string[] str = new string[] { };
       //     //Console.WriteLine("String Array elements won't get displayed since it's empty...");
       //     //for (int i = 0; i < str.Length; i++)
       //     //{
       //     //    string res1 = str[i];
       //     //    Debug.Print(res1);
       //     //}
       //     //Debug.Print(" Find_All_Zero_Ploady_Sring= " + Find_All_Zero_Ploady_Sring("2,0,0,0,0,0,0,0,0,0,0,0,0,LY"));
       //     //Debug.Print(" Find_All_Zero_Ploady_Sring= " + Find_All_Zero_Ploady_Sring("2,50,7,0,0,0,0,0,0,0,0,0,0,LY"));
       //     //Debug.Print(" Find_k_Index_Ploady_Sring, 13= " + Find_k_Index_Ploady_Sring("2,5,1,30,2,0,0,0,0,0,0,0,0,GY", 13));
       //     //Debug.Print(" Find_REPLACE_k_Index_Ploady_Sring, 3= " + Find_REPLACE_k_Index_Ploady_Sring("2,5,1,30,2,0,0,0,0,0,0,0,0,LY", 3, "23.5"));
       //     //string res = null;
       //     //res = Find_REPLACE_k_Index_Ploady_Sring("2,5,1,30,2,0,0,0,0,0,0,0,0,LY", 3, "23.5");
       //     //Debug.Print(" Find_REPLACE_k_Index_Ploady_Sring, 3= " + res);


       //     //double[] bbb = new double[5];
       //     ////Array.Clear(bbb, 0, bbb.Length);
       //     //Debug.Print( " HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH" +MakeDisplayable_1D_0_Double(bbb));

       //     //// arr[0]=node num, if single member end arr[1]=endindex(1 StartNode, 2 EndNode),,, if multimember arr[1]=555 last mem num,,,arr[2]=mem count, node not exists  all 0
       //     //1----M=1------2
       //     //              |
       //     //             |MEM=2
       //     //             |3

       //     //////int[] a=Find_Node_Mem_Num_AND_STARTorEND_From_nodecoordinate(nodecoordinate[1]) == 1 , 1 , 1
       //     //////a[0] + " , " + a[1] + " , " + a[2]== 1 , 1 , 1
       //     //int[] a = Find_Node_Mem_Num_AND_STARTorEND_From_nodecoordinate(nodecoordinate[4]);

       //     //Debug.Print(" @@@ Find_Node_Mem_Num_AND_STARTorEND_From_nodecoordinate(nodecoordinate[1]) == " + a[0] + " , " + a[1] + " , " + a[2]);

       //     ////a = Find_Node_Mem_Num_AND_STARTorEND_From_nodecoordinate(nodecoordinate[2]);
       //     ////Debug.Print(" @@@ Find_Node_Mem_Num_AND_STARTorEND_From_nodecoordinate(nodecoordinate[2]) == " + a[0] + " , " + a[1] + " , " + a[2]);
       //     ////a = Find_Node_Mem_Num_AND_STARTorEND_From_nodecoordinate(nodecoordinate[3]);
       //     ////Debug.Print(" @@@ Find_Node_Mem_Num_AND_STARTorEND_From_nodecoordinate(nodecoordinate[3]) == " + a[0] + " , " + a[1] + " , " + a[2]);
       //     ////a = Find_Node_Mem_Num_AND_STARTorEND_From_nodecoordinate(nodecoordinate[4]);
       //     ////Debug.Print(" @@@ Find_Node_Mem_Num_AND_STARTorEND_From_nodecoordinate(nodecoordinate[4]) == " + a[0] + " , " + a[1] + " , " + a[2]);
       //     //int[] c = Find_MemID_From_Joint(4);
       //     //Debug.Print(" @@@ Find_MemID_From_Joint(4) == " + c[0] + " , " + c[1] + " , " + c[2]);




       // }// end Bttn 3


////@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

}
}
