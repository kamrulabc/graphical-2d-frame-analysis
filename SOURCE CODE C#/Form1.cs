
//Fixed Support=1
//Pinned Support=2
//Roller Support Y Restrained only=3 //X, MZ free, Y  restrained ROLLER-Y
//Guided Support Y Restrained only=5 //Y, MZ free, X  restrained ROLLER-X
//Spring support Y Direction =6
//Inclined Roller Support Y Restrained only, rotation or angle of BASE of Support with GX degree anti-dock wise +ve=7

//Sup_Node_Number_Type // string StringSuppData
////======================================================



//   K_FreeLeftRow_BC_Top_Col_Kff             K_FreeRightRow_BC_Top_Col_Kfs               XX     ff(YL)
//----------------------------------------|-------------------------------------------- *---- =  -----
//  K_BC_LeftBottom_Row_LeftBottomCol_Ksf   K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss   dsup    fsup


//dsup=Support_Displacement

//Kff* XX  +  K_FreeRightRow_BC_Top_Col_Kfs*Support_Displacement=YL

//or XX=(YL- K_FreeRightRow_BC_Top_Col_Kfs*Support_Displacement) *[INV(Kff)].....................(1)


//K_BC_LeftBottom_Row_LeftBottomCol_Ksf*XX + K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss*Support_Displacement=fsup..(2)




//=============================================


//T Global to Local axex    |	TT Local to Global axex		
// c	s	0	            |	c	-s	0
//-s	c	0	            |	s	c	0
// 0	0	1	            |	0	0	1


//f1 = f1'cosθ − f2'sinθ
//f2 = f1'sinθ + f2'cosθ
//f3 = f3'
//f4 = f4'cosθ − f5'sinθ
//f5 = f4'sinθ + f5'cosθ
//f6 = f6'



//U´= | cosθ sinθ|  |UGX  >>>>T Global to Local axex
//V´ =|-sinθ cosθ|  |VGY
//        T

//UGX= |cosθ-sinθ|  |U´ >>>TT Local to Global axex	  
//VGY= |sinθ cosθ|  |V´
//         TT              >>>TT=  Inv(T)=Transpose (T)


//T Global to Local axex	          |TT Local to Global axex	
//Local force=T*Global force          |Global f= TT*f local
//Local d=T*Global d                  |Global d= TT*d local

//   kff   | kfs      | Df  |    | Ff |
//---------|------- X |-----| =  |----| ALL GLOBAL AXEX
//    ksf  |kss       | Ds  |    |Fs  |

//TT Local to Global axex:	
//double[,] TT = new double[,]
//            {
//    {c, -s, 0, 0, 0, 0},
//    {s, c, 0, 0, 0, 0},
//    {0, 0, 1, 0, 0, 0},
//    {0, 0, 0, c, -s, 0},
//    {0, 0, 0, s, c, 0},
//    {0, 0, 0, 0, 0, 1}
//};

//T Global to Local axex: 
//double[,] T = new double[,]
//            {
//    {c, s, 0, 0, 0, 0},
//    {-s, c, 0, 0, 0, 0},
//    {0, 0, 1, 0, 0, 0},
//    {0, 0, 0, c, s, 0},
//    {0, 0, 0, -s, c, 0},
//    {0, 0, 0, 0, 0, 1}
//};

//c=cosθ,s=sinθ;  Kglobal​=TT Klocal T
//TT=  Inv(T)=Transpose (T)
//---------------------------------------------------------------
//T Global to Local axex	          |TT Local to Global axex	
//Local force=T*Global force          |Global f= TT*f local
//Local d=T*Global d                  |Global d= TT*d local

using System.Text;
using System.IO;

using System.Drawing.Drawing2D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Globalization;



namespace Graphical_2D_Frame_Analysis_CSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

        }
        
        protected override void WndProc(ref Message m)
        {
            const int WM_PARENTNOTIFY = 0x0210;
            if (m.Msg == WM_PARENTNOTIFY)
            {
                if (!Focused)
                    Activate();
            }
            base.WndProc(ref m);
        }




        public static int totalmember, totalnode, totalnode_From_Lines;
        public static string pj_folder_path;
        public static int[,] Connectivity;
        public static double[] Ayz;
        public static double[] Iz;
        public static double[] Ey;
        public static int[] nodenumer;
        public static float GridX;
        public static float GridY;
        public static List<line> mLines = new List<line>();
        public static PointF[] nodecoordinate;

        public static string St_Read_From_Project;

        private PointF[] nodeall;




        private double nu = 0.3d;

        private double[] ForceEnd_Global_ALL_DOF;
        private double[] Disp_All_Nodes_Global;
        private double[] Disp_All_Nodes_Local;


        private double[,] ForceEnd_Local_To_Global;
        private double[,] ForceEnd_Local_MemComaIndex;
        private double[] fflocal1D;





        private PointF Location_1;
        private PointF Location_2;
        private PointF Location_Move;
        static int clk;
        private List<line> m_Lines1 = new List<line>();
        line Temp1 = new line();
        line Temp9 = new line();



        private List<line> m_Lines = new List<line>();
        private List<line> m_Lines2 = new List<line>();



        private double[,] PjG_memEnd_MebCommaConnectivity;
        private double[,] Result_Mem_Force_global;
        private double[,] Result_Mem_Force_Local;
        private double[,] Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0;
        private double[,] Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0;
        private double[] ForceEnd_Local_ALL_DOF;

        private int[] DOF_Free;

        public static string[] Pload;


        public static string[] PJoint;
        public static string[] Mproperty_A_I_E;

        public static int[] Sup_Node_Number;
        public static int[] suptype;
        private bool Draw_Model = false;

        private bool View_PicBox;
        private bool View_Shear;
        private bool View_Mom;
        private bool View_Theta;
        private bool View_Delta;
        private bool View_AxialForce;







        public static double[,] VxY;
        public static double[,] MxY;
        public static double[,] Y1xY;
        public static double[,] YxY;
        public static double[,] AFY;

        private double[,] VxY3;
        private double[,] MxY3;
        private double[,] Y1xY3;
        private double[,] YxY3;
        private double[,] AFY3;

        public static int segment2, segment3;

        public static double[,] Fact;



        private double[,] Fact3;


        public static double[, ,] BMD_ALL = new double[50, 6, 200];

        public static double[,] Relative_Disp = new double[50, 200];


        public static double[, ,] BMD_ALL3 = new double[50, 6, 200];
        public static double[] Vx = new double[13], Mx = new double[13], Y1x = new double[13], Yx = new double[13], AFx = new double[13];
        private double[] Vx3 = new double[13], Mx3 = new double[13], Y1x3 = new double[13], Yx3 = new double[13], AFx3 = new double[13];


        private bool bool_draw_Disp_Diagram = false;




        private bool ZOOMPlus_bool = false;

        private bool Test_PictureBox1_Paint = false;

        private static float PictureScale = 1.0F;




        private bool down;
        private bool down2;


        public static bool selln;
        public static bool selpt;
        public static bool delln;
        public static bool drawln;

        public static PointF ptSelln;
        public static PointF ptSelNode;
        line Temp_Selected_line = new line();
        public static int Last_index_line_selected = -1;

        private int Number_of_Selected_line = 0;
        public static int nodenumer_Selected = -1;
        public static bool BeamLoadDataInput = false;


        public static double SelBeamLength;
        public static double thetaxx;
        public static int SelBeamStartNode;
        public static int SelBeamEndNode;
        public static int SelBeamMEM_ID;
        public static int SelBeamINDEX;


        public static string[] PloadPcon;
        public static string[] PloadMcon;
        public static string[] PloadUDL;




        public static bool bool_FconcP = false;
        public static bool bool_FconcM = false;
        public static bool bool_FSupDisp = false;
        public static bool bool_FUDL = false;
        public static bool bool_FNodal = false;
        public static bool bool_FSuppData = false;
        public static bool bool_FDelSupp = false;
        public static bool bool_FBeamProp = false;
        public static bool bool_HINGE_Intermediate = false;
        public static bool bool_FShowTable = false;
        public static bool bool_FDirectSupp = false;



        public static bool bool_Mem_Load_Pload_from_Read = false;
        public static bool bool_Mem_Load_Pload_from_Input = false;
        public static bool bool_Node_Load_Joint_from_Read = false;
        public static bool bool_Node_Load_Joint_from_Input = false;
        public static bool bool_SUPPORT_from_Read = false;
        public static bool bool_SUPPORT_from_Input = false;

        public static bool bool_Hinge_from_Read = false;
        public static bool bool_Hinge_from_Input = false;

        public static string[] Sup_Node_Number_Type;

        public static string[] Sup_Node_Number_Type2;

        public static string[] Pload2 = new string[10];
        public static string[] PJoint2 = new string[10];





        private bool bool_DOF_Restrained_G_Greater_Than_2 = true;
        private bool bool_Force_Applied = false;


        private bool bool_delta;
        public static List<line> mLinesDeformed = new List<line>();


        public static string[,] Pconc = new string[100, 100];

        public static double[] Support_Displacement_temp;
        public static double[] Support_Displacement;
        public static string[] Support_Displacement_S;

        public static bool bool_EDIT = false;
        private bool bool_copy_once = true;


        public static bool bool_Fixed_support = false;
        public static bool bool_Pinned_support = false;
        public static bool bool_Delete_support = false;
        public static bool bool_Roller_Supp_Ydir = false;
        public static bool bool_Guided_Supp_Ydir = false;
        public static bool bool_SPRING_Supp_Ydir = false;
        public static bool bool_Inclied_Supp = false;
        public static bool bool_HINGE_Run_Analysis = false;


        public static int Node_Num_SPRING_Supp_Ydir = 0;
        public static double SPRING_Constant = 0;
        public static int Inclied_Supp_Node_num = -1;

        public static double Inclied_Supp_angle_Mem_base = 0;
        public static double Inc_Supp_angle_Mem_RY = 0;
        public static double Inc_Supp_angle_base_AND_GX = 0;
        public static double Inc_Supp_angle_RY_AND_GX = 0;
        private double Inc_Supp_Reaction;

        public static double[] Support_Reaction = new double[100];
        public static bool bool_draw_DOF = false;
        public static bool bool_Pan = false;
        public static int pan_Click = 1;
        public static float left, right, top, bottom;
        private static int fontsize = 8;
        private bool bool_View_Member_Number = true;
        private bool bool_View_Node_Number = true;
        private bool bool_View_Force_Value = true;

        public static PointF Pan_start;


        public static int lenght_factor = 0;
        private int HINGE_Num_total = 0;
        public static string[] HINGE_String;
        public static string[] HINGE_String2;
        public static int[] HINGE_NodeID;
        public static int[] HINGE_New_DOF;
        public static int[] HINGE_MemEnd_Index;
        public static int[] HINGE_MemID;



        public static string[] string_Read = null;
        public static string[] string_Input = null;
        private bool bool_button_run_clicked = false;
        private bool bool_cancel_draw_line = false;
        private bool bool_clear_picbox = false;


        private Bitmap Bm = null;

        private List<line> tempLines = new List<line>();

        private string read_fileName = null;

        private StringBuilder sbtemp11 = new StringBuilder();
        private StringBuilder sbtemp22 = new StringBuilder();
        private StringBuilder sbtemp33 = new StringBuilder();
        private StringBuilder stringBuilder33 = new StringBuilder();
        private StringBuilder stringBuilder22 = new StringBuilder();
        public static StringBuilder stringBuilderALL = new StringBuilder();
        private StringBuilder stringBuilder11 = new StringBuilder();
        private StringBuilderTraceListener traceListener;

        
        private double[] YL;


        private string String_info =

@"Sup_Node_Number_Type
Fixed Support=1
Pinned Support=2
Roller Support Y Restrained only=3 //X, MZ free, Y  restrained ROLLER-Y
Guided Support Y Restrained only=5 //Y, MZ free, X  restrained ROLLER-X
Spring support Y Direction =6
Inclined Roller Support Y Restrained only, rotation or angle of BASE of Support with GX degree anti-dock wise +ve=7

======================================================

   kff   | kfs      | Df  |    | Ff |
---------|------- X |-----| =  |----| ALL GLOBAL AXEX
    ksf  |kss       | Ds  |    |Fs  |

   K_FreeLeftRow_BC_Top_Col_Kff             K_FreeRightRow_BC_Top_Col_Kfs               XX     ff(YL)
----------------------------------------|-------------------------------------------- *---- =  -----
  K_BC_LeftBottom_Row_LeftBottomCol_Ksf   K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss   dsup    fsup


dsup=Support_Displacement

Kff* XX  +  K_FreeRightRow_BC_Top_Col_Kfs*Support_Displacement=YL

or XX=(YL- K_FreeRightRow_BC_Top_Col_Kfs*Support_Displacement) *[INV(Kff)].....................(1)


K_BC_LeftBottom_Row_LeftBottomCol_Ksf*XX + K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss*Support_Displacement=fsup..(2)

=============================================

	TT Local to Global axex:		
double[,] TT = new double[,]
            {
                {c, -s, 0, 0, 0, 0},
                {s, c, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 0},
                {0, 0, 0, c, -s, 0},
                {0, 0, 0, s, c, 0},
                {0, 0, 0, 0, 0, 1}
            };

T Global to Local axex:    
double[,] T = new double[,]
            {
                {c, s, 0, 0, 0, 0},
                {-s, c, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 0},
                {0, 0, 0, c, s, 0},
                {0, 0, 0, -s, c, 0},
                {0, 0, 0, 0, 0, 1}
            };

----------------------------------------------------------
c=cosθ,s=sinθ;  Kglobal​=TT Klocal T
TT=  Inv(T)=Transpose (T)
-------------------------------------------------------------
T Global to Local axex	            |TT Local to Global axex	
Local force=T*Global force          |Global f= TT*f local
Local d=T*Global d                  |Global d= TT*d local
--------------------------------------------------------------
";



       


        //=======================================================

        public void Start1_SbTraceListener()
        {
            stringBuilder11 = new StringBuilder();
            StringBuilderTraceListener traceListener = new StringBuilderTraceListener(stringBuilder11);

            Debug.Listeners.Add(traceListener);
        }
        public void Start2_SbTraceListener()
        {
            stringBuilder22 = new StringBuilder();
            StringBuilderTraceListener traceListener22 = new StringBuilderTraceListener(stringBuilder22);

            Debug.Listeners.Add(traceListener22);
        }
        public void Start3_SbTraceListener()
        {
            stringBuilder33 = new StringBuilder();
            StringBuilderTraceListener traceListener33 = new StringBuilderTraceListener(stringBuilder33);

            Debug.Listeners.Add(traceListener33);
        }
        public void Remove1_SbTraceListener()
        {
            if (traceListener != null)
            {
                Debug.Listeners.Remove(traceListener);
                traceListener.Dispose();
            }
        }


        //===================================================

      


        public static bool Find_bool_Support_Displacement()
        {
            bool bool_Support_Displacement = false;
            double dispx = 0;
            double dispy = 0;



            try
            {
                for (int j = 1; j <= totalnode; j++)
                {
                    if (!string.IsNullOrEmpty(Support_Displacement_S[j]))
                    {

                        string[] test9 = Support_Displacement_S[j].Split(new char[] { ',' });
                        for (int i = 0; i < test9.Length; i++)
                        {


                        }


                        dispx = double.Parse(test9[1]);
                        dispy = double.Parse(test9[2]);


                    }
                }
            }
            catch { }

            if (dispx != 0 | dispy != 0) bool_Support_Displacement = true;

            return bool_Support_Displacement;
        }


        public static int FIND_MEM_INC_FACTOR(int Mem_Id, string DirY)
        {
            int fac = 1;
            if (Math.Cos(Theta(Mem_Id) * Math.PI / 180) >= 0 & (DirY != "GY")) fac = 1;
            if (Math.Cos(Theta(Mem_Id) * Math.PI / 180) < 0 & (DirY != "GY")) fac = -1;
            return fac;
        }

        public bool FIND_MEM_PROPETY_NULL()
        {
            bool bool_MEM_PROPETY_NULL = false;

            for (int k = 1; k <= totalmember; k++)
            {
                if (string.IsNullOrEmpty(Mproperty_A_I_E[k]))
                {
                    bool_MEM_PROPETY_NULL = true;

                }

                //Debug.Print("k, Mproperty_A_I_E[k]= " + k + " , " + Mproperty_A_I_E[k]);

            }



            return bool_MEM_PROPETY_NULL;
        }










        public static bool Find_All_Zero_Ploady_Sring(string Ploady)
        {
            bool bool_Find_All_Zero_Ploady_AS_Sring = true;



            if (!string.IsNullOrEmpty(Ploady))
            {
                string[] test = Ploady.Split(new char[] { ',' });
                for (int j = 1; j < test.Length - 1; j++)
                {

                    test[j] = test[j].Replace(" ", "");
                }




                for (int i = 1; i <= 12; i++)
                {
                    if (test[i] != "0")
                    {
                        bool_Find_All_Zero_Ploady_AS_Sring = false;
                    }
                }
            }
            return bool_Find_All_Zero_Ploady_AS_Sring;
        }

        public static string Find_k_Index_Ploady_Sring(string Ploady, int k)
        {

            string[] test = new string[] { };



            if (!string.IsNullOrEmpty(Ploady))
            {
                test = Ploady.Split(new char[] { ',' });

                {

                    test[k] = test[k].Replace(" ", "");
                }





            }
            return test[k];
        }


        public static string Find_REPLACE_k_Index_Ploady_Sring(string Ploady, int k, string s)
        {
            string result = null;




            if (!string.IsNullOrEmpty(Ploady))
            {
                string[] test = Ploady.Split(new char[] { ',' });



                test[k] = s;
                result = string.Join(",", test);





            }
            return result;
        }




        private bool Find_Has_INCLINED_Support_SingleMem_withParallelBase(int nodenumz, int midx)
        {
            bool bool_has_7 = false;
            Array.Resize(ref suptype, 50 + 1);
            Array.Resize(ref Sup_Node_Number, totalnode + 1);
            int k = nodenumz;

            if (!string.IsNullOrEmpty(Sup_Node_Number_Type[k]))
            {

                string[] testx = Sup_Node_Number_Type[k].Split(new char[] { ',' });
                for (int i = 0; i < testx.Length; i++)
                {


                }
                suptype[k] = int.Parse(testx[1]);

                try
                {
                    suptype[k] = int.Parse(testx[1]);
                    if (suptype[k] == 7)
                    {
                        Inclied_Supp_Node_num = int.Parse(testx[0]);

                        Inc_Supp_angle_base_AND_GX = double.Parse(testx[2]);
                        if (double.Parse(Theta(midx).ToString("0.00")) - Inc_Supp_angle_base_AND_GX <= 0.2)
                        {
                            bool_has_7 = true;

                        }



                    }
                }
                catch (Exception ex)
                {
                    Debug.Print(k + "  Find_Has_INCLINED_Support_SingleMem_ParallelBase:" + ex.ToString());
                }

            }
            return bool_has_7;
        }



        private bool Find_Has_INCLINED_Support_ANY_Mem(int nodenumz)
        {
            bool bool_has_7 = false;
            Array.Resize(ref suptype, 50 + 1);
            Array.Resize(ref Sup_Node_Number, totalnode + 1);
            int k = nodenumz;
            try
            {
                if (!string.IsNullOrEmpty(Sup_Node_Number_Type[k]))
                {


                    string[] testx = Sup_Node_Number_Type[k].Split(new char[] { ',' });
                    for (int i = 0; i < testx.Length; i++)
                    {




                    }

                    suptype[k] = int.Parse(testx[1]);

                    if (suptype[k] == 7)
                    {
                        Inclied_Supp_Node_num = int.Parse(testx[0]);

                        bool_has_7 = true;




                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(k + "  Find_Has_INCLINED_Support_ANY_Mem:" + ex.ToString());
            }



            return bool_has_7;
        }



        public static bool Find_Has_Support(int Selected_Node_id, string[] StringSuppData)
        {

            bool bool_has_supp = false;
            int k = 0;


            for (int i = 1; i <= totalnode; i++)
            {
                if (!string.IsNullOrEmpty(StringSuppData[i]))
                {

                    string[] test = StringSuppData[i].Split(new char[] { ',' });

                    k = int.Parse(test[0]);

                }
                if (k == Selected_Node_id)
                {
                    bool_has_supp = true;
                }

            }
            return bool_has_supp;
        }




        private int[] Node_number_to_connectivity(int nn)
        {
            int[] aa = new int[3];
            for (int i = 1; i <= totalmember; i++)
            {
                if (Connectivity[i, 1] == nn)
                {
                    aa[1] = i;
                    aa[2] = 1;

                }
                if (Connectivity[i, 2] == nn)
                {
                    aa[1] = i;
                    aa[2] = 2;
                }
            }

            return aa;

        }





        private bool Find_All_Zero_Double_1Darray(double[] array1D)
        {
            for (int i = 0; i < array1D.Length; i++)
            {
                if (array1D[i] != 0)
                {
                    return false;
                }
            }

            return true;
        }







        #region 0777 Invoke




        public static void Invoke_Unselect()
        {
            PictureBox PictureBox1 = new PictureBox();

            PointF p = new PointF(-10, -10);
            ptSelln = p;
            ptSelNode = p;

            PictureBox1.Invalidate();
        }

        public static void Invoke_draw_Mem_load()
        {
            PictureBox PictureBox1 = new PictureBox();



            PictureBox1.Invalidate();
        }
        private void Invoke_FormLoad_BeforeEvent()
        {



            if (mLines.Count > 0 & Last_index_line_selected > -1)
            {



                Graphics g = PictureBox1.CreateGraphics();
                draw_Selected_Member(mLines, g);


            }

            if (mLines.Count > 0 & nodenumer_Selected > -1)
            {


                // Debug.Print("  RRRRRRRRRRRRRRRRRRRRRR =nodenumer_Selected " + nodenumer_Selected);
            }



        }

        private void Invoke_Paint()
        {
            if (bool_FShowTable == true & Last_index_line_selected > -1)
            {
                Invoke_FormLoad();
            }
            if (bool_HINGE_Intermediate == true & Last_index_line_selected > -1)
            {
                Invoke_FormLoad();
            }
            if (bool_FconcP == true & Last_index_line_selected > -1)
            {
                Invoke_FormLoad();
            }
            if (bool_FconcM == true & Last_index_line_selected > -1)
            {
                Invoke_FormLoad();
            }
            if (bool_FUDL == true & Last_index_line_selected > -1)
            {
                Invoke_FormLoad();
            }



            if (bool_FNodal == true & nodenumer_Selected > -1)
            {
                Invoke_FormLoad();
            }
            if (bool_FSupDisp == true & nodenumer_Selected > -1)
            {

                Invoke_FormLoad();


            }
            if (bool_FSuppData == true & nodenumer_Selected > -1)
            {
                Invoke_FormLoad();
            }

            if (bool_FBeamProp == true & Last_index_line_selected > -1)
            {
                Invoke_FormLoad();
            }
            if (bool_FDirectSupp == true & nodenumer_Selected > -1)
            {
                Invoke_FormLoad();
            }




        }

        private void Invoke_Comma_Separated_String_In_Connectivity()
        {


            Array.Resize(ref Pload2, totalmember + 1 + 55);

            Array.Resize(ref HINGE_String2, totalmember + 1 + 55);
            Array.Resize(ref PJoint, totalnode + 1);
            Array.Resize(ref Form1.PJoint2, totalnode + 1 + 55);
            Array.Resize(ref Sup_Node_Number_Type, totalnode + 1 + 55);
            Array.Resize(ref Sup_Node_Number_Type2, totalnode + 1 + 55);
            Array.Resize(ref Mproperty_A_I_E, totalmember + 1 + 55);
            Array.Resize(ref Support_Displacement_S, totalnode + 1);
        }

        private void Invoke_FormLoad()
        {


            Invoke_FormLoad_BeforeEvent();

            if (bool_EDIT == true | bool_FBeamProp == true | bool_FShowTable == true)
            {

                if (mLines.Count > 0 & Last_index_line_selected > -1 & bool_FShowTable == true)
                {

                    FShowTable frm2 = new FShowTable();
                    frm2.ShowDialog();

                }
                if (mLines.Count > 0 & Last_index_line_selected > -1 & bool_HINGE_Intermediate == true)
                {

                    FHINGE frm2 = new FHINGE();
                    frm2.ShowDialog();

                }
                if (mLines.Count > 0 & Last_index_line_selected > -1 & bool_FconcP == true)
                {

                    FconcP frm2 = new FconcP();
                    frm2.ShowDialog();

                }
                if (mLines.Count > 0 & Last_index_line_selected > -1 & bool_FconcM == true)
                {
                    FconcM frm2 = new FconcM();
                    frm2.ShowDialog();
                }
                if (mLines.Count > 0 & Last_index_line_selected > -1 & bool_FUDL == true)
                {
                    FUDL frm2 = new FUDL();
                    frm2.ShowDialog();
                }


                if (mLines.Count > 0 & nodenumer_Selected > -1 & bool_FSupDisp == true)
                {

                    if (Find_Has_Support(nodenumer_Selected, Sup_Node_Number_Type2) == false)
                    {
                        PointF p = new PointF(-10, -10);
                        ptSelln = p;
                        ptSelNode = p;
                        nodenumer_Selected = -1;
                        selpt = false;


                    }
                    else
                    {
                        FSupDisp frm2 = new FSupDisp();
                        frm2.ShowDialog();
                    }
                }
                if (mLines.Count > 0 & nodenumer_Selected > -1 & bool_FNodal == true)
                {
                    FNodal frm2 = new FNodal();
                    frm2.ShowDialog();
                }
                if (mLines.Count > 0 & nodenumer_Selected > -1 & bool_FSuppData == true)
                {
                    FSuppData frm2 = new FSuppData();
                    frm2.ShowDialog();
                }

                if (mLines.Count > 0 & Last_index_line_selected > -1 & bool_FBeamProp == true)
                {
                    FBeamProp frm2 = new FBeamProp();
                    frm2.ShowDialog();
                }
                if (mLines.Count > 0 & nodenumer_Selected > -1 & bool_FDirectSupp == true)
                {
                    FDirectSupp frm2 = new FDirectSupp();
                    frm2.ShowDialog();
                }





                bool_FconcP = false;
                bool_FconcM = false;
                bool_FSupDisp = false;
                bool_FUDL = false;
                bool_FNodal = false;
                bool_FSuppData = false;
                bool_FDelSupp = false;
                bool_FBeamProp = false;
                bool_HINGE_Intermediate = false;
                bool_FShowTable = false;
                bool_FDirectSupp = false;

            }




        }

        private void Invoke_Edit_False()
        {
            if (bool_EDIT == false)
            {


                Form1.Last_index_line_selected = -1;
                Form1.nodenumer_Selected = -1;
                Form1.selln = false;
                selpt = false;

                Form1.Invoke_Unselect();

            }
        }






        #endregion 0777 Invoke






        #region 333


        private void Draw_Force_Mem_Pconc_UDL_MCon(int Memb_id, string StringPload, Graphics g)
        {

            double L;

            int kk;

            double Pcn_1 = 0;
            double Xpcn_1 = 0;
            double Pcn_2 = 0;
            double Xpcn_2 = 0;
            double Pcn_3 = 0;
            double Xpcn_3 = 0;
            double W0_1 = 0;
            double W1_1 = 0;
            double Xw0_1 = 0;
            double Xw1_1 = 0;
            double MCn_1 = 0;
            double XMcn_1 = 0;
            string Ydirection = null;

            double fracx;


            L = Form1.L_n(Memb_id);

            string[] test;

            try
            {

                if (!string.IsNullOrEmpty(StringPload))
                {

                    test = StringPload.Split(new char[] { ',' });
                    for (int i = 0; i < test.Length; i++)
                    {


                    }


                    kk = int.Parse(test[0]);

                    Pcn_1 = -double.Parse(test[1]);
                    Xpcn_1 = double.Parse(test[2]);
                    Pcn_2 = -double.Parse(test[3]);
                    Xpcn_2 = double.Parse(test[4]);
                    Pcn_3 = -double.Parse(test[5]);
                    Xpcn_3 = double.Parse(test[6]);

                    W0_1 = -double.Parse(test[7]);
                    W1_1 = -double.Parse(test[8]);
                    Xw0_1 = double.Parse(test[9]);
                    Xw1_1 = double.Parse(test[10]);

                    MCn_1 = double.Parse(test[11]);
                    XMcn_1 = double.Parse(test[12]);

                    Ydirection = test[13];

                }



                if (Xpcn_1 < L && Xpcn_1 > 0)
                {

                    fracx = Xpcn_1 / L;
                    Draw_ForceArrow_Single_Memb(Memb_id, fracx, Pcn_1, Ydirection, g);
                }
                if (Xpcn_2 < L && Xpcn_2 > 0)
                {

                    fracx = Xpcn_2 / L;
                    Draw_ForceArrow_Single_Memb(Memb_id, fracx, Pcn_2, Ydirection, g);

                }

                if (Xpcn_3 < L && Xpcn_3 > 0)
                {
                    fracx = Xpcn_3 / L;
                    Draw_ForceArrow_Single_Memb(Memb_id, fracx, Pcn_3, Ydirection, g);
                }

                if (Xw0_1 < Xw1_1 && Xw0_1 >= 0 && Xw1_1 <= L && Xw1_1 > Xw0_1)
                {

                    Draw_ForceArrow_UDL(Memb_id, W0_1, W1_1, Xw0_1, Xw1_1, Ydirection, g);
                }

                if (XMcn_1 < L && XMcn_1 > 0)
                {

                    fracx = XMcn_1 / L;
                    Draw_MOM_Single_Memb(Memb_id, fracx, MCn_1, g);

                }


                if (Xpcn_1 > L | Xpcn_2 > L | Xpcn_3 > L | Xw0_1 > L | Xw1_1 > L | XMcn_1 > L)
                {
                    MessageBox.Show("Distance of meber force from start point not within member length ");
                }



            }
            catch (Exception ex)
            {
                Debug.Print("Draw_Force_Mem_Pconc_UDL_MCon Exception caught.", ex);
            }

        }





        private void Draw_ForceArrow_Single_Memb(int Mem_ID, double frac_from_start, double Point_load_value, string dirY, Graphics g)
        {


            if (Math.Abs(Form1.cosx(Mem_ID)) == 0 | Math.Abs(Form1.sinx(Mem_ID)) == 0)
            {
                dirY = "LY";
            }
            double th1 = 0;
            if (dirY == "LY")
            {
                th1 = Form1.Theta(Mem_ID);
            }
            if (dirY == "GY")
            {
                th1 = 0;
            }

            double x;
            double y;
            double angrad;
            double angle_1 = (90 - th1 + 180);
            angrad = angle_1 / 180 * Math.PI;



            PointF ptstart;
            PointF ptend;
            PointF Pt_1;

            if (Point_load_value != 0)
            {


                ptstart = Form1.nodecoordinate[Form1.Connectivity[Mem_ID, 1]];
                ptend = Form1.nodecoordinate[Form1.Connectivity[Mem_ID, 2]];
                Pt_1 = Point_ON_Line_AT_Fraction(ptstart, frac_from_start, ptend);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                Pen myPen2 = new Pen(Color.Green, 0.0F);
                AdjustableArrowCap bigArrow = new AdjustableArrowCap(3, 3);

                myPen2.CustomStartCap = bigArrow;





                x = Pt_1.X;
                y = Pt_1.Y;
                double Point_load_value1 = 40 * Math.Abs(Point_load_value) / Point_load_value;

                PointF point2 = new PointF((float)(x + Math.Cos(angrad) * Point_load_value1), (float)(y + Math.Sin(angrad) * Point_load_value1));


                g.DrawLine(myPen2, Pt_1, point2);
                int fac = 1;
                if (Math.Cos(Form1.Theta(Mem_ID) * Math.PI / 180) >= 0 & (dirY != "GY")) fac = 1;
                if (Math.Cos(Form1.Theta(Mem_ID) * Math.PI / 180) < 0 & (dirY != "GY")) fac = -1;
                if (bool_View_Force_Value == true)
                {
                    Point_load_value = -1 * Point_load_value * fac;
                    g.DrawString((Point_load_value.ToString() + "kN"), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Green), point2.X, point2.Y, new StringFormat(StringFormatFlags.NoClip));
                }

            }

        }


        private void Draw_ForceArrow_UDL(int Mem_ID, double UDL_Start, double UDL_End, double dist_UDL_Start, double dist_UDL_End, string dirY, Graphics g)
        {

            if (Math.Abs(Form1.cosx(Mem_ID)) == 0 | Math.Abs(Form1.sinx(Mem_ID)) == 0)
            {
                dirY = "LY";
            }
            double th1 = 0;
            if (dirY == "LY")
            {
                th1 = Form1.Theta(Mem_ID);
            }
            if (dirY == "GY" & (Math.Abs(Form1.cosx(Mem_ID)) != 0 & Math.Abs(Form1.sinx(Mem_ID)) != 0))
            {
                th1 = 0;
            }

            double x;
            double y;
            double angrad;
            double angle_1;
            double L_1;
            double facUDL_Start;
            double facUDL_length;
            double Arrow_length;
            L_1 = Form1.L_n(Mem_ID);
            angle_1 = (90 - th1 + 180);
            angrad = angle_1 / 180 * Math.PI;


            facUDL_Start = dist_UDL_Start / L_1;
            facUDL_length = dist_UDL_End / L_1;

            PointF ptstart;
            PointF ptend;
            PointF Pt_1;
            PointF Pt_2;
            ptstart = Form1.nodecoordinate[Form1.Connectivity[Mem_ID, 1]];
            ptend = Form1.nodecoordinate[Form1.Connectivity[Mem_ID, 2]];
            Pt_1 = Point_ON_Line_AT_Fraction(ptstart, facUDL_Start, ptend);
            Pt_2 = Point_ON_Line_AT_Fraction(ptstart, facUDL_length, ptend);

            int fac = 1;
            if (Math.Cos(Form1.Theta(Mem_ID) * Math.PI / 180) >= 0 & (dirY != "GY")) fac = 1;
            if (Math.Cos(Form1.Theta(Mem_ID) * Math.PI / 180) < 0 & (dirY != "GY")) fac = -1;

            double W0_1;
            double W1_1;
            W0_1 = UDL_Start;
            W1_1 = UDL_End;
            if (Math.Abs(W0_1) > Math.Abs(W1_1) & W1_1 != 0)
            {
                W0_1 = 30 * Math.Abs(W0_1) / W0_1;
                W1_1 = 15 * Math.Abs(W1_1) / W1_1;
            }
            if (Math.Abs(W0_1) < Math.Abs(W1_1) & W0_1 != 0)
            {
                W0_1 = 15 * Math.Abs(W0_1) / W0_1;
                W1_1 = 30 * Math.Abs(W1_1) / W1_1;
            }
            if ((W0_1) == (W1_1))
            {
                W0_1 = 20 * Math.Abs(W0_1) / W0_1;
                W1_1 = 20 * Math.Abs(W1_1) / W1_1;
            }
            if (Math.Abs(W0_1) > Math.Abs(W1_1) & W1_1 == 0)
            {
                W0_1 = 30 * Math.Abs(W0_1) / W0_1;
                W1_1 = 0;
            }
            if (Math.Abs(W0_1) < Math.Abs(W1_1) & W0_1 == 0)
            {
                W0_1 = 0;
                W1_1 = 30 * Math.Abs(W1_1) / W1_1;
            }




            g.SmoothingMode = SmoothingMode.AntiAlias;



            Pen myPen2 = new Pen(Color.Red, 0.0F);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(3, 3);

            myPen2.CustomStartCap = bigArrow;

            x = Pt_1.X;
            y = Pt_1.Y;
            Arrow_length = W0_1;
            PointF point2_1 = new PointF((float)(x + Math.Cos(angrad) * Arrow_length), (float)(y + Math.Sin(angrad) * Arrow_length));
            if (bool_View_Force_Value == true)
            {
                UDL_Start = -1 * UDL_Start * fac;
                g.DrawString((UDL_Start.ToString() + "kN/m"), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), point2_1.X, point2_1.Y, new StringFormat(StringFormatFlags.NoClip));
            }
            x = Pt_2.X;
            y = Pt_2.Y;
            Arrow_length = W1_1;
            PointF point2_2 = new PointF((float)(x + Math.Cos(angrad) * Arrow_length), (float)(y + Math.Sin(angrad) * Arrow_length));


            if (bool_View_Force_Value == true)
            {
                UDL_End = -1 * UDL_End * fac;
                g.DrawString((UDL_End.ToString() + "kN/m"), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), point2_2.X, point2_2.Y, new StringFormat(StringFormatFlags.NoClip));
            }

            g.DrawLine(myPen2, Pt_1, point2_1);
            g.DrawLine(myPen2, Pt_2, point2_2);

            bigArrow = new AdjustableArrowCap(0, 0);

            myPen2.CustomStartCap = bigArrow;
            g.DrawLine(myPen2, point2_1, point2_2);




            try
            {




                bigArrow = new AdjustableArrowCap(4, 4);

                myPen2.CustomStartCap = bigArrow;

                int kk = 0;
                int inter_ordinate_No = 0;
                double frac1 = 0;
                double frac2 = 0;
                double inclined_trapizoid_Lenght = 0;

                inter_ordinate_No = 3;
                double UDL_Lenght = Math.Sqrt(Math.Pow(Pt_1.X - Pt_2.X, 2) + Math.Pow(Pt_1.Y - Pt_2.Y, 2));
                inclined_trapizoid_Lenght = Math.Sqrt(Math.Pow(point2_1.X - point2_2.X, 2) + Math.Pow(point2_1.Y - point2_2.Y, 2));

                PointF[] P_inter_on_memb = new PointF[51];
                PointF[] P_inter_above_memb = new PointF[51];


                frac1 = 1 / (double)(inter_ordinate_No + 1);
                frac2 = 1 / (double)(inter_ordinate_No + 1);



                for (kk = 0; kk <= inter_ordinate_No; kk++)
                {
                    P_inter_on_memb[kk] = Point_ON_Line_AT_Fraction(Pt_1, frac1 * kk, Pt_2);
                    P_inter_above_memb[kk] = Point_ON_Line_AT_Fraction(point2_1, frac2 * kk, point2_2);
                    g.DrawLine(myPen2, P_inter_on_memb[kk], P_inter_above_memb[kk]);

                }
                for (kk = 0; kk < inter_ordinate_No; kk++)
                {

                }







            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Debug.Print("Draw_ForceArrow_UDL ex " + ex.ToString());
            }


        }



        private void DrawLine_Arrow(float x1, float y1, float x2, float y2, Graphics g)
        {
            using (Pen linePen = new Pen(Color.Black, 1))
            {

                System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                gp.AddLine(0, 0, 2, -5);
                gp.AddLine(0, 0, -2, -5);
                gp.AddLine(-2, -5, 2, -5);

                linePen.CustomStartCap = new System.Drawing.Drawing2D.CustomLineCap(gp, null);
                g.DrawLine(linePen, x1, y1, x2, y2);
            }


        }


        private void DrawLinePoint1(Graphics g)
        {
            Pen blackPen = new Pen(Color.Black, 1);
            int x1 = 0;
            int x2 = 50;
            int y1 = 0;
            int y2 = 50;


            g.DrawLine(blackPen, x1, y1, x2, y2);

        }




        private void Drawbigline(Graphics g)
        {
            g.DrawLine(Pens.Black, 0, 0, 1800, 1500);
        }

        private void Draw_HINGE_old(string HINGE_StringData, Graphics g)
        {
            Array.Resize(ref HINGE_NodeID, totalnode + 1);
            int nodenum;

            try
            {
                if (!string.IsNullOrEmpty(HINGE_StringData))
                {

                    string[] test = HINGE_StringData.Split(new char[] { ',' });
                    for (int i = 0; i < test.Length; i++)
                    {


                    }
                    nodenum = int.Parse(test[0]);

                    HINGE_NodeID[nodenum] = int.Parse(test[0]);



                }


                for (int u = 1; u <= totalnode; u++)
                {
                    if (HINGE_NodeID[u] > 0)
                    {
                        g.DrawEllipse(new Pen(Color.Gray, 3.0F), nodecoordinate[HINGE_NodeID[u]].X, nodecoordinate[HINGE_NodeID[u]].Y - 5, 10, 10);

                    }
                }


            }

            catch (Exception ex)
            {
                Debug.Print("private void draw_HINGE( Graphics g):" + ex.ToString());
            }

        }


        private void Draw_sup(string StringSuppData, Graphics g)
        {

            Array.Resize(ref suptype, 50 + 1);
            Array.Resize(ref Sup_Node_Number, totalnode + 1);
            for (int k = 1; k <= totalnode; k++)
            {

                if (!string.IsNullOrEmpty(Sup_Node_Number_Type[k]))
                {

                    string[] testx = Sup_Node_Number_Type[k].Split(new char[] { ',' });
                    for (int i = 0; i < testx.Length; i++)
                    {


                    }
                    suptype[k] = int.Parse(testx[1]);

                    try
                    {
                        suptype[k] = int.Parse(testx[1]);
                        if (suptype[k] == 7)
                        {

                            Inclied_Supp_Node_num = int.Parse(testx[0]);

                            Inc_Supp_angle_base_AND_GX = double.Parse(testx[2]);





                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(k + "  draw_sup Inclied_Supp =:" + ex.ToString());
                    }

                }
            }

            Array.Resize(ref suptype, 50 + 1);
            Array.Resize(ref Sup_Node_Number, totalnode + 1);




            if (!string.IsNullOrEmpty(StringSuppData))
            {

                string[] test = StringSuppData.Split(new char[] { ',' });
                for (int i = 0; i < test.Length; i++)
                {


                }
                int nodenum = int.Parse(test[0]);
                Sup_Node_Number[nodenum] = int.Parse(test[0]);
                suptype[nodenum] = int.Parse(test[1]);
            }
            for (int u = 1; u <= totalnode; u++)
            {
                if (Sup_Node_Number[u] > 0)
                {
                    if (suptype[u] == 2)
                    {
                        draw_traiangle(nodecoordinate[Sup_Node_Number[u]].X, nodecoordinate[Sup_Node_Number[u]].Y, g);
                    }
                    if (suptype[u] == 1)
                    {
                        draw_rectangle(nodecoordinate[Sup_Node_Number[u]].X, nodecoordinate[Sup_Node_Number[u]].Y, g);
                    }

                    if (suptype[u] == 0)
                    {
                        g.DrawEllipse(Pens.Blue, nodecoordinate[Sup_Node_Number[u]].X, nodecoordinate[Sup_Node_Number[u]].Y, 0, 0);
                    }

                    if (suptype[u] == 3)
                    {
                        draw_triangle_Roller_support(nodecoordinate[Sup_Node_Number[u]].X, nodecoordinate[Sup_Node_Number[u]].Y, 0, g);

                    }
                    if (suptype[u] == 5)
                    {

                        draw_tSupp_Guided_vertical(nodecoordinate[Sup_Node_Number[u]].X, nodecoordinate[Sup_Node_Number[u]].Y, g);

                    }

                    if (suptype[u] == 6)
                    {

                        draw_tSupp_Guided_vertical(nodecoordinate[Sup_Node_Number[u]].X, nodecoordinate[Sup_Node_Number[u]].Y, g);
                        draw_tSupp_SRING_Vert(nodecoordinate[Sup_Node_Number[u]].X, nodecoordinate[Sup_Node_Number[u]].Y + 15, g);

                    }

                    if (suptype[u] == 7)
                    {

                        draw_triangle_Roller_support(nodecoordinate[Sup_Node_Number[u]].X, nodecoordinate[Sup_Node_Number[u]].Y, Inc_Supp_angle_base_AND_GX, g);

                    }



                }

            }

        }










        public static void DrawArrow(Pen myPen, float x1, float y1, float x2, float y2, int arrLength, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            double degree = Math.Atan((y2 - y1) / (x2 - x1));


            double toLeft = 0;
            if (x2 > x1) toLeft = 0;
            if (x2 < x1) toLeft = Math.PI;
            if (x2 == x1) toLeft = 0;

            double degree1 = degree + 5 * Math.PI / 6 + toLeft;
            double degree2 = degree + 7 * Math.PI / 6 + toLeft;

            double px1 = x2 + Math.Cos(degree1) * arrLength;
            double py1 = y2 + Math.Sin(degree1) * arrLength;

            double px2 = x2 + Math.Cos(degree2) * arrLength;
            double py2 = y2 + Math.Sin(degree2) * arrLength;

            g.DrawLine(myPen, x1, y1, x2, y2);
            g.DrawLine(myPen, x2, y2, (float)px1, (float)py1);
            g.DrawLine(myPen, x2, y2, (float)px2, (float)py2);
        }


        private void Draw_HINGE(string HINGE_StringData, Graphics g)
        {
            int nodenum = 0;
            int memnum = 0;
            int memIndx = 0;

            string[] test = new string[] { };
            try
            {
                if (!string.IsNullOrEmpty(HINGE_StringData))
                {

                    test = HINGE_StringData.Split(new char[] { ',' });
                    for (int i = 0; i < test.Length; i++)
                    {


                    }

                    nodenum = int.Parse(test[0]);
                    memnum = int.Parse(test[1]);
                    memIndx = int.Parse(test[2]);

                }


                PointF ptstart;
                PointF ptend;
                PointF Pt_1 = new PointF(0, 0);


                float r = 2.5f;


                ptstart = Form1.nodecoordinate[Form1.Connectivity[memnum, 1]];
                ptend = Form1.nodecoordinate[Form1.Connectivity[memnum, 2]];

                if (memIndx == 1)
                {
                    Pt_1 = Point_ON_Line_AT_Fraction(ptstart, 0.1, ptend);
                }
                if (memIndx == 2)
                {
                    Pt_1 = Point_ON_Line_AT_Fraction(ptstart, 0.9, ptend);
                }
                if (memnum > 0 & memIndx > 0)
                {
                    g.DrawEllipse(new Pen(Color.Black, 3.0F), Pt_1.X - r, Pt_1.Y - r, 2 * r, 2 * r);

                }



                if (memnum == 0 & memIndx == 0)
                {

                    g.DrawEllipse(new Pen(Color.Black, 3.0F), Pt_1.X, Pt_1.Y - 5, 0, 0);
                }


            }




            catch (Exception ex)
            {
                Debug.Print("private void draw_HINGE( Graphics g):" + ex.ToString());
            }

        }





        private void draw_tDOF(int Nodenum, Graphics g)
        {
            float x = nodecoordinate[Nodenum].X;
            float y = nodecoordinate[Nodenum].Y;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen penx = new Pen(Color.Black, 0.0F);
            float s;
            s = 30;


            PointF p = new PointF(x, y);
            PointF p1 = new PointF((float)(x + s), (float)(y));
            PointF p2 = new PointF((float)(x), (float)(y - s));
            PointF p3 = new PointF(x, y);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen myPen2 = new Pen(Color.Green, 0.0F);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(6, 6, true);

            myPen2.CustomEndCap = bigArrow;

            Pen myPen22 = new Pen(Color.Gray, 0.0F);
            AdjustableArrowCap bigArrow22 = new AdjustableArrowCap(5, 5, false);

            myPen22.CustomEndCap = bigArrow22;




            DrawArrow(new Pen(Color.Green, 1.0F), p.X, p.Y, p1.X, p1.Y, 10, g);

            DrawArrow(new Pen(Color.Green, 1.0F), p.X, p.Y, p2.X, p2.Y, 10, g);
            Draw_arc_MOM_DOF(p, 30, g);

            {

                g.DrawString(((Nodenum * 3 - 2).ToString() + ""), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), p1.X, p1.Y, new StringFormat(StringFormatFlags.NoClip));
                g.DrawString(((Nodenum * 3 - 1).ToString() + ""), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), p2.X, p2.Y, new StringFormat(StringFormatFlags.NoClip));
                g.DrawString(((Nodenum * 3).ToString() + ""), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), p.X - 15, p.Y, new StringFormat(StringFormatFlags.NoClip));


            }



        }





        private void draw_tDOF_HINGE_AFTER_Read_Input_Data_Entered(int node_num, Graphics g)
        {

            int memnum = 0;
            int memIndx = 0;
            string[] testx = new string[] { };
            try
            {




                if (HINGE_Num_total > 0)
                {
                    if (!string.IsNullOrEmpty(HINGE_String[node_num]))
                    {

                        testx = HINGE_String[node_num].Split(new char[] { ',' });
                        for (int i = 0; i < testx.Length; i++)
                        {


                        }


                        memnum = int.Parse(testx[1]);
                        memIndx = int.Parse(testx[2]);

                        float x1 = nodecoordinate[node_num].X;
                        float y1 = nodecoordinate[node_num].Y;


                        PointF p = new PointF(x1 - 30, y1);
                        Draw_arc_MOM_DOF(p, 30, g);
                        g.DrawString(((HINGE_New_DOF[node_num]).ToString() + ""), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), p.X - 20, p.Y - 25, new StringFormat(StringFormatFlags.NoClip));
                    }
                }




            }
            catch (Exception ex)
            {
                Debug.Print("private void draw_tDOF_HINGE((int memnum, Graphics g), Graphics g) ex:" + ex.ToString());
            }

        }






        private void draw_tDOF_Old(int Nodenum, Graphics g)
        {
            float x = nodecoordinate[Nodenum].X;
            float y = nodecoordinate[Nodenum].Y;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen penx = new Pen(Color.Black, 0.0F);
            float s;
            s = 30;


            PointF p = new PointF(x, y);
            PointF p1 = new PointF((float)(x + s), (float)(y));
            PointF p2 = new PointF((float)(x), (float)(y - s));
            PointF p3 = new PointF(x, y);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen myPen2 = new Pen(Color.Green, 2.0F);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(3, 3);

            myPen2.CustomEndCap = bigArrow;








            g.DrawLine(myPen2, p, p1);
            g.DrawLine(myPen2, p, p2);
            Draw_arc_MOM(p, 30, g);

            {

                g.DrawString(((Nodenum * 3 - 2).ToString() + ""), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), p1.X, p1.Y, new StringFormat(StringFormatFlags.NoClip));
                g.DrawString(((Nodenum * 3 - 1).ToString() + ""), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), p2.X, p2.Y, new StringFormat(StringFormatFlags.NoClip));
                g.DrawString(((Nodenum * 3).ToString() + ""), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), p.X - 15, p.Y, new StringFormat(StringFormatFlags.NoClip));


            }



        }

        private void draw_Cursor(float x1, float y1, float rotate, Graphics g)
        {
            PointF pt = new PointF(x1, y1);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen penx = new Pen(Color.Gray, 0.0F);
            PointF[] px = new PointF[] 
                    { 
                        
                            new PointF { X = x1+0, Y = y1+12 }, 
                        new PointF { X = x1-8, Y = y1-2 },
                        new PointF { X = x1-3, Y = y1+0 },
                        new PointF { X = x1-3, Y = y1-8 }, 
                            new PointF { X = x1+3, Y = y1-8 }, 
                        new PointF { X = x1+3, Y = y1+0 },
                        new PointF { X = x1+8, Y = y1-2 }, 
                        
                                };

            if (rotate != 0)
            {
                GraphicsPath myPath = new GraphicsPath();
                GraphicsPath myPath1 = new GraphicsPath();
                GraphicsPath myPath2 = new GraphicsPath();
                GraphicsPath myPath3 = new GraphicsPath();
                Matrix translateMatrix = new Matrix();
                myPath.AddPolygon(px);


                translateMatrix.RotateAt((float)(-1 * rotate), pt);
                myPath.Transform(translateMatrix);
                g.DrawPath(new Pen(Color.Gray, 0.0F), myPath);

                myPath1.Transform(translateMatrix);
                g.DrawPath(new Pen(Color.Gray, 0.0F), myPath1);
            }

        }






        private void draw_Cursor_Node(float x1, float y1, float rotate, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen penx = new Pen(Color.Gray, 0.0F);
            float radius = 5;
            PointF[] px = new PointF[] 
                    { 
                        
                            new PointF { X = x1+0, Y = y1+12 }, 
                        new PointF { X = x1-8, Y = y1-2 },
                        new PointF { X = x1-3, Y = y1+0 },
                        new PointF { X = x1-3, Y = y1-8 }, 
                            new PointF { X = x1+3, Y = y1-8 }, 
                        new PointF { X = x1+3, Y = y1+0 },
                        new PointF { X = x1+8, Y = y1-2 }, 
                        
                                };

            if (rotate != 0)
            {
                GraphicsPath myPath = new GraphicsPath();
                GraphicsPath myPath1 = new GraphicsPath();
                GraphicsPath myPath2 = new GraphicsPath();
                GraphicsPath myPath3 = new GraphicsPath();
                Matrix translateMatrix = new Matrix();
                myPath.AddPolygon(px);
                myPath1.AddEllipse(x1 - radius, y1 + 12 - radius / 2, radius + radius, radius + radius);

                PointF pt = new PointF(x1, y1);


                translateMatrix.RotateAt((float)(-1 * rotate), pt);
                myPath.Transform(translateMatrix);
                g.DrawPath(new Pen(Color.Black, 3F), myPath);

                myPath1.Transform(translateMatrix);
                g.DrawPath(new Pen(Color.Black, 3F), myPath1);
            }

        }




        private void draw_traiangle(float x, float y, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen penx = new Pen(Color.Black, 0.0F);
            float s;
            s = 15;
            float radius = 4;
            double angle;
            double sinA;

            angle = Math.PI * 60 / 180;
            sinA = Math.Sin(angle);
            g.DrawEllipse(penx, x - radius, y,
                       radius + radius, radius + radius);
            x = x + 0;
            y = y + radius + radius;
            PointF p = new PointF((float)(x), (float)(y + s * sinA));
            PointF p2 = new PointF((float)(p.X - s / 2), (float)(p.Y));
            PointF p3 = new PointF((float)(p.X + s / 2), (float)(p.Y));
            PointF p1 = new PointF(x, y);
            PointF[] a = { p1, p2, p3 };
            g.DrawPolygon(penx, a);


            float rotate = 0;

            g.DrawLine(penx, p1, rotate_PointF(p2, p1, rotate));
            g.DrawLine(penx, rotate_PointF(p2, p1, rotate), rotate_PointF(p3, p1, rotate));
            g.DrawLine(penx, rotate_PointF(p3, p1, rotate), p1);




        }



        private void draw_rectangle(float x, float y, Graphics g)
        {
            Pen tpen = new Pen(System.Drawing.Color.Black, 0f);
            g.DrawRectangle(tpen, x - 15 / 2f, y, 15, 15);

        }


        private void draw_triangle_Pinned_Support(float x, float y, float rotate, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen penx = new Pen(Color.Black, 0.0F);
            float s;
            s = 15;
            double angle1;
            double sinA;

            angle1 = Math.PI * 60 / 180;
            sinA = Math.Sin(angle1);

            PointF p = new PointF((float)(x), (float)(y + s * sinA));
            PointF p2 = new PointF((float)(p.X - s / 2), (float)(p.Y));
            PointF p3 = new PointF((float)(p.X + s / 2), (float)(p.Y));
            PointF p1 = new PointF(x, y);
            PointF[] a = { p1, p2, p3 };
            g.DrawPolygon(penx, a);




            if (rotate > 0)
            {
                PointF[] aa = { p1, rotate_PointF(p2, p1, rotate), rotate_PointF(p3, p1, rotate) };
                g.DrawPolygon(penx, aa);
            }




        }






        private void draw_tSupp_Guided_vertical(float x, float y, Graphics g)
        {

            g.SmoothingMode = SmoothingMode.AntiAlias;
            double d = Math.PI * (180 + 30) / 180;
            float dy = 15;
            float radius = 4;
            float dia = radius * 2;
            Pen penx = new Pen(Color.Black, 0.0F);
            g.DrawLine(penx, x, y, x, y + dy);
            g.DrawLine(penx, x - radius * 2, y, x - radius * 2, y + dy);
            g.DrawLine(penx, x - dia, y, x - dia + 10 * (float)(Math.Cos(d)), y + 10 * (float)(Math.Sin(d)));
            g.DrawLine(penx, x - dia, y + 5, x - dia + 10 * (float)(Math.Cos(d)), y + 5 + 10 * (float)(Math.Sin(d)));
            g.DrawLine(penx, x - dia, y + 2 * 5, x - dia + 10 * (float)(Math.Cos(d)), y + 2 * 5 + 10 * (float)(Math.Sin(d)));
            g.DrawLine(penx, x - dia, y + 3 * 5, x - dia + 10 * (float)(Math.Cos(d)), y + 3 * 5 + 10 * (float)(Math.Sin(d)));





            g.DrawEllipse(penx, x - dia, y,
                       radius + radius, radius + radius);

            g.DrawEllipse(penx, x - dia, y + dy - radius * 2,
                      radius + radius, radius + radius);


        }

        private void draw_tSupp_Guided_horizontal(float x, float y, Graphics g)
        {

            g.SmoothingMode = SmoothingMode.AntiAlias;
            double d = Math.PI * (120) / 180;
            float dx = 15;
            float radius = 4;
            float dia = radius * 2;
            Pen penx = new Pen(Color.Black, 0.0F);
            g.DrawLine(penx, x, y, x + dx, y);

            g.DrawLine(penx, x, y + radius * 2, x + dx, y + radius * 2);

            g.DrawLine(penx, x, y + radius * 2, x + 10 * (float)(Math.Cos(d)), y + radius * 2 + 10 * (float)(Math.Sin(d)));
            g.DrawLine(penx, x + 5, y + radius * 2, x + 5 + 10 * (float)(Math.Cos(d)), y + radius * 2 + 10 * (float)(Math.Sin(d)));
            g.DrawLine(penx, x + 2 * 5, y + radius * 2, x + 2 * 5 + 10 * (float)(Math.Cos(d)), y + radius * 2 + 10 * (float)(Math.Sin(d)));
            g.DrawLine(penx, x + 3 * 5, y + radius * 2, x + 3 * 5 + 10 * (float)(Math.Cos(d)), y + radius * 2 + 10 * (float)(Math.Sin(d)));


            g.DrawEllipse(penx, x, y,
                       radius + radius, radius + radius);
            g.DrawEllipse(penx, x + dia, y,
                      radius + radius, radius + radius);




        }








        private void draw_triangle_Roller_support(float x, float y, double rotate, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen penx = new Pen(Color.Black, 0.0F);
            float s;
            s = 15; float radius = 4;
            double angle;
            double sinA;

            angle = Math.PI * 60 / 180;
            sinA = Math.Sin(angle);

            PointF p = new PointF((float)(x), (float)(y + s * sinA));
            PointF p2 = new PointF((float)(p.X - s / 2), (float)(p.Y));
            PointF p3 = new PointF((float)(p.X + s / 2), (float)(p.Y));
            PointF p1 = new PointF(x, y);


            PointF p22 = new PointF((p.X - s / 2), p.Y + radius + radius);
            PointF p33 = new PointF((p.X + s / 2), p.Y + radius + radius);
            PointF[] a = { p1, p2, p3 };

            if (rotate == 0)
            {
                g.DrawEllipse(penx, p.X - s / 2, p.Y,
                         radius + radius, radius + radius);

                g.DrawEllipse(penx, p.X + s / 2 - radius - radius, p.Y,
                          radius + radius, radius + radius);
                g.DrawPolygon(penx, a);
                g.DrawLine(penx, p22, p33);
            }



            if (rotate != 0)
            {
                GraphicsPath myPath = new GraphicsPath();
                GraphicsPath myPath1 = new GraphicsPath();
                GraphicsPath myPath2 = new GraphicsPath();
                GraphicsPath myPath3 = new GraphicsPath();
                Matrix translateMatrix = new Matrix();
                myPath.AddPolygon(a);
                myPath1.AddLine(p22, p33);
                myPath2.AddEllipse(p.X - s / 2, p.Y,
                         radius + radius, radius + radius);
                myPath3.AddEllipse(p.X + s / 2 - radius - radius, p.Y,
                          radius + radius, radius + radius);
                PointF pt = new PointF(x, y);

                translateMatrix.RotateAt((float)(-1 * rotate), pt);
                myPath.Transform(translateMatrix);
                g.DrawPath(new Pen(Color.Black, 0.0F), myPath);
                myPath1.Transform(translateMatrix);
                g.DrawPath(new Pen(Color.Black, 0.0F), myPath1);
                myPath2.Transform(translateMatrix);
                g.DrawPath(new Pen(Color.Black, 0.0F), myPath2);
                myPath3.Transform(translateMatrix);
                g.DrawPath(new Pen(Color.Black, 0.0F), myPath3);
            }

        }








        private void draw_tSupp_SRING_Vert(float x, float y, Graphics g)
        {

            g.SmoothingMode = SmoothingMode.AntiAlias;
            double d = Math.PI * (120) / 180;
            float radius = 4;

            float ln2 = 2.5f;
            float ln1 = 4f;

            float dia = radius * 2;
            Pen penx = new Pen(Color.Black, 0.0F);

            g.DrawLine(penx, x, y, x - radius, y + ln1);
            g.DrawLine(penx, x - radius, y + ln1, x + radius, y + ln1 + ln2);
            g.DrawLine(penx, x + radius, y + ln1 + ln2, x - radius, y + ln1 + ln2 + ln2);
            g.DrawLine(penx, x - radius, y + ln1 + ln2 + ln2, x + radius, y + ln1 + ln2 + ln2 + ln2);
            g.DrawLine(penx, x + radius, y + ln1 + ln2 + ln2 + ln2, x - radius, y + ln1 + ln2 + ln2 + ln2 + ln2);
            g.DrawLine(penx, x - radius, y + ln1 + ln2 + ln2 + ln2 + ln2, x, y + ln1 + ln2 + ln2 + ln2 + ln2 + ln1);
            g.DrawLine(penx, x, y + 18, x, y + 22);

            float y1 = y + 22;
            g.DrawLine(penx, x - 7.5f, y + 22, x + 7.5f, y + 22);

            g.DrawLine(penx, x - 7.5f, y1, x - 7.5f + 10 * (float)(Math.Cos(d)), y1 + 10 * (float)(Math.Sin(d)));
            g.DrawLine(penx, x - 7.5f + 5, y1, x - 7.5f + 5 + 10 * (float)(Math.Cos(d)), y1 + 10 * (float)(Math.Sin(d)));
            g.DrawLine(penx, x - 7.5f + 5 * 2, y1, x - 7.5f + 5 * 2 + 10 * (float)(Math.Cos(d)), y1 + 10 * (float)(Math.Sin(d)));
            g.DrawLine(penx, x - 7.5f + 5 * 3, y1, x - 7.5f + 5 * 3 + 10 * (float)(Math.Cos(d)), y1 + 10 * (float)(Math.Sin(d)));




        }






        private void draw_tSupp_SRING_Horz(float x, float y, Graphics g)
        {

            g.SmoothingMode = SmoothingMode.AntiAlias;
            double d = Math.PI * (-30) / 180;
            float radius = 4;

            float ln2 = 2.5f;
            float ln1 = 4f;

            float dia = radius * 2;
            Pen penx = new Pen(Color.Black, 0.0F);

            g.DrawLine(penx, x, y, x + ln1, y - radius);
            g.DrawLine(penx, x + ln1, y - radius, x + ln1 + ln2, y + radius);
            g.DrawLine(penx, x + ln1 + ln2, y + radius, x + ln1 + ln2 * 2, y - radius);
            g.DrawLine(penx, x + ln1 + ln2 * 2, y - radius, x + ln1 + ln2 * 3, y + radius);
            g.DrawLine(penx, x + ln1 + ln2 * 3, y + radius, x + ln1 + ln2 * 4, y - radius);
            g.DrawLine(penx, x + ln1 + ln2 * 4, y - radius, x + ln1 + ln2 * 4 + ln1, y);



            g.DrawLine(penx, x + 18, y, x + 22, y);


            g.DrawLine(penx, x + 22, y - 7.5f, x + 22, y + 7.5f);
            float y1 = y;
            float x1 = x + 22;
            g.DrawLine(penx, x1, y - 7.5f, x1 + 10 * (float)(Math.Cos(d)), y - 7.5f + 10 * (float)(Math.Sin(d)));
            g.DrawLine(penx, x1, y - 7.5f + 5, x1 + 10 * (float)(Math.Cos(d)), y - 7.5f + 5 + 10 * (float)(Math.Sin(d)));

            g.DrawLine(penx, x1, y - 7.5f + 5 * 2, x1 + 10 * (float)(Math.Cos(d)), y - 7.5f + 5 * 2 + 10 * (float)(Math.Sin(d)));
            g.DrawLine(penx, x1, y - 7.5f + 5 * 3, x1 + 10 * (float)(Math.Cos(d)), y - 7.5f + 5 * 3 + 10 * (float)(Math.Sin(d)));




        }

        public static int[] Find_Node_Mem_Num_AND_STARTorEND_From_nodecoordinate(PointF p)
        {



            int[] a = new int[totalmember + 1];
            int[] aa = new int[totalmember + 1];
            int[] bb = new int[5];
            int[] c = new int[5];
            int EndIndex = 0;
            for (int k = 1; k <= totalnode; k++)
            {

                for (int j = 0; j < mLines.Count; j++)
                {

                    if (mLines[j].StartPoint.X == p.X & mLines[j].StartPoint.Y == p.Y)
                    {

                        a[j + 1] = j + 1;
                        aa[j + 1] = 1;

                    }
                }
            }
            for (int k = 1; k <= totalnode; k++)
            {

                for (int j = 0; j < mLines.Count; j++)
                {

                    if (mLines[j].EndPoint.X == p.X & mLines[j].EndPoint.Y == p.Y)
                    {

                        a[j + 1] = j + 1;
                        aa[j + 1] = 2;

                    }
                }
            }
            int count = 0;
            for (int j = 0; j < mLines.Count; j++)
            {
                if (a[j + 1] > 0)
                {
                    count = count + 1;

                    c[3] = count;
                    c[1] = a[j + 1];
                    c[2] = aa[j + 1];
                }


            }
            for (int j = 0; j < mLines.Count; j++)
            {
                if (count == 1)
                {
                    EndIndex = aa[j + 1];
                    bb[0] = c[1];
                    bb[1] = c[2];
                    bb[2] = c[3];
                }
                else if (count > 1)
                {
                    bb[0] = c[1];
                    bb[1] = 555;
                    bb[2] = c[3];
                }
                else
                {
                    bb[0] = 0;
                    bb[1] = 0;
                    bb[2] = 0;
                }
            }
            return bb;
        }


        private PointF Point_ON_Line_AT_Fraction(PointF xy1, double fractionx, PointF xy2)
        {

            double Vx = 0;
            double Vy = 0;
            double d_1 = 0;
            double t = 0;


            Vx = xy2.X - xy1.X;
            Vy = xy2.Y - xy1.Y;
            d_1 = Math.Sqrt(Math.Pow(Vx, 2) + Math.Pow(Vy, 2));


            t = fractionx;

            PointF P_2 = new PointF((float)(((1 - t) * xy1.X + t * xy2.X)), (float)((1 - t) * xy1.Y + t * xy2.Y));


            return P_2;
        }





        private void Draw_MOM_Single_Memb(int Mem_ID, double frac_from_start, double MOM_value, Graphics g)
        {




            PointF ptstart;
            PointF ptend;
            PointF Pt_1;

            if (MOM_value != 0)
            {


                ptstart = Form1.nodecoordinate[Form1.Connectivity[Mem_ID, 1]];
                ptend = Form1.nodecoordinate[Form1.Connectivity[Mem_ID, 2]];
                Pt_1 = Point_ON_Line_AT_Fraction(ptstart, frac_from_start, ptend);




                Pen myPen2 = new Pen(Color.Green, 1);


                Draw_arc_MOM_DOF(Pt_1, MOM_value, g);
                if (bool_View_Force_Value == true)
                {
                    g.DrawString((MOM_value.ToString() + "kN-m"), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), Pt_1.X, Pt_1.Y, new StringFormat(StringFormatFlags.NoClip));
                }

            }

        }


        private void Draw_arc_MOM_DOF(PointF ptx, double Mom_Conx, Graphics gr)
        {


            if (Mom_Conx < 1)
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;

                SolidBrush redBrush = new SolidBrush(Color.Green);


                using (GraphicsPath start_path = new GraphicsPath())
                {
                    start_path.AddArc(-14, 0, 18, 18, 180, 180);



                    using (CustomLineCap start_cap = new CustomLineCap(null, start_path))
                    {


                        using (GraphicsPath end_path = new GraphicsPath())
                        {
                            end_path.AddLine(0, 0, -15, -15);
                            end_path.AddLine(0, 0, 15, -15);



                            using (CustomLineCap end_cap = new CustomLineCap(null, end_path))
                            {


                                using (Pen the_pen = new Pen(Color.Gray, 0.5F))
                                {
                                    the_pen.CustomStartCap = start_cap;
                                    the_pen.CustomEndCap = end_cap;

                                    the_pen.Color = Color.Gray;

                                    Rectangle rect1 = new Rectangle((int)ptx.X - 12, (int)ptx.Y - 12, 30, 30);

                                    gr.DrawArc(the_pen, rect1, 180, 210);



                                }
                            }
                        }
                    }
                }
            }



            if (Mom_Conx > 1)
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;

                SolidBrush redBrush = new SolidBrush(Color.Green);


                using (GraphicsPath start_path = new GraphicsPath())
                {

                    start_path.AddLine(0, 0, -15, -15);
                    start_path.AddLine(0, 0, 15, -15);


                    using (CustomLineCap start_cap = new CustomLineCap(null, start_path))
                    {


                        using (GraphicsPath end_path = new GraphicsPath())
                        {

                            end_path.AddArc(-14, 0, 18, 18, 180, 180);

                            using (CustomLineCap end_cap = new CustomLineCap(null, end_path))
                            {


                                using (Pen the_pen = new Pen(Color.Gray, 0.5F))
                                {
                                    the_pen.CustomStartCap = start_cap;
                                    the_pen.CustomEndCap = end_cap;

                                    the_pen.Color = Color.Gray;

                                    Rectangle rect1 = new Rectangle((int)ptx.X - 12, (int)ptx.Y - 12, 30, 30);

                                    gr.DrawArc(the_pen, rect1, 180, 210);


                                }
                            }
                        }
                    }
                }
            }


        }

        private void Draw_arc_MOM(PointF ptx, double Mom_Conx, Graphics gr)
        {



            if (Mom_Conx < 1)
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;

                SolidBrush redBrush = new SolidBrush(Color.Green);


                using (GraphicsPath start_path = new GraphicsPath())
                {
                    start_path.AddArc(-4, 0, 8, 8, 180, 180);



                    using (CustomLineCap start_cap = new CustomLineCap(null, start_path))
                    {


                        using (GraphicsPath end_path = new GraphicsPath())
                        {
                            end_path.AddLine(0, 0, -5, -5);
                            end_path.AddLine(0, 0, 5, -5);



                            using (CustomLineCap end_cap = new CustomLineCap(null, end_path))
                            {


                                using (Pen the_pen = new Pen(Color.Gray, 0.5F))
                                {
                                    the_pen.CustomStartCap = start_cap;
                                    the_pen.CustomEndCap = end_cap;

                                    the_pen.Color = Color.Gray;
                                    gr.DrawEllipse(Pens.Pink, ptx.X, ptx.Y, 2, 2);
                                    gr.FillEllipse(redBrush, ptx.X, ptx.Y, 2, 2);

                                    Rectangle rect1 = new Rectangle((int)ptx.X - 12, (int)ptx.Y - 12, 30, 30);

                                    gr.DrawArc(the_pen, rect1, 180, 210);



                                }
                            }
                        }
                    }
                }
            }



            if (Mom_Conx > 1)
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;

                SolidBrush redBrush = new SolidBrush(Color.Green);


                using (GraphicsPath start_path = new GraphicsPath())
                {

                    start_path.AddLine(0, 0, -5, -5);
                    start_path.AddLine(0, 0, 5, -5);


                    using (CustomLineCap start_cap = new CustomLineCap(null, start_path))
                    {


                        using (GraphicsPath end_path = new GraphicsPath())
                        {

                            end_path.AddArc(-4, 0, 8, 8, 180, 180);


                            using (CustomLineCap end_cap = new CustomLineCap(null, end_path))
                            {


                                using (Pen the_pen = new Pen(Color.Gray, 0.5F))
                                {
                                    the_pen.CustomStartCap = start_cap;
                                    the_pen.CustomEndCap = end_cap;

                                    the_pen.Color = Color.Gray;
                                    gr.DrawEllipse(Pens.Pink, ptx.X, ptx.Y, 2, 2);
                                    gr.FillEllipse(redBrush, ptx.X, ptx.Y, 2, 2);

                                    Rectangle rect1 = new Rectangle((int)ptx.X - 12, (int)ptx.Y - 12, 30, 30);

                                    gr.DrawArc(the_pen, rect1, 180, 210);


                                }
                            }
                        }
                    }
                }
            }


        }

        #endregion 333





        private void Draw_ForceArrow_Mom_ALL_Node(string PJointx, Graphics g)
        {

            int k = 0;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen myPen2 = new Pen(Color.Green, 0);
            AdjustableArrowCap bigArrow2 = new AdjustableArrowCap(3, 3);
            myPen2.CustomStartCap = bigArrow2;

            double[] ffjG;

            if (!string.IsNullOrEmpty(PJointx))
            {


                ffjG = Joint_Global_force_0_3(PJointx);
                k = Convert.ToInt32(ffjG[0]);
                for (int i = 0; i < ffjG.Length; i++)
                {

                }




                if (ffjG[1] > 0)
                {
                    g.DrawLine(myPen2, nodecoordinate[k].X, nodecoordinate[k].Y, nodecoordinate[k].X - 30, nodecoordinate[k].Y);
                    if (bool_View_Force_Value == true)
                    {
                        g.DrawString((ffjG[1].ToString() + "kN"), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), nodecoordinate[k].X - 30 - 10, nodecoordinate[k].Y, new StringFormat(StringFormatFlags.NoClip));
                    }
                }
                if (ffjG[1] < 0)
                {

                    g.DrawLine(myPen2, nodecoordinate[k].X - 30, nodecoordinate[k].Y, nodecoordinate[k].X, nodecoordinate[k].Y);
                    if (bool_View_Force_Value == true)
                    {
                        g.DrawString((ffjG[1].ToString() + "kN"), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), nodecoordinate[k].X - 30 - 10, nodecoordinate[k].Y, new StringFormat(StringFormatFlags.NoClip));

                    }
                }
                if (ffjG[2] > 0)
                {
                    g.DrawLine(myPen2, nodecoordinate[k].X, nodecoordinate[k].Y + 30, nodecoordinate[k].X, nodecoordinate[k].Y);
                    if (bool_View_Force_Value == true)
                    {

                        g.DrawString((ffjG[2].ToString() + "kN"), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), nodecoordinate[k].X, nodecoordinate[k].Y - 30, new StringFormat(StringFormatFlags.NoClip));
                    }
                }
                if (ffjG[2] < 0)
                {

                    g.DrawLine(myPen2, nodecoordinate[k].X, nodecoordinate[k].Y, nodecoordinate[k].X, nodecoordinate[k].Y - 30);
                    if (bool_View_Force_Value == true)
                    {
                        g.DrawString((ffjG[2].ToString() + "kN"), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), nodecoordinate[k].X, nodecoordinate[k].Y + 30, new StringFormat(StringFormatFlags.NoClip));
                    }
                }


                if (ffjG[3] != 1 && ffjG[3] != 0)
                {
                    Draw_arc_MOM_DOF(nodecoordinate[k], ffjG[3], g);

                    if (bool_View_Force_Value == true)
                    {
                        g.DrawString((ffjG[3].ToString() + "kNm"), new Font("Courier New", fontsize, FontStyle.Regular), new SolidBrush(Color.Magenta), nodecoordinate[k].X, nodecoordinate[k].Y, new StringFormat(StringFormatFlags.NoClip));
                    }
                }

            }

        }

        #region 0155 plot

        private void Button22_Click(System.Object sender, System.EventArgs e)
        {

            View_Shear = false;
            View_Mom = true;
            View_Theta = false;
            View_Delta = false;
            View_AxialForce = false;


            PictureBox1.Refresh();
        }

        private void Button33_Click(System.Object sender, System.EventArgs e)
        {

            View_Shear = false;
            View_Mom = false;
            View_Theta = true;
            View_Delta = false;
            View_AxialForce = false;


            PictureBox1.Refresh();

        }
        private void button44_Click(System.Object sender, System.EventArgs e)
        {

            View_Shear = false;
            View_Mom = false;
            View_Theta = false;
            View_Delta = true;
            View_AxialForce = false;


            PictureBox1.Refresh();

        }

        private void Button55_Click(System.Object sender, System.EventArgs e)
        {



            View_Shear = false;
            View_Mom = false;
            View_Theta = false;
            View_Delta = false;
            View_AxialForce = true;
            PictureBox1.Refresh();
        }

        public double[,] BMDX2(int Mem_ID)// draw3 disp
        {



            int segment = 120;//segment2;

            double[] Vx = new double[segment + 1], Mx = new double[segment + 1], Y1x = new double[segment + 1], Yx = new double[segment + 1], AFx = new double[segment + 1];
            double[,] BMD = new double[6, segment + 1];

            int Midx = Mem_ID;

            //BMD_ALL = new double[totalmember+1,6, segment + 1]; //delete all

            double[] Flocal = new double[6];
            double Ma = 0;
            double Va = 0;
            double Mb = 0;
            double Vb = 0;
            double ThetaA = 0;
            double ThetaB = 0;
            double Ya = 0;
            double Yb = 0;
            double X = 0;
            double L = 0;





            int i = 0;
            int j = 0;

            double Pcn_1 = 0;
            double Xpcn_1 = 0;
            double Pcn_2 = 0;
            double Xpcn_2 = 0;
            double Pcn_3 = 0;
            double Xpcn_3 = 0;
            double W0_1 = 0;
            double W1_1 = 0;
            double Xw0_1 = 0;
            double Xw1_1 = 0;
            double MCn_1 = 0;
            double XMcn_1 = 0;
            string Ydirection = null;



            string St_memLoad = Pload[Midx];
            if (!string.IsNullOrEmpty(St_memLoad))
            {

                string[] test = new string[] { };




                test = St_memLoad.Split(new char[] { ',' });
                for (i = 0; i < test.Length; i++)
                {

                }



                Pcn_1 = -double.Parse(test[1]);
                Xpcn_1 = double.Parse(test[2]);
                Pcn_2 = -double.Parse(test[3]);
                Xpcn_2 = double.Parse(test[4]);
                Pcn_3 = -double.Parse(test[5]);
                Xpcn_3 = double.Parse(test[6]);

                W0_1 = -double.Parse(test[7]);
                W1_1 = -double.Parse(test[8]);
                Xw0_1 = double.Parse(test[9]);
                Xw1_1 = double.Parse(test[10]);

                MCn_1 = -double.Parse(test[11]);
                XMcn_1 = double.Parse(test[12]);
                Ydirection = test[13];



            }






            if (Math.Abs(cosx(Mem_ID)) == 0 | Math.Abs(sinx(Mem_ID)) == 0)
            {
                Ydirection = "LY";
            }
            double EyI = 0;






            EyI = Iz[Midx] * Ey[Midx];

            L = L_n(Midx);



            j = Midx;



            Ya = Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[Midx, Connectivity[Midx, 1] * 3 - 1];
            ThetaA = Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[Midx, Connectivity[Midx, 1] * 3];

            Yb = Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[Midx, Connectivity[Midx, 2] * 3 - 1];
            ThetaB = Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[Midx, Connectivity[Midx, 2] * 3];



            Va = Result_Mem_Force_Local[j, Connectivity[j, 1] * 3 - 1];
            Ma = -Result_Mem_Force_Local[j, Connectivity[j, 1] * 3];

            Vb = -Result_Mem_Force_Local[j, Connectivity[j, 2] * 3 - 1];
            Mb = Result_Mem_Force_Local[j, Connectivity[j, 2] * 3];
            //Debug.Print("Result_Mem_Force_Local BMDX2...Vb = " + Vb + " Mb=" + Mb + Environment.NewLine);



            if (Ydirection == "GY")
            {

                W0_1 = W0_1 * cosx(Mem_ID);
                W1_1 = W1_1 * cosx(Mem_ID);
                Pcn_1 = Pcn_1 * cosx(Mem_ID);
                Pcn_2 = Pcn_2 * cosx(Mem_ID);
                Pcn_3 = Pcn_3 * cosx(Mem_ID);

            }

            bool bool_inc = false;
            bool bool_inc1 = Find_Has_INCLINED_Support_SingleMem_withParallelBase(Connectivity[Midx, 1], Mem_ID);
            bool bool_inc2 = Find_Has_INCLINED_Support_SingleMem_withParallelBase(Connectivity[Midx, 2], Mem_ID);
            if (bool_inc1 == true | bool_inc2 == true) bool_inc = true;

            double t = 0;
            if (cosx(Mem_ID) == 0)
            { t = 1; }
            else if (cosx(Mem_ID) != 0)
            { t = sinx(Mem_ID) / cosx(Mem_ID); }

            try
            {

                for (i = 0; i <= segment; i++)
                {
                    X = i * L / segment;


                    double[] P_P1 = PConc_Fun(Pcn_1, Xpcn_1, Pcn_2, Xpcn_2, Pcn_3, Xpcn_3, X, EyI);
                    double[] M_M1 = MCon_Fun(MCn_1, XMcn_1, X, EyI);
                    double[] Trap_UDL1 = UDL_TRAP_W0_W1_Fun(W0_1, W1_1, Xw0_1, Xw1_1, X, EyI);



                    Vx[i] = P_P1[1] + M_M1[1] + Trap_UDL1[1] + Va * B_Sing(X, 0, 0) + Ma * B_Sing(X, 0, -1);

                    BMD[1, i] = Vx[i];
                    BMD_ALL[Mem_ID, 1, i] = Vx[i];



                    Mx[i] = P_P1[2] + M_M1[2] + Trap_UDL1[2] + Va * B_Sing(X, 0, 1) + Ma * B_Sing(X, 0, 0);
                    BMD[2, i] = Mx[i];
                    BMD_ALL[Mem_ID, 2, i] = -Mx[i];


                    Y1x[i] = ThetaA + 1 / EyI * (Va / 2 * B_Sing(X, 0, 2) + Ma * B_Sing(X, 0, 1)) + (P_P1[3] + M_M1[3] + Trap_UDL1[3]);
                    BMD[3, i] = Y1x[i];
                    BMD_ALL[Mem_ID, 3, i] = Y1x[i];

                    Yx[i] = Ya + ThetaA * B_Sing(X, 0, 1) + 1 / EyI * (Va / 6 * B_Sing(X, 0, 3) + Ma / 2 * B_Sing(X, 0, 2)) + (P_P1[4] + M_M1[4] + Trap_UDL1[4]);
                    BMD[4, i] = Yx[i];
                    BMD_ALL[Mem_ID, 4, i] = Yx[i];


                    AFx[0] = -Result_Mem_Force_Local[Midx, Connectivity[Midx, 1] * 3 - 2];

                    AFx[segment] = Result_Mem_Force_Local[Midx, Connectivity[Midx, 2] * 3 - 2];

                    double AFdelta = AFx[0] - AFx[segment];
                    AFx[i] = AFx[0] + AFdelta * i * L / segment;

                    BMD[5, i] = AFx[i];
                    BMD_ALL[Mem_ID, 5, i] = AFx[i];

                    if ((bool_inc == true) | (Ydirection == "GY" & Ydirection != "LY" & cosx(Mem_ID) != 0))
                    {
                        AFx[i] = AFx[0] - P_P1[1] * t - Trap_UDL1[1] * t;
                        BMD[5, i] = AFx[i];
                        BMD_ALL[Mem_ID, 5, i] = AFx[i];
                    }




                }
            }
            catch (Exception ex)
            {
                Debug.Print("public double[,] BMDX2(int Mem_ID) " + ex.ToString());
            }
            return BMD;



        }












        public double[,] BMDX3(int Mem_ID)// draw6 all except disp
        {


            int segment = 120;//segment3;


            double[] Vx3 = new double[segment + 1], Mx3 = new double[segment + 1], Y1x3 = new double[segment + 1], Yx3 = new double[segment + 1], AFx3 = new double[segment + 1];
            double[,] BMD3 = new double[6, segment + 1];
            //BMD_ALL3 = new double[totalmember + 1, 6, segment + 1];//delete all
            int Midx = Mem_ID;


            double[] Flocal = new double[6];
            double Ma = 0;
            double Va = 0;
            double Mb = 0;
            double Vb = 0;
            double ThetaA = 0;
            double ThetaB = 0;
            double Ya = 0;
            double Yb = 0;
            double X = 0;
            double L = 0;




            int i = 0;
            int j = 0;

            double Pcn_1 = 0;
            double Xpcn_1 = 0;
            double Pcn_2 = 0;
            double Xpcn_2 = 0;
            double Pcn_3 = 0;
            double Xpcn_3 = 0;
            double W0_1 = 0;
            double W1_1 = 0;
            double Xw0_1 = 0;
            double Xw1_1 = 0;
            double MCn_1 = 0;
            double XMcn_1 = 0;
            string Ydirection = null;



            string St_memLoad = Pload[Midx];
            if (!string.IsNullOrEmpty(St_memLoad))
            {

                string[] test = new string[] { };





                test = St_memLoad.Split(new char[] { ',' });
                for (i = 0; i < test.Length; i++)
                {

                }



                Pcn_1 = -double.Parse(test[1]);
                Xpcn_1 = double.Parse(test[2]);
                Pcn_2 = -double.Parse(test[3]);
                Xpcn_2 = double.Parse(test[4]);
                Pcn_3 = -double.Parse(test[5]);
                Xpcn_3 = double.Parse(test[6]);

                W0_1 = -double.Parse(test[7]);
                W1_1 = -double.Parse(test[8]);
                Xw0_1 = double.Parse(test[9]);
                Xw1_1 = double.Parse(test[10]);

                MCn_1 = -double.Parse(test[11]);
                XMcn_1 = double.Parse(test[12]);
                Ydirection = test[13];




            }


            if (Math.Abs(cosx(Mem_ID)) == 0 | Math.Abs(sinx(Mem_ID)) == 0)
            {
                Ydirection = "LY";
            }
            double EyI = 0;






            EyI = Iz[Midx] * Ey[Midx];

            L = L_n(Midx);


            j = Midx;



            Ya = Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[Midx, Connectivity[Midx, 1] * 3 - 1];
            ThetaA = Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[Midx, Connectivity[Midx, 1] * 3];


            Yb = Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[Midx, Connectivity[Midx, 2] * 3 - 1];
            ThetaB = Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[Midx, Connectivity[Midx, 2] * 3];



            Va = Result_Mem_Force_Local[j, Connectivity[j, 1] * 3 - 1];
            Ma = -Result_Mem_Force_Local[j, Connectivity[j, 1] * 3];

            Vb = -Result_Mem_Force_Local[j, Connectivity[j, 2] * 3 - 1];
            Mb = Result_Mem_Force_Local[j, Connectivity[j, 2] * 3];


            if (Ydirection == "GY")
            {

                W0_1 = W0_1 * cosx(Mem_ID);
                W1_1 = W1_1 * cosx(Mem_ID);
                Pcn_1 = Pcn_1 * cosx(Mem_ID);
                Pcn_2 = Pcn_2 * cosx(Mem_ID);
                Pcn_3 = Pcn_3 * cosx(Mem_ID);

            }

            bool bool_inc = false;
            bool bool_inc1 = Find_Has_INCLINED_Support_SingleMem_withParallelBase(Connectivity[Midx, 1], Mem_ID);
            bool bool_inc2 = Find_Has_INCLINED_Support_SingleMem_withParallelBase(Connectivity[Midx, 2], Mem_ID);
            if (bool_inc1 == true | bool_inc2 == true) bool_inc = true;

            double t = 0;
            if (cosx(Mem_ID) == 0)
            { t = 1; }
            else if (cosx(Mem_ID) != 0)
            { t = sinx(Mem_ID) / cosx(Mem_ID); }

            try
            {

                for (i = 0; i <= segment; i++)
                {
                    X = i * L / segment;


                    double[] P_P1 = PConc_Fun(Pcn_1, Xpcn_1, Pcn_2, Xpcn_2, Pcn_3, Xpcn_3, X, EyI);
                    double[] M_M1 = MCon_Fun(MCn_1, XMcn_1, X, EyI);
                    double[] Trap_UDL1 = UDL_TRAP_W0_W1_Fun(W0_1, W1_1, Xw0_1, Xw1_1, X, EyI);


                    Vx3[i] = P_P1[1] + M_M1[1] + Trap_UDL1[1] + Va * B_Sing(X, 0, 0) + Ma * B_Sing(X, 0, -1);


                    BMD3[1, i] = Vx3[i];
                    BMD_ALL3[Mem_ID, 1, i] = Vx3[i];


                    Mx3[i] = P_P1[2] + M_M1[2] + Trap_UDL1[2] + Va * B_Sing(X, 0, 1) + Ma * B_Sing(X, 0, 0);
                    BMD3[2, i] = Mx3[i];
                    BMD_ALL3[Mem_ID, 2, i] = Mx3[i];

                    Y1x3[i] = ThetaA + 1 / EyI * (Va / 2 * B_Sing(X, 0, 2) + Ma * B_Sing(X, 0, 1)) + (P_P1[3] + M_M1[3] + Trap_UDL1[3]);
                    BMD3[3, i] = Y1x3[i];
                    BMD_ALL3[Mem_ID, 3, i] = Y1x3[i];



                    Yx3[i] = Ya + ThetaA * B_Sing(X, 0, 1) + 1 / EyI * (Va / 6 * B_Sing(X, 0, 3) + Ma / 2 * B_Sing(X, 0, 2)) + (P_P1[4] + M_M1[4] + Trap_UDL1[4]);
                    BMD3[4, i] = Yx3[i];
                    BMD_ALL3[Mem_ID, 4, i] = Yx3[i];


                    AFx3[0] = -Result_Mem_Force_Local[Midx, Connectivity[Midx, 1] * 3 - 2];



                    AFx3[segment] = Result_Mem_Force_Local[Midx, Connectivity[Midx, 2] * 3 - 2];



                    double AFdelta = AFx3[0] - AFx3[segment];
                    AFx3[i] = AFx3[0] + AFdelta * i * L / segment;
                    //////AFx3[i] = (AFx3[0] + AFx3[segment])/2; //+15-15=0

                    BMD3[5, i] = AFx3[i];
                    BMD_ALL3[Mem_ID, 5, i] = AFx3[i];

                    if ((bool_inc == true) | (Ydirection == "GY" & Ydirection != "LY" & cosx(Mem_ID) != 0))
                    {
                        AFx3[i] = AFx3[0] - P_P1[1] * t - Trap_UDL1[1] * t;
                        //Debug.Print(i + "," + Midx + "=P_P1[1] * t,Trap_UDL1[1]t, AFx3[i], BMDX3-bool_inc =" + P_P1[1] * t + "," + Trap_UDL1[1]*t + "," + AFx3[i] + "," + AFx3[0] + "," + AFx3[segment]);
                        BMD3[5, i] = AFx3[i];
                        BMD_ALL3[Mem_ID, 5, i] = AFx3[i];
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.Print("public double[,] BMDX3(int Mem_ID) " + ex.ToString());
            }
            return BMD3;



        }














        private void Generate_ordinate_values_2()
        {




            int segment = 120;//segment2;

            BMD_ALL = new double[totalmember + 1, 6, segment + 1];

            Relative_Disp = new double[totalmember + 1, segment + 1];


            Fact = new double[totalmember + 1, 6];




            VxY = new double[totalmember + 1, segment + 1];
            MxY = new double[totalmember + 1, segment + 1];
            Y1xY = new double[totalmember + 1, segment + 1];
            YxY = new double[totalmember + 1, segment + 1];
            AFY = new double[totalmember + 1, segment + 1];

            double[] ptxy11 = new double[totalmember + 1];
            double[] ptxy21 = new double[totalmember + 1];
            double[] ptxy31 = new double[totalmember + 1];
            double[] ptxy41 = new double[totalmember + 1];
            double[] ptxy51 = new double[totalmember + 1];

            try
            {

                for (int j = 1; j <= totalmember; j++)
                {


                    double[,] BMDZ2 = BMDX2(j);
                    for (int i = 0; i <= segment; i++)
                    {
                        VxY[j, i] = BMDZ2[1, i];
                        MxY[j, i] = -BMDZ2[2, i];
                        Y1xY[j, i] = BMDZ2[3, i];
                        YxY[j, i] = 1000 * BMDZ2[4, i];
                        AFY[j, i] = BMDZ2[5, i];

                    }





                    ptxy11[j] = FindMAX_Double_2D_Absolute(j, VxY);
                    ptxy21[j] = FindMAX_Double_2D_Absolute(j, MxY);
                    ptxy31[j] = FindMAX_Double_2D_Absolute(j, Y1xY);
                    ptxy41[j] = FindMAX_Double_2D_Absolute(j, YxY);
                    ptxy51[j] = FindMAX_Double_2D_Absolute(j, AFY);

                }


                double FY_MAX = FindMAX_Double_1D_Absolute(ptxy11);
                double MZ_MAX = FindMAX_Double_1D_Absolute(ptxy21);
                double THETA_MAX = FindMAX_Double_1D_Absolute(ptxy31);
                double DELTA_MAX = FindMAX_Double_1D_Absolute(ptxy41);
                double FX_MAX = FindMAX_Double_1D_Absolute(ptxy51);


                double[] res = new double[6];
                res[1] = FY_MAX;
                res[2] = MZ_MAX;
                res[3] = THETA_MAX;
                res[4] = DELTA_MAX;
                res[5] = FX_MAX;


                for (int j = 1; j <= totalmember; j++)
                {

                    Fact[j, 1] = sclae_value(res[1]);

                    Fact[j, 2] = sclae_value(res[2]);

                    Fact[j, 3] = sclae_value(res[3]);

                    if ((Math.Abs(Scale_Deflection()) > Math.Abs(res[4])) & (Scale_Deflection() != 0))
                    {
                        Fact[j, 4] = sclae_value(Scale_Deflection());

                        //Debug.Print(j + " =MemID if sclae_value(Scale_Deflection())is big Scale_Deflection,res[4] " + Scale_Deflection() + " , " + res[4] + sclae_value(Scale_Deflection()) + " , " + sclae_value(res[4]));
                    }

                    if ((Math.Abs(Scale_Deflection()) < Math.Abs(res[4])) & (Scale_Deflection() != 0))
                    {
                        Fact[j, 4] = sclae_value(res[4]);

                        //Debug.Print(j + " =MemID if sclae_value(res[4]) is big======Scale_Deflection, res[4] " + Scale_Deflection() + " , " + res[4] + " , " + sclae_value(Scale_Deflection()) + " , " + sclae_value(res[4]));
                    }

                    if ((Math.Abs(Scale_Deflection()) == Math.Abs(res[4])) & (Scale_Deflection() != 0))
                    {
                        Fact[j, 4] = sclae_value(res[4]);

                        //Debug.Print(j + " =MemID if(EQUAL),Scale_Deflection,res[4] " + Scale_Deflection() + " , " + res[4] + " , " + sclae_value(Scale_Deflection()) + " , " + sclae_value(res[4]));
                    }

                    if (Math.Abs(Scale_Deflection()) == 0)
                    {
                        Fact[j, 4] = sclae_value(res[4]);
                        //Debug.Print(j + " if(Scale_Deflection()==0) all Fixed supp,Scale_Deflection,res[4] " + Scale_Deflection() + " , " + res[4] + " , " + sclae_value(Scale_Deflection()) + " , " + sclae_value(res[4]));
                    }

                    Fact[j, 5] = sclae_value(res[5]);

                }

            }
            catch (Exception ex)
            {
                Debug.Print("Generate_ordinate_values_2()" + ex.ToString());
            }

        }




        private void Generate_ordinate_values_3()
        {


            int segment = 120;//segment3;



            Fact3 = new double[totalmember + 1, 6];


            double[] ptxy1 = new double[totalmember + 1];
            double[] ptxy2 = new double[totalmember + 1];
            double[] ptxy3 = new double[totalmember + 1];
            double[] ptxy4 = new double[totalmember + 1];
            double[] ptxy5 = new double[totalmember + 1];

            VxY3 = new double[totalmember + 1, segment + 1];
            MxY3 = new double[totalmember + 1, segment + 1];
            Y1xY3 = new double[totalmember + 1, segment + 1];
            YxY3 = new double[totalmember + 1, segment + 1];
            AFY3 = new double[totalmember + 1, segment + 1];

            try
            {


                for (int j = 1; j <= totalmember; j++)
                {
                    double[,] BMDZ3 = BMDX3(j);

                    //for (int i = 0; i <= segment; i++)
                    //{
                    //    VxY3[j, i] = BMD_ALL[j, 1, i];
                    //    MxY3[j, i] = -BMD_ALL[j, 2, i];
                    //    Y1xY3[j, i] = BMD_ALL[j, 3, i];
                    //    YxY3[j, i] = 1000 * BMD_ALL[j, 4, i];
                    //    AFY3[j, i] = BMD_ALL[j, 5, i];

                    //}

                    for (int i = 0; i <= segment; i++)
                    {
                        VxY3[j, i] = BMDZ3[1, i];
                        MxY3[j, i] = -BMDZ3[2, i];
                        Y1xY3[j, i] = BMDZ3[3, i];
                        YxY3[j, i] = 1000 * BMDZ3[4, i];
                        AFY3[j, i] = BMDZ3[5, i];

                    }


                    ptxy1[j] = FindMAX_Double_2D_Absolute(j, VxY3);
                    ptxy2[j] = FindMAX_Double_2D_Absolute(j, MxY3);
                    ptxy3[j] = FindMAX_Double_2D_Absolute(j, Y1xY3);
                    ptxy4[j] = FindMAX_Double_2D_Absolute(j, YxY3);
                    ptxy5[j] = FindMAX_Double_2D_Absolute(j, AFY3);

                }


                double FY_MAX3 = FindMAX_Double_1D_Absolute(ptxy1);
                double MZ_MAX3 = FindMAX_Double_1D_Absolute(ptxy2);
                double THETA_MAX3 = FindMAX_Double_1D_Absolute(ptxy3);
                double DELTA_MAX3 = FindMAX_Double_1D_Absolute(ptxy4);
                double FX_MAX3 = FindMAX_Double_1D_Absolute(ptxy5);
                double[] res = new double[6];
                res[1] = FY_MAX3;
                res[2] = MZ_MAX3;
                res[3] = THETA_MAX3;
                res[4] = DELTA_MAX3;
                res[5] = FX_MAX3;


                for (int j = 1; j <= totalmember; j++)
                {

                    Fact3[j, 1] = sclae_value(res[1]);

                    Fact3[j, 2] = sclae_value(res[2]);

                    Fact3[j, 3] = sclae_value(res[3]);

                    if ((Math.Abs(Scale_Deflection()) > Math.Abs(res[4])) & (Scale_Deflection() != 0))
                    {
                        Fact3[j, 4] = sclae_value(Scale_Deflection());

                        //Debug.Print(j + " =MemID if sclae_value(Scale_Deflection())is big Scale_Deflection,res[4] " + Scale_Deflection() + " , " + res[4] + sclae_value(Scale_Deflection()) + " , " + sclae_value(res[4]));
                    }

                    if ((Math.Abs(Scale_Deflection()) < Math.Abs(res[4])) & (Scale_Deflection() != 0))
                    {
                        Fact3[j, 4] = sclae_value(res[4]);

                        //Debug.Print(j + " =MemID if sclae_value(res[4]) is big======Scale_Deflection, res[4] " + Scale_Deflection() + " , " + res[4] + " , " + sclae_value(Scale_Deflection()) + " , " + sclae_value(res[4]));
                    }

                    if ((Math.Abs(Scale_Deflection()) == Math.Abs(res[4])) & (Scale_Deflection() != 0))
                    {
                        Fact3[j, 4] = sclae_value(res[4]);
                        //Debug.Print(j + " =MemID if(EQUAL),Scale_Deflection,res[4] " + Scale_Deflection() + " , " + res[4] + " , " + sclae_value(Scale_Deflection()) + " , " + sclae_value(res[4]));
                    }

                    if (Math.Abs(Scale_Deflection()) == 0)
                    {
                        Fact3[j, 4] = sclae_value(res[4]);
                        //Debug.Print(j + " if(Scale_Deflection()==0) all Fixed supp,Scale_Deflection,res[4] " + Scale_Deflection() + " , " + res[4] + " , " + sclae_value(Scale_Deflection()) + " , " + sclae_value(res[4]));
                    }

                    Fact3[j, 5] = sclae_value(res[5]);

                }


            }



            catch (Exception)
            {

                //Debug.Print("Generate_ordinate_values_3()    " + Environment.NewLine + ex.ToString());
            }

        }





        private double FindMAX_Double_1D_Absolute(double[] ptx)
        {

            int j = 0;
            int k = 0;

            int p = ptx.Length - 1;
            double[] ary = new double[p + 1];
            double[] pt = new double[p + 1];

            for (j = 0; j <= p; j++)
            {
                pt[j] = ptx[j];
            }



            for (j = 0; j <= p; j++)
            {
                for (k = 0; k < p; k++)
                {
                    if (Math.Abs(pt[k]) > Math.Abs(pt[k + 1]))
                    {
                        double temp1 = pt[k];
                        pt[k] = pt[k + 1];
                        pt[k + 1] = temp1;

                    }
                }
            }

            for (j = 0; j <= p; j++)
            {
                ary[j] = (pt[j]);
            }

            return ary[p];

        }


        private double FindMAX_Double_2D_Absolute(int Mem_ID, double[,] ptx)
        {




            int j = 0;
            int k = 0;

            int p = ptx.GetLength(1) - 1;
            double[] ary = new double[p + 1];
            double[] pt = new double[p + 1];

            for (j = 0; j <= p; j++)
            {
                pt[j] = ptx[Mem_ID, j];
            }



            for (j = 0; j <= p; j++)
            {
                for (k = 0; k < p; k++)
                {

                    if (Math.Abs((pt[k])) > Math.Abs((pt[k + 1])))
                    {
                        double temp1 = pt[k];
                        pt[k] = pt[k + 1];
                        pt[k + 1] = temp1;

                    }
                }
            }

            for (j = 0; j <= p; j++)
            {
                ary[j] = pt[j];
            }

            return ary[p];




        }



        private double FindMAX_Double_2D(int Mem_ID, double[,] ptx)
        {





            int j = 0;
            int k = 0;

            int p = ptx.GetLength(1) - 1;
            double[] ary = new double[p + 1];
            double[] pt = new double[p + 1];

            for (j = 0; j <= p; j++)
            {
                pt[j] = ptx[Mem_ID, j];
            }



            for (j = 0; j <= p; j++)
            {
                for (k = 0; k < p; k++)
                {
                    if (pt[k] > pt[k + 1])
                    {
                        double temp1 = pt[k];
                        pt[k] = pt[k + 1];
                        pt[k + 1] = temp1;

                    }
                }
            }

            for (j = 0; j <= p; j++)
            {
                ary[j] = pt[j];
            }

            return ary[p];




        }
        private double FindMIN_Double_2D(int Mem_ID, double[,] ptx)
        {


            int j = 0;
            int k = 0;

            int p = ptx.GetLength(1) - 1;
            double[] ary = new double[p + 1];
            double[] pt = new double[p + 1];

            for (j = 0; j <= p; j++)
            {
                pt[j] = ptx[Mem_ID, j];
            }




            for (j = 0; j <= p; j++)
            {
                for (k = 0; k < p; k++)
                {
                    if (pt[k] < pt[k + 1])
                    {
                        double temp1 = pt[k];
                        pt[k] = pt[k + 1];
                        pt[k + 1] = temp1;

                    }
                }
            }

            for (j = 0; j <= p; j++)
            {
                ary[j] = pt[j];
            }

            return ary[p];




        }
        private double sclae_value(double val)
        {
            double temp = 0;
            double temp2 = 0;
            temp = val;

            if (val != 0)
            {
                temp2 = 30 / temp;
            }

            if (val == 0)
            {
                temp2 = 0.0000000000000001;
            }


            return Math.Abs(temp2);

        }

        #endregion















        private PointF rotate_PointF(PointF a, PointF Pf_pivot, float angle)
        {


            PointF b = PointF.Empty;
            float x_pivot = Pf_pivot.X;
            float y_pivot = Pf_pivot.Y;

            float x_shifted = a.X - x_pivot;
            float y_shifted = a.Y - y_pivot;


            b.X = x_pivot + x_shifted * (float)Math.Cos(angle * (Math.PI) / 180) - y_shifted * (float)Math.Sin(angle * (Math.PI) / 180);
            b.Y = y_pivot + (x_shifted * (float)Math.Sin(angle * (Math.PI) / 180) + y_shifted * (float)Math.Cos(angle * (Math.PI) / 180));
            return b;


        }




        private PointF[] rotate_Array_PointF(PointF[] a, PointF Pf_pivot, float angle)
        {


            PointF[] b = new PointF[a.Length];
            float x_pivot = Pf_pivot.X;
            float y_pivot = Pf_pivot.Y;

            for (int i = 0; i < a.Length; i++)
            {
                float x_shifted = a[i].X - x_pivot;
                float y_shifted = a[i].Y - y_pivot;


                b[i].X = x_pivot + x_shifted * (float)Math.Cos(angle * (Math.PI) / 180) - y_shifted * (float)Math.Sin(angle * (Math.PI) / 180);
                b[i].Y = y_pivot + (x_shifted * (float)Math.Sin(angle * (Math.PI) / 180) + y_shifted * (float)Math.Cos(angle * (Math.PI) / 180));

            }
            return b;


        }




        private PointF Translate_PointF(PointF a, float tx, float ty)
        {
            PointF b = PointF.Empty;

            b.X = a.X - tx;
            b.Y = a.Y - ty;
            return b;

        }



        private bool validate_LINE_ON_LINE()
        {
            bool LINE_ON_LINE = false;
            double[] c = new double[101];
            double[] d = new double[101];
            double[] m1 = new double[101];

            for (int i = 0; i < m_Lines1.Count; i++)
            {
                PointF ptx = m_Lines1[i].StartPoint;
                for (int j = 0; j < m_Lines1.Count; j++)
                {
                    if (i != j)
                    {


                        if (m_Lines1[i].StartPoint.Y > m_Lines1[i].EndPoint.Y)
                            if (m_Lines1[i].StartPoint.Y >= ptx.Y && ptx.Y >= m_Lines1[i].EndPoint.Y)
                            {
                            }
                        if (m_Lines1[i].StartPoint.Y < m_Lines1[i].EndPoint.Y)
                            if (m_Lines1[i].StartPoint.Y <= ptx.Y && ptx.Y <= m_Lines1[i].EndPoint.Y)
                            {
                            }
                        m1[i] = (m_Lines1[i].StartPoint.Y - m_Lines1[i].EndPoint.Y) / (m_Lines1[i].StartPoint.X - m_Lines1[i].EndPoint.X);


                        c[i] = (m_Lines1[i].StartPoint.Y - m1[i] * m_Lines1[i].StartPoint.X);

                        d[i] = Math.Abs(ptx.Y - m1[i] * ptx.X - c[i]) / Math.Sqrt(m1[i] * m1[i] + 1);

                        {

                            PointF xy1 = PointF.Empty;
                            PointF xy2 = PointF.Empty;
                            double fractionx = 0;
                            double Vx = 0;
                            double Vy = 0;
                            double d_1 = 0;
                            double t;



                            Vx = xy2.X - xy1.X;
                            Vy = xy2.Y - xy1.Y;
                            d_1 = Math.Sqrt(Math.Pow(Vx, 2) + Math.Pow(Vy, 2));





                            t = fractionx;

                            PointF P_2 = new PointF((float)(((1 - t) * xy1.X + t * xy2.X)), (float)((1 - t) * xy1.Y + t * xy2.Y));


                        }


                    }
                }

            }

            return LINE_ON_LINE;

        }



        private double Scale_Deflection()
        {
            double[,] DispG = new double[totalmember + 1, totalnode * 3 + 1];
            for (int k = 1; k <= totalmember; k++)
            {
                DispG[k, Connectivity[k, 1] * 3 - 2] = Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 2];
                DispG[k, Connectivity[k, 1] * 3 - 1] = Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 1];
                DispG[k, Connectivity[k, 1] * 3] = 0;
                DispG[k, Connectivity[k, 2] * 3 - 2] = Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 2];
                DispG[k, Connectivity[k, 2] * 3 - 1] = Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 1];
                DispG[k, Connectivity[k, 2] * 3] = 0;
            }

            double[] dispMaxG = new double[totalmember + 1];
            for (int k = 1; k <= totalmember; k++)
            {
                dispMaxG[k] = FindMAX_Double_2D_Absolute(k, DispG);

            }
            double dispMax_ALLG = FindMAX_Double_1D_Absolute(dispMaxG);

            return dispMax_ALLG * 1000;

        }





        private string[] Number_Format(double Total)
        {
            string StTotal = "";
            StTotal = string.Format("{0:#.###E+00}", Total);


            string[] parts = StTotal.Split(new char[] { 'E' });

            parts[1] = "1E" + parts[1];

            return parts;
        }




        public static PointF[] sort_no_repeat_PointF(PointF[] pt)
        {
            int p = pt.Length - 1;
            PointF[] pt1 = new PointF[0];

            PointF temp1 = PointF.Empty;
            int j = 0;
            int k = 0;

            try
            {
                Array.Resize(ref pt, (2 * p) + 1);
                Array.Resize(ref pt1, (2 * p) + 1);


                for (j = 0; j <= p; j++)
                {
                    for (k = 1; k < p; k++)
                    {
                        if (pt[j] == pt[j + k])
                        {
                            temp1 = pt[j + 1];
                            pt[j + 1] = pt[j + k];
                            pt[j + k] = temp1;


                        }
                    }
                }



                for (k = 0; k <= p; k++)
                {
                    for (j = 0; j < p; j++)
                    {
                        temp1 = pt[j];

                        if (pt[j] == pt[j + 1])
                        {

                            pt[j] = new PointF();
                            pt[j + 1] = temp1;


                        }
                    }
                }


                k = 0;
                for (j = 0; j <= p; j++)
                {
                    if (pt[j] != new PointF())
                    {
                        pt1[k] = pt[j];
                        k = k + 1;
                    }

                }


            }
            catch (Exception ex)
            {
                Debug.Print("sort_no_repeat_PointF" + ex.ToString());
            }
            Array.Resize(ref pt1, k - 1 + 1);

            return pt1;

        }





        private string[] sort_no_repeat_string(string[] pt)
        {

            int p = pt.Length - 1;

            string[] pt1 = new string[pt.Length];
            string temp1;
            int j;
            int k;
            k = 0;


            try
            {



                for (j = 0; j <= p; j++)
                {
                    for (k = 1; k <= p - j; k++)
                    {
                        if (pt[j] == pt[j + k])
                        {
                            temp1 = pt[j + 1];
                            pt[j + 1] = pt[j + k];
                            pt[j + k] = temp1;


                        }
                    }
                }

                for (j = 0; j <= p - 1; j++)
                {

                    temp1 = pt[j];

                    if (pt[j] == pt[j + 1])
                    {

                        pt[j] = null;
                        pt[j + 1] = temp1;


                    }
                }



                k = 0;
                for (j = 0; j <= p; j++)
                {
                    if (pt[j] != null)
                    {
                        pt1[k] = pt[j];
                        k = k + 1;
                    }

                }

                Array.Resize(ref pt1, k - 1 + 1);
            }
            catch (Exception ex)
            {
                Debug.Print("private string[] sort_no_repeat_string" + ex.ToString());
            }
            return pt1;
        }




        private int[] sort_no_repeat_INT(int[] pt)
        {
            int p = pt.Length - 1;
            int[] pt1 = new int[pt.Length];
            string temp1 = string.Empty;
            int j = 0;
            int k = 0;
            int kk = 0;
            string[] First_Mat = new string[pt.Length];
            string[] Second_Mat = new string[pt.Length];
            for (j = 0; j < pt.Length; j++)
            {
                First_Mat[j] = pt[j].ToString();
            }


            try
            {

                for (j = 0; j <= p; j++)
                {
                    for (k = 1; k <= p - j; k++)
                    {
                        if (First_Mat[j] == First_Mat[j + k])
                        {
                            temp1 = First_Mat[j + 1];
                            First_Mat[j + 1] = First_Mat[j + k];
                            First_Mat[j + k] = temp1;


                        }
                    }
                }



                kk = 0;
                for (j = 0; j <= p - 1; j++)
                {

                    temp1 = First_Mat[j];

                    if (First_Mat[j] == First_Mat[j + 1])
                    {

                        First_Mat[j] = null;
                        First_Mat[j + 1] = temp1;


                    }
                }




                kk = 0;
                for (j = 0; j <= p; j++)
                {
                    if (First_Mat[j] != null)
                    {
                        Second_Mat[kk] = First_Mat[j];
                        pt1[kk] = Convert.ToInt32(Second_Mat[kk]);
                        kk = kk + 1;
                    }

                }


                Array.Resize(ref pt1, kk - 1 + 1);





            }
            catch (Exception ex)
            {
                Debug.Print("sort_no_repeat_INT" + ex.ToString());
            }
            return pt1;

        }



        private void button2_Click(object sender, EventArgs e)
        {


            if (drawln == true)
            {
                Read_Input_Data_Entered();
            }
            else
                if (!(string_Read == null || string_Read.Length == 0))
                {
                    READ_From_Given_Array_Of_Line_String_HINGE_L_Fact(string_Read);

                    READ_From_Given_Array_Of_Line_String(string_Read);

                }

            ZOOMPlus_bool = false;

            Draw_Model = true;
            View_Shear = false;
            View_Mom = false;
            View_Theta = false;
            View_Delta = false;
            View_AxialForce = false;


            PictureBox1.Invalidate();

        }







        private void Line_To_Node_AND_Connectivity2(List<line> mLinesy)
        {

            try
            {

                PointF[] all_points = new PointF[mLinesy.Count * 2 + 1];

                int m = 0;

                m = 0;


                for (int j = 0; j <= mLinesy.Count - 1; j++)
                {

                    all_points[m] = mLinesy[j].StartPoint;


                    m = m + 1;
                }


                for (int j = 0; j <= mLinesy.Count - 1; j++)
                {


                    all_points[m] = mLinesy[j].EndPoint;

                    m = m + 1;
                }


                nodeall = sort_no_repeat_PointF(all_points);
                totalmember = mLinesy.Count;
                totalnode = nodeall.Length;

                for (int i = 0; i < nodeall.Length; i++)
                {

                }

                if (nodeall.Length >= 2)
                {

                    nodecoordinate = new PointF[nodeall.Length + 1];
                    nodenumer = new int[nodeall.Length + 1];
                }
                for (int j = 0; j < nodeall.Length; j++)
                {
                    nodecoordinate[j + 1] = nodeall[j];
                    nodenumer[j + 1] = j + 1;

                }

                for (int j = 0; j < nodecoordinate.Length; j++)
                {


                }


                Connectivity = new int[totalmember + 1, 3];

                for (int k = 1; k < nodeall.Length; k++)
                {

                    for (int j = 0; j < mLinesy.Count; j++)
                    {

                        if (mLinesy[j].StartPoint == nodecoordinate[k])
                        {


                            Connectivity[j + 1, 1] = nodenumer[k];

                        }

                    }
                }


                for (int k = 1; k <= totalnode; k++)
                {

                    for (int j = 0; j < mLinesy.Count; j++)
                    {

                        if (mLinesy[j].EndPoint == nodecoordinate[k])
                        {
                            Connectivity[j + 1, 2] = nodenumer[k];

                        }

                    }
                }



                Invoke_Comma_Separated_String_In_Connectivity();

            }
            catch (Exception ex)
            {



                Debug.Print(" Line_To_Node_AND_Connectivity ex :" + Environment.NewLine + ex.ToString());
            }



        }











        private int[] Find_Array_Node_Num_From_Coordinate(PointF p)
        {


            int i;
            int k = 0;
            int[] mx = new int[10];
            for (i = 1; i <= totalnode; i++)
            {
                if (nodecoordinate[i].X == p.X && nodecoordinate[i].Y == p.Y)
                {

                    Array.Resize(ref mx, k + 1);
                    mx[k] = i;
                    k = k + 1;

                }

            }
            return mx;
        }



        private string[] Line_To_ArryLine_String(List<line> mLineW)
        {


            System.Text.StringBuilder result = new System.Text.StringBuilder();

            double xx1 = 0;
            double xx2 = 0;
            double yy1 = 0;
            double yy2 = 0;



            if (mLineW.Count > 0)
            {
                for (int i = 0; i < mLineW.Count; i++)
                {
                    xx1 = mLineW[i].StartPoint.X / GridX;
                    yy1 = mLineW[i].StartPoint.Y / GridX;
                    xx2 = mLineW[i].EndPoint.X / GridX;
                    yy2 = mLineW[i].EndPoint.Y / GridX;




                    result.Append("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                    result.AppendLine();
                }
            }


            string[] st99 = null;

            string stxx = result.ToString();

            st99 = stxx.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < st99.Length; i++)
            {
                st99[i] = st99[i].Replace(" ", "");


            }

            return st99;

        }




        private void Line_To_Node_AND_Connectivity(List<line> mLinesy)
        {

            try
            {

                PointF[] all_points = new PointF[mLinesy.Count * 2 + 1];

                int m = 0;

                m = 0;


                for (int j = 0; j <= mLinesy.Count - 1; j++)
                {

                    all_points[m] = mLinesy[j].StartPoint;


                    m = m + 1;
                }


                for (int j = 0; j <= mLinesy.Count - 1; j++)
                {


                    all_points[m] = mLinesy[j].EndPoint;


                    m = m + 1;
                }





                PointF[] nodeallx = sort_no_repeat_PointF(all_points);
                totalnode_From_Lines = nodeallx.Length;

                PointF[] nodeall = new PointF[nodeallx.Length + 50];

                nodeall[0] = mLinesy[0].StartPoint;
                nodeall[1] = mLinesy[0].EndPoint;

                m = 2;

                for (int j = 1; j <= mLinesy.Count - 1; j++)
                {

                    nodeall[m] = mLinesy[j].EndPoint;
                    m = m + 1;

                }



                totalmember = mLinesy.Count;
                totalnode = nodeall.Length;

                for (int i = 0; i < nodeall.Length; i++)
                {

                }
                if (nodeall.Length >= 2)
                {


                    nodecoordinate = new PointF[nodeall.Length + 1];
                    nodenumer = new int[nodeall.Length + 1];
                }






                for (int j = 0; j < nodeall.Length; j++)
                {
                    nodecoordinate[j + 1] = nodeall[j];
                    nodenumer[j + 1] = j + 1;

                }

                for (int j = 0; j < nodecoordinate.Length; j++)
                {


                }



                int[] exx = new int[10];
                for (int i = 0; i < mLines.Count; i++)
                {
                    exx = Find_Array_Node_Num_From_Coordinate(mLines[i].EndPoint);

                    if (Find_Array_Node_Num_From_Coordinate(mLines[i].EndPoint).Length > 1)
                    {
                        //Debug.Print(i + " = Find_Array_Node_Num_From_Coordinate(mLines[i].EndPoint)" + MakeDisplayable_1D_j_0_INT(exx));
                        nodeall[exx[1] - 1] = PointF.Empty;
                        nodecoordinate[exx[1]] = PointF.Empty;

                        for (int a = exx[1]; a < nodeall.Length - 1; a++)
                        {

                            nodeall[a - 1] = nodeall[a];
                            nodecoordinate[a] = nodecoordinate[a + 1];
                        }

                    }

                }

                Connectivity = new int[totalmember + 1, 3];

                for (int k = 1; k < nodeall.Length; k++)
                {

                    for (int j = 0; j < mLinesy.Count; j++)
                    {

                        if (mLinesy[j].StartPoint == nodecoordinate[k])
                        {


                            Connectivity[j + 1, 1] = nodenumer[k];

                        }

                    }
                }


                for (int k = 1; k <= totalnode; k++)
                {

                    for (int j = 0; j < mLinesy.Count; j++)
                    {

                        if (mLinesy[j].EndPoint == nodecoordinate[k])
                        {
                            Connectivity[j + 1, 2] = nodenumer[k];

                        }

                    }
                }


                Invoke_Comma_Separated_String_In_Connectivity();

                PointF p = new PointF(-10, -10);
                ptSelln = p;
                ptSelNode = p;
                if (bool_EDIT == true)
                {
                    Invoke_Member_Edit();
                }

            }
            catch (Exception ex)
            {



                Debug.Print(" Line_To_Node_AND_Connectivity ex :" + Environment.NewLine + ex.ToString());
            }




        }





        private void Invoke_Member_Edit()
        {








            Draw_Model = true;


            PictureBox1.Invalidate();

        }


        private bool Find_PointF_Exists_On_Point_ARRAY(PointF pt, List<PointF> pty)
        {
            bool bool_Find_PointF_Exists = false;
            for (int j = 0; j <= pty.Count - 1; j++)
            {

                if (pt == pty[j])
                {
                    bool_Find_PointF_Exists = true;
                }

            }

            return bool_Find_PointF_Exists;

        }





        private PointF[] Line_To_Point_sort_no_repeat_PointF(List<line> mLinesx)
        {
            PointF[] all_points = new PointF[mLinesx.Count * 2 + 1];

            int m = 0;

            m = 0;


            for (int j = 0; j <= mLinesx.Count - 1; j++)
            {

                all_points[m] = mLinesx[j].StartPoint;


                m = m + 1;
            }


            for (int j = 0; j <= mLinesx.Count - 1; j++)
            {


                all_points[m] = mLinesx[j].EndPoint;


                m = m + 1;
            }

            PointF[] myPoints2 = sort_no_repeat_PointF(all_points);


            return myPoints2;
        }








        private PointF[] sort_no_repeat_PointF_Line_To_Point1(List<line> mLinesx)
        {
            int mm = 0;
            PointF[] myPoints = new PointF[(mLinesx.Count) * 2];
            for (int i = 0; i < mLinesx.Count; i++)
            {

                myPoints[mm] = mLinesx[i].StartPoint;
                mm = mm + 1;
                myPoints[mm] = mLinesx[i].EndPoint;
                mm = mm + 1;

            }

            PointF[] myPoints2 = sort_no_repeat_PointF(myPoints);


            return myPoints2;
        }


        //===============================================


        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("NOTEPAD> VIEW> UNCHECK WORD WRAP !!!");

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fffffff"); // ffffff for 7 decimal points of seconds.
                string uniqueId = Guid.NewGuid().ToString("N"); // "N" format removes hyphens.
                Console.WriteLine("Unique String: " + uniqueId.ToString());
                Console.WriteLine("Unique String: " + timestamp.ToString());
                string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_fffffff") + uniqueId;



                string filePath = pj_folder_path + "\\00OUTPUT\\"+"\\"+uniqueFileName+".txt";
                Console.WriteLine(filePath);
                string content = stringBuilderALL.ToString();//"Hello, this is some text written to a file."; // The content to write.


            try
            {

                string directoryPath = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllText(filePath, content); // Write the content to the file.
                Console.WriteLine("File created successfully at: {filePath}");
            }
            catch (Exception)
                {
                Console.WriteLine("An error occurred: {ex.Message}");
            }

                //=================================================



            Process.Start("notepad.exe", filePath);


               //======================================================


        }  /////private void button4_Click(object sender, EventArgs e)



        private void FIND_Left_Right_Bottom_Top(string[] St_a)
        {

            int i, j, k;

            int ID = 0;
            string[] Each_Line_Without_Alphabet = null;

            string[] parts_split_comma = new string[] { };

            double[] x1 = new double[1];
            double[] y1 = new double[1];
            double[] x2 = new double[1];
            double[] y2 = new double[1];


            try
            {

                i = 0;
                j = 0;
                for (i = 0; i < St_a.Length; i++)
                {
                    if (St_a[i].Substring(0, 1) == "L")
                    {
                        Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                        Each_Line_Without_Alphabet[j] = St_a[i].Replace("L", "");

                        j = j + 1;
                    }
                }
                x1 = new double[Each_Line_Without_Alphabet.Length];
                y1 = new double[Each_Line_Without_Alphabet.Length];
                x2 = new double[Each_Line_Without_Alphabet.Length];
                y2 = new double[Each_Line_Without_Alphabet.Length];



                j = 0;

                ID = 0;
                for (i = 0; i < Each_Line_Without_Alphabet.Length; i++)
                {


                    parts_split_comma = Each_Line_Without_Alphabet[i].Split(new char[] { ',' });
                    for (k = 0; k < parts_split_comma.Length; k++)
                    {

                    }

                    ID = int.Parse(parts_split_comma[0]);




                    x1[i] = (Convert.ToDouble(parts_split_comma[1]));
                    y1[i] = (Convert.ToDouble(parts_split_comma[2]));
                    x2[i] = (Convert.ToDouble(parts_split_comma[3]));
                    y2[i] = (Convert.ToDouble(parts_split_comma[4]));



                }

                double[] resultX = new double[x1.Length + x2.Length];
                for (i = 0; i < x1.Length; i++)
                {
                    resultX[i] = x1[i];
                }
                for (i = 0; i < x2.Length; i++)
                {
                    resultX[x1.Length + i] = x2[i];
                }

                for (k = 0; k < resultX.Length; k++)
                {

                }
                left = (float)FindMIN_Double_1D(resultX);
                right = (float)FindMAX_Double_1D(resultX);

                double[] resultY = new double[x1.Length + x2.Length];
                for (i = 0; i < y1.Length; i++)
                {
                    resultY[i] = y1[i];
                }
                for (i = 0; i < y2.Length; i++)
                {
                    resultY[y1.Length + i] = y2[i];
                }

                for (k = 0; k < resultY.Length; k++)
                {

                }
                bottom = (float)FindMIN_Double_1D(resultY);
                top = (float)FindMAX_Double_1D(resultY);
                //Debug.Print("NEW >>>>>> FIND_Left_Right_Bottom_Top right + left  + bottom (from downside, new X,Y) +  top(from downside, new X,Y) =" + right + "," + left + "," + bottom + "," + top);






            }
            catch (Exception ex)
            {
                Debug.Print(" FIND_Left_Right_Bottom_Top(string[] St_a) ex " + ex.ToString());
            }

        }






        private void FIND_Left_Right_Bottom_Top_OLD(string[] St_a)
        {
            try
            {

                int i, j, k;
                List<line> mLinesW = new List<line>();
                int ID = 0;
                string[] Each_Line_Without_Alphabet = null;

                string[] parts_split_comma = new string[] { };




                mLinesW.Clear();




                i = 0;
                j = 0;
                for (i = 0; i < St_a.Length; i++)
                {
                    if (St_a[i].Substring(0, 1) == "L")
                    {
                        Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                        Each_Line_Without_Alphabet[j] = St_a[i].Replace("L", "");

                        j = j + 1;
                    }
                }



                j = 0;

                ID = 0;
                for (i = 0; i < Each_Line_Without_Alphabet.Length; i++)
                {


                    parts_split_comma = Each_Line_Without_Alphabet[i].Split(new char[] { ',' });
                    for (k = 0; k < parts_split_comma.Length; k++)
                    {

                    }

                    ID = int.Parse(parts_split_comma[0]);


                    line temp11 = new line();


                    temp11.StartPoint.X = (float)(Convert.ToDouble(parts_split_comma[1]) * GridX);
                    temp11.StartPoint.Y = (float)(Convert.ToDouble(parts_split_comma[2]) * GridX);
                    temp11.EndPoint.X = (float)(Convert.ToDouble(parts_split_comma[3]) * GridX);
                    temp11.EndPoint.Y = (float)(Convert.ToDouble(parts_split_comma[4]) * GridX);
                    mLinesW.Add(temp11);
                    temp11 = null;



                }

                double[] x1 = new double[mLinesW.Count];
                double[] y1 = new double[mLinesW.Count];
                double[] x2 = new double[mLinesW.Count];
                double[] y2 = new double[mLinesW.Count];
                for (i = 0; i < mLinesW.Count; i++)
                {

                    x1[i] = (mLinesW[i].StartPoint.X / GridX);
                    y1[i] = (mLinesW[i].StartPoint.Y / GridX);
                    x2[i] = (mLinesW[i].EndPoint.X / GridX);
                    y2[i] = (mLinesW[i].EndPoint.Y / GridX);
                }
                double z1 = FindMAX_Double_1D(x1);
                double z1x = FindMAX_Double_1D(x2);
                if (z1 >= z1x) right = (float)z1;
                if (z1x >= z1) right = (float)z1x;
                double z2 = FindMAX_Double_1D(y1);
                double z2y = FindMAX_Double_1D(y2);
                if (z2 >= z2y) bottom = (float)z2;
                if (z2y >= z2) bottom = (float)z2y;

                double z11 = FindMIN_Double_1D(x1);
                double z11x = FindMIN_Double_1D(x2);
                if (z11 >= z11x) left = (float)z11x;
                if (z11x >= z11) left = (float)z11;

                double z22 = FindMIN_Double_1D(y1);
                double z22y = FindMIN_Double_1D(y2);
                if (z22 >= z22y) top = (float)z22y;
                if (z22y >= z22) top = (float)z22;

                //Debug.Print("OLD>>>FIND_Left_Right_Bottom_Top right + left  + bottom (from upside, old X,Y) +  top(from upside, old X,Y) =" + right + "," + left + "," + bottom + "," + top);




            }
            catch (Exception ex)
            {
                Debug.Print(" FIND_Left_Right_Bottom_Top(string[] St_a) ex " + ex.ToString());
            }

        }






        public int[] FIND_totalmember_totalnode(string[] St_a)
        {
            int totalmemberxyz = 0;
            int totalnodexyz = 0;
            int[] kkk = new int[3];
            kkk[0] = 0;
            kkk[1] = 0;
            kkk[2] = 0;
            int i = 0;
            int k = 0;
            int j = 0;
            HINGE_Num_total = 0;
            lenght_factor = 1;

            bool_Mem_Load_Pload_from_Read = true;
            bool_SUPPORT_from_Read = true;
            bool_Node_Load_Joint_from_Read = true;
            bool_Hinge_from_Read = true;





            int ID = 0;

            string[] Each_Line_Without_Alphabet = null;

            string[] parts_split_comma = new string[] { };
            bool Proceed_further = false;


            for (i = 0; i < St_a.Length; i++)
            {
                Proceed_further = false;

                if (!string.IsNullOrEmpty(St_a[i]))
                {
                    Proceed_further = true;
                }
            }

            List<line> mLinesxyz = new List<line>();
            if (Proceed_further == true)
            {


                try
                {




                    for (i = 0; i < St_a.Length; i++)
                    {

                        St_a[i] = St_a[i].Replace(" ", "");
                    }


                    i = 0;
                    j = 0;
                    for (i = 0; i < St_a.Length; i++)
                    {
                        if (St_a[i].Substring(0, 1) == "L")
                        {
                            Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                            Each_Line_Without_Alphabet[j] = St_a[i].Replace("L", "");

                            j = j + 1;
                        }
                    }



                    j = 0;

                    ID = 0;
                    for (i = 0; i < Each_Line_Without_Alphabet.Length; i++)
                    {


                        parts_split_comma = Each_Line_Without_Alphabet[i].Split(new char[] { ',' });
                        for (k = 0; k < parts_split_comma.Length; k++)
                        {

                        }

                        ID = int.Parse(parts_split_comma[0]);


                        line temp = new line();

                        temp.StartPoint.X = (float)(Convert.ToDouble(parts_split_comma[1]) * GridX);
                        temp.StartPoint.Y = -(float)(Convert.ToDouble(parts_split_comma[2]) * GridX) + 16 * GridX;
                        temp.EndPoint.X = (float)(Convert.ToDouble(parts_split_comma[3]) * GridX);
                        temp.EndPoint.Y = -(float)(Convert.ToDouble(parts_split_comma[4]) * GridX) + 16 * GridX;



                        mLinesxyz.Add(temp);
                        temp = null;




                    }

                    totalmemberxyz = mLinesxyz.Count;

                    kkk[1] = totalmemberxyz;





                    PointF[] all_points = new PointF[mLinesxyz.Count * 2 + 1];

                    int m = 0;

                    m = 0;


                    for (j = 0; j <= mLinesxyz.Count - 1; j++)
                    {

                        all_points[m] = mLinesxyz[j].StartPoint;


                        m = m + 1;
                    }


                    for (j = 0; j <= mLinesxyz.Count - 1; j++)
                    {


                        all_points[m] = mLinesxyz[j].EndPoint;

                        m = m + 1;
                    }





                    PointF[] nodeallx = sort_no_repeat_PointF(all_points);
                    totalnodexyz = nodeallx.Length;
                    kkk[2] = totalnodexyz;


                }
                catch (Exception ex)
                {
                    Debug.Print("FIND_totalmember_totalnode, ex " + ex.ToString());
                }
            }

            return kkk;
        }



        public void READ_From_Given_Array_Of_Line_String_HINGE_L_Fact(string[] St_a)
        {

            int totalnodexyz = 0;
            int totalmemberxyz = 0;
            int[] xyz = FIND_totalmember_totalnode(St_a);
            totalmemberxyz = xyz[1];
            totalnodexyz = xyz[2];


            int i = 0;
            int j = 0;
            HINGE_Num_total = 0;
            lenght_factor = 1;

            bool_Mem_Load_Pload_from_Read = true;
            bool_SUPPORT_from_Read = true;
            bool_Node_Load_Joint_from_Read = true;
            bool_Hinge_from_Read = true;

            HINGE_String = new string[1];

            string[] Each_Line_Without_Alphabet = null;

            string[] parts_split_comma = new string[] { };
            bool Proceed_further = false;


            for (i = 0; i < St_a.Length; i++)
            {
                Proceed_further = false;

                if (!string.IsNullOrEmpty(St_a[i]))
                {
                    Proceed_further = true;
                }
            }






            if (Proceed_further == true)
            {

                try
                {


                    j = 0;
                    for (i = 0; i < St_a.Length; i++)
                    {
                        if (St_a[i].Substring(0, 1) == "F")
                        {
                            Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                            Each_Line_Without_Alphabet[j] = St_a[i].Replace("F", "");


                            lenght_factor = int.Parse(Each_Line_Without_Alphabet[j]);



                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print("READ_From_Given_Array_Of_Line_String_HINGE_L_Fact " + ex.ToString());
                }

            }


            label1.Text = "SCALE FACTOR" + " 1 GRID DIVISION= " + lenght_factor + " M";
            label1.BackColor = System.Drawing.Color.Gold;


            if (Proceed_further == true)
            {
                try
                {

                    Array.Resize(ref HINGE_String, totalnodexyz + 1);

                    j = 0;
                    for (i = 0; i < St_a.Length; i++)
                    {
                        if (St_a[i].Substring(0, 1) == "H")
                        {
                            Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                            Each_Line_Without_Alphabet[j] = St_a[i].Replace("H", "");



                            j = j + 1;
                        }
                    }

                    HINGE_Num_total = j;
                    //Debug.Print("lenght_factor , HINGE_Num_total= " + lenght_factor + " , " + HINGE_Num_total);
                }
                catch (Exception ex)
                {
                    Debug.Print("READ_From_Given_Array_Of_Line_String_HINGE_L_Fact " + ex.ToString());
                }


            }

        }

        private string[] Find_Coordinate_XY_at_Origin_From_Line(List<line> mLinesy1)
        {
            List<line> mLines_00 = new List<line>();
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            System.Text.StringBuilder result_00 = new System.Text.StringBuilder();
            float left0, bottom0;
            string[] st99 = null;
            string stxx = null;

            string[] st_00 = null;
            string stxx_00 = null;

            try
            {




                double xx1 = 0;
                double xx2 = 0;
                double yy1 = 0;
                double yy2 = 0;
                mLines_00 = mLinesy1;
                if (mLines_00.Count > 0)
                {
                    for (int i = 0; i < mLines_00.Count; i++)
                    {
                        xx1 = mLines_00[i].StartPoint.X / GridX;
                        yy1 = mLines_00[i].StartPoint.Y / GridX;
                        xx2 = mLines_00[i].EndPoint.X / GridX;
                        yy2 = mLines_00[i].EndPoint.Y / GridX;

                        yy1 = 16 - yy1;
                        yy2 = 16 - yy2;


                        Debug.Print("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);

                        result.Append("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                        result.AppendLine();
                    }
                }



                stxx = result.ToString();

                st99 = stxx.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < st99.Length; i++)
                {
                    st99[i] = st99[i].Replace(" ", "");


                }
                FIND_Left_Right_Bottom_Top(st99);
                left0 = left;
                bottom0 = bottom;


                if (mLines_00.Count > 0)
                {
                    for (int i = 0; i < mLines_00.Count; i++)
                    {
                        xx1 = mLines_00[i].StartPoint.X / GridX - left0;
                        yy1 = mLines_00[i].StartPoint.Y / GridX;
                        xx2 = mLines_00[i].EndPoint.X / GridX - left0;
                        yy2 = mLines_00[i].EndPoint.Y / GridX;

                        yy1 = 16 - yy1 - bottom0;
                        yy2 = 16 - yy2 - bottom0;


                        Debug.Print(left0 + "," + bottom0 + "," + "L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                        result_00.Append("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                        result_00.AppendLine();

                    }
                }


                stxx_00 = result_00.ToString();

                st_00 = stxx_00.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < st_00.Length; i++)
                {
                    st_00[i] = st_00[i].Replace(" ", "");


                }

            }
            catch (Exception ex)
            {
                Debug.Print(" Find_Coordinate_XY_at_Origin_From_Line ex " + ex.ToString());
            }

            return st_00;

        }


        private string[] Find_Coordinate_XY_at_Origin_string(string[] St_a)
        {
            List<line> mLines_00 = new List<line>();
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            System.Text.StringBuilder result_00 = new System.Text.StringBuilder();
            float left0, right0, top0, bottom0;

            string[] st_00 = null;
            string stxx_00 = null;

            int i, j, k;

            int ID = 0;
            string[] Each_Line_Without_Alphabet = null;

            string[] parts_split_comma = new string[] { };

            double[] x1 = new double[1];
            double[] y1 = new double[1];
            double[] x2 = new double[1];
            double[] y2 = new double[1];


            try
            {

                i = 0;
                j = 0;
                for (i = 0; i < St_a.Length; i++)
                {
                    if (St_a[i].Substring(0, 1) == "L")
                    {
                        Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                        Each_Line_Without_Alphabet[j] = St_a[i].Replace("L", "");

                        j = j + 1;
                    }
                }
                x1 = new double[Each_Line_Without_Alphabet.Length];
                y1 = new double[Each_Line_Without_Alphabet.Length];
                x2 = new double[Each_Line_Without_Alphabet.Length];
                y2 = new double[Each_Line_Without_Alphabet.Length];



                j = 0;

                ID = 0;
                for (i = 0; i < Each_Line_Without_Alphabet.Length; i++)
                {


                    parts_split_comma = Each_Line_Without_Alphabet[i].Split(new char[] { ',' });
                    for (k = 0; k < parts_split_comma.Length; k++)
                    {

                    }

                    ID = int.Parse(parts_split_comma[0]);




                    x1[i] = (Convert.ToDouble(parts_split_comma[1]));
                    y1[i] = (Convert.ToDouble(parts_split_comma[2]));
                    x2[i] = (Convert.ToDouble(parts_split_comma[3]));
                    y2[i] = (Convert.ToDouble(parts_split_comma[4]));



                }

                double[] resultX = new double[x1.Length + x2.Length];
                for (i = 0; i < x1.Length; i++)
                {
                    resultX[i] = x1[i];
                }
                for (i = 0; i < x2.Length; i++)
                {
                    resultX[x1.Length + i] = x2[i];
                }


                left0 = (float)FindMIN_Double_1D(resultX);
                right0 = (float)FindMAX_Double_1D(resultX);

                double[] resultY = new double[x1.Length + x2.Length];
                for (i = 0; i < y1.Length; i++)
                {
                    resultY[i] = y1[i];
                }
                for (i = 0; i < y2.Length; i++)
                {
                    resultY[y1.Length + i] = y2[i];
                }


                bottom0 = (float)FindMIN_Double_1D(resultY);
                top0 = (float)FindMAX_Double_1D(resultY);
                //Debug.Print("NEW >>>>>> FIND_Left_Right_Bottom_Top right + left  + bottom (from downside, new X,Y) +  top(from downside, new X,Y) =" + right + "," + left + "," + bottom + "," + top);

                //=====================================

                resultX = new double[x1.Length + x2.Length];
                for (i = 0; i < x1.Length; i++)
                {
                    resultX[i] = x1[i] - left0;

                }
                for (i = 0; i < x2.Length; i++)
                {
                    resultX[x1.Length + i] = x2[i] - left0;
                }




                resultY = new double[x1.Length + x2.Length];


                for (i = 0; i < y1.Length; i++)
                {
                    resultY[i] = y1[i] - bottom0;
                }
                for (i = 0; i < y2.Length; i++)
                {
                    resultY[y1.Length + i] = y2[i] - bottom0;
                }


                for (i = 0; i < y2.Length; i++)
                {
                    //result_00.Append("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                    result_00.Append("L" + (i + 1) + "," + resultX[i] + "," + resultY[i] + "," + resultX[x1.Length + i] + "," + resultY[y1.Length + i]);
                    result_00.AppendLine();

                }
                //=====================================


                stxx_00 = result_00.ToString();

                st_00 = stxx_00.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                for (i = 0; i < st_00.Length; i++)
                {
                    st_00[i] = st_00[i].Replace(" ", "");


                }

            }
            catch (Exception ex)
            {
                Debug.Print(" Find_Coordinate_XY_at_Origin_string ex " + ex.ToString());
            }

            return st_00;



        }




        private void READ_From_Given_Array_Of_Line_String(string[] St_a)
        {
            bool Proceed_further = false;
            int i = 0;

            int k = 0;
            int j = 0;

            string[] parts_split_comma0 = new string[] { };
            string[] Each_Line_Without_Alphabet0 = new string[] { };

            string[] st_002 = null;

            for (i = 0; i < St_a.Length; i++)
            {
                Proceed_further = false;

                if (!string.IsNullOrEmpty(St_a[i]))
                {
                    Proceed_further = true;
                }
            }



            //======================================

            //Start_TextWriterTraceListener();
            //=================================
            stringBuilderALL = new StringBuilder();
            Start1_SbTraceListener();
            //===================================
            if (Proceed_further == true)
            {
                for (i = 0; i < St_a.Length; i++)
                {
                    Debug.Print(St_a[i]);
                }
                Debug.Print("=====================================================");




                //=============================================================
                st_002 = Find_Coordinate_XY_at_Origin_string(St_a);
                for (i = 0; i < st_002.Length; i++)
                {
                    if (st_002[i].Substring(0, 1) == "L")
                    {
                        //Debug.Print("00XY= " + st_002[i]);
                        Array.Resize(ref Each_Line_Without_Alphabet0, j + 1);
                        Each_Line_Without_Alphabet0[j] = st_002[i].Replace("L", "");

                        j = j + 1;
                    }


                }

                j = 0;
                for (i = 0; i < Each_Line_Without_Alphabet0.Length; i++)
                {
                    parts_split_comma0 = Each_Line_Without_Alphabet0[i].Split(new char[] { ',' });
                    //Debug.Print("st_002[i]= " + st_002[i]);
                    Debug.Print("Member # " + (i + 1) + " , Start coordinate,  End coordinate : " + parts_split_comma0[1] + "," + parts_split_comma0[2] + "," + parts_split_comma0[3] + "," + parts_split_comma0[4]);

                }


                for (i = 0; i < st_002.Length; i++)
                {

                }

            }

            Debug.Print("=====================================================");

            //=============================================================================================
            bool_Mem_Load_Pload_from_Read = true;
            bool_SUPPORT_from_Read = true;
            bool_Node_Load_Joint_from_Read = true;
            bool_Hinge_from_Read = true;

            m_Lines1.Clear();
            mLines.Clear();
            totalmember = 0;
            totalnode = 0;


            try
            {
                //======================================== 

                Pload = new string[1];
                PJoint = new string[1];
                Sup_Node_Number_Type = new string[1];
                Pload2 = new string[1];
                PJoint2 = new string[1];
                Sup_Node_Number_Type2 = new string[1];
                Sup_Node_Number = new int[1];
                suptype = new int[1];
                Support_Displacement_S = new string[1];
                Mproperty_A_I_E = new string[100];
                HINGE_String = new string[1];
                HINGE_NodeID = new int[1];
                HINGE_MemID = new int[1];
                HINGE_New_DOF = new int[1];
                HINGE_MemEnd_Index = new int[1];




                //==================================================


                Array.Clear(Pload, 0, Pload.Length);
                Array.Clear(PJoint, 0, PJoint.Length);
                Array.Clear(Sup_Node_Number_Type, 0, Sup_Node_Number_Type.Length);
                Array.Clear(Pload2, 0, Pload2.Length);
                Array.Clear(PJoint2, 0, PJoint2.Length);
                Array.Clear(Sup_Node_Number_Type2, 0, Sup_Node_Number_Type2.Length);
                Array.Clear(Sup_Node_Number, 0, Sup_Node_Number.Length);
                Array.Clear(suptype, 0, suptype.Length);
                Array.Clear(Support_Displacement_S, 0, Support_Displacement_S.Length);
                Array.Clear(Mproperty_A_I_E, 0, Mproperty_A_I_E.Length);
                Array.Clear(HINGE_String, 0, HINGE_String.Length);
                Array.Clear(HINGE_String2, 0, HINGE_String2.Length);
                Array.Clear(HINGE_NodeID, 0, HINGE_NodeID.Length);
                Array.Clear(HINGE_MemID, 0, HINGE_MemID.Length);
                Array.Clear(HINGE_New_DOF, 0, HINGE_New_DOF.Length);
                Array.Clear(HINGE_MemEnd_Index, 0, HINGE_MemEnd_Index.Length);
            }
            catch (Exception ex)
            {
                Debug.Print("READ_From_Given_Array_Of_Line_String Exception ex: " + ex.ToString());
            }


            Pload = new string[totalmember + 1];
            PJoint = new string[totalnode + 1];
            Sup_Node_Number_Type = new string[totalnode + 1];
            Mproperty_A_I_E = new string[totalmember + 1];
            Support_Displacement_S = new string[totalnode + 1];
            HINGE_String = new string[totalnode + 1];




            int ID = 0;

            string[] Each_Line_Without_Alphabet = null;

            string[] parts_split_comma = new string[] { };




            if (Proceed_further == true)
            {


                try
                {
                    FIND_Left_Right_Bottom_Top(St_a);



                    for (i = 0; i < St_a.Length; i++)
                    {

                        St_a[i] = St_a[i].Replace(" ", "");
                    }


                    i = 0;
                    j = 0;
                    for (i = 0; i < St_a.Length; i++)
                    {
                        if (St_a[i].Substring(0, 1) == "L")
                        {

                            Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                            Each_Line_Without_Alphabet[j] = St_a[i].Replace("L", "");
                            //Debug.Print(i+1 + " =Member # , Start coordinate,  End coordinate : "+ Each_Line_Without_Alphabet[i]);
                            j = j + 1;
                        }
                    }


                    j = 0;

                    ID = 0;
                    for (i = 0; i < Each_Line_Without_Alphabet.Length; i++)
                    {


                        parts_split_comma = Each_Line_Without_Alphabet[i].Split(new char[] { ',' });
                        for (k = 0; k < parts_split_comma.Length; k++)
                        {

                        }

                        ID = int.Parse(parts_split_comma[0]);


                        line temp = new line();




                        temp.StartPoint.X = (float)((Convert.ToDouble(parts_split_comma[1]) + (42 - right + left) / 2 - left) * GridX);
                        temp.StartPoint.Y = -(float)(Convert.ToDouble(parts_split_comma[2]) * GridX) + (top + 2) * GridX;
                        temp.EndPoint.X = (float)((Convert.ToDouble(parts_split_comma[3]) + (42 - right + left) / 2 - left) * GridX);
                        temp.EndPoint.Y = -(float)((Convert.ToDouble(parts_split_comma[4])) * GridX) + (top + 2) * GridX;


                        mLines.Add(temp);
                        temp = null;




                    }


                    Line_To_Node_AND_Connectivity(mLines);
                    totalnode = totalnode_From_Lines;




                    totalmember = mLines.Count;



                    string[] stx = Line_To_ArryLine_String(mLines);

                    FIND_Left_Right_Bottom_Top(stx);

                    for (j = 0; j < mLines.Count; j++)
                    {
                        //Debug.Print(j + " mLines[j]= " + mLines[j].StartPoint.X / GridX + " , " + mLines[j].StartPoint.Y / GridX + " , " + mLines[j].EndPoint.X / GridX + " , " + mLines[j].EndPoint.Y / GridX);

                    }

                    for (k = 1; k <= totalmember; k++)
                    {
                        //Debug.Print(k + " =Member # , Connectivity[k, 1] , Connectivity[k, 2] :" + Connectivity[k, 1] + "  " + Connectivity[k, 2]);

                    }

                    for (k = 1; k <= totalmember; k++)
                    {


                        Debug.Print("Member # " + k + ", cos , sin, Theta deg, Lenght..............." + cosx(k) + " " + sinx(k) + "  " + Theta(k) + "  " + L_n(k));
                    }



                }
                catch (Exception ex)
                {
                    Debug.Print(" READ_From_Given_Array_Of_Line_String- members, connectivity ex " + ex.ToString());
                }






                Array.Resize(ref Sup_Node_Number_Type, totalnode + 1);

                try
                {



                    j = 0;
                    for (i = 0; i < St_a.Length; i++)
                    {
                        if (St_a[i].Substring(0, 1) == "S")
                        {
                            Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                            Each_Line_Without_Alphabet[j] = St_a[i].Replace("S", "");

                            parts_split_comma = Each_Line_Without_Alphabet[j].Split(new char[] { ',' });
                            ID = int.Parse(parts_split_comma[0]);
                            Sup_Node_Number_Type[ID] = Each_Line_Without_Alphabet[j];




                            j = j + 1;
                        }
                    }



                    Array.Resize(ref Sup_Node_Number_Type, totalnode + 1); ;

                    for (k = 1; k < Pload.Length; k++)
                    {

                        //Debug.Print("k,  Sup_Node_Number_Type.Length ,  Sup_Node_Number_Type[k]= " + k + " , " + Sup_Node_Number_Type.Length + " , " + Sup_Node_Number_Type[k]);
                        if (!string.IsNullOrEmpty(Sup_Node_Number_Type[k]))
                        {
                            Debug.Print("Node #" + k + " , Suppport_Node_Number_Type = " + k + " , " + PJoint[k]);
                        }

                    }





                }
                catch (Exception ex)
                {
                    Debug.Print("ex 2----read Support info:" + ex.ToString());
                }



                try
                {

                    Array.Resize(ref Pload, totalmember + 1);


                    ID = 0;
                    j = 0;
                    for (i = 0; i < St_a.Length; i++)
                    {
                        if (St_a[i].Substring(0, 1) == "P")
                        {
                            Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                            Each_Line_Without_Alphabet[j] = St_a[i].Replace("P", "");

                            parts_split_comma = Each_Line_Without_Alphabet[j].Split(new char[] { ',' });
                            ID = int.Parse(parts_split_comma[0]);
                            Pload[ID] = Each_Line_Without_Alphabet[j];

                            j = j + 1;
                        }
                    }


                    Array.Resize(ref Pload, totalmember + 1);


                    for (k = 1; k < Pload.Length; k++)
                    {

                        //Debug.Print("k,  Pload.Length ,  Pload[k]= " + k + " , " + Pload.Length + " , " + Pload[k]);

                        if (!string.IsNullOrEmpty(Pload[k]))
                        {
                            Debug.Print("Member#" + k + " , Member load = " + Pload[k]);
                        }

                    }



                }
                catch (Exception ex)
                {
                    Debug.Print("ex 3---- Member load from READ_From_Given_Array_Of_Line_String: ex " + ex.ToString());
                }






                Array.Resize(ref PJoint, totalnode + 1);


                ID = 0;
                j = 0;
                for (i = 0; i < St_a.Length; i++)
                {
                    if (St_a[i].Substring(0, 1) == "J")
                    {
                        Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                        Each_Line_Without_Alphabet[j] = St_a[i].Replace("J", "");

                        parts_split_comma = Each_Line_Without_Alphabet[j].Split(new char[] { ',' });
                        ID = int.Parse(parts_split_comma[0]);
                        PJoint[ID] = Each_Line_Without_Alphabet[j];

                        j = j + 1;
                    }
                }


                Array.Resize(ref PJoint, totalnode + 1);


                for (k = 1; k < PJoint.Length; k++)
                {

                    //Debug.Print("k,  PJoint.Length ,  PJoint[k]= " + k + " , " + PJoint.Length + " , " + PJoint[k]);
                    if (!string.IsNullOrEmpty(PJoint[k]))
                    {
                        Debug.Print("Node #" + k + " , Jointload = " + PJoint[k]);
                    }
                }










                try
                {
                    string[] test = { };
                    Array.Resize(ref Iz, totalmember + 1);
                    Array.Resize(ref Ayz, totalmember + 1);
                    Array.Resize(ref Ey, totalmember + 1);

                    Array.Resize(ref Mproperty_A_I_E, totalmember + 1);

                    ID = 0;
                    j = 0;

                    for (i = 0; i < St_a.Length; i++)
                    {
                        if (St_a[i].Substring(0, 1) == "M")
                        {
                            Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                            Each_Line_Without_Alphabet[j] = St_a[i].Replace("M", "");

                            parts_split_comma = Each_Line_Without_Alphabet[j].Split(new char[] { ',' });
                            ID = int.Parse(parts_split_comma[0]);
                            Mproperty_A_I_E[ID] = Each_Line_Without_Alphabet[j];

                            j = j + 1;
                        }
                    }

                    Array.Resize(ref Mproperty_A_I_E, totalmember + 1);



                    j = 0;
                    i = 0;
                    for (i = 1; i <= totalmember; i++)
                    {

                        if (!string.IsNullOrEmpty(Mproperty_A_I_E[i]))
                        {
                            test = Mproperty_A_I_E[i].Split(new char[] { ',' });


                            Ayz[i] = double.Parse(test[1]);
                            Iz[i] = double.Parse(test[2]);
                            Ey[i] = double.Parse(test[3]);


                        }






                    }
                    //Debug.Print(Environment.NewLine);
                    for (k = 1; k < Mproperty_A_I_E.Length; k++)
                    {
                        //Debug.Print("k,  Mproperty_A_I_E.Length ,  Mproperty_A_I_E[k]= " + k + " , " + Mproperty_A_I_E.Length + " , " + Mproperty_A_I_E[k]);
                        if (!string.IsNullOrEmpty(Mproperty_A_I_E[k]))
                        {
                            Debug.Print("Member# " + k + " , Mproperty_A_I_E = " + Mproperty_A_I_E[k]);
                        }


                    }



                }
                catch (Exception ex)
                {
                    Debug.Print("ex 5----Member Mproperty_A_I_E READ_From_Given_Array_Of_Line_String " + ex.ToString());
                }




                Array.Resize(ref   Support_Displacement_S, totalnode + 1);

                try
                {



                    j = 0;
                    for (i = 0; i < St_a.Length; i++)
                    {
                        if (St_a[i].Substring(0, 1) == "D")
                        {
                            Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                            Each_Line_Without_Alphabet[j] = St_a[i].Replace("D", "");

                            parts_split_comma = Each_Line_Without_Alphabet[j].Split(new char[] { ',' });
                            ID = int.Parse(parts_split_comma[0]);
                            Support_Displacement_S[ID] = Each_Line_Without_Alphabet[j];





                            j = j + 1;
                        }
                    }



                    Array.Resize(ref Support_Displacement_S, totalnode + 1);

                    //Debug.Print(Environment.NewLine);
                    for (k = 1; k < Support_Displacement_S.Length; k++)
                    {
                        //Debug.Print("k,  Support_Displacement_S.Length ,  Support_Displacement_S[k]= " + k + " , " + Support_Displacement_S.Length + " , " + Support_Displacement_S[k]);
                        if (!string.IsNullOrEmpty(Support_Displacement_S[k]))
                        {
                            Debug.Print("Node # " + k + " , Support_Displacement = " + Support_Displacement_S[k]);
                        }

                    }



                }
                catch (Exception ex)
                {
                    Debug.Print("ex 6----read Support disp:  Support_Displacement_S  READ_From_Given_Array_Of_Line_String " + ex.ToString());
                }





                try
                {


                    Array.Resize(ref HINGE_String, totalnode_From_Lines + 1);
                    Array.Resize(ref  HINGE_NodeID, totalnode_From_Lines + 1);
                    Array.Resize(ref  HINGE_MemID, totalnode_From_Lines + 1);
                    Array.Resize(ref HINGE_New_DOF, totalnode_From_Lines + 1);
                    Array.Resize(ref HINGE_MemEnd_Index, totalnode_From_Lines + 1);
                    int NODENUM = 0;
                    int MEMNUM = 0;




                    j = 0;
                    for (i = 0; i < St_a.Length; i++)
                    {
                        if (St_a[i].Substring(0, 1) == "H")
                        {
                            Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                            Each_Line_Without_Alphabet[j] = St_a[i].Replace("H", "");

                            parts_split_comma = Each_Line_Without_Alphabet[j].Split(new char[] { ',' });
                            NODENUM = int.Parse(parts_split_comma[0]);
                            HINGE_NodeID[NODENUM] = NODENUM;
                            HINGE_String[NODENUM] = Each_Line_Without_Alphabet[j];
                            MEMNUM = int.Parse(parts_split_comma[1]);
                            HINGE_MemID[NODENUM] = int.Parse(parts_split_comma[1]);
                            HINGE_New_DOF[NODENUM] = totalnode_From_Lines * 3 + 1 + j;
                            HINGE_MemEnd_Index[NODENUM] = int.Parse(parts_split_comma[2]);




                            j = j + 1;
                        }
                    }

                    HINGE_Num_total = j;



                    Array.Resize(ref HINGE_String, totalnode_From_Lines + 1);


                    for (k = 1; k < HINGE_String.Length; k++)
                    {
                        //Debug.Print("k,  HINGE_String.Length ,  HINGE_String[k]= " + k + " , " + HINGE_String.Length + " , " + HINGE_String[k]);
                        if (!string.IsNullOrEmpty(HINGE_String[k]))
                        {
                            Debug.Print("Node #" + k + ",  HINGE_String = " + HINGE_String[k]);
                        }
                    }


                    for (k = 1; k < totalnode_From_Lines + 1; k++)
                    {


                    }


                }
                catch (Exception ex)
                {
                    Debug.Print("7-read HINGE READ_From_Given_Array_Of_Line_String " + ex.ToString());
                }




            }
            //Debug.Print(Environment.NewLine);
            Debug.Print("lenght_factor, HINGE_Num_total= " + lenght_factor + " , " + HINGE_Num_total);
            PictureBox1.Invalidate();

            Debug.Print("=====================================================");
            //============================================================================


            Remove1_SbTraceListener();
            sbtemp11 = new StringBuilder();
            sbtemp11.Append(stringBuilder11.ToString());

            Remove1_SbTraceListener();




            //===========================================================================

            Start3_SbTraceListener();
            Debug.Print("=====================================================");
            //====================================

        }// end private void READ_From_Given_Array_Of_Line_String(string[] St_a)









        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {




            int x = e.X;
            int y = e.Y;
            SnapToGrid(ref x, ref y);
            Location_Move = new Point(x, y);

            if (bool_cancel_draw_line == true)
            {
                Location_2 = Location_1;

                Location_Move = Location_1;
                clk = 1;
                bool_cancel_draw_line = false;
            };

            PictureBox1.Invalidate();

        }




        public void Draw_Diagramsx(int Mem_ID, Graphics gg)
        {


            int j = Mem_ID;


            if (View_PicBox == true)
            {
                if (View_Shear == true)
                {
                    gg.Clear(PictureBox1.BackColor);
                    for (j = 1; j <= totalmember; j++)
                    {
                        draw6(j, Fact3[j, 1], 1, gg);

                    }
                }
                if (View_Mom == true)
                {
                    gg.Clear(PictureBox1.BackColor);
                    for (j = 1; j <= totalmember; j++)
                    {
                        draw6(j, Fact3[j, 2], 2, gg);
                    }
                }

                if (View_Delta == true)
                {
                    bool_delta = true;
                    gg.Clear(PictureBox1.BackColor);
                    for (j = 1; j <= totalmember; j++)
                    {


                        draw3(j, Fact[j, 4], gg);






                    }


                }

                if (View_AxialForce == true)
                {
                    gg.Clear(PictureBox1.BackColor);
                    for (j = 1; j <= totalmember; j++)
                    {
                        draw6(j, Fact3[j, 5], 5, gg);
                    }
                }


            }

        }







        private void draw3(int Mem_ID, double scalev, Graphics gg)
        {



            int segment = 120;//segment2;
            double[,] arr2 = new double[totalmember + 1, segment + 1];
            int j = 0;
            for (j = 0; j <= segment; j++)
            {
                arr2[Mem_ID, j] = BMD_ALL[Mem_ID, 4, j];
            }

            double x_2 = 0;
            double x_1 = 0;
            double y_2 = 0;
            double y_1 = 0;
            double L_1 = 0;
            double angle = 0;

            x_2 = nodecoordinate[Connectivity[Mem_ID, 2]].X;
            x_1 = nodecoordinate[Connectivity[Mem_ID, 1]].X;
            y_2 = nodecoordinate[Connectivity[Mem_ID, 2]].Y;
            y_1 = nodecoordinate[Connectivity[Mem_ID, 1]].Y;
            L_1 = Math.Sqrt(Math.Pow(x_2 - x_1, 2) + Math.Pow(y_2 - y_1, 2));

            angle = (Math.Atan2((y_1 - y_2), (x_2 - x_1))) * 180 / Math.PI;
            if (angle < 0)
            {
                angle = 360 + angle;
            }


            int Midx = Mem_ID;



            double delta_1X = Disp_All_Nodes_Global[Connectivity[Midx, 1] * 3 - 2] * 1000 * scalev;
            double delta_2X = Disp_All_Nodes_Global[Connectivity[Midx, 2] * 3 - 2] * 1000 * scalev;
            double delta_1Y = Disp_All_Nodes_Global[Connectivity[Midx, 1] * 3 - 1] * 1000 * scalev;
            double delta_2Y = Disp_All_Nodes_Global[Connectivity[Midx, 2] * 3 - 1] * 1000 * scalev;

            PointF[] pt11 = new PointF[totalmember + 1];
            PointF[] pt22 = new PointF[totalmember + 1];
            int u = Mem_ID - 1;

            pt11[u].X = mLines[u].StartPoint.X;
            pt11[u].Y = mLines[u].StartPoint.Y;
            pt22[u].X = mLines[u].EndPoint.X;
            pt22[u].Y = mLines[u].EndPoint.Y;

            gg.DrawLine(new Pen(Color.Gray, 0.0F), pt11[u].X, pt11[u].Y, pt22[u].X, pt22[u].Y);

            u = Mem_ID - 1;
            PointF[] pt1 = new PointF[totalmember + 1];
            PointF[] pt2 = new PointF[totalmember + 1];
            pt1[u].X = mLines[u].StartPoint.X + (float)delta_1X;
            pt1[u].Y = mLines[u].StartPoint.Y - (float)delta_1Y;
            pt2[u].X = mLines[u].EndPoint.X + (float)delta_2X;
            pt2[u].Y = mLines[u].EndPoint.Y - (float)delta_2Y;

            PointF[] Gy = new PointF[segment + 1];
            PointF[] Gx = new PointF[segment + 1];




            double x1 = 0;
            double y1 = 0;
            double x2 = 0;
            double y2 = 0;
            double y2_actual = 0;

            double xx = 0;
            double[] dy = new double[segment + 1];
            string[] Str = new string[segment + 1];
            double frac = L_1 / segment;
            for (j = 0; j <= segment; j++)
            {



                xx = j * frac;

                x1 = x_1;
                y1 = (y_1);
                x2 = (x_1 + xx);
                y2 = (y_1 - arr2[Mem_ID, j] * scalev * 1000);

                y2_actual = 300 - arr2[Mem_ID, j] * 1000;

                Gx[j].X = (float)x2;
                Gx[j].Y = (float)y1;
                Gy[j].X = (float)x2;
                Gy[j].Y = (float)y2;
            }



            PointF[] G3 = new PointF[segment + 1];
            for (j = 0; j <= segment; j++)
            {
                G3[j].X = Point_ON_Line_AT_Fraction(pt1[Mem_ID - 1], j / (double)segment, pt2[Mem_ID - 1]).X;
                G3[j].Y = Point_ON_Line_AT_Fraction(pt1[Mem_ID - 1], j / (double)segment, pt2[Mem_ID - 1]).Y;

            }



            angle = (Math.Atan2((pt11[u].Y - pt22[u].Y), (pt22[u].X - pt11[u].X))) * 180 / Math.PI;


            if (angle < 0)
            {
                angle = 360 + angle;
            }


            PointF p0 = new PointF(0, 0);

            PointF pt = new PointF((float)x_1, (float)y_1);



            PointF[] Foot_of_Ordinate = new PointF[segment + 1];
            PointF[] Ordinate = new PointF[segment + 1];
            for (j = 0; j <= segment; j++)
            {
                Foot_of_Ordinate[j] = Gx[j];
                Ordinate[j] = Gy[j];
            }





            int jj = -1;





            PointF[] curvePoints2x = rotate_Array_PointF(Foot_of_Ordinate, p0, jj * (float)angle);


            PointF[] curvePoints3x = Translate_Array_PointF(curvePoints2x, p0.X, p0.Y);

            PointF[] curvePoints33x = new PointF[segment + 1];



            angle = (Math.Atan2((Gy[0].Y - Gy[segment].Y), (Gx[segment].X - Gx[0].X))) * 180 / Math.PI;

            if (angle < 0)
            {
                angle = 360 + angle;
            }
            jj = 1;
            PointF[] curvePoints222y = rotate_Array_PointF(Ordinate, p0, jj * (float)angle);
            PointF[] curvePointsy = Translate_Array_PointF(curvePoints222y, p0.X, p0.Y);
            PointF[] curvePoints333y = Translate_Array_PointF(curvePointsy, pt.X, pt.Y);

            PointF[] Relative_Disp_CURVE = new PointF[segment + 1];


            for (j = 0; j <= segment; j += 1)
            {
                Relative_Disp_CURVE[j].X = curvePoints333y[j].X;
                Relative_Disp_CURVE[j].Y = curvePoints333y[j].Y;
                Relative_Disp[Mem_ID, j] = (curvePoints333y[j].Y - (float)y_1) / scalev;

            }

            angle = (Math.Atan2((pt1[u].Y - pt2[u].Y), (pt2[u].X - pt1[u].X))) * 180 / Math.PI;
            if (angle < 0)
            {
                angle = 360 + angle;
            }

            jj = -1;

            PointF[] curvePoints2y = rotate_Array_PointF(Relative_Disp_CURVE, p0, jj * (float)angle);
            PointF[] curvePoints3y = Translate_Array_PointF(curvePoints2y, p0.X, p0.Y);
            PointF[] curvePoints3yy = Translate_Array_PointF(curvePoints3y, pt1[u].X, pt1[u].Y);

            gg.DrawCurve(new Pen(Color.Green, 0.0F), curvePoints3yy);





        }





        private void draw6(int Mem_ID, double scalev, int nnn, Graphics gg)
        {

            int segment = 120;//segment3;

            double[,] arr2 = new double[totalmember + 1, segment + 1];

            double x_2 = 0;
            double x_1 = 0;
            double y_2 = 0;
            double y_1 = 0;
            double L_1 = 0;
            double angle = 0;

            x_2 = nodecoordinate[Connectivity[Mem_ID, 2]].X;
            x_1 = nodecoordinate[Connectivity[Mem_ID, 1]].X;
            y_2 = nodecoordinate[Connectivity[Mem_ID, 2]].Y;
            y_1 = nodecoordinate[Connectivity[Mem_ID, 1]].Y;
            L_1 = Math.Sqrt(Math.Pow(x_2 - x_1, 2) + Math.Pow(y_2 - y_1, 2));

            angle = (Math.Atan2((y_1 - y_2), (x_2 - x_1))) * 180 / Math.PI;

            if (angle < 0)
            {
                angle = 360 + angle;
            }


            PointF[] Gy = new PointF[segment + 1];
            PointF[] Gx = new PointF[segment + 1];


            int j = 0;

            double x1 = 0;
            double y1 = 0;
            double x2 = 0;
            double y2 = 0;
            double y2_actual = 0;

            double xx = 0;
            double[] dy = new double[segment + 1];
            string[] Str = new string[segment + 1];
            double frac = L_1 / segment;
            for (j = 0; j <= segment; j++)
            {
                arr2[Mem_ID, j] = BMD_ALL3[Mem_ID, nnn, j];
                xx = j * frac;

                x1 = x_1;
                y1 = (y_1);
                x2 = (x_1 + xx);
                dy[j] = arr2[Mem_ID, j] * scalev;


                y2 = (y_1 - arr2[Mem_ID, j] * scalev);
                if (nnn != 4 & nnn != 2)
                {
                    y2 = (y1 - arr2[Mem_ID, j] * scalev);
                }

                if (nnn != 4 & nnn == 2)
                {
                    y2 = (y1 - arr2[Mem_ID, j] * scalev * (-1));
                }

                y2_actual = 300 - arr2[Mem_ID, j];
                Gx[j].X = (float)x2;
                Gx[j].Y = (float)y1;
                Gy[j].X = (float)x2;
                Gy[j].Y = (float)y2;





            }


            PointF p0 = new PointF(0, 0);

            PointF pt = new PointF((float)x_1, (float)y_1);

            PointF[] Foot_of_Ordinate = new PointF[segment + 1];
            PointF[] Ordinate = new PointF[segment + 1 + 2];
            Ordinate[0] = Gx[0];
            Ordinate[segment + 2] = Gx[segment];
            for (j = 0; j <= segment; j++)
            {
                Foot_of_Ordinate[j] = Gx[j];
                Ordinate[j + 1] = Gy[j];
            }

            int jj = -1;
            PointF[] curvePoints2x = rotate_Array_PointF(Foot_of_Ordinate, p0, jj * (float)angle);
            PointF[] curvePoints3x = Translate_Array_PointF(curvePoints2x, p0.X, p0.Y);

            PointF[] curvePoints33x = new PointF[segment + 1];


            curvePoints33x = Translate_Array_PointF(curvePoints3x, pt.X, pt.Y);
            gg.DrawCurve(new Pen(Color.Gray, 0.0F), curvePoints33x);

            jj = -1;
            PointF[] curvePoints222y = rotate_Array_PointF(Ordinate, p0, jj * (float)angle);
            PointF[] curvePointsy = Translate_Array_PointF(curvePoints222y, p0.X, p0.Y);
            PointF[] curvePoints333y = Translate_Array_PointF(curvePointsy, pt.X, pt.Y);
            gg.DrawLines(new Pen(Color.Gray, 0.0F), curvePoints333y);

            int k = 0;
            if (segment == 120) k = 12;
            if (segment == 12) k = 1;
            if (segment == 10) k = 1;
            for (j = 0; j <= segment; j += k)
            {

                gg.DrawLine(new Pen(Color.Gray, 0.0F), curvePoints33x[j], curvePoints333y[j + 1]);


            }



        }






        private PointF[] Translate_Array_PointF(PointF[] a, float tx, float ty)
        {
            PointF[] b = new PointF[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                b[i].X = a[i].X + tx - a[0].X;
                b[i].Y = a[i].Y + ty - a[0].Y;
            }
            return b;

        }


        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {


            Invoke_Paint();

            Size size1 = new Size(Convert.ToInt32(PictureScale * PictureBox1.Width), Convert.ToInt32(PictureScale * PictureBox1.Height));

            Graphics gr = e.Graphics;
            gr.Clear(PictureBox1.BackColor);
            gr.SmoothingMode = SmoothingMode.AntiAlias;












            if (bool_Pan == true & mLines.Count > 0)
            {


                Matrix myMatrix = new Matrix();


                myMatrix.Translate(Location_Move.X - left * GridX, Location_Move.Y - top * GridX, MatrixOrder.Append);

                gr.Transform = myMatrix;

            }


            if (ZOOMPlus_bool == true & mLines.Count > 0)
            {

                Matrix myMatrix = new Matrix();
                myMatrix.Scale((float)PictureScale, (float)PictureScale, MatrixOrder.Append);


                gr.Transform = myMatrix;


                if (PictureScale < 1)
                {
                    Size size = new Size(Convert.ToInt32((float)(1 / PictureScale) * PictureBox1.Width), Convert.ToInt32((float)(1 / PictureScale) * PictureBox1.Height));
                    PictureBox1.Size = size;


                }




            }



            if (bool_draw_DOF == true)
            {

                draw_grid(PictureBox1, gr, GridX, GridX);

                for (int i = 0; i < mLines.Count; i++)
                {
                    gr.DrawLine(new Pen(Color.Black, 1.0F), mLines[i].StartPoint, mLines[i].EndPoint);
                    DrawArrow(new Pen(Color.Black, 1.0F), mLines[i].StartPoint.X, mLines[i].StartPoint.Y, (mLines[i].StartPoint.X + mLines[i].EndPoint.X) / 2, (mLines[i].StartPoint.Y + mLines[i].EndPoint.Y) / 2, 10, gr);

                }
                for (int i = 1; i <= totalnode_From_Lines; i++)
                {

                    draw_tDOF(i, gr);

                }

                for (int j = 1; j <= totalnode_From_Lines; j++)
                {
                    draw_tDOF_HINGE_AFTER_Read_Input_Data_Entered(j, gr);
                }


            }





            if (Draw_Model == true)
            {


                gr.Clear(PictureBox1.BackColor);
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                draw_In_Pbox(gr);
            }


            if (selln == true & mLines.Count > 0)
            {
                draw_Selected_Member(mLines, gr);

            }




            if (selpt == true)
            {
                Draw_selected_Node(gr);

            }


            draw_grid(PictureBox1, gr, GridX, GridX);

            if (clk % 2 == 0 & clk > 1)
            {
                gr.DrawLine(new Pen(Color.Magenta, 4.0F), Location_1, Location_Move);
            }
            if (clk % 2 != 0 & clk > 2)
            {

            }

            for (int i = 0; i < m_Lines1.Count; i++)
            {
                gr.DrawLine(new Pen(Color.Gray, 1.0F), m_Lines1[i].StartPoint, m_Lines1[i].EndPoint);


            }







            if (drawln == true)
            {

                draw_grid(PictureBox1, gr, GridX, GridX);

                if (clk % 2 == 0 & clk > 1)
                {
                    gr.DrawLine(new Pen(Color.Magenta, 4.0F), Location_1, Location_Move);
                }
                if (clk % 2 != 0 & clk > 2)
                {

                }

                for (int i = 0; i < m_Lines1.Count; i++)
                {
                    gr.DrawLine(new Pen(Color.Gray, 1.0F), m_Lines1[i].StartPoint, m_Lines1[i].EndPoint);


                }


            }





            if (View_Shear == true || View_Mom == true || View_Theta == true || View_Delta == true || View_AxialForce == true)
            {
                gr.Clear(PictureBox1.BackColor);



                for (int i = 1; i <= totalmember; i++)
                {


                    Draw_Diagramsx(i, gr);

                }

                if (bool_SUPPORT_from_Read == true)
                {
                    for (int j = 1; j <= totalnode; j++)
                    {
                        Draw_sup(Sup_Node_Number_Type[j], gr);

                    }
                }
                if (bool_SUPPORT_from_Input == true)
                {

                    for (int j = 1; j <= totalnode; j++)
                    {
                        Draw_sup(Sup_Node_Number_Type2[j], gr);

                    }
                }


            }





            if (bool_draw_Disp_Diagram == true)
            {

                gr.SmoothingMode = SmoothingMode.AntiAlias;
                for (int i = 0; i < mLines.Count; i++)
                {
                    gr.DrawLine(new Pen(Color.Gray, 0), mLines[i].StartPoint, mLines[i].EndPoint);


                }
                if (selln == true & mLines.Count > 0)
                {
                    draw_Selected_Member(mLines, gr);

                }
            }




            if (bool_View_Member_Number == true) draw_Member_number(mLines, gr);

            if (bool_View_Node_Number == true) draw_Node_number(gr);










            draw_Selected_Member_Hover(mLines, Location_Move, gr);
            Draw_selected_Node_Hover(Location_Move, gr);



            if (bool_clear_picbox == true)
            {

                gr.Clear(PictureBox1.BackColor);
                draw_grid(PictureBox1, gr, GridX, GridX);
            }


        }




        private void Draw_selected_Node_Hover(PointF ptf, Graphics g)
        {
            double[] dist = new double[totalnode + 2];
            for (int i = 1; i <= totalnode; i++)
            {



                dist[i] = Math.Sqrt(Math.Pow(nodecoordinate[i].X - ptf.X, 2) + Math.Pow(nodecoordinate[i].Y - ptf.Y, 2));
                if (dist[i] <= 7)
                {


                    int radius = 3;
                    g.DrawEllipse(new Pen(Color.Green, 6.0F), ptf.X - radius, ptf.Y - radius,
                  radius + radius, radius + radius);


                }
            }

        }









        private void Draw_selected_Node(Graphics g)
        {
            double[] dist = new double[totalnode + 2];
            for (int i = 1; i <= totalnode; i++)
            {



                dist[i] = Math.Sqrt(Math.Pow(nodecoordinate[i].X - ptSelNode.X, 2) + Math.Pow(nodecoordinate[i].Y - ptSelNode.Y, 2));
                if (dist[i] <= 7)
                {


                    int radius = 3;
                    g.DrawEllipse(new Pen(Color.Red, 6.0F), ptSelNode.X - radius, ptSelNode.Y - radius,
                  radius + radius, radius + radius);
                    nodenumer_Selected = i;


                }
            }

        }

        public void Find_Selected_Member(List<line> mLinesy1)
        {



            for (int i = 0; i < mLinesy1.Count; i++)
            {

                double[] d = new double[totalmember + 2];
                d[i] = FindDistanceFromPointToSegment(ptSelln, mLinesy1[i].StartPoint, mLinesy1[i].EndPoint);


                if (d[i] <= 7)
                {


                    Last_index_line_selected = i;



                }



            }

        }






        public void draw_Selected_Member(List<line> mLinesy1, Graphics g)
        {


            for (int i = 0; i < mLinesy1.Count; i++)
            {

                double[] d = new double[101];
                d[i] = FindDistanceFromPointToSegment(ptSelln, mLinesy1[i].StartPoint, mLinesy1[i].EndPoint);



                if (d[i] <= 7)
                {

                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawLine(new Pen(Color.Red, 4), mLinesy1[i].StartPoint, mLinesy1[i].EndPoint);

                    Last_index_line_selected = i;


                }


            }

        }






        public void draw_Selected_Member_Hover(List<line> mLinesy1, PointF ptf, Graphics g)
        {


            for (int i = 0; i < mLinesy1.Count; i++)
            {

                double[] d = new double[101];
                d[i] = FindDistanceFromPointToSegment(ptf, mLinesy1[i].StartPoint, mLinesy1[i].EndPoint);



                if (d[i] <= 7)
                {

                    float[] dashValues = { 4, 4 };
                    Pen p = new Pen(Color.Black, 2);
                    p.DashPattern = dashValues;
                    var dottedPen = new Pen(Color.Gray, 2) { DashPattern = new[] { 2f, 2f } };
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawLine(p, mLinesy1[i].StartPoint, mLinesy1[i].EndPoint);


                }

            }

        }


        private double FindDistanceFromPointToSegment(PointF pt, PointF p1, PointF p2)
        {

            double d = 100;

            PointF closest = p1;
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0))
            {

                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }


            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) / (dx * dx + dy * dy);


            if (t < 0)
            {
                closest = new PointF(p1.X, p1.Y);
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;

            }
            else if (t > 1)
            {
                closest = new PointF(p2.X, p2.Y);
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;

            }
            else
            {
                closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;

            }

            if (t > 0.3 & t < 0.7)
            {
                d = Math.Sqrt(dx * dx + dy * dy);

            }

            return d;

        }

        private void draw_In_Pbox(Graphics g)
        {
            g.Clear(PictureBox1.BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            for (int i = 0; i < mLines.Count; i++)
            {
                g.DrawLine(new Pen(Color.Gray, 0), mLines[i].StartPoint, mLines[i].EndPoint);


            }


            if (bool_SUPPORT_from_Read == true)
            {
                for (int j = 1; j <= totalnode; j++)
                {
                    Draw_sup(Sup_Node_Number_Type[j], g);

                }
            }
            if (bool_SUPPORT_from_Input == true)
            {

                for (int j = 1; j <= totalnode; j++)
                {
                    Draw_sup(Sup_Node_Number_Type2[j], g);

                }
            }



            if (GridX > 15)
            {
                draw_grid(PictureBox1, g, GridX, GridY);
            }


            for (int j = 1; j <= totalmember; j++)
            {
                if (bool_Mem_Load_Pload_from_Read == true)
                {

                    if (!string.IsNullOrEmpty(Pload[j]))
                    {

                        Draw_Force_Mem_Pconc_UDL_MCon(j, Pload[j], g);

                    }
                }
            }

            for (int j = 1; j <= totalmember; j++)
            {
                if (bool_Mem_Load_Pload_from_Input == true)
                {
                    if (!string.IsNullOrEmpty(Pload2[j]))
                    {

                        Draw_Force_Mem_Pconc_UDL_MCon(j, Pload2[j], g);


                    }
                }

            }
            if (bool_Node_Load_Joint_from_Read == true)
            {
                for (int j = 1; j <= totalnode; j++)
                {

                    if (!string.IsNullOrEmpty(HINGE_String[j]))
                    {
                        Draw_HINGE(HINGE_String[j], g);
                    }
                }
            }

            if (bool_Hinge_from_Input == true)
            {
                for (int j = 1; j <= totalnode; j++)
                {

                    if (!string.IsNullOrEmpty(HINGE_String2[j]))
                    {
                        Draw_HINGE(HINGE_String2[j], g);
                    }
                }
            }

            if (bool_Node_Load_Joint_from_Read == true)
            {
                for (int j = 1; j <= totalnode; j++)
                {
                    Draw_ForceArrow_Mom_ALL_Node(PJoint[j], g);
                }
            }
            if (bool_Node_Load_Joint_from_Input == true)
            {
                for (int j = 1; j <= totalnode; j++)
                {
                    Draw_ForceArrow_Mom_ALL_Node(PJoint2[j], g);
                }
            }



        }







        private bool Find_PointF_Exists_ON_Lines(PointF pt, List<line> mLinesy)
        {
            bool bool_Find_PointF_Exists = false;
            for (int j = 0; j <= mLinesy.Count - 1; j++)
            {
                if (pt == mLinesy[j].EndPoint | pt == mLinesy[j].StartPoint)
                {
                    bool_Find_PointF_Exists = true;
                }

            }

            return bool_Find_PointF_Exists;

        }









        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {


            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {

                ZOOMPlus_bool = false;
                PictureScale = 1.0F;




                Size size = new Size(Convert.ToInt32(PictureScale * 2500), Convert.ToInt32(PictureScale * 1500));
                PictureBox1.Size = size;
                PictureBox1.Invalidate();
            }




            if (selpt == true)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {

                    ptSelNode = e.Location;


                    PictureBox1.Invalidate();
                }
            }


            if (selln == true)
            {

                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {

                    ptSelln = e.Location;
                    PictureBox1.Invalidate();
                    Find_Selected_Member(mLines);

                }
                if (Number_of_Selected_line == 2 | Number_of_Selected_line > 2)
                {


                    MessageBox.Show("Plese select one beam at time by clicking cntral part of a beam!!!");
                    Number_of_Selected_line = 0;
                }

            }




            if (drawln == true)
            {

                bool Beam_ON_Beam = false;
                bool crossBeam_Not_Coliniar = false;
                bool Dupliate_Structure_Meassage = true;
                bool areEquivalent = false;
                bool LineSegment_overlap_Contain_LineSegment_Colinear = false;
                bool Dupliate_Structure = false;
                bool bool_PointF_Exists_ON_Lines = true;
                int[] a = null, b = null;

                int x11, y11, x22, y22;

                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {

                    PointF Loc1 = e.Location;
                    PointF Loc2 = e.Location;
                    clk = clk + 1;

                    if (clk % 2 == 0)
                    {

                        int x = e.X;
                        int y = e.Y;
                        SnapToGrid(ref x, ref y);
                        Location_1 = new Point(x, y);


                        Temp1 = new line();
                        Temp1.StartPoint = Location_1;
                    }
                    if (clk % 2 != 0)
                    {

                        int x = e.X;
                        int y = e.Y;
                        SnapToGrid(ref x, ref y);
                        Location_2 = new Point(x, y);



                        if (Location_1 != Location_2)
                        {
                            Temp1.EndPoint = Location_2;

                            for (int k = 0; k <= m_Lines1.Count - 1; k++)
                            {
                                //Debug.Print(k + "= k   m_Lines1.Count= " + m_Lines1.Count);

                                PointF ptx1 = m_Lines1[k].StartPoint;
                                PointF ptx2 = m_Lines1[k].EndPoint;

                                int x1 = Convert.ToInt32(m_Lines1[k].StartPoint.X), y1 = Convert.ToInt32(m_Lines1[k].StartPoint.Y), x2 = Convert.ToInt32(m_Lines1[k].EndPoint.X), y2 = Convert.ToInt32(m_Lines1[k].EndPoint.Y);
                                x11 = Convert.ToInt32(Temp1.StartPoint.X);
                                y11 = Convert.ToInt32(Temp1.StartPoint.Y);
                                x22 = Convert.ToInt32(Temp1.EndPoint.X);
                                y22 = Convert.ToInt32(Temp1.EndPoint.Y);

                                a = new int[] { x1, y1, x2, y2 };
                                b = new int[] { x11, y11, x22, y22 };


                                bool_PointF_Exists_ON_Lines = Find_PointF_Exists_ON_Lines(Temp1.StartPoint, m_Lines1);


                                areEquivalent = a.SequenceEqual(b);

                                int count = 0;

                                if (doIntersect_Not_Coliniar(ptx1, ptx2, Temp1.StartPoint, Temp1.EndPoint) & ptx1 != Temp1.StartPoint & ptx2 != Temp1.StartPoint & ptx1 != Temp1.EndPoint & ptx2 != Temp1.EndPoint)
                                {
                                    count = count + 1;

                                    crossBeam_Not_Coliniar = true;
                                    Beam_ON_Beam = true;
                                }
                                if (doIntersect(ptx1, ptx2, Temp1.StartPoint, Temp1.EndPoint))
                                {
                                    LineSegment_overlap_Contain_LineSegment_Colinear = true;

                                }
                                else
                                {
                                    LineSegment_overlap_Contain_LineSegment_Colinear = false;

                                }

                                if (m_Lines1.Count > 0 & (ARRAY_ARE_Equal(a, b) == true | LineSegment_overlap_Contain_LineSegment_Colinear == true))
                                {


                                    Dupliate_Structure_Meassage = false;
                                    Beam_ON_Beam = true;

                                }

                                if (m_Lines1.Count > 0 & LineSegIsolated(Temp1, m_Lines1) == true)
                                {



                                    Dupliate_Structure = true;


                                }


                            }


                            if (Beam_ON_Beam == true)
                            {

                                MessageBox.Show("Member on Member/Node not accepted!!!");
                            }

                            if (Dupliate_Structure == true & Dupliate_Structure_Meassage == true)
                            {
                                MessageBox.Show("More than one structure!!!!");

                            }


                            bool secend_Line = true;

                            if (m_Lines1.Count == 0)
                            {

                                m_Lines1.Add(Temp1);
                                Temp1 = null;
                                secend_Line = false;

                            }
                            if (bool_PointF_Exists_ON_Lines == false)
                            {
                                MessageBox.Show("A beam or member must start from drawn end point of beam or member!!!");
                            }
                            if (m_Lines1.Count > 0 & secend_Line)
                            {

                                if (!(Beam_ON_Beam == true | Dupliate_Structure == true | crossBeam_Not_Coliniar == true))
                                {


                                    if (bool_PointF_Exists_ON_Lines == true)
                                    {
                                        m_Lines1.Add(Temp1);
                                        Temp1 = null;

                                    }

                                }
                            }


                        }

                    }



                    mLines = m_Lines1;
                    if (mLines.Count > 0)
                    {

                        Line_To_Node_AND_Connectivity(mLines);
                    }

                }


                PictureBox1.Invalidate();
            }

        }




        private bool LineSegIsolated(line tmp, List<line> mLinesx)
        {
            bool IsLineSegIsolated = true;
            int[] ab = new int[mLinesx.Count + 1];
            try
            {
                Array.Clear(ab, 0, ab.Length);
                for (int k = 0; k <= mLinesx.Count - 1; k++)
                {
                    if (mLinesx.Count > 0 && tmp.StartPoint != mLinesx[k].StartPoint && tmp.EndPoint != mLinesx[k].StartPoint && tmp.StartPoint != mLinesx[k].EndPoint && tmp.EndPoint != mLinesx[k].EndPoint)
                    {
                        ab[k] = 1;
                    }

                    else
                    {
                        ab[k] = 2;
                    }


                    for (int kk = 0; kk <= ab.Length - 1; kk++)
                    {
                        if (ab[kk] == 2)
                        {
                            IsLineSegIsolated = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                Debug.Print(" private bool LineSegIsolated " + Environment.NewLine + ex.ToString());
            }
            return IsLineSegIsolated;
        }





        private Boolean onSegment(PointF p, PointF q, PointF r)
        {



            bool point_online_seg = false;

            if ((p.Y - r.Y) / (double)(p.X - r.X) != 0)
            {
                if (q.X < Math.Max(p.X, r.X) && q.X > Math.Min(p.X, r.X) &&
                    q.Y < Math.Max(p.Y, r.Y) && q.Y > Math.Min(p.Y, r.Y))
                {
                    point_online_seg = true;
                }
            }

            if ((p.Y - r.Y) == 0)
            {
                if (q.X < Math.Max(p.X, r.X) && q.X > Math.Min(p.X, r.X))
                {
                    point_online_seg = true;
                }
            }

            if ((p.X - r.X) == 0)
            {
                if (q.Y < Math.Max(p.Y, r.Y) && q.Y > Math.Min(p.Y, r.Y))
                {
                    point_online_seg = true;
                }
            }
            return point_online_seg;

        }




        private int orientation(PointF p, PointF q, PointF r)
        {

            int val = Convert.ToInt32((q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y));

            if (val == 0) return 0;


            if (val > 0) return 1;



            if (val < 0) return 2;

            return 5;


        }


        private Boolean doIntersect(PointF p1, PointF q1, PointF p2, PointF q2)
        {

            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);


            if (o1 == 0 && onSegment(p1, p2, q1)) return true;


            if (o2 == 0 && onSegment(p1, q2, q1)) return true;


            if (o3 == 0 && onSegment(p2, p1, q2)) return true;


            if (o4 == 0 && onSegment(p2, q1, q2)) return true;

            return false;
        }






        private Boolean doIntersect_Not_Coliniar(PointF p1, PointF q1, PointF p2, PointF q2)
        {

            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);


            if (o1 != o2 && o3 != o4)


                return true;



            return false;
        }



        private Boolean Line_Contains_Point(PointF p, PointF q, PointF r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;

            return false;
        }




        private int Area_Of_Triangle(PointF p, PointF q, PointF r)
        {
            return Convert.ToInt32((q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y));
        }




        private bool ARRAY_ARE_Equal(int[] arr1, int[] arr2)
        {

            bool Equivalent = false;

            try
            {

                int n = arr1.Length;
                int m = arr2.Length;



                if (n != m)
                    Equivalent = false;


                for (int i = 0; i < n; i++)
                    if (arr1[i] != arr2[i])
                        Equivalent = false;



                int count = 0;

                if (arr1.Length > 0 & arr2.Length > 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (arr1[i] == arr2[i])
                        {
                            count = count + 1;

                        }
                    }
                    if (count == n) Equivalent = true;
                }


            }
            catch (Exception ex)
            {

                Debug.Print("private bool ARRAY_ARE_Equal " + Environment.NewLine + ex.ToString());
            }

            if (arr1.Length == 0 | arr2.Length == 0)
            {
                Equivalent = true;


            }

            return Equivalent;

        }

        private bool ARRAY_ARE_Equivalent22(int[] arr1, int[] arr2)
        {
            try
            {
                int n = arr1.Length;
                int m = arr2.Length;


                if (n != m)
                    return false;


                Array.Sort(arr1);
                Array.Sort(arr2);


                for (int i = 0; i < n; i++)
                    if (arr1[i] != arr2[i])
                        return false;




            }
            catch (Exception ex)
            {

                Debug.Print("private bool ARRAY_ARE_Equivalent22   " + Environment.NewLine + ex.ToString());
            }

            return true;
        }



        private bool ARRAY_ARE_Equivalent2(int[] arr1, int[] arr2)
        {
            bool Equivalent = false;

            try
            {

                int n = arr1.Length;
                int m = arr2.Length;


                if (n != m)
                    return false;
                if (arr1.Length == 0 || arr2.Length == 0) return false;


                if (arr1.Length > 0 & arr2.Length > 0)
                {
                    int[] c = Uncommon_Matrix_1D_1D_Intger(arr1, arr2);
                    int[] d = Uncommon_Matrix_1D_1D_Intger(arr2, arr1);

                    if (c.Length == 0 & d.Length == 0) Equivalent = true;

                }



            }
            catch (Exception ex)
            {

                Debug.Print("private bool ARRAY_ARE_Equivalent2   " + Environment.NewLine + ex.ToString());
            }

            if (arr1 == null | arr2 == null) Equivalent = true;
            {



            }
            if (!(arr1 != null && arr1.Length != 0 && arr2 != null && arr2.Length != 0)) Equivalent = true;

            {

            }

            return Equivalent;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            readFileToolStripMenuItem.PerformClick();


        }

        private void toolStripButton_resize(ToolStripButton tb)
        {

            int a = 40;
            int h = 40;
            ToolStrip1.AutoSize = false;

            tb.AutoSize = false; tb.Width = a; tb.Height = h;


            int sourceWidth = tb.Image.Width;
            int sourceHeight = tb.Image.Height;
            Bitmap b = new Bitmap(a, h);
            using (Graphics g = Graphics.FromImage((Image)b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(tb.Image, 0, 0, a, h);
            }
            Image myResizedImg = (Image)b;


            tb.Image = myResizedImg;
            ToolStrip1.ImageScalingSize = new Size(a, h);
        }




        //============================================

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {



            if (toolStripComboBox1.SelectedIndex == 0) { read_Example_DRAW(0); }

            if (toolStripComboBox1.SelectedIndex == 1) { read_Example_DRAW(1); }
            if (toolStripComboBox1.SelectedIndex == 2) { read_Example_DRAW(2); }
            if (toolStripComboBox1.SelectedIndex == 3) { read_Example_DRAW(3); }

            if (toolStripComboBox1.SelectedIndex == 4) { read_Example_DRAW(4); }
            if (toolStripComboBox1.SelectedIndex == 5) { read_Example_DRAW(5); }
            if (toolStripComboBox1.SelectedIndex == 6) { read_Example_DRAW(6); }

            if (toolStripComboBox1.SelectedIndex == 7) { read_Example_DRAW(7); }
            if (toolStripComboBox1.SelectedIndex == 8) { read_Example_DRAW(8); }
            if (toolStripComboBox1.SelectedIndex == 9) { read_Example_DRAW(9); }

            if (toolStripComboBox1.SelectedIndex == 10) { read_Example_DRAW(10); }
            if (toolStripComboBox1.SelectedIndex == 11) { read_Example_DRAW(11); }
            if (toolStripComboBox1.SelectedIndex == 12) { read_Example_DRAW(12); }
            if (toolStripComboBox1.SelectedIndex == 13) { read_Example_DRAW(13); }
            if (toolStripComboBox1.SelectedIndex == 14) { read_Example_DRAW(14); }
            if (toolStripComboBox1.SelectedIndex == 15) { read_Example_DRAW(15); }
            if (toolStripComboBox1.SelectedIndex == 16) { read_Example_DRAW(16); }
            if (toolStripComboBox1.SelectedIndex == 17) { read_Example_DRAW(17); }

            if (toolStripComboBox1.SelectedIndex == 18) { toolStripComboBox1.DroppedDown = false; }


            fILEToolStripMenuItem.HideDropDown();
        }

        //=====================================================

        //======================================================
        private void Form1_Load(object sender, EventArgs e)
        {

         
            //==================================================
            toolStripComboBox1.Size = new Size(250, 26);
            toolStripComboBox1.Text = "SELECT EXAMPLES";

            toolStripComboBox1.Items.Add("Single span beam with no DOF");//0
            toolStripComboBox1.Items.Add("Multi span beam");//1


            toolStripComboBox1.Items.Add("Single storied frame");//2
            toolStripComboBox1.Items.Add("2 storied frame");//3
            toolStripComboBox1.Items.Add("Single storied portal frame-1 LY");//4
            toolStripComboBox1.Items.Add("Single storied portal frame-2 GY");//5
            toolStripComboBox1.Items.Add("Multi storied frame");//6
            toolStripComboBox1.Items.Add("Multi bay frame");//7

            toolStripComboBox1.Items.Add("Spring supported frame -1");//8
            toolStripComboBox1.Items.Add("Spring supported frame -2");//9

            toolStripComboBox1.Items.Add("Inclined Roller supported frame -1");//10
            toolStripComboBox1.Items.Add("Inclined Roller supported frame -2");//11
            toolStripComboBox1.Items.Add("Inclined Roller supported frame -3");//12


            toolStripComboBox1.Items.Add("Frame with Intermediate Hinge -1");//13
            toolStripComboBox1.Items.Add("Frame with Intermediate Hinge -2");//14

            toolStripComboBox1.Items.Add("Frame with Support displacement");//15

            toolStripComboBox1.Items.Add("Frame HINGE with Support displacement");//16

            toolStripComboBox1.Items.Add("Guided sliding FY free support");//17

            toolStripComboBox1.Items.Add("CLOSE EXAMPLE");//18
            //==================================================








            //==================================================

            postAnalysisToolStripMenuItem.Enabled = false;
            viewPostAnalysisToolStripMenuItem.Enabled = false;



            foreach (ToolStripButton button in this.ToolStrip1.Items.OfType<ToolStripButton>())
            {
                toolStripButton_resize(button);
                if (button != sender)
                {

                }
            }


            this.KeyPreview = true;


            segment2 = 120;
            segment3 = 120;





            Array.Resize(ref Mproperty_A_I_E, 55 + 1);
            for (int k = 1; k <= 55; k++)
            {
                Mproperty_A_I_E[k] = k + "," + "0.0225,4.21875E-05,200000000";



            }




            Array.Resize(ref Pload2, 55 + 1);
            for (int u = 1; u <= 55; u++)
            {
                {

                    Pload2[u] = u + ",0,0,0,0,0,0,0,0,0,0,0,0,0";
                }
            }

            HINGE_String2 = new string[10];
            PJoint2 = new string[1];
            Sup_Node_Number_Type2 = new string[1];
            Support_Displacement_S = new string[1];

            HINGE_String = new string[1];
            PJoint = new string[1];
            Sup_Node_Number_Type = new string[1];
            Support_Displacement_S = new string[1];




            ZOOMPlus_bool = false;
            Test_PictureBox1_Paint = false;



            Test_PictureBox1_Paint = false;



            ZOOMPlus_bool = false;
            bool_View_Force_Value = true;

            bool_Mem_Load_Pload_from_Read = false;
            bool_Mem_Load_Pload_from_Input = true;

            bool_Node_Load_Joint_from_Read = false;
            bool_Node_Load_Joint_from_Input = true;

            bool_SUPPORT_from_Read = false;
            bool_SUPPORT_from_Input = true;

            bool_Hinge_from_Read = false;
            bool_Hinge_from_Input = true;



            selln = false;
            down = false;
            down2 = false;
            drawln = false;
            button17.Visible = false;
            Last_index_line_selected = -1;
            nodenumer_Selected = -1;




            ////================================================
            clk = 1;



            Panel1.BorderStyle = BorderStyle.FixedSingle;

            Panel1.AutoScroll = true;


            PictureBox1.Parent = Panel1;


            Size size = new Size(Convert.ToInt32(PictureScale * 2500), Convert.ToInt32(PictureScale * 1500));

            PictureBox1.Size = size;

            PictureBox1.Invalidate();

            GridX = 30;
            GridY = GridX;



            //=======================================



        }



        private double B_Sing(double x, double a, int n)
        {

            double Sing_Func = 0;


            if ((x - a) >= 0 && n > 0)
            {
                Sing_Func = Math.Pow(x - a, n);
            }
            else if ((x - a) >= 0 && n == 0)
            {
                Sing_Func = 1;
            }
            else if ((x - a) < 0 || n < 0)
            {
                Sing_Func = 0;
            }

            return Sing_Func;

        }
        private double[] UDL_TRAP_W0_W1_Fun(double WW0, double WW1, double XX0, double XX1, double X, double EYN_I)
        {
            double[] TRAP = new double[5];

            double W_X = 0;

            if (XX0 - XX1 != 0)
            {
                W_X = (WW0 - WW1) / (XX0 - XX1);
            }
            else
            {
                W_X = 0;
            }

            TRAP[1] = -WW0 * B_Sing(X, XX0, 1) - (W_X / 2) * B_Sing(X, XX0, 2) + WW1 * B_Sing(X, XX1, 1) + (W_X / 2) * B_Sing(X, XX1, 2);

            TRAP[2] = -(WW0 / 2) * B_Sing(X, XX0, 2) - (W_X / 6) * B_Sing(X, XX0, 3) + (WW1 / 2) * B_Sing(X, XX1, 2) + (W_X / 6) * B_Sing(X, XX1, 3);

            TRAP[3] = (1 / EYN_I) * (-(WW0 / 6) * B_Sing(X, XX0, 3) - (W_X / 24) * B_Sing(X, XX0, 4) + (WW1 / 6) * B_Sing(X, XX1, 3) + (W_X / 24) * B_Sing(X, XX1, 4));

            TRAP[4] = (1 / EYN_I) * (-(WW0 / 24) * B_Sing(X, XX0, 4) - (W_X / 120) * B_Sing(X, XX0, 5) + (WW1 / 24) * B_Sing(X, XX1, 4) + (W_X / 120) * B_Sing(X, XX1, 5));


            return TRAP;

        }





        private double[] PConc_Fun(double PCon1, double XPCon1, double PCon2, double XPCon2, double PCon3, double XPCon3, double X, double EYN_I)
        {

            double[] PConcentrated = new double[5];





            PConcentrated[1] = -PCon1 * B_Sing(X, XPCon1, 0) - PCon2 * B_Sing(X, XPCon2, 0) - PCon3 * B_Sing(X, XPCon3, 0);



            PConcentrated[2] = -PCon1 * B_Sing(X, XPCon1, 1) - PCon2 * B_Sing(X, XPCon2, 1) - PCon3 * B_Sing(X, XPCon3, 1);


            PConcentrated[3] = -(PCon1 / 2) * B_Sing(X, XPCon1, 2) / EYN_I - (PCon2 / 2) * B_Sing(X, XPCon2, 2) / EYN_I - (PCon3 / 2) * B_Sing(X, XPCon3, 2) / EYN_I;



            PConcentrated[4] = -(PCon1 / 6) * B_Sing(X, XPCon1, 3) / EYN_I - (PCon2 / 6) * B_Sing(X, XPCon2, 3) / EYN_I - (PCon3 / 6) * B_Sing(X, XPCon3, 3) / EYN_I;


            return PConcentrated;



        }
        private double[] MCon_Fun(double MCon, double XMCon, double X, double EYN_I)
        {

            double[] MConcentrated = new double[5];



            MConcentrated[1] = MCon * B_Sing(X, XMCon, -1);


            MConcentrated[2] = MCon * B_Sing(X, XMCon, 0);



            MConcentrated[3] = MCon * B_Sing(X, XMCon, 1) / EYN_I;


            MConcentrated[4] = (MCon / 2) * B_Sing(X, XMCon, 2) / EYN_I;
            return MConcentrated;
        }




        //=================================================

        private double[,] FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal(int Memb_id, string St_memLoad)
        {



            double[] temp12 = new double[6];

            double[] Flocal = new double[6];
            double[,] Flocal2D = new double[totalmember + 1, 6];



            int j = 0;
            int kk = 0;

            double Pcn_1 = 0;
            double Xpcn_1 = 0;
            double Pcn_2 = 0;
            double Xpcn_2 = 0;
            double Pcn_3 = 0;
            double Xpcn_3 = 0;
            double W0_1 = 0;
            double W1_1 = 0;
            double Xw0_1 = 0;
            double Xw1_1 = 0;
            double MCn_1 = 0;
            double XMcn_1 = 0;
            string Ydirection = null;

            string[] test = { };


            if (!string.IsNullOrEmpty(St_memLoad))
            {
                test = St_memLoad.Split(new char[] { ',' });
                for (j = 0; j < test.Length; j++)
                {

                }





                kk = int.Parse(test[0]);

                Pcn_1 = -double.Parse(test[1]);
                Xpcn_1 = double.Parse(test[2]);
                Pcn_2 = -double.Parse(test[3]);
                Xpcn_2 = double.Parse(test[4]);
                Pcn_3 = -double.Parse(test[5]);
                Xpcn_3 = double.Parse(test[6]);

                W0_1 = -double.Parse(test[7]);
                W1_1 = -double.Parse(test[8]);
                Xw0_1 = double.Parse(test[9]);
                Xw1_1 = double.Parse(test[10]);

                MCn_1 = -double.Parse(test[11]);
                XMcn_1 = double.Parse(test[12]);
                Ydirection = test[13];

            }


            if (Math.Abs(cosx(Memb_id)) == 0 | Math.Abs(sinx(Memb_id)) == 0)
            {
                Ydirection = "LY";
            }


            double a1 = 0;
            double b1 = 0;
            double c1 = 0;
            double a2 = 0;
            double b2 = 0;
            double c2 = 0;
            double x1 = 0;
            double y1 = 0;

            double X = 0;
            double VVa = 0;
            double MMa = 0;
            double VVb = 0;
            double MMb = 0;
            double Eq1 = 0;
            double Eq2 = 0;
            double Eq3 = 0;
            double Eq4 = 0;
            double EyI = 0;
            double Eq11 = 0;
            double Eq21 = 0;
            double Eq31 = 0;
            double Eq41 = 0;
            double[] P_P = null;
            double[] M_M = null;
            double[] Trap_UDL = null;
            double L = L_n(Memb_id);

            double Trap_UDL_Area = 0, Trap_UDL_XCG_0 = 0, Trap_UDL_XCG_Xw0_1 = 0, AF_Trap_UDL_AT_0 = 0, AF_Trap_UDL_AT_L = 0;
            if (W0_1 != 0 | W1_1 != 0)
            {
                Trap_UDL_Area = (Xw1_1 - Xw0_1) * (W0_1 + W1_1) / 2;
                Trap_UDL_XCG_Xw0_1 = Xw0_1 + (Xw1_1 - Xw0_1) * (W0_1 + 2 * W1_1) / (W0_1 + W1_1) / 3;

                AF_Trap_UDL_AT_0 = Trap_UDL_Area * (L - Trap_UDL_XCG_Xw0_1) * sinx(Memb_id) / L;
                AF_Trap_UDL_AT_L = Trap_UDL_Area * Trap_UDL_XCG_Xw0_1 * sinx(Memb_id) / L;
            }
            double AF_P_AT_0 = (Pcn_1 * (L - Xpcn_1) + Pcn_2 * (L - Xpcn_2) + Pcn_3 * (L - Xpcn_3)) * sinx(Memb_id) / L;
            double AF_P_AT_L = (Pcn_1 * Xpcn_1 + Pcn_2 * Xpcn_2 + Pcn_3 * Xpcn_3) * sinx(Memb_id) / L;//P_P = PConc_Fun(Pcn_1, Xpcn_1, Pcn_2, Xpcn_2, Pcn_3, Xpcn_3, X, EyI);

            double AF_ALL_0 = AF_Trap_UDL_AT_0 + AF_P_AT_0;
            double AF_ALL_L = AF_Trap_UDL_AT_L + AF_P_AT_L;
            //Debug.Print(" AF_Trap_UDL_AT_0, L= " + AF_Trap_UDL_AT_0 + ", " + AF_Trap_UDL_AT_L + "," + Trap_UDL_XCG_0 + "," + Trap_UDL_Area);
            //Debug.Print(" AF_P_AT_0, L= " + AF_P_AT_0 + ", " + AF_P_AT_L );




            EyI = Ey[Memb_id] * Iz[Memb_id];
            if (EyI == 0)
            {
                MessageBox.Show("Material property unspecified");
            }

            X = L;


            if (Ydirection == "GY")
                if (Math.Abs(cosx(Memb_id)) != 0 | Math.Abs(sinx(Memb_id)) != 0)
                {

                    {

                        W0_1 = W0_1 * cosx(Memb_id);
                        W1_1 = W1_1 * cosx(Memb_id);
                        Pcn_1 = Pcn_1 * cosx(Memb_id);
                        Pcn_2 = Pcn_2 * cosx(Memb_id);
                        Pcn_3 = Pcn_3 * cosx(Memb_id);

                    }
                }





            Trap_UDL = UDL_TRAP_W0_W1_Fun(W0_1, W1_1, Xw0_1, Xw1_1, X, EyI);
            Eq11 = (Trap_UDL[1]);
            Eq21 = (Trap_UDL[2]);
            Eq31 = (Trap_UDL[3]) * EyI;
            Eq41 = (Trap_UDL[4]) * EyI;


            P_P = PConc_Fun(Pcn_1, Xpcn_1, Pcn_2, Xpcn_2, Pcn_3, Xpcn_3, X, EyI);


            M_M = MCon_Fun(MCn_1, XMcn_1, X, EyI);


            Eq3 = (P_P[3] + M_M[3] + Trap_UDL[3]) * EyI;
            Eq4 = (P_P[4] + M_M[4] + Trap_UDL[4]) * EyI;



            c1 = Eq3;
            c2 = Eq4;
            a1 = -1 / (double)2 * B_Sing(X, 0, 2);
            b1 = -B_Sing(X, 0, 1);

            a2 = -1 / (double)6 * B_Sing(X, 0, 3);
            b2 = -1 / (double)2 * B_Sing(X, 0, 2);



            x1 = (c1 * b2 - c2 * b1) / (a1 * b2 - a2 * b1);
            y1 = (c1 * a2 - a1 * c2) / (b1 * a2 - a1 * b2);

            VVa = x1;
            MMa = y1;

            Eq1 = (P_P[1] + M_M[1] + Trap_UDL[1]);
            Eq2 = (P_P[2] + M_M[2] + Trap_UDL[2]);
            VVb = (P_P[1] + M_M[1] + Trap_UDL[1]) + VVa * B_Sing(X, 0, 0) + MMa * B_Sing(X, 0, -1);
            MMb = (P_P[2] + M_M[2] + Trap_UDL[2]) + VVa * B_Sing(X, 0, 1) + MMa * B_Sing(X, 0, 0);









            // Debug.Print(L + " , " + Memb_id + "L,Memb_id FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal  EI = " + EyI + "Eq1 = " + Eq1 + " Eq2 = " + Eq2 + "Eq3*EI = " + Eq3 + " Eq4*EI = " + Eq4);



            // Debug.Print(L + " , " + Memb_id + "L,Memb_id FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal  memid = " + Memb_id + "Va Global=" + VVa + " Ma Global =" + -MMa + "Vb Global=" + -VVb + " Mb Global =" + MMb + Environment.NewLine);

            if (Ydirection == "GY")
            {
                //Debug.Print(" GY" + Ydirection.Equals("GY"));
                Flocal[0] = -AF_ALL_0;
                Flocal[1] = -VVa;
                Flocal[2] = MMa;
                Flocal[3] = -AF_ALL_L;
                Flocal[4] = VVb;
                Flocal[5] = -MMb;
            }


            if (Ydirection == "LY")
            {
                //Debug.Print(" LY" + Ydirection.Equals("LY"));
                Flocal[0] = 0;
                Flocal[1] = -VVa;
                Flocal[2] = MMa;
                Flocal[3] = 0;
                Flocal[4] = VVb;
                Flocal[5] = -MMb;
            }


            for (j = 0; j <= 5; j++)
            {
                Flocal2D[Memb_id, j] = Flocal[j];
            }

            // Debug.Print(L + " =L = " + Memb_id + "FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal " + Environment.NewLine + MakeDisplayable_1D_0_Double(Flocal));




            return Flocal2D;

        }

        //=================================







        private double[,] FORCE_Eqiv_Nod_MembComaP_ij_1_0_TO_Global(int Memb_id, string St_memLoad)
        {

            double[] temp12 = new double[6];
            double[,] mat1 = null;
            double[,] mat2 = null;
            double[] Flocal = new double[6];
            double[,] Flocal2 = null;
            double[] Fglobal = null;
            double[,] Flocal2D = new double[totalmember + 1, 6];

            if (!string.IsNullOrEmpty(St_memLoad))
            {

                if (Memb_id == int.Parse(St_memLoad.Substring(0, 1)))
                {
                    Flocal2 = FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal(Memb_id, St_memLoad);






                    mat1 = Trasform_mat_G_TO_L_ij_00(cosx(Memb_id), sinx(Memb_id));
                    mat2 = transposeMatrix(mat1);

                    Flocal[0] = Flocal2[Memb_id, 0];
                    Flocal[1] = Flocal2[Memb_id, 1];
                    Flocal[2] = Flocal2[Memb_id, 2];
                    Flocal[3] = Flocal2[Memb_id, 3];
                    Flocal[4] = Flocal2[Memb_id, 4];
                    Flocal[5] = Flocal2[Memb_id, 5];


                    Fglobal = Multiply_Matrix_2DBY1D_0_0(mat2, Flocal);

                }

            }
            for (int j = 0; j <= 5; j++)
            {
                Flocal2D[Memb_id, j] = Fglobal[j];
            }


            //Debug.Print(Memb_id + " FORCE_Eqiv_Nod_MembComaP_ij_1_0_TO_Global " + Environment.NewLine + MakeDisplayable_1D_0_Double(Flocal));

            return Flocal2D;

        }

        //===================================================




        public static string MakeDisplayable(double[,] sourceMatrix)
        {
            int rows = 0;
            int cols = 0;
            int eachRow = 0;
            int eachCol = 0;
            System.Text.StringBuilder result = new System.Text.StringBuilder();


            rows = sourceMatrix.GetUpperBound(0) + 1;
            cols = sourceMatrix.GetUpperBound(1) + 1;
            for (eachRow = 0; eachRow < rows; eachRow++)
            {

                if (eachRow > 0)
                {
                    result.AppendLine();
                }
                for (eachCol = 0; eachCol < cols; eachCol++)
                {

                    if (eachCol > 0)
                    {
                        result.Append(",");
                    }
                    result.Append(sourceMatrix[eachRow, eachCol].ToString());
                }
            }


            return result.ToString();
        }










        private double[,] SUBTRACT_MATRIX_2D2D_00(double[,] a, double[,] b)
        {
            int rowsA = a.GetLength(0);
            int colsA = a.GetLength(1);
            int rowsB = b.GetLength(0);
            int colsB = b.GetLength(1);

            // Check if the dimensions of both matrices are the same
            if (rowsA != rowsB || colsA != colsB)
            {
                MessageBox.Show("Matrix dimensions must match!");

            }

            // Create a new matrix to store the result, with the same dimensions
            double[,] result = new double[rowsA, colsA];

            // Perform element-wise subtraction
            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsA; j++)
                {
                    result[i, j] = a[i, j] - b[i, j];
                }
            }

            return result;
        }
        private double[,] SUBTRACT_MATRIX_2D2D_11(double[,] a, double[,] b)
        {
            int rowsA = a.GetLength(0);
            int colsA = a.GetLength(1);
            int rowsB = b.GetLength(0);
            int colsB = b.GetLength(1);

            // Check if the dimensions of both matrices are the same
            if (rowsA != rowsB || colsA != colsB)
            {
                MessageBox.Show("Matrix dimensions must match!");

            }

            // Create a new matrix to store the result, with the same dimensions
            double[,] result = new double[rowsA, colsA];

            // Perform element-wise subtraction
            for (int i = 1; i < rowsA; i++)
            {
                for (int j = 1; j < colsA; j++)
                {
                    result[i, j] = a[i, j] - b[i, j];
                }
            }

            return result;
        }

        private double[,] ADD_MATRIX_2D2D_00(double[,] a, double[,] b)
        {
            int rowsA = a.GetLength(0);
            int colsA = a.GetLength(1);
            int rowsB = b.GetLength(0);
            int colsB = b.GetLength(1);

            // Check if the dimensions of both matrices are the same
            if (rowsA != rowsB || colsA != colsB)
            {
                MessageBox.Show("Matrix dimensions must match!");

            }

            // Create a new matrix to store the result, with the same dimensions
            double[,] result = new double[rowsA, colsA];

            // Perform element-wise subtraction
            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsA; j++)
                {
                    result[i, j] = a[i, j] + b[i, j];
                }
            }

            return result;
        }
        private double[,] ADD_MATRIX_2D2D_11(double[,] a, double[,] b)
        {
            int rowsA = a.GetLength(0);
            int colsA = a.GetLength(1);
            int rowsB = b.GetLength(0);
            int colsB = b.GetLength(1);

            // Check if the dimensions of both matrices are the same
            if (rowsA != rowsB || colsA != colsB)
            {
                MessageBox.Show("Matrix dimensions must match!");

            }

            // Create a new matrix to store the result, with the same dimensions
            double[,] result = new double[rowsA, colsA];

            // Perform element-wise subtraction
            for (int i = 1; i < rowsA; i++)
            {
                for (int j = 1; j < colsA; j++)
                {
                    result[i, j] = a[i, j] + b[i, j];
                }
            }

            return result;
        }





        #region 222 Force_end_local_SingFunc















        private double[] FindMAX_Index_Double_1D(double[] array1)
        {

            double[] result = new double[2];

            int[] index = new int[array1.Length];

            for (int k = 1; k <= array1.Length - 1; k++)
            {
                index[k] = k;


            }


            for (int i = 1; i <= array1.Length; i++)
            {
                for (int j = 0; j < index.Length - 1; j++)
                {
                    if (array1[index[j]] > array1[index[j + 1]])
                    {
                        int tmp = index[j + 1];
                        index[j + 1] = index[j];
                        index[j] = tmp;

                    }

                }
            }


            Debug.WriteLine(index[array1.Length - 1] + " , " + array1[index[array1.Length - 1]]);
            result[0] = array1[index[array1.Length - 1]];
            result[1] = index[array1.Length - 1];
            return result;

        }











        private double FindMAX_Double_1D(double[] ptx)
        {

            int j = 0;
            int k = 0;

            int p = ptx.Length - 1;
            double[] ary = new double[p + 1];
            double[] pt = new double[p + 1];

            for (j = 0; j <= p; j++)
            {
                pt[j] = ptx[j];
            }



            for (j = 0; j <= p; j++)
            {
                for (k = 0; k < p; k++)
                {
                    if (pt[k] > pt[k + 1])
                    {
                        double temp1 = pt[k];
                        pt[k] = pt[k + 1];
                        pt[k + 1] = temp1;

                    }
                }
            }

            for (j = 0; j <= p; j++)
            {
                ary[j] = pt[j];
            }

            return ary[p];



        }
        private double FindMIN_Double_1D(double[] ptx)
        {

            int j = 0;
            int k = 0;

            int p = ptx.Length - 1;
            double[] ary = new double[p + 1];
            double[] pt = new double[p + 1];

            for (j = 0; j <= p; j++)
            {
                pt[j] = ptx[j];
            }



            for (j = 0; j <= p; j++)
            {
                for (k = 0; k < p; k++)
                {
                    if (pt[k] < pt[k + 1])
                    {
                        double temp1 = pt[k];
                        pt[k] = pt[k + 1];
                        pt[k + 1] = temp1;

                    }
                }
            }

            for (j = 0; j <= p; j++)
            {
                ary[j] = pt[j];
            }

            return ary[p];



        }
        #endregion


        #region 333 Kg3 with Shear
        private double[,] K_2_G_ElementShear_i_1_j_1(int Memb_ID)
        {



            double[,] Cmp = new double[0, 0];
            double[,] Cmp1 = new double[0, 0];
            double II1 = 0;
            double AA1 = 0;
            double EE1 = 0;
            double LL1 = 0;
            double c = 0;
            double s = 0;
            double[,] mat3 = new double[7, 7];
            int i = 0;
            int j = 0;
            int q = Memb_ID;
            LL1 = L_n(q);
            II1 = Iz[q];
            AA1 = Ayz[q];
            EE1 = Ey[q];
            c = cosx(q);
            s = sinx(q);


            double[,] TT1 = new double[,]
			{
				{c, -s, 0, 0, 0, 0},
				{s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, -s, 0},
				{0, 0, 0, s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};
            double alpha = 0;
            double Si = 0;
            double F = 0;
            double F2 = 0;
            double F4 = 0;
            alpha = 6 / 5.0;
            Si = 24 * alpha * (1 + nu) * Math.Pow(Math.Sqrt(II1 / AA1) / LL1, 2);
            Si = 0;
            F = 1 + Si;
            F2 = 1 - Si / 2;
            F4 = 1 + Si / 4;

            double[,] ML = new double[,]
			{
				{EE1 * AA1 / LL1, 0, 0, -EE1 * AA1 / LL1, 0, 0},
				{0, (12 * EE1 * II1) / (F * Math.Pow(LL1, 3)), (6 * EE1 * II1) / (F * Math.Pow(LL1, 2)), 0, -(12 * EE1 * II1) / (F * Math.Pow(LL1, 3)), (6 * EE1 * II1) / (F * Math.Pow(LL1, 2))},
				{0, (6 * EE1 * II1) / (F * Math.Pow(LL1, 2)), (4 * EE1 * II1 * F4) / LL1, 0, -(6 * EE1 * II1) / (F * Math.Pow(LL1, 2)), (2 * EE1 * II1 * F2) / LL1},
				{-EE1 * AA1 / LL1, 0, 0, EE1 * AA1 / LL1, 0, 0},
				{0, -(12 * EE1 * II1) / (F * Math.Pow(LL1, 3)), -(6 * EE1 * II1) / (F * Math.Pow(LL1, 2)), 0, (12 * EE1 * II1) / (F * Math.Pow(LL1, 3)), -(6 * EE1 * II1) / (F * Math.Pow(LL1, 2))},
				{0, (6 * EE1 * II1) / (F * Math.Pow(LL1, 2)), (2 * EE1 * II1 * F2) / LL1, 0, -(6 * EE1 * II1) / (F * Math.Pow(LL1, 2)), (4 * EE1 * II1 * F4) / LL1}
			};



            double[,] T1 = new double[,]
			{
				{c, s, 0, 0, 0, 0},
				{-s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, s, 0},
				{0, 0, 0, -s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};



            Cmp1 = Multiply_2D_2D_DiffSiz_ii_00(Multiply_2D_2D_DiffSiz_ii_00(TT1, ML), T1);


            double[,] cmp11 = new double[Cmp1.GetUpperBound(0) + 2, Cmp1.GetUpperBound(0) + 2];


            int tempVar = Cmp1.GetUpperBound(0);
            for (i = 0; i <= tempVar; i++)
            {
                for (j = 0; j <= Cmp1.GetUpperBound(1); j++)
                {

                    cmp11[i + 1, j + 1] = Cmp1[i, j];

                }
            }

            return cmp11;



        }

        #endregion




        public static double L_n(int mem)
        {

            double x_2 = 0;
            double x_1 = 0;
            double y_2 = 0;
            double y_1 = 0;
            double L_1 = 0;
            if (totalnode >= 2)
            {
                x_2 = nodecoordinate[Connectivity[mem, 2]].X;
                x_1 = nodecoordinate[Connectivity[mem, 1]].X;
                y_2 = nodecoordinate[Connectivity[mem, 2]].Y;
                y_1 = nodecoordinate[Connectivity[mem, 1]].Y;
                L_1 = Math.Sqrt(Math.Pow(x_2 - x_1, 2) + Math.Pow(y_2 - y_1, 2));
                L_1 = L_1 / (GridX);

            }
            return L_1 * lenght_factor;
        }






        private void SnapToGrid(ref int x, ref int y)
        {


            int x1 = Convert.ToInt32(((double)x / (double)GridX));
            int y1 = Convert.ToInt32(((double)y / (double)GridY));
            x = Convert.ToInt32(x1 * GridX);
            y = Convert.ToInt32(y1 * GridY);

        }


        private PointF SnapToGrid1(PointF pt)
        {

            PointF p = PointF.Empty;
            float X = pt.X;
            float Y = pt.Y;
            int ix = Convert.ToInt32(X / GridX);
            int iy = Convert.ToInt32(Y / GridY);
            X = (int)(ix * GridX);
            Y = (int)(iy * GridY);
            p.X = X;
            p.Y = Y;
            return p;


        }
        private void draw_grid(PictureBox pic, Graphics g, float ggx, float ggy)
        {

            PointF px = new PointF(0f, 0f);


            for (float x = 0; x <= PictureBox1.ClientSize.Width; x += GridX)
            {

                for (float y = 0; y <= PictureBox1.ClientSize.Height; y += GridX)
                {

                    px.X = x;
                    px.Y = y;



                    g.FillEllipse(new SolidBrush(Color.Black), px.X, px.Y, (float)(1 / PictureScale), (float)(4 / PictureScale));
                }
            }

        }
















        private double[,] K_Local_Element_Matrix_i_0_j_0(int Memb_ID)
        {
            int q = Memb_ID;
            double L = L_n(q);
            double I = Iz[q];
            double A = Ayz[q];
            double E = Ey[q];
            double c = cosx(q);
            double s = sinx(q);



            double[,] matrixD = new double[6, 6];







            matrixD[0, 0] = A * E / L;
            matrixD[0, 1] = 0;
            matrixD[0, 2] = 0;
            matrixD[0, 3] = -A * E / L;
            matrixD[0, 4] = 0;
            matrixD[0, 5] = 0;


            matrixD[1, 0] = 0;
            matrixD[1, 1] = 12 * E * I / Math.Pow(L, 3);
            matrixD[1, 2] = 6 * E * I / Math.Pow(L, 2);
            matrixD[1, 3] = 0;
            matrixD[1, 4] = -12 * E * I / Math.Pow(L, 3);
            matrixD[1, 5] = 6 * E * I / Math.Pow(L, 2);



            matrixD[2, 0] = 0;
            matrixD[2, 1] = 6 * E * I / Math.Pow(L, 2);
            matrixD[2, 2] = 4 * E * I / L;
            matrixD[2, 3] = 0;
            matrixD[2, 4] = -6 * E * I / Math.Pow(L, 2);
            matrixD[2, 5] = 2 * E * I / L;




            matrixD[3, 0] = -A * E / L;
            matrixD[3, 1] = 0;
            matrixD[3, 2] = 0;
            matrixD[3, 3] = A * E / L;
            matrixD[3, 4] = 0;
            matrixD[3, 5] = 0;




            matrixD[4, 0] = 0;
            matrixD[4, 1] = -12 * E * I / Math.Pow(L, 3);
            matrixD[4, 2] = -6 * E * I / Math.Pow(L, 2);
            matrixD[4, 3] = 0;
            matrixD[4, 4] = 12 * E * I / Math.Pow(L, 3);
            matrixD[4, 5] = -6 * E * I / Math.Pow(L, 2);


            matrixD[5, 0] = 0;
            matrixD[5, 1] = 6 * E * I / Math.Pow(L, 2);
            matrixD[5, 2] = 2 * E * I / L;
            matrixD[5, 3] = 0;
            matrixD[5, 4] = -6 * E * I / Math.Pow(L, 2);
            matrixD[5, 5] = 4 * E * I / L;

            return matrixD;
        }






        private double[,] K_Global_Element_Matrix_i_0_j_0(int Memb_ID)
        {
            int q = Memb_ID;
            double L = L_n(q);
            double I = Iz[q];
            double A = Ayz[q];
            double E = Ey[q];
            double c = cosx(q);
            double s = sinx(q);



            double[,] matrixD = new double[6, 6];


            double[,] TT1 = new double[,]
			{
				{c, -s, 0, 0, 0, 0},
				{s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, -s, 0},
				{0, 0, 0, s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};







            double[,] T1 = new double[,]
			{
				{c, s, 0, 0, 0, 0},
				{-s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, s, 0},
				{0, 0, 0, -s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};







            matrixD[0, 0] = A * E / L;
            matrixD[0, 1] = 0;
            matrixD[0, 2] = 0;
            matrixD[0, 3] = -A * E / L;
            matrixD[0, 4] = 0;
            matrixD[0, 5] = 0;


            matrixD[1, 0] = 0;
            matrixD[1, 1] = 12 * E * I / Math.Pow(L, 3);
            matrixD[1, 2] = 6 * E * I / Math.Pow(L, 2);
            matrixD[1, 3] = 0;
            matrixD[1, 4] = -12 * E * I / Math.Pow(L, 3);
            matrixD[1, 5] = 6 * E * I / Math.Pow(L, 2);



            matrixD[2, 0] = 0;
            matrixD[2, 1] = 6 * E * I / Math.Pow(L, 2);
            matrixD[2, 2] = 4 * E * I / L;
            matrixD[2, 3] = 0;
            matrixD[2, 4] = -6 * E * I / Math.Pow(L, 2);
            matrixD[2, 5] = 2 * E * I / L;




            matrixD[3, 0] = -A * E / L;
            matrixD[3, 1] = 0;
            matrixD[3, 2] = 0;
            matrixD[3, 3] = A * E / L;
            matrixD[3, 4] = 0;
            matrixD[3, 5] = 0;




            matrixD[4, 0] = 0;
            matrixD[4, 1] = -12 * E * I / Math.Pow(L, 3);
            matrixD[4, 2] = -6 * E * I / Math.Pow(L, 2);
            matrixD[4, 3] = 0;
            matrixD[4, 4] = 12 * E * I / Math.Pow(L, 3);
            matrixD[4, 5] = -6 * E * I / Math.Pow(L, 2);


            matrixD[5, 0] = 0;
            matrixD[5, 1] = 6 * E * I / Math.Pow(L, 2);
            matrixD[5, 2] = 2 * E * I / L;
            matrixD[5, 3] = 0;
            matrixD[5, 4] = -6 * E * I / Math.Pow(L, 2);
            matrixD[5, 5] = 4 * E * I / L;



            double[,] Cmp1 = Mult_2D_2D_Mat_00(Mult_2D_2D_Mat_00(TT1, matrixD), T1);

            return Cmp1;
        }




        private double[,] K_G_Element_ij_11(int Memb_ID)
        {

            int q = Memb_ID;
            double L = L_n(q);
            double I = Iz[q];
            double A = Ayz[q];
            double E = Ey[q];
            double c = cosx(q);
            double s = sinx(q);


            double[,] matrixD = new double[7, 7];

            Array.Clear(matrixD, 0, matrixD.Length);
            double k_1 = 0;
            double k_2 = 0;
            double k_3 = 0;
            double k_4 = 0;

            k_1 = E * A / L;
            k_2 = 12 * E * I / Math.Pow(L, 3);
            k_3 = E * I / L;
            k_4 = 6 * E * I / Math.Pow(L, 2);


            matrixD[1, 1] = Math.Pow(c, 2) * k_1 + Math.Pow(s, 2) * k_2;
            matrixD[1, 2] = s * c * (k_1 - k_2);
            matrixD[1, 3] = -s * k_4;
            matrixD[1, 4] = -(Math.Pow(c, 2) * k_1 + Math.Pow(s, 2) * k_2);
            matrixD[1, 5] = s * c * (-k_1 + k_2);
            matrixD[1, 6] = -s * k_4;

            matrixD[2, 1] = s * c * (k_1 - k_2);
            matrixD[2, 2] = Math.Pow(s, 2) * k_1 + Math.Pow(c, 2) * k_2;
            matrixD[2, 3] = c * k_4;
            matrixD[2, 4] = s * c * (-k_1 + k_2);
            matrixD[2, 5] = -(Math.Pow(s, 2) * k_1 + Math.Pow(c, 2) * k_2);
            matrixD[2, 6] = c * k_4;

            matrixD[3, 1] = -s * k_4;
            matrixD[3, 2] = c * k_4;
            matrixD[3, 3] = 4 * k_3;
            matrixD[3, 4] = s * k_4;
            matrixD[3, 5] = -c * k_4;
            matrixD[3, 6] = 2 * k_3;

            matrixD[4, 1] = -(Math.Pow(c, 2) * k_1 + Math.Pow(s, 2) * k_2);
            matrixD[4, 2] = s * c * (-k_1 + k_2);
            matrixD[4, 3] = s * k_4;
            matrixD[4, 4] = Math.Pow(c, 2) * k_1 + Math.Pow(s, 2) * k_2;
            matrixD[4, 5] = s * c * (k_1 - k_2);
            matrixD[4, 6] = s * k_4;

            matrixD[5, 1] = s * c * (-k_1 + k_2);
            matrixD[5, 2] = -(Math.Pow(s, 2) * k_1 + Math.Pow(c, 2) * k_2);
            matrixD[5, 3] = -c * k_4;
            matrixD[5, 4] = s * c * (k_1 - k_2);
            matrixD[5, 5] = Math.Pow(s, 2) * k_1 + Math.Pow(c, 2) * k_2;
            matrixD[5, 6] = -c * k_4;

            matrixD[6, 1] = -s * k_4;
            matrixD[6, 2] = c * k_4;
            matrixD[6, 3] = 2 * k_3;
            matrixD[6, 4] = s * k_4;
            matrixD[6, 5] = -c * k_4;
            matrixD[6, 6] = 4 * k_3;


            return matrixD;

        }



        private double[,] K_G_Element_ij_00(int Memb_ID)
        {
            int i = 0;
            int j = 0;

            int q = Memb_ID;
            double L = L_n(q);
            double h = Iz[q];
            double A = Ayz[q];
            double E = Ey[q];
            double c = cosx(q);
            double s = sinx(q);
            double[,] matrixD = new double[7, 7];
            double[,] matrixE = new double[6, 6];

            Array.Clear(matrixD, 0, matrixD.Length);
            double k_1 = 0;
            double k_2 = 0;
            double k_3 = 0;
            double k_4 = 0;

            k_1 = E * A / L;
            k_2 = 12 * E * h / Math.Pow(L, 3);
            k_3 = E * h / L;
            k_4 = 6 * E * h / Math.Pow(L, 2);


            matrixD[1, 1] = Math.Pow(c, 2) * k_1 + Math.Pow(s, 2) * k_2;
            matrixD[1, 2] = s * c * (k_1 - k_2);
            matrixD[1, 3] = -s * k_4;
            matrixD[1, 4] = -(Math.Pow(c, 2) * k_1 + Math.Pow(s, 2) * k_2);
            matrixD[1, 5] = s * c * (-k_1 + k_2);
            matrixD[1, 6] = -s * k_4;

            matrixD[2, 1] = s * c * (k_1 - k_2);
            matrixD[2, 2] = Math.Pow(s, 2) * k_1 + Math.Pow(c, 2) * k_2;
            matrixD[2, 3] = c * k_4;
            matrixD[2, 4] = s * c * (-k_1 + k_2);
            matrixD[2, 5] = -(Math.Pow(s, 2) * k_1 + Math.Pow(c, 2) * k_2);
            matrixD[2, 6] = c * k_4;

            matrixD[3, 1] = -s * k_4;
            matrixD[3, 2] = c * k_4;
            matrixD[3, 3] = 4 * k_3;
            matrixD[3, 4] = s * k_4;
            matrixD[3, 5] = -c * k_4;
            matrixD[3, 6] = 2 * k_3;

            matrixD[4, 1] = -(Math.Pow(c, 2) * k_1 + Math.Pow(s, 2) * k_2);
            matrixD[4, 2] = s * c * (-k_1 + k_2);
            matrixD[4, 3] = s * k_4;
            matrixD[4, 4] = Math.Pow(c, 2) * k_1 + Math.Pow(s, 2) * k_2;
            matrixD[4, 5] = s * c * (k_1 - k_2);
            matrixD[4, 6] = s * k_4;

            matrixD[5, 1] = s * c * (-k_1 + k_2);
            matrixD[5, 2] = -(Math.Pow(s, 2) * k_1 + Math.Pow(c, 2) * k_2);
            matrixD[5, 3] = -c * k_4;
            matrixD[5, 4] = s * c * (k_1 - k_2);
            matrixD[5, 5] = Math.Pow(s, 2) * k_1 + Math.Pow(c, 2) * k_2;
            matrixD[5, 6] = -c * k_4;

            matrixD[6, 1] = -s * k_4;
            matrixD[6, 2] = c * k_4;
            matrixD[6, 3] = 2 * k_3;
            matrixD[6, 4] = s * k_4;
            matrixD[6, 5] = -c * k_4;
            matrixD[6, 6] = 4 * k_3;

            for (i = 1; i <= 6; i++)
            {
                for (j = 1; j <= 6; j++)
                {
                    matrixE[i - 1, j - 1] = matrixD[i, j];
                }
            }

            return matrixE;

        }



        public static double Theta(int mem)
        {
            double x_2 = 0;
            double x_1 = 0;
            double y_2 = 0;
            double y_1 = 0;
            double L_1 = 0;
            double anglex = 0;

            if (totalnode >= 2)
            {

                x_2 = nodecoordinate[Connectivity[mem, 2]].X;
                x_1 = nodecoordinate[Connectivity[mem, 1]].X;
                y_2 = nodecoordinate[Connectivity[mem, 2]].Y;
                y_1 = nodecoordinate[Connectivity[mem, 1]].Y;
                L_1 = Math.Sqrt(Math.Pow(x_2 - x_1, 2) + Math.Pow(y_2 - y_1, 2));
                L_1 = L_1 / (GridX);
                anglex = (Math.Atan2((y_1 - y_2), (x_2 - x_1))) * 180 / Math.PI;


                if (anglex < 0)
                {
                    anglex = 360 + anglex;
                }

            }
            return anglex;

        }



        private int FindMAX_Integer_1D(int[] ptx)
        {

            int j = 0;
            int k = 0;

            int p = ptx.Length - 1;
            int[] ary = new int[p + 1];
            int[] pt = new int[p + 1];

            for (j = 0; j <= p; j++)
            {
                pt[j] = ptx[j];
            }



            for (j = 0; j <= p; j++)
            {
                for (k = 0; k < p; k++)
                {
                    if ((pt[k]) > (pt[k + 1]))
                    {
                        int temp1 = pt[k];
                        pt[k] = pt[k + 1];
                        pt[k + 1] = temp1;

                    }
                }
            }

            for (j = 0; j <= p; j++)
            {
                ary[j] = (pt[j]);
            }

            return ary[p];



        }
        private int[] sort_Integer_1D_j_1(int[] ptx)
        {
            int temp1;
            int j = 0;
            int k = 0;

            int p = ptx.Length - 1;
            int[] ary = new int[p + 1];
            int[] pt = new int[p + 1];

            for (j = 1; j <= p; j++)
            {
                pt[j] = ptx[j];
            }




            for (j = 1; j <= p; j++)
            {
                for (k = 1; k < p; k++)
                {
                    if ((pt[k]) > (pt[k + 1]))
                    {
                        temp1 = pt[k];
                        pt[k] = pt[k + 1];
                        pt[k + 1] = temp1;

                    }
                }
            }

            for (j = 1; j <= p; j++)
            {
                ary[j] = (pt[j]);
            }

            return ary;


        }




        private int[] sort_Integer_1D_j_0(int[] ptx)
        {
            int temp1;
            int j = 0;
            int k = 0;

            int p = ptx.Length - 1;
            int[] ary = new int[p + 1];
            int[] pt = new int[p + 1];

            for (j = 0; j <= p; j++)
            {
                pt[j] = ptx[j];
            }




            for (j = 0; j <= p; j++)
            {
                for (k = 0; k < p; k++)
                {
                    if ((pt[k]) > (pt[k + 1]))
                    {
                        temp1 = pt[k];
                        pt[k] = pt[k + 1];
                        pt[k + 1] = temp1;

                    }
                }
            }

            for (j = 0; j <= p; j++)
            {
                ary[j] = (pt[j]);
            }

            return ary;


        }

        private double[] sort_double_1D_j_0(double[] ptx)
        {
            double temp1 = 0;
            int j = 0;
            int k = 0;

            int p = ptx.Length - 1;
            double[] ary = new double[p + 1];

            double[] pt = new double[p + 1];

            for (j = 0; j <= p; j++)
            {

                pt[j] = ptx[j];
            }



            for (j = 0; j <= p; j++)
            {
                for (k = 0; k < p; k++)
                {

                    if ((pt[k]) > (pt[k + 1]))
                    {
                        temp1 = pt[k];
                        pt[k] = pt[k + 1];
                        pt[k + 1] = temp1;

                    }
                }
            }

            for (j = 0; j <= p; j++)
            {
                ary[j] = (pt[j]);
            }

            return ary;


        }




        public static double cosx(int mem)
        {
            double tempcosx = 0;
            double x_2 = 0;
            double x_1 = 0;
            double y_2 = 0;
            double y_1 = 0;
            double L_1 = 0;
            if (totalnode >= 2)
            {
                x_2 = nodecoordinate[Connectivity[mem, 2]].X;
                x_1 = nodecoordinate[Connectivity[mem, 1]].X;
                y_2 = nodecoordinate[Connectivity[mem, 2]].Y;
                y_1 = nodecoordinate[Connectivity[mem, 1]].Y;
                L_1 = Math.Sqrt(Math.Pow(x_2 - x_1, 2) + Math.Pow(y_2 - y_1, 2));

                tempcosx = ((x_2 - x_1) / L_1);



            }


            return tempcosx;
        }
        public static double sinx(int mem)
        {
            double tempsinx = 0;
            double x_2 = 0;
            double x_1 = 0;
            double y_2 = 0;
            double y_1 = 0;
            double L_1 = 0;
            if (totalnode >= 2)
            {
                x_2 = nodecoordinate[Connectivity[mem, 2]].X;
                x_1 = nodecoordinate[Connectivity[mem, 1]].X;
                y_2 = nodecoordinate[Connectivity[mem, 2]].Y;
                y_1 = nodecoordinate[Connectivity[mem, 1]].Y;
                L_1 = Math.Sqrt(Math.Pow(x_2 - x_1, 2) + Math.Pow(y_2 - y_1, 2));
                tempsinx = -((y_2 - y_1) / L_1);


            }
            return tempsinx;
        }




        private int[] Uncommon_Matrix_1D_1D_Intger(int[] First_M, int[] Second_M)
        {
            int[] Uncommon_M = new int[1];
            try
            {
                int j = 0;
                int k = 0;
                int p = First_M.Length - 1;
                int m = Second_M.Length - 1;

                Uncommon_M = new int[(p * 2) + 1];
                string[] First_Mat = new string[First_M.Length];
                string[] Second_Mat = new string[Second_M.Length];
                for (j = 0; j < First_M.Length; j++)
                {
                    First_Mat[j] = First_M[j].ToString();
                }
                for (j = 0; j < Second_M.Length; j++)
                {
                    Second_Mat[j] = Second_M[j].ToString();
                }



                Array.Resize(ref First_Mat, (2 * p) + 1);
                Array.Resize(ref Second_Mat, (2 * m) + 1);


                int[] First_Mat1 = new int[(2 * p) + 1];


                for (j = 0; j <= m; j++)
                {
                    for (k = 0; k <= p; k++)
                    {
                        if (Second_Mat[j] == First_Mat[k])
                        {
                            First_Mat[k] = null;


                        }
                    }
                }




                int cx = 0;
                for (k = 0; k <= p; k++)
                {
                    if (!string.IsNullOrEmpty(First_Mat[k]))
                    {
                        First_Mat1[cx] = Convert.ToInt32(First_Mat[k]);
                        cx = cx + 1;
                    }
                }

                for (k = 0; k < cx; k++)
                {

                    Uncommon_M[k] = (int)First_Mat1[k];
                }



                Array.Resize(ref Uncommon_M, cx);
            }
            catch (Exception ex)
            {


                Debug.Print(" Uncommon_Matrix_1D_1D_Intger(int[] First_M, int[] Second_M) ex   :" + Environment.NewLine + ex.ToString());
            }
            return Uncommon_M;

        }

        private int[] Common_Matrix_1D_1D_Intger(int[] First_M, int[] Second_M)
        {
            int[] Common_M_no_repeat = { };
            int kk = 0;
            int j = 0;
            int k = 0;

            int p = First_M.Length - 1;
            int m = Second_M.Length - 1;
            string[] Third_M = new string[(p + m) + 1];
            int[] Common_M = new int[(p + m) + 1];
            string[] First_Mat = new string[First_M.Length];
            string[] Second_Mat = new string[Second_M.Length];

            try
            {

                for (j = 0; j < First_M.Length; j++)
                {
                    First_Mat[j] = First_M[j].ToString();
                }
                for (j = 0; j < Second_M.Length; j++)
                {
                    Second_Mat[j] = Second_M[j].ToString();
                }



                Array.Resize(ref First_Mat, p + 1 + 1);
                Array.Resize(ref Second_Mat, m + 1 + 1);


                kk = 0;
                for (j = 0; j <= p; j++)
                {
                    for (k = 0; k <= m; k++)
                    {
                        if (Second_Mat[j] == First_Mat[k])
                        {

                            Third_M[kk] = First_Mat[k];

                            kk = kk + 1;
                        }
                    }
                }

                Array.Resize(ref Third_M, kk - 1 + 1);





                for (k = 0; k <= Third_M.Length - 1; k++)
                {

                    Common_M[k] = Convert.ToInt32(Third_M[k]);
                }



                Array.Resize(ref Common_M, Third_M.Length);
                Common_M_no_repeat = sort_no_repeat_INT(Common_M);
            }
            catch (Exception ex)
            {

                Debug.Print(" Common_Matrix_1D_1D_Intger(int[] First_M, int[] Second_M) ex   :" + Environment.NewLine + ex.ToString());
            }
            return Common_M_no_repeat;

        }


        private double[] Add_Matrix_1D_1D_Double(double[] First_Mat, double[] Second_Mat)
        {



            int p = First_Mat.Length - 1;
            int g = Second_Mat.Length - 1;
            double[] YZ = new double[p + 1];
            int j = 0;

            try
            {

                for (j = 0; j <= p; j++)
                {
                    YZ[j] = First_Mat[j] + Second_Mat[j];

                }

            }
            catch (Exception ex)
            {

                Debug.Print(" Add_Matrix_1D_1D_Double(double[] First_Mat, double[] Second_Mat) ex   :" + Environment.NewLine + ex.ToString());
            }


            return YZ;

        }


        private double[] Union_Matrix_1D_1D_Double(double[] First_Mat, double[] Second_Mat)
        {


            double[] YZ = null;
            int p = First_Mat.Length - 1;
            int g = Second_Mat.Length - 1;

            try
            {


                int i = 0;
                Array.Resize(ref YZ, p + g + 2);


                for (i = 0; i <= p; i++)
                {


                    YZ[i] = First_Mat[i];

                }




                for (i = p + 1; i <= p + g + 1; i++)
                {
                    YZ[i] = Second_Mat[i - p - 1];

                }

                Array.Resize(ref YZ, p + g + 2);
            }
            catch (Exception ex)
            {

                Debug.Print("Union_Matrix_1D_1D_Double ex " + ex.ToString());
                return null;
            }


            return YZ;

        }





        private int[] Union_Matrix_1D_1D_Intger(int[] First_Mat, int[] Second_Mat)
        {


            int[] YZ = null;
            int p = First_Mat.Length - 1;
            int g = Second_Mat.Length - 1;

            try
            {


                int i = 0;
                Array.Resize(ref YZ, p + g + 2);


                for (i = 0; i <= p; i++)
                {


                    YZ[i] = First_Mat[i];

                }






                for (i = p + 1; i <= p + g + 1; i++)
                {
                    YZ[i] = Second_Mat[i - p - 1];

                }

                Array.Resize(ref YZ, p + g + 2);
            }
            catch (Exception ex)
            {

                Debug.Print("Union_Matrix_1D_1D_Intger ex" + ex.ToString());
                return null;
            }


            return YZ;

        }
        private double[] Multiply_Matrix_1DBYFactor_0(double[] matrixA, double fact)
        {
            double[] YZ = new double[matrixA.GetUpperBound(0) + 1];
            int p = matrixA.GetUpperBound(0);

            try
            {
                int i = 0;

                for (i = 0; i <= matrixA.GetUpperBound(0); i++)
                {
                    YZ[i] = fact * matrixA[i];



                }



            }
            catch (Exception ex)
            {

                Debug.Print("Multiply_Matrix_1DBYFactor_0 ex " + ex.ToString());

            }

            return YZ;
        }






        private double[] Multiply_Matrix_2DBY1D_0_0(double[,] matrixA, double[] ZZ)
        {
            double[] YZ = new double[matrixA.GetUpperBound(0) + 1];
            int p = matrixA.GetUpperBound(1);
            if (matrixA.GetUpperBound(1) != ZZ.GetUpperBound(0))
            {
                MessageBox.Show("Number of rows of 1st array is not equal to Number of rows of 2nd array while Mutiplying");
            }
            try
            {
                if (matrixA.GetUpperBound(1) == ZZ.GetUpperBound(0))
                {
                    int i = 0;
                    int j = 0;

                    for (i = 0; i <= matrixA.GetUpperBound(0); i++)
                    {
                        for (j = 0; j <= ZZ.GetUpperBound(0); j++)
                        {

                            YZ[i] = YZ[i] + (matrixA[i, j] * ZZ[j]);


                        }



                    }
                }


            }
            catch (Exception ex)
            {

                Debug.Print("Multiply_Matrix_2DBY1D_0_0 ex " + ex.ToString());

            }

            return YZ;
        }



        private double[,] Trasform_mat(double c, double s)
        {
            double[,] matrixB = new double[6, 6];
            double[,] matrixC = new double[6, 6];
            double[,] matrixD = new double[6, 6];
            double[,] matrixE = new double[6, 6];
            double[,] matrixF = new double[6, 6];



            double[,] matrixA =
			{
				{c, s, 0, 0, 0, 0},
				{-s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, s, 0},
				{0, 0, 0, -s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};





            return matrixA;

        }
        private double[,] Trasform_mat_G_TO_L_ij_00(double c, double s)
        {




            double[,] matrixA =
			{
				{c, s, 0, 0, 0, 0},
				{-s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, s, 0},
				{0, 0, 0, -s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};





            return matrixA;

        }

        private double[,] Trasform_mat_TT_L_TO_G_ij_00(double c, double s)
        {


            double[,] TT1 = new double[,]
			{
				{c, -s, 0, 0, 0, 0},
				{s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, -s, 0},
				{0, 0, 0, s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};





            return TT1;

        }
        private double[,] Trasform_mat_TT_L_TO_G_ij_00_Mem_Id(int Mem_Id)
        {
            double c = cosx(Mem_Id);
            double s = sinx(Mem_Id);

            double[,] TT1 = new double[,]
			{
				{c, -s, 0, 0, 0, 0},
				{s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, -s, 0},
				{0, 0, 0, s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};





            return TT1;

        }

        private double[,] Trasform_COORDINATE_mat_G_TO_L_ij_00(double c, double s)
        {

            double[,] matrixB = new double[6, 6];
            double[,] matrixC = new double[6, 6];
            double[,] matrixD = new double[6, 6];
            double[,] matrixE = new double[6, 6];
            double[,] matrixF = new double[6, 6];



            double[,] matrixA =
			{
				{c, s, 0, 0, 0, 0},
				{-s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, s, 0},
				{0, 0, 0, -s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};





            return matrixA;

        }

        private double[,] Trasform_L_To_G_Line_Ends(double c, double s)
        {

            double[,] matrix = new double[6, 6];
            double[,] matrixC = new double[6, 6];
            double[,] matrixD = new double[6, 6];
            double[,] matrixE = new double[6, 6];
            double[,] matrixF = new double[6, 6];



            double[,] matrixA =
			{
				{c, -s},
				{s, c}
			};





            return matrixA;

        }

        private double[,] Trasform_G_To_L_Line_Ends(double c, double s)
        {

            double[,] matrix = new double[6, 6];
            double[,] matrixC = new double[6, 6];
            double[,] matrixD = new double[6, 6];
            double[,] matrixE = new double[6, 6];
            double[,] matrixF = new double[6, 6];



            double[,] matrixA =
			{
				{c, s},
				{-s, c}
			};





            return matrixA;

        }


        private string MakeDisplayable_2D2D_ij_1_1(double[,] Array2D)
        {
            int row = 0;
            int col = 0;
            string s1 = null;


            s1 = "";



            for (row = 1; row <= Array2D.GetUpperBound(0); row++)
            {
                for (col = 1; col <= Array2D.GetUpperBound(1); col++)
                {

                    s1 = s1 + (Array2D[row, col].ToString()) + " ";
                }
                s1 = s1 + Environment.NewLine;
            }

            return s1;

        }




        private string MakeDisplayable_2D2D_ij_0_0(double[,] Array2D)
        {
            int row = 0;
            int col = 0;
            string s1 = null;


            s1 = "";


            for (row = 0; row <= Array2D.GetUpperBound(0); row++)
            {
                for (col = 0; col <= Array2D.GetUpperBound(1); col++)
                {

                    s1 = s1 + (Array2D[row, col].ToString()) + " ";
                }
                s1 = s1 + Environment.NewLine;
            }

            return s1;

        }

        private string MakeDisplayable_Scilab_2D2D_ij_0_0(double[,] Array2D)
        {
            int row = 0;
            int col = 0;
            string s1 = null;


            s1 = "";
            for (row = 0; row < Array2D.GetLength(0); row++)
            {
                for (col = 0; col < Array2D.GetLength(1); col++)
                {

                    if (col < Array2D.GetLength(1) - 1)
                    {
                        s1 = s1 + (Array2D[row, col].ToString("0.0")) + ",";
                    }
                    if (col == Array2D.GetLength(1) - 1)
                    {
                        s1 = s1 + (Array2D[row, col].ToString("0.000")) + ";";
                    }

                }
                s1 = s1 + Environment.NewLine;
            }

            return s1;

        }

        private string MakeDisplayable_Scilab_Column_1D_i_0(double[] Array1D)
        {
            int row = 0;
            string s1 = null;


            s1 = "";
            for (row = 0; row < Array1D.GetLength(0); row++)
            {


                {
                    s1 = s1 + (Array1D[row].ToString("0.000")) + "," + Environment.NewLine;
                }


            }

            return s1;

        }

        private string MakeDisplayable_INT_2D2D_ij_0_0(int[,] Array2D)
        {
            int row = 0;
            int col = 0;
            string s1 = null;


            s1 = "";


            for (row = 0; row <= Array2D.GetUpperBound(0); row++)
            {
                for (col = 0; col <= Array2D.GetUpperBound(1); col++)
                {

                    s1 = s1 + (Array2D[row, col].ToString()) + " ";
                }
                s1 = s1 + Environment.NewLine;
            }

            return s1;

        }

















        private double[,] transposeMatrix(double[,] theMatrix)
        {
            int i = 0;
            int j = 0;
            double[,] matrixResult = new double[theMatrix.GetUpperBound(1) + 1, theMatrix.GetUpperBound(0) + 1];





            if (theMatrix != null)
            {

                for (i = 0; i <= theMatrix.GetUpperBound(0); i++)
                {
                    for (j = 0; j <= theMatrix.GetUpperBound(1); j++)
                    {
                        matrixResult[j, i] = theMatrix[i, j];
                    }
                }
            }
            return matrixResult;
        }







        private string MakeDisplayable_1D_j_0_INT(int[] Array1D)
        {

            string str = new string("xx"[0], 5);
            int col = 0;
            string s1 = null;
            string s2 = null;
            string s3 = null;


            s1 = "";
            s2 = "";
            s3 = "Inetger array print as below: ";
            for (col = 0; col <= Array1D.GetUpperBound(0); col++)
            {

                s2 = s2 + col.ToString() + new string(' ', col.ToString().Length);
                s1 = s1 + (Array1D[col].ToString()) + " ";

            }

            s1 = s3 + Environment.NewLine + s2 + Environment.NewLine + s1;
            return s1;

        }


        private string MakeDisplayable_1D_j_1_INT(int[] Array1D)
        {

            string str = new string("xx"[0], 5);
            int col = 0;
            string s1 = null;
            string s2 = null;
            string s3 = null;


            s1 = "";
            s2 = "";
            s3 = "Inetger array print as below: ";
            for (col = 1; col <= Array1D.GetUpperBound(0); col++)
            {

                s2 = s2 + col.ToString() + new string(' ', col.ToString().Length);
                s1 = s1 + (Array1D[col].ToString()) + " ";

            }

            s1 = s3 + Environment.NewLine + s2 + Environment.NewLine + s1;
            return s1;

        }


        private string MakeDisplayable_1D_j_0_string(string[] Array1D)
        {
            int col = 0;
            StringBuilder s = new StringBuilder();
            for (col = 0; col <= Array1D.GetUpperBound(0); col++)
            {
                s.Append(col.ToString() + " ");


            }
            s.AppendLine();
            for (col = 0; col <= Array1D.GetUpperBound(0); col++)
            {



                s.Append((Array1D[col]) + " ");


            }


            return s.ToString();

        }


        private string MakeDisplayable_1D_j_1_string(string[] Array1D)
        {
            int col = 0;
            StringBuilder s = new StringBuilder();
            for (col = 1; col <= Array1D.GetUpperBound(0) + 1; col++)
            {
                s.Append(col.ToString() + " ");


            }
            s.AppendLine();
            for (col = 1; col <= Array1D.GetUpperBound(0) + 1; col++)
            {


                s.Append((Array1D[col]) + " ");


            }


            return s.ToString();

        }



        private string MakeDisplayable_2D(double[,] sourceMatrix)
        {

            int rows = 0;
            int cols = 0;
            int eachRow = 0;
            int eachCol = 0;
            System.Text.StringBuilder result = new System.Text.StringBuilder();


            rows = sourceMatrix.GetUpperBound(0);
            cols = sourceMatrix.GetUpperBound(1);
            for (eachRow = 0; eachRow <= rows; eachRow++)
            {

                if (eachRow > 0)
                {
                    result.AppendLine();
                }
                for (eachCol = 0; eachCol <= cols; eachCol++)
                {

                    if (eachCol > 0)
                    {
                        result.Append("  ");
                    }
                    result.Append(sourceMatrix[eachRow, eachCol].ToString());
                }
            }
            result.AppendLine();

            return result.ToString();
        }









        private void rearrange(PointF[] ar, int m)
        {
            int k = 0;
            PointF temp;
            k = ar.Length - 1;
            ar[m] = PointF.Empty;
            temp = ar[ar.Length - 1];
            ar[ar.Length - 1] = PointF.Empty;
            ar[m] = temp;
            k = k - 1;

            Array.Resize(ref ar, k + 1);
        }




        private double[] Solve_Eqn(double[,] a, double[] YLX)
        {
            double[] Matrix_Multx = new double[a.GetUpperBound(0) + 1];

            double rgh = 0;

            int Max_Index = 0;
            int k = 0;
            int L = 0;
            Max_Index = a.GetUpperBound(0);

            for (k = 0; k <= Max_Index; k++)
            {
                rgh = 0;

                for (L = 0; L <= Max_Index; L++)
                {


                    rgh = rgh + a[k, L] * YLX[L];


                }

                Matrix_Multx[k] = rgh;

            }
            return Matrix_Multx;
        }





        private void mnuFileExit_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
        }




        private void draw_Node_number(Graphics g)
        {
            if (totalnode >= 2)
            {

                for (int u = 1; u <= totalnode_From_Lines; u++)
                {
                    g.DrawString(u.ToString(), new Font("Courier New", 10, FontStyle.Italic), new SolidBrush(Color.Green), (nodecoordinate[u].X + 5), (nodecoordinate[u].Y));

                }
            }
        }



        public void draw_Member_number(List<line> mLinesy1, Graphics g)
        {


            for (int i = 0; i < mLinesy1.Count; i++)
            {
                try
                {


                    if (mLinesy1[i].StartPoint.Y == mLinesy1[i].EndPoint.Y)
                    {
                        g.DrawString((i + 1).ToString(), new Font("Courier New", 10, FontStyle.Regular), new SolidBrush(Color.Black), (mLinesy1[i].StartPoint.X + mLinesy1[i].EndPoint.X) / 2, (mLinesy1[i].StartPoint.Y + 2));

                    }
                    else if (mLinesy1[i].StartPoint.X == mLinesy1[i].EndPoint.X)
                    {
                        g.DrawString((i + 1).ToString(), new Font("Courier New", 10, FontStyle.Regular), new SolidBrush(Color.Black), (mLinesy1[i].StartPoint.X + 2), (mLinesy1[i].StartPoint.Y + mLinesy1[i].EndPoint.Y) / 2);

                    }

                    else if (mLinesy1[i].StartPoint.X != mLinesy1[i].EndPoint.X && mLinesy1[i].StartPoint.Y != mLinesy1[i].EndPoint.Y)
                    {

                        double M1 = (mLinesy1[i].EndPoint.Y - mLinesy1[i].StartPoint.Y) / (mLinesy1[i].EndPoint.X - mLinesy1[i].StartPoint.X);
                        double C1 = -(M1 * mLinesy1[i].StartPoint.X) + mLinesy1[i].StartPoint.Y;
                        if (M1 < 0)
                        {
                            M1 = (-M1);
                        }
                        g.DrawString((i + 1).ToString(), new Font("Courier New", 10, FontStyle.Regular), new SolidBrush(Color.Black), (float)((mLinesy1[i].StartPoint.X + mLinesy1[i].EndPoint.X) / 2), (float)((mLinesy1[i].StartPoint.Y + mLinesy1[i].EndPoint.Y) / 2 - 1 / M1 * 10 + 15));

                    }

                }
                catch (Exception ex)
                {

                    Debug.Print("public void draw_Member_number   " + Environment.NewLine + ex.ToString());
                }
            }


        }


        private int Find_Node_Num_From_Coordinate(PointF p)
        {

            int i = 0;
            int k = 0;
            i = -1;

            for (i = 1; i <= totalnode; i++)
            {
                if (nodecoordinate[i].X == p.X && nodecoordinate[i].Y == p.Y)
                {


                    k = i;
                }
            }


            return k;
        }

















        private PointF[] sortnode_p(PointF[] pt, int p)
        {
            PointF[] pt1 = { };
            PointF temp1 = PointF.Empty;
            int j = 0;
            int k = 0;

            try
            {
                Array.Resize(ref pt, (2 * p) + 1);
                Array.Resize(ref pt1, (2 * p) + 1);
            }
            catch (Exception)
            {
            }

            for (j = 0; j <= p; j++)
            {
                for (k = 1; k < p; k++)
                {
                    if (pt[j] == pt[j + k])
                    {
                        temp1 = pt[j + 1];
                        pt[j + 1] = pt[j + k];
                        pt[j + k] = temp1;


                    }
                }
            }



            k = 0;

            k = 0;
            for (j = 0; j < p; j++)
            {

                temp1 = pt[j];

                if (pt[j] == pt[j + 1])
                {

                    pt[j] = PointF.Empty;
                    pt[j + 1] = temp1;


                }
            }



            k = 0;
            for (j = 0; j <= p; j++)
            {
                if (pt[j] != PointF.Empty)
                {
                    pt1[k] = pt[j];
                    k = k + 1;
                }

            }


            Array.Resize(ref pt1, k);
            return pt1;

        }

        private PointF[] uncommon_p(PointF[] st, PointF[] stx, int p, int m)
        {
            try
            {
                Array.Resize(ref st, (2 * p) + 1);
                Array.Resize(ref stx, (2 * m) + 1);
            }
            catch (Exception)
            {
            }

            PointF[] st1 = new PointF[10];
            int j = 0;
            int k = 0;

            for (j = 0; j <= m; j++)
            {
                for (k = 0; k <= p; k++)
                {
                    if (stx[j] == st[k])
                    {
                        st[k] = PointF.Empty;


                    }
                }
            }





            int c = 0;
            for (k = 0; k <= p; k++)
            {
                if (st[k] != null)
                {
                    st1[c] = st[k];
                    c = c + 1;
                }
            }


            Array.Resize(ref st1, c);
            return st1;


        }





        private int[] Find_MemID_From_Joint(int node_num)
        {
            int[] joint = new int[20];
            PointF p = nodecoordinate[node_num];
            int k = 0;

            for (int i = 1; i <= totalmember; i++)
            {
                if (Connectivity[i, 1] == node_num)
                {
                    k = k + 1;
                    joint[k] = i;

                }

                if (Connectivity[i, 2] == node_num)
                {
                    k = k + 1;
                    joint[k] = i;

                }
            }
            return joint;
        }

        private int Find_Joint(PointF[] ptv, float x, float y)
        {
            int k = 0;
            Array.Resize(ref ptv, 10);
            for (int i = 1; i < ptv.Length; i++)
            {
                if (ptv[i].X == x && ptv[i].Y == y)
                {
                    k = i;
                }
            }
            return k;
        }




        private string[] Read_Textfile_AS_Array_Of_Line_String_from_FilePath(string dirx)
        {
            int j = 50000;
            int i = 0;
            int k = 0;


            string[] stx = new string[j + 1];
            if (File.Exists(dirx))
            {


                StreamReader FileReader = new StreamReader(dirx);



                while (!FileReader.EndOfStream)
                {
                    for (i = 0; i <= j; i++)
                    {

                        stx[i] = FileReader.ReadLine();



                    }

                }

                FileReader.Close();
            }

            Array.Resize(ref stx, i + 1);
            string[] stxx = new string[i + 1];
            k = 0;
            for (i = 0; i < stx.Length; i++)
            {
                if (!string.IsNullOrEmpty(stx[i].Trim(' ')))
                {
                    stxx[k] = stx[i];
                    k = k + 1;
                }


            }

            Array.Resize(ref stxx, k);




            return stxx;


        }




        private string Read_Textfile_AS_String_from_FilePath(string dirx)
        {
            int j = 50000;
            int i = 0;
            int k = 0;


            string[] stx = new string[j + 1];
            if (File.Exists(dirx))
            {


                StreamReader FileReader = new StreamReader(dirx);



                while (!FileReader.EndOfStream)
                {
                    for (i = 0; i <= j; i++)
                    {

                        stx[i] = FileReader.ReadLine();



                    }

                }

                FileReader.Close();
            }

            Array.Resize(ref stx, i + 1);
            string[] stxx = new string[i + 1];
            string S99 = null;
            k = 0;
            for (i = 0; i < stx.Length; i++)
            {
                if (!string.IsNullOrEmpty(stx[i]))
                {
                    stxx[k] = stx[i];
                    k = k + 1;
                }


            }

            Array.Resize(ref stxx, k);


            S99 = "";

            for (i = 0; i < stxx.Length; i++)
            {
                S99 = S99 + stxx[i];

            }


            string value = File.ReadAllText(dirx);

            return value;

        }









        public static double[] Joint_Global_force_0_3(string pj)
        {
            double[] temp12 = new double[6];
            double Pjx = 0;
            double Pjy = 0;
            double Mjzz = 0;
            double[] Flocal = new double[4];
            int k = 0;
            string[] test = new string[] { };

            try
            {


                if (!string.IsNullOrEmpty(pj))
                {
                    test = pj.Split(new char[] { ',' });
                    for (k = 0; k < test.Length; k++)
                    {

                    }
                    k = int.Parse(test[0]);
                    Pjx = double.Parse(test[1]);
                    Pjy = double.Parse(test[2]);
                    Mjzz = double.Parse(test[3]);
                    Flocal[0] = k;
                    Flocal[1] = Pjx;
                    Flocal[2] = Pjy;
                    Flocal[3] = Mjzz;

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Debug.Print("Joint_Global_force_0_3 Exception= " + ex.ToString());
            }

            return Flocal;
        }




        private void DeleteLoadDataToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {

            Array.Clear(Pload, 0, Pload.Length);
            Array.Clear(PJoint, 0, PJoint.Length);
        }





        private string MakeDisplayable_Top_Side_No_Connectivity_3D3D_ij_0_0(double[, ,] threeD)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            string s1 = null;


            s1 = "";
            s1 = s1 + "MakeDisplayable_Top_Side_No_Connectivity_3D3D_ij_0_0 below :";
            s1 = s1 + Environment.NewLine;


            for (k = 1; k <= threeD.GetUpperBound(0); k++)
            {

                // s1 = s1 + " mem_id= ...." + k.ToString() + Environment.NewLine;

                for (j = 0; j <= threeD.GetUpperBound(2); j++)
                {

                    s1 = s1 + "   " + j.ToString();

                }

                s1 = s1 + Environment.NewLine;




                for (i = 0; i <= threeD.GetUpperBound(1); i++)
                {
                    s1 = s1 + "     " + i.ToString() + "  ";

                    for (j = 0; j <= threeD.GetUpperBound(2); j++)
                    {

                        s1 = s1 + (threeD[k, i, j].ToString()) + " ";
                    }

                    s1 = s1 + Environment.NewLine;
                }


            }

            return s1;

        }


        private string MakeDisplayable_ij_00_2DARRAY_DOF_Index(double[,] Array2D)
        {
            int i = 0;
            int j = 0;
            string s1 = null;
            string[] a = new string[100001];
            try
            {

                s1 = "";

                s1 = s1 + "MakeDisplayable_ij_00_2DARRAY_DOF_Index - array print as below: ";


                s1 = s1 + Environment.NewLine;



                s1 = s1 + Environment.NewLine;

                for (j = 0; j <= Array2D.GetUpperBound(0); j++)
                {

                    s1 = s1 + "   " + j.ToString();


                }



                s1 = s1 + Environment.NewLine;




                for (i = 0; i <= Array2D.GetUpperBound(0); i++)
                {
                    s1 = s1 + i.ToString() + "  ";
                    for (j = 0; j <= Array2D.GetUpperBound(1); j++)
                    {

                        s1 = s1 + " " + Array2D[i, j].ToString();
                    }

                    s1 = s1 + " " + Environment.NewLine;
                }



            }
            catch (Exception ex)
            {

                Debug.Print("  MakeDisplayable_ij_00_2DARRAY_DOF_Index ex :" + Environment.NewLine + ex.ToString());

            }



            return s1;

        }
        private string MakeDisplayable_ij_00_1DArrayOld_coma_2DARRAY_OLDNEW_Index(double[] Arrayoldindex, double[,] Array2D)
        {
            int i = 0;
            int j = 0;
            string s1 = null;
            string[] a = new string[100001];
            try
            {

                s1 = "";

                s1 = s1 + "MakeDisplayable_ij_00_1DArrayOld_coma_2DARRAY_OLDNEW_Index - array print as below: ";


                s1 = s1 + Environment.NewLine;

                for (j = 0; j <= Arrayoldindex.GetUpperBound(0); j++)
                {

                    s1 = s1 + "   " + Arrayoldindex[j].ToString();
                    a[j] = a[j] + "   " + Arrayoldindex[j].ToString();

                }

                s1 = s1 + Environment.NewLine;

                for (j = 0; j <= Array2D.GetUpperBound(0); j++)
                {

                    s1 = s1 + "   " + j.ToString();


                }



                s1 = s1 + Environment.NewLine;




                for (i = 0; i <= Array2D.GetUpperBound(0); i++)
                {
                    s1 = s1 + a[i] + " " + i.ToString() + "  ";
                    for (j = 0; j <= Array2D.GetUpperBound(1); j++)
                    {

                        s1 = s1 + (Array2D[i, j].ToString()) + " ";

                    }

                    s1 = s1 + Environment.NewLine;
                }



            }
            catch (Exception ex)
            {


                Debug.Print(" MakeDisplayable_ij_00_1DArrayOld_coma_2DARRAY_OLDNEW_Index ex:" + Environment.NewLine + ex.ToString());
            }
            return s1;


        }


        private string MakeDisplayable_ij_11_1DArrayOld_coma_2DARRAY_OLDNEW_Index(double[] Arrayoldindex, double[,] Array2D)
        {
            int i = 0;
            int j = 0;
            string s1 = null;
            string[] a = new string[100001];
            try
            {

                s1 = "";

                s1 = s1 + "MakeDisplayable_ij_11_1DArrayOld_coma_2DARRAY_OLDNEW_Index - array print as below: ";


                s1 = s1 + Environment.NewLine;

                for (j = 0; j <= Arrayoldindex.GetUpperBound(0); j++)
                {

                    s1 = s1 + "   " + Arrayoldindex[j].ToString();
                    a[j] = a[j] + "   " + Arrayoldindex[j].ToString();

                }

                s1 = s1 + Environment.NewLine;

                for (j = 0; j <= Array2D.GetUpperBound(0); j++)
                {

                    s1 = s1 + "   " + j.ToString();


                }



                s1 = s1 + Environment.NewLine;




                for (i = 1; i <= Array2D.GetUpperBound(0); i++)
                {
                    s1 = s1 + a[i] + " " + i.ToString() + "  ";
                    for (j = 1; j <= Array2D.GetUpperBound(1); j++)
                    {

                        s1 = s1 + (Array2D[i, j].ToString()) + " ";

                    }

                    s1 = s1 + Environment.NewLine;
                }



            }
            catch (Exception ex)
            {


                Debug.Print(" MakeDisplayable_ij_00_1DArrayOld_coma_2DARRAY_OLDNEW_Index ex:" + Environment.NewLine + ex.ToString());
            }
            return s1;


        }




        private string MakeDisplayable_1DARRAYj0_Index_6_6_Matrix(int mem_idx, double[,] Array2D)
        {

            int i = 0;
            int j = 0;
            int k = 0;
            string s1 = "";

            string[] a = new string[100001];







            k = mem_idx;
            int[] Index_1 = Index_6_From_MemID_1D_1_7_HINGE(k);
            try
            {




                s1 = "";
                s1 = s1 + Environment.NewLine;

                s1 = s1 + "mem, MakeDisplayable_1DARRAYj0_Index_6_6_Matrix...." + k.ToString() + Environment.NewLine;

                for (j = 1; j <= 3; j++)
                {

                    s1 = s1 + "   " + Index_1[j].ToString();
                    a[j - 1] = Index_1[j].ToString();
                }

                for (j = 1; j <= 3; j++)
                {

                    s1 = s1 + "   " + Index_1[j].ToString();
                    a[j - 1 + 3] = Index_1[3 + j].ToString();

                }

                s1 = s1 + Environment.NewLine;




                for (i = 1; i <= Array2D.GetUpperBound(0); i++)
                {
                    s1 = s1 + "     " + a[i - 1].ToString() + "  ";

                    for (j = 1; j <= Array2D.GetUpperBound(1); j++)
                    {

                        s1 = s1 + Array2D[i, j].ToString() + " ";
                    }

                    s1 = s1 + Environment.NewLine;
                }



            }
            catch (Exception ex)
            {


                Debug.Print(" MakeDisplayable_1DARRAYj0_Index_6_6_Matrix " + ex.ToString());


            }
            return s1;


        }

        private string MakeDisplayable_ij_00_2DArrayOld_coma_2DARRAY_OLDNEW_Index(double[,] Arrayoldindex, double[,] Array2D)
        {




            int i = 0;
            int j = 0;

            string s1 = null;

            string[] a = new string[100001];

            try
            {
                a[0] = "";
                s1 = "";


                s1 = "";
                s1 = s1 + "Arrayoldindex(i,3)=row, Arrayoldindex(j,4)=col, MakeDisplayable_ij_00_1DArrayOld_coma_2DARRAY_OLDNEW_Index - array print as below: ";

                s1 = s1 + Environment.NewLine;




                for (j = 0; j <= Arrayoldindex.GetUpperBound(0); j++)
                {

                    s1 = s1 + "   " + Arrayoldindex[j, 4].ToString();


                }

                s1 = s1 + Environment.NewLine;

                for (j = 0; j <= Array2D.GetUpperBound(1); j++)
                {

                    s1 = s1 + "   " + j.ToString();


                }


                s1 = s1 + Environment.NewLine;

                for (i = 0; i <= Arrayoldindex.GetUpperBound(0); i++)
                {

                    a[i] = a[i] + "   " + Arrayoldindex[i, 3].ToString();


                }



                s1 = s1 + Environment.NewLine;




                for (i = 0; i <= Array2D.GetUpperBound(0); i++)
                {
                    s1 = s1 + a[i].ToString() + " " + i.ToString() + "  ";

                    for (j = 0; j <= Array2D.GetUpperBound(1); j++)
                    {

                        s1 = s1 + (Array2D[i, j].ToString()) + " ";
                    }

                    s1 = s1 + Environment.NewLine;
                }




            }
            catch (Exception ex)
            {
                Debug.Print(" MakeDisplayable_ij_00_2DArrayOld_coma_2DARRAY_OLDNEW_Index " + ex.ToString());


            }

            return s1;


        }

        private string MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(double[,] Arrayoldindex, double[,] Array2D)
        {




            int i = 0;
            int j = 0;

            string s1 = null;

            string[] a = new string[100001];

            try
            {
                a[0] = "";
                s1 = "";


                s1 = "";
                //s1 = s1 + "Arrayoldindex(i,3)=row, Arrayoldindex(j,4)=col, MakeDisplayable_ij_11_1DArrayOld_coma_2DARRAY_OLDNEW_Index - array print as below: ";

                s1 = s1 + Environment.NewLine;


                s1 = " " + "-";

                for (j = 0; j <= Arrayoldindex.GetUpperBound(0); j++)
                {

                    if (j == 0)
                    {
                        ////s1 = s1 + " " + Arrayoldindex[j, 4].ToString(); //header 1st row col wise
                        s1 = s1 + " " + "-"; //header 1st row col wise
                    }
                    else if (Arrayoldindex[j, 4] != 0)
                    {
                        s1 = s1 + "   " + Arrayoldindex[j, 4].ToString(); //header 1st row col wise
                    }

                }

                s1 = s1 + Environment.NewLine + " " + "-";

                for (j = 0; j <= Array2D.GetUpperBound(1); j++)
                {
                    if (j == 0)
                    {
                        ////s1 = s1 + " " + j.ToString();
                        s1 = s1 + " " + "-";
                    }
                    else
                    {
                        s1 = s1 + "   " + j.ToString();//header 2nd row col wise
                    }

                }

                //end header 2nd row
                s1 = s1 + Environment.NewLine;

                for (i = 0; i <= Arrayoldindex.GetUpperBound(0); i++)
                {

                    a[i] = a[i] + "   " + Arrayoldindex[i, 3].ToString();//left side 1st col row wise


                }



                s1 = s1 + Environment.NewLine;




                for (i = 1; i <= Array2D.GetUpperBound(0); i++)
                {
                    s1 = s1 + a[i].ToString() + " " + i.ToString() + "  ";//left side 1st,2nd col row wise

                    for (j = 1; j <= Array2D.GetUpperBound(1); j++)
                    {

                        s1 = s1 + (Array2D[i, j].ToString()) + " ";//mattrix after left side 1st,2nd col row wise
                    }

                    s1 = s1 + Environment.NewLine;
                }




            }
            catch (Exception ex)
            {
                Debug.Print(" MakeDisplayable_ij_00_2DArrayOld_coma_2DARRAY_OLDNEW_Index " + ex.ToString());


            }

            return s1;


        }




        private double[,] Trasform_mat_T_G_TO_L_ij_00_INC_SUPP(int Memb_ID)
        {



            int q = 0;
            q = Memb_ID;
            double c = 0;
            double s = 0;
            int NodeNumx = 0;
            NodeNumx = Inclied_Supp_Node_num;

            double angx = 0;
            double c1 = 0;
            double s1 = 0;

            c1 = cosx(q);
            s1 = sinx(q);
            c = cosx(q);
            s = sinx(q);


            angx = -Inc_Supp_angle_base_AND_GX + Theta(q);

            if (Connectivity[q, 1] == Inclied_Supp_Node_num)
            {
                c1 = Math.Cos(Math.PI / 180 * angx);
                s1 = Math.Sin(Math.PI / 180 * angx);

                c = cosx(q);
                s = sinx(q);

            }
            if (Connectivity[q, 2] == Inclied_Supp_Node_num)
            {
                c = Math.Cos(Math.PI / 180 * angx);
                s = Math.Sin(Math.PI / 180 * angx);

                c1 = cosx(q);
                s1 = sinx(q);

            }


            double[,] T1 = new double[,]
			{
				{c1, s1, 0, 0, 0, 0},
				{-s1, c1, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, s, 0},
				{0, 0, 0, -s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};



            return T1;

        }


        private double[,] Trasform_mat_TT_L_TO_G_ij_00_INC_SUPP(int Memb_ID)
        {



            int q = 0;
            q = Memb_ID;
            double c = 0;
            double s = 0;
            int NodeNumx = 0;
            NodeNumx = Inclied_Supp_Node_num;

            double angx = 0;
            double c1 = 0;
            double s1 = 0;


            c1 = cosx(q);
            s1 = sinx(q);
            c = cosx(q);
            s = sinx(q);


            angx = -Inc_Supp_angle_base_AND_GX + Theta(q);

            if (Connectivity[q, 1] == Inclied_Supp_Node_num)
            {
                c1 = Math.Cos(Math.PI / 180 * angx);
                s1 = Math.Sin(Math.PI / 180 * angx);

                c = cosx(q);
                s = sinx(q);

            }
            if (Connectivity[q, 2] == Inclied_Supp_Node_num)
            {
                c = Math.Cos(Math.PI / 180 * angx);
                s = Math.Sin(Math.PI / 180 * angx);

                c1 = cosx(q);
                s1 = sinx(q);

            }



            double[,] TT1 = new double[,]
			{
				{c1, -s1, 0, 0, 0, 0},
				{s1, c1, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, -s, 0},
				{0, 0, 0, s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};





            return TT1;

        }


        private double[,] FORCE_Eqiv_Nod_MembComaP_ij_1_0_TO_Global_INC_SUP(int Memb_id, string St_memLoad)
        {

            double[] temp12 = new double[6];
            double[] Flocal = new double[6];
            double[,] Flocal2 = null;
            double[] Fglobal = null;
            double[,] Flocal2D = new double[totalmember + 1, 6];
            int q = 0;
            q = Memb_id;
            double c = 0;
            double s = 0;


            int NodeNumx = 0;
            NodeNumx = Inclied_Supp_Node_num;
            double angx = 0;
            double c1 = 0;
            double s1 = 0;

            c1 = cosx(q);
            s1 = sinx(q);
            c = cosx(q);
            s = sinx(q);


            angx = -Inc_Supp_angle_base_AND_GX + Theta(q);

            if (Connectivity[q, 1] == Inclied_Supp_Node_num)
            {
                c1 = Math.Cos(Math.PI / 180 * angx);
                s1 = Math.Sin(Math.PI / 180 * angx);

                c = cosx(q);
                s = sinx(q);

            }
            if (Connectivity[q, 2] == Inclied_Supp_Node_num)
            {
                c = Math.Cos(Math.PI / 180 * angx);
                s = Math.Sin(Math.PI / 180 * angx);

                c1 = cosx(q);
                s1 = sinx(q);

            }



            if (!string.IsNullOrEmpty(St_memLoad))
            {

                if (Memb_id == int.Parse(St_memLoad.Substring(0, 1)))
                {
                    Flocal2 = FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal(Memb_id, St_memLoad);


                    double[,] TT1 = new double[,]
			{
				{c1, -s1, 0, 0, 0, 0},
				{s1, c1, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, -s, 0},
				{0, 0, 0, s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};






                    Flocal[0] = Flocal2[Memb_id, 0];
                    Flocal[1] = Flocal2[Memb_id, 1];
                    Flocal[2] = Flocal2[Memb_id, 2];
                    Flocal[3] = Flocal2[Memb_id, 3];
                    Flocal[4] = Flocal2[Memb_id, 4];
                    Flocal[5] = Flocal2[Memb_id, 5];


                    Fglobal = Multiply_Matrix_2DBY1D_0_0(TT1, Flocal);

                }

            }
            for (int j = 0; j <= 5; j++)
            {
                Flocal2D[Memb_id, j] = Fglobal[j];
            }




            return Flocal2D;

        }




        private double[,] K_2_G_Element_i_0_j_0_INCLINED_SUP(int Memb_ID)
        {

            double[,] Cmp = new double[0, 0];
            double[,] Cmp1 = new double[0, 0];
            double I = 0;
            double A = 0;
            double E = 0;
            double L = 0;

            double c = 0;
            double s = 0;


            int q = Memb_ID;

            L = L_n(q);
            I = Iz[q];
            A = Ayz[q];
            E = Ey[q];
            c = cosx(q);
            s = sinx(q);

            if (Iz[q] == 0 | Ayz[q] == 0 | Ey[q] == 0)
            {
                MessageBox.Show("Material property unspecified");
            }


            int NodeNumx = 0;
            NodeNumx = Inclied_Supp_Node_num;
            double angx = 0;
            double c1 = 0;
            double s1 = 0;

            c1 = cosx(q);
            s1 = sinx(q);
            c = cosx(q);
            s = sinx(q);


            angx = -Inc_Supp_angle_base_AND_GX + Theta(q);

            if (Connectivity[q, 1] == Inclied_Supp_Node_num)
            {
                c1 = Math.Cos(Math.PI / 180 * angx);
                s1 = Math.Sin(Math.PI / 180 * angx);

                c = cosx(q);
                s = sinx(q);

            }
            if (Connectivity[q, 2] == Inclied_Supp_Node_num)
            {
                c = Math.Cos(Math.PI / 180 * angx);
                s = Math.Sin(Math.PI / 180 * angx);

                c1 = cosx(q);
                s1 = sinx(q);

            }

            double[,] TT1 = new double[,]
			{
				{c1, -s1, 0, 0, 0, 0},
				{s1, c1, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, -s, 0},
				{0, 0, 0, s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};


            double[,] ML = new double[,]
			{
				{E * A / L, 0, 0, -E * A / L, 0, 0},
				{0, (12 * E * I) / Math.Pow(L, 3), (6 * E * I) / Math.Pow(L, 2), 0, -(12 * E * I) / Math.Pow(L, 3), (6 * E * I) / Math.Pow(L, 2)},
				{0, (6 * E * I) / Math.Pow(L, 2), (4 * E * I) / L, 0, -(6 * E * I) / Math.Pow(L, 2), (2 * E * I) / L},
				{-E * A / L, 0, 0, E * A / L, 0, 0},
				{0, -(12 * E * I) / Math.Pow(L, 3), -(6 * E * I) / Math.Pow(L, 2), 0, (12 * E * I) / Math.Pow(L, 3), -(6 * E * I) / Math.Pow(L, 2)},
				{0, (6 * E * I) / Math.Pow(L, 2), (2 * E * I) / L, 0, -(6 * E * I) / Math.Pow(L, 2), (4 * E * I) / L}
			};



            double[,] T1 = new double[,]
			{
				{c1, s1, 0, 0, 0, 0},
				{-s1, c1, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, s, 0},
				{0, 0, 0, -s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};


            Cmp1 = Multiply_2D_2D_DiffSiz_ii_00(Multiply_2D_2D_DiffSiz_ii_00(TT1, ML), T1);







            return Cmp1;
        }



        private double[,] K_2_G_Element_i_1_j_1_INCLINED_SUP(int Memb_ID)
        {

            double[,] Cmp = new double[0, 0];
            double[,] Cmp1 = new double[0, 0];
            double I = 0;
            double A = 0;
            double E = 0;
            double L = 0;

            double c = 0;
            double s = 0;


            int q = Memb_ID;

            L = L_n(q);
            I = Iz[q];
            A = Ayz[q];
            E = Ey[q];
            c = cosx(q);
            s = sinx(q);

            if (Iz[q] == 0 | Ayz[q] == 0 | Ey[q] == 0)
            {
                MessageBox.Show("Material property unspecified");
            }


            int NodeNumx = 0;
            NodeNumx = Inclied_Supp_Node_num;
            double angx = 0;
            double c1 = 0;
            double s1 = 0;

            c1 = cosx(q);
            s1 = sinx(q);
            c = cosx(q);
            s = sinx(q);


            angx = -Inc_Supp_angle_base_AND_GX + Theta(q);

            if (Connectivity[q, 1] == Inclied_Supp_Node_num)
            {
                c1 = Math.Cos(Math.PI / 180 * angx);
                s1 = Math.Sin(Math.PI / 180 * angx);

                c = cosx(q);
                s = sinx(q);
                // Debug.Print(q + " K_2_G_Element_i_1_j_1_INCLINED_SUP== 1 c1,s1,c,s,angx = " + c1 + "," + s1 + "," + c + "," + s + "," + angx);
            }
            if (Connectivity[q, 2] == Inclied_Supp_Node_num)
            {
                c = Math.Cos(Math.PI / 180 * angx);
                s = Math.Sin(Math.PI / 180 * angx);

                c1 = cosx(q);
                s1 = sinx(q);
                // Debug.Print(q + " K_2_G_Element_i_1_j_1_INCLINED_SUP== 2 c1,s1,c,s,angx = " + c1 + "," + s1 + "," + c + "," + s + "," + angx);
            }

            double[,] TT1 = new double[,]
			{
				{c1, -s1, 0, 0, 0, 0},
				{s1, c1, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, -s, 0},
				{0, 0, 0, s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};


            double[,] ML = new double[,]
			{
				{E * A / L, 0, 0, -E * A / L, 0, 0},
				{0, (12 * E * I) / Math.Pow(L, 3), (6 * E * I) / Math.Pow(L, 2), 0, -(12 * E * I) / Math.Pow(L, 3), (6 * E * I) / Math.Pow(L, 2)},
				{0, (6 * E * I) / Math.Pow(L, 2), (4 * E * I) / L, 0, -(6 * E * I) / Math.Pow(L, 2), (2 * E * I) / L},
				{-E * A / L, 0, 0, E * A / L, 0, 0},
				{0, -(12 * E * I) / Math.Pow(L, 3), -(6 * E * I) / Math.Pow(L, 2), 0, (12 * E * I) / Math.Pow(L, 3), -(6 * E * I) / Math.Pow(L, 2)},
				{0, (6 * E * I) / Math.Pow(L, 2), (2 * E * I) / L, 0, -(6 * E * I) / Math.Pow(L, 2), (4 * E * I) / L}
			};



            double[,] T1 = new double[,]
			{
				{c1, s1, 0, 0, 0, 0},
				{-s1, c1, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, s, 0},
				{0, 0, 0, -s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};


            Cmp1 = Multiply_2D_2D_DiffSiz_ii_00(Multiply_2D_2D_DiffSiz_ii_00(TT1, ML), T1);



            double[,] cmp11 = new double[Cmp1.GetUpperBound(0) + 2, Cmp1.GetUpperBound(0) + 2];

            int i, j;


            for (i = 0; i <= Cmp1.GetUpperBound(0); i++)
            {
                for (j = 0; j <= Cmp1.GetUpperBound(1); j++)
                {

                    cmp11[i + 1, j + 1] = Cmp1[i, j];

                }
            }

            return cmp11;



        }


        private double[,] K_2_G_Element_i_1_j_1(int Memb_ID)
        {




            double[,] Cmp1;
            double I = 0;
            double A = 0;
            double E = 0;
            double L = 0;

            double c = 0;
            double s = 0;



            int i = 0;
            int j = 0;
            int q = Memb_ID;
            L = L_n(q);
            I = Iz[q];
            A = Ayz[q];
            E = Ey[q];
            c = cosx(q);
            s = sinx(q);


            if (Iz[q] == 0 | Ayz[q] == 0 | Ey[q] == 0)
            {
                MessageBox.Show("Material property not found");

            }


            double[,] TT1 = new double[,]
			{
				{c, -s, 0, 0, 0, 0},
				{s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, -s, 0},
				{0, 0, 0, s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};



            double[,] ML = new double[,]
			{
				{E * A / L, 0, 0, -E * A / L, 0, 0},
				{0, (12 * E * I) / Math.Pow(L, 3), (6 * E * I) / Math.Pow(L, 2), 0, -(12 * E * I) / Math.Pow(L, 3), (6 * E * I) / Math.Pow(L, 2)},
				{0, (6 * E * I) / Math.Pow(L, 2), (4 * E * I) / L, 0, -(6 * E * I) / Math.Pow(L, 2), (2 * E * I) / L},
				{-E * A / L, 0, 0, E * A / L, 0, 0},
				{0, -(12 * E * I) / Math.Pow(L, 3), -(6 * E * I) / Math.Pow(L, 2), 0, (12 * E * I) / Math.Pow(L, 3), -(6 * E * I) / Math.Pow(L, 2)},
				{0, (6 * E * I) / Math.Pow(L, 2), (2 * E * I) / L, 0, -(6 * E * I) / Math.Pow(L, 2), (4 * E * I) / L}
			};

            double[,] T1 = new double[,]
			{
				{c, s, 0, 0, 0, 0},
				{-s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, s, 0},
				{0, 0, 0, -s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};



            Cmp1 = Mult_2D_2D_Mat_00(Mult_2D_2D_Mat_00(TT1, ML), T1);


            double[,] cmp11 = new double[Cmp1.GetUpperBound(0) + 2, Cmp1.GetUpperBound(0) + 2];




            for (i = 0; i <= Cmp1.GetUpperBound(0); i++)
            {
                for (j = 0; j <= Cmp1.GetUpperBound(1); j++)
                {

                    cmp11[i + 1, j + 1] = Cmp1[i, j];

                }
            }

            return cmp11;



        }

        ///////////////////////////////////////////////////////////////



        private double[,] K_G_Geometric_11(int Memb_ID)
        {

            //12345
            double P = 0;
            int k = Memb_ID;
            double AF1 = Result_Mem_Force_Local[k, ((Connectivity[k, 1] * 3 - 2))];

            double AF2 = Result_Mem_Force_Local[k, ((Connectivity[k, 2] * 3 - 2))];
            if ((AF1 + AF2) != 0) P = (AF1 + AF2) / 2;
            if ((AF1 + AF2) == 0)
            {
                if (AF1 < 0) P = AF1;
                if (AF2 < 0) P = AF2;
            }

            Debug.Print(Memb_ID + "  =" + P + ", " + AF1 + ", " + AF2);
            double[,] Cmp1;
            double I = 0;
            double A = 0;
            double E = 0;
            double L = 0;

            double c = 0;
            double s = 0;



            int i = 0;
            int j = 0;
            int q = Memb_ID;
            L = L_n(q);






            double[,] TT1 = new double[,]
			{
				{c, -s, 0, 0, 0, 0},
				{s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, -s, 0},
				{0, 0, 0, s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};



            double[,] KGeo = new double[,]
			{
				{0, 0, 0, 0, 0, 0},
				{0, P * 1.2 / L, P * 0.1, 0, -P * 1.2 / L, P * 0.1},
				{0, P * 0.1, P * L * 2 / 15, 0, -P * 0.1, -P * L / 30},
				{0, 0, 0, 0, 0, 0},
				{0, -P * 1.2 / L, -P * 0.1, 0, P * 1.2 / L, -P * 0.1},
				{0, P * 0.1, -P * L / 30, 0, -P * 0.1, P * L * 2 / 15}
			};



            double[,] T1 = new double[,]
			{
				{c, s, 0, 0, 0, 0},
				{-s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, s, 0},
				{0, 0, 0, -s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};



            Cmp1 = Mult_2D_2D_Mat_00(Mult_2D_2D_Mat_00(TT1, KGeo), T1);


            double[,] cmp11 = new double[Cmp1.GetUpperBound(0) + 2, Cmp1.GetUpperBound(0) + 2];




            for (i = 0; i <= Cmp1.GetUpperBound(0); i++)
            {
                for (j = 0; j <= Cmp1.GetUpperBound(1); j++)
                {

                    cmp11[i + 1, j + 1] = Cmp1[i, j];

                }
            }

            return cmp11;



        }

        //=====================================================================================
        // Local geometric stiffness matrix for a 2D frame member
        private double[,] K_L_Geometric_00(int Memb_ID)
        {
            //12345
            double P = 0;
            int k = Memb_ID;
            double AF1 = Result_Mem_Force_Local[k, ((Connectivity[k, 1] * 3 - 2))];

            double AF2 = Result_Mem_Force_Local[k, ((Connectivity[k, 2] * 3 - 2))];
            if ((AF1 + AF2) != 0) P = (AF1 + AF2) / 2;
            if ((AF1 + AF2) == 0)
            {
                if (AF1 < 0) P = AF1;
                if (AF2 < 0) P = AF2;
            }
            Debug.Print(Memb_ID + " K_L_Geometric_00 =" + P + ", " + AF1 + ", " + AF2);
            int q = Memb_ID;
            double L = L_n(q);
            double[,] KGeo = new double[,]
			{
				{0, 0, 0, 0, 0, 0},
				{0, P * 1.2 / L, P * 0.1, 0, -P * 1.2 / L, P * 0.1},
				{0, P * 0.1, P * L * 2 / 15, 0, -P * 0.1, -P * L / 30},
				{0, 0, 0, 0, 0, 0},
				{0, -P * 1.2 / L, -P * 0.1, 0, P * 1.2 / L, -P * 0.1},
				{0, P * 0.1, -P * L / 30, 0, -P * 0.1, P * L * 2 / 15}
			};




            return KGeo;
        }

        //=====================================================================================

        private double[,] K_2_G_Element_i_1_j_1_Beam(int Memb_ID)
        {



            double[,] Cmp = new double[0, 0];
            double[,] Cmp1 = new double[0, 0];
            double I = 0;
            double A = 0;
            double E = 0;
            double L = 0;

            double c = 0;
            double s = 0;


            int q = Memb_ID;

            L = L_n(q);
            I = Iz[q];
            A = Ayz[q];
            E = Ey[q];
            c = cosx(q);
            s = sinx(q);
            double[,] mat3 = new double[7, 7];



            double[,] TT1 = new double[,]
			{
				{c, -s, 0, 0, 0, 0},
				{s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, -s, 0},
				{0, 0, 0, s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};



            double[,] ML = new double[,]
			{
				{E * A / L, 0, 0, -E * A / L, 0, 0},
				{0, (12 * E * I) / Math.Pow(L, 3), (6 * E * I) / Math.Pow(L, 2), 0, -(12 * E * I) / Math.Pow(L, 3), (6 * E * I) / Math.Pow(L, 2)},
				{0, (6 * E * I) / Math.Pow(L, 2), (4 * E * I) / L, 0, -(6 * E * I) / Math.Pow(L, 2), (2 * E * I) / L},
				{-E * A / L, 0, 0, E * A / L, 0, 0},
				{0, -(12 * E * I) / Math.Pow(L, 3), -(6 * E * I) / Math.Pow(L, 2), 0, (12 * E * I) / Math.Pow(L, 3), -(6 * E * I) / Math.Pow(L, 2)},
				{0, (6 * E * I) / Math.Pow(L, 2), (2 * E * I) / L, 0, -(6 * E * I) / Math.Pow(L, 2), (4 * E * I) / L}
			};



            double[,] T1 = new double[,]
			{
				{c, s, 0, 0, 0, 0},
				{-s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, s, 0},
				{0, 0, 0, -s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};



            Cmp1 = Multiply_2D_2D_DiffSiz_ii_00(Multiply_2D_2D_DiffSiz_ii_00(TT1, ML), T1);

            double[,] cmp11 = new double[Cmp1.GetUpperBound(0) + 2, Cmp1.GetUpperBound(0) + 2];


            int i, j;

            for (i = 1; i <= Cmp1.GetUpperBound(0); i++)
            {
                for (j = 1; j <= Cmp1.GetUpperBound(1); j++)
                {

                    cmp11[i + 1, j + 1] = Cmp1[i, j];

                }
            }

            return cmp11;

        }


        private double[,] K_Local_Element_Matrix_i_0_j_0(double LL, double AA, double II, double EE)
        {

            double[,] matrixD = new double[6, 6];





            matrixD[0, 0] = AA * EE / LL;
            matrixD[0, 1] = 0;
            matrixD[0, 2] = 0;
            matrixD[0, 3] = -AA * EE / LL;
            matrixD[0, 4] = 0;
            matrixD[0, 5] = 0;


            matrixD[1, 0] = 0;
            matrixD[1, 1] = 12 * EE * II / Math.Pow(LL, 3);
            matrixD[1, 2] = 6 * EE * II / Math.Pow(LL, 2);
            matrixD[1, 3] = 0;
            matrixD[1, 4] = -12 * EE * II / Math.Pow(LL, 3);
            matrixD[1, 5] = 6 * EE * II / Math.Pow(LL, 2);



            matrixD[2, 0] = 0;
            matrixD[2, 1] = 6 * EE * II / Math.Pow(LL, 2);
            matrixD[2, 2] = 4 * EE * II / LL;
            matrixD[2, 3] = 0;
            matrixD[2, 4] = -6 * EE * II / Math.Pow(LL, 2);
            matrixD[2, 5] = 2 * EE * II / LL;




            matrixD[3, 0] = -AA * EE / LL;
            matrixD[3, 1] = 0;
            matrixD[3, 2] = 0;
            matrixD[3, 3] = AA * EE / LL;
            matrixD[3, 4] = 0;
            matrixD[3, 5] = 0;



            matrixD[4, 0] = 0;
            matrixD[4, 1] = -12 * EE * II / Math.Pow(LL, 3);
            matrixD[4, 2] = -6 * EE * II / Math.Pow(LL, 2);
            matrixD[4, 3] = 0;
            matrixD[4, 4] = 12 * EE * II / Math.Pow(LL, 3);
            matrixD[4, 5] = -6 * EE * II / Math.Pow(LL, 2);


            matrixD[5, 0] = 0;
            matrixD[5, 1] = 6 * EE * II / Math.Pow(LL, 2);
            matrixD[5, 2] = 2 * EE * II / LL;
            matrixD[5, 3] = 0;
            matrixD[5, 4] = -6 * EE * II / Math.Pow(LL, 2);
            matrixD[5, 5] = 4 * EE * II / LL;

            return matrixD;
        }


        private double[,] K_2_G_Element_i_0_j_0(int Memb_ID)
        {



            double[,] Cmp = new double[0, 0];
            double[,] Cmp1 = new double[0, 0];
            double I = 0;
            double A = 0;
            double E = 0;
            double L = 0;

            double c = 0;
            double s = 0;


            int q = Memb_ID;

            L = L_n(q);
            I = Iz[q];
            A = Ayz[q];
            E = Ey[q];
            c = cosx(q);
            s = sinx(q);

            if (Iz[q] == 0 | Ayz[q] == 0 | Ey[q] == 0)
            {
                MessageBox.Show("Material property unspecified");
            }




            double[,] TT1 = new double[,]
			{
				{c, -s, 0, 0, 0, 0},
				{s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, -s, 0},
				{0, 0, 0, s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};



            double[,] ML = new double[,]
			{
				{E * A / L, 0, 0, -E * A / L, 0, 0},
				{0, (12 * E * I) / Math.Pow(L, 3), (6 * E * I) / Math.Pow(L, 2), 0, -(12 * E * I) / Math.Pow(L, 3), (6 * E * I) / Math.Pow(L, 2)},
				{0, (6 * E * I) / Math.Pow(L, 2), (4 * E * I) / L, 0, -(6 * E * I) / Math.Pow(L, 2), (2 * E * I) / L},
				{-E * A / L, 0, 0, E * A / L, 0, 0},
				{0, -(12 * E * I) / Math.Pow(L, 3), -(6 * E * I) / Math.Pow(L, 2), 0, (12 * E * I) / Math.Pow(L, 3), -(6 * E * I) / Math.Pow(L, 2)},
				{0, (6 * E * I) / Math.Pow(L, 2), (2 * E * I) / L, 0, -(6 * E * I) / Math.Pow(L, 2), (4 * E * I) / L}
			};



            double[,] T1 = new double[,]
			{
				{c, s, 0, 0, 0, 0},
				{-s, c, 0, 0, 0, 0},
				{0, 0, 1, 0, 0, 0},
				{0, 0, 0, c, s, 0},
				{0, 0, 0, -s, c, 0},
				{0, 0, 0, 0, 0, 1}
			};



            Cmp1 = Multiply_2D_2D_DiffSiz_ii_00(Multiply_2D_2D_DiffSiz_ii_00(TT1, ML), T1);




            return Cmp1;
        }




        private double[,] Multiply_2D_2D_DiffSiz_ii_00(double[,] Matrix1, double[,] Matrix2)
        {



            if (object.ReferenceEquals(null, Matrix1))
            {
                throw new ArgumentNullException("Matrix1");
            }
            else if (object.ReferenceEquals(null, Matrix2))
            {
                throw new ArgumentNullException("Matrix2");
            }

            int r1 = Matrix1.GetLength(0);
            int c1 = Matrix1.GetLength(1);

            int r2 = Matrix2.GetLength(0);
            int c2 = Matrix2.GetLength(1);

            if (c1 != r2)
            {
                throw new ArgumentOutOfRangeException("Matrix2", "Matrixes dimensions don't fit.");
            }

            double[,] result = new double[r1, c1];


            for (int r = 0; r < r1; r++)
            {
                for (int c = 0; c < c2; c++)
                {
                    double s = 0;

                    for (int z = 0; z < c1; z++)
                    {
                        s += Matrix1[r, z] * Matrix2[z, c];
                    }

                    result[r, c] = s;
                }
            }

            return result;
        }


        private string MakeDisplayable_Top_AND_Side_withIndex_3D3D_ij_1_1(double[, ,] threeD)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            string s1 = null;

            s1 = "";
            s1 = "MakeDisplayable_Top_AND_Side_withIndex_3D3D_ij_1_1 threeD(,,) Below Printed : ";
            s1 = s1 + Environment.NewLine;
            double[, ,] threeD_11 = new double[threeD.GetUpperBound(0), threeD.GetUpperBound(1) + 1, threeD.GetUpperBound(2) + 1];


            for (k = 1; k <= threeD.GetUpperBound(0); k++)
            {

                for (i = 0; i <= threeD.GetUpperBound(1); i++)
                {
                    for (j = 0; j <= threeD.GetUpperBound(2); j++)
                    {
                        threeD_11[k, i + 1, j + 1] = threeD[k, i, j];
                    }
                }
            }







            s1 = s1 + Environment.NewLine;

            for (k = 1; k <= threeD_11.GetUpperBound(0); k++)
            {

                s1 = s1 + "3D mem_id_ij_11= ...." + k.ToString() + Environment.NewLine;

                for (j = 1; j <= threeD_11.GetUpperBound(2); j++)
                {

                    s1 = s1 + "   " + j.ToString();

                }

                s1 = s1 + Environment.NewLine;



                for (i = 1; i <= threeD_11.GetUpperBound(1); i++)
                {
                    s1 = s1 + "     " + i.ToString() + "  ";
                    for (j = 1; j <= threeD_11.GetUpperBound(2); j++)
                    {

                        s1 = s1 + (threeD_11[k, i, j].ToString()) + " ";
                    }

                    s1 = s1 + Environment.NewLine;
                }


            }

            return s1;

        }
        private string MakeDisplayable_Top_AND_Side_with_CONNECTIVITY_Index_3D3D_ij_0_0(double[, ,] threeD)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            string s1 = null;
            string[] a = new string[6];




            s1 = "";
            s1 = s1 + "MakeDisplayable_Top_AND_Side_with_CONNECTIVITY_Index_3D3D_ij_0_0 threeD(,,) :Below Print:";
            s1 = s1 + Environment.NewLine;


            for (k = 1; k <= threeD.GetUpperBound(0); k++)
            {

                s1 = s1 + " mem_id= ...." + k.ToString() + Environment.NewLine;

                for (j = 1; j <= 3; j++)
                {

                    s1 = s1 + "   " + ((Connectivity[k, 1] * 3 - 3 + j)).ToString();
                    a[j - 1] = ((Connectivity[k, 1] * 3 - 3 + j)).ToString();

                }

                for (j = 1; j <= 3; j++)
                {

                    s1 = s1 + "   " + ((Connectivity[k, 2] * 3 - 3 + j)).ToString();
                    a[j - 1 + 3] = ((Connectivity[k, 2] * 3 - 3 + j)).ToString();

                }

                s1 = s1 + Environment.NewLine;



                for (i = 0; i <= threeD.GetUpperBound(1); i++)
                {
                    s1 = s1 + "     " + a[i].ToString() + "  ";

                    for (j = 0; j <= threeD.GetUpperBound(2); j++)
                    {

                        s1 = s1 + (threeD[k, i, j].ToString()) + " ";
                    }

                    s1 = s1 + Environment.NewLine;
                }


            }

            return s1;

        }

        //9691
        private string MakeDisplayable_HINGE_ij_11_MemIDComma2DARRAY_CONNECTIVITY_Index(int MemberID, double[,] Array2D)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            string s1 = "";
            string[] a = new string[Array2D.GetUpperBound(0) + 2];
            k = MemberID;
            try
            {

                s1 = "";
                s1 = "";
                s1 = s1 + Environment.NewLine;


                int[] IndH = HINGE_DOF_Index_1_6_MemID_1D(MemberID);


                for (j = 1; j <= 3; j++)
                {
                    //if (j == 1) { s1 = s1 + "         " + ((Connectivity[k, 1] * 3 - 3 + j)).ToString(); }
                    if (j == 1) { s1 = s1 + "         " + IndH[j].ToString(); }
                    else
                    {
                        //s1 = s1 + "   " + ((Connectivity[k, 1] * 3 - 3 + j)).ToString();
                        s1 = s1 + "   " + IndH[j].ToString();

                    }
                    //a[j - 1] = ((Connectivity[k, 1] * 3 - 3 + j)).ToString();
                    a[j - 1] = IndH[j].ToString();
                }

                for (j = 1; j <= 3; j++)
                {

                    //s1 = s1 + "   " + ((Connectivity[k, 2] * 3 - 3 + j)).ToString();
                    //a[j - 1 + 3] = ((Connectivity[k, 2] * 3 - 3 + j)).ToString();
                    s1 = s1 + "   " + IndH[j + 3].ToString();
                    a[j - 1 + 3] = IndH[j + 3].ToString();

                }

                s1 = s1 + Environment.NewLine;




                for (i = 1; i <= Array2D.GetUpperBound(0); i++)
                {
                    s1 = s1 + "     " + a[i - 1].ToString() + "  ";
                    for (j = 1; j <= Array2D.GetUpperBound(1); j++)
                    {

                        s1 = s1 + Array2D[i, j].ToString() + " ";
                    }

                    s1 = s1 + Environment.NewLine;
                }



            }
            catch (Exception ex)
            {

                Debug.Print(" MakeDisplayable_HINGE_ij_11_MemIDComma2DARRAY_CONNECTIVITY_Index(int MemberID, double[,] Array2D)" + ex.ToString());


            }
            return s1;

        }

        //===========================================
        private string MakeDisplayable_ij_11_MemIDComma2DARRAY_CONNECTIVITY_Index(int MemberID, double[,] Array2D)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            string s1 = "";
            string[] a = new string[Array2D.GetUpperBound(0) + 2];
            k = MemberID;
            try
            {

                s1 = "";
                s1 = "";
                s1 = s1 + Environment.NewLine;




                for (j = 1; j <= 3; j++)
                {
                    if (j == 1) { s1 = s1 + "         " + ((Connectivity[k, 1] * 3 - 3 + j)).ToString(); }
                    else
                    {
                        s1 = s1 + "   " + ((Connectivity[k, 1] * 3 - 3 + j)).ToString();

                    }
                    a[j - 1] = ((Connectivity[k, 1] * 3 - 3 + j)).ToString();
                }

                for (j = 1; j <= 3; j++)
                {

                    s1 = s1 + "   " + ((Connectivity[k, 2] * 3 - 3 + j)).ToString();
                    a[j - 1 + 3] = ((Connectivity[k, 2] * 3 - 3 + j)).ToString();

                }

                s1 = s1 + Environment.NewLine;




                for (i = 1; i <= Array2D.GetUpperBound(0); i++)
                {
                    s1 = s1 + "     " + a[i - 1].ToString() + "  ";
                    for (j = 1; j <= Array2D.GetUpperBound(1); j++)
                    {

                        s1 = s1 + Array2D[i, j].ToString() + " ";
                    }

                    s1 = s1 + Environment.NewLine;
                }



            }
            catch (Exception ex)
            {

                Debug.Print(" MakeDisplayable_ij_11_MemIDComma2DARRAY_CONNECTIVITY_Index(int MemberID, double[,] Array2D)" + ex.ToString());


            }
            return s1;

        }

        private int Find_Node_Num_From_DOF_Num_int(int n)
        {

            int nodenumber = 0;
            if (n > 0) nodenumber = (int)Math.Ceiling(n / 3d);


            return nodenumber;
        }



        private string[] Find_Node_Num_From_DOF_Num_string(int nn)
        {
            string[] s = new string[4];
            int nodenumber = 0;

            int n = 16;
            int m = n % 3;
            double d = nn / 3d;
            nodenumber = Convert.ToInt32(Math.Ceiling(d));

            if (n % 3 == 1) s[1] = "FX";
            if (n % 3 == 2) s[2] = "FY";
            if (n % 3 == 0) s[3] = Convert.ToInt32(Math.Ceiling(d)).ToString();

            return s;
        }

        private string PRINT_2D_ARRAY_ALL_MemID_CONN(double[,] Array2D)
        {
            int j = 0;
            int k = 0;
            string s1 = "";

            s1 = "";

            {
                if (Array2D.GetUpperBound(1) > 4)
                {



                    for (k = 1; k <= totalmember; k++)
                    {
                        s1 = s1 + "MakeDisplayable_2D_ARRAY_ALL_MemID_CONN: mem_id= " + k.ToString() + "   NODE   " + Connectivity[k, 1] + " TO " + Connectivity[k, 2];

                        s1 = s1 + Environment.NewLine;
                        for (j = 1; j <= 3; j++)
                        {

                            s1 = s1 + "        " + ((Connectivity[k, 1] * 3 - 3 + j)).ToString() + " ";


                        }

                        for (j = 1; j <= 3; j++)
                        {

                            s1 = s1 + "        " + ((Connectivity[k, 2] * 3 - 3 + j)).ToString() + " ";


                        }



                        s1 = s1 + Environment.NewLine;


                        for (j = 1; j <= 3; j++)
                        {

                            s1 = s1 + Array2D[k, ((Connectivity[k, 1] * 3 - 3 + j))].ToString() + " ";

                        }
                        for (j = 1; j <= 3; j++)
                        {

                            s1 = s1 + Array2D[k, ((Connectivity[k, 2] * 3 - 3 + j))].ToString() + " ";


                        }
                        s1 = s1 + Environment.NewLine;

                        s1 = s1 + Environment.NewLine;

                    }


                }


            }

            return s1;


        }




        private string MakeDisplayable_1D_0_Double_Mem_Index(int mid, double[] Array1D)
        {
            int col = 0;
            string s1 = null;
            string s2 = null;
            string s3 = null;

            int[] indx = Index_6_From_MemID_1D_1_7_HINGE(mid);
            s1 = "";
            s2 = "";
            s3 = "Double array print as below: ";
            for (col = 0; col <= Array1D.GetUpperBound(0); col++)
            {

                s2 = s2 + indx[col].ToString() + " ";
                s1 = s1 + (Array1D[col].ToString()) + " ";

            }

            s1 = s3 + Environment.NewLine + s2 + Environment.NewLine + s1;
            return s1;

        }



        private string MakeDisplayable_1D_0_Double(double[] Array1D)
        {
            int col = 0;
            string s1 = null;
            string s2 = null;
            string s3 = null;


            s1 = "";
            s2 = "";
            s3 = "Double array print as below: ";
            for (col = 0; col <= Array1D.GetUpperBound(0); col++)
            {

                s2 = s2 + col.ToString() + " ";
                s1 = s1 + (Array1D[col].ToString()) + " ";

            }

            s1 = s3 + Environment.NewLine + s2 + Environment.NewLine + s1;
            return s1;

        }




        private string MakeDisplayable_1D_1_Double(double[] Array1D)
        {
            int col = 0;
            string s1 = null;
            string s2 = null;
            string s3 = null;


            s1 = "";
            s2 = "";
            s3 = "Double array print as below: ";
            for (col = 1; col <= Array1D.GetUpperBound(0); col++)
            {

                s2 = s2 + col.ToString() + " ";
                s1 = s1 + (Array1D[col].ToString()) + " ";

            }

            s1 = s3 + Environment.NewLine + s2 + Environment.NewLine + s1;
            return s1;

        }


        private string MakeDisplayable_ij_00_2DARRAY_CONNECTIVITY_Index(double[,] Array2D)
        {




            int i = 0;
            int j = 0;
            string s1 = null;
            string[] a = new string[100001];
            try
            {

                s1 = "";

                s1 = s1 + "MakeDisplayable_ij_00_2DARRAY_CONNECTIVITY_Index - array print as below: ";


                s1 = s1 + Environment.NewLine;



                s1 = s1 + Environment.NewLine;

                for (j = 0; j <= Array2D.GetUpperBound(0); j++)
                {

                    s1 = s1 + "   " + j.ToString();


                }



                s1 = s1 + Environment.NewLine;




                for (i = 0; i <= Array2D.GetUpperBound(0); i++)
                {
                    s1 = s1 + i.ToString() + "  ";

                    for (j = 0; j <= Array2D.GetUpperBound(1); j++)
                    {

                        s1 = s1 + " " + Array2D[i, j].ToString();
                    }

                    s1 = s1 + " " + Environment.NewLine;
                }



            }
            catch (Exception ex)
            {


                Debug.Print("  MakeDisplayable_ij_00_2DARRAY_CONNECTIVITY_Index(double[,] Array2D) ex :" + Environment.NewLine + ex.ToString());

            }
            return s1;

        }




        private string MakeDisplayable_ij_11_2DARRAY_CONNECTIVITY_Index(double[,] Array2D)
        {




            int i = 0;
            int j = 0;
            string s1 = null;
            string[] a = new string[100001];
            try
            {

                s1 = "";

                s1 = s1 + "MakeDisplayable_ij_00_2DARRAY_CONNECTIVITY_Index - array print as below: ";


                s1 = s1 + Environment.NewLine;



                s1 = s1 + Environment.NewLine;

                for (j = 1; j <= Array2D.GetUpperBound(0); j++)
                {

                    s1 = s1 + "   " + j.ToString();


                }



                s1 = s1 + Environment.NewLine;




                for (i = 1; i <= Array2D.GetUpperBound(0); i++)
                {
                    s1 = s1 + i.ToString() + "  ";

                    for (j = 1; j <= Array2D.GetUpperBound(1); j++)
                    {

                        s1 = s1 + " " + Array2D[i, j].ToString();
                    }

                    s1 = s1 + " " + Environment.NewLine;
                }



            }
            catch (Exception ex)
            {


                Debug.Print("  MakeDisplayable_ij_11_2DARRAY_CONNECTIVITY_Index(double[,] Array2D) ex :" + Environment.NewLine + ex.ToString());

            }



            return s1;

        }



        private string MakeDisplayable_2D_ARRAY_ALL_MemID_CONN(double[,] Array2D)
        {
            int j = 0;
            int k = 0;
            string s1 = "";

            s1 = "";
            try
            {
                if (Array2D.GetUpperBound(1) > 4)
                {



                    for (k = 1; k <= totalmember; k++)
                    {
                        s1 = s1 + "MakeDisplayable_2D_ARRAY_ALL_MemID_CONN: mem_id= " + k.ToString() + "   NODE   " + Connectivity[k, 1] + " TO " + Connectivity[k, 2];

                        s1 = s1 + Environment.NewLine;
                        for (j = 1; j <= 3; j++)
                        {

                            s1 = s1 + "        " + ((Connectivity[k, 1] * 3 - 3 + j)).ToString() + " ";


                        }

                        for (j = 1; j <= 3; j++)
                        {

                            s1 = s1 + "        " + ((Connectivity[k, 2] * 3 - 3 + j)).ToString() + " ";


                        }



                        s1 = s1 + Environment.NewLine;


                        for (j = 1; j <= 3; j++)
                        {

                            s1 = s1 + Array2D[k, ((Connectivity[k, 1] * 3 - 3 + j))].ToString() + " ";

                        }
                        for (j = 1; j <= 3; j++)
                        {

                            s1 = s1 + Array2D[k, ((Connectivity[k, 2] * 3 - 3 + j))].ToString() + " ";

                        }
                        s1 = s1 + Environment.NewLine;

                        s1 = s1 + Environment.NewLine;

                    }


                }

            }
            catch (Exception ex)
            {


                Debug.Print(" MakeDisplayable_ij_00_2DARRAY_CONNECTIVITY_Index(double[,] Array2D) " + ex.ToString());
            }
            return s1;


        }



        private string MakeDisplayable_2D_ARRAY_ALL_MemID_Member_Index(double[,] Array2D)
        {
            int j = 0;
            int k = 0;
            string s1 = "";

            s1 = "";
            try
            {

                for (k = 1; k <= totalmember; k++)
                {
                    int[] Index_1 = Index_6_From_MemID_1D_1_7_HINGE(k);
                    s1 = s1 + "MakeDisplayable_2D_ARRAY_ALL_MemID_CONN: mem_id= " + k.ToString() + "   NODE   " + Connectivity[k, 1] + " TO " + Connectivity[k, 2];

                    s1 = s1 + Environment.NewLine;
                    for (j = 1; j <= 3; j++)
                    {


                        s1 = s1 + "   " + Index_1[j].ToString();

                    }

                    for (j = 1; j <= 3; j++)
                    {


                        s1 = s1 + "   " + Index_1[j + 3].ToString();


                    }



                    s1 = s1 + Environment.NewLine;



                    for (j = 1; j <= 3; j++)
                    {


                        s1 = s1 + Array2D[k, Index_1[j]].ToString() + " ";


                    }
                    for (j = 1; j <= 3; j++)
                    {


                        s1 = s1 + Array2D[k, Index_1[j + 3]].ToString() + " ";

                    }
                    s1 = s1 + Environment.NewLine;

                    s1 = s1 + Environment.NewLine;

                }


            }
            catch (Exception ex)
            {


                Debug.Print(" MakeDisplayable_2D_ARRAY_ALL_MemID_Member_Index(double[,] Array2D) " + ex.ToString());
            }
            return s1;


        }




        private string PRINT_1D_ARRAY_GLOBAL_REACTION_X_Y(double[] Array1D)
        {
            int col = 0;
            string s1 = "";
            string s2 = "";


            s1 = "";
            s1 = s1 + Environment.NewLine;
            s1 = s1 + "PRINT SUPPORT REACTION:- FORCE GX, FORCE GY, MOMENT Z(ALL UNITS ARE kN AND METER):";
            s1 = s1 + Environment.NewLine;

            for (col = 1; col <= totalnode; col++)
            {
                if (Find_Has_INCLINED_Support_ANY_Mem(col) == false)
                {
                    if (col == Sup_Node_Number[col])
                    {

                        s1 = s1 + "           " + "FORCE GX" + "      " + "FORCE GY" + "      " + "MOMENT Z";
                        s1 = s1 + Environment.NewLine;


                        s1 = s1 + "JOINT# " + Sup_Node_Number[col].ToString() + "       " + (Array1D[col * 3 - 2]).ToString("0.00") + "      " + (Array1D[col * 3 - 1]).ToString("0.00") + "        " + Array1D[col * 3 - 0].ToString("0.00");

                        s1 = s1 + Environment.NewLine;

                    }



                }

                if (Find_Has_INCLINED_Support_ANY_Mem(col) == true)
                {
                    ////s2 = "INCLINED ROLLER SUPPORT REACTION = " + Inc_Supp_Reaction + " , " + col + " , " + Inc_Supp_angle_base_AND_GX;
                    s2 = s2 + Environment.NewLine;

                    s2 = s2 + "           " + "FORCE GX" + "      " + "FORCE GY" + "      " + "MOMENT Z";
                    s2 = s2 + Environment.NewLine;

                    double xx, yy;
                    xx = -Inc_Supp_Reaction * Math.Sin(Inc_Supp_angle_base_AND_GX * Math.PI / 180);
                    yy = Inc_Supp_Reaction * Math.Cos(Inc_Supp_angle_base_AND_GX * Math.PI / 180);
                    s2 = s2 + "JOINT# " + Sup_Node_Number[col].ToString() + "       " + xx.ToString("0.00") + "      " + yy.ToString("0.00") + "      " + 0.ToString("0.00");
                    //s2 = s2 + Environment.NewLine;

                    //=============================

                }

                s1 = s1 + s2;

            }
            s1 = s1 + Environment.NewLine;

            return s1;

        }




        private string PRINT_1D_ARRAY_ALL_GLOBAL_DISP_X_Y_Z(double[] Array1D)
        {
            int col = 0;
            string s1 = "";



            s1 = "";
            s1 = s1 + Environment.NewLine;
            s1 = s1 + "PRINT GLOBAL DISPLACEMENT X Y Z ROTATION: JOINT DISPLACEMENT (ALL UNITS ARE CM AND RADIAN) :";
            s1 = s1 + Environment.NewLine;

            for (col = 1; col <= totalnode; col++)
            {



                s1 = s1 + "             " + "GLOBAL X" + "      " + "GLOBAL Y" + "      " + "ROTATION Z";
                s1 = s1 + Environment.NewLine;


                s1 = s1 + "JOINT# " + col.ToString() + "       " + (Array1D[col * 3 - 2] * 100).ToString("0.0000") + "        " + (Array1D[col * 3 - 1] * 100).ToString("0.0000") + "      " + Array1D[col * 3 - 0].ToString("0.0000");

                s1 = s1 + Environment.NewLine;

            }


            s1 = s1 + Environment.NewLine;

            return s1;

        }





        private string PRINT_2D_ARRAY_ALL_MemID_CONN_GLOBAL_FX_FY_MZ(double[,] Array2D)
        {
            int j = 0;
            int k = 0;
            string s1 = "";

            string sx = "";
            int JointNum = 0;
            try
            {

                {



                    s1 = s1 + "PRINT GLOBAL MEMBER END FORCES (ALL UNITS ARE KN AND METER): ";
                    s1 = s1 + Environment.NewLine;

                    for (k = 1; k <= totalmember; k++)
                    {
                        s1 = s1 + Environment.NewLine;


                        s1 = s1 + Environment.NewLine;
                        for (j = 1; j <= 3; j++)
                        {
                            if ((Connectivity[k, 1] * 3 - 3 + j) % 3 == 1) sx = "FX";
                            if ((Connectivity[k, 1] * 3 - 3 + j) % 3 == 2) sx = "FY";
                            if ((Connectivity[k, 1] * 3 - 3 + j) % 3 == 0) sx = "MOM Z";
                            if ((Connectivity[k, 1] * 3 - 3 + j) % 3 == 0) JointNum = (Connectivity[k, 1] * 3 - 3 + j) / 3;
                            s1 = s1 + " " + sx + " ";


                        }
                        s1 = s1 + Environment.NewLine;

                        s1 = s1 + "MEMBER# " + k.ToString() + " Joint# " + JointNum.ToString() + ": ";
                        for (j = 1; j <= 3; j++)
                        {

                            s1 = s1 + "            " + Array2D[k, ((Connectivity[k, 1] * 3 - 3 + j))].ToString("0.00") + "      ";

                        }
                        s1 = s1 + Environment.NewLine;


                        for (j = 1; j <= 3; j++)
                        {
                            if ((Connectivity[k, 2] * 3 - 3 + j) % 3 == 1) sx = "FX";
                            if ((Connectivity[k, 2] * 3 - 3 + j) % 3 == 2) sx = "FY";
                            if ((Connectivity[k, 2] * 3 - 3 + j) % 3 == 0) sx = "MOM Z";
                            if ((Connectivity[k, 2] * 3 - 3 + j) % 3 == 0) JointNum = (Connectivity[k, 2] * 3 - 3 + j) / 3;
                            s1 = s1 + "" + sx + " ";


                        }
                        s1 = s1 + Environment.NewLine;

                        s1 = s1 + "MEMBER# " + k.ToString() + " Joint# " + JointNum.ToString() + ": ";
                        for (j = 1; j <= 3; j++)
                        {

                            s1 = s1 + "             " + Array2D[k, ((Connectivity[k, 2] * 3 - 3 + j))].ToString("0.00") + "      ";

                        }
                    }
                }
                s1 = s1 + Environment.NewLine;

            }
            catch (Exception ex)
            {

                Debug.Print(" PRINT_2D_ARRAY_ALL_MemID_CONN_GLOBAL_FX_FY_MZ(double[,] Array2D) " + ex.ToString());
            }
            return s1;


        }





        private string PRINT_2D_ARRAY_ALL_MemID_CONN_LOCAL_AF_SHEAR_MZ(double[,] Array2D)
        {
            int j = 0;
            int k = 0;
            string s1 = "";

            string sx = "";
            int JointNum = 0;
            try
            {

                {



                    s1 = s1 + "PRINT LOCAL MEMBER END FORCES (ALL UNITS ARE KN AND METER): ";
                    s1 = s1 + Environment.NewLine;

                    for (k = 1; k <= totalmember; k++)
                    {
                        s1 = s1 + Environment.NewLine;

                        s1 = s1 + Environment.NewLine;
                        for (j = 1; j <= 3; j++)
                        {
                            if ((Connectivity[k, 1] * 3 - 3 + j) % 3 == 1) sx = "       AXIAL FORCE";
                            if ((Connectivity[k, 1] * 3 - 3 + j) % 3 == 2) sx = "SHEAR";
                            if ((Connectivity[k, 1] * 3 - 3 + j) % 3 == 0) sx = "MOM Z";
                            if ((Connectivity[k, 1] * 3 - 3 + j) % 3 == 0) JointNum = (Connectivity[k, 1] * 3 - 3 + j) / 3;
                            s1 = s1 + "         " + sx + "";


                        }
                        s1 = s1 + Environment.NewLine;

                        s1 = s1 + "MEMBER# " + k.ToString() + " Joint# " + JointNum.ToString() + ": ";
                        for (j = 1; j <= 3; j++)
                        {

                            s1 = s1 + Array2D[k, ((Connectivity[k, 1] * 3 - 3 + j))].ToString("0.00") + "         ";

                        }
                        s1 = s1 + Environment.NewLine;


                        for (j = 1; j <= 3; j++)
                        {
                            if ((Connectivity[k, 2] * 3 - 3 + j) % 3 == 1) sx = "       AXIAL FORCE";
                            if ((Connectivity[k, 2] * 3 - 3 + j) % 3 == 2) sx = "SHEAR";
                            if ((Connectivity[k, 2] * 3 - 3 + j) % 3 == 0) sx = "MOM Z";
                            if ((Connectivity[k, 2] * 3 - 3 + j) % 3 == 0) JointNum = (Connectivity[k, 2] * 3 - 3 + j) / 3;
                            s1 = s1 + "         " + sx + "";


                        }
                        s1 = s1 + Environment.NewLine;

                        s1 = s1 + "MEMBER# " + k.ToString() + " Joint# " + JointNum.ToString() + ": ";
                        for (j = 1; j <= 3; j++)
                        {

                            s1 = s1 + Array2D[k, ((Connectivity[k, 2] * 3 - 3 + j))].ToString("0.00") + "         ";

                        }
                    }
                }
                s1 = s1 + Environment.NewLine;

            }
            catch (Exception ex)
            {

                Debug.Print(" PRINT_2D_ARRAY_ALL_MemID_CONN_LOCAL_AF_SHEAR_MZ(double[,] Array2D)" + ex.ToString());
            }
            return s1;


        }





        private string MakeDisplayable_ij_00_MemIDComma2DARRAY__CONNECTIVITY_Index(int MemberID, double[,] Array2D)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            string s1 = "";
            string[] a = new string[Array2D.GetUpperBound(0) + Array2D.GetUpperBound(1) + 2];


            Array.Clear(a, 0, a.Length);

            k = MemberID;

            {
                if (MemberID >= 1 & Array2D.GetUpperBound(0) > 1 & Array2D.GetUpperBound(1) > 1)
                {
                    s1 = "";

                    s1 = s1 + "MemID Comma 2DARRAY....MakeDisplayable_ij_00_MemIDComma2DARRAY__CONNECTIVITY_Index - array print as below: ";


                    s1 = s1 + Environment.NewLine;



                    s1 = s1 + "2DARRAY....MakeDisplayable_ij_00.. mem_id= " + k.ToString() + Environment.NewLine;
                    s1 = s1 + Environment.NewLine + "     ";
                    for (j = 1; j <= Array2D.GetUpperBound(1); j++)
                    {
                        s1 = s1 + "   " + j.ToString();
                    }
                    s1 = s1 + Environment.NewLine + "     ";

                    for (j = 0; j <= 3; j++)
                    {

                        s1 = s1 + "   " + ((Connectivity[k, 1] * 3 - 3 + j)).ToString("0", CultureInfo.InvariantCulture);
                        a[j - 1] = ((Connectivity[k, 1] * 3 - 3 + j)).ToString("0", CultureInfo.InvariantCulture);


                    }

                    for (j = 0; j <= 3; j++)
                    {
                        s1 = s1 + "   " + ((Connectivity[k, 2] * 3 - 3 + j)).ToString("0", CultureInfo.InvariantCulture);
                        a[j - 1 + 3] = ((Connectivity[k, 2] * 3 - 3 + j)).ToString("0", CultureInfo.InvariantCulture);

                    }

                    s1 = s1 + Environment.NewLine;



                    for (i = 0; i <= Array2D.GetUpperBound(0); i++)
                    {
                        s1 = s1 + "     " + (a[i]).ToString() + "  ";

                        for (j = 0; j <= Array2D.GetUpperBound(1); j++)
                        {


                            s1 = s1 + Array2D[i, j].ToString() + " ";



                        }

                        s1 = s1 + Environment.NewLine;
                    }




                }


            }







            s1 = string.Format(CultureInfo.InvariantCulture, "{0:#,#}", s1);

            return s1;

        }



        private string MakeDisplayable_ij_00_MemIDComma2DARRAY_Member_Index(int MemberID, double[,] Array2D)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            string s1 = "";
            string[] a = new string[Array2D.GetUpperBound(0) + Array2D.GetUpperBound(1) + 2];


            Array.Clear(a, 0, a.Length);

            k = MemberID;
            int[] Index_1 = Index_6_From_MemID_1D_1_7_HINGE(k);

            {
                if (MemberID >= 1 & Array2D.GetUpperBound(0) > 1 & Array2D.GetUpperBound(1) > 1)
                {
                    s1 = "";

                    s1 = s1 + "MemID Comma 2DARRAY....MakeDisplayable_ij_00_MemIDComma2DARRAY__CONNECTIVITY_Index - array print as below: ";


                    s1 = s1 + Environment.NewLine;



                    s1 = s1 + "2DARRAY....MakeDisplayable_ij_00.. mem_id= " + k.ToString() + Environment.NewLine;
                    s1 = s1 + Environment.NewLine + "     ";
                    for (j = 1; j <= Array2D.GetUpperBound(1); j++)
                    {
                        s1 = s1 + "   " + j.ToString();
                    }
                    s1 = s1 + Environment.NewLine + "     ";

                    for (j = 0; j <= 3; j++)
                    {

                        s1 = s1 + "   " + ((Connectivity[k, 1] * 3 - 3 + j)).ToString("0", CultureInfo.InvariantCulture);
                        a[j - 1] = ((Connectivity[k, 1] * 3 - 3 + j)).ToString("0", CultureInfo.InvariantCulture);

                        s1 = s1 + "   " + Index_1[j].ToString();
                        a[j - 1] = Index_1[j].ToString();
                    }

                    for (j = 0; j <= 3; j++)
                    {
                        s1 = s1 + "   " + ((Connectivity[k, 2] * 3 - 3 + j)).ToString("0", CultureInfo.InvariantCulture);


                        s1 = s1 + "   " + Index_1[j + 3].ToString();
                        a[j - 1 + 3] = Index_1[j + 3].ToString();
                    }

                    s1 = s1 + Environment.NewLine;



                    for (i = 0; i <= Array2D.GetUpperBound(0); i++)
                    {
                        s1 = s1 + "     " + (a[i]).ToString() + "  ";

                        for (j = 0; j <= Array2D.GetUpperBound(1); j++)
                        {


                            s1 = s1 + Array2D[i, j].ToString() + " ";



                        }

                        s1 = s1 + Environment.NewLine;
                    }




                }


            }







            s1 = string.Format(CultureInfo.InvariantCulture, "{0:#,#}", s1);

            return s1;

        }





        private double[,] Mult_2D_2D_Mat_00(double[,] matrixA, double[,] matrixB)
        {

            int r1 = matrixA.GetLength(0);
            int c1 = matrixA.GetLength(1);

            int r2 = matrixB.GetLength(0);
            int c2 = matrixB.GetLength(1);

            if (c1 != r2)
            {
                throw new ArgumentOutOfRangeException("Matrixes dimensions don't fit.");
            }


            double[,] matrixC = new double[matrixB.GetUpperBound(0) + 1, matrixB.GetUpperBound(1) + 1];
            int x = 0;
            int y = 0;
            int z = 0;
            for (x = 0; x <= matrixB.GetUpperBound(0); x++)
            {


                for (y = 0; y <= matrixB.GetUpperBound(0); y++)
                {
                    for (z = 0; z <= matrixB.GetUpperBound(0); z++)
                    {
                        matrixC[x, y] = matrixC[x, y] + (matrixA[x, z] * matrixB[z, y]);
                    }

                }
            }

            return matrixC;
        }










        private int[] Index_6_From_MemID_1D_i_0(int mem_id)
        {
            int[] Index_6 = new int[6];
            Index_6[0] = Connectivity[mem_id, 1] * 3 - 2;
            Index_6[1] = Connectivity[mem_id, 1] * 3 - 1;
            Index_6[2] = Connectivity[mem_id, 1] * 3;

            Index_6[3] = Connectivity[mem_id, 2] * 3 - 2;
            Index_6[4] = Connectivity[mem_id, 2] * 3 - 1;
            Index_6[5] = Connectivity[mem_id, 2] * 3;
            return Index_6;
        }
        private int[] Index_6_From_MemID_1D_i_1(int mem_id)
        {
            int[] Index_6 = new int[7];
            Index_6[0] = 0;
            Index_6[1] = Connectivity[mem_id, 1] * 3 - 2;
            Index_6[2] = Connectivity[mem_id, 1] * 3 - 1;
            Index_6[3] = Connectivity[mem_id, 1] * 3;

            Index_6[4] = Connectivity[mem_id, 2] * 3 - 2;
            Index_6[5] = Connectivity[mem_id, 2] * 3 - 1;
            Index_6[6] = Connectivity[mem_id, 2] * 3;
            return Index_6;
        }



        //============================
        private int[] HINGE_DOF_Index_1_6_MemID_1D(int mem_id)
        {
            int[] Hinge_Index = new int[(totalnode * 3) + 1 + HINGE_Num_total];
            int j, k;
            k = 0;
            int[] Index_1 = Index_6_From_MemID_1D_1_7_HINGE(mem_id);
            for (j = 1; j < Index_1.Length; j++)
            {
                if (Index_1[j] != 0)
                {
                    k = k + 1;

                    //Debug.Print( j + " =Hinge DOF 1 = " + Index_1[j]);

                    Hinge_Index[j] = Index_1[j];


                }

            }
            Array.Resize(ref Hinge_Index, k + 1);
            for (j = 1; j < Hinge_Index.Length; j++)
            {
                //Debug.Print(j + " =Hinge DOF 2= " + Hinge_Index[j]);
            }

            return Hinge_Index;
        }

        //============================


        private int[] Index_6_From_MemID_1D_1_7_HINGE(int mem_id)
        {
            int[] Index_6 = new int[(totalnode * 3) + 1 + HINGE_Num_total];

            Index_6[0] = 0;
            Index_6[1] = Connectivity[mem_id, 1] * 3 - 2;
            Index_6[2] = Connectivity[mem_id, 1] * 3 - 1;
            Index_6[3] = Connectivity[mem_id, 1] * 3;

            Index_6[4] = Connectivity[mem_id, 2] * 3 - 2;
            Index_6[5] = Connectivity[mem_id, 2] * 3 - 1;
            Index_6[6] = Connectivity[mem_id, 2] * 3;
            for (int k = 1; k <= totalnode_From_Lines; k++)
                if (mem_id == HINGE_MemID[k])
                {
                    Index_6[HINGE_MemEnd_Index[k] * 3] = HINGE_New_DOF[k];


                    // Debug.Print(mem_id + " mem_id, Index_6[HINGE_MemEnd_Index[k] * 3], HINGE_New_DOF[k] "  + Index_6[HINGE_MemEnd_Index[k] * 3] + " , " + HINGE_New_DOF[k]);

                }
            return Index_6;
        }



        private void Run_Analysis_HINGE()
        {

            double[] ff2 = new double[6];
            double[,] t_mem1 = null;

            double[,] t_mem2 = null;

            double[] Disp_Element_I_To_J = new double[6];
            double[] temp12G = new double[6];

            double[] cn = new double[6];

            double[] ForeEnd_j0_global = new double[6];
            double[] ForeEnd_j0_Local = new double[6];
            double[] Disp_j0_global = new double[6];
            double[] Disp_j0_Local = null;
            double[] dloc = new double[6];

            double[] ff10 = new double[6];
            double[] ff11 = new double[6];
            double[] qq1 = new double[6];
            double[] qq2 = new double[6];

            double[,] Result_Mem_Force_global_Fixed = new double[totalmember + 1, (totalnode * 3) + 1];

            double[,] Result_Mem_Force_Local_Fixed = new double[totalmember + 1, (totalnode * 3) + 1];

            Disp_All_Nodes_Local = new double[totalnode * 3 + 1];




            double[] SUM_mem = new double[(totalnode * 3) + 1];

            int i = 0;
            int j = 0;
            int k = 0;


            int[] DOF_Restrained_Gx = new int[3 * totalnode + 1];
            int[] DOF_Restrained = new int[3 * totalnode + 1];

            int[] ALL_DOF_Free_Restrained = new int[3 * totalnode + HINGE_Num_total];


            int[] ALL_Restrained_DOF_Free = new int[3 * totalnode + HINGE_Num_total];


            //Debug.Print("lenght_factor, HINGE_Num_total= " + lenght_factor + " , " + HINGE_Num_total);

            for (j = 0; j <= 3 * totalnode - 1 + HINGE_Num_total; j++)
            {

                ALL_DOF_Free_Restrained[j] = j + 1;
                // Debug.Print(j + "ALL_DOF_Free_Restrained:HINGE_Num_total= " + ALL_DOF_Free_Restrained[j] + "," + HINGE_Num_total);
            }

            // Debug.Print("ALL_DOF_Free_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(ALL_DOF_Free_Restrained));

            Array.Resize(ref suptype, totalnode + 1);
            Array.Resize(ref Sup_Node_Number, totalnode + 1);
            for (k = 1; k <= totalnode; k++)
            {
                string[] testx = new string[] { };
                if (!string.IsNullOrEmpty(Sup_Node_Number_Type[k]))
                {

                    testx = Sup_Node_Number_Type[k].Split(new char[] { ',' });
                    for (i = 0; i < testx.Length; i++)
                    {

                    }

                    Sup_Node_Number[k] = int.Parse(testx[0]);
                    suptype[k] = int.Parse(testx[1]);

                }
            }

            for (j = 1; j <= totalnode; j++)
            {


                if (suptype[j] == 1)
                {

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j]] = 3 * Sup_Node_Number[j];
                    // Debug.Print(j + "Sup_Node_Number[j]fixed:" + Sup_Node_Number[j]);
                }
                if (suptype[j] == 2)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;

                    // Debug.Print("Sup_Node_Number[j] hinged:" + Sup_Node_Number[j]);
                }
                if (suptype[j] == 3)
                {

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;

                    // Debug.Print("Sup_Node_Number[j] X, MZ free, Y  restrained ROLLER-Y:" + Sup_Node_Number[j]);
                }

                if (suptype[j] == 4)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;


                    // Debug.Print("Sup_Node_Number[j] Y, MZ free, X  restrained ROLLER-X:" + Sup_Node_Number[j]);
                }

                if (suptype[j] == 5)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j]] = 3 * Sup_Node_Number[j];

                }

            }


            // Debug.Print("DOF_Restrained_Gx :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Restrained_Gx));
            int tx = 0;
            k = 0;


            for (j = 0; j <= totalnode * 3; j++)
            {
                tx = DOF_Restrained_Gx[j];
                if (tx > 0)
                {


                    Array.Resize(ref DOF_Restrained, k + 1);
                    DOF_Restrained[k] = tx;
                    k = k + 1;
                }
            }
            // Debug.Print("DOF_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Restrained));


            bool_DOF_Restrained_G_Greater_Than_2 = true;
            if (DOF_Restrained.Length < 3)
            {
                bool_DOF_Restrained_G_Greater_Than_2 = false;
                // Debug.Print("DOF_Restrained.Length = " + DOF_Restrained.Length);
                MessageBox.Show("RUN ANALYSIS is QUITED because" + Environment.NewLine
                 + "the structure needs more supports !!! " + Environment.NewLine
                + "Degree of Restraint of 2D frame structure IS LESS THAN 3 !!!");
                return;

            }


            DOF_Free = Uncommon_Matrix_1D_1D_Intger(ALL_DOF_Free_Restrained, DOF_Restrained);
            bool bool_DOF_Free_Greater_Than_0 = true;
            if (DOF_Free.Length == 0) bool_DOF_Free_Greater_Than_0 = false;




            ALL_DOF_Free_Restrained = Union_Matrix_1D_1D_Intger(DOF_Free, DOF_Restrained);

            ALL_Restrained_DOF_Free = Union_Matrix_1D_1D_Intger(DOF_Restrained, DOF_Free);

            //Debug.Print("DOF_Free :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Free));
            //Debug.Print("ALL_DOF_Free_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(ALL_DOF_Free_Restrained));

            double[] Support_Displacement = new double[DOF_Restrained.Length];



            if (DOF_Free.Length > 0)
            {
                Debug.Print("DOF_Free :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Free));
            }
            else
            {
                Debug.Print("CAUTION - ALL DEGREES OF FREEDOM FIXED !!!");

            }
            Debug.Print("DOF_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Restrained));
            Debug.Print("ALL_DOF_Free_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(ALL_DOF_Free_Restrained));



            // Debug.Print("Support_Displacement :" + Environment.NewLine + MakeDisplayable_1D_0_Double(Support_Displacement));
            Array.Resize(ref   Support_Displacement_S, totalnode + 1);
            Array.Resize(ref Support_Displacement_temp, totalnode * 3 + 1);

            // Debug.Print("Support_Displacement :" + Environment.NewLine + MakeDisplayable_1D_0_Double(Support_Displacement));

            string[] test9 = new string[] { };


            // Debug.Print("Support_Displacement_S :" + Environment.NewLine + MakeDisplayable_1D_j_0_string(Support_Displacement_S));

            for (j = 1; j <= totalnode; j++)
            {
                if (!string.IsNullOrEmpty(Support_Displacement_S[j]))
                {

                    test9 = Support_Displacement_S[j].Split(new char[] { ',' });
                    for (i = 0; i < test9.Length; i++)
                    {

                        //Debug.Print(i + "Support_Displacement_S test  =" + test9[i]);
                    }

                    Support_Displacement_temp[j * 3 - 2] = double.Parse(test9[1]);
                    Support_Displacement_temp[j * 3 - 1] = double.Parse(test9[2]);
                }
            }


            for (j = 0; j < DOF_Restrained.Length; j++)
            {
                Support_Displacement[j] = Support_Displacement_temp[ALL_Restrained_DOF_Free[j]];
            }


            int mem_id = 0;
            double[] YL = new double[DOF_Free.Length];


            double[,] PjG_memEnd_MebCommaConnectivity = new double[totalmember + 1, (totalnode * 3) + 1];
            double[,] PjLOCAL_memEnd_MebCommaConnectivity = new double[totalmember + 1, (totalnode * 3) + 1];
            int k12 = 0;

            for (k12 = 1; k12 <= totalmember; k12++)
            {


                mem_id = k12;


                try
                {

                    if (!string.IsNullOrEmpty(Pload[mem_id]))
                    {

                        ForceEnd_Local_MemComaIndex = FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal(mem_id, Pload[mem_id]);
                        ForceEnd_Local_To_Global = FORCE_Eqiv_Nod_MembComaP_ij_1_0_TO_Global(mem_id, Pload[mem_id]);

                        // Debug.Print(" ForceEnd_Local_To_Global " + MakeDisplayable(ForceEnd_Local_To_Global));
                        // Debug.Print(" ForceEnd_Global_ALL_DOF " + MakeDisplayable(ForceEnd_Local_To_Global));


                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 2] = ForceEnd_Local_MemComaIndex[mem_id, 0];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 1] = ForceEnd_Local_MemComaIndex[mem_id, 1];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3] = ForceEnd_Local_MemComaIndex[mem_id, 2];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 2] = ForceEnd_Local_MemComaIndex[mem_id, 3];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 1] = ForceEnd_Local_MemComaIndex[mem_id, 4];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3] = ForceEnd_Local_MemComaIndex[mem_id, 5];

                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 2] = ForceEnd_Local_To_Global[mem_id, 0];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 1] = ForceEnd_Local_To_Global[mem_id, 1];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3] = ForceEnd_Local_To_Global[mem_id, 2];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 2] = ForceEnd_Local_To_Global[mem_id, 3];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 1] = ForceEnd_Local_To_Global[mem_id, 4];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3] = ForceEnd_Local_To_Global[mem_id, 5];

                    }

                }
                catch (Exception ex)
                {

                    Debug.Print("ForceEnd_Local_MemComaIndex = FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal +Global" + ex.ToString());
                }


            }




            // Debug.Print("PjLOCAL_memEnd_MebCommaConnectivity :" + Environment.NewLine + MakeDisplayable_2D_ARRAY_ALL_MemID_CONN(PjLOCAL_memEnd_MebCommaConnectivity));
            // Debug.Print("PjG_memEnd_MebCommaConnectivity :" + Environment.NewLine + MakeDisplayable_2D_ARRAY_ALL_MemID_CONN(PjG_memEnd_MebCommaConnectivity));



            double[,] PjLOCAL_memEnd_Mem_Index = new double[totalmember + 1, (totalnode * 3) + 1 + HINGE_Num_total];
            double[,] PjG_memEnd_Mem_Index = new double[totalmember + 1, (totalnode * 3) + 1 + HINGE_Num_total];



            for (k = 1; k <= totalmember; k++)
            {

                mem_id = k;



                {
                    int[] indx1 = Index_6_From_MemID_1D_1_7_HINGE(mem_id);




                    PjLOCAL_memEnd_Mem_Index[mem_id, indx1[1]] = PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 2];
                    PjLOCAL_memEnd_Mem_Index[mem_id, indx1[2]] = PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 1];
                    PjLOCAL_memEnd_Mem_Index[mem_id, indx1[3]] = PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3];
                    PjLOCAL_memEnd_Mem_Index[mem_id, indx1[4]] = PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 2];
                    PjLOCAL_memEnd_Mem_Index[mem_id, indx1[5]] = PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 1];
                    PjLOCAL_memEnd_Mem_Index[mem_id, indx1[6]] = PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3];


                    PjG_memEnd_Mem_Index[mem_id, indx1[1]] = PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 2];
                    PjG_memEnd_Mem_Index[mem_id, indx1[2]] = PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 1];
                    PjG_memEnd_Mem_Index[mem_id, indx1[3]] = PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3];
                    PjG_memEnd_Mem_Index[mem_id, indx1[4]] = PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 2];
                    PjG_memEnd_Mem_Index[mem_id, indx1[5]] = PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 1];
                    PjG_memEnd_Mem_Index[mem_id, indx1[6]] = PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3];

                }

            }

            for (k = 1; k <= totalmember; k++)
            {
                int[] indx1 = Index_6_From_MemID_1D_1_7_HINGE(k);
                for (j = 1; j <= 6; j++)
                {
                    // Debug.Print(k + "= mid " + " , " + j + " Index_6_From_MemID_1D_1_7_HINGE(k) " + indx1[j]);
                }
            }
            // Debug.Print("PjLOCAL_MakeDisplayable_2D_ARRAY_ALL_MemID_Member_Index :" + Environment.NewLine + MakeDisplayable_2D_ARRAY_ALL_MemID_Member_Index(PjLOCAL_memEnd_Mem_Index));
            // Debug.Print("PjG_MakeDisplayable_2D_ARRAY_ALL_MemID_Member_Index :" + Environment.NewLine + MakeDisplayable_2D_ARRAY_ALL_MemID_Member_Index(PjG_memEnd_Mem_Index));



            if (bool_DOF_Free_Greater_Than_0 == true)
            {
                Debug.Print("lenght_factor, HINGE_Num_total= " + lenght_factor + " , " + HINGE_Num_total);
                double[] Pj_joint_WithLoad = new double[(totalnode * 3) + 1 + HINGE_Num_total];
                double[] Pj_joint_ALL = new double[(totalnode * 3) + 1 + HINGE_Num_total];
                double[] Pmem_joint = new double[(totalnode * 3) + 1 + HINGE_Num_total];
                try
                {

                    ForceEnd_Local_ALL_DOF = new double[(totalnode * 3) + 1 + HINGE_Num_total];

                    ForceEnd_Global_ALL_DOF = new double[(totalnode * 3) + 1 + HINGE_Num_total];

                    for (j = 1; j <= DOF_Free.Length; j++)
                    {
                        for (k = 1; k <= totalmember; k++)
                        {
                            int[] indx1 = Index_6_From_MemID_1D_1_7_HINGE(k);
                            ForceEnd_Local_ALL_DOF[indx1[j]] = ForceEnd_Local_ALL_DOF[indx1[j]] + PjLOCAL_memEnd_Mem_Index[k, indx1[j]];
                            ForceEnd_Global_ALL_DOF[indx1[j]] = ForceEnd_Global_ALL_DOF[indx1[j]] + PjG_memEnd_Mem_Index[k, indx1[j]];
                        }

                    }
                    // Debug.Print(" ForceEnd_Local_ALL_DOF " + MakeDisplayable_1D_0_Double(ForceEnd_Local_ALL_DOF));
                    // Debug.Print(" ForceEnd_Global_ALL_DOF eqv nodal force" + MakeDisplayable_1D_0_Double(ForceEnd_Global_ALL_DOF));


                    SUM_mem = new double[(totalnode * 3) + 1 + HINGE_Num_total];


                    for (k = 1; k <= totalmember; k++)
                    {
                        int[] indx1 = Index_6_From_MemID_1D_1_7_HINGE(k);
                        for (j = 1; j <= 6; j++)
                        {
                            SUM_mem[indx1[j]] = SUM_mem[indx1[j]] + PjG_memEnd_Mem_Index[k, indx1[j]];
                        }
                    }


                    // Debug.Print(" SUM_mem :" + Environment.NewLine + MakeDisplayable_1D_0_Double(SUM_mem));


                    double[] ffjG = null;
                    for (j = 1; j <= totalnode; j++)
                    {

                        if (!string.IsNullOrEmpty(PJoint[j]))
                        {


                            ffjG = Joint_Global_force_0_3(PJoint[j]);
                            k = Convert.ToInt32(ffjG[0]);
                            if (k >= 1)
                            {
                                Pj_joint_WithLoad[3 * k - 2] = ffjG[1];
                                Pj_joint_WithLoad[3 * k - 1] = ffjG[2];
                                Pj_joint_WithLoad[3 * k] = ffjG[3];
                            }

                        }
                    }
                    Array.Resize(ref Pj_joint_WithLoad, (totalnode * 3) + 1 + HINGE_Num_total);



                    for (k = 1; k <= totalmember; k++)
                    {
                        int[] indx1 = Index_6_From_MemID_1D_1_7_HINGE(k);
                        for (j = 1; j <= 6; j++)


                            Pj_joint_ALL[indx1[j]] = Pj_joint_ALL[indx1[j]] + Pj_joint_WithLoad[indx1[j]];

                    }
                    // Debug.Print("  Pj_joint :" + Environment.NewLine + MakeDisplayable_1D_0_Double(Pj_joint_ALL));




                    for (k = 1; k <= totalmember; k++)
                    {
                        int[] indx1 = Index_6_From_MemID_1D_1_7_HINGE(k);
                        for (j = 1; j <= 6; j++)
                            Pmem_joint[indx1[j]] = SUM_mem[indx1[j]] + Pj_joint_ALL[indx1[j]];
                    }
                    // Debug.Print("  Pmem_joint(j ) :" + Environment.NewLine + MakeDisplayable_1D_0_Double(Pmem_joint));


                    YL = new double[DOF_Free.Length];
                    Debug.Print("" + Environment.NewLine + " Load Vector is as follows: .......... " + Environment.NewLine);
                    for (j = 0; j < DOF_Free.Length; j++)
                    {
                        YL[j] = Pmem_joint[DOF_Free[j]];

                        Debug.Print("DOF_Free[" + j + "] ,  " + "YL[" + j + "]  =" + DOF_Free[j] + "  , " + YL[j] + "");


                    }



                }
                catch (Exception ex)
                {

                    Debug.Print("AFTER End of load vector Run_Analysis_HINGE DOF_Free[j]   YL[j]=.....     " + Environment.NewLine + ex.ToString());
                }



                try
                {

                    double[,] Member_KG = null;
                    double[, ,] KGM_ALL = new double[totalmember + 1, totalnode * 3 + 1 + HINGE_Num_total, totalnode * 3 + 1 + HINGE_Num_total];
                    Array.Clear(KGM_ALL, 0, KGM_ALL.Length);
                    double[, ,] KGM_ALL2 = new double[totalmember + 1, totalnode * 3 + 1 + HINGE_Num_total, totalnode * 3 + 1 + HINGE_Num_total];
                    Array.Clear(KGM_ALL2, 0, KGM_ALL2.Length);

                    double[, ,] Mem_KG_mat = new double[totalmember + 1, 7, 7];

                    int i1 = 0;
                    int j1 = 0;




                    //============================
                    for (i = 1; i <= totalmember; i++)
                    {

                        Debug.Print("Member# " + i + "= Global Member Stiffness Matrix: " + Environment.NewLine + MakeDisplayable_HINGE_ij_11_MemIDComma2DARRAY_CONNECTIVITY_Index(i, K_2_G_Element_i_1_j_1(i)));
                        Debug.Print("Member# " + i + "= Index_6_From_MemID_1D_1_7_HINGE(i): " + MakeDisplayable_1D_j_1_INT(Index_6_From_MemID_1D_1_7_HINGE(i)));
                        Member_KG = K_2_G_Element_i_1_j_1(i);


                        int[] Index_1 = Index_6_From_MemID_1D_1_7_HINGE(i);

                        for (i1 = 1; i1 <= 6; i1++)
                        {

                            for (j1 = 1; j1 <= 6; j1++)
                            {

                                KGM_ALL2[i, Index_1[i1], Index_1[j1]] = Member_KG[i1, j1];

                            }
                        }







                        for (k = 1; k <= 3; k++)
                        {
                            for (j = 1; j <= 3; j++)
                            {


                                KGM_ALL[i, Connectivity[i, 1] * 3 - 3 + k, Connectivity[i, 1] * 3 - 3 + j] = Member_KG[k, j];


                                KGM_ALL[i, Connectivity[i, 1] * 3 - 3 + k, Connectivity[i, 2] * 3 - 3 + j] = Member_KG[k, j + 3];



                                KGM_ALL[i, Connectivity[i, 2] * 3 - 3 + k, Connectivity[i, 1] * 3 - 3 + j] = Member_KG[k + 3, j];


                                KGM_ALL[i, Connectivity[i, 2] * 3 - 3 + k, Connectivity[i, 2] * 3 - 3 + j] = Member_KG[k + 3, j + 3];


                            }
                        }

                        Array.Clear(Member_KG, 0, Member_KG.Length);

                    }




                    double[,] result = new double[totalnode * 3 + 1 + HINGE_Num_total, totalnode * 3 + 1 + HINGE_Num_total];
                    for (i = 1; i <= totalmember; i++)
                    {

                        for (j = 1; j <= totalnode * 3 + HINGE_Num_total; j++)
                        {

                            for (k = 1; k <= totalnode * 3 + HINGE_Num_total; k++)
                            {
                                result[j, k] = result[j, k] + KGM_ALL2[i, j, k];
                            }
                        }
                    }

                    Debug.Print("Structure Stiffness Matrix SSM  =" + MakeDisplayable_ij_11_2DARRAY_CONNECTIVITY_Index(result));







                    Result_Mem_Force_global = new double[totalmember + 1, (totalnode * 3) + 1 + HINGE_Num_total];
                    Result_Mem_Force_Local = new double[totalmember + 1, (totalnode * 3) + 1 + HINGE_Num_total];
                    Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0 = new double[totalmember + 1, (totalnode * 3) + 1 + HINGE_Num_total];
                    Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0 = new double[totalmember + 1, (totalnode * 3) + 1 + HINGE_Num_total];
                    Disp_All_Nodes_Global = new double[totalnode * 3 + 1 + HINGE_Num_total];




                    double[,] K_SSM_Rearranged = new double[(totalnode * 3) + 1 + HINGE_Num_total, (totalnode * 3) + 1 + HINGE_Num_total];

                    int[] K_SSM_Rearranged_oldindex_1DArray = new int[(totalnode * 3) + 1 + HINGE_Num_total];
                    double[,] K_SSM_Rearranged_oldindex_2DArray = new double[(totalnode * 3) + 1 + HINGE_Num_total, 5];

                    for (i = 1; i <= totalnode * 3 + HINGE_Num_total; i++)
                    {

                        for (j = 1; j <= totalnode * 3 + HINGE_Num_total; j++)
                        {

                            K_SSM_Rearranged[i, j] = result[ALL_DOF_Free_Restrained[i - 1], ALL_DOF_Free_Restrained[j - 1]];

                            K_SSM_Rearranged_oldindex_1DArray[j] = ALL_DOF_Free_Restrained[j - 1];

                            K_SSM_Rearranged_oldindex_2DArray[i, 3] = ALL_DOF_Free_Restrained[i - 1];
                            K_SSM_Rearranged_oldindex_2DArray[j, 4] = ALL_DOF_Free_Restrained[j - 1];


                        }
                    }


                    Debug.Print("DOF_Free + DOF_Restrained Rearranged index and Rearranged Structure Stiffness Matrix, SSM_Rearranged = " + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(K_SSM_Rearranged_oldindex_2DArray, K_SSM_Rearranged));



                    // Debug.Print("TOTAL FREE DOOF without Specified i.e. restrained DOF= " + DOF_Free.Length);
                    double[,] Mat_Kff_2Darray_OLDINDEX = new double[DOF_Free.Length + 1, 5];
                    double[,] Mat_Kff = new double[DOF_Free.Length, DOF_Free.Length];
                    double[,] Mat_Kff11 = new double[DOF_Free.Length, DOF_Free.Length];
                    for (i = 0; i < DOF_Free.Length; i++)
                    {
                        for (j = 0; j < DOF_Free.Length; j++)
                        {
                            Mat_Kff[i, j] = K_SSM_Rearranged[i + 1, j + 1];
                            Mat_Kff11[i, j] = K_SSM_Rearranged[i + 1, j + 1];

                            Mat_Kff_2Darray_OLDINDEX[i + 1, 3] = DOF_Free[i];
                            Mat_Kff_2Darray_OLDINDEX[j + 1, 4] = DOF_Free[j];

                        }
                    }



                    Debug.Print("ALL DOF_Free Rearranged index and Rearranged Matrix Kff = " + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Kff_2Darray_OLDINDEX, Mat_Kff11));



                    double[,] Mat_Ksf_2Darray_OLDINDEX = new double[totalnode * 3 + HINGE_Num_total + 1, 5];
                    double[,] K_BC_LeftBottom_Row_LeftBottomCol_Ksf = new double[DOF_Restrained.Length, DOF_Free.Length];
                    double[,] K_BC_LeftBottom_Row_LeftBottomCol_Ksf11 = new double[DOF_Restrained.Length + 1, DOF_Free.Length + 1];



                    for (i = 0; i <= DOF_Restrained.Length - 1; i++)
                    {
                        for (j = 0; j <= DOF_Free.Length - 1; j++)
                        {
                            K_BC_LeftBottom_Row_LeftBottomCol_Ksf[i, j] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, j + 1];
                            K_BC_LeftBottom_Row_LeftBottomCol_Ksf11[i + 1, j + 1] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, j + 1];


                            Mat_Ksf_2Darray_OLDINDEX[i + 1, 3] = DOF_Restrained[i];
                            Mat_Ksf_2Darray_OLDINDEX[j + 1, 4] = DOF_Free[j];


                        }
                    }


                    Debug.Print("Rearranged index and Rearranged Matrix  K_BC_LeftBottom_Row_LeftBottomCol_Ksf(,) =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Ksf_2Darray_OLDINDEX, K_BC_LeftBottom_Row_LeftBottomCol_Ksf11));


                    double[,] Mat_Kfs_2Darray_OLDINDEX = new double[totalnode * 3 + HINGE_Num_total + 1, 5];
                    double[,] K_FreeRightRow_BC_Top_Col_Kfs = new double[DOF_Free.Length, DOF_Restrained.Length];
                    double[,] K_FreeRightRow_BC_Top_Col_Kfs11 = new double[DOF_Free.Length + 1, DOF_Restrained.Length + 1];


                    for (i = 0; i <= DOF_Free.Length - 1; i++)
                    {
                        for (j = 0; j <= DOF_Restrained.Length - 1; j++)
                        {
                            K_FreeRightRow_BC_Top_Col_Kfs[i, j] = K_SSM_Rearranged[i + 1, DOF_Free.Length - 1 + 1 + j + 1];
                            K_FreeRightRow_BC_Top_Col_Kfs11[i + 1, j + 1] = K_SSM_Rearranged[i + 1, DOF_Free.Length - 1 + 1 + j + 1];


                            Mat_Kfs_2Darray_OLDINDEX[i + 1, 3] = DOF_Free[i];
                            Mat_Kfs_2Darray_OLDINDEX[j + 1, 4] = DOF_Restrained[j];


                        }
                    }


                    Debug.Print("Rearranged index and Rearranged Matrix  K_FreeRightRow_BC_Top_Col_Kfs(,) =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Kfs_2Darray_OLDINDEX, K_FreeRightRow_BC_Top_Col_Kfs11));


                    double[,] Mat_Kss_2Darray_OLDINDEX = new double[totalnode * 3 + HINGE_Num_total + 1, 5];
                    double[,] K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss = new double[DOF_Restrained.Length, DOF_Restrained.Length];
                    double[,] K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss11 = new double[DOF_Restrained.Length + 1, DOF_Restrained.Length + 1];


                    for (i = 0; i <= DOF_Restrained.Length - 1; i++)
                    {
                        for (j = 0; j <= DOF_Restrained.Length - 1; j++)
                        {
                            K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss[i, j] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, DOF_Free.Length - 1 + 1 + j + 1];
                            K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss11[i + 1, j + 1] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, DOF_Free.Length - 1 + 1 + j + 1];


                            Mat_Kss_2Darray_OLDINDEX[i + 1, 3] = DOF_Restrained[i];
                            Mat_Kss_2Darray_OLDINDEX[j + 1, 4] = DOF_Restrained[j];


                        }
                    }


                    Debug.Print("Rearranged index and Rearranged Matrix  K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss(,) =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Kss_2Darray_OLDINDEX, K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss11));

                    double[,] matrixB = null;


                    double[] XX = null;

                    Debug.Print("free dof matrix kff: " + Environment.NewLine + MakeDisplayable_ij_00_2DARRAY_DOF_Index(Mat_Kff));

                    Debug.Print(String_info);



                    matrixB = Inverse_Mat(Mat_Kff);
                    Debug.Print("Inverse free dof matrix Kff: " + Environment.NewLine + MakeDisplayable_2D2D_ij_0_0(matrixB));






                    ////Support_Displacement=============================================

                    //Debug.Print("Support_Displacement ......." + Environment.NewLine + MakeDisplayable_1D_0_Double(Support_Displacement));

                    //Debug.Print(Support_Displacement.GetUpperBound(0) + " , " + K_FreeRightRow_BC_Top_Col_Kfs.GetUpperBound(0) + " , " + K_FreeRightRow_BC_Top_Col_Kfs.GetUpperBound(1));





                    double[] dispsup_x_Kfs = Multiply_Matrix_2DBY1D_0_0(K_FreeRightRow_BC_Top_Col_Kfs, Support_Displacement);
                    Debug.Print("Multiply_Matrix_2DBY1D_0_0(K_FreeRightRow_BC_Top_Col_Kfs,Support_Displacement) = dispsup_x_Kfs = " + Environment.NewLine + MakeDisplayable_1D_0_Double(dispsup_x_Kfs));






                    Debug.Print("YL......." + Environment.NewLine + MakeDisplayable_1D_0_Double(YL));
                    for (j = 0; j <= DOF_Free.Length - 1; j++)
                    {

                        Debug.Print(j + " YL[j], dispsup_x_Kfs[j] , YL[j]-dispsup_x_Kfs[j]= " + j + " , " + +YL[j] + " , " + dispsup_x_Kfs[j] + " = " + (YL[j] - dispsup_x_Kfs[j]));

                        YL[j] = YL[j] - dispsup_x_Kfs[j];
                    }


                    //=============================================


                    bool_Force_Applied = true;
                    if (Find_All_Zero_Double_1Darray(YL) == true)
                    {
                        bool_Force_Applied = false;
                    }

                    if (bool_Force_Applied == false)
                    {
                        //Debug.Print("Find_All_Zero_Double_1Darray= " + Find_All_Zero_Double_1Darray(YL));
                        //Debug.Print("  Pmem_joint(j ) :" + Environment.NewLine + MakeDisplayable_1D_0_Double(YL));
                        MessageBox.Show("RUN ANALYSIS is QUITED because" + Environment.NewLine
                      + "No force on BEAM or NODE was APPLIED !!!");
                        return;
                    }



                    Debug.Print("DOF_Free GLOBAL displacements:");
                    XX = Multiply_Matrix_2DBY1D_0_0(matrixB, YL);





                    for (j = 0; j < totalnode * 3; j++)
                    {
                        Disp_All_Nodes_Global[j] = 0;
                    }

                    for (j = 0; j < XX.Length; j++)
                    {
                        Debug.Print("XX(" + DOF_Free[j] + ")" + XX[j]);
                        Disp_All_Nodes_Global[DOF_Free[j]] = XX[j];
                    }
                    for (j = 0; j < XX.Length; j++)
                    {
                        Debug.Print("Free DOF wise Global displacement=  " + DOF_Free[j] + "  " + Disp_All_Nodes_Global[DOF_Free[j]]);
                    }
                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {
                        Disp_All_Nodes_Global[DOF_Restrained[j]] = Disp_All_Nodes_Global[DOF_Restrained[j]] + Support_Displacement[j];
                        Debug.Print("DOF_Restrained wise Global displacement=  " + DOF_Restrained[j] + "  " + Disp_All_Nodes_Global[DOF_Restrained[j]]);
                    }
                    for (j = 0; j < ALL_DOF_Free_Restrained.Length; j++)
                    {

                        Debug.Print("ALL_DOF_Free_Restrained DOF wise Global displacement=  " + ALL_DOF_Free_Restrained[j] + "  " + Disp_All_Nodes_Global[ALL_DOF_Free_Restrained[j]]);
                    }

                    Debug.Print("Disp_All_Nodes_Global displacement=  " + MakeDisplayable_1D_0_Double(Disp_All_Nodes_Global));









                    for (j = 0; j < ALL_DOF_Free_Restrained.Length; j++)
                    {

                        Debug.Print("ALL_DOF_Free_Restrained wise Global displacement=  " + ALL_DOF_Free_Restrained[j] + "  " + Disp_All_Nodes_Global[ALL_DOF_Free_Restrained[j]]);
                    }




                    double[] Reaction = Multiply_Matrix_2DBY1D_0_0(K_BC_LeftBottom_Row_LeftBottomCol_Ksf, XX);









                    Debug.Print("Support_Displacement" + Environment.NewLine + MakeDisplayable_1D_0_Double(Support_Displacement));



                    Debug.Print(Support_Displacement.GetUpperBound(0) + " , " + K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss.GetUpperBound(0) + " , " + K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss.GetUpperBound(1));

                    double[] dispsup_x_Kss = Multiply_Matrix_2DBY1D_0_0(K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss, Support_Displacement);


                    Debug.Print("Multiply_Matrix_2DBY1D_0_0(K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss,Support_Displacement) = dispsup_x_Kss[j] =" + Environment.NewLine + MakeDisplayable_1D_0_Double(dispsup_x_Kss));







                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {

                        Debug.Print(j + " Reaction[j] , dispsup_x_Kss[j] ,  Reaction[j] + dispsup_x_Kss[j]= " + Reaction[j] + " , " + dispsup_x_Kss[j] + " = " + (Reaction[j] + dispsup_x_Kss[j]));
                        Reaction[j] = Reaction[j] + dispsup_x_Kss[j];
                    }














                    double[] SupportReaction1 = new double[totalnode * 3 + 1];
                    Support_Reaction = new double[totalnode * 3 + 1];
                    double[] SupportReaction2 = new double[totalnode * 3 + 1];
                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {
                        SupportReaction1[j] = Reaction[j] - ForceEnd_Global_ALL_DOF[DOF_Restrained[j]];
                        Debug.Print(j + " , " + DOF_Restrained[j] + "= DOF_Restrained , Reaction,  Equivalent nodal force, (Reaction - Equivalent nodal force) = " + Reaction[j] + " , " + ForceEnd_Global_ALL_DOF[DOF_Restrained[j]] + " , " + SupportReaction1[j]);

                    }




                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {
                        SupportReaction2[DOF_Restrained[j]] = SupportReaction1[j];
                        Support_Reaction[DOF_Restrained[j]] = SupportReaction2[DOF_Restrained[j]];

                        Debug.Print(j + "," + DOF_Restrained[j] + "  SupportReaction  Reaction =" + SupportReaction2[DOF_Restrained[j]] + "," + Support_Reaction[DOF_Restrained[j]]);
                    }









                    for (k = 1; k <= totalmember; k++)
                    {

                        Disp_j0_global = new double[6];
                        Disp_j0_Local = new double[6];
                        double[] Force_Local_Axix = new double[totalnode * 3 + 1];




                        int[] indx = Index_6_From_MemID_1D_1_7_HINGE(k);
                        Disp_j0_global[0] = Disp_All_Nodes_Global[indx[1]];
                        Disp_j0_global[1] = Disp_All_Nodes_Global[indx[2]];
                        Disp_j0_global[2] = Disp_All_Nodes_Global[indx[3]];
                        Disp_j0_global[3] = Disp_All_Nodes_Global[indx[4]];
                        Disp_j0_global[4] = Disp_All_Nodes_Global[indx[5]];
                        Disp_j0_global[5] = Disp_All_Nodes_Global[indx[6]];




                        double[,] matrix1 = null;
                        matrix1 = Trasform_mat_G_TO_L_ij_00(cosx(k), sinx(k));

                        Disp_j0_Local = Multiply_Matrix_2DBY1D_0_0(matrix1, Disp_j0_global);
                        for (j = 0; j <= 5; j++)
                        {

                        }
                        double[,] matrix2 = K_Local_Element_Matrix_i_0_j_0(L_n(k), Ayz[k], Iz[k], Ey[k]);



                        double[] Force_FROM_Local_Displacement = Multiply_Matrix_2DBY1D_0_0(matrix2, Disp_j0_Local);

                        Force_Local_Axix[Connectivity[k, 1] * 3 - 2] = Force_FROM_Local_Displacement[0] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 2];
                        Force_Local_Axix[Connectivity[k, 1] * 3 - 1] = Force_FROM_Local_Displacement[1] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 1];
                        Force_Local_Axix[Connectivity[k, 1] * 3] = Force_FROM_Local_Displacement[2] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3];
                        Force_Local_Axix[Connectivity[k, 2] * 3 - 2] = Force_FROM_Local_Displacement[3] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 2];
                        Force_Local_Axix[Connectivity[k, 2] * 3 - 1] = Force_FROM_Local_Displacement[4] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 1];
                        Force_Local_Axix[Connectivity[k, 2] * 3] = Force_FROM_Local_Displacement[5] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3];


                        double[] Force_Local = new double[6];

                        Force_Local[0] = Force_Local_Axix[Connectivity[k, 1] * 3 - 2];
                        Force_Local[1] = Force_Local_Axix[Connectivity[k, 1] * 3 - 1];
                        Force_Local[2] = Force_Local_Axix[Connectivity[k, 1] * 3];
                        Force_Local[3] = Force_Local_Axix[Connectivity[k, 2] * 3 - 2];
                        Force_Local[4] = Force_Local_Axix[Connectivity[k, 2] * 3 - 1];
                        Force_Local[5] = Force_Local_Axix[Connectivity[k, 2] * 3];
                        //Debug.Print(k + "Force_Local=  " + Environment.NewLine + MakeDisplayable_1D_0_Double(Force_Local));

                        double[] Force_Global = null;
                        Force_Global = Multiply_Matrix_2DBY1D_0_0(Trasform_mat_TT_L_TO_G_ij_00(cosx(k), sinx(k)), Force_Local);

                        //Debug.Print("Member Number= " + k + "  Force_Global=  " + Environment.NewLine + MakeDisplayable_1D_0_Double(Force_Global));



                    }










                    for (k = 1; k <= totalmember; k++)
                    {
                        int[] indx = Index_6_From_MemID_1D_1_7_HINGE(k);







                        Disp_j0_global[0] = Disp_All_Nodes_Global[indx[1]];
                        Disp_j0_global[1] = Disp_All_Nodes_Global[indx[2]];
                        Disp_j0_global[2] = Disp_All_Nodes_Global[indx[3]];
                        Disp_j0_global[3] = Disp_All_Nodes_Global[indx[4]];
                        Disp_j0_global[4] = Disp_All_Nodes_Global[indx[5]];
                        Disp_j0_global[5] = Disp_All_Nodes_Global[indx[6]];
                        //Debug.Print("Disp_All_Nodes_Global MakeDisplayable_1D_0_Double_Mem_Index= " + Environment.NewLine + MakeDisplayable_1D_0_Double_Mem_Index(k, Disp_All_Nodes_Global));

                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 2] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 2];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 1] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 1];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 2] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 2];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 1] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 1];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3];




                        double[] testdisp = { 0.169612510753683, -0.0001, -0.026161497728352, 0, 0, 0 };




                        t_mem2 = Trasform_mat_G_TO_L_ij_00(cosx(k), sinx(k));

                        Disp_j0_Local = Multiply_Matrix_2DBY1D_0_0(t_mem2, Disp_j0_global);


                        Disp_All_Nodes_Local[Connectivity[k, 1] * 3 - 2] = Disp_j0_Local[0];
                        Disp_All_Nodes_Local[Connectivity[k, 1] * 3 - 1] = Disp_j0_Local[1];
                        Disp_All_Nodes_Local[Connectivity[k, 1] * 3] = Disp_j0_Local[2];
                        Disp_All_Nodes_Local[Connectivity[k, 2] * 3 - 2] = Disp_j0_Local[3];
                        Disp_All_Nodes_Local[Connectivity[k, 2] * 3 - 1] = Disp_j0_Local[4];
                        Disp_All_Nodes_Local[Connectivity[k, 2] * 3] = Disp_j0_Local[5];


                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 2] = Disp_j0_Local[0];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 1] = Disp_j0_Local[1];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3] = Disp_j0_Local[2];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 2] = Disp_j0_Local[3];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 1] = Disp_j0_Local[4];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3] = Disp_j0_Local[5];







                        t_mem1 = K_Local_Element_Matrix_i_0_j_0(L_n(k), Ayz[k], Iz[k], Ey[k]);
                        double[] Force_FROM_Local_Displacement = Multiply_Matrix_2DBY1D_0_0(t_mem1, Disp_j0_Local);
                        Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 2] = Force_FROM_Local_Displacement[0] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 2];
                        Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 1] = Force_FROM_Local_Displacement[1] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 1];
                        Result_Mem_Force_Local[k, Connectivity[k, 1] * 3] = Force_FROM_Local_Displacement[2] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3];
                        Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 2] = Force_FROM_Local_Displacement[3] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 2];
                        Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 1] = Force_FROM_Local_Displacement[4] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 1];
                        Result_Mem_Force_Local[k, Connectivity[k, 2] * 3] = Force_FROM_Local_Displacement[5] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3];

                        double[] Force_Local = new double[6];

                        Force_Local[0] = Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 2];
                        Force_Local[1] = Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 1];
                        Force_Local[2] = Result_Mem_Force_Local[k, Connectivity[k, 1] * 3];
                        Force_Local[3] = Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 2];
                        Force_Local[4] = Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 1];
                        Force_Local[5] = Result_Mem_Force_Local[k, Connectivity[k, 2] * 3];
                        t_mem2 = Trasform_mat_TT_L_TO_G_ij_00(cosx(k), sinx(k));
                        double[] Force_Global = Multiply_Matrix_2DBY1D_0_0(t_mem2, Force_Local);
                        Result_Mem_Force_global[k, Connectivity[k, 1] * 3 - 2] = Force_Global[0];
                        Result_Mem_Force_global[k, Connectivity[k, 1] * 3 - 1] = Force_Global[1];
                        Result_Mem_Force_global[k, Connectivity[k, 1] * 3] = Force_Global[2];
                        Result_Mem_Force_global[k, Connectivity[k, 2] * 3 - 2] = Force_Global[3];
                        Result_Mem_Force_global[k, Connectivity[k, 2] * 3 - 1] = Force_Global[4];
                        Result_Mem_Force_global[k, Connectivity[k, 2] * 3] = Force_Global[5];




                    }


                    //Debug.Print(" Result_Mem_Force_global  :" + Environment.NewLine + MakeDisplayable(Result_Mem_Force_global));

                    //Debug.Print(" Result_Mem_Force Local  :" + Environment.NewLine + MakeDisplayable(Result_Mem_Force_Local));





                }

                catch (Exception ex)
                {

                    Debug.Print("AFTER End of assignment of local and global equivalent forces,Run_Analysis_HINGE, Result_Mem_Force_global and local " + Environment.NewLine + ex.ToString());
                }



            }









            StringBuilder sbtemp33 = new StringBuilder();
            sbtemp33.Append(stringBuilder33.ToString());
            //============================================================================






            Remove1_SbTraceListener();
            //=======================================
            Start2_SbTraceListener();
            //===================================



            Debug.Print(PRINT_1D_ARRAY_GLOBAL_REACTION_X_Y(Support_Reaction));
            Debug.Print("Disp_All_Nodes_Global  =  " + PRINT_1D_ARRAY_ALL_GLOBAL_DISP_X_Y_Z(Disp_All_Nodes_Global));
            Debug.Print(" Result_Mem_Force Local  :" + Environment.NewLine + PRINT_2D_ARRAY_ALL_MemID_CONN_LOCAL_AF_SHEAR_MZ(Result_Mem_Force_Local));

            StringBuilder sbtemp22 = new StringBuilder();
            sbtemp22.Append(stringBuilder22.ToString());
            Remove1_SbTraceListener();
            //============================================================================


            //===========================================================================
            stringBuilderALL = new StringBuilder();
            stringBuilderALL.Append(sbtemp11.ToString());
            stringBuilderALL.Append(sbtemp22.ToString());
            stringBuilderALL.Append(sbtemp33.ToString());

            //===========================================================================







        }//// End private void Run_Analysis_HINGE()

        //==============================

        //================================






        private void Run_Analysis()
        {



            double[] ff2 = new double[6];
            double[,] t_mem1 = null;

            double[,] t_mem2 = null;

            double[] Disp_Element_I_To_J = new double[6];
            double[] temp12G = new double[6];

            double[] cn = new double[6];

            double[] ForeEnd_j0_global = new double[6];
            double[] ForeEnd_j0_Local = new double[6];
            double[] Disp_j0_global = new double[6];
            double[] Disp_j0_Local = null;
            double[] dloc = new double[6];

            double[] ff10 = new double[6];
            double[] ff11 = new double[6];
            double[] qq1 = new double[6];
            double[] qq2 = new double[6];


            double[,] Result_Mem_Force_global_Fixed = new double[totalmember + 1, (totalnode * 3) + 1];

            double[,] Result_Mem_Force_Local_Fixed = new double[totalmember + 1, (totalnode * 3) + 1];

            Disp_All_Nodes_Local = new double[totalnode * 3 + 1];




            double[] SUM_mem = new double[(totalnode * 3) + 1];
            PjG_memEnd_MebCommaConnectivity = new double[totalmember + 1, (totalnode * 3) + 1];
            double[] PjG_memEnd_1D_Connectivity = new double[(totalnode * 3) + 1];
            double[] Pj_joint_WithLoad = new double[(totalnode * 3) + 1];
            double[] Pj_joint_ALL = new double[(totalnode * 3) + 1];
            double[] Pmem_joint = new double[(totalnode * 3) + 1];
            double[,] PjLOCAL_memEnd_MebCommaConnectivity = new double[totalmember + 1, (totalnode * 3) + 1];
            Result_Mem_Force_global = new double[totalmember + 1, (totalnode * 3) + 1];
            Result_Mem_Force_Local = new double[totalmember + 1, (totalnode * 3) + 1];
            Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0 = new double[totalmember + 1, (totalnode * 3) + 1];
            Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0 = new double[totalmember + 1, (totalnode * 3) + 1];
            Disp_All_Nodes_Global = new double[totalnode * 3 + 1];



            int i = 0;
            int j = 0;
            int k = 0;


            int[] DOF_Restrained_Gx = new int[3 * totalnode + 1];
            double[] ForceEnd_Local_ALL_DOF = new double[3 * totalnode];
            int[] ALL_DOF_Free_Restrained = new int[3 * totalnode];
            int[] DOF_Restrained = new int[3 * totalnode + 1];
            int[] ALL_Restrained_DOF_Free = new int[3 * totalnode];


            for (j = 0; j <= 3 * totalnode - 1; j++)
            {
                ALL_DOF_Free_Restrained[j] = j + 1;

            }



            Array.Resize(ref suptype, totalnode + 1);
            Array.Resize(ref Sup_Node_Number, totalnode + 1);
            for (k = 1; k <= totalnode; k++)
            {
                string[] testx = new string[] { };
                if (!string.IsNullOrEmpty(Sup_Node_Number_Type[k]))
                {

                    testx = Sup_Node_Number_Type[k].Split(new char[] { ',' });
                    for (i = 0; i < testx.Length; i++)
                    {


                    }

                    Sup_Node_Number[k] = int.Parse(testx[0]);
                    suptype[k] = int.Parse(testx[1]);

                }
            }





            for (j = 1; j <= totalnode; j++)
            {


                if (suptype[j] == 1)
                {

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j]] = 3 * Sup_Node_Number[j];
                    //Debug.Print(j + "Sup_Node_Number[j]fixed:" + Sup_Node_Number[j]);
                }
                if (suptype[j] == 2)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;

                    //Debug.Print("Sup_Node_Number[j] hinged:" + Sup_Node_Number[j]);
                }
                if (suptype[j] == 3)
                {

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;

                    //Debug.Print("Sup_Node_Number[j] X, MZ free, Y  restrained ROLLER-Y:" + Sup_Node_Number[j]);
                }

                if (suptype[j] == 4)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;


                    //Debug.Print("Sup_Node_Number[j] Y, MZ free, X  restrained ROLLER-X:" + Sup_Node_Number[j]);
                }

                if (suptype[j] == 5)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j]] = 3 * Sup_Node_Number[j];

                }











            }


            //Debug.Print("DOF_Restrained_Gx :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Restrained_Gx));
            int tx = 0;
            k = 0;


            for (j = 0; j <= totalnode * 3; j++)
            {
                tx = DOF_Restrained_Gx[j];
                if (tx > 0)
                {


                    Array.Resize(ref DOF_Restrained, k + 1);
                    DOF_Restrained[k] = tx;
                    k = k + 1;
                }
            }


            bool_DOF_Restrained_G_Greater_Than_2 = true;
            if (DOF_Restrained.Length < 3)
            {
                bool_DOF_Restrained_G_Greater_Than_2 = false;
                //Debug.Print("DOF_Restrained.Length = " + DOF_Restrained.Length);
                MessageBox.Show("RUN ANALYSIS is QUITED because" + Environment.NewLine
                 + "the structure needs more supports !!! " + Environment.NewLine
                + "Degree of Restraint of 2D frame structure IS LESS THAN 3 !!!");
                return;

            }


            DOF_Free = Uncommon_Matrix_1D_1D_Intger(ALL_DOF_Free_Restrained, DOF_Restrained);
            bool bool_DOF_Free_Greater_Than_0 = true;
            if (DOF_Free.Length == 0) bool_DOF_Free_Greater_Than_0 = false;



            ALL_DOF_Free_Restrained = Union_Matrix_1D_1D_Intger(DOF_Free, DOF_Restrained);

            ALL_Restrained_DOF_Free = Union_Matrix_1D_1D_Intger(DOF_Restrained, DOF_Free);


            if (DOF_Free.Length > 0)
            {
                Debug.Print("DOF_Free :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Free));
            }
            else
            {
                Debug.Print("CAUTION - ALL DEGREES OF FREEDOM FIXED !!!");

            }
            Debug.Print("DOF_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Restrained));
            Debug.Print("ALL_DOF_Free_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(ALL_DOF_Free_Restrained));










            double[] Support_Displacement = new double[DOF_Restrained.Length];


            //Debug.Print("Support_Displacement :" + Environment.NewLine + MakeDisplayable_1D_0_Double(Support_Displacement));
            Array.Resize(ref   Support_Displacement_S, totalnode + 1);
            Array.Resize(ref Support_Displacement_temp, totalnode * 3 + 1);
            //Debug.Print("DOF_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Restrained));
            //Debug.Print("Support_Displacement :" + Environment.NewLine + MakeDisplayable_1D_0_Double(Support_Displacement));





            string[] test9 = new string[] { };


            //Debug.Print("Support_Displacement_S :" + Environment.NewLine + MakeDisplayable_1D_j_0_string(Support_Displacement_S));


            for (j = 1; j <= totalnode; j++)
            {
                if (!string.IsNullOrEmpty(Support_Displacement_S[j]))
                {

                    test9 = Support_Displacement_S[j].Split(new char[] { ',' });
                    for (i = 0; i < test9.Length; i++)
                    {

                        //Debug.Print(j+" , "+i + "Support_Displacement_S test  =" + test9[i]);
                    }





                    Support_Displacement_temp[j * 3 - 2] = double.Parse(test9[1]);
                    Support_Displacement_temp[j * 3 - 1] = double.Parse(test9[2]);

                    //Debug.Print(j + ","+(j * 3 - 2)+ "Support_Displacement[j * 3 - 2]  =" + double.Parse(test9[1]));
                    //Debug.Print(j + "," + (j * 3 - 1) + "Support_Displacement[j * 3 - 1]  =" + double.Parse(test9[2]));
                }
            }


            for (j = 0; j < DOF_Restrained.Length; j++)
            {
                Support_Displacement[j] = Support_Displacement_temp[ALL_Restrained_DOF_Free[j]];
                Debug.Print("j= " + j + " , DOF_Restrained[j]= " + DOF_Restrained[j] + " Support_Displacement = " + Support_Displacement[j]);
            }









            int mem_id = 0;

            double[,] matrixB = null;

            double[] YL = new double[DOF_Free.Length];

            double[] XX = null;





            fflocal1D = new double[6];
            int k12 = 0;

            for (k12 = 1; k12 <= totalmember; k12++)
            {


                mem_id = k12;

                Array.Clear(fflocal1D, 0, fflocal1D.Length);
                try
                {

                    if (!string.IsNullOrEmpty(Pload[mem_id]))
                    {

                        ForceEnd_Local_MemComaIndex = FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal(mem_id, Pload[mem_id]);
                        ForceEnd_Local_To_Global = FORCE_Eqiv_Nod_MembComaP_ij_1_0_TO_Global(mem_id, Pload[mem_id]);
                        //Debug.Print(" ForceEnd_Local_To_Global " + MakeDisplayable(ForceEnd_Local_To_Global));
                        //Debug.Print(" ForceEnd_Global_ALL_DOF " + MakeDisplayable(ForceEnd_Local_To_Global));


                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 2] = ForceEnd_Local_MemComaIndex[mem_id, 0];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 1] = ForceEnd_Local_MemComaIndex[mem_id, 1];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3] = ForceEnd_Local_MemComaIndex[mem_id, 2];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 2] = ForceEnd_Local_MemComaIndex[mem_id, 3];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 1] = ForceEnd_Local_MemComaIndex[mem_id, 4];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3] = ForceEnd_Local_MemComaIndex[mem_id, 5];

                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 2] = ForceEnd_Local_To_Global[mem_id, 0];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 1] = ForceEnd_Local_To_Global[mem_id, 1];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3] = ForceEnd_Local_To_Global[mem_id, 2];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 2] = ForceEnd_Local_To_Global[mem_id, 3];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 1] = ForceEnd_Local_To_Global[mem_id, 4];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3] = ForceEnd_Local_To_Global[mem_id, 5];

                    }


                }
                catch (Exception ex)
                {

                    Debug.Print("ForceEnd_Local_MemComaIndex = FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal + global " + ex.ToString());
                }


            }


            //Debug.Print("PjLOCAL_memEnd_MebCommaConnectivity :" + Environment.NewLine + MakeDisplayable_2D_ARRAY_ALL_MemID_CONN(PjLOCAL_memEnd_MebCommaConnectivity));
            //Debug.Print("PjG_memEnd_MebCommaConnectivity :" + Environment.NewLine + MakeDisplayable_2D_ARRAY_ALL_MemID_CONN(PjG_memEnd_MebCommaConnectivity));




            if (bool_DOF_Free_Greater_Than_0 == true)
            {
                try
                {

                    ForceEnd_Local_ALL_DOF = new double[totalnode * 3 + 1];

                    ForceEnd_Global_ALL_DOF = new double[totalnode * 3 + 1];

                    for (j = 1; j <= totalnode * 3; j++)
                    {
                        for (k = 1; k <= totalmember; k++)
                        {
                            ForceEnd_Local_ALL_DOF[j] = ForceEnd_Local_ALL_DOF[j] + PjLOCAL_memEnd_MebCommaConnectivity[k, j];
                            ForceEnd_Global_ALL_DOF[j] = ForceEnd_Global_ALL_DOF[j] + PjG_memEnd_MebCommaConnectivity[k, j];
                        }

                    }
                    //Debug.Print(" ForceEnd_Local_ALL_DOF " + MakeDisplayable_1D_0_Double(ForceEnd_Local_ALL_DOF));
                    //Debug.Print(" ForceEnd_Global_ALL_DOF eqv nodal force" + MakeDisplayable_1D_0_Double(ForceEnd_Global_ALL_DOF));


                    SUM_mem = new double[(totalnode * 3) + 1];

                    for (j = 1; j <= totalnode * 3; j++)
                    {
                        for (k = 1; k <= totalmember; k++)
                        {
                            SUM_mem[j] = SUM_mem[j] + PjG_memEnd_MebCommaConnectivity[k, j];
                        }

                    }

                    //Debug.Print(" SUM_mem :" + Environment.NewLine + MakeDisplayable_1D_0_Double(SUM_mem));

                    //Debug.Print(" totalnode,  PJoint.Length  :" + totalnode + ", " + PJoint.Length);
                    double[] ffjG = null;
                    for (j = 1; j <= totalnode; j++)
                    {

                        if (!string.IsNullOrEmpty(PJoint[j]))
                        {


                            ffjG = Joint_Global_force_0_3(PJoint[j]);
                            k = Convert.ToInt32(ffjG[0]);
                            if (k >= 1)
                            {
                                Pj_joint_WithLoad[3 * k - 2] = ffjG[1];
                                Pj_joint_WithLoad[3 * k - 1] = ffjG[2];
                                Pj_joint_WithLoad[3 * k] = ffjG[3];
                            }

                        }
                    }




                    for (j = 1; j <= totalnode * 3; j++)
                    {

                        Pj_joint_ALL[j] = Pj_joint_ALL[j] + Pj_joint_WithLoad[j];

                    }
                    //Debug.Print("  Pj_joint :" + Environment.NewLine + MakeDisplayable_1D_0_Double(Pj_joint_ALL));




                    for (j = 1; j <= totalnode * 3; j++)
                    {
                        Pmem_joint[j] = SUM_mem[j] + Pj_joint_ALL[j];
                    }
                    //Debug.Print("  Pmem_joint(j ) :" + Environment.NewLine + MakeDisplayable_1D_0_Double(Pmem_joint));




                    YL = new double[DOF_Free.Length];
                    Debug.Print("" + Environment.NewLine + " Load Vector is as follows: .......... " + Environment.NewLine);
                    for (j = 0; j < DOF_Free.Length; j++)
                    {
                        YL[j] = Pmem_joint[DOF_Free[j]];

                        Debug.Print("DOF_Free[" + j + "] ,  " + "YL[" + j + "]  =" + DOF_Free[j] + "  , " + YL[j] + "");


                    }


                    double[,] Member_KG = null;
                    double[, ,] KGM_ALL = new double[totalmember + 1, totalnode * 3 + 1, totalnode * 3 + 1];
                    Array.Clear(KGM_ALL, 0, KGM_ALL.Length);
                    double[, ,] KGM_ALL2 = new double[totalmember + 1, totalnode * 3 + 1, totalnode * 3 + 1];
                    Array.Clear(KGM_ALL2, 0, KGM_ALL2.Length);

                    double[, ,] Mem_KG_mat = new double[totalmember + 1, 7, 7];

                    int i1 = 0;
                    int j1 = 0;

                    for (i = 1; i <= totalmember; i++)
                    {

                        Debug.Print("Member# " + i + "= Global Member Stiffness Matrix: " + Environment.NewLine + MakeDisplayable_ij_11_MemIDComma2DARRAY_CONNECTIVITY_Index(i, K_2_G_Element_i_1_j_1(i)));



                        Member_KG = K_G_Element_ij_11(i);

                        int[] Index_1 = Index_6_From_MemID_1D_i_1(i);
                        for (i1 = 1; i1 <= 6; i1++)
                        {

                            for (j1 = 1; j1 <= 6; j1++)
                            {


                                KGM_ALL2[i, Index_1[i1], Index_1[j1]] = Member_KG[i1, j1];

                            }
                        }


                        //Debug.Print(i + " Member_KG Global ............. Matrix: " + Environment.NewLine + MakeDisplayable_ij_11_MemIDComma2DARRAY_CONNECTIVITY_Index(i, Member_KG));



                        for (k = 1; k <= 3; k++)
                        {
                            for (j = 1; j <= 3; j++)
                            {


                                KGM_ALL[i, Connectivity[i, 1] * 3 - 3 + k, Connectivity[i, 1] * 3 - 3 + j] = Member_KG[k, j];


                                KGM_ALL[i, Connectivity[i, 1] * 3 - 3 + k, Connectivity[i, 2] * 3 - 3 + j] = Member_KG[k, j + 3];


                                KGM_ALL[i, Connectivity[i, 2] * 3 - 3 + k, Connectivity[i, 1] * 3 - 3 + j] = Member_KG[k + 3, j];


                                KGM_ALL[i, Connectivity[i, 2] * 3 - 3 + k, Connectivity[i, 2] * 3 - 3 + j] = Member_KG[k + 3, j + 3];



                            }
                        }

                        Array.Clear(Member_KG, 0, Member_KG.Length);

                    }

                    //Debug.Print("KGM_ALL: 3D with index " + MakeDisplayable_Top_Side_No_Connectivity_3D3D_ij_0_0(KGM_ALL));
                    //Debug.Print("KGM_ALL2  : 3D with index " + MakeDisplayable_Top_Side_No_Connectivity_3D3D_ij_0_0(KGM_ALL2));








                    double[,] result = new double[(totalnode * 3) + 1, (totalnode * 3) + 1];
                    for (i = 1; i <= totalmember; i++)
                    {

                        for (j = 1; j <= totalnode * 3; j++)
                        {

                            for (k = 1; k <= totalnode * 3; k++)
                            {
                                result[j, k] = result[j, k] + KGM_ALL[i, j, k];
                            }
                        }
                    }

                    //Debug.Print("NOT Rearranged SSM 3D to 2D result(,) ="+ MakeDisplayable_ij_00_2DARRAY_CONNECTIVITY_Index(result));

                    Array.Clear(result, 0, result.Length);
                    for (i = 1; i <= totalmember; i++)
                    {

                        for (j = 1; j <= totalnode * 3; j++)
                        {

                            for (k = 1; k <= totalnode * 3; k++)
                            {
                                result[j, k] = result[j, k] + KGM_ALL2[i, j, k];
                            }
                        }
                    }

                    Debug.Print("Structure Stiffness Matrix SSM  =" + MakeDisplayable_ij_11_2DARRAY_CONNECTIVITY_Index(result));








                    double[,] K_SSM_Rearranged = new double[(totalnode * 3) + 1, (totalnode * 3) + 1];

                    int[] K_SSM_Rearranged_oldindex_1DArray = new int[(totalnode * 3) + 1];
                    double[,] K_SSM_Rearranged_oldindex_2DArray = new double[(totalnode * 3) + 1, 5];

                    for (i = 1; i <= totalnode * 3; i++)
                    {

                        for (j = 1; j <= totalnode * 3; j++)
                        {

                            K_SSM_Rearranged[i, j] = result[ALL_DOF_Free_Restrained[i - 1], ALL_DOF_Free_Restrained[j - 1]];


                            K_SSM_Rearranged_oldindex_1DArray[j] = ALL_DOF_Free_Restrained[j - 1];

                            K_SSM_Rearranged_oldindex_2DArray[i, 3] = ALL_DOF_Free_Restrained[i - 1];
                            K_SSM_Rearranged_oldindex_2DArray[j, 4] = ALL_DOF_Free_Restrained[j - 1];


                        }
                    }
                    //Debug.Print("1D Rearranged index " + MakeDisplayable_1D_j_0_INT(K_SSM_Rearranged_oldindex_1DArray));
                    //Debug.Print("2D Rearranged index" + MakeDisplayable_2D2D_ij_0_0(K_SSM_Rearranged_oldindex_2DArray));

                    Debug.Print("DOF_Free + DOF_Restrained Rearranged index and Rearranged Structure Stiffness Matrix, SSM_Rearranged =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(K_SSM_Rearranged_oldindex_2DArray, K_SSM_Rearranged));



                    //================================================================

                    //Debug.Print("TOTAL FREE DOOF without  restrained DOF = " + DOF_Free.Length);
                    double[,] Mat_Kff_2Darray_OLDINDEX = new double[DOF_Free.Length + 1, DOF_Free.Length + 5];
                    double[,] Mat_Kff = new double[DOF_Free.Length, DOF_Free.Length];
                    double[,] Mat_Kff11 = new double[DOF_Free.Length + 1, DOF_Free.Length + 1];
                    for (i = 0; i < DOF_Free.Length; i++)
                    {
                        for (j = 0; j < DOF_Free.Length; j++)
                        {
                            Mat_Kff[i, j] = K_SSM_Rearranged[i + 1, j + 1];
                            Mat_Kff11[i + 1, j + 1] = K_SSM_Rearranged[i + 1, j + 1];
                            Mat_Kff_2Darray_OLDINDEX[i + 1, 3] = DOF_Free[i];
                            Mat_Kff_2Darray_OLDINDEX[j + 1, 4] = DOF_Free[j];

                        }
                    }




                    Debug.Print("ALL DOF_Free Rearranged index and Rearranged Matrix Kff =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Kff_2Darray_OLDINDEX, Mat_Kff11));





                    double[,] Mat_Ksf_2Darray_OLDINDEX = new double[3 * totalnode + 1, 5];
                    double[,] K_BC_LeftBottom_Row_LeftBottomCol_Ksf = new double[DOF_Restrained.Length, DOF_Free.Length];
                    double[,] K_BC_LeftBottom_Row_LeftBottomCol_Ksf11 = new double[DOF_Restrained.Length + 1, DOF_Free.Length + 1];



                    for (i = 0; i <= DOF_Restrained.Length - 1; i++)
                    {
                        for (j = 0; j <= DOF_Free.Length - 1; j++)
                        {
                            K_BC_LeftBottom_Row_LeftBottomCol_Ksf[i, j] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, j + 1];
                            K_BC_LeftBottom_Row_LeftBottomCol_Ksf11[i + 1, j + 1] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, j + 1];


                            Mat_Ksf_2Darray_OLDINDEX[i + 1, 3] = DOF_Restrained[i];
                            Mat_Ksf_2Darray_OLDINDEX[j + 1, 4] = DOF_Free[j];

                        }
                    }



                    Debug.Print("Rearranged index and Rearranged Matrix  K_BC_LeftBottom_Row_LeftBottomCol_Ksf(,) =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Ksf_2Darray_OLDINDEX, K_BC_LeftBottom_Row_LeftBottomCol_Ksf11));



                    double[,] Mat_Kfs_2Darray_OLDINDEX = new double[3 * totalnode + 1, 5];
                    double[,] K_FreeRightRow_BC_Top_Col_Kfs = new double[DOF_Free.Length, DOF_Restrained.Length];
                    double[,] K_FreeRightRow_BC_Top_Col_Kfs11 = new double[DOF_Free.Length + 1, DOF_Restrained.Length + 1];



                    for (i = 0; i <= DOF_Free.Length - 1; i++)
                    {
                        for (j = 0; j <= DOF_Restrained.Length - 1; j++)
                        {
                            K_FreeRightRow_BC_Top_Col_Kfs[i, j] = K_SSM_Rearranged[i + 1, DOF_Free.Length - 1 + 1 + j + 1];
                            K_FreeRightRow_BC_Top_Col_Kfs11[i + 1, j + 1] = K_SSM_Rearranged[i + 1, DOF_Free.Length - 1 + 1 + j + 1];


                            Mat_Kfs_2Darray_OLDINDEX[i + 1, 3] = DOF_Free[i];
                            Mat_Kfs_2Darray_OLDINDEX[j + 1, 4] = DOF_Restrained[j];


                        }
                    }


                    Debug.Print("Rearranged index and Rearranged Matrix  K_FreeRightRow_BC_Top_Col_Kfs(,) =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Kfs_2Darray_OLDINDEX, K_FreeRightRow_BC_Top_Col_Kfs11));




                    double[,] Mat_Kss_2Darray_OLDINDEX = new double[3 * totalnode + 1, 5];
                    double[,] K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss = new double[DOF_Restrained.Length, DOF_Restrained.Length];
                    double[,] K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss11 = new double[DOF_Restrained.Length + 1, DOF_Restrained.Length + 1];



                    for (i = 0; i <= DOF_Restrained.Length - 1; i++)
                    {
                        for (j = 0; j <= DOF_Restrained.Length - 1; j++)
                        {
                            K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss[i, j] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, DOF_Free.Length - 1 + 1 + j + 1];
                            K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss11[i + 1, j + 1] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, DOF_Free.Length - 1 + 1 + j + 1];


                            Mat_Kss_2Darray_OLDINDEX[i + 1, 3] = DOF_Restrained[i];
                            Mat_Kss_2Darray_OLDINDEX[j + 1, 4] = DOF_Restrained[j];


                        }
                    }


                    Debug.Print("Rearranged index and Rearranged Matrix  K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss(,) =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Kss_2Darray_OLDINDEX, K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss11));




                    //=============================================================




                    Debug.Print(String_info);



                    matrixB = LU.Inverse(Mat_Kff);
                    Debug.Print("Inverse free dof matrix Kff: " + Environment.NewLine + MakeDisplayable_2D2D_ij_0_0(matrixB));







                    ////Support_Displacement=============================================

                    //Debug.Print("Support_Displacement ......." + Environment.NewLine + MakeDisplayable_1D_0_Double(Support_Displacement));

                    //Debug.Print(Support_Displacement.GetUpperBound(0) + " , " + K_FreeRightRow_BC_Top_Col_Kfs.GetUpperBound(0) + " , " + K_FreeRightRow_BC_Top_Col_Kfs.GetUpperBound(1));





                    double[] dispsup_x_Kfs = Multiply_Matrix_2DBY1D_0_0(K_FreeRightRow_BC_Top_Col_Kfs, Support_Displacement);
                    Debug.Print("Multiply_Matrix_2DBY1D_0_0(K_FreeRightRow_BC_Top_Col_Kfs,Support_Displacement) = dispsup_x_Kfs = " + Environment.NewLine + MakeDisplayable_1D_0_Double(dispsup_x_Kfs));






                    Debug.Print("YL......." + Environment.NewLine + MakeDisplayable_1D_0_Double(YL));
                    for (j = 0; j <= DOF_Free.Length - 1; j++)
                    {

                        Debug.Print(j + " YL[j], dispsup_x_Kfs[j] , YL[j]-dispsup_x_Kfs[j]= " + j + " , " + +YL[j] + " , " + dispsup_x_Kfs[j] + " = " + (YL[j] - dispsup_x_Kfs[j]));

                        YL[j] = YL[j] - dispsup_x_Kfs[j];
                    }


                    //=============================================


                    bool_Force_Applied = true;
                    if (Find_All_Zero_Double_1Darray(YL) == true)
                    {
                        bool_Force_Applied = false;
                    }

                    if (bool_Force_Applied == false)
                    {
                        //Debug.Print("Find_All_Zero_Double_1Darray= " + Find_All_Zero_Double_1Darray(YL));
                        //Debug.Print("  Pmem_joint(j ) :" + Environment.NewLine + MakeDisplayable_1D_0_Double(YL));
                        MessageBox.Show("RUN ANALYSIS is QUITED because" + Environment.NewLine
                      + "No force on BEAM or NODE was APPLIED !!!");
                        return;
                    }



                    Debug.Print("DOF_Free GLOBAL displacements:");
                    XX = Multiply_Matrix_2DBY1D_0_0(matrixB, YL);





                    for (j = 0; j < totalnode * 3; j++)
                    {
                        Disp_All_Nodes_Global[j] = 0;
                    }

                    for (j = 0; j < XX.Length; j++)
                    {
                        Debug.Print("XX(" + DOF_Free[j] + ")" + XX[j]);
                        Disp_All_Nodes_Global[DOF_Free[j]] = XX[j];
                    }
                    for (j = 0; j < XX.Length; j++)
                    {
                        Debug.Print("Free DOF wise Global displacement=  " + DOF_Free[j] + "  " + Disp_All_Nodes_Global[DOF_Free[j]]);
                    }
                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {
                        Disp_All_Nodes_Global[DOF_Restrained[j]] = Disp_All_Nodes_Global[DOF_Restrained[j]] + Support_Displacement[j];
                        Debug.Print("DOF_Restrained wise Global displacement=  " + DOF_Restrained[j] + "  " + Disp_All_Nodes_Global[DOF_Restrained[j]]);
                    }
                    for (j = 0; j < ALL_DOF_Free_Restrained.Length; j++)
                    {

                        Debug.Print("ALL_DOF_Free_Restrained DOF wise Global displacement=  " + ALL_DOF_Free_Restrained[j] + "  " + Disp_All_Nodes_Global[ALL_DOF_Free_Restrained[j]]);
                    }

                    Debug.Print("Disp_All_Nodes_Global displacement=  " + MakeDisplayable_1D_0_Double(Disp_All_Nodes_Global));









                    for (j = 0; j < ALL_DOF_Free_Restrained.Length; j++)
                    {

                        Debug.Print("ALL_DOF_Free_Restrained wise Global displacement=  " + ALL_DOF_Free_Restrained[j] + "  " + Disp_All_Nodes_Global[ALL_DOF_Free_Restrained[j]]);
                    }




                    double[] Reaction = Multiply_Matrix_2DBY1D_0_0(K_BC_LeftBottom_Row_LeftBottomCol_Ksf, XX);









                    Debug.Print("Support_Displacement" + Environment.NewLine + MakeDisplayable_1D_0_Double(Support_Displacement));



                    Debug.Print(Support_Displacement.GetUpperBound(0) + " , " + K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss.GetUpperBound(0) + " , " + K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss.GetUpperBound(1));

                    double[] dispsup_x_Kss = Multiply_Matrix_2DBY1D_0_0(K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss, Support_Displacement);


                    Debug.Print("Multiply_Matrix_2DBY1D_0_0(K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss,Support_Displacement) = dispsup_x_Kss[j] =" + Environment.NewLine + MakeDisplayable_1D_0_Double(dispsup_x_Kss));







                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {

                        Debug.Print(j + " Reaction[j] , dispsup_x_Kss[j] ,  Reaction[j] + dispsup_x_Kss[j]= " + Reaction[j] + " , " + dispsup_x_Kss[j] + " = " + (Reaction[j] + dispsup_x_Kss[j]));
                        Reaction[j] = Reaction[j] + dispsup_x_Kss[j];
                    }






                    double[] SupportReaction1 = new double[totalnode * 3 + 1];
                    Support_Reaction = new double[totalnode * 3 + 1];
                    double[] SupportReaction2 = new double[totalnode * 3 + 1];
                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {
                        SupportReaction1[j] = Reaction[j] - ForceEnd_Global_ALL_DOF[DOF_Restrained[j]];
                        Debug.Print(j + " , " + DOF_Restrained[j] + "= DOF_Restrained , Reaction,  Equivalent nodal force, (Reaction - Equivalent nodal force) = " + Reaction[j] + " , " + ForceEnd_Global_ALL_DOF[DOF_Restrained[j]] + " , " + SupportReaction1[j]);

                    }




                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {
                        SupportReaction2[DOF_Restrained[j]] = SupportReaction1[j];
                        Support_Reaction[DOF_Restrained[j]] = SupportReaction2[DOF_Restrained[j]];

                        Debug.Print(j + "," + DOF_Restrained[j] + "  SupportReaction  Reaction =" + SupportReaction2[DOF_Restrained[j]] + "," + Support_Reaction[DOF_Restrained[j]]);
                    }











                    for (k = 1; k <= totalmember; k++)
                    {

                        Disp_j0_global[0] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 2];
                        Disp_j0_global[1] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 1];
                        Disp_j0_global[2] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3];
                        Disp_j0_global[3] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 2];
                        Disp_j0_global[4] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 1];
                        Disp_j0_global[5] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3];


                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 2] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 2];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 1] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 1];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 2] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 2];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 1] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 1];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3];









                        t_mem2 = Trasform_mat_G_TO_L_ij_00(cosx(k), sinx(k));

                        Disp_j0_Local = Multiply_Matrix_2DBY1D_0_0(t_mem2, Disp_j0_global);


                        Disp_All_Nodes_Local[Connectivity[k, 1] * 3 - 2] = Disp_j0_Local[0];
                        Disp_All_Nodes_Local[Connectivity[k, 1] * 3 - 1] = Disp_j0_Local[1];
                        Disp_All_Nodes_Local[Connectivity[k, 1] * 3] = Disp_j0_Local[2];
                        Disp_All_Nodes_Local[Connectivity[k, 2] * 3 - 2] = Disp_j0_Local[3];
                        Disp_All_Nodes_Local[Connectivity[k, 2] * 3 - 1] = Disp_j0_Local[4];
                        Disp_All_Nodes_Local[Connectivity[k, 2] * 3] = Disp_j0_Local[5];


                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 2] = Disp_j0_Local[0];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 1] = Disp_j0_Local[1];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3] = Disp_j0_Local[2];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 2] = Disp_j0_Local[3];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 1] = Disp_j0_Local[4];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3] = Disp_j0_Local[5];


                        t_mem1 = K_Local_Element_Matrix_i_0_j_0(L_n(k), Ayz[k], Iz[k], Ey[k]);
                        double[] Force_FROM_Local_Displacement = Multiply_Matrix_2DBY1D_0_0(t_mem1, Disp_j0_Local);
                        Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 2] = Force_FROM_Local_Displacement[0] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 2];
                        Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 1] = Force_FROM_Local_Displacement[1] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 1];
                        Result_Mem_Force_Local[k, Connectivity[k, 1] * 3] = Force_FROM_Local_Displacement[2] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3];
                        Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 2] = Force_FROM_Local_Displacement[3] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 2];
                        Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 1] = Force_FROM_Local_Displacement[4] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 1];
                        Result_Mem_Force_Local[k, Connectivity[k, 2] * 3] = Force_FROM_Local_Displacement[5] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3];

                        double[] Force_Local = new double[6];

                        Force_Local[0] = Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 2];
                        Force_Local[1] = Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 1];
                        Force_Local[2] = Result_Mem_Force_Local[k, Connectivity[k, 1] * 3];
                        Force_Local[3] = Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 2];
                        Force_Local[4] = Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 1];
                        Force_Local[5] = Result_Mem_Force_Local[k, Connectivity[k, 2] * 3];

                        t_mem2 = Trasform_mat_TT_L_TO_G_ij_00(cosx(k), sinx(k));

                        double[] Force_Global = Multiply_Matrix_2DBY1D_0_0(t_mem2, Force_Local);
                        Result_Mem_Force_global[k, Connectivity[k, 1] * 3 - 2] = Force_Global[0];
                        Result_Mem_Force_global[k, Connectivity[k, 1] * 3 - 1] = Force_Global[1];
                        Result_Mem_Force_global[k, Connectivity[k, 1] * 3] = Force_Global[2];
                        Result_Mem_Force_global[k, Connectivity[k, 2] * 3 - 2] = Force_Global[3];
                        Result_Mem_Force_global[k, Connectivity[k, 2] * 3 - 1] = Force_Global[4];
                        Result_Mem_Force_global[k, Connectivity[k, 2] * 3] = Force_Global[5];

                    }


                    //Debug.Print(" Result_Mem_Force_global  :" + Environment.NewLine + MakeDisplayable(Result_Mem_Force_global));

                    //Debug.Print(" Result_Mem_Force Local  :" + Environment.NewLine + MakeDisplayable(Result_Mem_Force_Local));


                }

                catch (Exception ex)
                {

                    Debug.Print("AFTER End of deduction of local and global equivalent forces to nodal forces in Run_Analysis() " + Environment.NewLine + ex.ToString());
                }

            }




            double[] supportreaction_no_dof = new double[totalnode * 3 + 2];


            if (bool_DOF_Free_Greater_Than_0 == false)
            {




                for (k = 1; k <= totalmember; k++)
                {




                    Result_Mem_Force_global_Fixed[k, Connectivity[k, 1] * 3 - 2] = -PjG_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 2];
                    Result_Mem_Force_global_Fixed[k, Connectivity[k, 1] * 3 - 1] = -PjG_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 1];
                    Result_Mem_Force_global_Fixed[k, Connectivity[k, 1] * 3] = -PjG_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3];
                    Result_Mem_Force_global_Fixed[k, Connectivity[k, 2] * 3 - 2] = -PjG_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 2];
                    Result_Mem_Force_global_Fixed[k, Connectivity[k, 2] * 3 - 1] = -PjG_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 1];
                    Result_Mem_Force_global_Fixed[k, Connectivity[k, 2] * 3] = -PjG_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3];




                    Result_Mem_Force_Local_Fixed[k, Connectivity[k, 1] * 3 - 2] = -PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 2];
                    Result_Mem_Force_Local_Fixed[k, Connectivity[k, 1] * 3 - 1] = -PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 1];
                    Result_Mem_Force_Local_Fixed[k, Connectivity[k, 1] * 3] = -PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3];
                    Result_Mem_Force_Local_Fixed[k, Connectivity[k, 2] * 3 - 2] = -PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 2];
                    Result_Mem_Force_Local_Fixed[k, Connectivity[k, 2] * 3 - 1] = -PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 1];
                    Result_Mem_Force_Local_Fixed[k, Connectivity[k, 2] * 3] = -PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3];

                    ////=========================================================



                }
                //=================================



                ////======================================


                //Debug.Print(" Result_Mem_Force_global_Fixed  :" + Environment.NewLine + MakeDisplayable(Result_Mem_Force_global_Fixed));

                //Debug.Print(" Result_Mem_Force_Local_Fixed  :" + Environment.NewLine + MakeDisplayable(Result_Mem_Force_Local_Fixed));

                //Debug.Print(" Result_Mem_Force_Local_Fixed  :" + Environment.NewLine + MakeDisplayable_2D_ARRAY_ALL_MemID_CONN(Result_Mem_Force_global_Fixed));

                //Debug.Print(" Result_Mem_Force_Local_Fixed  :" + Environment.NewLine + MakeDisplayable_2D_ARRAY_ALL_MemID_CONN(Result_Mem_Force_Local_Fixed));

                Debug.Print("Member# " + i + "= Global Member Stiffness Matrix: " + Environment.NewLine + MakeDisplayable_ij_11_MemIDComma2DARRAY_CONNECTIVITY_Index(1, K_2_G_Element_i_1_j_1(1)));

                supportreaction_no_dof[1] = Result_Mem_Force_global_Fixed[1, 1];
                supportreaction_no_dof[2] = Result_Mem_Force_global_Fixed[1, 2];
                supportreaction_no_dof[3] = Result_Mem_Force_global_Fixed[1, 3];
                supportreaction_no_dof[4] = Result_Mem_Force_global_Fixed[1, 4];
                supportreaction_no_dof[5] = Result_Mem_Force_global_Fixed[1, 5];
                supportreaction_no_dof[6] = Result_Mem_Force_global_Fixed[1, 6];


                //Support_Reaction = Result_Mem_Force_global_Fixed;
                Result_Mem_Force_Local = Result_Mem_Force_Local_Fixed;
                Result_Mem_Force_global = Result_Mem_Force_global_Fixed;





                View_Shear = false;
                View_Mom = false;
                View_Theta = false;
                View_Delta = false;
                View_AxialForce = false;
                View_PicBox = true;


                Generate_ordinate_values_2();

                Generate_ordinate_values_3();


            }






            StringBuilder sbtemp33 = new StringBuilder();
            sbtemp33.Append(stringBuilder33.ToString());
            //============================================================================






            Remove1_SbTraceListener();
            //=======================================
            Start2_SbTraceListener();
            //===================================
            if (bool_DOF_Free_Greater_Than_0 == false)
            {
                Debug.Print(PRINT_1D_ARRAY_GLOBAL_REACTION_X_Y(supportreaction_no_dof));
            }
            if (bool_DOF_Free_Greater_Than_0 == true)
            {
                Debug.Print(PRINT_1D_ARRAY_GLOBAL_REACTION_X_Y(Support_Reaction));
            }


            Debug.Print("Disp_All_Nodes_Global  =  " + PRINT_1D_ARRAY_ALL_GLOBAL_DISP_X_Y_Z(Disp_All_Nodes_Global));
            Debug.Print(" Result_Mem_Force Local  :" + Environment.NewLine + PRINT_2D_ARRAY_ALL_MemID_CONN_LOCAL_AF_SHEAR_MZ(Result_Mem_Force_Local));

            StringBuilder sbtemp22 = new StringBuilder();
            sbtemp22.Append(stringBuilder22.ToString());
            Remove1_SbTraceListener();
            //============================================================================


            //===========================================================================
            stringBuilderALL = new StringBuilder();
            stringBuilderALL.Append(sbtemp11.ToString());
            stringBuilderALL.Append(sbtemp22.ToString());
            stringBuilderALL.Append(sbtemp33.ToString());

            //===========================================================================






        }//// public void Run_Analysis()









        private void Run_Analysis_SPRING()
        {


            double[] ff1 = new double[6];
            double[] ff2;
            double[,] t_mem1 = null;

            double[,] t_mem2 = null;

            double[] Disp_Element_I_To_J = new double[6];
            double[] temp12G = new double[6];

            double[] cn = new double[6];

            double[] ForeEnd_j0_global = new double[6];
            double[] ForeEnd_j0_Local = new double[6];
            double[] Disp_j0_global = new double[6];
            double[] Disp_j0_Local = null;
            double[] dloc = new double[6];

            double[] ff10 = new double[6];
            double[] ff11 = new double[6];
            double[] qq1 = new double[6];
            double[] qq2 = new double[6];


            double[,] Result_Mem_Force_global_Fixed = new double[totalmember + 1, (totalnode * 3) + 1];

            double[,] Result_Mem_Force_Local_Fixed = new double[totalmember + 1, (totalnode * 3) + 1];

            Disp_All_Nodes_Local = new double[totalnode * 3 + 1];




            double[] SUM_mem = new double[(totalnode * 3) + 1];
            PjG_memEnd_MebCommaConnectivity = new double[totalmember + 1, (totalnode * 3) + 1];
            double[] PjG_memEnd_1D_Connectivity = new double[(totalnode * 3) + 1];
            double[] Pj_joint_WithLoad = new double[(totalnode * 3) + 1];
            double[] Pj_joint_ALL = new double[(totalnode * 3) + 1];
            double[] Pmem_joint = new double[(totalnode * 3) + 1];
            double[,] PjLOCAL_memEnd_MebCommaConnectivity = new double[totalmember + 1, (totalnode * 3) + 1];
            Result_Mem_Force_global = new double[totalmember + 1, (totalnode * 3) + 1];
            Result_Mem_Force_Local = new double[totalmember + 1, (totalnode * 3) + 1];
            Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0 = new double[totalmember + 1, (totalnode * 3) + 1];
            Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0 = new double[totalmember + 1, (totalnode * 3) + 1];
            Disp_All_Nodes_Global = new double[totalnode * 3 + 1];



            int i = 0;
            int j = 0;
            int k = 0;


            int[] DOF_Restrained_Gx = new int[3 * totalnode + 1];
            double[] ForceEnd_Local_ALL_DOF = new double[3 * totalnode];
            int[] ALL_DOF_Free_Restrained = new int[3 * totalnode];
            int[] DOF_Restrained = new int[3 * totalnode + 1];
            int[] ALL_Restrained_DOF_Free = new int[3 * totalnode];


            for (j = 0; j <= 3 * totalnode - 1; j++)
            {
                ALL_DOF_Free_Restrained[j] = j + 1;

            }

            Debug.Print("ALL_DOF_Free_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(ALL_DOF_Free_Restrained));










            for (j = 1; j <= totalnode; j++)
            {


                if (suptype[j] == 1)
                {

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j]] = 3 * Sup_Node_Number[j];
                    Debug.Print(j + "Sup_Node_Number[j]fixed:" + Sup_Node_Number[j]);
                }
                if (suptype[j] == 2)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;

                    Debug.Print("Sup_Node_Number[j] hinged:" + Sup_Node_Number[j]);
                }
                if (suptype[j] == 3)
                {

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;

                    Debug.Print("Sup_Node_Number[j] X, MZ free, Y  restrained ROLLER-Y:" + Sup_Node_Number[j]);
                }

                if (suptype[j] == 4)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;


                    Debug.Print("Sup_Node_Number[j] Y, MZ free, X  restrained ROLLER-X:" + Sup_Node_Number[j]);
                }

                if (suptype[j] == 5)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j]] = 3 * Sup_Node_Number[j];

                }

                if (suptype[j] == 6)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j]] = 3 * Sup_Node_Number[j];

                    Node_Num_SPRING_Supp_Ydir = Sup_Node_Number[j];
                    bool_SPRING_Supp_Ydir = true;
                }

            }



            Debug.Print("DOF_Restrained_Gx :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Restrained_Gx));
            int tx = 0;
            k = 0;


            for (j = 0; j <= totalnode * 3; j++)
            {
                tx = DOF_Restrained_Gx[j];
                if (tx > 0)
                {


                    Array.Resize(ref DOF_Restrained, k + 1);
                    DOF_Restrained[k] = tx;
                    k = k + 1;
                }
            }
            Debug.Print("DOF_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Restrained));


            bool_DOF_Restrained_G_Greater_Than_2 = true;
            if (DOF_Restrained.Length < 3)
            {
                bool_DOF_Restrained_G_Greater_Than_2 = false;
                Debug.Print("DOF_Restrained.Length = " + DOF_Restrained.Length);
                MessageBox.Show("RUN ANALYSIS is QUITED because" + Environment.NewLine
                 + "the structure needs more supports !!! " + Environment.NewLine
                + "Degree of Restraint of 2D frame structure IS LESS THAN 3 !!!");
                return;

            }


            DOF_Free = Uncommon_Matrix_1D_1D_Intger(ALL_DOF_Free_Restrained, DOF_Restrained);
            bool bool_DOF_Free_Greater_Than_0 = true;
            if (DOF_Free.Length == 0) bool_DOF_Free_Greater_Than_0 = false;



            ALL_DOF_Free_Restrained = Union_Matrix_1D_1D_Intger(DOF_Free, DOF_Restrained);

            ALL_Restrained_DOF_Free = Union_Matrix_1D_1D_Intger(DOF_Restrained, DOF_Free);

            if (DOF_Free.Length > 0)
            {
                Debug.Print("DOF_Free :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Free));
            }
            else
            {
                Debug.Print("CAUTION - ALL DEGREES OF FREEDOM FIXED !!!");

            }
            Debug.Print("DOF_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Restrained));
            Debug.Print("ALL_DOF_Free_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(ALL_DOF_Free_Restrained));





            int mem_id = 0;

            double[,] matrixB = null;

            double[] YL = new double[DOF_Free.Length];

            double[] XX = null;





            fflocal1D = new double[6];
            int k12 = 0;

            for (k12 = 1; k12 <= totalmember; k12++)
            {


                mem_id = k12;

                Array.Clear(fflocal1D, 0, fflocal1D.Length);
                try
                {

                    if (!string.IsNullOrEmpty(Pload[mem_id]))
                    {

                        ForceEnd_Local_MemComaIndex = FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal(mem_id, Pload[mem_id]);
                        ForceEnd_Local_To_Global = FORCE_Eqiv_Nod_MembComaP_ij_1_0_TO_Global_INC_SUP(mem_id, Pload[mem_id]);
                        //Debug.Print(" ForceEnd_Local_To_Global " + MakeDisplayable(ForceEnd_Local_To_Global));
                        //Debug.Print(" ForceEnd_Global_ALL_DOF " + MakeDisplayable(ForceEnd_Local_To_Global));


                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 2] = ForceEnd_Local_MemComaIndex[mem_id, 0];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 1] = ForceEnd_Local_MemComaIndex[mem_id, 1];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3] = ForceEnd_Local_MemComaIndex[mem_id, 2];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 2] = ForceEnd_Local_MemComaIndex[mem_id, 3];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 1] = ForceEnd_Local_MemComaIndex[mem_id, 4];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3] = ForceEnd_Local_MemComaIndex[mem_id, 5];

                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 2] = ForceEnd_Local_To_Global[mem_id, 0];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 1] = ForceEnd_Local_To_Global[mem_id, 1];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3] = ForceEnd_Local_To_Global[mem_id, 2];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 2] = ForceEnd_Local_To_Global[mem_id, 3];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 1] = ForceEnd_Local_To_Global[mem_id, 4];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3] = ForceEnd_Local_To_Global[mem_id, 5];






                    }


                }
                catch (Exception ex)
                {

                    Debug.Print("ForceEnd_Local_MemComaIndex = FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal " + ex.ToString());
                }


            }


            //Debug.Print("PjLOCAL_memEnd_MebCommaConnectivity :" + Environment.NewLine + MakeDisplayable_2D_ARRAY_ALL_MemID_CONN(PjLOCAL_memEnd_MebCommaConnectivity));
            //Debug.Print("PjG_memEnd_MebCommaConnectivity :" + Environment.NewLine + MakeDisplayable_2D_ARRAY_ALL_MemID_CONN(PjG_memEnd_MebCommaConnectivity));




            if (bool_DOF_Free_Greater_Than_0 == true)
            {
                try
                {

                    ForceEnd_Local_ALL_DOF = new double[totalnode * 3 + 1];

                    ForceEnd_Global_ALL_DOF = new double[totalnode * 3 + 1];

                    for (j = 1; j <= totalnode * 3; j++)
                    {
                        for (k = 1; k <= totalmember; k++)
                        {
                            ForceEnd_Local_ALL_DOF[j] = ForceEnd_Local_ALL_DOF[j] + PjLOCAL_memEnd_MebCommaConnectivity[k, j];
                            ForceEnd_Global_ALL_DOF[j] = ForceEnd_Global_ALL_DOF[j] + PjG_memEnd_MebCommaConnectivity[k, j];
                        }

                    }
                    //Debug.Print(" ForceEnd_Local_ALL_DOF " + MakeDisplayable_1D_0_Double(ForceEnd_Local_ALL_DOF));
                    //Debug.Print(" ForceEnd_Global_ALL_DOF eqv nodal force" + MakeDisplayable_1D_0_Double(ForceEnd_Global_ALL_DOF));


                    SUM_mem = new double[(totalnode * 3) + 1];

                    for (j = 1; j <= totalnode * 3; j++)
                    {
                        for (k = 1; k <= totalmember; k++)
                        {
                            SUM_mem[j] = SUM_mem[j] + PjG_memEnd_MebCommaConnectivity[k, j];
                        }

                    }

                    //Debug.Print(" SUM_mem :" + Environment.NewLine + MakeDisplayable_1D_0_Double(SUM_mem));

                    //Debug.Print(" totalnode,  PJoint.Length  :" + totalnode + ", " + PJoint.Length);
                    double[] ffjG = null;
                    for (j = 1; j <= totalnode; j++)
                    {

                        if (!string.IsNullOrEmpty(PJoint[j]))
                        {


                            ffjG = Joint_Global_force_0_3(PJoint[j]);
                            k = Convert.ToInt32(ffjG[0]);
                            if (k >= 1)
                            {
                                Pj_joint_WithLoad[3 * k - 2] = ffjG[1];
                                Pj_joint_WithLoad[3 * k - 1] = ffjG[2];
                                Pj_joint_WithLoad[3 * k] = ffjG[3];
                            }

                        }
                    }




                    for (j = 1; j <= totalnode * 3; j++)
                    {

                        Pj_joint_ALL[j] = Pj_joint_ALL[j] + Pj_joint_WithLoad[j];

                    }
                    //Debug.Print("  Pj_joint :" + Environment.NewLine + MakeDisplayable_1D_0_Double(Pj_joint_ALL));




                    for (j = 1; j <= totalnode * 3; j++)
                    {
                        Pmem_joint[j] = SUM_mem[j] + Pj_joint_ALL[j];
                    }
                    //Debug.Print("  Pmem_joint(j ) :" + Environment.NewLine + MakeDisplayable_1D_0_Double(Pmem_joint));




                    YL = new double[DOF_Free.Length];
                    Debug.Print("" + Environment.NewLine + " Load Vector is as follows: .......... " + Environment.NewLine);
                    for (j = 0; j < DOF_Free.Length; j++)
                    {
                        YL[j] = Pmem_joint[DOF_Free[j]];

                        Debug.Print("DOF_Free[" + j + "] ,  " + "YL[" + j + "]  =" + DOF_Free[j] + "  , " + YL[j] + "");


                    }






                    double[,] Member_KG = null;
                    double[, ,] KGM_ALL = new double[totalmember + 1, totalnode * 3 + 1, totalnode * 3 + 1];
                    Array.Clear(KGM_ALL, 0, KGM_ALL.Length);
                    double[, ,] KGM_ALL2 = new double[totalmember + 1, totalnode * 3 + 1, totalnode * 3 + 1];
                    Array.Clear(KGM_ALL2, 0, KGM_ALL2.Length);

                    double[, ,] Mem_KG_mat = new double[totalmember + 1, 7, 7];

                    int i1 = 0;
                    int j1 = 0;

                    for (i = 1; i <= totalmember; i++)
                    {


                        Debug.Print("Member# " + i + "= Global Member Stiffness Matrix: " + Environment.NewLine + MakeDisplayable_ij_11_MemIDComma2DARRAY_CONNECTIVITY_Index(i, K_2_G_Element_i_1_j_1(i)));



                        Member_KG = K_2_G_Element_i_1_j_1(i);



                        int[] Index_1 = Index_6_From_MemID_1D_i_1(i);
                        for (i1 = 1; i1 <= 6; i1++)
                        {

                            for (j1 = 1; j1 <= 6; j1++)
                            {


                                KGM_ALL2[i, Index_1[i1], Index_1[j1]] = Member_KG[i1, j1];

                            }
                        }


                        //Debug.Print(i + " Member_KG Global ............. Matrix: " + Environment.NewLine + MakeDisplayable_ij_11_MemIDComma2DARRAY_CONNECTIVITY_Index(i, Member_KG));


                    }

                    Array.Clear(Member_KG, 0, Member_KG.Length);





                    double[,] result = new double[(totalnode * 3) + 1, (totalnode * 3) + 1];




                    for (i = 1; i <= totalmember; i++)
                    {

                        for (j = 1; j <= totalnode * 3; j++)
                        {

                            for (k = 1; k <= totalnode * 3; k++)
                            {
                                result[j, k] = result[j, k] + KGM_ALL2[i, j, k];
                            }
                        }
                    }

                    Debug.Print("Structure Stiffness Matrix SSM  =" + MakeDisplayable_ij_11_2DARRAY_CONNECTIVITY_Index(result));



                    if (bool_SPRING_Supp_Ydir == true)
                    {
                        result[Node_Num_SPRING_Supp_Ydir * 3 - 1, Node_Num_SPRING_Supp_Ydir * 3 - 1] = result[Node_Num_SPRING_Supp_Ydir * 3 - 1, Node_Num_SPRING_Supp_Ydir * 3 - 1] + SPRING_Constant;
                    }




                    double[,] K_SSM_Rearranged = new double[(totalnode * 3) + 1, (totalnode * 3) + 1];

                    int[] K_SSM_Rearranged_oldindex_1DArray = new int[(totalnode * 3) + 1];
                    double[,] K_SSM_Rearranged_oldindex_2DArray = new double[(totalnode * 3) + 1, 5];

                    for (i = 1; i <= totalnode * 3; i++)
                    {

                        for (j = 1; j <= totalnode * 3; j++)
                        {

                            K_SSM_Rearranged[i, j] = result[ALL_DOF_Free_Restrained[i - 1], ALL_DOF_Free_Restrained[j - 1]];

                            K_SSM_Rearranged_oldindex_1DArray[j] = ALL_DOF_Free_Restrained[j - 1];

                            K_SSM_Rearranged_oldindex_2DArray[i, 3] = ALL_DOF_Free_Restrained[i - 1];
                            K_SSM_Rearranged_oldindex_2DArray[j, 4] = ALL_DOF_Free_Restrained[j - 1];


                        }
                    }
                    //Debug.Print("1D Rearranged index " + MakeDisplayable_1D_j_0_INT(K_SSM_Rearranged_oldindex_1DArray));
                    //Debug.Print("2D Rearranged index" + MakeDisplayable_2D2D_ij_0_0(K_SSM_Rearranged_oldindex_2DArray));

                    Debug.Print("DOF_Free + DOF_Restrained Rearranged index and Rearranged Structure Stiffness Matrix, SSM_Rearranged = " + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(K_SSM_Rearranged_oldindex_2DArray, K_SSM_Rearranged));

                    //=================================================

                    //Debug.Print("TOTAL FREE DOOF without Specified i.e. restrained DOF= " + DOF_Free.Length);
                    double[,] Mat_Kff_2Darray_OLDINDEX = new double[DOF_Free.Length + 1, DOF_Free.Length + 5];
                    double[,] Mat_Kff = new double[DOF_Free.Length, DOF_Free.Length];
                    double[,] Mat_Kff11 = new double[DOF_Free.Length + 1, DOF_Free.Length + 1];
                    for (i = 0; i < DOF_Free.Length; i++)
                    {
                        for (j = 0; j < DOF_Free.Length; j++)
                        {
                            Mat_Kff[i, j] = K_SSM_Rearranged[i + 1, j + 1];
                            Mat_Kff11[i + 1, j + 1] = K_SSM_Rearranged[i + 1, j + 1];
                            Mat_Kff_2Darray_OLDINDEX[i + 1, 3] = DOF_Free[i];
                            Mat_Kff_2Darray_OLDINDEX[j + 1, 4] = DOF_Free[j];

                        }
                    }




                    Debug.Print("ALL DOF_Free Rearranged index and Rearranged Matrix Kff =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Kff_2Darray_OLDINDEX, Mat_Kff11));













                    double[,] Mat_Ksf_2Darray_OLDINDEX = new double[3 * totalnode + 1, 5];
                    double[,] K_BC_LeftBottom_Row_LeftBottomCol_Ksf = new double[DOF_Restrained.Length, DOF_Free.Length];
                    double[,] K_BC_LeftBottom_Row_LeftBottomCol_Ksf11 = new double[DOF_Restrained.Length + 1, DOF_Free.Length + 1];



                    for (i = 0; i <= DOF_Restrained.Length - 1; i++)
                    {
                        for (j = 0; j <= DOF_Free.Length - 1; j++)
                        {
                            K_BC_LeftBottom_Row_LeftBottomCol_Ksf[i, j] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, j + 1];
                            K_BC_LeftBottom_Row_LeftBottomCol_Ksf11[i + 1, j + 1] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, j + 1];


                            Mat_Ksf_2Darray_OLDINDEX[i + 1, 3] = DOF_Restrained[i];
                            Mat_Ksf_2Darray_OLDINDEX[j + 1, 4] = DOF_Free[j];

                        }
                    }



                    Debug.Print("Rearranged index and Rearranged Matrix  K_BC_LeftBottom_Row_LeftBottomCol_Ksf(,) =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Ksf_2Darray_OLDINDEX, K_BC_LeftBottom_Row_LeftBottomCol_Ksf11));



                    double[,] Mat_Kfs_2Darray_OLDINDEX = new double[3 * totalnode + 1, 5];
                    double[,] K_FreeRightRow_BC_Top_Col_Kfs = new double[DOF_Free.Length, DOF_Restrained.Length];
                    double[,] K_FreeRightRow_BC_Top_Col_Kfs11 = new double[DOF_Free.Length + 1, DOF_Restrained.Length + 1];



                    for (i = 0; i <= DOF_Free.Length - 1; i++)
                    {
                        for (j = 0; j <= DOF_Restrained.Length - 1; j++)
                        {
                            K_FreeRightRow_BC_Top_Col_Kfs[i, j] = K_SSM_Rearranged[i + 1, DOF_Free.Length - 1 + 1 + j + 1];
                            K_FreeRightRow_BC_Top_Col_Kfs11[i + 1, j + 1] = K_SSM_Rearranged[i + 1, DOF_Free.Length - 1 + 1 + j + 1];


                            Mat_Kfs_2Darray_OLDINDEX[i + 1, 3] = DOF_Free[i];
                            Mat_Kfs_2Darray_OLDINDEX[j + 1, 4] = DOF_Restrained[j];


                        }
                    }


                    Debug.Print("Rearranged index and Rearranged Matrix  K_FreeRightRow_BC_Top_Col_Kfs(,) =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Kfs_2Darray_OLDINDEX, K_FreeRightRow_BC_Top_Col_Kfs11));




                    double[,] Mat_Kss_2Darray_OLDINDEX = new double[3 * totalnode + 1, 5];
                    double[,] K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss = new double[DOF_Restrained.Length, DOF_Restrained.Length];
                    double[,] K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss11 = new double[DOF_Restrained.Length + 1, DOF_Restrained.Length + 1];



                    for (i = 0; i <= DOF_Restrained.Length - 1; i++)
                    {
                        for (j = 0; j <= DOF_Restrained.Length - 1; j++)
                        {
                            K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss[i, j] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, DOF_Free.Length - 1 + 1 + j + 1];
                            K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss11[i + 1, j + 1] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, DOF_Free.Length - 1 + 1 + j + 1];


                            Mat_Kss_2Darray_OLDINDEX[i + 1, 3] = DOF_Restrained[i];
                            Mat_Kss_2Darray_OLDINDEX[j + 1, 4] = DOF_Restrained[j];


                        }
                    }


                    Debug.Print("Rearranged index and Rearranged Matrix  K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss(,) =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Kss_2Darray_OLDINDEX, K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss11));










                    //====================================================




                    Debug.Print(String_info);

                    matrixB = LU.Inverse(Mat_Kff);
                    Debug.Print("Inverse free dof matrix Kff: " + Environment.NewLine + MakeDisplayable_2D2D_ij_0_0(matrixB));






                    bool_Force_Applied = true;
                    if (Find_All_Zero_Double_1Darray(YL) == true)
                    {
                        bool_Force_Applied = false;
                    }

                    if (bool_Force_Applied == false)
                    {
                        Debug.WriteLine("Find_All_Zero_Double_1Darray= " + Find_All_Zero_Double_1Darray(YL));
                        Debug.Print("  Pmem_joint(j ) :" + Environment.NewLine + MakeDisplayable_1D_0_Double(YL));
                        MessageBox.Show("RUN ANALYSIS is QUITED because" + Environment.NewLine
                      + "No force on BEAM or NODE was APPLIED !!!");
                        return;
                    }



                    Debug.Print("DOF_Free GLOBAL displacements:");
                    XX = Multiply_Matrix_2DBY1D_0_0(matrixB, YL);









                    double[] disp_Element = new double[6];


                    for (j = 0; j < totalnode * 3; j++)
                    {
                        Disp_All_Nodes_Global[j] = 0;
                    }

                    for (j = 0; j < XX.Length; j++)
                    {
                        Debug.Print("XX(" + DOF_Free[j] + ")" + XX[j]);
                        Disp_All_Nodes_Global[DOF_Free[j]] = XX[j];
                    }
                    for (j = 0; j < XX.Length; j++)
                    {
                        Debug.Print("Free DOF wise Global disp=  " + DOF_Free[j] + "  " + Disp_All_Nodes_Global[DOF_Free[j]]);
                    }





                    for (j = 0; j < ALL_DOF_Free_Restrained.Length; j++)
                    {

                        Debug.Print("ALL_DOF_Free_Restrained wise Global disp=  " + ALL_DOF_Free_Restrained[j] + "  " + Disp_All_Nodes_Global[ALL_DOF_Free_Restrained[j]]);
                    }

                    Debug.Print("Disp_All_Nodes_Global =  " + MakeDisplayable_1D_0_Double(Disp_All_Nodes_Global));



                    for (j = 0; j < ALL_DOF_Free_Restrained.Length; j++)
                    {

                        Debug.Print("ALL_DOF_Free_Restrained wise Global disp=  " + ALL_DOF_Free_Restrained[j] + "  " + Disp_All_Nodes_Global[ALL_DOF_Free_Restrained[j]]);
                    }




                    double[] Reaction = Multiply_Matrix_2DBY1D_0_0(K_BC_LeftBottom_Row_LeftBottomCol_Ksf, XX);








                    double[] SupportReaction1 = new double[totalnode * 3 + 1];
                    Support_Reaction = new double[totalnode * 3 + 1];
                    int[] a22 = new int[] { Node_Num_SPRING_Supp_Ydir * 3 - 1 };
                    int[] b22 = Union_Matrix_1D_1D_Intger(DOF_Restrained, a22);
                    //Debug.Print("SPRING b22 " + MakeDisplayable_1D_j_0_INT(b22));
                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {
                        SupportReaction1[j] = Reaction[j] - ForceEnd_Global_ALL_DOF[DOF_Restrained[j]];
                        Debug.Print(DOF_Restrained[j] + "Reaction,  eq FEM, R-FEM " + Reaction[j] + " , " + ForceEnd_Global_ALL_DOF[DOF_Restrained[j]] + " , " + SupportReaction1[j]);

                    }
                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {
                        Debug.Print(DOF_Restrained[j] + "  SupportReaction incuding Spring Reaction =" + SupportReaction1[j]);

                    }


                    if (bool_SPRING_Supp_Ydir == true)
                    {
                        Debug.Print("Node_Num_SPRING_Supp_Ydir * 3 - 1  =" + (Node_Num_SPRING_Supp_Ydir * 3 - 1) + ", " + Disp_All_Nodes_Global[Node_Num_SPRING_Supp_Ydir * 3 - 1] + ", " + Disp_All_Nodes_Global[Node_Num_SPRING_Supp_Ydir * 3 - 1] * SPRING_Constant);

                    }

                    SupportReaction1[DOF_Restrained.Length] = SPRING_Constant * Disp_All_Nodes_Global[Node_Num_SPRING_Supp_Ydir * 3 - 1];

                    for (j = 0; j < DOF_Restrained.Length + 1; j++)
                    {
                        Debug.Print(j + "," + b22[j] + "  SupportReaction incuding Spring Reaction 1 =" + SupportReaction1[j]);
                    }



                    double[] SupportReaction2 = new double[totalnode * 3 + 1];
                    for (j = 0; j < DOF_Restrained.Length + 1; j++)
                    {
                        SupportReaction2[b22[j]] = SupportReaction1[j];

                        Debug.Print(j + "," + b22[j] + "  SupportReaction incuding Spring Reaction 2 =" + SupportReaction2[b22[j]]);
                    }


                    for (j = 0; j < DOF_Restrained.Length + 1; j++)
                    {

                        Debug.Print(j + "," + b22[j] + "  SupportReaction incuding Spring Reaction 3 =" + SupportReaction2[b22[j]]);
                    }



                    int[] c22 = sort_Integer_1D_j_0(b22);

                    for (j = 0; j < c22.Length; j++)
                    {
                        Support_Reaction[c22[j]] = SupportReaction2[c22[j]];
                        Debug.Print(j + "," + c22[j] + "  SupportReaction incuding Spring Reaction 4 =" + SupportReaction2[c22[j]] + ",," + Support_Reaction[c22[j]]);
                    }







                    for (k = 1; k <= totalmember; k++)
                    {

                        Disp_j0_global[0] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 2];
                        Disp_j0_global[1] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 1];
                        Disp_j0_global[2] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3];
                        Disp_j0_global[3] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 2];
                        Disp_j0_global[4] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 1];
                        Disp_j0_global[5] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3];


                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 2] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 2];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 1] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 1];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 2] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 2];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 1] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 1];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3];







                        t_mem2 = Trasform_mat_G_TO_L_ij_00(cosx(k), sinx(k));

                        Disp_j0_Local = Multiply_Matrix_2DBY1D_0_0(t_mem2, Disp_j0_global);


                        Disp_All_Nodes_Local[Connectivity[k, 1] * 3 - 2] = Disp_j0_Local[0];
                        Disp_All_Nodes_Local[Connectivity[k, 1] * 3 - 1] = Disp_j0_Local[1];
                        Disp_All_Nodes_Local[Connectivity[k, 1] * 3] = Disp_j0_Local[2];
                        Disp_All_Nodes_Local[Connectivity[k, 2] * 3 - 2] = Disp_j0_Local[3];
                        Disp_All_Nodes_Local[Connectivity[k, 2] * 3 - 1] = Disp_j0_Local[4];
                        Disp_All_Nodes_Local[Connectivity[k, 2] * 3] = Disp_j0_Local[5];


                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 2] = Disp_j0_Local[0];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 1] = Disp_j0_Local[1];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3] = Disp_j0_Local[2];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 2] = Disp_j0_Local[3];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 1] = Disp_j0_Local[4];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3] = Disp_j0_Local[5];








                        t_mem1 = K_2_G_Element_i_0_j_0(k);




                        ff1 = Multiply_Matrix_2DBY1D_0_0(t_mem1, Disp_j0_global);








                        Result_Mem_Force_global[k, Connectivity[k, 1] * 3 - 2] = ff1[0] - PjG_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 2];
                        Result_Mem_Force_global[k, Connectivity[k, 1] * 3 - 1] = ff1[1] - PjG_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 1];
                        Result_Mem_Force_global[k, Connectivity[k, 1] * 3] = ff1[2] - PjG_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3];
                        Result_Mem_Force_global[k, Connectivity[k, 2] * 3 - 2] = ff1[3] - PjG_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 2];
                        Result_Mem_Force_global[k, Connectivity[k, 2] * 3 - 1] = ff1[4] - PjG_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 1];
                        Result_Mem_Force_global[k, Connectivity[k, 2] * 3] = ff1[5] - PjG_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3];



                        t_mem2 = Trasform_mat_G_TO_L_ij_00(cosx(k), sinx(k));

                        ff2 = Multiply_Matrix_2DBY1D_0_0(t_mem2, ff1);




                        Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 2] = ff2[0] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 2];
                        Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 1] = ff2[1] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 1];
                        Result_Mem_Force_Local[k, Connectivity[k, 1] * 3] = ff2[2] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3];
                        Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 2] = ff2[3] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 2];
                        Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 1] = ff2[4] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 1];
                        Result_Mem_Force_Local[k, Connectivity[k, 2] * 3] = ff2[5] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3];

                    }


                    //Debug.Print(" Result_Mem_Force_global  :" + Environment.NewLine + MakeDisplayable(Result_Mem_Force_global));

                    //Debug.Print(" Result_Mem_Force Local  :" + Environment.NewLine + MakeDisplayable(Result_Mem_Force_Local));





                }

                catch (Exception ex)
                {

                    Debug.Print("AFTER End of assignment of local and global equivalent forces to nodal forces :" + Environment.NewLine + ex.ToString());
                }




            }

            StringBuilder sbtemp33 = new StringBuilder();
            sbtemp33.Append(stringBuilder33.ToString());
            //============================================================================






            Remove1_SbTraceListener();
            //=======================================
            Start2_SbTraceListener();
            //===================================



            Debug.Print(PRINT_1D_ARRAY_GLOBAL_REACTION_X_Y(Support_Reaction));
            Debug.Print("Disp_All_Nodes_Global  =  " + PRINT_1D_ARRAY_ALL_GLOBAL_DISP_X_Y_Z(Disp_All_Nodes_Global));
            Debug.Print(" Result_Mem_Force Local  :" + Environment.NewLine + PRINT_2D_ARRAY_ALL_MemID_CONN_LOCAL_AF_SHEAR_MZ(Result_Mem_Force_Local));

            StringBuilder sbtemp22 = new StringBuilder();
            sbtemp22.Append(stringBuilder22.ToString());
            Remove1_SbTraceListener();
            //============================================================================


            //===========================================================================
            stringBuilderALL = new StringBuilder();
            stringBuilderALL.Append(sbtemp11.ToString());
            stringBuilderALL.Append(sbtemp22.ToString());
            stringBuilderALL.Append(sbtemp33.ToString());

            //===========================================================================












        }//// end  private void Run_Analysis_SPRING()








        private void Run_Analysis_Inclided_Supp()
        {



            double[] ff2 = new double[6];
            double[,] t_mem1 = null;

            double[,] t_mem2 = null;

            double[] Disp_Element_I_To_J = new double[6];
            double[] temp12G = new double[6];

            double[] cn = new double[6];

            double[] ForeEnd_j0_global = new double[6];
            double[] ForeEnd_j0_Local = new double[6];
            double[] Disp_j0_global = new double[6];
            double[] Disp_j0_Local = null;
            double[] dloc = new double[6];

            double[] ff10 = new double[6];
            double[] ff11 = new double[6];
            double[] qq1 = new double[6];
            double[] qq2 = new double[6];


            double[,] Result_Mem_Force_global_Fixed = new double[totalmember + 1, (totalnode * 3) + 1];

            double[,] Result_Mem_Force_Local_Fixed = new double[totalmember + 1, (totalnode * 3) + 1];

            Disp_All_Nodes_Local = new double[totalnode * 3 + 1];




            double[] SUM_mem = new double[(totalnode * 3) + 1];
            PjG_memEnd_MebCommaConnectivity = new double[totalmember + 1, (totalnode * 3) + 1];
            double[] PjG_memEnd_1D_Connectivity = new double[(totalnode * 3) + 1];
            double[] Pj_joint_WithLoad = new double[(totalnode * 3) + 1];
            double[] Pj_joint_ALL = new double[(totalnode * 3) + 1];
            double[] Pmem_joint = new double[(totalnode * 3) + 1];
            double[,] PjLOCAL_memEnd_MebCommaConnectivity = new double[totalmember + 1, (totalnode * 3) + 1];
            Result_Mem_Force_global = new double[totalmember + 1, (totalnode * 3) + 1];
            Result_Mem_Force_Local = new double[totalmember + 1, (totalnode * 3) + 1];
            Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0 = new double[totalmember + 1, (totalnode * 3) + 1];
            Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0 = new double[totalmember + 1, (totalnode * 3) + 1];
            Disp_All_Nodes_Global = new double[totalnode * 3 + 1];



            int i = 0;
            int j = 0;
            int k = 0;


            int[] DOF_Restrained_Gx = new int[3 * totalnode + 1];
            double[] ForceEnd_Local_ALL_DOF = new double[3 * totalnode];
            int[] ALL_DOF_Free_Restrained = new int[3 * totalnode];
            int[] DOF_Restrained = new int[3 * totalnode + 1];
            int[] ALL_Restrained_DOF_Free = new int[3 * totalnode];


            for (j = 0; j <= 3 * totalnode - 1; j++)
            {
                ALL_DOF_Free_Restrained[j] = j + 1;

            }

            //Debug.Print("ALL_DOF_Free_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(ALL_DOF_Free_Restrained));









            for (j = 1; j <= totalnode; j++)
            {


                if (suptype[j] == 1)
                {

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j]] = 3 * Sup_Node_Number[j];
                    //Debug.Print(j + "Sup_Node_Number[j]fixed:" + Sup_Node_Number[j]);
                }
                if (suptype[j] == 2)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;

                    //Debug.Print("Sup_Node_Number[j] hinged:" + Sup_Node_Number[j]);
                }
                if (suptype[j] == 3)
                {

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;

                    //Debug.Print("Sup_Node_Number[j] X, MZ free, Y  restrained ROLLER-Y:" + Sup_Node_Number[j]);
                }

                if (suptype[j] == 4)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;


                    //Debug.Print("Sup_Node_Number[j] Y, MZ free, X  restrained ROLLER-X:" + Sup_Node_Number[j]);
                }

                if (suptype[j] == 5)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j]] = 3 * Sup_Node_Number[j];

                }

                if (suptype[j] == 6)
                {
                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 2] = 3 * Sup_Node_Number[j] - 2;

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j]] = 3 * Sup_Node_Number[j];

                    Node_Num_SPRING_Supp_Ydir = Sup_Node_Number[j];
                    bool_SPRING_Supp_Ydir = true;
                }


                if (suptype[j] == 7)
                {

                    DOF_Restrained_Gx[3 * Sup_Node_Number[j] - 1] = 3 * Sup_Node_Number[j] - 1;

                    //Debug.Print("Sup_Node_Number[j] X, MZ free, Y  restrained INC ROLLER-Y:" + Sup_Node_Number[j]);

                    bool_Inclied_Supp = true;
                    bool_SPRING_Supp_Ydir = false;













                    //Debug.Print( Inc_Supp_angle_base_AND_GX  " + Inc_Supp_angle_base_AND_GX + " , " + Inclied_Supp_Node_num);

                }
            }




            //Debug.Print("DOF_Restrained_Gx :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Restrained_Gx));
            int tx = 0;
            k = 0;


            for (j = 0; j <= totalnode * 3; j++)
            {
                tx = DOF_Restrained_Gx[j];
                if (tx > 0)
                {


                    Array.Resize(ref DOF_Restrained, k + 1);
                    DOF_Restrained[k] = tx;
                    k = k + 1;
                }
            }


            bool_DOF_Restrained_G_Greater_Than_2 = true;
            if (DOF_Restrained.Length < 3)
            {
                bool_DOF_Restrained_G_Greater_Than_2 = false;
                //Debug.Print("DOF_Restrained.Length = " + DOF_Restrained.Length);
                MessageBox.Show("RUN ANALYSIS is QUITED because" + Environment.NewLine
                 + "the structure needs more supports !!! " + Environment.NewLine
                + "Degree of Restraint of 2D frame structure IS LESS THAN 3 !!!");
                return;

            }


            DOF_Free = Uncommon_Matrix_1D_1D_Intger(ALL_DOF_Free_Restrained, DOF_Restrained);
            bool bool_DOF_Free_Greater_Than_0 = true;
            if (DOF_Free.Length == 0) bool_DOF_Free_Greater_Than_0 = false;



            ALL_DOF_Free_Restrained = Union_Matrix_1D_1D_Intger(DOF_Free, DOF_Restrained);

            if (DOF_Free.Length > 0)
            {
                Debug.Print("DOF_Free :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Free));
            }
            else
            {
                Debug.Print("CAUTION - ALL DEGREES OF FREEDOM FIXED !!!");

            }
            Debug.Print("DOF_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(DOF_Restrained));
            Debug.Print("ALL_DOF_Free_Restrained :" + Environment.NewLine + MakeDisplayable_1D_j_0_INT(ALL_DOF_Free_Restrained));










            int mem_id = 0;

            double[,] matrixB = null;

            double[] YL = new double[DOF_Free.Length];

            double[] XX = null;





            fflocal1D = new double[6];
            int k12 = 0;

            for (k12 = 1; k12 <= totalmember; k12++)
            {


                mem_id = k12;

                Array.Clear(fflocal1D, 0, fflocal1D.Length);
                try
                {

                    if (!string.IsNullOrEmpty(Pload[mem_id]))
                    {

                        ForceEnd_Local_MemComaIndex = FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal(mem_id, Pload[mem_id]);
                        ForceEnd_Local_To_Global = FORCE_Eqiv_Nod_MembComaP_ij_1_0_TO_Global_INC_SUP(mem_id, Pload[mem_id]);
                        Debug.Print(" ForceEnd_Local_To_Global " + MakeDisplayable(ForceEnd_Local_To_Global));
                        Debug.Print(" ForceEnd_Global_ALL_DOF " + MakeDisplayable(ForceEnd_Local_To_Global));


                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 2] = ForceEnd_Local_MemComaIndex[mem_id, 0];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 1] = ForceEnd_Local_MemComaIndex[mem_id, 1];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3] = ForceEnd_Local_MemComaIndex[mem_id, 2];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 2] = ForceEnd_Local_MemComaIndex[mem_id, 3];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 1] = ForceEnd_Local_MemComaIndex[mem_id, 4];
                        PjLOCAL_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3] = ForceEnd_Local_MemComaIndex[mem_id, 5];

                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 2] = ForceEnd_Local_To_Global[mem_id, 0];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3 - 1] = ForceEnd_Local_To_Global[mem_id, 1];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 1] * 3] = ForceEnd_Local_To_Global[mem_id, 2];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 2] = ForceEnd_Local_To_Global[mem_id, 3];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3 - 1] = ForceEnd_Local_To_Global[mem_id, 4];
                        PjG_memEnd_MebCommaConnectivity[mem_id, Connectivity[mem_id, 2] * 3] = ForceEnd_Local_To_Global[mem_id, 5];






                    }


                }
                catch (Exception ex)
                {

                    Debug.Print("ForceEnd_Local_MemComaIndex = FORCE_Eqiv_Nod_MembComaP_ij_1_0_Loacal " + ex.ToString());
                }


            }


            //Debug.Print("PjLOCAL_memEnd_MebCommaConnectivity :" + Environment.NewLine + MakeDisplayable_2D_ARRAY_ALL_MemID_CONN(PjLOCAL_memEnd_MebCommaConnectivity));
            //Debug.Print("PjG_memEnd_MebCommaConnectivity :" + Environment.NewLine + MakeDisplayable_2D_ARRAY_ALL_MemID_CONN(PjG_memEnd_MebCommaConnectivity));




            if (bool_DOF_Free_Greater_Than_0 == true)
            {
                try
                {

                    ForceEnd_Local_ALL_DOF = new double[totalnode * 3 + 1];

                    ForceEnd_Global_ALL_DOF = new double[totalnode * 3 + 1];

                    for (j = 1; j <= totalnode * 3; j++)
                    {
                        for (k = 1; k <= totalmember; k++)
                        {
                            ForceEnd_Local_ALL_DOF[j] = ForceEnd_Local_ALL_DOF[j] + PjLOCAL_memEnd_MebCommaConnectivity[k, j];
                            ForceEnd_Global_ALL_DOF[j] = ForceEnd_Global_ALL_DOF[j] + PjG_memEnd_MebCommaConnectivity[k, j];
                        }

                    }
                    Debug.Print(" ForceEnd_Local_ALL_DOF " + MakeDisplayable_1D_0_Double(ForceEnd_Local_ALL_DOF));
                    Debug.Print(" ForceEnd_Global_ALL_DOF eqv nodal force" + MakeDisplayable_1D_0_Double(ForceEnd_Global_ALL_DOF));


                    SUM_mem = new double[(totalnode * 3) + 1];

                    for (j = 1; j <= totalnode * 3; j++)
                    {
                        for (k = 1; k <= totalmember; k++)
                        {
                            SUM_mem[j] = SUM_mem[j] + PjG_memEnd_MebCommaConnectivity[k, j];
                        }

                    }

                    Debug.Print(" SUM_mem :" + Environment.NewLine + MakeDisplayable_1D_0_Double(SUM_mem));

                    Debug.Print(" totalnode,  PJoint.Length  :" + totalnode + ", " + PJoint.Length);
                    double[] ffjG = null;
                    for (j = 1; j <= totalnode; j++)
                    {

                        if (!string.IsNullOrEmpty(PJoint[j]))
                        {


                            ffjG = Joint_Global_force_0_3(PJoint[j]);
                            k = Convert.ToInt32(ffjG[0]);
                            if (k >= 1)
                            {
                                Pj_joint_WithLoad[3 * k - 2] = ffjG[1];
                                Pj_joint_WithLoad[3 * k - 1] = ffjG[2];
                                Pj_joint_WithLoad[3 * k] = ffjG[3];
                            }

                        }
                    }




                    for (j = 1; j <= totalnode * 3; j++)
                    {

                        Pj_joint_ALL[j] = Pj_joint_ALL[j] + Pj_joint_WithLoad[j];

                    }
                    Debug.Print("  Pj_joint :" + Environment.NewLine + MakeDisplayable_1D_0_Double(Pj_joint_ALL));




                    for (j = 1; j <= totalnode * 3; j++)
                    {
                        Pmem_joint[j] = SUM_mem[j] + Pj_joint_ALL[j];
                    }
                    Debug.Print("  Pmem_joint(j ) :" + Environment.NewLine + MakeDisplayable_1D_0_Double(Pmem_joint));




                    YL = new double[DOF_Free.Length];
                    Debug.Print("" + Environment.NewLine + " Load Vector is as follows: .......... " + Environment.NewLine);
                    for (j = 0; j < DOF_Free.Length; j++)
                    {
                        YL[j] = Pmem_joint[DOF_Free[j]];

                        Debug.Print("DOF_Free[" + j + "] ,  " + "YL[" + j + "]  =" + DOF_Free[j] + "  , " + YL[j] + "");


                    }






                    double[,] Member_KG = null;
                    double[, ,] KGM_ALL = new double[totalmember + 1, totalnode * 3 + 1, totalnode * 3 + 1];
                    Array.Clear(KGM_ALL, 0, KGM_ALL.Length);
                    double[, ,] KGM_ALL2 = new double[totalmember + 1, totalnode * 3 + 1, totalnode * 3 + 1];
                    Array.Clear(KGM_ALL2, 0, KGM_ALL2.Length);

                    double[, ,] Mem_KG_mat = new double[totalmember + 1, 7, 7];

                    int i1 = 0;
                    int j1 = 0;

                    for (i = 1; i <= totalmember; i++)
                    {

                        Debug.Print("Member# " + i + "= Global Member Stiffness Matrix INCLINED_SUP: " + Environment.NewLine + MakeDisplayable_ij_11_MemIDComma2DARRAY_CONNECTIVITY_Index(i, K_2_G_Element_i_1_j_1_INCLINED_SUP(i)));

                        Member_KG = K_2_G_Element_i_1_j_1_INCLINED_SUP(i);



                        int[] Index_1 = Index_6_From_MemID_1D_i_1(i);
                        for (i1 = 1; i1 <= 6; i1++)
                        {

                            for (j1 = 1; j1 <= 6; j1++)
                            {


                                KGM_ALL2[i, Index_1[i1], Index_1[j1]] = Member_KG[i1, j1];

                            }
                        }




                        for (k = 1; k <= 3; k++)
                        {
                            for (j = 1; j <= 3; j++)
                            {


                                KGM_ALL[i, Connectivity[i, 1] * 3 - 3 + k, Connectivity[i, 1] * 3 - 3 + j] = Member_KG[k, j];

                                KGM_ALL[i, Connectivity[i, 1] * 3 - 3 + k, Connectivity[i, 2] * 3 - 3 + j] = Member_KG[k, j + 3];

                                KGM_ALL[i, Connectivity[i, 2] * 3 - 3 + k, Connectivity[i, 1] * 3 - 3 + j] = Member_KG[k + 3, j];

                                KGM_ALL[i, Connectivity[i, 2] * 3 - 3 + k, Connectivity[i, 2] * 3 - 3 + j] = Member_KG[k + 3, j + 3];



                            }
                        }

                        Array.Clear(Member_KG, 0, Member_KG.Length);

                    }




                    double[,] result = new double[(totalnode * 3) + 1, (totalnode * 3) + 1];

                    for (i = 1; i <= totalmember; i++)
                    {

                        for (j = 1; j <= totalnode * 3; j++)
                        {

                            for (k = 1; k <= totalnode * 3; k++)
                            {
                                result[j, k] = result[j, k] + KGM_ALL2[i, j, k];
                            }
                        }
                    }

                    Debug.Print("Structure Stiffness Matrix SSM  =" + MakeDisplayable_ij_11_2DARRAY_CONNECTIVITY_Index(result));









                    double[,] K_SSM_Rearranged = new double[(totalnode * 3) + 1, (totalnode * 3) + 1];

                    int[] K_SSM_Rearranged_oldindex_1DArray = new int[(totalnode * 3) + 1];
                    double[,] K_SSM_Rearranged_oldindex_2DArray = new double[(totalnode * 3) + 1, 5];

                    for (i = 1; i <= totalnode * 3; i++)
                    {

                        for (j = 1; j <= totalnode * 3; j++)
                        {

                            K_SSM_Rearranged[i, j] = result[ALL_DOF_Free_Restrained[i - 1], ALL_DOF_Free_Restrained[j - 1]];

                            K_SSM_Rearranged_oldindex_1DArray[j] = ALL_DOF_Free_Restrained[j - 1];

                            K_SSM_Rearranged_oldindex_2DArray[i, 3] = ALL_DOF_Free_Restrained[i - 1];
                            K_SSM_Rearranged_oldindex_2DArray[j, 4] = ALL_DOF_Free_Restrained[j - 1];


                        }
                    }

                    Debug.Print("DOF_Free + DOF_Restrained Rearranged index and Rearranged Structure Stiffness Matrix, SSM_Rearranged = " + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(K_SSM_Rearranged_oldindex_2DArray, K_SSM_Rearranged));







                    //Debug.Print("TOTAL FREE DOOF without Specified i.e. restrained DOF= " + DOF_Free.Length);
                    double[,] Mat_Kff_2Darray_OLDINDEX = new double[DOF_Free.Length + 1, DOF_Free.Length + 5];
                    double[,] Mat_Kff = new double[DOF_Free.Length, DOF_Free.Length];
                    double[,] Mat_Kff11 = new double[DOF_Free.Length + 1, DOF_Free.Length + 1];
                    for (i = 0; i < DOF_Free.Length; i++)
                    {
                        for (j = 0; j < DOF_Free.Length; j++)
                        {
                            Mat_Kff[i, j] = K_SSM_Rearranged[i + 1, j + 1];
                            Mat_Kff11[i + 1, j + 1] = K_SSM_Rearranged[i + 1, j + 1];
                            Mat_Kff_2Darray_OLDINDEX[i + 1, 3] = DOF_Free[i];
                            Mat_Kff_2Darray_OLDINDEX[j + 1, 4] = DOF_Free[j];

                        }
                    }




                    Debug.Print("ALL DOF_Free Rearranged index and Rearranged Matrix Kff =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Kff_2Darray_OLDINDEX, Mat_Kff11));













                    double[,] Mat_Ksf_2Darray_OLDINDEX = new double[3 * totalnode + 1, 5];
                    double[,] K_BC_LeftBottom_Row_LeftBottomCol_Ksf = new double[DOF_Restrained.Length, DOF_Free.Length];
                    double[,] K_BC_LeftBottom_Row_LeftBottomCol_Ksf11 = new double[DOF_Restrained.Length + 1, DOF_Free.Length + 1];



                    for (i = 0; i <= DOF_Restrained.Length - 1; i++)
                    {
                        for (j = 0; j <= DOF_Free.Length - 1; j++)
                        {
                            K_BC_LeftBottom_Row_LeftBottomCol_Ksf[i, j] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, j + 1];
                            K_BC_LeftBottom_Row_LeftBottomCol_Ksf11[i + 1, j + 1] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, j + 1];


                            Mat_Ksf_2Darray_OLDINDEX[i + 1, 3] = DOF_Restrained[i];
                            Mat_Ksf_2Darray_OLDINDEX[j + 1, 4] = DOF_Free[j];

                        }
                    }



                    Debug.Print("Rearranged index and Rearranged Matrix  K_BC_LeftBottom_Row_LeftBottomCol_Ksf(,) =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Ksf_2Darray_OLDINDEX, K_BC_LeftBottom_Row_LeftBottomCol_Ksf11));



                    double[,] Mat_Kfs_2Darray_OLDINDEX = new double[3 * totalnode + 1, 5];
                    double[,] K_FreeRightRow_BC_Top_Col_Kfs = new double[DOF_Free.Length, DOF_Restrained.Length];
                    double[,] K_FreeRightRow_BC_Top_Col_Kfs11 = new double[DOF_Free.Length + 1, DOF_Restrained.Length + 1];



                    for (i = 0; i <= DOF_Free.Length - 1; i++)
                    {
                        for (j = 0; j <= DOF_Restrained.Length - 1; j++)
                        {
                            K_FreeRightRow_BC_Top_Col_Kfs[i, j] = K_SSM_Rearranged[i + 1, DOF_Free.Length - 1 + 1 + j + 1];
                            K_FreeRightRow_BC_Top_Col_Kfs11[i + 1, j + 1] = K_SSM_Rearranged[i + 1, DOF_Free.Length - 1 + 1 + j + 1];


                            Mat_Kfs_2Darray_OLDINDEX[i + 1, 3] = DOF_Free[i];
                            Mat_Kfs_2Darray_OLDINDEX[j + 1, 4] = DOF_Restrained[j];


                        }
                    }


                    Debug.Print("Rearranged index and Rearranged Matrix  K_FreeRightRow_BC_Top_Col_Kfs(,) =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Kfs_2Darray_OLDINDEX, K_FreeRightRow_BC_Top_Col_Kfs11));




                    double[,] Mat_Kss_2Darray_OLDINDEX = new double[3 * totalnode + 1, 5];
                    double[,] K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss = new double[DOF_Restrained.Length, DOF_Restrained.Length];
                    double[,] K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss11 = new double[DOF_Restrained.Length + 1, DOF_Restrained.Length + 1];



                    for (i = 0; i <= DOF_Restrained.Length - 1; i++)
                    {
                        for (j = 0; j <= DOF_Restrained.Length - 1; j++)
                        {
                            K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss[i, j] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, DOF_Free.Length - 1 + 1 + j + 1];
                            K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss11[i + 1, j + 1] = K_SSM_Rearranged[DOF_Free.Length - 1 + 1 + i + 1, DOF_Free.Length - 1 + 1 + j + 1];


                            Mat_Kss_2Darray_OLDINDEX[i + 1, 3] = DOF_Restrained[i];
                            Mat_Kss_2Darray_OLDINDEX[j + 1, 4] = DOF_Restrained[j];


                        }
                    }


                    Debug.Print("Rearranged index and Rearranged Matrix  K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss(,) =" + Environment.NewLine + MakeDisplayable_ij_11_2DArrayOld_coma_2DARRAY_OLDNEW_Index(Mat_Kss_2Darray_OLDINDEX, K_BC_LeftBottom_Row_BC_RightBottom_Col_Kss11));

















                    Debug.Print(String_info);

                    matrixB = LU.Inverse(Mat_Kff);
                    Debug.Print("Inverse free dof matrix Kff: " + Environment.NewLine + MakeDisplayable_2D2D_ij_0_0(matrixB));











                    bool_Force_Applied = true;
                    if (Find_All_Zero_Double_1Darray(YL) == true)
                    {
                        bool_Force_Applied = false;
                    }

                    if (bool_Force_Applied == false)
                    {
                        Debug.WriteLine("Find_All_Zero_Double_1Darray= " + Find_All_Zero_Double_1Darray(YL));
                        Debug.Print("  Pmem_joint(j ) :" + Environment.NewLine + MakeDisplayable_1D_0_Double(YL));
                        MessageBox.Show("RUN ANALYSIS is QUITED because" + Environment.NewLine
                      + "No force on BEAM or NODE was APPLIED !!!");
                        return;
                    }



                    Debug.Print("DOF_Free GLOBAL displacements:");
                    XX = Multiply_Matrix_2DBY1D_0_0(matrixB, YL);









                    double[] disp_Element = new double[6];


                    for (j = 0; j < totalnode * 3; j++)
                    {
                        Disp_All_Nodes_Global[j] = 0;
                    }

                    for (j = 0; j < XX.Length; j++)
                    {
                        Debug.Print("XX(" + DOF_Free[j] + ")" + XX[j]);
                        Disp_All_Nodes_Global[DOF_Free[j]] = XX[j];
                    }
                    for (j = 0; j < XX.Length; j++)
                    {
                        Debug.Print("Free DOF wise Global disp=  " + DOF_Free[j] + "  " + Disp_All_Nodes_Global[DOF_Free[j]]);
                    }





                    for (j = 0; j < ALL_DOF_Free_Restrained.Length; j++)
                    {

                        Debug.Print("ALL_DOF_Free_Restrained wise Global disp=  " + ALL_DOF_Free_Restrained[j] + "  " + Disp_All_Nodes_Global[ALL_DOF_Free_Restrained[j]]);
                    }

                    Debug.Print(" Disp_All_Nodes_Global =  " + MakeDisplayable_1D_0_Double(Disp_All_Nodes_Global));






                    for (j = 0; j < ALL_DOF_Free_Restrained.Length; j++)
                    {

                        Debug.Print("ALL_DOF_Free_Restrained wise Global disp=  " + ALL_DOF_Free_Restrained[j] + "  " + Disp_All_Nodes_Global[ALL_DOF_Free_Restrained[j]]);
                    }



                    double[] dispINC_G = new double[3];
                    for (j = 1; j <= totalnode * 3; j++)
                    {

                        Debug.Print(" ALL Global disp=  " + (j) + " , " + Disp_All_Nodes_Global[j]);
                        if ((j + 2) / 3d == Inclied_Supp_Node_num)
                        {
                            dispINC_G[0] = Disp_All_Nodes_Global[j];
                            Debug.Print(" X ALL Global disp=  " + (j) + " , " + Disp_All_Nodes_Global[j]);

                        }
                        if ((j + 1) / 3d == Inclied_Supp_Node_num)
                        {
                            dispINC_G[1] = Disp_All_Nodes_Global[j];
                            Debug.Print(" Y ALL Global disp=  " + (j) + " , " + Disp_All_Nodes_Global[j]);

                        }
                        if ((j + 0) / 3d == Inclied_Supp_Node_num)
                        {
                            dispINC_G[2] = Disp_All_Nodes_Global[j];
                            Debug.Print(" Z ALL Global disp=  " + (j) + " , " + Disp_All_Nodes_Global[j]);

                        }

                    }




                    for (j = 1; j <= totalnode * 3; j++)
                    {


                        if ((j + 2) / 3d == Inclied_Supp_Node_num)
                        {
                            Disp_All_Nodes_Global[j] = dispINC_G[0] * Math.Cos(Math.PI / 180 * Inc_Supp_angle_base_AND_GX);
                            Debug.Print(" X ALL Global disp  =  " + (j) + " , " + Disp_All_Nodes_Global[j]);

                        }
                        if ((j + 1) / 3d == Inclied_Supp_Node_num)
                        {
                            Disp_All_Nodes_Global[j] = dispINC_G[0] * Math.Sin(Math.PI / 180 * Inc_Supp_angle_base_AND_GX);
                            Debug.Print(" Y ALL Global disp  =  " + (j) + " , " + Disp_All_Nodes_Global[j]);

                        }

                    }

                    for (j = 1; j <= totalnode * 3; j++)
                    {
                        Debug.Print(" ALL Global disp  =  " + (j) + " , " + Disp_All_Nodes_Global[j]);
                    }






                    double[] Reaction = Multiply_Matrix_2DBY1D_0_0(K_BC_LeftBottom_Row_LeftBottomCol_Ksf, XX);





                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {
                        Debug.Print(DOF_Restrained[j] + "= DOF_Restrained , Reaction,  Equivalent nodal force, (Reaction - Equivalent nodal force) = " + Reaction[j] + " , " + ForceEnd_Global_ALL_DOF[DOF_Restrained[j]] + " , " + (Reaction[j] - ForceEnd_Global_ALL_DOF[DOF_Restrained[j]]));

                    }
                    //============================================


                    //=======================================================
                    double[] SupportReaction1 = new double[totalnode * 3 + 1];
                    Support_Reaction = new double[totalnode * 3 + 1];
                    double[] SupportReaction2 = new double[totalnode * 3 + 1];
                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {
                        SupportReaction1[j] = Reaction[j] - ForceEnd_Global_ALL_DOF[DOF_Restrained[j]];


                        int u = 0;

                        Debug.Print(j + " , " + DOF_Restrained[j] + "= DOF_Restrained , Reaction,  Equivalent nodal force, (Reaction - Equivalent nodal force) = " + Reaction[j] + " , " + ForceEnd_Global_ALL_DOF[DOF_Restrained[j]] + " , " + SupportReaction1[j]);

                        u = Find_Node_Num_From_DOF_Num_int(DOF_Restrained[j]);
                        if (Find_Has_INCLINED_Support_ANY_Mem(u) == true)
                        {
                            Inc_Supp_Reaction = (Reaction[j] - ForceEnd_Global_ALL_DOF[DOF_Restrained[j]]);
                            Debug.Print(j + " , " + DOF_Restrained[j] + "=j, DOF_Restrained , u,  Inc_Supp_Reaction " + u + " , " + Inc_Supp_Reaction);
                        }
                    }


                    for (j = 0; j < DOF_Restrained.Length; j++)
                    {
                        SupportReaction2[DOF_Restrained[j]] = SupportReaction1[j];
                        Support_Reaction[DOF_Restrained[j]] = SupportReaction2[DOF_Restrained[j]];

                        Debug.Print(j + "," + DOF_Restrained[j] + "  SupportReaction  Reaction =" + SupportReaction2[DOF_Restrained[j]] + "," + Support_Reaction[DOF_Restrained[j]]);
                    }






                    //==========================================================

                    for (k = 1; k <= totalmember; k++)
                    {

                        Disp_j0_global[0] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 2];
                        Disp_j0_global[1] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 1];
                        Disp_j0_global[2] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3];
                        Disp_j0_global[3] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 2];
                        Disp_j0_global[4] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 1];
                        Disp_j0_global[5] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3];


                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 2] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 2];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 1] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3 - 1];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3] = Disp_All_Nodes_Global[Connectivity[k, 1] * 3];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 2] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 2];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 1] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3 - 1];
                        Disp_All_Nodes_GLOBAL_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3] = Disp_All_Nodes_Global[Connectivity[k, 2] * 3];







                        t_mem2 = Trasform_mat_G_TO_L_ij_00(cosx(k), sinx(k));

                        Disp_j0_Local = Multiply_Matrix_2DBY1D_0_0(t_mem2, Disp_j0_global);


                        Disp_All_Nodes_Local[Connectivity[k, 1] * 3 - 2] = Disp_j0_Local[0];
                        Disp_All_Nodes_Local[Connectivity[k, 1] * 3 - 1] = Disp_j0_Local[1];
                        Disp_All_Nodes_Local[Connectivity[k, 1] * 3] = Disp_j0_Local[2];
                        Disp_All_Nodes_Local[Connectivity[k, 2] * 3 - 2] = Disp_j0_Local[3];
                        Disp_All_Nodes_Local[Connectivity[k, 2] * 3 - 1] = Disp_j0_Local[4];
                        Disp_All_Nodes_Local[Connectivity[k, 2] * 3] = Disp_j0_Local[5];


                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 2] = Disp_j0_Local[0];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3 - 1] = Disp_j0_Local[1];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 1] * 3] = Disp_j0_Local[2];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 2] = Disp_j0_Local[3];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3 - 1] = Disp_j0_Local[4];
                        Disp_All_Nodes_Local_MembComa_NodeNum_ij_0_0[k, Connectivity[k, 2] * 3] = Disp_j0_Local[5];




                        t_mem1 = K_Local_Element_Matrix_i_0_j_0(L_n(k), Ayz[k], Iz[k], Ey[k]);
                        double[] Force_FROM_Local_Displacement = Multiply_Matrix_2DBY1D_0_0(t_mem1, Disp_j0_Local);
                        Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 2] = Force_FROM_Local_Displacement[0] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 2];
                        Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 1] = Force_FROM_Local_Displacement[1] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3 - 1];
                        Result_Mem_Force_Local[k, Connectivity[k, 1] * 3] = Force_FROM_Local_Displacement[2] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 1] * 3];
                        Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 2] = Force_FROM_Local_Displacement[3] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 2];
                        Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 1] = Force_FROM_Local_Displacement[4] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3 - 1];
                        Result_Mem_Force_Local[k, Connectivity[k, 2] * 3] = Force_FROM_Local_Displacement[5] - PjLOCAL_memEnd_MebCommaConnectivity[k, Connectivity[k, 2] * 3];

                        double[] Force_Local = new double[6];

                        Force_Local[0] = Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 2];
                        Force_Local[1] = Result_Mem_Force_Local[k, Connectivity[k, 1] * 3 - 1];
                        Force_Local[2] = Result_Mem_Force_Local[k, Connectivity[k, 1] * 3];
                        Force_Local[3] = Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 2];
                        Force_Local[4] = Result_Mem_Force_Local[k, Connectivity[k, 2] * 3 - 1];
                        Force_Local[5] = Result_Mem_Force_Local[k, Connectivity[k, 2] * 3];


                        t_mem2 = Trasform_mat_TT_L_TO_G_ij_00(cosx(k), sinx(k));

                        double[] Force_Global = Multiply_Matrix_2DBY1D_0_0(t_mem2, Force_Local);
                        Result_Mem_Force_global[k, Connectivity[k, 1] * 3 - 2] = Force_Global[0];
                        Result_Mem_Force_global[k, Connectivity[k, 1] * 3 - 1] = Force_Global[1];
                        Result_Mem_Force_global[k, Connectivity[k, 1] * 3] = Force_Global[2];
                        Result_Mem_Force_global[k, Connectivity[k, 2] * 3 - 2] = Force_Global[3];
                        Result_Mem_Force_global[k, Connectivity[k, 2] * 3 - 1] = Force_Global[4];
                        Result_Mem_Force_global[k, Connectivity[k, 2] * 3] = Force_Global[5];



                    }


                    //Debug.Print(" Result_Mem_Force_global  :" + Environment.NewLine + MakeDisplayable(Result_Mem_Force_global));

                    //Debug.Print(" Result_Mem_Force Local  :" + Environment.NewLine + MakeDisplayable(Result_Mem_Force_Local));





                }

                catch (Exception ex)
                {

                    Debug.Print("AFTER End of assignment of local and global equivalent forces to nodal forces TO singularity function Last   " + Environment.NewLine + ex.ToString());
                }




            }

            StringBuilder sbtemp33 = new StringBuilder();
            sbtemp33.Append(stringBuilder33.ToString());
            //============================================================================


            //======================================================





            Remove1_SbTraceListener();
            //=======================================
            Start2_SbTraceListener();
            //===================================



            Debug.Print(PRINT_1D_ARRAY_GLOBAL_REACTION_X_Y(Support_Reaction));
            Debug.Print("Disp_All_Nodes_Global  =  " + PRINT_1D_ARRAY_ALL_GLOBAL_DISP_X_Y_Z(Disp_All_Nodes_Global));
            Debug.Print(" Result_Mem_Force Local  :" + Environment.NewLine + PRINT_2D_ARRAY_ALL_MemID_CONN_LOCAL_AF_SHEAR_MZ(Result_Mem_Force_Local));

            StringBuilder sbtemp22 = new StringBuilder();
            sbtemp22.Append(stringBuilder22.ToString());
            Remove1_SbTraceListener();
            //============================================================================


            //===========================================================================
            stringBuilderALL = new StringBuilder();
            stringBuilderALL.Append(sbtemp11.ToString());
            stringBuilderALL.Append(sbtemp22.ToString());
            stringBuilderALL.Append(sbtemp33.ToString());

            //===========================================================================





        }//// private void Run_Analysis_Inclided_Supp()





        private void button5_Click(object sender, EventArgs e)
        {


            if (drawln == true)
            {
                Read_Input_Data_Entered();
            }
            else
                if (!(string_Read == null || string_Read.Length == 0))
                {
                    READ_From_Given_Array_Of_Line_String_HINGE_L_Fact(string_Read);

                    READ_From_Given_Array_Of_Line_String(string_Read);

                }

            ZOOMPlus_bool = false;

            Draw_Model = true;
            View_Shear = false;
            View_Mom = false;
            View_Theta = false;
            View_Delta = false;
            View_AxialForce = false;


            bool b1 = bool_Mem_Load_Pload_from_Read == false;
            bool b2 = bool_Mem_Load_Pload_from_Input == true;

            bool b3 = bool_Node_Load_Joint_from_Read == false;
            bool b4 = bool_Node_Load_Joint_from_Input == true;

            bool b5 = bool_SUPPORT_from_Read == false;
            bool b6 = bool_SUPPORT_from_Input == true;

            bool b7 = bool_Hinge_from_Read == false;
            bool b8 = bool_Hinge_from_Input == true;
            if (b1 & b2 & b3 & b4 & b5 & b6 & b7 & b8)
            {
                Read_Input_Data_Entered();
            }













            if (!(string_Read == null || string_Read.Length == 0))
            {
                READ_From_Given_Array_Of_Line_String_HINGE_L_Fact(string_Read);

                READ_From_Given_Array_Of_Line_String(string_Read);
            }


            bool_draw_DOF = true;
            ZOOMPlus_bool = true;
            PictureScale = 1.0F;

            selpt = false;
            selln = false;
            drawln = false;
            Draw_Model = false;


            PictureBox1.Invalidate();


        }





        private void button8_Click()
        {
            try
            {
                Array.Resize(ref Support_Displacement_S, totalnode + 1);
                if (mLines.Count == 0)
                {
                    MessageBox.Show("No Member Found !!!");
                    return;
                }



                string DestinationFile = pj_folder_path + "";
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.InitialDirectory = pj_folder_path + "";


                saveFileDialog1.Filter = "Text Files|*.txt";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.OverwritePrompt = true;
                DialogResult dlgResult = saveFileDialog1.ShowDialog();
                if (dlgResult == DialogResult.OK)
                {

                    DestinationFile = saveFileDialog1.FileName;


                }
                else
                {


                    MessageBox.Show("You have to type a name for the file and click SAVE button !");
                    return;
                }


                System.Text.StringBuilder result = new System.Text.StringBuilder();

                double xx1 = 0;
                double xx2 = 0;
                double yy1 = 0;
                double yy2 = 0;



                StreamWriter FileWriter = null;
                if (!string.IsNullOrEmpty(DestinationFile))
                {

                    FileWriter = new StreamWriter(DestinationFile, false);

                    if (mLines.Count > 0)
                    {
                        for (int i = 0; i < mLines.Count; i++)
                        {
                            xx1 = mLines[i].StartPoint.X / GridX;
                            yy1 = mLines[i].StartPoint.Y / GridX;
                            xx2 = mLines[i].EndPoint.X / GridX;
                            yy2 = mLines[i].EndPoint.Y / GridX;

                            yy1 = 16 - yy1;
                            yy2 = 16 - yy2;


                            Debug.Print("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);

                            result.Append("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                            result.AppendLine();
                        }
                    }


                    string[] st99 = null;
                    string stxx = null;
                    stxx = result.ToString();

                    st99 = stxx.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < st99.Length; i++)
                    {
                        st99[i] = st99[i].Replace(" ", "");


                    }
                    FIND_Left_Right_Bottom_Top(st99);



                    if (mLines.Count > 0)
                    {
                        for (int i = 0; i < mLines.Count; i++)
                        {
                            xx1 = mLines[i].StartPoint.X / GridX - left;
                            yy1 = mLines[i].StartPoint.Y / GridX;
                            xx2 = mLines[i].EndPoint.X / GridX - left;
                            yy2 = mLines[i].EndPoint.Y / GridX;

                            yy1 = 16 - yy1 - bottom;
                            yy2 = 16 - yy2 - bottom;

                            FileWriter.WriteLine("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                            Debug.Print(left + "," + bottom + "," + "L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);


                        }
                    }



                    if (bool_Hinge_from_Read == false & bool_Hinge_from_Input == true)
                        for (int j = 1; j <= totalnode_From_Lines; j++)
                        {
                            if (!string.IsNullOrEmpty(HINGE_String2[j]))
                            {
                                string[] test = new string[] { };
                                test = HINGE_String2[j].Split(new char[] { ',' });
                                if (test[1] != "0" | test[2] != "0")
                                {
                                    FileWriter.WriteLine("H" + HINGE_String2[j]);
                                    Debug.Print("H HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH" + HINGE_String2[j]);
                                }
                            }
                        }

                    if (bool_Hinge_from_Read == true & bool_Hinge_from_Input == false)
                        for (int j = 1; j <= totalnode_From_Lines; j++)
                        {
                            if (!string.IsNullOrEmpty(HINGE_String[j]))
                            {
                                string[] test = new string[] { };
                                test = HINGE_String[j].Split(new char[] { ',' });
                                if (test[1] != "0" | test[2] != "0")
                                {
                                    FileWriter.WriteLine("H" + HINGE_String[j]);
                                    Debug.Print("H" + HINGE_String[j]);
                                }
                            }
                        }

                    FileWriter.WriteLine("F" + lenght_factor);
                    Debug.Print("FFFFFFFFFFFFFFF lenght_factor " + lenght_factor);

                    if (bool_SUPPORT_from_Read == false & bool_SUPPORT_from_Input == true)
                    {
                        for (int j = 1; j <= totalnode_From_Lines; j++)
                        {
                            if (!string.IsNullOrEmpty(Sup_Node_Number_Type2[j]))
                            {
                                string[] test = new string[] { };
                                test = Sup_Node_Number_Type2[j].Split(new char[] { ',' });
                                if (test[1] != "0")
                                {
                                    FileWriter.WriteLine("S" + Sup_Node_Number_Type2[j]);
                                    Debug.Print("S" + Sup_Node_Number_Type2[j]);
                                }
                            }
                        }
                    }
                    if (bool_SUPPORT_from_Read == true & bool_SUPPORT_from_Input == false)
                    {
                        for (int j = 1; j <= totalnode_From_Lines; j++)
                        {
                            if (!string.IsNullOrEmpty(Sup_Node_Number_Type[j]))
                            {



                                {
                                    FileWriter.WriteLine("S" + Sup_Node_Number_Type[j]);
                                    Debug.Print("S" + Sup_Node_Number_Type[j]);
                                }
                            }
                        }
                    }





                    for (int j = 1; j <= totalnode_From_Lines; j++)
                    {
                        if (!string.IsNullOrEmpty(Support_Displacement_S[j]))
                        {
                            string[] test = new string[] { };
                            test = Support_Displacement_S[j].Split(new char[] { ',' });
                            if (test[1] != "0" | test[2] != "0")
                            {
                                FileWriter.WriteLine("D" + Support_Displacement_S[j]);
                                Debug.Print("D" + Support_Displacement_S[j]);
                            }
                        }
                    }





                    if (bool_Mem_Load_Pload_from_Read == true)
                    {
                        for (int k = 1; k <= totalmember; k++)
                        {
                            if (!string.IsNullOrEmpty(Pload[k]))
                            {

                                FileWriter.WriteLine("P" + Pload[k]);
                                Debug.Print("P" + Pload[k]);
                            }
                        }
                    }

                    if (bool_Mem_Load_Pload_from_Input == true)
                    {
                        for (int k = 1; k <= totalmember; k++)
                        {

                            if (!Find_All_Zero_Ploady_Sring(Pload2[k]))
                            {
                                FileWriter.WriteLine("P" + Pload2[k]);
                                Debug.Print("P" + Pload2[k]);
                            }
                        }
                    }





                    if (bool_Node_Load_Joint_from_Read == true)
                    {
                        for (int k = 1; k <= totalnode_From_Lines; k++)
                        {

                            if (!string.IsNullOrEmpty(PJoint[k]))
                            {
                                FileWriter.WriteLine("J" + PJoint[k]);
                                Debug.Print("J" + PJoint[k]);
                            }
                        }
                    }
                    if (bool_Node_Load_Joint_from_Input == true)
                    {
                        for (int k = 1; k <= totalnode_From_Lines; k++)
                        {

                            if (!string.IsNullOrEmpty(PJoint2[k]))
                            {
                                string[] test = new string[] { };
                                test = PJoint2[k].Split(new char[] { ',' });
                                if (test[1] != "0" | test[2] != "0" | test[2] != "0")
                                {
                                    FileWriter.WriteLine("J" + PJoint2[k]);
                                    Debug.Print("J" + PJoint2[k]);
                                }
                            }
                        }
                    }

















                    for (int k = 1; k <= totalmember; k++)
                    {

                        if (!string.IsNullOrEmpty(Mproperty_A_I_E[k]))
                        {
                            FileWriter.WriteLine("M" + Mproperty_A_I_E[k]);
                            Debug.Print("M" + Mproperty_A_I_E[k]);
                        }

                    }




                    FileWriter.Close();

                }

            }

            catch (Exception ex)
            {
                Debug.Print("b8 write ex " + ex.ToString());
            }

        }




        private string read_Example(int n)
        {


            string[] stx1 = new string[500];
            string stx2 = null;





            stx1[0] =
@"L1,0,0,8,6
F1
S1,1
S2,1
P1,-800,1,-600,2,0,0,-200,-100,4,8,300,3,GY
M1,0.0225,4.21875E-05,2e+08";




            stx1[1] =

                  @"L1,0,0,4,0
L2,4,0,7,0
L3,7,0,11,0
L4,11,0,14,0
F1
S1,2
S2,2
S3,2
S4,2
S5,2
P1,-100,1,0,0,0,0,0,0,0,0,0,0,LY
P2,0,0,0,0,0,0,-40,-40,0,3,0,0,LY
P3,0,0,0,0,0,0,0,0,0,0,-50,1,0
P4,-200,2,0,0,0,0,0,0,0,0,0,0,LY
M1,0.0225,4.21875E-05,2e+08
M2,0.0225,4.21875E-05,2e+08
M3,0.0225,4.21875E-05,2e+08
M4,0.0225,4.21875E-05,2e+08";




            stx1[2] =

      @"L1,0,0,0,6
L2,0,6,6,6
L3,6,6,6,0
F1
S1,2
S4,2
P2,-800,1,0,0,0,0,0,0,0,0,0,0,LY
M1,0.0225,4.21875E-05,200000000
M2,0.0225,4.21875E-05,200000000
M3,0.0225,4.21875E-05,200000000";

            stx1[3] =

     @"L1,0,0,0,4
L2,0,4,4,4
L3,4,4,4,0
L4,0,4,0,8
L5,0,8,4,8
L6,4,8,4,4
L7,4,8,7,8
F1
S1,2
S4,2
P2,0,0,0,0,0,0,-200,-100,0,4,0,0,LY
P5,-800,1,0,0,0,0,0,0,0,0,0,0,LY
J3,0,0,0
J5,500,0,0
J7,0,-100,0
M1,0.0225,4.21875E-05,200000000
M2,0.0225,4.21875E-05,200000000
M3,0.0225,4.21875E-05,200000000
M4,0.0225,4.21875E-05,200000000
M5,0.0225,4.21875E-05,200000000
M6,0.0225,4.21875E-05,200000000
M7,0.0225,4.21875E-05,200000000";




            stx1[4] = @"L1,0,0,0,6
L2,0,6,8,8
L3,8,8,16,6
L4,16,6,16,0
F1
S1,2
S5,2
P2,0,0,0,0,0,0,-10,-10,0,8.2462,0,0,LY
P3,0,0,0,0,0,0,-10,-10,0,8.2462,0,0,LY
M1,0.0225,4.21875E-05,2e+08
M2,0.0225,4.21875E-05,2e+08
M3,0.0225,4.21875E-05,2e+08
M4,0.0225,4.21875E-05,2e+08";

            stx1[5] = @"L1,0,0,0,6
L2,0,6,8,8
L3,8,8,16,6
L4,16,6,16,0
F1
S1,2
S5,2
P2,0,0,0,0,0,0,-10,-10,0,8.2462,0,0,GY
P3,0,0,0,0,0,0,-10,-10,0,8.2462,0,0,GY
M1,0.0225,4.21875E-05,2e+08
M2,0.0225,4.21875E-05,2e+08
M3,0.0225,4.21875E-05,2e+08
M4,0.0225,4.21875E-05,2e+08";

            stx1[6] =

    @"L1,0,0,0,3
L2,0,3,4,3
L3,4,3,4,0
L4,0,3,0,6
L5,0,6,4,6
L6,4,6,4,3
L7,0,6,0,10
L8,0,10,4,10
L9,4,10,4,6
L10,0,10,0,13
L11,0,13,4,13
L12,4,13,4,10
F1
S1,2
S4,3
J2,100,0,0
J5,150,0,0
J7,200,0,0
J9,250,0,0
M1,0.0225,4.21875E-05,2e+08
M2,0.0225,4.21875E-05,2e+08
M3,0.0225,4.21875E-05,2e+08
M4,0.0225,4.21875E-05,2e+08
M5,0.0225,4.21875E-05,2e+08
M6,0.0225,4.21875E-05,2e+08
M7,0.0225,4.21875E-05,2e+08
M8,0.0225,4.21875E-05,2e+08
M9,0.0225,4.21875E-05,2e+08
M10,0.0225,4.21875E-05,2e+08
M11,0.0225,4.21875E-05,2e+08
M12,0.0225,4.21875E-05,2e+08";



            stx1[7] =

                @"L1,0,0,0,2
L2,0,2,3,2
L3,3,2,3,0
L4,0,2,0,4
L5,0,4,3,4
L6,3,4,3,2
L7,3,4,6,4
L8,6,4,6,2
L9,6,2,3,2
L10,6,2,6,0
L11,6,4,9,4
L12,9,4,9,2
L13,9,2,6,2
L14,9,2,9,0
F1
S1,2
S4,2
S9,2
S12,2
P7,-800,1,0,0,0,0,0,0,0,0,0,0,LY
J2,40,0,0
J5,80,0,0
M1,0.0225,4.21875E-05,200000000
M2,0.0225,4.21875E-05,200000000
M3,0.0225,4.21875E-05,200000000
M4,0.0225,4.21875E-05,200000000
M5,0.0225,4.21875E-05,200000000
M6,0.0225,4.21875E-05,200000000
M7,0.0225,4.21875E-05,200000000
M8,0.0225,4.21875E-05,200000000
M9,0.0225,4.21875E-05,200000000
M10,0.0225,4.21875E-05,200000000
M11,0.0225,4.21875E-05,200000000
M12,0.0225,4.21875E-05,200000000
M13,0.0225,4.21875E-05,200000000
M14,0.0225,4.21875E-05,200000000";



            ////4 portal ly
            ////5 portal gy
            ////6 multi story
            ////7 multi bay

            //===============================

            stx1[8] = @"L1,0,0,4,0
L2,4,0,8,0
F1
S1,2
S2,6,200
S3,3
J2,0,-12,0
M1,0.049,0.0002,70000000
M2,0.049,0.0002,70000000";



            stx1[9] = @"L1,0,0,0,6
L2,0,6,6,6
L3,6,6,6,0
F1
S1,2
S4,6,900
P2,-800,1,0,0,0,0,0,0,0,0,0,0,LY
M1,0.0225,4.21875E-05,200000000
M2,0.0225,4.21875E-05,200000000
M3,0.0225,4.21875E-05,200000000";

            ////8 spring 1
            ////9 spring 2
            //============================================
            stx1[10] = @"L1,0,0,4,3
F1
S1,2
S2,7,36.86,1
P1,-15,2.5,0,0,0,0,0,0,0,0,0,0,GY
M1,0.0225,4.21875E-05,200000000";


            stx1[11] = @"L1,0,3,4,3
L2,4,3,6,0
F1
S1,1
S3,7,20,2
P1,0,0,0,0,0,0,-20,-20,0,4,0,0,LY
J2,-50,0,-80
M1,60E-03,4E-04,200000000
M2,60E-03,4E-04,200000000";

            stx1[12] = @"L1,0,0,0,6
L2,0,6,6,6
L3,6,6,6,0
L4,0,6,6,0
F1
S1,2
S4,7,36.86
P2,-800,1,0,0,0,0,0,0,0,0,0,0,LY
M1,0.0225,4.21875E-05,2e+08
M2,0.0225,4.21875E-05,2e+08
M3,0.0225,4.21875E-05,2e+08
M4,0.0225,4.21875E-05,2e+08";

            ////10 inc 1
            ////11 inc 2
            ////12 inc 3
            //==================================
            stx1[13] = @"L1,8,8,8,14
L2,8,14,14,14
L3,14,14,14,8
H3,2,2
F1
S1,2
S4,2
P2,-800,1,0,0,0,0,0,0,0,0,0,0,LY
M1,0.0225,4.21875E-05,200000000
M2,0.0225,4.21875E-05,200000000
M3,0.0225,4.21875E-05,200000000";

            stx1[14] = @"L1,0,0,0,5
L2,0,5,3,5
L3,3,5,6,5
L4,6,5,6,0
L5,0,5,0,10
L6,0,10,6,10
L7,6,10,6,5
H3,2,2
H6,6,1
F2
S1,1
S5,1
P2,0,0,0,0,0,0,-37.5,-37.5,0,6,0,0,LY
P3,0,0,0,0,0,0,-37.5,-37.5,0,6,0,0,LY
P6,0,0,0,0,0,0,-18.75,-18.75,0,12,0,0,LY
M1,0.01097,0.0001977,200000000
M2,0.01097,0.0001977,200000000
M3,0.01097,0.0001977,200000000
M4,0.01097,0.0001977,200000000
M5,0.01097,0.0001977,200000000
M6,0.01097,0.0001977,200000000
M7,0.01097,0.0001977,200000000";


            stx1[15] = @"L1,0,0,0,6
L2,0,6,6,6
L3,6,6,6,0
F1
S1,2
S4,2
D1,0.1,-0.15
P2,-800,1,0,0,0,0,0,0,0,0,0,0,LY
M1,0.0225,4.21875E-05,2e+08
M2,0.0225,4.21875E-05,2e+08
M3,0.0225,4.21875E-05,2e+08";

            ////15 sup disp

            stx1[16] = @"L1,0,0,0,6
L2,0,6,6,6
L3,6,6,6,0
H3,2,2
F1
S1,2
S4,2
D1,0.3,-0.3
P2,-800,1,0,0,0,0,0,0,0,0,0,0,LY
M1,0.0225,4.21875E-05,2e+08
M2,0.0225,4.21875E-05,2e+08
M3,0.0225,4.21875E-05,2e+08";
            ////16 sup disp HINGE

            //===========================================
            stx1[17] = @"L1,0,0,0,6
L2,0,6,6,6
L3,6,6,6,0
F1
S1,2
S4,5
P2,-800,1,0,0,0,0,0,0,0,0,0,0,LY
M1,0.0225,4.21875E-05,200000000
M2,0.0225,4.21875E-05,200000000
M3,0.0225,4.21875E-05,200000000";


            //16 guided

            //==================================











            stx2 = stx1[n];

            return stx2;




        }

        private void read_Example_DRAW(int n)
        {
            selpt = false;
            selln = false;
            drawln = false;

            bool_Mem_Load_Pload_from_Read = true;
            bool_Mem_Load_Pload_from_Input = false;

            bool_Node_Load_Joint_from_Read = true;
            bool_Node_Load_Joint_from_Input = false;

            bool_SUPPORT_from_Read = true;
            bool_SUPPORT_from_Input = false;

            bool_Hinge_from_Read = true;
            bool_Hinge_from_Input = false;


            string[] starr9 = null;




            string St_Read_From_Project9 = null;

            double ii = Math.Pow(0.15, 4) / 12;







            try
            {

                St_Read_From_Project9 = read_Example(n);

                starr9 = St_Read_From_Project9.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                string_Read = new string[starr9.Length];
                for (int i = 0; i < starr9.Length; i++)
                {
                    starr9[i] = starr9[i].Replace(" ", "");
                    string_Read[i] = starr9[i];

                }



                READ_From_Given_Array_Of_Line_String_HINGE_L_Fact(string_Read);

                READ_From_Given_Array_Of_Line_String(string_Read);



                Draw_Model = true;


                View_Shear = false;
                View_Mom = false;
                View_Theta = false;
                View_Delta = false;
                View_AxialForce = false;

                button16.Visible = false;
                button17.Visible = false;
                button13.Visible = false;
                drawAddBeamToolStripMenuItem.Enabled = false;

                button16.Visible = false;
                deleteBeamToolStripMenuItem.Enabled = false;
                button17.Visible = false;
                undoDeleteBeamToolStripMenuItem.Enabled = false;

            }
            catch (Exception ex)
            {
                Debug.Print("read_Example_DRAW(int n) " + ex.ToString());
            }

            PictureBox1.Invalidate();



        }



        private void button9_Click(object sender, EventArgs e)
        {

            toolStripComboBox1.DroppedDown = true;


        }








        private void button11_Click(object sender, EventArgs e)
        {
            ZoomPlus.Enabled = false;
            ZOOM_Minus.Enabled = false;
            ZOOMPlus_bool = false;
            //===============================
            
            MessageBox.Show("Select a Member by clicking on it !!!");
            selpt = false;
            selln = true;
            drawln = false;


            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;



            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }


            bool_FShowTable = true;



        }





        private void READ_CONVERT()
        {
            try
            {

                int i, j, k;
                List<line> mLinesW = new List<line>();
                int ID = 0;
                string[] Each_Line_Without_Alphabet = null;

                string[] parts_split_comma = new string[] { };

                string St_Read_From_Project9 = null;
                string[] starr9 = null;




                string selecetedFile = "";
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.InitialDirectory = pj_folder_path + "\\0 prev all example y=-y\\";





                Debug.Print(" file path = " + openDialog.InitialDirectory);

                openDialog.Filter = "Text Files|*.txt";
                DialogResult dlgResult = openDialog.ShowDialog();
                if (dlgResult == DialogResult.OK)
                {

                    selecetedFile = openDialog.FileName;

                    St_Read_From_Project9 = Read_Textfile_AS_String_from_FilePath(selecetedFile);

                }
                else
                {
                    MessageBox.Show("You did not select a file !");

                    return;

                }
                Debug.Print("selecetedFile = " + selecetedFile);

                string[] s = (openDialog.FileName.ToString()).Split('\\');
                int count = s.Length;


                string Filename_openDialog = System.IO.Path.GetFileName(openDialog.FileName);



                starr9 = St_Read_From_Project9.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                for (i = 0; i < starr9.Length; i++)
                {


                    starr9[i] = starr9[i].Replace(" ", "");
                }







                Debug.Print("END READ_CONVERT----READ ALL LINES DONE!!!!!!!!>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");




                FIND_Left_Right_Bottom_Top_OLD(starr9);
                Debug.Print("CONVERT WWWWW + left  + bottom (from upside, old X,Y) +  top(from upside, old X,Y) =" + right + "," + left + "," + bottom + "," + top);










                mLinesW.Clear();
                i = 0;
                j = 0;
                for (i = 0; i < starr9.Length; i++)
                {
                    if (starr9[i].Substring(0, 1) == "L")
                    {
                        Array.Resize(ref Each_Line_Without_Alphabet, j + 1);
                        Each_Line_Without_Alphabet[j] = starr9[i].Replace("L", "");

                        j = j + 1;
                    }
                }



                j = 0;

                ID = 0;
                for (i = 0; i < Each_Line_Without_Alphabet.Length; i++)
                {


                    parts_split_comma = Each_Line_Without_Alphabet[i].Split(new char[] { ',' });
                    for (k = 0; k < parts_split_comma.Length; k++)
                    {

                    }

                    ID = int.Parse(parts_split_comma[0]);


                    line temp11 = new line();


                    temp11.StartPoint.X = (float)(Convert.ToDouble(parts_split_comma[1]) * GridX);
                    temp11.StartPoint.Y = (float)(Convert.ToDouble(parts_split_comma[2]) * GridX);
                    temp11.EndPoint.X = (float)(Convert.ToDouble(parts_split_comma[3]) * GridX);
                    temp11.EndPoint.Y = (float)(Convert.ToDouble(parts_split_comma[4]) * GridX);
                    mLinesW.Add(temp11);
                    temp11 = null;


                    Debug.Print("mLinesW[j]= " + mLinesW[j].StartPoint.X.ToString() + " , " + mLinesW[j].StartPoint.Y.ToString() + " , " + mLinesW[j].EndPoint.X.ToString() + " , " + mLinesW[j].EndPoint.Y.ToString());

                }






                string DestinationFile = pj_folder_path + "";
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.InitialDirectory = pj_folder_path + "";
                saveFileDialog1.InitialDirectory = pj_folder_path + "\\0 New coordinate\\";


                saveFileDialog1.Filter = "Text Files|*.txt";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.OverwritePrompt = true;
                saveFileDialog1.FileName = Filename_openDialog;
                DialogResult dlgResult1 = saveFileDialog1.ShowDialog();
                if (dlgResult1 == DialogResult.OK)
                {

                    DestinationFile = saveFileDialog1.FileName;


                }
                else
                {


                    MessageBox.Show("You have to type a name for the file and click SAVE button !");
                    return;
                }



                double xx1 = 0;
                double xx2 = 0;
                double yy1 = 0;
                double yy2 = 0;


                StreamWriter FileWriter = null;
                if (!string.IsNullOrEmpty(DestinationFile))
                {

                    FileWriter = new StreamWriter(DestinationFile, false);

                    if (mLinesW.Count > 0)
                    {
                        for (i = 0; i < mLinesW.Count; i++)
                        {
                            xx1 = mLinesW[i].StartPoint.X / GridX;
                            yy1 = mLinesW[i].StartPoint.Y / GridX;
                            xx2 = mLinesW[i].EndPoint.X / GridX;
                            yy2 = mLinesW[i].EndPoint.Y / GridX;
                            yy1 = 16 - yy1 + top - 2;
                            yy2 = 16 - yy2 + top - 2;
                            FileWriter.WriteLine("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                            Debug.Print("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                        }
                    }

                    i = 0;
                    j = 0;
                    for (i = 0; i < starr9.Length; i++)
                    {
                        if (starr9[i].Substring(0, 1) == "S")
                        {
                            FileWriter.WriteLine(starr9[i]);
                            Debug.Print("SSSSSSSSSSS  " + starr9[i]);
                            j = j + 1;
                        }
                    }



                    i = 0;
                    j = 0;

                    for (i = 0; i < starr9.Length; i++)
                    {
                        if (starr9[i].Substring(0, 1) == "D")
                        {
                            FileWriter.WriteLine(starr9[i]);
                            Debug.Print("DDDDD  " + starr9[i]);
                            j = j + 1;
                        }
                    }



                    i = 0;
                    j = 0;
                    for (i = 0; i < starr9.Length; i++)
                    {
                        if (starr9[i].Substring(0, 1) == "P")
                        {
                            FileWriter.WriteLine(starr9[i]);
                            Debug.Print("PPPPPP  " + starr9[i]);
                            j = j + 1;
                        }
                    }


                    i = 0;
                    j = 0;
                    for (i = 0; i < starr9.Length; i++)
                    {
                        if (starr9[i].Substring(0, 1) == "J")
                        {
                            FileWriter.WriteLine(starr9[i]);
                            Debug.Print("JJJJJJ  " + starr9[i]);
                            j = j + 1;
                        }
                    }



                    i = 0;
                    j = 0;
                    for (i = 0; i < starr9.Length; i++)
                    {
                        if (starr9[i].Substring(0, 1) == "M")
                        {
                            FileWriter.WriteLine(starr9[i]);
                            Debug.Print("MMMMMM  " + starr9[i]);
                            j = j + 1;
                        }
                    }




                    FileWriter.Close();
                }


                mLinesW.Clear();




            }
            catch (Exception ex)
            {
                Debug.Print("READ_CONVERT ex " + ex.ToString());
            }

        }



        private void Read_Input_Data_Entered()
        {




            System.Text.StringBuilder result = new System.Text.StringBuilder();
            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
                return;
            }

            double xx1 = 0;
            double xx2 = 0;
            double yy1 = 0;
            double yy2 = 0;

            if (mLines.Count > 0)
            {
                for (int i = 0; i < mLines.Count; i++)
                {
                    xx1 = mLines[i].StartPoint.X / GridX;
                    yy1 = mLines[i].StartPoint.Y / GridX;
                    xx2 = mLines[i].EndPoint.X / GridX;
                    yy2 = mLines[i].EndPoint.Y / GridX;

                    yy1 = 16 - yy1;
                    yy2 = 16 - yy2;




                    result.Append("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                    result.AppendLine();
                    Debug.Print("L Append " + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                }
            }







            if (bool_Hinge_from_Input == true)
                for (int j = 1; j <= totalnode_From_Lines; j++)
                {
                    if (!string.IsNullOrEmpty(HINGE_String2[j]))
                    {
                        string[] test = new string[] { };
                        test = HINGE_String2[j].Split(new char[] { ',' });
                        if (test[1] != "0" | test[2] != "0")
                        {
                            result.Append("H" + HINGE_String2[j]);
                            result.AppendLine();
                            Debug.Print("HINGE_String2 " + HINGE_String2[j]);
                        }
                    }
                }


            for (int j = 1; j <= totalnode_From_Lines; j++)
            {
                if (!string.IsNullOrEmpty(Support_Displacement_S[j]))
                {
                    string[] test = new string[] { };
                    test = Support_Displacement_S[j].Split(new char[] { ',' });
                    if (test[1] != "0" | test[2] != "0")
                    {
                        result.Append("D" + Support_Displacement_S[j]);
                        Debug.Print("D" + Support_Displacement_S[j]);
                        result.AppendLine();
                    }
                }
            }




            result.Append("F" + lenght_factor);
            result.AppendLine();
            Debug.Print("FFFFFFFFFFFFFFF lenght_factor " + lenght_factor);

            if (bool_SUPPORT_from_Input == true)
            {
                for (int j = 1; j <= totalnode_From_Lines; j++)
                {
                    if (!string.IsNullOrEmpty(Sup_Node_Number_Type2[j]))
                    {
                        string[] test = new string[] { };
                        test = Sup_Node_Number_Type2[j].Split(new char[] { ',' });
                        if (test[1] != "0")
                        {

                            Debug.Print("S" + Sup_Node_Number_Type2[j]);
                            result.Append("S" + Sup_Node_Number_Type2[j]);
                            result.AppendLine();
                        }
                    }
                }
            }








            if (bool_Mem_Load_Pload_from_Input == true)
            {
                for (int k = 1; k <= totalmember; k++)
                {


                    if (!string.IsNullOrEmpty(Pload2[k]))
                    {

                        Debug.Print("P" + Pload2[k]);
                        result.Append("P" + Pload2[k]);
                        result.AppendLine();
                    }
                }
            }

            if (bool_Node_Load_Joint_from_Input == true)
            {
                for (int k = 1; k <= totalnode_From_Lines; k++)
                {

                    if (!string.IsNullOrEmpty(PJoint2[k]))
                    {

                        Debug.Print("J" + PJoint2[k]);
                        result.Append("J" + PJoint2[k]);
                        result.AppendLine();
                    }
                }
            }



            for (int k = 1; k <= totalmember; k++)
            {

                if (!string.IsNullOrEmpty(Mproperty_A_I_E[k]))
                {

                    Debug.Print(" M " + Mproperty_A_I_E[k]);
                    result.Append("M" + Mproperty_A_I_E[k]);
                    result.AppendLine();
                }

            }

            Debug.Print("Read_Input_Data_Entered QQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQ" + Environment.NewLine + result);



            bool_Mem_Load_Pload_from_Read = true;
            bool_Mem_Load_Pload_from_Input = false;

            bool_Node_Load_Joint_from_Read = true;
            bool_Node_Load_Joint_from_Input = false;

            bool_SUPPORT_from_Read = true;
            bool_SUPPORT_from_Input = false;

            bool_Hinge_from_Read = true;
            bool_Hinge_from_Input = false;

            string[] starr9 = null;




            starr9 = (result.ToString()).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string_Input = new string[starr9.Length];
            for (int i = 0; i < starr9.Length; i++)
            {
                starr9[i] = starr9[i].Replace(" ", "");
                string_Input[i] = starr9[i];

            }


            READ_From_Given_Array_Of_Line_String_HINGE_L_Fact(string_Input);

            READ_From_Given_Array_Of_Line_String(string_Input);

            Draw_Model = true;
            View_Shear = false;
            View_Mom = false;
            View_Theta = false;
            View_Delta = false;
            View_AxialForce = false;

            button16.Visible = false;
            button17.Visible = false;

            PictureBox1.Invalidate();
        }







        private void button12_Click(object sender, EventArgs e)
        {


            ZoomPlus.Enabled = false;
            ZOOM_Minus.Enabled = false;
            ZOOMPlus_bool = false;
            //===============================


            button18.Enabled = true;
            bool_EDIT = false;
            bool_button_run_clicked = true;


            bool mprop = false;
            mprop = FIND_MEM_PROPETY_NULL();
            if (mprop == true)
            {
                MessageBox.Show("MEMBER PROPERTY NOT ASSIGNED !!!!");
                return;
            }
            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
                return;
            }
            Array.Resize(ref Mproperty_A_I_E, totalmember + 1);
            Array.Resize(ref Support_Displacement_S, totalnode + 1);
            //Debug.Print("Support_Displacement_S "+ Environment.NewLine + MakeDisplayable_1D_j_0_string(Support_Displacement_S));
            //Debug.Print("  totalmember,  totalnode " + totalmember + " , " + totalnode);
            bool b1 = bool_Mem_Load_Pload_from_Read == false;
            bool b2 = bool_Mem_Load_Pload_from_Input == true;

            bool b3 = bool_Node_Load_Joint_from_Read == false;
            bool b4 = bool_Node_Load_Joint_from_Input == true;

            bool b5 = bool_SUPPORT_from_Read == false;
            bool b6 = bool_SUPPORT_from_Input == true;

            bool b7 = bool_Hinge_from_Read == false;
            bool b8 = bool_Hinge_from_Input == true;
            if (b1 & b2 & b3 & b4 & b5 & b6 & b7 & b8)
            {
                Read_Input_Data_Entered();
            }






            bool_SPRING_Supp_Ydir = false;
            bool_Inclied_Supp = false;

            SPRING_Constant = 0;
            Inclied_Supp_Node_num = 0;

            Array.Resize(ref suptype, 50 + 1);
            Array.Resize(ref Sup_Node_Number, totalnode + 1);
            for (int k = 1; k <= totalnode; k++)
            {

                if (!string.IsNullOrEmpty(Sup_Node_Number_Type[k]))
                {

                    string[] testx = Sup_Node_Number_Type[k].Split(new char[] { ',' });
                    for (int i = 0; i < testx.Length; i++)
                    {


                    }

                    Sup_Node_Number[k] = int.Parse(testx[0]);
                    suptype[k] = int.Parse(testx[1]);
                    if (suptype[k] > 0 & suptype[k] < 6 & suptype[k] == 8)
                    {
                        bool_Inclied_Supp = false;

                    }
                    try
                    {
                        if (suptype[k] == 6)
                        {
                            SPRING_Constant = double.Parse(testx[2]);

                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(k + " SPRING_Constant =:" + ex.ToString());
                    }



                    try
                    {
                        if (suptype[k] == 7)
                        {
                            Inclied_Supp_Node_num = int.Parse(testx[0]);

                            Inc_Supp_angle_base_AND_GX = double.Parse(testx[2]);


                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(k + " Inclied_Supp =:" + ex.ToString());
                    }

                }
            }

            if (SPRING_Constant > 0)
            {
                bool_SPRING_Supp_Ydir = true;
                bool_Inclied_Supp = false;
                bool_HINGE_Run_Analysis = false;
            }
            if (Inclied_Supp_Node_num > 0)
            {
                bool_Inclied_Supp = true;
                bool_SPRING_Supp_Ydir = false;
                bool_HINGE_Run_Analysis = false;
            }
            if (HINGE_Num_total > 0)
            {
                bool_HINGE_Run_Analysis = true;
                bool_Inclied_Supp = false;
                bool_SPRING_Supp_Ydir = false;
            }


            //Debug.Print(" bool_Inclied_Supp , bool_SPRING_Supp_Ydir =:" + bool_Inclied_Supp + " , " + bool_SPRING_Supp_Ydir);
            if (bool_Inclied_Supp == true & bool_SPRING_Supp_Ydir == true)
            {
                MessageBox.Show("RUN ANALYSIS is QUITED because" + Environment.NewLine
                 + "this version of software doesnot support both INCLINED support with SPRING support !!! ");
                return;
            }





            if (bool_Inclied_Supp == true & bool_SPRING_Supp_Ydir == false & bool_HINGE_Run_Analysis == false)
            {



                Run_Analysis_Inclided_Supp();
            }


            if (bool_SPRING_Supp_Ydir == true & bool_Inclied_Supp == false & bool_HINGE_Run_Analysis == false)
            {
                Run_Analysis_SPRING();
            }
            if (bool_HINGE_Run_Analysis == true & bool_Inclied_Supp == false & bool_SPRING_Supp_Ydir == false)
            {
                Run_Analysis_HINGE();
            }
            if (bool_Inclied_Supp == false & bool_SPRING_Supp_Ydir == false & bool_HINGE_Run_Analysis == false)
            {
                Run_Analysis();
            }


            if (bool_Force_Applied == true)
            {

                View_Shear = false;
                View_Mom = false;
                View_Theta = false;
                View_Delta = false;
                View_AxialForce = false;
                View_PicBox = true;


                Generate_ordinate_values_2();

                Generate_ordinate_values_3();
            }
            bool_copy_once = true;


            deflectionDiagramToolStripMenuItem.PerformClick();

            bool_draw_Disp_Diagram = true;


            Array.Resize(ref Pload2, 55 + 1);
            for (int u = 1; u <= 55; u++)
            {

                {

                    Pload2[u] = u.ToString() + ",0,0,0,0,0,0,0,0,0,0,0,0,0";
                }
            }



            for (int u = 1; u <= totalmember; u++)
            {
                if (!string.IsNullOrEmpty(Pload[u]))
                    Pload2[u] = Pload[u];

            }

            Array.Resize(ref PJoint, totalnode + 1);
            Array.Resize(ref Sup_Node_Number_Type, totalnode + 1);
            Array.Resize(ref Sup_Node_Number_Type2, totalnode + 1);
            Array.Resize(ref PJoint2, totalnode + 1);
            Array.Resize(ref Support_Displacement_S, totalnode + 1);
            Array.Resize(ref HINGE_String, totalnode + 1);

            for (int u = 1; u <= totalnode; u++)
            {
                if (!string.IsNullOrEmpty(PJoint[u]))
                    PJoint2[u] = PJoint[u];

            }




            Array.Resize(ref Sup_Node_Number_Type2, totalnode + 1);
            for (int u = 1; u <= totalnode; u++)
            {
                if (!string.IsNullOrEmpty(Sup_Node_Number_Type[u]))
                    Sup_Node_Number_Type2[u] = Sup_Node_Number_Type[u];

            }

            Array.Resize(ref HINGE_String2, totalnode + 1);
            for (int u = 1; u <= totalnode; u++)
            {
                if (!string.IsNullOrEmpty(HINGE_String[u]))
                {
                    HINGE_String2[u] = HINGE_String[u];
                }

            }






            ZoomPlus.Enabled = true;
            ZOOM_Minus.Enabled = true;
            ZOOMPlus_bool = true;







            bool_View_Force_Value = true;

            bool_Mem_Load_Pload_from_Read = false;
            bool_Mem_Load_Pload_from_Input = true;

            bool_Node_Load_Joint_from_Read = false;
            bool_Node_Load_Joint_from_Input = true;

            bool_SUPPORT_from_Read = false;
            bool_SUPPORT_from_Input = true;

            bool_Hinge_from_Read = false;
            bool_Hinge_from_Input = true;






            selpt = false;
            selln = false;

            button17.Visible = false;
            Last_index_line_selected = -1;
            nodenumer_Selected = -1;



            postAnalysisToolStripMenuItem.Enabled = true;
            viewPostAnalysisToolStripMenuItem.Enabled = true;








        }

        private void button13_Click(object sender, EventArgs e)
        {


            if (mLines.Count == 0)
            {
                contextMenuStrip2.Show(button13, new Point(0, button13.Height));
            }
            else
            {
                contextMenuStrip2.Visible = false;
                draw_beam();
            }



        }
        private void draw_beam()
        {




            button18.Enabled = false;
            ZOOMPlus_bool = false;
            PictureScale = 1.0F;

            selpt = false;
            selln = false;
            drawln = true;
            bool_EDIT = true;




            Draw_Model = true;


            bool_Mem_Load_Pload_from_Read = false;
            bool_Mem_Load_Pload_from_Input = true;

            bool_Node_Load_Joint_from_Read = false;
            bool_Node_Load_Joint_from_Input = true;

            bool_SUPPORT_from_Read = false;
            bool_SUPPORT_from_Input = true;

            bool_Hinge_from_Read = false;
            bool_Hinge_from_Input = true;

            button1.Enabled = false;



            deleteBeamToolStripMenuItem.Enabled = false;
            button17.Visible = false;
            undoDeleteBeamToolStripMenuItem.Enabled = false;


            PictureBox1.Invalidate();
        }


        private void button14_Click(object sender, EventArgs e)
        {


            selpt = false;
            selln = true;
            drawln = false;

            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;


            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }

        }

        private void button15_Click(object sender, EventArgs e)
        {


            selpt = true;
            selln = false;
            drawln = false;

            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;


            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }
        }


        private void button16_Click(object sender, EventArgs e)
        {



            selpt = false;
            selln = true;
            drawln = false;


            {


                if (mLines.Count > 0)
                {


                    tempLines.Add(mLines[mLines.Count - 1]);
                    mLines.RemoveAt(mLines.Count - 1);

                    Debug.Print(" deleted lines XXXXXXXXXXXXXX  " + (mLines.Count));
                }



                button17.Visible = false;

                if (mLines.Count > 0)
                {

                    Line_To_Node_AND_Connectivity(mLines);
                }
                if (mLines.Count <= 0)
                {

                    bool_clear_picbox = true;
                    PictureBox1.Invalidate();
                }
                else
                {
                    bool_clear_picbox = false;

                }

            }





            button17.Visible = true;
            PictureBox1.Invalidate();
            Debug.Print("DELETE QQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQ = " + (mLines.Count) + "," + (tempLines.Count));

        }

        private void button17_Click(object sender, EventArgs e)
        {



            {

                if (tempLines.Count > 0)
                {






                    {

                        mLines.Add(tempLines[tempLines.Count - 1]);

                        tempLines.RemoveAt(tempLines.Count - 1);
                        //Debug.Print("= UN DELETE  = " + mLines.Count + "," + tempLines.Count + "," + mLines[0].ToString());
                    }
                }

            }




            if (mLines.Count > 0)
            {

                Line_To_Node_AND_Connectivity(mLines);
            }

            if (mLines.Count <= 0)
            {

                bool_clear_picbox = false;
                PictureBox1.Invalidate();
            }
            else
            {
                bool_clear_picbox = false;

            }

            PictureBox1.Invalidate();

        }

        private void pointMomentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bool_EDIT == true | Last_index_line_selected > -1) MessageBox.Show("Select a Member by clicking on it !!!");
            selpt = false;
            selln = true;
            drawln = false;

            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }

            bool_FconcM = true;
            Invoke_Edit_False();
        }

        private void pOINTLOADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bool_EDIT == true | Last_index_line_selected > -1) MessageBox.Show("Select a Member by clicking on it !!!");

            selpt = false;
            selln = true;
            drawln = false;


            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;



            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }

            bool_FconcP = true;

            Invoke_Edit_False();
        }







        private void distributedLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (bool_EDIT == true | Last_index_line_selected > -1) MessageBox.Show("Select a Member by clicking on it !!!");
            selpt = false;
            selln = true;
            drawln = false;

            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;


            bool_FUDL = true;
            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }

            Invoke_Edit_False();

        }





        private void button18_Click(object sender, EventArgs e)
        {
            if (mLines.Count > 0)
            {

                MessageBox.Show("WARNING: If not saved data will be lost !!!");

                button13.Visible = true;


                if (bool_copy_once == true)
                {
                    EDIT_Copy_Old_Data_Once();
                    bool_copy_once = false;

                }
                bool_EDIT = true;

            }
            else
            {
                MessageBox.Show("No member found !!!");
                return;
            }


        }



        private void EDIT_Copy_Old_Data_Once()
        {

            bool_Mem_Load_Pload_from_Read = false;
            bool_Mem_Load_Pload_from_Input = true;

            bool_Node_Load_Joint_from_Read = false;
            bool_Node_Load_Joint_from_Input = true;

            bool_SUPPORT_from_Read = false;
            bool_SUPPORT_from_Input = true;

            bool_Hinge_from_Read = false;
            bool_Hinge_from_Input = true;

            m_Lines1 = mLines;





            Debug.Print(" EDIT_Copy_Old_Data_Once RRRRRRRRR totalmember,  totalnode,totalnode_From_Lines =  " + totalmember + " , " + totalnode + " , " + totalnode_From_Lines);
            Array.Resize(ref Pload, 55 + 1);
            Array.Resize(ref Pload2, 55 + 1);
            //if (drawln == true)
            //if (bool_EDIT == true)
            {

                for (int u = 1; u <= 55; u++)
                {

                    {


                        Pload2[u] = u.ToString() + ",0,0,0,0,0,0,0,0,0,0,0,0,0";
                    }
                }
            }



            for (int u = 1; u <= totalmember; u++)
            {
                if (!string.IsNullOrEmpty(Pload[u]))
                    Pload2[u] = Pload[u];

            }




            Array.Resize(ref Sup_Node_Number_Type2, totalnode + 1);
            Array.Resize(ref PJoint2, totalnode + 1);
            Array.Resize(ref Support_Displacement_S, totalnode + 1);
            Array.Resize(ref HINGE_String2, totalnode + 1);

            for (int u = 1; u <= totalnode; u++)
            {
                if (!string.IsNullOrEmpty(PJoint[u]))
                    PJoint2[u] = PJoint[u];

            }

            for (int u = 1; u <= totalnode; u++)
            {
                if (!string.IsNullOrEmpty(HINGE_String[u]))
                    HINGE_String2[u] = HINGE_String[u];

            }


            Array.Resize(ref Sup_Node_Number_Type2, totalnode + 1);
            for (int u = 1; u <= totalnode; u++)
            {
                if (!string.IsNullOrEmpty(Sup_Node_Number_Type[u]))
                    Sup_Node_Number_Type2[u] = Sup_Node_Number_Type[u];

            }











            Pload = new string[55 + 1];
            HINGE_String = new string[totalnode + 1];
            PJoint = new string[totalnode + 1];
            Sup_Node_Number_Type = new string[totalnode + 1];



            //Debug.Print("Pload2 :QQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQ "
            //    + Environment.NewLine + MakeDisplayable_1D_j_0_string(Pload2));
            //Debug.Print("PJoint :QQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQ"
            //    + Environment.NewLine + MakeDisplayable_1D_j_0_string(PJoint));
            //Debug.Print("PJoint2 :QQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQ"
            //    + Environment.NewLine + MakeDisplayable_1D_j_0_string(PJoint2));
            //Debug.Print("Sup_Node_Number_Type2 :QQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQ"
            //    + Environment.NewLine + MakeDisplayable_1D_j_0_string(Sup_Node_Number_Type2));

            //Debug.Print("Support_Displacement_S :RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR"
            //  + Environment.NewLine + MakeDisplayable_1D_j_0_string(Support_Displacement_S));
            //Debug.Print(" HINGE_String2 :RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR"
            //  + Environment.NewLine + MakeDisplayable_1D_j_0_string(HINGE_String2));

            ZOOMPlus_bool = false;
            PictureScale = 1.0F;





            Draw_Model = true;
            View_Shear = false;
            View_Mom = false;
            View_Theta = false;
            View_Delta = false;
            View_AxialForce = false;



            button1.Enabled = false;
            ZoomPlus.Enabled = false;
            ZOOM_Minus.Enabled = false;
            button16.Visible = false;
            deleteBeamToolStripMenuItem.Enabled = false;
            button17.Visible = false;
            undoDeleteBeamToolStripMenuItem.Enabled = false;
            PictureBox1.Invalidate();


        }



        private void drawAddBeamToolStripMenuItem_Click(object sender, EventArgs e)
        {

            selpt = false;
            selln = false;
            drawln = true;
            ZoomPlus.Enabled = false;
            ZOOM_Minus.Enabled = false;
        }

        private void deleteBeamToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                Temp_Selected_line = new line();


                Temp_Selected_line = mLines[Last_index_line_selected];
                Temp_Selected_line.EndPoint = mLines[Last_index_line_selected].EndPoint;
                if (mLines.Count > 0 & (mLines.Count - 1) == Last_index_line_selected)
                {
                    Debug.Print(" selected lines  " + mLines[Last_index_line_selected]);
                    mLines.RemoveAt(Last_index_line_selected);
                }
                else
                {
                    MessageBox.Show("FROM LAST to FIRST ORDER drawn Member has to deleted one by one till needed !!!");
                    button17.Visible = false;
                }
                if (mLines.Count > 0)
                {

                    Line_To_Node_AND_Connectivity(mLines);
                }

            }
            catch (Exception)
            {

            }

            button17.Visible = true;
            PictureBox1.Invalidate();
            Debug.Print("!!!!!after undo del mLines.Count - 1= " + (mLines.Count - 1));

        }

        private void undoDeleteBeamToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                mLines.Add(Temp_Selected_line);
                Temp_Selected_line = null;

            }
            catch (Exception)
            {

            }
            if (mLines.Count > 0)
            {

                Line_To_Node_AND_Connectivity(mLines);
            }
            button17.Visible = false;
            PictureBox1.Invalidate();
            Debug.Print("!!!!!after undo delete NNNNNNNNNNNNNNNNNN mLines.Count - 1= " + (mLines.Count - 1));

        }

        private void seletBeamToolStripMenuItem_Click(object sender, EventArgs e)
        {

            selpt = false;
            selln = true;
            drawln = false;

            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;


            Debug.Print("Last_index_line_selected= " + Last_index_line_selected);
        }

        private void selectPointToolStripMenuItem_Click(object sender, EventArgs e)
        {

            selpt = true;
            selln = false;

            drawln = false;

            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;
        }

        private void readFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            read_text_file();
        }

        private void read_text_file()
        {
            if (bool_button_run_clicked == true)
            {



            }


            bool_copy_once = true;
            selpt = false;
            selln = false;
            drawln = false;
            Draw_Model = true;


            bool_Mem_Load_Pload_from_Read = true;
            bool_Mem_Load_Pload_from_Input = false;

            bool_Node_Load_Joint_from_Read = true;
            bool_Node_Load_Joint_from_Input = false;

            bool_SUPPORT_from_Read = true;
            bool_SUPPORT_from_Input = false;

            bool_Hinge_from_Read = true;
            bool_Hinge_from_Input = false;


            string St_Read_From_Project9 = null;
            string[] starr9 = null;




            string selecetedFile = "";
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = pj_folder_path;






            openDialog.Filter = "Text Files|*.txt";
            DialogResult dlgResult = openDialog.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {

                selecetedFile = openDialog.FileName;

                St_Read_From_Project9 = Read_Textfile_AS_String_from_FilePath(selecetedFile);


                string fileName = @selecetedFile;
                string path = @pj_folder_path;


                read_fileName = System.IO.Path.GetFileName(fileName);

            }
            else
            {

                MessageBox.Show("You did not select a file !");

                return;












            }

            starr9 = St_Read_From_Project9.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string_Read = new string[starr9.Length];
            for (int i = 0; i < starr9.Length; i++)
            {
                starr9[i] = starr9[i].Replace(" ", "");
                string_Read[i] = starr9[i];

            }

            READ_From_Given_Array_Of_Line_String_HINGE_L_Fact(string_Read);

            READ_From_Given_Array_Of_Line_String(string_Read);








            View_Shear = false;
            View_Mom = false;
            View_Theta = false;
            View_Delta = false;
            View_AxialForce = false;
            button13.Visible = false;
            button16.Visible = false;
            button17.Visible = false;



        }


        private void analysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button12_Click(sender, e);
        }

        private void axialForceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View_Shear = false;
            View_Mom = false;
            View_Theta = false;
            View_Delta = false;
            View_AxialForce = true;
            PictureBox1.Refresh();
        }

        private void deflectionDiagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View_Shear = false;
            View_Mom = false;
            View_Theta = false;
            View_Delta = true;
            View_AxialForce = false;


            PictureBox1.Refresh();
        }

        private void slopeDiagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View_Shear = false;
            View_Mom = false;

            View_Delta = false;
            View_AxialForce = false;


            PictureBox1.Refresh();
        }

        private void momentDiagramToolStripMenuItem_Click(object sender, EventArgs e)
        {

            View_Shear = false;
            View_Mom = true;
            View_Theta = false;
            View_Delta = false;
            View_AxialForce = false;


            PictureBox1.Refresh();
        }

        private void viewShearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View_Shear = true;
            View_Mom = false;
            View_Theta = false;
            View_Delta = false;
            View_AxialForce = false;
            PictureBox1.Refresh();
        }

        private void nodalForceMomentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bool_EDIT == true | nodenumer_Selected > -1) MessageBox.Show("Select a Node by clicking on it !!!");
            selpt = true;
            selln = false;
            drawln = false;



            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }

            bool_FNodal = true;
            Invoke_Edit_False();
        }

        private void supportSettlementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bool_EDIT == true | nodenumer_Selected > -1) MessageBox.Show("Select a Node by clicking on it !!!");
            selpt = true;
            selln = false;
            drawln = false;



            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }

            bool_FSupDisp = true;
            Invoke_Edit_False();

        }







        private void assignDefaultPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
                return;
            }







            MessageBox.Show("Ayz=0.0225 m^2,Iz=4.21875E-05 m^4,E=200000000 kN/m^2 Assigned!!!");
            for (int k = 1; k <= totalmember; k++)
            {
                Mproperty_A_I_E[k] = k + "," + "0.0225,4.21875E-05,2e+08";

                Debug.Print("Mproperty_A_I_E[k]= " + Mproperty_A_I_E[k]);



            }
        }

        private void assignDefaultBeamPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Select a Member by clicking on it !!!");
            selpt = false;
            selln = true;
            drawln = false;


            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;


            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }
            bool_FBeamProp = true;
            Invoke_Edit_False();
        }

        private void fileSaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            save_write_file();
        }

        private void save_write_file()
        {


            try
            {
                Array.Resize(ref Support_Displacement_S, totalnode + 1);
                if (mLines.Count == 0)
                {
                    MessageBox.Show("No Member Found !!!");
                    return;
                }



                string DestinationFile = pj_folder_path + "";
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.InitialDirectory = pj_folder_path;



                saveFileDialog1.Filter = "Text Files|*.txt";


                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.OverwritePrompt = true;
                saveFileDialog1.FileName = read_fileName;
                DialogResult dlgResult = saveFileDialog1.ShowDialog();
                if (dlgResult == DialogResult.OK)
                {

                    DestinationFile = saveFileDialog1.FileName;


                }
                else
                {


                    MessageBox.Show("You have to type a name for the file and click SAVE button !");
                    return;
                }


                System.Text.StringBuilder result = new System.Text.StringBuilder();




                StreamWriter FileWriter = null;
                if (!string.IsNullOrEmpty(DestinationFile))
                {

                    FileWriter = new StreamWriter(DestinationFile, false);





                    double xx1 = 0;
                    double xx2 = 0;
                    double yy1 = 0;
                    double yy2 = 0;

                    if (mLines.Count > 0)
                    {
                        for (int i = 0; i < mLines.Count; i++)
                        {
                            xx1 = mLines[i].StartPoint.X / GridX;
                            yy1 = mLines[i].StartPoint.Y / GridX;
                            xx2 = mLines[i].EndPoint.X / GridX;
                            yy2 = mLines[i].EndPoint.Y / GridX;

                            yy1 = 16 - yy1;
                            yy2 = 16 - yy2;


                            Debug.Print("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);

                            result.Append("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                            result.AppendLine();
                        }
                    }


                    string[] st99 = null;
                    string stxx = null;
                    stxx = result.ToString();

                    st99 = stxx.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < st99.Length; i++)
                    {
                        st99[i] = st99[i].Replace(" ", "");


                    }
                    FIND_Left_Right_Bottom_Top(st99);



                    if (mLines.Count > 0)
                    {
                        for (int i = 0; i < mLines.Count; i++)
                        {
                            xx1 = mLines[i].StartPoint.X / GridX - left;
                            yy1 = mLines[i].StartPoint.Y / GridX;
                            xx2 = mLines[i].EndPoint.X / GridX - left;
                            yy2 = mLines[i].EndPoint.Y / GridX;

                            yy1 = 16 - yy1 - bottom;
                            yy2 = 16 - yy2 - bottom;

                            FileWriter.WriteLine("L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);
                            Debug.Print(left + "," + bottom + "," + "L" + (i + 1) + "," + xx1 + "," + yy1 + "," + xx2 + "," + yy2);


                        }
                    }



                    if (bool_Hinge_from_Read == false & bool_Hinge_from_Input == true)
                        for (int j = 1; j <= totalnode_From_Lines; j++)
                        {
                            if (!string.IsNullOrEmpty(HINGE_String2[j]))
                            {
                                string[] test = new string[] { };
                                test = HINGE_String2[j].Split(new char[] { ',' });
                                if (test[1] != "0" | test[2] != "0")
                                {
                                    FileWriter.WriteLine("H" + HINGE_String2[j]);
                                    Debug.Print("H HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH" + HINGE_String2[j]);
                                }
                            }
                        }

                    if (bool_Hinge_from_Read == true & bool_Hinge_from_Input == false)
                        for (int j = 1; j <= totalnode_From_Lines; j++)
                        {
                            if (!string.IsNullOrEmpty(HINGE_String[j]))
                            {
                                string[] test = new string[] { };
                                test = HINGE_String[j].Split(new char[] { ',' });
                                if (test[1] != "0" | test[2] != "0")
                                {
                                    FileWriter.WriteLine("H" + HINGE_String[j]);
                                    Debug.Print("H" + HINGE_String[j]);
                                }
                            }
                        }

                    FileWriter.WriteLine("F" + lenght_factor);
                    Debug.Print("FFFFFFFFFFFFFFF lenght_factor " + lenght_factor);

                    if (bool_SUPPORT_from_Read == false & bool_SUPPORT_from_Input == true)
                    {
                        for (int j = 1; j <= totalnode_From_Lines; j++)
                        {
                            if (!string.IsNullOrEmpty(Sup_Node_Number_Type2[j]))
                            {
                                string[] test = new string[] { };
                                test = Sup_Node_Number_Type2[j].Split(new char[] { ',' });
                                if (test[1] != "0")
                                {
                                    FileWriter.WriteLine("S" + Sup_Node_Number_Type2[j]);
                                    Debug.Print("S" + Sup_Node_Number_Type2[j]);
                                }
                            }
                        }
                    }
                    if (bool_SUPPORT_from_Read == true & bool_SUPPORT_from_Input == false)
                    {
                        for (int j = 1; j <= totalnode_From_Lines; j++)
                        {
                            if (!string.IsNullOrEmpty(Sup_Node_Number_Type[j]))
                            {



                                {
                                    FileWriter.WriteLine("S" + Sup_Node_Number_Type[j]);
                                    Debug.Print("S" + Sup_Node_Number_Type[j]);
                                }
                            }
                        }
                    }





                    for (int j = 1; j <= totalnode_From_Lines; j++)
                    {
                        if (!string.IsNullOrEmpty(Support_Displacement_S[j]))
                        {
                            string[] test = new string[] { };
                            test = Support_Displacement_S[j].Split(new char[] { ',' });
                            if (test[1] != "0" | test[2] != "0")
                            {
                                FileWriter.WriteLine("D" + Support_Displacement_S[j]);
                                Debug.Print("D" + Support_Displacement_S[j]);
                            }
                        }
                    }





                    if (bool_Mem_Load_Pload_from_Read == true)
                    {
                        for (int k = 1; k <= totalmember; k++)
                        {
                            if (!string.IsNullOrEmpty(Pload[k]))
                            {

                                FileWriter.WriteLine("P" + Pload[k]);
                                Debug.Print("P" + Pload[k]);
                            }
                        }
                    }

                    if (bool_Mem_Load_Pload_from_Input == true)
                    {
                        for (int k = 1; k <= totalmember; k++)
                        {

                            if (!Find_All_Zero_Ploady_Sring(Pload2[k]))
                            {
                                FileWriter.WriteLine("P" + Pload2[k]);
                                Debug.Print("P" + Pload2[k]);
                            }
                        }
                    }





                    if (bool_Node_Load_Joint_from_Read == true)
                    {
                        for (int k = 1; k <= totalnode_From_Lines; k++)
                        {

                            if (!string.IsNullOrEmpty(PJoint[k]))
                            {
                                FileWriter.WriteLine("J" + PJoint[k]);
                                Debug.Print("J" + PJoint[k]);
                            }
                        }
                    }
                    if (bool_Node_Load_Joint_from_Input == true)
                    {
                        for (int k = 1; k <= totalnode_From_Lines; k++)
                        {

                            if (!string.IsNullOrEmpty(PJoint2[k]))
                            {
                                string[] test = new string[] { };
                                test = PJoint2[k].Split(new char[] { ',' });
                                if (test[1] != "0" | test[2] != "0" | test[2] != "0")
                                {
                                    FileWriter.WriteLine("J" + PJoint2[k]);
                                    Debug.Print("J" + PJoint2[k]);
                                }
                            }
                        }
                    }

















                    for (int k = 1; k <= totalmember; k++)
                    {

                        if (!string.IsNullOrEmpty(Mproperty_A_I_E[k]))
                        {
                            FileWriter.WriteLine("M" + Mproperty_A_I_E[k]);
                            Debug.Print("M" + Mproperty_A_I_E[k]);
                        }

                    }




                    FileWriter.Close();

                }

            }

            catch (Exception ex)
            {
                Debug.Print(" save_write_file ex " + ex.ToString());
            }

        }





























        private void deleteAllBeamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_Lines1.Clear();
            mLines.Clear();
            totalmember = 0;
            totalnode = 0;
            PictureBox1.Invalidate();
        }

        private void modelViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drawln == true)
            {
                Read_Input_Data_Entered();
            }
            else
                if (!(string_Read == null || string_Read.Length == 0))
                {
                    READ_From_Given_Array_Of_Line_String_HINGE_L_Fact(string_Read);

                    READ_From_Given_Array_Of_Line_String(string_Read);

                }

            ZOOMPlus_bool = false;

            Draw_Model = true;
            View_Shear = false;
            View_Mom = false;
            View_Theta = false;
            View_Delta = false;
            View_AxialForce = false;



            PictureBox1.Invalidate();

        }


        private void zoomPlusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drawln == true)
            {
                Read_Input_Data_Entered();
            }
            else
                if (!(string_Read == null || string_Read.Length == 0))
                {
                    READ_From_Given_Array_Of_Line_String_HINGE_L_Fact(string_Read);

                    READ_From_Given_Array_Of_Line_String(string_Read);

                }

            ZOOMPlus_bool = false;

            Draw_Model = true;
            View_Shear = false;
            View_Mom = false;
            View_Theta = false;
            View_Delta = false;
            View_AxialForce = false;


            bool b1 = bool_Mem_Load_Pload_from_Read == false;
            bool b2 = bool_Mem_Load_Pload_from_Input == true;

            bool b3 = bool_Node_Load_Joint_from_Read == false;
            bool b4 = bool_Node_Load_Joint_from_Input == true;

            bool b5 = bool_SUPPORT_from_Read == false;
            bool b6 = bool_SUPPORT_from_Input == true;

            bool b7 = bool_Hinge_from_Read == false;
            bool b8 = bool_Hinge_from_Input == true;
            if (b1 & b2 & b3 & b4 & b5 & b6 & b7 & b8)
            {
                Read_Input_Data_Entered();
            }













            if (!(string_Read == null || string_Read.Length == 0))
            {
                READ_From_Given_Array_Of_Line_String_HINGE_L_Fact(string_Read);

                READ_From_Given_Array_Of_Line_String(string_Read);
            }


            bool_draw_DOF = true;
            ZOOMPlus_bool = true;
            PictureScale = 1.0F;

            selpt = false;
            selln = false;
            drawln = false;
            Draw_Model = false;


            PictureBox1.Invalidate();


        }


        private void ZoomPlus_Click(object sender, EventArgs e)
        {
            ZOOMPlus_bool = true;
            PictureScale = PictureScale + .2F;


            Debug.Print("PictureScale=  " + PictureScale);




            Size size = new Size(Convert.ToInt32(PictureScale * 2500), Convert.ToInt32(PictureScale * 1500));
            PictureBox1.Size = size;
            PictureBox1.Invalidate();

        }

        private void button20_Click(object sender, EventArgs e)
        {
            PointF p = new PointF(-10, -10);
            ptSelln = p;
            ptSelNode = p;
            PictureBox1.Invalidate();
        }

        private void unselectBeamNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PointF p = new PointF(-10, -10);
            ptSelln = p;
            ptSelNode = p;
            PictureBox1.Invalidate();
        }

        private void addFixedSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }
            selpt = true;
            selln = false;
            drawln = false;
            bool_Fixed_support = true;
            bool_Pinned_support = false;

            bool_SUPPORT_from_Read = false;
            bool_SUPPORT_from_Input = true;




        }
        private void addHingedSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }
            else
            {
                selpt = true;
                selln = false;
                drawln = false;
                bool_Pinned_support = true;
                bool_Fixed_support = false;
                bool_SUPPORT_from_Read = false;
                bool_SUPPORT_from_Input = true;



            }






        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            selpt = true;
            selln = false;
            drawln = false;
            bool_Fixed_support = true;
            bool_Pinned_support = false;
            bool_Delete_support = false;
            if (bool_EDIT == true | nodenumer_Selected > -1) MessageBox.Show("Select a Node by clicking on it !!!");





            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }

            bool_FDirectSupp = true;
            Invoke_Edit_False();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            button12_Click(sender, e);
            //button12_Click(object sender, EventArgs e);
            //button12.PerformClick();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            selpt = true;
            selln = false;
            drawln = false;
            bool_Fixed_support = false;
            bool_Pinned_support = false;
            bool_Delete_support = true;
            bool_Roller_Supp_Ydir = false;
            if (bool_EDIT == true | nodenumer_Selected > -1) MessageBox.Show("Select a Node by clicking on it !!!");


            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }
            bool_FDirectSupp = true;
            Invoke_Edit_False();

        }


        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            ZOOMPlus_bool = false;

            selpt = false;
            selln = false;
            drawln = true;

            Array.Resize(ref Sup_Node_Number_Type2, totalnode + 1);

            Draw_Model = true;


            bool_SUPPORT_from_Read = false;
            bool_SUPPORT_from_Input = true;

            button1.Enabled = false;
            ZoomPlus.Enabled = false;
            ZOOM_Minus.Enabled = false;
            button16.Visible = false;
            deleteBeamToolStripMenuItem.Enabled = false;
            button17.Visible = false;
            undoDeleteBeamToolStripMenuItem.Enabled = false;
            PictureBox1.Invalidate();
        }





        private void addRemoveSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (bool_EDIT == true | nodenumer_Selected > -1) MessageBox.Show("Select a Node by clicking on it !!!");

            selpt = true;
            selln = false;
            drawln = false;



            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }
            bool_FSuppData = true;

            Invoke_Edit_False();


        }

        private void supportDisplacementToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
                return;
            }
            MessageBox.Show("Select a Node with Support by clicking on it !!!");

            selpt = true;
            selln = false;
            drawln = false;

            bool_FconcP = false;
            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = true;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;

            bool_Mem_Load_Pload_from_Read = false;
            bool_Mem_Load_Pload_from_Input = true;

            bool_Node_Load_Joint_from_Read = false;
            bool_Node_Load_Joint_from_Input = true;

            bool_SUPPORT_from_Read = false;
            bool_SUPPORT_from_Input = true;

            bool_Hinge_from_Read = false;
            bool_Hinge_from_Input = true;


        }

        private void button21_Click(object sender, EventArgs e)
        {


            PictureBox PictureBox1 = new PictureBox();

            PointF p = new PointF(-10, -10);

            ptSelln = p;
            ptSelNode = p;

            PictureBox1.Invalidate();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Select a Node with Support by clicking on it !!!");

            selpt = true;
            selln = false;
            drawln = false;

            MessageBox.Show("Select a Node by clicking on it !!!");

            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }

            bool_FSupDisp = true;
            Invoke_Edit_False();



        }

        private void button22_Click(object sender, EventArgs e)
        {



















































        }

        private void ToolStripButton3_Click(object sender, EventArgs e)
        {

            selpt = true;
            selln = false;
            drawln = false;
            bool_Fixed_support = false;
            bool_Pinned_support = false;
            bool_Delete_support = true;
            if (bool_EDIT == true | nodenumer_Selected > -1) MessageBox.Show("Select a Node by clicking on it !!!");


            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }
            bool_FDirectSupp = true;
            Invoke_Edit_False();

        }

        private void eXITToolStripMenuItem_Click(object sender, EventArgs e)
        {

            
        }




        private void ForceValueToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (ForceValueToolStripMenuItem.Checked == true)
            {
                bool_View_Force_Value = true;
                PictureBox1.Invalidate();
            }
            if (ForceValueToolStripMenuItem.Checked == false)
            {
                bool_View_Force_Value = false;
                PictureBox1.Invalidate();
            }
        }

        private void View_Node_Number_CheckedChanged(object sender, EventArgs e)
        {
            if (View_Node_Number.Checked == true)
            {
                bool_View_Node_Number = true;
                PictureBox1.Invalidate();
            }
            if (View_Node_Number.Checked == false)
            {
                bool_View_Node_Number = false;
                PictureBox1.Invalidate();
            }
        }

        private void View_Member_Number_CheckedChanged(object sender, EventArgs e)
        {
            if (View_Member_Number.Checked == true)
            {
                bool_View_Member_Number = true;
                PictureBox1.Invalidate();
            }
            if (View_Member_Number.Checked == false)
            {
                bool_View_Member_Number = false;
                PictureBox1.Invalidate();
            }
        }

        private void ZOOM_Minus_Click(object sender, EventArgs e)
        {
            PictureScale = PictureScale - .2F;
            if (PictureScale < 0.5F) PictureScale = 0.5F;
            ZOOMPlus_bool = true;


            Debug.Print("PictureScale=  " + PictureScale);



            Size size = new Size(Convert.ToInt32(PictureScale * 2500), Convert.ToInt32(PictureScale * 1500));
            PictureBox1.Size = size;
            PictureBox1.Invalidate();

        }

        private void hingeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bool_EDIT == true | Last_index_line_selected > -1) MessageBox.Show("Select a Member by clicking on it !!!");
            selpt = false;
            selln = true;
            drawln = false;


            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;


            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }

            bool_HINGE_Intermediate = true;
            Invoke_Edit_False();
        }






        private double[,] Inverse_Mat(double[,] a)
        {












            int dm = a.GetUpperBound(0) + 1;
            double[,] Matrix_A = new double[(dm * 2) + 1, (dm * 2) + 1];
            double[,] Operations_Matrix = new double[(dm * 2) + 1, (dm * 2) + 1];
            double[,] Inverse_Matrix = new double[dm + 1, dm + 1];
            double[,] Inverse_MatrixY = new double[dm, dm];

            double temporary_1 = 0;
            double elem1 = 0;
            double multiplier_1 = 0;

            int Max_Index = 0;
            int m = 0;
            int n = 0;
            int k = 0;
            int line_1 = 0;
            int Double_Size_Max_Index = 0;
            Max_Index = a.GetUpperBound(0) + 1;



            for (n = 0; n < Max_Index; n++)
            {
                for (m = 0; m < Max_Index; m++)
                {
                    Matrix_A[m + 1, n + 1] = a[m, n];
                }
            }

            for (n = 1; n <= Max_Index; n++)
            {
                for (m = 1; m <= Max_Index; m++)
                {
                    Operations_Matrix[m, n] = Matrix_A[m, n];
                }
            }


            for (n = 1; n <= Max_Index; n++)
            {
                for (m = 1; m <= Max_Index; m++)
                {
                    if (n == m)
                    {
                        Operations_Matrix[m, n + Max_Index] = 1;
                    }
                    else
                    {
                        Operations_Matrix[m, n + Max_Index] = 0;
                    }
                }
            }


            for (k = 1; k <= Max_Index; k++)
            {

                if (Operations_Matrix[k, k] == 0)
                {
                    for (n = k; n <= Max_Index; n++)
                    {
                        if (Operations_Matrix[n, k] != 0)
                        {
                            line_1 = n;
                            break;
                        }
                    }



                    for (m = k; m <= Max_Index * 2; m++)
                    {
                        temporary_1 = Operations_Matrix[k, m];
                        Operations_Matrix[k, m] = Operations_Matrix[line_1, m];
                        Operations_Matrix[line_1, m] = temporary_1;
                    }
                }

                elem1 = Operations_Matrix[k, k];


                for (n = k; n <= 2 * Max_Index; n++)
                {
                    Operations_Matrix[k, n] = Operations_Matrix[k, n] / elem1;
                    if (elem1 == 0)
                    {
                        MessageBox.Show("Not invertible matrix, determinant zero !");
                        System.Environment.Exit(1);

                    }

                }





                Double_Size_Max_Index = 2 * Max_Index;
                for (n = 1; n <= Max_Index; n++)
                {
                    if (n == k && n == Double_Size_Max_Index)
                    {
                        break;
                    }
                    if (n == k && n < Double_Size_Max_Index)
                    {
                        n = n + 1;
                    }
                    if (Operations_Matrix[n, k] != 0)
                    {
                        multiplier_1 = Operations_Matrix[n, k] / Operations_Matrix[k, k];


                        for (m = k; m <= 2 * Max_Index; m++)
                        {
                            Operations_Matrix[n, m] = Operations_Matrix[n, m] - Operations_Matrix[k, m] * multiplier_1;
                        }
                    }
                }
            }


            for (n = 1; n <= Max_Index; n++)
            {
                for (k = 1; k <= Max_Index; k++)
                {
                    Inverse_Matrix[n, k] = Operations_Matrix[n, Max_Index + k];
                }
            }

            for (n = 1; n <= Max_Index; n++)
            {
                for (k = 1; k <= Max_Index; k++)
                {
                    Inverse_MatrixY[n - 1, k - 1] = Inverse_Matrix[n, k];
                }
            }






            return Inverse_MatrixY;


        }

        private void viewPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Select a Member by clicking on it !!!");
            selpt = false;
            selln = true;
            drawln = false;


            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = true;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;


            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }


        }

        private void viewResultTableToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Select a Member by clicking on it !!!");
            selpt = false;
            selln = true;
            drawln = false;


            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;



            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }


            bool_FShowTable = true;

        }




        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyValue > 31 & drawln)
            {

                MessageBox.Show("Do you wan to canel the line ?");

                bool_cancel_draw_line = true;


            }
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            selpt = true;
            selln = false;
            drawln = false;
            bool_Fixed_support = true;
            bool_Pinned_support = false;
            bool_Delete_support = false;
            bool_Roller_Supp_Ydir = false;

            if (bool_EDIT == true | nodenumer_Selected > -1) MessageBox.Show("Select a Node by clicking on it !!!");





            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }

            bool_FDirectSupp = true;
            Invoke_Edit_False();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            selpt = true;
            selln = false;
            drawln = false;
            bool_Fixed_support = false;
            bool_Pinned_support = true;
            bool_Delete_support = false;
            bool_Roller_Supp_Ydir = false;

            if (bool_EDIT == true | nodenumer_Selected > -1) MessageBox.Show("Select a Node by clicking on it !!!");





            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }

            bool_FDirectSupp = true;
            Invoke_Edit_False();

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            ZOOMPlus_bool = false;
            PictureScale = 1.0F;

            Debug.Print("PictureScale=  " + PictureScale);


            Size size = new Size(Convert.ToInt32(PictureScale * 2500), Convert.ToInt32(PictureScale * 1500));

            PictureBox1.Size = size;
            PictureBox1.Invalidate();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {

            selpt = true;
            selln = false;
            drawln = false;
            bool_Fixed_support = false;
            bool_Pinned_support = false;
            bool_Delete_support = false;
            bool_Roller_Supp_Ydir = true;
            if (bool_EDIT == true | nodenumer_Selected > -1) MessageBox.Show("Select a Node by clicking on it !!!");





            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;

            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }

            bool_FDirectSupp = true;
            Invoke_Edit_False();

        }

        private void gridDiv1mToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw_beam();
            lenght_factor = 1;
        }

        private void gridDiv2mToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw_beam();
            lenght_factor = 2;
        }

        private void gridDiv3mToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw_beam();
            lenght_factor = 3;
        }

        private void gridDiv4mToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw_beam();
            lenght_factor = 4;
        }



        private void toolStripButton3_Click(object sender, EventArgs e)
        {

            selpt = true;
            selln = false;
            drawln = false;

            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;


            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }
        }



        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            button11_Click(sender, e);
        }

        private void viewPostAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View_Shear = false;
            View_Mom = false;
            View_Theta = false;
            View_Delta = true;
            View_AxialForce = false;


            PictureBox1.Refresh();
        }

        private void showGraphAndTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomPlus.Enabled = false;
            ZOOM_Minus.Enabled = false;
            ZOOMPlus_bool = false;
            //===============================
            MessageBox.Show("Select a Member by clicking on it !!!");
            selpt = false;
            selln = true;
            drawln = false;


            bool_FconcP = false;
            bool_FconcM = false;
            bool_FSupDisp = false;
            bool_FUDL = false;
            bool_FNodal = false;
            bool_FSuppData = false;
            bool_FDelSupp = false;
            bool_FBeamProp = false;
            bool_HINGE_Intermediate = false;
            bool_FShowTable = false;
            bool_FDirectSupp = false;



            if (mLines.Count == 0)
            {
                MessageBox.Show("No Member Found !!!");
            }


            bool_FShowTable = true;



        }






        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Test_PictureBox1_Paint = true;
            PictureBox1.Refresh();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            fileSaveAsToolStripMenuItem.PerformClick();
        }




        ////=====================================================



        









        private void button7_Click(object sender, EventArgs e)
        {
            Foutput f = new Foutput();

            f.ShowDialog();
        }

        private void aBOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FABOUT f = new FABOUT();

            f.ShowDialog();
        }





















































































































    }










    public class line
    {







        public PointF StartPoint;
        public PointF EndPoint;

        public override string ToString()
        {
            return "StartPoint.X,StartPoint.Y,EndPoint.X,EndPoint.Y= " + StartPoint.X + " , " + StartPoint.Y + " , " +
                EndPoint.X + " , " + EndPoint.Y;
        }

    }

    ////%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

    ////===============================
    public class StringBuilderTraceListener : TraceListener
    {
        private StringBuilder stringBuilder1;



        public StringBuilderTraceListener(StringBuilder stringBuilderx)
        {
            stringBuilder1 = stringBuilderx;
        }

        public override void Write(string message)
        {
            stringBuilder1.Append(message);
        }

        public override void WriteLine(string message)
        {
            stringBuilder1.AppendLine(message);
        }
    }

    ////===============================

    ////===================================
}


