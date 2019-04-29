
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
//using System.IO.Ports;
using System.Threading;
using UnityEngine.UI;

public class Hand3 : MonoBehaviour
{
    public struct RaderAngLog
    {
        public struct max
        {
            public static int A1Ang = 0;
            public static int A2Ang = 0;
            public static int B1Ang = 0;
            public static int B2Ang = 0;
            public static int C1Ang = 0;
            public static int C2Ang = 0;
            public static int D1Ang = 0;
            public static int D2Ang = 0;
            public static int E1Ang = 0;
            public static int E2Ang = 0;

            public static double AAng = 0;
            public static double BAng = 0;
            public static double CAng = 0;
            public static double DAng = 0;
            public static double EAng = 0;
        }

        public struct min
        {
            public static int A1Ang = 0;
            public static int A2Ang = 0;
            public static int B1Ang = 0;
            public static int B2Ang = 0;
            public static int C1Ang = 0;
            public static int C2Ang = 0;
            public static int D1Ang = 0;
            public static int D2Ang = 0;
            public static int E1Ang = 0;
            public static int E2Ang = 0;
        }

        public struct delta
        {
            public static int A1Ang = 0;
            public static int A2Ang = 0;
            public static int B1Ang = 0;
            public static int B2Ang = 0;
            public static int C1Ang = 0;
            public static int C2Ang = 0;
            public static int D1Ang = 0;
            public static int D2Ang = 0;
            public static int E1Ang = 0;
            public static int E2Ang = 0;
        }
    }

    //NodeFlexTest_Activater
    public static int Button_Enable = 0;
    public static int[] NodeEnable;
    public static int[] timer;
    public static int[] StartAng;
    public static int[] EndAng;
    public static int TestState = 0;
    public static int[] NodeState;
    public static double[] FlexScore;


    public static int Debug_Rand_Data=0;

    //Init_IO_Port
    //串口
//    private SerialPort port = new SerialPort();

    //Init_Fingers_Nodes
    public GameObject Bone001;

    public GameObject Bone002;
    public GameObject Bone003;
    public GameObject Bone004;
    public GameObject Bone005;

    public GameObject Bone006;
    public GameObject Bone007;
    public GameObject Bone008;
    public GameObject Bone009;
    public GameObject Bone010;

    public GameObject Bone011;
    public GameObject Bone012;
    public GameObject Bone013;
    public GameObject Bone014;
    public GameObject Bone015;

    public GameObject Bone016;
    public GameObject Bone017;
    public GameObject Bone018;
    public GameObject Bone019;
    public GameObject Bone020;

    public GameObject Bone021;
    public GameObject Bone022;
    public GameObject Bone023;
    public GameObject Bone024;
    public GameObject Bone025;


    public GameObject Bone032;
    
    //Finger_Ang_Recorder
    static int[] RHandAng;
    static int[] LHandAng;
    private byte[] ByteAng;
    private string TempStrAng;
    //缓存没有用过的数据
    public static string StrAng = "";
    //缓存依照约定拆解的字符串
    private string[] portDataSplit;
    //缓存有效数据
    private string validData = "";


    //Finger_IntAng_Recorder
    static int A1Ang = 0;
    static int A2Ang = 0;
    static int B1Ang = 0;
    static int B2Ang = 0;
    static int C1Ang = 0;
    static int C2Ang = 0;
    static int D1Ang = 0;
    static int D2Ang = 0;
    static int E1Ang = 0;
    static int E2Ang = 0;

    //Update_Frequency_Delay_Timer
    static int counter = 1;

    public Text FlexScoreText;
    public PolygonImage polygonImage;
    private double[] angles=new double[5];
    List<float> weights;
    public bool isTesting = false;
    public GameObject barChart;
    public Transform[] barTransforms;
    public Image[] barImages;
    public Text startBtnTxt;
    public GameObject root;
    public Transform[] boneTransformInit;
    
    private void Awake()
    {
        Button_Enable = 1;
        //Init_IO_Port();
    }

    /*private void Init_IO_Port()
    {
        //UART_Configure
        port.PortName = "COM3";
        port.ReadBufferSize = 128;
        port.BaudRate = 115200;
        port.Parity = Parity.None;
        port.DataBits = 8;
        port.StopBits = StopBits.One;
        port.ReadTimeout = 500;
        port.Open();
    }*/


    void Start()
    {
        //Find_GameObject_Connection
        Bone001 = GameObject.Find("Bone001");
        Bone002 = GameObject.Find("Bone002");
        Bone003 = GameObject.Find("Bone003");
        Bone004 = GameObject.Find("Bone004");
        Bone005 = GameObject.Find("Bone005");
        Bone006 = GameObject.Find("Bone006");
        Bone007 = GameObject.Find("Bone007");
        Bone008 = GameObject.Find("Bone008");
        Bone009 = GameObject.Find("Bone009");
        Bone010 = GameObject.Find("Bone010");
        Bone011 = GameObject.Find("Bone011");
        Bone012 = GameObject.Find("Bone012");
        Bone013 = GameObject.Find("Bone013");
        Bone014 = GameObject.Find("Bone014");
        Bone015 = GameObject.Find("Bone015");
        Bone016 = GameObject.Find("Bone016");
        Bone017 = GameObject.Find("Bone017");
        Bone018 = GameObject.Find("Bone018");
        Bone019 = GameObject.Find("Bone019");
        Bone020 = GameObject.Find("Bone020");
        Bone021 = GameObject.Find("Bone021");
        Bone022 = GameObject.Find("Bone022");
        Bone023 = GameObject.Find("Bone023");
        Bone024 = GameObject.Find("Bone024");
        Bone025 = GameObject.Find("Bone025");
        Bone032 = GameObject.Find("Bone032");

        //Init_Ang_Recorder
        RHandAng = new int[10];
        LHandAng = new int[10];
        weights = polygonImage.edgeWeights.Weights;
        barTransforms = barChart.GetComponentsInChildren<Transform>();
        barImages = barChart.GetComponentsInChildren<Image>();
        boneTransformInit = root.GetComponentsInChildren<Transform>();
    }


    void Update()
    {
        /***********************************
         **********Call_Delay_Timer*********
         ***********************************/

        //这是一个计数器，用于延迟更新速度
        if (counter % 1 != 0)//控制次量即可控制延迟，1表示不延迟
        {
            counter += 1;
            return;
        }
        else
        {
            counter = 1;
        }
        //这里计数器结束

        /***********************************
         **********Call_Delay_Timer*********
         ***********************************/

        /**********************************/

        if (Debug_Rand_Data >= 90)
            Debug_Rand_Data = 0;
        else
            Debug_Rand_Data += 1;


        for (int i = 0; i < 10; i++)
            RHandAng[i] = Debug_Rand_Data;
        /***********************************
        ***********Read_from_Port***********
        ***********************************/

        //Read_from_Port();

        /***********************************
         **********Read_from_Port***********
         ***********************************/

        /**********************************/

        /***********************************
         *****Load_VectorAng_to_HandAng*****
         ***********************************/

        /*int a, b;//存储角度的十位和个位 
        a = b = 0;
        int count = 0;
        int flag = 0;

        for (int i = 1; i < 32; i = i + 1)
        {
            if (StrAng[i] == '$')
            {


                if (flag == 1)
                {
                    RHandAng[count] = a;

                }
                else if (flag == 2)
                {
                    RHandAng[count] = b + 10 * a;

                }
                count += 1;
                flag = 0;
                if (count == 10)
                {
                    break;
                }
            }
            else
            {

                // if (StrAng[i] == '0' && count == 10)
                // {
                //     break;
                // }
                // else
                // {
                if (flag == 0)
                {
                    a = (int)StrAng[i] - 48;
                    flag = 1;
                }
                else if (flag == 1)
                {
                    b = (int)StrAng[i] - 48;
                    flag = 2;
                }
                //}
            }
        }*/

        /***********************************
        *****Load_VectorAng_to_HandAng*****
        ***********************************/

        /***********************************/

        /***********************************
        ************Log_Recoder************
        ***********************************/

        

        if (isTesting)
        {
            Rotate();
            RaderAngLog_Activation_Manager();
            NodeFlexTest_Activation_Manager();
            /***********************************
            ************Log_Recoder************
            ***********************************/

            /***********************************/

            /***********************************
            *****Handle_Ang_Data_to_3DVector****
            ***********************************/




//        Debug.Log(FlexScore[1]);
        
            ShowFlexScore();
            ShowAngles();
        }
    }

//    private void Read_from_Port()
//    {
//        if (!port.IsOpen)
//        {
//            return;
//        }
//
//        //读取串口数据
//        ByteAng = new byte[port.ReadBufferSize];
//        try
//        {
//            int count = port.Read(ByteAng, 0, port.ReadBufferSize);
//            //转化成字符串
//            TempStrAng = Encoding.ASCII.GetString(ByteAng, 0, count);
//            if (count > 1)
//            {
//                //保存字符串
//                StrAng = TempStrAng;
//                //Debug.Log(StrAng);
//            }
//        }
//        catch (TimeoutException)
//        {
//        }
//    }

    private void Rotate()
    {
                //Call_Filter_Load_Ang_to_Temp_Recorder
        A1Ang = (A1Ang + 4 * RHandAng[0]) / 5;
        A2Ang = (A2Ang + 4 * RHandAng[1]) / 5;
        B1Ang = (B1Ang + 4 * RHandAng[2]) / 5;
        B2Ang = (B2Ang + 4 * RHandAng[3]) / 5;
        C1Ang = (C1Ang + 4 * RHandAng[4]) / 5;
        C2Ang = (C2Ang + 4 * RHandAng[5]) / 5;
        D1Ang = (D1Ang + 4 * RHandAng[6]) / 5;
        D2Ang = (D2Ang + 4 * RHandAng[7]) / 5;
        E1Ang = (E1Ang + 4 * RHandAng[8]) / 5;
        E2Ang = (E2Ang + 4 * RHandAng[9]) / 5;

//        Bone032.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -(float)A1Ang);
//        Bone002.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -(float)A1Ang);//Got Mess Here!!!
        Bone003.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -(float)A2Ang);
        Bone004.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -(float)A2Ang / 2);

        Bone007.transform.localEulerAngles = new Vector3(0.0f, -(float)B1Ang, 0.0f);
        Bone008.transform.localEulerAngles = new Vector3(0.0f, -(float)B2Ang, 0.0f);
        Bone009.transform.localEulerAngles = new Vector3(0.0f, -(float)B2Ang / 2, 0.0f);

        Bone012.transform.localEulerAngles = new Vector3(0.0f, -(float)C1Ang, 0.0f);
        Bone013.transform.localEulerAngles = new Vector3(0.0f, -(float)C2Ang, 0.0f);
        Bone014.transform.localEulerAngles = new Vector3(0.0f, -(float)C2Ang / 2, 0.0f);

        Bone017.transform.localEulerAngles = new Vector3(0.0f, -(float)D1Ang, 0.0f);
        Bone018.transform.localEulerAngles = new Vector3(0.0f, -(float)D2Ang, 0.0f);
        Bone019.transform.localEulerAngles = new Vector3(0.0f, -(float)D2Ang / 2, 0.0f);

        Bone022.transform.localEulerAngles = new Vector3(0.0f, -(float)E1Ang, 0.0f);
        Bone023.transform.localEulerAngles = new Vector3(0.0f, -(float)E2Ang, 0.0f);
        Bone024.transform.localEulerAngles = new Vector3(0.0f, -(float)E2Ang / 2, 0.0f);
        /***********************************
        *****Handle_Ang_Data_to_3DVector****
        ***********************************/
    }

    public void SetIsTesting()
    {
        isTesting = !isTesting;
        if (isTesting)
        {
            startBtnTxt.text = "STOP";
        }
        else
        {
            startBtnTxt.text = "START";
            Init();
        }
    }

    private void Init()
    {
        A1Ang = 0;
        A2Ang = 0;
        B1Ang = 0;
        B2Ang = 0;
        C1Ang = 0;
        C2Ang = 0;
        D1Ang = 0;
        D2Ang = 0;
        E1Ang = 0;
        E2Ang = 0;
        
        RaderAngLog_Activation_Manager();
        NodeFlexTest_Activation_Manager();
        
        ShowFlexScore();
        ShowAngles();
    }

    private void NodeFlexTest_Activation_Manager() //灵敏度评估函数
    {
        if (Button_Enable == 1)
        {
            if (TestState == 0)
            {
                NodeEnable = new int[10] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };//这里传入节点开关
                NodeState = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                timer = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                StartAng = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//这里传入起始角度
                EndAng = new int[10] { 90, 90, 90, 90, 90, 90, 90, 90, 90, 90 };//这里传入终止角度
                FlexScore = new double[10] { 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00 };
                TestState = 1;
            }

            for (int i = 0; i < 10; i++)
            {
                if (NodeEnable[i] == 1)
                {
                    if (NodeState[i] == 0)
                    {
                        if (RHandAng[i] + 2 <= StartAng[i] || RHandAng[i] - 2 >= StartAng[i])
                            NodeState[i] = 1;
                    }
                    else
                    {
                        timer[i]++;
                        if ((RHandAng[i] >= EndAng[i] && EndAng[i] > StartAng[i]) || (RHandAng[i] <= EndAng[i] && EndAng[i] < StartAng[i]))//这里对于每一个测试节点而言是出口，在括号里把timer打到评估系统
                        {
                            NodeEnable[i] = 0;
                            NodeState[i] = 0;

                            FlexScore[i] = 1 - 0.05 * (timer[i] - 24) / 6;//6代表几帧为一个采样点（单片机使用200ms采样周期，但实际更长）；10代表健康阈值，使好于这个阈值的得分均为1，0.20表示0分所代表的周期数
                            if (FlexScore[i] > 1)
                                FlexScore[i] = 1;//评分就是FlexScore
                            else if (FlexScore[i] < 0)
                            {
                                FlexScore[i] = 0;
                            }

                        }
                    }
                }
            }
        }
        else
        {
            TestState = 0;
        }
    }
    private void RaderAngLog_Activation_Manager() //角度评估函数
    {
        //RaderAngLog
        RaderAngLog.max.A1Ang = RaderAngLog.max.A1Ang > A1Ang ? RaderAngLog.max.A1Ang : A1Ang;
        RaderAngLog.max.A2Ang = RaderAngLog.max.A2Ang > A2Ang ? RaderAngLog.max.A2Ang : A2Ang;
        RaderAngLog.max.B1Ang = RaderAngLog.max.B1Ang > B1Ang ? RaderAngLog.max.B1Ang : B1Ang;
        RaderAngLog.max.B2Ang = RaderAngLog.max.B2Ang > B2Ang ? RaderAngLog.max.B2Ang : B2Ang;
        RaderAngLog.max.C1Ang = RaderAngLog.max.C1Ang > C1Ang ? RaderAngLog.max.C1Ang : C1Ang;
        RaderAngLog.max.C2Ang = RaderAngLog.max.C2Ang > C2Ang ? RaderAngLog.max.C2Ang : C2Ang;
        RaderAngLog.max.D1Ang = RaderAngLog.max.D1Ang > D1Ang ? RaderAngLog.max.D1Ang : D1Ang;
        RaderAngLog.max.D2Ang = RaderAngLog.max.D2Ang > D2Ang ? RaderAngLog.max.D2Ang : D2Ang;
        RaderAngLog.max.E1Ang = RaderAngLog.max.E1Ang > E1Ang ? RaderAngLog.max.E1Ang : E1Ang;
        RaderAngLog.max.E2Ang = RaderAngLog.max.E2Ang > E2Ang ? RaderAngLog.max.E2Ang : E2Ang;
        RaderAngLog.min.A1Ang = RaderAngLog.min.A1Ang < A1Ang ? RaderAngLog.min.A1Ang : A1Ang;
        RaderAngLog.min.A2Ang = RaderAngLog.min.A2Ang < A2Ang ? RaderAngLog.min.A2Ang : A2Ang;
        RaderAngLog.min.B1Ang = RaderAngLog.min.B1Ang < B1Ang ? RaderAngLog.min.B1Ang : B1Ang;
        RaderAngLog.min.B2Ang = RaderAngLog.min.B2Ang < B2Ang ? RaderAngLog.min.B2Ang : B2Ang;
        RaderAngLog.min.C1Ang = RaderAngLog.min.C1Ang < C1Ang ? RaderAngLog.min.C1Ang : C1Ang;
        RaderAngLog.min.C2Ang = RaderAngLog.min.C2Ang < C2Ang ? RaderAngLog.min.C2Ang : C2Ang;
        RaderAngLog.min.D1Ang = RaderAngLog.min.D1Ang < D1Ang ? RaderAngLog.min.D1Ang : D1Ang;
        RaderAngLog.min.D2Ang = RaderAngLog.min.D2Ang < D2Ang ? RaderAngLog.min.D2Ang : D2Ang;
        RaderAngLog.min.E1Ang = RaderAngLog.min.E1Ang < E1Ang ? RaderAngLog.min.E1Ang : E1Ang;
        RaderAngLog.min.E2Ang = RaderAngLog.min.E2Ang < E2Ang ? RaderAngLog.min.E2Ang : E2Ang;
        RaderAngLog.delta.A1Ang = RaderAngLog.max.A1Ang - RaderAngLog.min.A1Ang;
        RaderAngLog.delta.A2Ang = RaderAngLog.max.A2Ang - RaderAngLog.min.A2Ang;
        RaderAngLog.delta.B1Ang = RaderAngLog.max.B1Ang - RaderAngLog.min.B1Ang;
        RaderAngLog.delta.B2Ang = RaderAngLog.max.B2Ang - RaderAngLog.min.B2Ang;
        RaderAngLog.delta.C1Ang = RaderAngLog.max.C1Ang - RaderAngLog.min.C1Ang;
        RaderAngLog.delta.C2Ang = RaderAngLog.max.C2Ang - RaderAngLog.min.C2Ang;
        RaderAngLog.delta.D1Ang = RaderAngLog.max.D1Ang - RaderAngLog.min.D1Ang;
        RaderAngLog.delta.D2Ang = RaderAngLog.max.D2Ang - RaderAngLog.min.D2Ang;
        RaderAngLog.delta.E1Ang = RaderAngLog.max.E1Ang - RaderAngLog.min.E1Ang;
        RaderAngLog.delta.E2Ang = RaderAngLog.max.E2Ang - RaderAngLog.min.E2Ang;
        //取出的五个角度值
        RaderAngLog.max.AAng = 1.00*(RaderAngLog.delta.A1Ang + RaderAngLog.delta.A2Ang) / 180;
        RaderAngLog.max.BAng = 1.00*(RaderAngLog.delta.B1Ang + RaderAngLog.delta.B2Ang) / 180;
        RaderAngLog.max.CAng = 1.00*(RaderAngLog.delta.C1Ang + RaderAngLog.delta.C2Ang) / 180;
        RaderAngLog.max.DAng = 1.00*(RaderAngLog.delta.D1Ang + RaderAngLog.delta.D2Ang) / 180;
        RaderAngLog.max.EAng = 1.00*(RaderAngLog.delta.E1Ang + RaderAngLog.delta.E2Ang) / 180;

        angles[0] = RaderAngLog.max.AAng;
        angles[1] = RaderAngLog.max.BAng;
        angles[2] = RaderAngLog.max.CAng;
        angles[3] = RaderAngLog.max.DAng;
        angles[4] = RaderAngLog.max.EAng;
    }

    void ShowFlexScore()
    {
//        FlexScoreText.text = "";
//        String text = "";
//        for (int i = 0; i < FlexScore.Length; i++)
//        {
//            text +="("+(i+1)+")\t"+FlexScore[i] + "\n";
//        }
//        FlexScoreText.text = text;

        for (int i = 0; i < FlexScore.Length; i++)
        {
            barTransforms[i+1].transform.localScale=new Vector3((float)FlexScore[i],1.0f,1.0f);
        }
        barChart.transform.localScale=new Vector3(0.73335f,0.73335f,0.73335f);
    }

    void ShowAngles()
    {
        polygonImage.SetVerticesDirty();
        for (int i = 0; i < weights.Count; i++)
        {
            weights[i] = (float)angles[i];
        }
    }
}






