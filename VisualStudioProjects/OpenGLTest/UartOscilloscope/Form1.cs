﻿///   <summary>
///   C#實作Uart接收程式，By Jimmy
///   </summary>
using System;                                                                   //  使用System函式庫
using System.Windows.Forms;                                                     //  使用System.Windows.Forms函式庫
using System.IO.Ports;                                                          //  使用System.IO.Ports函式庫
using System.Collections;                                                       //  使用System.Collections函式庫
//  System.Collections函式庫定義Queue資料型態
using System.Collections.Generic;                                               //  使用System.Collections.Generic函式庫
//  System.Collections.Generic函式庫定義非泛型Queue資料型態
using System.Drawing;                                                           //  使用System.Drawing函式庫
using SharpGL;                                                                  //  使用SharpGL函式庫

namespace WindowsFormsApplication6                                              //  命名空間為本程式
{                                                                               //  進入命名空間
    public partial class Form1 : Form                                           //  Form1類別
    {                                                                           //  進入Form1類別
        //-----全域變數宣告-----
        public static float Program_Vision = 27;                                //  宣告Program_Vision靜態全域變數，記錄程式版本
        public static int Program_Work_Mode = 0;                                //  宣告Program_Work_Mode靜態全域變數，控制程式執行模式
        public struct Debug_Login_Account_struct                                //  宣告Debug_Login_Account_struct全域結構
        {                                                                       //  進入Debug_Login_Account_struct結構設定
            public string Debug_Login_Account, Debug_Login_Password;            //  在Debug_Login_Account_struct結構中有兩項字串(Account與Password)
            public Debug_Login_Account_struct(string p1, string p2)             //  設定結構成員
            {                                                                   //  設定結構成員
                Debug_Login_Account = p1;                                       //  設定結構成員
                Debug_Login_Password = p2;                                      //  設定結構成員
            }                                                                   //  設定結構成員完成
        }                                                                       //  結束Debug_Login_Account_struct結構設定
        public static Debug_Login_Account_struct Debug_Account1 =               //  宣告Debug_Account1靜態全域除錯帳戶1
            new Debug_Login_Account_struct("Debug", "debug");                   //  設定Debug_Account1除錯帳戶1帳號密碼
        public static DateTime localDate;                                       //  宣告localDate時間變數，記錄現在時間
        public static int BaudRate;                                             //  宣告BaudRate靜態全域變數，控制SerialPort連線鮑率
        public static int Parity_num;                                           //  宣告Parity_num靜態全域變數，控制SerialPort串列埠之Parity同位位元設定
        public static int DataBits_num;                                         //  宣告DataBits_num靜態全域變數，控制SerialPort串列埠之DataBits數值
        public static int Error_Code;                                           //  宣告Error_Code靜態全域變數，記錄錯誤編碼，協助偵錯
        public static Font textBox1_Font;                                       //  宣告textBox1_Font靜態字型變數，控制接收字串資料文字方塊字型
        public static SerialPort Uart_comport;                                  //  宣告新的SerialPort通訊埠，名稱為Uart_comport
        public static bool Uart_comport_connected;                              //  宣告Uart_comport_connected布林變數，表示Uart_comport連線狀態
        public static char[] Transmission_Buffer_CharArray;                     //  宣告Transmission_Buffer_CharArray(Uart通訊傳輸Buffer資料)字元陣列，用於字串資料解析
        public static char[] Transmission_Analysis_CharArray;                   //  宣告Transmission_Analysis_CharArray字元陣列，用於字串資料解析
        public static int Transmission_Analysis_CharArray_Datanum = 0;
        //  宣告Transmission_Analysis_CharArray_Datanum全域靜態變數，記錄Transmission_Analysis_CharArray字元陣列中已填入資料長度，並初始化為0
        public static Queue Transmission_Analysis_Queue;                        //  宣告Transmission_Analysis_Queue(Uart通訊傳輸資料分析字元佇列)為全域靜態佇列
        public static int Total_Transmission_Length = 0;                        //  宣告UART通訊傳輸字串累計長度統計全域靜態變數，並初始化為0
        public static int Analysed_Length = -1;
        //  宣告Analysed_Length全域靜態變數，記錄Transmission_Analysis_CharArray字元陣列中已分析字串長度(陣列值)，並初始化為-1(因陣列從0開始)
        public static int COM_Port_num;                                         //  宣告COM_Port_num全域靜態變數，記錄已連線的SerialPort數量
        public static uint button1_Click_Runtimes = 0;                          //  宣告button1_Click_Runtimes全域靜態變數，記錄button1_Click副程式執行次數，並初始化為0
        public static uint button2_Click_Runtimes = 0;                          //  宣告button2_Click_Runtimes全域靜態變數，記錄button2_Click副程式執行次數，並初始化為0
        public static uint button3_Click_Runtimes = 0;                          //  宣告button3_Click_Runtimes全域靜態變數，記錄button3_Click副程式執行次數，並初始化為0
        public static uint Transmission_Setting_Click_Runtimes = 0;
        //  宣告Transmission_Setting_Click_Runtimes全域靜態變數，記錄設定_傳輸設定ToolStripMenuItem_Click副程式執行次數，並初始化為0
        public static uint User_Interface_Setting_Click_Runtimes = 0;
        //  宣告User_Interface_Setting_Click_Runtimes全域靜態變數，記錄設定_介面設定ToolStripMenuItem_Click副程式執行次數，並初始化為0
        public static uint list_SerialPort_Runtimes = 0;                        //  宣告list_SerialPort_Runtimes全域靜態變數，記錄list_SerialPort副程式執行次數
        public static uint Uart_comport_handle_Runtimes = 0;                    //  宣告Uart_comport_handle_Runtimes全域靜態變數，記錄Uart_comport_handle副程式執行次數
        public static uint comport_DataReceived_Runtimes = 0;                   //  宣告comport_DataReceived_Runtimes全域靜態變數，記錄comport_DataReceived副程式執行次數
        public static uint DisplayText_Runtimes = 0;                            //  宣告DisplayText_Runtimes全域靜態變數，記錄DisplayText副程式執行次數
        public static int Uart_Buffer_Size = 0;                                 //  宣告Uart_Buffer_Size全域靜態變數，記錄Uart接收資料Buffer(資料緩衝區)大小
        public static string Uart_Buffer_ASCII_Data = "";                       //  宣告Uart_Buffer_ASCII_Data全域靜態字串，記錄Uart傳輸之Buffer資料(ASCII編碼值)
        ///***以下宣告示波器功能實作變數***
        public static int[] ADC_Raw_Data_X;                                     //  宣告ADC_Raw_Data_X全域整數陣列變數，記錄X通道ADC原始資料
        public static int[] ADC_Raw_Data_Y;                                     //  宣告ADC_Raw_Data_Y全域整數陣列變數，記錄Y通道ADC原始資料
        public static int[] ADC_Raw_Data_Z;                                     //  宣告ADC_Raw_Data_Z全域整數陣列變數，記錄Z通道ADC原始資料
        public static int ADC_Raw_Data_X_num = 0;                               //  宣告ADC_Raw_Data_X_num全域整數變數，記錄ADC_Raw_Data_X資料量
        public static int ADC_Raw_Data_Y_num = 0;                               //  宣告ADC_Raw_Data_Y_num全域整數變數，記錄ADC_Raw_Data_Y資料量
        public static int ADC_Raw_Data_Z_num = 0;                               //  宣告ADC_Raw_Data_Z_num全域整數變數，記錄ADC_Raw_Data_Z資料量
        public static int ADC_Raw_Data_Max = 100;                             //  宣告ADC_Raw_Data_Max全域整數變數，記錄ADC_Raw_Data陣列大小
        public static Queue<int> Data_Graphic_Queue_X;                          //  宣告X通道資料繪圖用整數型態佇列Data_Graphic_Queue_X
        public static Queue<int> Data_Graphic_Queue_Y;                          //  宣告Y通道資料繪圖用整數型態佇列Data_Graphic_Queue_Y
        public static Queue<int> Data_Graphic_Queue_Z;                          //  宣告Z通道資料繪圖用整數型態佇列Data_Graphic_Queue_Z
        public struct OpenGL_Graph_point                                        //  宣告OpenGL_Graph_point結構，用於OpenGL繪圖座標宣告
        {                                                                       //  進入OpenGL_Graph_point結構
            public double point_X, point_Y;                                     //  在OpenGL_Graph_point結構中有兩項雙精度浮點數
            public OpenGL_Graph_point(double X, double Y)                       //  設定結構成員
            {                                                                   //  設定結構成員
                point_X = X;                                                    //  宣告OpenGL_Graph_point結構內部元素(X座標)
                point_Y = Y;                                                    //  宣告OpenGL_Graph_point結構內部元素(Y座標)
            }                                                                   //  設定結構成員完成
        }                                                                       //  結束OpenGL_Graph_point結構
        public static int loop_num;                                             //  宣告loop_num全域靜態變數，供迴圈使用
        delegate void Display(byte[] buffer);                                   //  定義Display型態
        //delegate 是可用來封裝具名方法或匿名方法的參考型別。
        public Form1()                                                          //  宣告Form1
        {                                                                       //  進入Form1
            InitializeComponent();                                              //  初始化表單
            Error_Code = 0;                                                     //  初始化Error_Code為0
        }                                                                       //  結束Form1
        public void Form1_Load(object sender, EventArgs e)                      //  Form1表單載入時執行
        {                                                                       //  進入Form1_Load副程式
            timer1.Interval = 100;                                              //  設定timer1執行頻率
            timer1.Enabled = true;                                              //  啟動timer1，即時更新現在時間
            textBox1_Font = new Font("新細明體", 9, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
            //  設定接收字串資料文字方塊預設字型
            BaudRate = 9600;                                                    //  預設BaudRate數值為9600
            Parity_num = 0;                                                     //  預設Parity_num數值為0(無同位位元檢查)
            DataBits_num = 8;                                                   //  預設DataBits_num數值為8
            COM_Port_num = 0;                                                   //  預設COM_Port_num為0
            Array.Resize(ref ADC_Raw_Data_X, ADC_Raw_Data_Max);                 //  指派ADC_Raw_Data_X靜態整數陣列大小
            Array.Resize(ref ADC_Raw_Data_Y, ADC_Raw_Data_Max);                 //  指派ADC_Raw_Data_Y靜態整數陣列大小
            Array.Resize(ref ADC_Raw_Data_Z, ADC_Raw_Data_Max);                 //  指派ADC_Raw_Data_Z靜態整數陣列大小
            Transmission_Analysis_Queue = new Queue();                          //  初始化Transmission_Analysis_Queue(Uart通訊傳輸資料分析字元佇列)
            Data_Graphic_Queue_X = new Queue<int>();                            //  初始化Data_Graphic_Queue_X(X通道資料繪圖用整數型態佇列)
            Data_Graphic_Queue_Y = new Queue<int>();                            //  初始化Data_Graphic_Queue_Y(Y通道資料繪圖用整數型態佇列)
            Data_Graphic_Queue_Z = new Queue<int>();                            //  初始化Data_Graphic_Queue_Z(Z通道資料繪圖用整數型態佇列)
            label6.Text = "未連線";                                             //  顯示連線狀態為"未連線"
            Uart_comport_connected = false;                                     //  預設Uart_comport_connected值為False
            list_SerialPort();                                                  //  呼叫list_SerialPort副程式
        }                                                                       //  結束Form1_Load副程式
        private void button1_Click(object sender, EventArgs e)                  //  當按下"重新偵測SerialPort"按鈕
        {                                                                       //  進入button1_Click副程式
            button1_Click_Runtimes = button1_Click_Runtimes + 1;                //  遞增button1_Click_Runtimes變數
            list_SerialPort();                                                  //  呼叫list_SerialPort(偵測並列出已連線SerialPort)副程式
        }                                                                       //  結束button1_Click副程式
        private void button2_Click(object sender, EventArgs e)                  //  當按下"連線/中斷連線"按鈕
        {                                                                       //  進入button2_Click副程式
            button2_Click_Runtimes = button2_Click_Runtimes + 1;                //  遞增button2_Click_Runtimes變數
            button2.Enabled = false;                                            //  暫時關閉"連線/中斷連線"按鈕功能
            Uart_comport_handle(comboBox1.Text, BaudRate, Parity_num);          //  呼叫Uart_comport_handle副程式
        }                                                                       //  結束button2_Click副程式
        private void button3_Click(object sender, EventArgs e)                  //  當按下"清除"(button3)按鈕
        {                                                                       //  進入button3_Click副程式
            button3_Click_Runtimes = button3_Click_Runtimes + 1;                //  遞增button3_Click_Runtimes變數
            textBox1.Clear();                                                   //  清除textBox1(接收字串資料文字方塊)資料
        }                                                                       //  結束button3_Click副程式
        //  ToolStripMenuItem選單相關副程式
        private void 設定_傳輸設定ToolStripMenuItem_Click(object sender, EventArgs e)
        //  宣告設定_傳輸設定ToolStripMenuItem_Click副程式
        {                                                                       //  進入設定_傳輸設定ToolStripMenuItem_Click副程式
            Transmission_Setting_Click_Runtimes = Transmission_Setting_Click_Runtimes + 1;
            //  遞增Transmission_Setting_Click_Runtimes變數
            Form2 Transmission_Setting_form = new Form2();                      //  宣告transmission_setting_form代表Form2
            Transmission_Setting_form.Show();                                   //  顯示transmission_setting_form
        }                                                                       //  結束設定_傳輸設定ToolStripMenuItem_Click副程式

        private void 設定_介面設定ToolStripMenuItem_Click(object sender, EventArgs e)
        //  宣告設定_介面設定ToolStripMenuItem_Click副程式
        {                                                                       //  進入設定_介面設定ToolStripMenuItem_Click副程式
            User_Interface_Setting_Click_Runtimes = User_Interface_Setting_Click_Runtimes + 1;
            //  遞增User_Interface_Setting_Click_Runtimes變數
            Form3 User_Interface_Setting_form = new Form3();                    //  宣告User_Interface_Setting_form代表Form3
            User_Interface_Setting_form.Show();                                 //  顯示User_Interface_Setting_form
        }                                                                       //  結束設定_介面設定ToolStripMenuItem_Click副程式
        public void list_SerialPort()                                           //  偵測並列出已連線SerialPort副程式
        {                                                                       //  進入list_SerialPort副程式
            list_SerialPort_Runtimes = list_SerialPort_Runtimes + 1;            //  遞增list_SerialPort_Runtimes變數
            string[] ports = SerialPort.GetPortNames();                         //  偵測已連線的SerialPort並儲存結果至陣列ports
            comboBox1.Items.Clear();                                            //  清空下拉式選單所有項目
            if (ports.Length == 0)                                              //  若偵測不到任何已連線的SerialPort(ports.Length為0)
            {                                                                   //  進入if敘述
                Error_Code = 010001;                                            //  記錄Error_Code
                var warning = MessageBox.Show                                   //  顯示警告訊息
                (                                                               //  進入警告訊息MessageBox設定
                    "未偵測到任何已連接的SerialPort，Error_Code=" + Error_Code, //  顯示警告訊息"未偵測到任何已連接的SerialPort"，同時顯示Error_Code
                    "None of SerialPort",                                       //  顯示警告訊息標題為"None of SerialPort"
                    MessageBoxButtons.OK,                                       //  顯示"OK"按鈕
                    MessageBoxIcon.Warning                                      //  顯示警告標誌
                );                                                              //  結束警告訊息MessageBox設定
                button2.Enabled = false;                                        //  關閉"連線/中斷連線"按鈕功能
                textBox1.Enabled = false;                                       //  關閉textBox1(接收字串資料文字方塊)功能
                return;                                                         //  提早結束list_SerialPort副程式
            }                                                                   //  結束if敘述
            else                                                                //  若偵測到已連線的SerialPort
            {                                                                   //  進入else敘述
                COM_Port_num = ports.Length;                                    //  記錄已連線的SerialPort數量
                foreach (string port in ports)                                  //  依序處理每個已連線的SerialPort
                {                                                               //  進入foreach敘述
                    comboBox1.Items.Add(port);                                  //  以條列式選單(comboBox1)列出已連線的SerialPort
                }                                                               //  結束foreach敘述
                button2.Enabled = true;                                         //  開啟連線功能
                textBox1.Enabled = true;                                        //  開啟textBox1(接收字串資料文字方塊)功能
                return;                                                         //  結束list_SerialPort副程式
            }                                                                   //  結束else敘述
        }                                                                       //  結束list_SerialPort副程式
        public void Uart_comport_handle                                         //  串列埠連線處理Uart_comport_handle副程式
        (string comport_name,int Baud_Rate,int Parity_set)
        //  處理Uart_comport連線設定
        //  呼叫格式為Uart_comport_handle(comport名稱,連線鮑率,同位位元設定,)
        //  同位位元設定說明：0為不檢查(None),1為奇同位檢察,2為偶同位檢察,3為同位位元恆為1,4為同位位元恆為0
        {                                                                       //  進入Uart_comport_handle副程式
            Uart_comport_handle_Runtimes = Uart_comport_handle_Runtimes + 1;    //  遞增Uart_comport_handle_Runtimes變數
            if(Uart_comport_connected == true)                                  
            //  若Uart_comport_connected為True，代表Uart_comport連線中，將執行中斷連線
            {                                                                   //  進入if敘述
                label6.Text = (comport_name + "正在中斷連線");
                //  顯示連線狀態為(comport_name + "正在中斷連線")，如"COM1正在中斷連線"
                Uart_comport_connected = false;                                 //  更新Uart_comport_connected
                Uart_comport.Close();                                           //  關閉Uart_comport連線
                button2.Text = "連線";                                          //  更改button2文字為"連線"
                button2.Enabled = true;                                         //  重新開啟"連線/中斷連線"按鈕功能
                label6.Text = "未連線";                                         //  顯示連線狀態為"未連線"
                return;                                                         //  結束Uart_comport_handle副程式
            }                                                                   //  結束if敘述
            else                                                                //  若Uart_comport_connected為False，執行連線
            {                                                                   //  進入else敘述
                label6.Text = "偵測連接埠設定";                                 //  顯示連線狀態為"偵測連接埠設定"
                if (comport_name == "")                                         //  若comport_name為空白(Combobox1未選定)
                {                                                               //  進入if敘述
                    Error_Code = 010002;                                        //  記錄Error_Code
                    var ConnectError = MessageBox.Show                          //  顯示錯誤訊息
                     (                                                          //  進入錯誤訊息MessageBox設定
                        "未選定連接埠，Error_Code=" + Error_Code,               //  顯示錯誤訊息"未選定連接埠"，同時顯示Error_Code
                        "Connect Error",                                        //  顯示錯誤訊息標題為"Connect Error"
                        MessageBoxButtons.OK,                                   //  顯示"OK"按鈕
                        MessageBoxIcon.Error                                    //  顯示錯誤標誌
                    );                                                          //  結束錯誤訊息MessageBox設定
                    button2.Enabled = true;                                     //  重新開啟"連線/中斷連線"按鈕功能
                    return;                                                     //  結束Uart_comport_handle副程式
                }                                                               //  結束if敘述
                else                                                            //  已選定連接埠
                {                                                               //  進入else敘述
                    label6.Text = (comport_name + "正在嘗試連線");
                    //  顯示連線狀態為(comport_name + "正在嘗試連線")，如"COM1正在嘗試連線"
                    try                                                         //  嘗試以comport_name建立串列通訊連線
                    {                                                           //  進入try敘述
                        Uart_comport = new SerialPort(comport_name);            //  Uart_comport串列埠建立comport_name連線
                    }                                                           //  結束try敘述
                    catch (System.IO.IOException)                               //  當IO發生錯誤時的例外狀況
                    {                                                           //  進入catch敘述
                        Error_Code = 010003;                                    //  記錄Error_Code
                        var warning = MessageBox.Show                           //  顯示警告訊息
                        (                                                       //  顯示警告訊息MessageBox設定
                             comport_name + "裝置不存在或無法建立連線，Error_Code=" + Error_Code,
                             // 顯示錯誤訊息(連線裝置名稱)裝置不存在或無法建立連線
                             "Connect Error",                                   //  顯示錯誤訊息標題為"Connect Error"
                             MessageBoxButtons.OK,                              //  顯示"OK"按鈕
                             MessageBoxIcon.Warning                             //  顯示警告標誌
                        );                                                      //  結束警告訊息MessageBox設定
                        button2.Enabled = true;                                 //  重新開啟"連線/中斷連線"按鈕功能
                        return;                                                 //  結束Uart_comport_handle副程式
                    }                                                           //  結束catch敘述
                    Uart_comport.BaudRate = Baud_Rate;                          //  設定Uart_comport連線之BaudRate
                    Uart_comport.DataBits = DataBits_num;                       //  設定Uart_comport連線之DataBits
                    switch (Parity_set)                                         //  根據Parity_set變數進行同位位元設定
                    {                                                           //  進入switch-case敘述
                        case 0:                                                 //  若為case0(Parity_set為0)
                            Uart_comport.Parity = Parity.None;                  //  無同位元檢查
                            break;                                              //  結束case0敘述、跳出switch-case
                        case 1:                                                 //  若為case1(Parity_set為1)
                            Uart_comport.Parity = Parity.Odd;                   //  奇同位位元檢查
                            break;                                              //  結束case1敘述、跳出switch-case
                        case 2:                                                 //  若為case2(Parity_set為2)
                            Uart_comport.Parity = Parity.Even;                  //  偶同位位元檢查
                            break;                                              //  結束case2敘述、跳出switch-case
                        case 3:                                                 //  若為case3(Parity_set為3)
                            Uart_comport.Parity = Parity.Mark;                  //  將同位位元恆設為1
                            break;                                              //  結束case3敘述、跳出switch-case
                        case 4:                                                 //  若為case4(Parity_set為4)
                            Uart_comport.Parity = Parity.Space;                 //  將同位位元恆設為0
                            break;                                              //  結束case4敘述、跳出switch-case
                    }                                                           //  結束switch-case敘述
                    try                                                         //  以try方式執行資料接收
                    {                                                           //  進入try敘述
                        if (!Uart_comport.IsOpen)                               //  若Uart_comport未開啟
                        {                                                       //  進入if敘述
                            Uart_comport.Open();                                //  開啟Uart_comport
                            label6.Text = (comport_name + "已連線");
                            //  顯示連線狀態為(comport_name + "已連線")，如"COM1已連線"
                            Uart_comport_connected = true;                      //  更新Uart_comport_connected狀態
                            button2.Text = "中斷連線";                          //  更改button2文字為"中斷連線"
                            button2.Enabled = true;                             //  重新開啟"連線/中斷連線"按鈕功能
                        }                                                       //  結束if敘述
                        Uart_comport.DataReceived += new SerialDataReceivedEventHandler(comport_DataReceived);
                        //  處理資料接收
                    }                                                           //  結束try敘述
                    catch (Exception ex)                                        //  若發生錯誤狀況
                    {                                                           //  進入catch敘述
                        var result = MessageBox.Show                            //  顯示錯誤訊息
                            (                                                   //  進入錯誤訊息MessageBox設定
                                ex.ToString(), "DataReceived Error",            //  顯示錯誤訊息ex.ToString()，標題為"DataReceived Error"
                                MessageBoxButtons.OK,                           //  MessageBox選項為OK
                                MessageBoxIcon.Error                            //  顯示錯誤標誌
                            );                                                  //  結束錯誤訊息MessageBox設定
                        Uart_comport.Close();                                   //  關閉Uart_comport連線
                        button2.Text = "連線";                                  //  更改button2文字為"連線"
                        button2.Enabled = true;                                 //  重新開啟"連線/中斷連線"按鈕功能
                        return;                                                 //  結束Uart_comport_handle副程式
                    }                                                           //  結束catch敘述
                }                                                               //  結束else敘述
            }                                                                   //  結束else敘述
        }                                                                       //  結束Uart_comport_handle副程式        
        private void comport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //  comport資料接收處理副程式
        {                                                                       //  進入comport資料接收處理副程式
            comport_DataReceived_Runtimes = comport_DataReceived_Runtimes + 1;  //  遞增comport_DataReceived_Runtimes變數
            if (Uart_comport_connected == false)                                //  若Uart_comport未連線(Uart_comport_connected值為False)
            {                                                                   //  進入if敘述
                return;                                                         //  結束comport資料接收處理副程式
            }                                                                   //  結束if敘述
            byte[] buffer = new byte[1024];                                     //  宣告buffer暫存讀取資料，型態為Byte
            int length = 0;                                                     //  宣告length變數用於記錄串列資料緩衝區長度
            try                                                                 //  以try語法嘗試讀取Uart串列資料緩衝區
            {                                                                   //  進入try敘述
                length = (sender as SerialPort).Read(buffer, 0, buffer.Length); //  讀取Uart串列資料緩衝區
            }                                                                   //  結束try敘述
            catch (ArgumentNullException)                                       //  當發生NULL例外狀況
            {                                                                   //  進入catch敘述

            }                                                                   //  結束catch敘述
            Uart_Buffer_Size = length;                                          //  更新Uart_Buffer_Size數值
            try                                                                 //  嘗試調整buffer陣列大小
            {                                                                   //  進入try敘述
                Array.Resize(ref buffer, length);                               //  調整buffer陣列大小
            }                                                                   //  結束try敘述
            catch (ArgumentOutOfRangeException outOfRange)                      //  若調整buffer陣列空間時範圍錯誤
            {                                                                   //  進入catch敘述
                var result = MessageBox.Show                                    //  顯示錯誤訊息
                    (                                                           //  進入錯誤訊息MessageBox設定
                        outOfRange.Message,"",
                        MessageBoxButtons.OK,                                   //  MessageBox選項為OK
                        MessageBoxIcon.Error                                    //  顯示錯誤標誌
                    );                                                          //  結束錯誤訊息MessageBox設定
            }                                                                   //  結束catch敘述
            Display d = new Display(DisplayText);
            Invoke(d, new object[] { buffer });
            string message = BitConverter.ToString(buffer,0);                   //  取得Uart傳輸之Buffer資料(ASCII編碼值)
            Uart_Buffer_ASCII_Data = message;                                   //  將Uart傳輸之Buffer資料(ASCII編碼值)填入Uart_Buffer_ASCII_Data
        }                                                                       //  結束comport資料接收處理副程式
        private void DisplayText(byte[] buffer)                                 //  DisplayText顯示文字副程式
        {                                                                       //  進入DisplayText顯示文字副程式
            DisplayText_Runtimes = DisplayText_Runtimes + 1;                    //  遞增DisplayText_Runtimes變數
            textBox1.Font = textBox1_Font;                                      //  設定文字方塊顯示字型
            //textBox1.Text += String.Format("{0}{1}", BitConverter.ToString(buffer), Environment.NewLine);
            //  將buffer中之接收文字(ascii原始碼)顯示於textBox1上
            textBox1.Text += System.Text.Encoding.ASCII.GetString(buffer);
            //  將buffer中之接收資料(ascii原始碼)轉換為字串(符號字元)，並顯示於textBox1上
            Transmission_Buffer_CharArray = System.Text.Encoding.ASCII.GetString(buffer).ToCharArray();
            //  將buffer中之接收資料(ascii原始碼)轉換為字串(符號字元)後，再轉換為字元陣列(CharArray)，存入Transmission_Buffer_CharArray
            /*for (loop_num=0; loop_num<buffer.Length; loop_num++)
            {
                MessageBox.Show(Transmission_Buffer_CharArray[loop_num].ToString());
            }*/
            textBox1.ScrollBars = ScrollBars.Vertical;                          //  控制textBox1顯示垂直捲軸
            textBox1.SelectionStart = textBox1.Text.Length;                     //  控制textBox1游標位置為textBox1內容結束處
            textBox1.ScrollToCaret();                                           //  捲動textBox1捲軸至游標位置(自動捲動至最新資料)
            Total_Transmission_Length = Total_Transmission_Length + buffer.Length;
            //  累計接收字串長度
            Label2.Text = Total_Transmission_Length.ToString();                 //  顯示累計字串長度
            //Uart_Data_Analysis副程式與Uart_Data_Analysis_Queue副程式擇一使用
            Uart_Data_Analysis();                                               //  呼叫Uart_Data_Analysis副程式
            //Uart_Data_Analysis_Queue();                                         //  呼叫Uart_Data_Analysis_Queue副程式
        }                                                                       //  結束DisplayText顯示文字副程式
        public void Uart_Data_Analysis()
        //  宣告Uart_Data_Analysis副程式，分析Uart通訊傳輸接收資料
        /****使用全域變數：
        **Transmission_Analysis_CharArray(Uart通訊傳輸資料分析字元陣列，初始時為空陣列)、
        **Transmission_Buffer_CharArray(Uart通訊傳輸Buffer資料字元陣列，初始時為空陣列)、
        **Transmission_Analysis_CharArray_Datanum(已填入資料長度，初始值為0)、
        **Analysed_Length(已分析字串長度，初始值為-1)、
        **loop_num(迴圈用變數，初始值為0)、
        **ADC_Raw_Data_X(示波器功能實作變數，記錄X通道ADC原始資料，初始時為空陣列)
        **ADC_Raw_Data_Y(示波器功能實作變數，記錄Y通道ADC原始資料，初始時為空陣列)
        **ADC_Raw_Data_Z(示波器功能實作變數，記錄Z通道ADC原始資料，初始時為空陣列)
        **ADC_Raw_Data_X_num(示波器功能實作變數，記錄ADC_Raw_Data_X資料量，初始值為0)
        **ADC_Raw_Data_Y_num(示波器功能實作變數，記錄ADC_Raw_Data_Y資料量，初始值為0)
        **ADC_Raw_Data_Z_num(示波器功能實作變數，記錄ADC_Raw_Data_Z資料量，初始值為0)
        */
        {                                                                       //  進入Uart_Data_Analysis副程式
            char Channel_Name_temp = ' ';                                       //  宣告Channel_Name區域字元變數，暫存通道名稱
            string ADC_Data_temp = "";                                          //  宣告ADC_Data_temp區域字串變數，累計數值
            Array.Resize(ref Transmission_Analysis_CharArray,                   //  以Array.Resize函數重新配置Transmission_Analysis_CharArray字元陣列大小
            (Transmission_Analysis_CharArray_Datanum + Transmission_Buffer_CharArray.Length));
            //  指派Transmission_Analysis_CharArray字元陣列大小=
            //  已填入字元數(Transmission_Analysis_CharArray_Datanum)
            // +Buffer資料字元數(Transmission_Buffer_CharArray.Length)
            for (loop_num = Transmission_Analysis_CharArray_Datanum;            //  以for迴圈依序將Buffer資料填入Transmission_Analysis_CharArray
                loop_num < (Transmission_Analysis_CharArray_Datanum + Transmission_Buffer_CharArray.Length);
                loop_num++)
            {                                                                   //  進入for迴圈
                Transmission_Analysis_CharArray[loop_num] = Transmission_Buffer_CharArray[loop_num - Transmission_Analysis_CharArray_Datanum];
            }                                                                   //  結束for迴圈
            Transmission_Analysis_CharArray_Datanum += Transmission_Buffer_CharArray.Length;
            //  更新已填入字元數(Transmission_Analysis_CharArray_Datanum)
            //MessageBox.Show(Transmission_Analysis_CharArray[Analysed_Length + 1].ToString());
            //  顯示未分析的第一個字元
            for (loop_num= (Analysed_Length + 1);                               //  以for迴圈依序掃描每一個未分析字元，起始值設為(已分析字元數+1)
                loop_num < Transmission_Analysis_CharArray_Datanum;             //  掃描剩餘已填入字元
                loop_num++)                                                     //  遞增loop_num變數
            {                                                                   //  進入for迴圈
                //MessageBox.Show(Transmission_Analysis_CharArray[loop_num].ToString());
                //顯示當前分析字元
                if(Transmission_Analysis_CharArray[loop_num] == 'X' ||          //  若當前分析字元為X(通道名稱)
                   Transmission_Analysis_CharArray[loop_num] == 'Y' ||          //  或當前分析字元為Y(通道名稱)
                   Transmission_Analysis_CharArray[loop_num] == 'Z')            //  或當前分析字元為Z(通道名稱)
                {                                                               //  進入if敘述
                    if(Channel_Name_temp=='X')                                  //  若記錄下的Channel_Name為X
                    {                                                           //  進入if敘述
                        ADC_Raw_Data_X[ADC_Raw_Data_X_num] = int.Parse(ADC_Data_temp);
                        //  填入數值至ADC_Raw_Data_X
                        Data_Graphic_Queue_X.Enqueue(int.Parse(ADC_Data_temp)); //  填入數值至Data_Graphic_Queue_X(X通道資料繪圖用整數型態佇列)
                        ADC_Raw_Data_X_num = (ADC_Raw_Data_X_num + 1) % ADC_Raw_Data_Max;
                        //  遞增ADC_Raw_Data_X_num變數，並取餘數避免超出陣列大小
                    }                                                           //  結束if敘述
                    else if(Channel_Name_temp == 'Y')                           //  若記錄下的Channel_Name為Y
                    {                                                           //  進入else if敘述
                        ADC_Raw_Data_Y[ADC_Raw_Data_Y_num] = int.Parse(ADC_Data_temp);
                        //  填入數值至ADC_Raw_Data_Y
                        Data_Graphic_Queue_Y.Enqueue(int.Parse(ADC_Data_temp)); //  填入數值至Data_Graphic_Queue_Y(Y通道資料繪圖用整數型態佇列)
                        ADC_Raw_Data_Y_num = (ADC_Raw_Data_Y_num + 1) % ADC_Raw_Data_Max;
                        //  遞增ADC_Raw_Data_Y_num變數，並取餘數避免超出陣列大小
                    }                                                           //  結束else if敘述
                    else if (Channel_Name_temp == 'Z')                          //  若記錄下的Channel_Name為Z
                    {                                                           //  進入else if敘述
                        ADC_Raw_Data_Z[ADC_Raw_Data_Z_num] = int.Parse(ADC_Data_temp);
                        //  填入數值至ADC_Raw_Data_Z
                        Data_Graphic_Queue_Z.Enqueue(int.Parse(ADC_Data_temp)); //  填入數值至Data_Graphic_Queue_Z(Z通道資料繪圖用整數型態佇列)
                        ADC_Raw_Data_Z_num = (ADC_Raw_Data_Z_num + 1) % ADC_Raw_Data_Max;
                        //  遞增ADC_Raw_Data_Z_num變數，並取餘數避免超出陣列大小
                    }                                                           //  結束else if敘述
                    Channel_Name_temp = Transmission_Analysis_CharArray[loop_num];   
                    //  記錄下一筆通道名稱
                    ADC_Data_temp = "";                                         //  清空ADC_Data_temp暫存字串
                    Analysed_Length = loop_num - 1;                             //  更新Analysed_Length數值
                }                                                               //  結束if敘述
                else if(Transmission_Analysis_CharArray[loop_num] == '0' ||     //  若當前分析字元為數值
                        Transmission_Analysis_CharArray[loop_num] == '1' ||
                        Transmission_Analysis_CharArray[loop_num] == '2' ||
                        Transmission_Analysis_CharArray[loop_num] == '3' ||
                        Transmission_Analysis_CharArray[loop_num] == '4' ||
                        Transmission_Analysis_CharArray[loop_num] == '5' ||
                        Transmission_Analysis_CharArray[loop_num] == '6' ||
                        Transmission_Analysis_CharArray[loop_num] == '7' ||
                        Transmission_Analysis_CharArray[loop_num] == '8' ||
                        Transmission_Analysis_CharArray[loop_num] == '9' )
                {                                                               //  進入else if敘述
                    ADC_Data_temp += Transmission_Analysis_CharArray[loop_num]; //  累計數值字元
                }                                                               //  結束else if敘述
            }                                                                   //  結束for迴圈
            //Data_Graphic副程式與Data_Graphic_Queue副程式擇一使用
            //Data_Graphic();                                                     //  呼叫Data_Graphic副程式
            Data_Graphic_Queue();                                               //  呼叫Data_Graphic_Queue副程式
            //  顯示ADC_Raw_Data_X、ADC_Raw_Data_Y、ADC_Raw_Data_Z
            /*for (loop_num = 0;loop_num< ADC_Raw_Data_X_num; loop_num++)
            {
                MessageBox.Show("X[" + loop_num.ToString() + "]：" +
                    ADC_Raw_Data_X[loop_num].ToString());
                MessageBox.Show("Y[" + loop_num.ToString() + "]：" +
                    ADC_Raw_Data_Y[loop_num].ToString());
                MessageBox.Show("Z["+ loop_num.ToString()+"]：" + 
                    ADC_Raw_Data_Z[loop_num].ToString());
            }*/
        }                                                                       //  結束Uart_Data_Analysis副程式
        public void Uart_Data_Analysis_Queue()
        //  宣告Uart_Data_Analysis_Queue副程式，以Base Class Library中的Queue資料結構分析Uart通訊傳輸接收資料
        /****使用全域變數：
        **Transmission_Analysis_Queue(Uart通訊傳輸資料分析字元佇列，為全域靜態佇列)、
        **Transmission_Buffer_CharArray(Uart通訊傳輸Buffer資料字元陣列，初始時為空陣列)、
        **loop_num(迴圈用變數，初始值為0)
        */
        {                                                                       //  進入Uart_Data_Analysis_Queue副程式
            //Transmission_Analysis_Queue.Enqueue( "Test" );                      //  將"Test"資料放入佇列測試
            //MessageBox.Show(Transmission_Analysis_Queue.Dequeue().ToString());  //  將資料取出佇列並顯示測試
            for (loop_num = 0; loop_num < Transmission_Buffer_CharArray.Length; loop_num++)
            //  以for迴圈依序將Buffer資料填入Transmission_Analysis_Queue
            {                                                                   //  進入for迴圈
                Transmission_Analysis_Queue.Enqueue( Transmission_Buffer_CharArray[loop_num] );
                //將Buffer資料填入Transmission_Analysis_Queue
            }                                                                   //  結束for迴圈
            //MessageBox.Show(Transmission_Analysis_Queue.Peek().ToString());     //  資料顯示(不取出)測試
            //MessageBox.Show(Transmission_Analysis_Queue.Count.ToString());    //  顯示Transmission_Analysis_Queue元素個數
            /*以while迴圈將Transmission_Analysis_Queue中的元素全部取出
            while (Transmission_Analysis_Queue.Count > 0)                       //  若Transmission_Analysis_Queue元素個數大於0
            {                                                                   //  進入while迴圈
                MessageBox.Show(Transmission_Analysis_Queue.Dequeue().ToString());
                //  將資料取出佇列並顯示
            }                                                                   //  結束while迴圈
            */
            //MessageBox.Show(Transmission_Analysis_Queue.Dequeue().GetType().ToString());
            //顯示Transmission_Analysis_Queue佇列內容資料型態
            char Analysis_Char;                                                 //  宣告當前分析字元區域變數Analysis_Char
            char Channel_Name_temp = ' ';                                       //  宣告Channel_Name區域字元變數，暫存通道名稱
            string ADC_Data_temp;                                               //  宣告ADC_Data_temp區域字串變數，累計數值
            Boolean flag = true;                                                //  宣告旗標變數flag，決定字元分析是否執行
            while(flag)                                                         //  由flag旗標決定字元分析是否執行
            {                                                                   //  進入while迴圈
                ADC_Data_temp = "";                                             //  初始化ADC_Data_temp字串
                Analysis_Char = Convert.ToChar(Transmission_Analysis_Queue.Peek());
                //  讀取佇列資料(不移除)填入當前分析字元變數
                //MessageBox.Show(Analysis_Char.ToString());                      //  顯示當前分析字元
                if (Analysis_Char == 'X' && Transmission_Analysis_Queue.Contains('Y'))
                //  若當前分析字元為X，且佇列中存在字元'Y'，代表該筆X資料完整
                {                                                               //  進入if敘述
                    Channel_Name_temp = Convert.ToChar(Transmission_Analysis_Queue.Dequeue());
                    //由佇列Transmission_Analysis_Queue取出字元填入Channel_Name_temp(通道名稱暫存變數)
                    while (Convert.ToChar(Transmission_Analysis_Queue.Peek()) != 'Y')
                    //  若佇列資料不為字元Y，則取出資料填入ADC_Data_temp
                    {                                                           //  進入while迴圈
                        ADC_Data_temp += Convert.ToChar(Transmission_Analysis_Queue.Dequeue());
                    }                                                           //  結束while迴圈
                    ADC_Raw_Data_X[ADC_Raw_Data_X_num] = int.Parse(ADC_Data_temp);
                    //  填入數值至ADC_Raw_Data_X
                    Data_Graphic_Queue_X.Enqueue(int.Parse(ADC_Data_temp));     //  填入數值至Data_Graphic_Queue_X(X通道資料繪圖用整數型態佇列)
                    ADC_Raw_Data_X_num = (ADC_Raw_Data_X_num + 1) % ADC_Raw_Data_Max;
                    //  遞增ADC_Raw_Data_X_num變數，並取餘數避免超出陣列大小
                }                                                               //  結束if敘述
                else if (Analysis_Char == 'Y' && Transmission_Analysis_Queue.Contains('Z'))
                //  若當前分析字元為Y，且佇列中存在字元'Z'，代表該筆Y資料完整
                {                                                               //  進入else if敘述
                    Channel_Name_temp = Convert.ToChar(Transmission_Analysis_Queue.Dequeue());
                    //由佇列Transmission_Analysis_Queue取出字元填入Channel_Name_temp(通道名稱暫存變數)
                    while (Convert.ToChar(Transmission_Analysis_Queue.Peek()) != 'Z')
                    //  若佇列資料不為字元Z，則取出資料填入ADC_Data_temp
                    {                                                           //  進入while迴圈
                        ADC_Data_temp += Convert.ToChar(Transmission_Analysis_Queue.Dequeue());
                    }                                                           //  結束while迴圈
                    ADC_Raw_Data_Y[ADC_Raw_Data_Y_num] = int.Parse(ADC_Data_temp);
                    //  填入數值至ADC_Raw_Data_Y
                    Data_Graphic_Queue_Y.Enqueue(int.Parse(ADC_Data_temp));     //  填入數值至Data_Graphic_Queue_Y(Y通道資料繪圖用整數型態佇列)
                    ADC_Raw_Data_Y_num = (ADC_Raw_Data_Y_num + 1) % ADC_Raw_Data_Max;
                    //  遞增ADC_Raw_Data_Y_num變數，並取餘數避免超出陣列大小
                }                                                               //  結束else if敘述
                else if (Analysis_Char == 'Z' && Transmission_Analysis_Queue.Contains('X'))
                //  若當前分析字元為Z，且佇列中存在字元'X'，代表該筆Z資料完整
                {                                                               //  進入else if敘述
                    Channel_Name_temp = Convert.ToChar(Transmission_Analysis_Queue.Dequeue());
                    //由佇列Transmission_Analysis_Queue取出字元填入Channel_Name_temp(通道名稱暫存變數)
                    while (Convert.ToChar(Transmission_Analysis_Queue.Peek()) != 'X')
                    //  若佇列資料不為字元X，則取出資料填入ADC_Data_temp
                    {                                                           //  進入while迴圈
                        ADC_Data_temp += Convert.ToChar(Transmission_Analysis_Queue.Dequeue());
                    }                                                           //  結束while迴圈
                    ADC_Raw_Data_Z[ADC_Raw_Data_Z_num] = int.Parse(ADC_Data_temp);
                    //  填入數值至ADC_Raw_Data_Z
                    Data_Graphic_Queue_Z.Enqueue(int.Parse(ADC_Data_temp));     //  填入數值至Data_Graphic_Queue_Z(Z通道資料繪圖用整數型態佇列)
                    ADC_Raw_Data_Z_num = (ADC_Raw_Data_Z_num + 1) % ADC_Raw_Data_Max;
                    //  遞增ADC_Raw_Data_Z_num變數，並取餘數避免超出陣列大小
                }                                                               //  結束else if敘述
                else                                                            //  若當前分析字元非X、Y、Z
                {                                                               //  進入else敘述
                    flag = false;                                               //  結束字元分析
                }                                                               //  結束else敘述
            }                                                                   //  結束while迴圈
            //  顯示ADC_Raw_Data_X、ADC_Raw_Data_Y、ADC_Raw_Data_Z
            /*for (loop_num = 0;loop_num< ADC_Raw_Data_X_num; loop_num++)
            {
                MessageBox.Show("X[" + loop_num.ToString() + "]：" +
                    ADC_Raw_Data_X[loop_num].ToString());
                MessageBox.Show("Y[" + loop_num.ToString() + "]：" +
                    ADC_Raw_Data_Y[loop_num].ToString());
                MessageBox.Show("Z["+ loop_num.ToString()+"]：" + 
                    ADC_Raw_Data_Z[loop_num].ToString());
            }*/
            //Data_Graphic副程式與Data_Graphic_Queue副程式擇一使用
            //Data_Graphic();                                                     //  呼叫Data_Graphic副程式
            Data_Graphic_Queue();                                               //  呼叫Data_Graphic_Queue副程式
        }                                                                       //  結束Uart_Data_Analysis_Queue副程式
        public void Data_Graphic()                                              //  宣告Data_Graphic(資料繪圖副程式)
        {                                                                       //  進入Data_Graphic副程式
            /****使用全域變數：
            **ADC_Raw_Data_X(記錄X通道ADC原始資料)、
            **ADC_Raw_Data_Y(記錄Y通道ADC原始資料)、
            **ADC_Raw_Data_Z(記錄Z通道ADC原始資料)、
            **ADC_Raw_Data_X_num(記錄ADC_Raw_Data_X資料量)、
            **ADC_Raw_Data_Y_num(記錄ADC_Raw_Data_Y資料量)、
            **ADC_Raw_Data_Z_num(記錄ADC_Raw_Data_Z資料量)、
            **ADC_Raw_Data_Max(記錄ADC_Raw_Data陣列大小)、
            **loop_num(迴圈用變數)
            */
            //***區域變數宣告***
            Graphics Oscilloscope_Graphic;                                      //  宣告Oscilloscope_Graphic示波器波型繪圖區
            Pen pen_Frame;                                                      //  宣告外框畫筆
            Pen pen_Grid_Line;                                                  //  宣告繪圖網格線畫筆
            Pen pen_X;                                                          //  宣告X通道畫筆
            Pen pen_Y;                                                          //  宣告Y通道畫筆
            Pen pen_Z;                                                          //  宣告Z通道畫筆
            //Rectangle rectangle_point = 
            //    new Rectangle(5, 5, 5, 5);
            //***指定畫布***
            Oscilloscope_Graphic = panel1.CreateGraphics();                     //  取得繪圖區物件
            //***設定畫筆屬性***
            pen_Frame = new Pen(Color.Black, 3);                                //  設定外框畫筆為黑色、粗細為3點
            pen_Grid_Line = new Pen(Color.Gray);                                //  設定格線繪製畫筆屬性
            pen_X = new Pen(Color.Red, 2);                                      //  設定X通道折線圖畫筆屬性
            pen_Y = new Pen(Color.Green, 2);                                    //  設定Y通道折線圖畫筆屬性
            pen_Z = new Pen(Color.Blue, 2);                                     //  設定Z通道折線圖畫筆屬性
            Oscilloscope_Graphic.Clear(Color.White);                            //  清除畫布背景為白色
            //***繪製外框***
            Oscilloscope_Graphic.DrawLine(pen_Frame,                            //  以pen_Frame畫筆繪製上框線
                  new Point(0, 0),                                              //  以(0, 0)為起始點
                  new Point(panel1.Size.Width, 0));                             //  以(panel1.Size.Width, 0)為結束點
            Oscilloscope_Graphic.DrawLine(pen_Frame,                            //  以pen_Frame畫筆繪製左框線
                  new Point(0, 0),                                              //  以(0, 0)為起始點
                  new Point(0, panel1.Size.Height));                            //  以(0, panel1.Size.Height)為結束點
            Oscilloscope_Graphic.DrawLine(pen_Frame,                            //  以pen_Frame畫筆繪製下框線
                  new Point(0, panel1.Size.Height),                             //  以(0, panel1.Size.Height)為起點
                  new Point(panel1.Size.Width, panel1.Size.Height));            //  以(panel1.Size.Width, panel1.Size.Height)為結束點
            Oscilloscope_Graphic.DrawLine(pen_Frame,                            //  以pen_Frame畫筆繪製右框線
                  new Point(panel1.Size.Width, 0),                              //  以(panel1.Size.Width, 0)為起始點
                  new Point(panel1.Size.Width, panel1.Size.Height));            //  以(panel1.Size.Width, panel1.Size.Height)為結束點
            //***水平格線繪製***
            for (loop_num=0;loop_num<10;loop_num++)                             //  以for迴圈依序繪製水平格線
            {                                                                   //  進入for迴圈
                Oscilloscope_Graphic.DrawLine(pen_Grid_Line,                    //  以pen_Grid_Line畫筆繪製水平格線
                new Point(0, panel1.Size.Height * loop_num / 10),               //  以(0, panel1.Size.Height * loop_num / 10)為起始點
                new Point(panel1.Size.Width, panel1.Size.Height * loop_num / 10));
                //以(panel1.Size.Width, panel1.Size.Height * loop_num / 10)為結束點
            }                                                                   //  結束for迴圈
            //***垂直格線繪製***
            for (loop_num = 0; loop_num < 10; loop_num++)                       //  以for迴圈依序繪製垂直格線
            {                                                                   //  進入for迴圈
                Oscilloscope_Graphic.DrawLine(pen_Grid_Line,                    //  以pen_Grid_Line畫筆繪製垂直格線
                new Point(panel1.Size.Width * loop_num / 10, 0),                //  以(panel1.Size.Width * loop_num / 10, 0)為起始點
                new Point(panel1.Size.Width * loop_num / 10, panel1.Size.Height));
                //以(panel1.Size.Width * loop_num / 10, panel1.Size.Height)為結束點
            }                                                                   //  結束for迴圈
            //***ADC_Raw_Data資料繪製折線圖***
            for (loop_num = 0; loop_num < (ADC_Raw_Data_X_num - 1); loop_num++)
            //X通道資料折線圖繪製
            {                                                                   //  進入for迴圈
                Oscilloscope_Graphic.DrawLine(pen_X,
                new Point( (loop_num * panel1.Size.Width / ADC_Raw_Data_Max) ,
                            panel1.Size.Height - (ADC_Raw_Data_X[loop_num] * panel1.Size.Height / 4096)),
                new Point( ( (loop_num + 1) * panel1.Size.Width / ADC_Raw_Data_Max) ,
                            panel1.Size.Height - (ADC_Raw_Data_X[loop_num + 1] * panel1.Size.Height / 4096) ));
                //rectangle_point = new Rectangle(((loop_num + 1) * panel1.Size.Width / ADC_Raw_Data_Max) ,
                //                  panel1.Size.Height - (ADC_Raw_Data_X[loop_num + 1] * panel1.Size.Height / 4096) ,
                //                  3, 3);
                //Oscilloscope_Graphic.DrawRectangle(Pens.Red, rectangle_point);
            }                                                                   //  結束for迴圈
            for (loop_num = 0; loop_num < (ADC_Raw_Data_Y_num - 1); loop_num++)
            //Y通道資料折線圖繪製
            {                                                                   //  進入for迴圈
                Oscilloscope_Graphic.DrawLine(pen_Y,
                new Point((loop_num * panel1.Size.Width / ADC_Raw_Data_Max),
                            panel1.Size.Height - (ADC_Raw_Data_Y[loop_num] * panel1.Size.Height / 4096)),
                new Point(((loop_num + 1) * panel1.Size.Width / ADC_Raw_Data_Max),
                            panel1.Size.Height - (ADC_Raw_Data_Y[loop_num + 1] * panel1.Size.Height / 4096)));
            }                                                                   //  結束for迴圈
            for (loop_num = 0; loop_num < (ADC_Raw_Data_Z_num - 1); loop_num++)
            //Z通道資料折線圖繪製
            {                                                                   //  進入for迴圈
                Oscilloscope_Graphic.DrawLine(pen_Z,
                new Point((loop_num * panel1.Size.Width / ADC_Raw_Data_Max),
                            panel1.Size.Height - (ADC_Raw_Data_Z[loop_num] * panel1.Size.Height / 4096)),
                new Point(((loop_num + 1) * panel1.Size.Width / ADC_Raw_Data_Max),
                            panel1.Size.Height - (ADC_Raw_Data_Z[loop_num + 1] * panel1.Size.Height / 4096)));
            }                                                                   //  結束for迴圈
        }                                                                       //  結束Data_Graphic副程式
        public void Data_Graphic_Queue()                                        //  宣告Data_Graphic_Queue(以佇列結構實作之資料繪圖副程式)
        {                                                                       //  進入Data_Graphic_Queue副程式
            /****使用全域變數：
            **Data_Graphic_Queue_X(X通道資料繪圖用整數型態佇列)、
            **Data_Graphic_Queue_Y(Y通道資料繪圖用整數型態佇列)、
            **Data_Graphic_Queue_Z(Z通道資料繪圖用整數型態佇列)、
            **loop_num(迴圈用變數)
            */
            //***區域變數宣告***
            Graphics Oscilloscope_Graphic;                                      //  宣告Oscilloscope_Graphic示波器波型繪圖區
            Pen pen_Frame;                                                      //  宣告外框畫筆
            Pen pen_Grid_Line;                                                  //  宣告繪圖網格線畫筆
            Pen pen_X;                                                          //  宣告X通道畫筆
            Pen pen_Y;                                                          //  宣告Y通道畫筆
            Pen pen_Z;                                                          //  宣告Z通道畫筆
            int Data_Graphic_Queue_Count_Max = 100;                             //  宣告Data_Graphic_Queue_Count_Max整數區域變數，記錄繪圖用整數型態佇列元素數量最大值
            Point Graph_point_temp = new Point();                               //  宣告Graph_point_temp(繪圖暫存座標)
            //***指定畫布***
            Oscilloscope_Graphic = panel1.CreateGraphics();                     //  取得繪圖區物件
            //***設定畫筆屬性***
            pen_Frame = new Pen(Color.Black, 3);                                //  設定外框畫筆為黑色、粗細為3點
            pen_Grid_Line = new Pen(Color.Gray);                                //  設定格線繪製畫筆屬性
            pen_X = new Pen(Color.Red, 2);                                      //  設定X通道折線圖畫筆屬性
            pen_Y = new Pen(Color.Green, 2);                                    //  設定Y通道折線圖畫筆屬性
            pen_Z = new Pen(Color.Blue, 2);                                     //  設定Z通道折線圖畫筆屬性
            Oscilloscope_Graphic.Clear(Color.White);                            //  清除畫布背景為白色
            //***繪製外框***
            Oscilloscope_Graphic.DrawLine(pen_Frame,                            //  以pen_Frame畫筆繪製上框線
                  new Point(0, 0),                                              //  以(0, 0)為起始點
                  new Point(panel1.Size.Width, 0));                             //  以(panel1.Size.Width, 0)為結束點
            Oscilloscope_Graphic.DrawLine(pen_Frame,                            //  以pen_Frame畫筆繪製左框線
                  new Point(0, 0),                                              //  以(0, 0)為起始點
                  new Point(0, panel1.Size.Height));                            //  以(0, panel1.Size.Height)為結束點
            Oscilloscope_Graphic.DrawLine(pen_Frame,                            //  以pen_Frame畫筆繪製下框線
                  new Point(0, panel1.Size.Height),                             //  以(0, panel1.Size.Height)為起點
                  new Point(panel1.Size.Width, panel1.Size.Height));            //  以(panel1.Size.Width, panel1.Size.Height)為結束點
            Oscilloscope_Graphic.DrawLine(pen_Frame,                            //  以pen_Frame畫筆繪製右框線
                  new Point(panel1.Size.Width, 0),                              //  以(panel1.Size.Width, 0)為起始點
                  new Point(panel1.Size.Width, panel1.Size.Height));            //  以(panel1.Size.Width, panel1.Size.Height)為結束點
            //***水平格線繪製***
            for (loop_num = 0; loop_num < 10; loop_num++)                       //  以for迴圈依序繪製水平格線
            {                                                                   //  進入for迴圈
                Oscilloscope_Graphic.DrawLine(pen_Grid_Line,                    //  以pen_Grid_Line畫筆繪製水平格線
                new Point(0, panel1.Size.Height * loop_num / 10),               //  以(0, panel1.Size.Height * loop_num / 10)為起始點
                new Point(panel1.Size.Width, panel1.Size.Height * loop_num / 10));
                //以(panel1.Size.Width, panel1.Size.Height * loop_num / 10)為結束點
            }                                                                   //  結束for迴圈
            //***垂直格線繪製***
            for (loop_num = 0; loop_num < 10; loop_num++)                       //  以for迴圈依序繪製垂直格線
            {                                                                   //  進入for迴圈
                Oscilloscope_Graphic.DrawLine(pen_Grid_Line,                    //  以pen_Grid_Line畫筆繪製垂直格線
                new Point(panel1.Size.Width * loop_num / 10, 0),                //  以(panel1.Size.Width * loop_num / 10, 0)為起始點
                new Point(panel1.Size.Width * loop_num / 10, panel1.Size.Height));
                //以(panel1.Size.Width * loop_num / 10, panel1.Size.Height)為結束點
            }                                                                   //  結束for迴圈
            //***以while迴圈清除顯示佇列過多資料***
            while (Data_Graphic_Queue_X.Count > Data_Graphic_Queue_Count_Max)   //  以while迴圈依序移除Data_Graphic_Queue_X佇列過多的資料
            {                                                                   //  進入while迴圈
                Data_Graphic_Queue_X.Dequeue();                                 //  移除Data_Graphic_Queue_X佇列資料
            }                                                                   //  結束while迴圈
            while (Data_Graphic_Queue_Y.Count > Data_Graphic_Queue_Count_Max)   //  以while迴圈依序移除Data_Graphic_Queue_Y佇列過多的資料
            {                                                                   //  進入while迴圈
                Data_Graphic_Queue_Y.Dequeue();                                 //  移除Data_Graphic_Queue_Y佇列資料
            }                                                                   //  結束while迴圈
            while (Data_Graphic_Queue_Z.Count > Data_Graphic_Queue_Count_Max)   //  以while迴圈依序移除Data_Graphic_Queue_Z佇列過多的資料
            {                                                                   //  進入while迴圈
                Data_Graphic_Queue_Z.Dequeue();                                 //  移除Data_Graphic_Queue_Z佇列資料
            }                                                                   //  結束while迴圈
            //***依序繪製資料***
            loop_num = 0;
            foreach (int ADC_Data in Data_Graphic_Queue_X)                      //  依序以Data_Graphic_Queue_X資料繪圖
            {                                                                   //  進入foreach敘述
                //MessageBox.Show(ADC_Data.ToString());                           //  依序顯示Data_Graphic_Queue_X佇列資料
                if(loop_num==0)                                                 //  若為佇列中第一筆資料
                {                                                               //  進入if敘述
                    Graph_point_temp = new Point(                               //  指定Graph_point_temp座標
                    (loop_num * panel1.Size.Width / Data_Graphic_Queue_Count_Max),
                    panel1.Size.Height - (ADC_Data * panel1.Size.Height / 4096));
                    //  填入座標  
                }                                                               //  結束if敘述
                else                                                            //  若不為佇列中第一筆資料
                {                                                               //  進入else敘述
                    Oscilloscope_Graphic.DrawLine(pen_X, Graph_point_temp,
                        new Point((loop_num * panel1.Size.Width / Data_Graphic_Queue_Count_Max),
                            panel1.Size.Height - (ADC_Data * panel1.Size.Height / 4096) ));
                    Graph_point_temp = new Point(                               //  指定Graph_point_temp座標
                    (loop_num * panel1.Size.Width / Data_Graphic_Queue_Count_Max),
                    panel1.Size.Height - (ADC_Data * panel1.Size.Height / 4096));
                    //  填入座標  
                }                                                               //  結束else敘述
                loop_num = loop_num + 1;                                        //  遞增loop_num變數
            }                                                                   //  結束foreach敘述
            loop_num = 0;
            foreach (int ADC_Data in Data_Graphic_Queue_Y)                      //  依序以Data_Graphic_Queue_Y資料繪圖
            {                                                                   //  進入foreach敘述
                //MessageBox.Show(ADC_Data.ToString());                           //  依序顯示Data_Graphic_Queue_Y佇列資料
                if (loop_num == 0)                                              //  若為佇列中第一筆資料
                {                                                               //  進入if敘述
                    Graph_point_temp = new Point(                               //  指定Graph_point_temp座標
                    (loop_num * panel1.Size.Width / Data_Graphic_Queue_Count_Max),
                    panel1.Size.Height - (ADC_Data * panel1.Size.Height / 4096));
                    //  填入座標  
                }                                                               //  結束if敘述
                else                                                            //  若不為佇列中第一筆資料
                {                                                               //  進入else敘述
                    Oscilloscope_Graphic.DrawLine(pen_Y, Graph_point_temp,
                        new Point((loop_num * panel1.Size.Width / Data_Graphic_Queue_Count_Max),
                            panel1.Size.Height - (ADC_Data * panel1.Size.Height / 4096) ));
                    Graph_point_temp = new Point(                               //  指定Graph_point_temp座標
                    (loop_num * panel1.Size.Width / Data_Graphic_Queue_Count_Max),
                    panel1.Size.Height - (ADC_Data * panel1.Size.Height / 4096));
                    //  填入座標  
                }                                                               //  結束else敘述
                loop_num = loop_num + 1;                                        //  遞增loop_num變數
            }                                                                   //  結束foreach敘述
            loop_num = 0;
            foreach (int ADC_Data in Data_Graphic_Queue_Z)                      //  依序以Data_Graphic_Queue_Z資料繪圖
            {                                                                   //  進入foreach敘述
                //MessageBoZ.Show(ADC_Data.ToString());                           //  依序顯示Data_Graphic_Queue_Z佇列資料
                if (loop_num == 0)                                              //  若為佇列中第一筆資料
                {                                                               //  進入if敘述
                    Graph_point_temp = new Point(                               //  指定Graph_point_temp座標
                    (loop_num * panel1.Size.Width / Data_Graphic_Queue_Count_Max),
                    panel1.Size.Height - (ADC_Data * panel1.Size.Height / 4096));
                    //  填入座標  
                }                                                               //  結束if敘述
                else                                                            //  若不為佇列中第一筆資料
                {                                                               //  進入else敘述
                    Oscilloscope_Graphic.DrawLine(pen_Z, Graph_point_temp,
                        new Point((loop_num * panel1.Size.Width / Data_Graphic_Queue_Count_Max),
                            panel1.Size.Height - (ADC_Data * panel1.Size.Height / 4096) ));
                    Graph_point_temp = new Point(                               //  指定Graph_point_temp座標
                    (loop_num * panel1.Size.Width / Data_Graphic_Queue_Count_Max),
                    panel1.Size.Height - (ADC_Data * panel1.Size.Height / 4096));
                    //  填入座標  
                }                                                               //  結束else敘述
                loop_num = loop_num + 1;                                        //  遞增loop_num變數
            }                                                                   //  結束foreach敘述
            //Data_Graphic_OpenGL副程式與Data_Graphic_Queue_OpenGL副程式擇一使用
            //Data_Graphic_OpenGL();                                              //  呼叫Data_Graphic_OpenGL(OpenGL資料繪圖)副程式
            Data_Graphic_Queue_OpenGL();                                        //  呼叫Data_Graphic_Queue_OpenGL(OpenGL佇列資料繪圖)副程式
        }                                                                       //  結束Data_Graphic_Queue副程式
        public void Data_Graphic_OpenGL()                                       //  宣告Data_Graphic_OpenGL(OpenGL資料繪圖副程式)
        {                                                                       //  進入Data_Graphic_OpenGL副程式
            /****使用全域變數：
            **ADC_Raw_Data_X(記錄X通道ADC原始資料)、
            **ADC_Raw_Data_Y(記錄Y通道ADC原始資料)、
            **ADC_Raw_Data_Z(記錄Z通道ADC原始資料)、
            **ADC_Raw_Data_X_num(記錄ADC_Raw_Data_X資料量)、
            **ADC_Raw_Data_Y_num(記錄ADC_Raw_Data_Y資料量)、
            **ADC_Raw_Data_Z_num(記錄ADC_Raw_Data_Z資料量)、
            **ADC_Raw_Data_Max(記錄ADC_Raw_Data陣列大小)、
            **loop_num(迴圈用變數)
            */
            //***區域變數宣告***
            
            //***指定畫布***
            OpenGL Gsensor_GL = openGLControl1.OpenGL;                          //  取得OpenGL繪圖物件
            Gsensor_GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            //  清除深度緩存
            Gsensor_GL.LoadIdentity();
            //  重置模型觀察矩陣，並將其初始化
            Gsensor_GL.Translate(0f, 0f, -2.5f);                                  //  轉移繪圖中心座標
            //***繪製外框***
            Gsensor_GL.Color(1.0f, 1.0f, 1.0f);                                 //  設定繪圖顏色
            Gsensor_GL.Begin(OpenGL.GL_LINE_LOOP);                              //  繪製封閉直線線段
            Gsensor_GL.Vertex(1, 1, 0);
            Gsensor_GL.Vertex(1, -1, 0);
            Gsensor_GL.Vertex(-1, -1, 0);
            Gsensor_GL.Vertex(-1, 1, 0);
            Gsensor_GL.End();                                                   //  結束繪製外框
            //***水平格線繪製***
            for (loop_num = -5; loop_num <= 5; loop_num++)                      //  以for迴圈依序繪製水平格線
            {                                                                   //  進入for迴圈
                Gsensor_GL.Begin(OpenGL.GL_LINES);
                Gsensor_GL.Vertex(-1, ((float)loop_num / 5), 0);
                Gsensor_GL.Vertex(1, ((float)loop_num / 5), 0);
                Gsensor_GL.End();
            }                                                                   //  結束for迴圈
            //***垂直格線繪製***
            for (loop_num = -5; loop_num <= 5; loop_num++)                      //  以for迴圈依序繪製垂直格線
            {                                                                   //  進入for迴圈
                Gsensor_GL.Begin(OpenGL.GL_LINES);
                Gsensor_GL.Vertex(((float)loop_num / 5), -1, 0);
                Gsensor_GL.Vertex(((float)loop_num / 5), 1, 0);
                Gsensor_GL.End();
            }                                                                   //  結束for迴圈
            //***ADC_Raw_Data資料繪製折線圖***
            Gsensor_GL.Color(1.0f, 0.0f, 0.0f);                                 //  設定繪圖顏色
            for (loop_num = 0; loop_num < (ADC_Raw_Data_X_num - 1); loop_num++)
            //X通道資料折線圖繪製
            {                                                                   //  進入for迴圈
                Gsensor_GL.Begin(OpenGL.GL_LINES);
                Gsensor_GL.Vertex(  ( (float)loop_num / ADC_Raw_Data_Max) * 2 - 1,
                                    ( ((double)ADC_Raw_Data_X[loop_num] / 4096) ) * 2 - 1,
                                    0);
                Gsensor_GL.Vertex(  ( ((float)loop_num + 1) / ADC_Raw_Data_Max) * 2 - 1,
                                    ( ((double)ADC_Raw_Data_X[loop_num + 1] / 4096)) * 2 - 1,
                                    0);
                Gsensor_GL.End();                                               //  結束線段繪製
            }                                                                   //  結束for迴圈
            Gsensor_GL.Color(0.0f, 1.0f, 0.0f);                                 //  設定繪圖顏色
            for (loop_num = 0; loop_num < (ADC_Raw_Data_Y_num - 1); loop_num++)
            //X通道資料折線圖繪製
            {                                                                   //  進入for迴圈
                Gsensor_GL.Begin(OpenGL.GL_LINES);
                Gsensor_GL.Vertex(((float)loop_num / ADC_Raw_Data_Max) * 2 - 1,
                                    (((double)ADC_Raw_Data_Y[loop_num] / 4096)) * 2 - 1,
                                    0);
                Gsensor_GL.Vertex((((float)loop_num + 1) / ADC_Raw_Data_Max) * 2 - 1,
                                    (((double)ADC_Raw_Data_Y[loop_num + 1] / 4096)) * 2 - 1,
                                    0);
                Gsensor_GL.End();                                               //  結束線段繪製
            }                                                                   //  結束for迴圈
            Gsensor_GL.Color(0.0f, 0.0f, 1.0f);                                 //  設定繪圖顏色
            for (loop_num = 0; loop_num < (ADC_Raw_Data_Z_num - 1); loop_num++)
            //X通道資料折線圖繪製
            {                                                                   //  進入for迴圈
                Gsensor_GL.Begin(OpenGL.GL_LINES);
                Gsensor_GL.Vertex(((float)loop_num / ADC_Raw_Data_Max) * 2 - 1,
                                    (((double)ADC_Raw_Data_Z[loop_num] / 4096)) * 2 - 1,
                                    0);
                Gsensor_GL.Vertex((((float)loop_num + 1) / ADC_Raw_Data_Max) * 2 - 1,
                                    (((double)ADC_Raw_Data_Z[loop_num + 1] / 4096)) * 2 - 1,
                                    0);
                Gsensor_GL.End();                                               //  結束線段繪製
            }                                                                   //  結束for迴圈
            Gsensor_GL.Flush();                                                 //  強制更新繪圖
        }                                                                       //  結束Data_Graphic_OpenGL副程式
        public void Data_Graphic_Queue_OpenGL()                                 //  宣告Data_Graphic_Queue_OpenGL(OpenGL佇列資料繪圖副程式)
        {                                                                       //  進入Data_Graphic_Queue_OpenGL副程式
             /****使用全域變數：
            **OpenGL_Graph_point結構(OpenGL座標宣告)
            **Data_Graphic_Queue_X(X通道資料繪圖用整數型態佇列)、
            **Data_Graphic_Queue_Y(Y通道資料繪圖用整數型態佇列)、
            **Data_Graphic_Queue_Z(Z通道資料繪圖用整數型態佇列)、
            **loop_num(迴圈用變數)
            */
            //***區域變數宣告***
            int Data_Graphic_Queue_Count_Max = 100;                             //  宣告Data_Graphic_Queue_Count_Max整數區域變數，記錄繪圖用整數型態佇列元素數量最大值
            OpenGL_Graph_point OpenGL_Graph_point_temp = new OpenGL_Graph_point();
            //  宣告OpenGL_Graph_point_temp(OpenGL繪圖暫存座標)
            OpenGL_Graph_point_temp.point_X = 0;                                //  初始化OpenGL_Graph_point_temp(OpenGL繪圖暫存座標)X座標數值
            OpenGL_Graph_point_temp.point_Y = 0;                                //  初始化OpenGL_Graph_point_temp(OpenGL繪圖暫存座標)Y座標數值
            //***以while迴圈清除顯示佇列過多資料***
            while (Data_Graphic_Queue_X.Count > Data_Graphic_Queue_Count_Max)   //  以while迴圈依序移除Data_Graphic_Queue_X佇列過多的資料
            {                                                                   //  進入while迴圈
                Data_Graphic_Queue_X.Dequeue();                                 //  移除Data_Graphic_Queue_X佇列資料
            }                                                                   //  結束while迴圈
            while (Data_Graphic_Queue_Y.Count > Data_Graphic_Queue_Count_Max)   //  以while迴圈依序移除Data_Graphic_Queue_Y佇列過多的資料
            {                                                                   //  進入while迴圈
                Data_Graphic_Queue_Y.Dequeue();                                 //  移除Data_Graphic_Queue_Y佇列資料
            }                                                                   //  結束while迴圈
            while (Data_Graphic_Queue_Z.Count > Data_Graphic_Queue_Count_Max)   //  以while迴圈依序移除Data_Graphic_Queue_Z佇列過多的資料
            {                                                                   //  進入while迴圈
                Data_Graphic_Queue_Z.Dequeue();                                 //  移除Data_Graphic_Queue_Z佇列資料
            }                                                                   //  結束while迴圈
            //***指定畫布***
            OpenGL Gsensor_GL = openGLControl1.OpenGL;                          //  取得OpenGL繪圖物件
            Gsensor_GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            //  清除深度緩存
            Gsensor_GL.LoadIdentity();
            //  重置模型觀察矩陣，並將其初始化
            Gsensor_GL.Translate(0f, 0f, -2.5f);                                //  轉移繪圖中心座標
            //***繪製外框***
            Gsensor_GL.Color(1.0f, 1.0f, 1.0f);                                 //  設定繪圖顏色
            Gsensor_GL.Begin(OpenGL.GL_LINE_LOOP);                              //  繪製封閉直線線段
            Gsensor_GL.Vertex(1, 1, 0);
            Gsensor_GL.Vertex(1, -1, 0);
            Gsensor_GL.Vertex(-1, -1, 0);
            Gsensor_GL.Vertex(-1, 1, 0);
            Gsensor_GL.End();                                                   //  結束繪製外框
            //***水平格線繪製***
            for (loop_num = -5; loop_num <= 5; loop_num++)                      //  以for迴圈依序繪製水平格線
            {                                                                   //  進入for迴圈
                Gsensor_GL.Begin(OpenGL.GL_LINES);                              //  開始繪製直線線段
                Gsensor_GL.Vertex(-1, ((float)loop_num / 5), 0);
                Gsensor_GL.Vertex(1, ((float)loop_num / 5), 0);
                Gsensor_GL.End();
            }                                                                   //  結束for迴圈
            //***垂直格線繪製***
            for (loop_num = -5; loop_num <= 5; loop_num++)                      //  以for迴圈依序繪製垂直格線
            {                                                                   //  進入for迴圈
                Gsensor_GL.Begin(OpenGL.GL_LINES);                              //  開始繪製直線線段
                Gsensor_GL.Vertex(((float)loop_num / 5), -1, 0);
                Gsensor_GL.Vertex(((float)loop_num / 5), 1, 0);
                Gsensor_GL.End();
            }                                                                   //  結束for迴圈
            //***依序繪製資料***
            loop_num = 0;
            foreach (int ADC_Data in Data_Graphic_Queue_X)                      //  依序以Data_Graphic_Queue_X資料繪圖
            {                                                                   //  進入foreach敘述
                if (loop_num == 0)                                              //  若為佇列中第一筆資料
                {                                                               //  進入if敘述
                    OpenGL_Graph_point_temp.point_X = ((double)loop_num / ADC_Raw_Data_Max) * 2 - 1;
                    //  指定OpenGL_Graph_point_temp之X座標
                    OpenGL_Graph_point_temp.point_Y = (((double)ADC_Data / 4096)) * 2 - 1;
                    //  指定OpenGL_Graph_point_temp之Y座標
                }                                                               //  結束if敘述
                else                                                            //  若不為佇列中第一筆資料
                {                                                               //  進入else敘述
                    Gsensor_GL.Begin(OpenGL.GL_LINES);                          //  開始繪製直線線段
                    Gsensor_GL.Color(1.0f, 0.0f, 0.0f);                         //  設定繪圖顏色
                    Gsensor_GL.Vertex(OpenGL_Graph_point_temp.point_X,
                                      OpenGL_Graph_point_temp.point_Y,
                                      0);                                       //  設定直線繪製起始點
                    Gsensor_GL.Vertex(((double)loop_num / ADC_Raw_Data_Max) * 2 - 1,
                                      (((double)ADC_Data / 4096)) * 2 - 1, 
                                      0);                                       //  設定直線繪製結束點
                    Gsensor_GL.End();                                           //  繪圖結束
                    OpenGL_Graph_point_temp.point_X = ((double)loop_num / ADC_Raw_Data_Max) * 2 - 1;
                    //  指定OpenGL_Graph_point_temp之X座標
                    OpenGL_Graph_point_temp.point_Y = (((double)ADC_Data / 4096)) * 2 - 1;
                    //  指定OpenGL_Graph_point_temp之Y座標
                }                                                               //  結束else敘述
                loop_num = loop_num + 1;                                        //  遞增loop_num變數
            }                                                                   //  結束foreach敘述
            loop_num = 0;
            foreach (int ADC_Data in Data_Graphic_Queue_Y)                      //  依序以Data_Graphic_Queue_Y資料繪圖
            {                                                                   //  進入foreach敘述
                if (loop_num == 0)                                              //  若為佇列中第一筆資料
                {                                                               //  進入if敘述
                    OpenGL_Graph_point_temp.point_X = ((double)loop_num / ADC_Raw_Data_Max) * 2 - 1;
                    //  指定OpenGL_Graph_point_temp之X座標
                    OpenGL_Graph_point_temp.point_Y = (((double)ADC_Data / 4096)) * 2 - 1;
                    //  指定OpenGL_Graph_point_temp之Y座標
                }                                                               //  結束if敘述
                else                                                            //  若不為佇列中第一筆資料
                {                                                               //  進入else敘述
                    Gsensor_GL.Begin(OpenGL.GL_LINES);                          //  開始繪製直線線段
                    Gsensor_GL.Color(0.0f, 1.0f, 0.0f);                         //  設定繪圖顏色
                    Gsensor_GL.Vertex(OpenGL_Graph_point_temp.point_X,
                                      OpenGL_Graph_point_temp.point_Y,
                                      0);                                       //  設定直線繪製起始點
                    Gsensor_GL.Vertex(((double)loop_num / ADC_Raw_Data_Max) * 2 - 1,
                                      (((double)ADC_Data / 4096)) * 2 - 1,
                                      0);                                       //  設定直線繪製結束點
                    Gsensor_GL.End();                                           //  繪圖結束
                    OpenGL_Graph_point_temp.point_X = ((double)loop_num / ADC_Raw_Data_Max) * 2 - 1;
                    //  指定OpenGL_Graph_point_temp之X座標
                    OpenGL_Graph_point_temp.point_Y = (((double)ADC_Data / 4096)) * 2 - 1;
                    //  指定OpenGL_Graph_point_temp之Y座標
                }                                                               //  結束else敘述
                loop_num = loop_num + 1;                                        //  遞增loop_num變數
            }                                                                   //  結束foreach敘述
            loop_num = 0;
            foreach (int ADC_Data in Data_Graphic_Queue_Z)                      //  依序以Data_Graphic_Queue_Z資料繪圖
            {                                                                   //  進入foreach敘述
                if (loop_num == 0)                                              //  若為佇列中第一筆資料
                {                                                               //  進入if敘述
                    OpenGL_Graph_point_temp.point_X = ((double)loop_num / ADC_Raw_Data_Max) * 2 - 1;
                    //  指定OpenGL_Graph_point_temp之X座標
                    OpenGL_Graph_point_temp.point_Y = (((double)ADC_Data / 4096)) * 2 - 1;
                    //  指定OpenGL_Graph_point_temp之Y座標
                }                                                               //  結束if敘述
                else                                                            //  若不為佇列中第一筆資料
                {                                                               //  進入else敘述
                    Gsensor_GL.Begin(OpenGL.GL_LINES);                          //  開始繪製直線線段
                    Gsensor_GL.Color(0.0f, 0.0f, 1.0f);                         //  設定繪圖顏色
                    Gsensor_GL.Vertex(OpenGL_Graph_point_temp.point_X,
                                      OpenGL_Graph_point_temp.point_Y,
                                      0);                                       //  設定直線繪製起始點
                    Gsensor_GL.Vertex(((double)loop_num / ADC_Raw_Data_Max) * 2 - 1,
                                      (((double)ADC_Data / 4096)) * 2 - 1,
                                      0);                                       //  設定直線繪製結束點
                    Gsensor_GL.End();                                           //  繪圖結束
                    OpenGL_Graph_point_temp.point_X = ((double)loop_num / ADC_Raw_Data_Max) * 2 - 1;
                    //  指定OpenGL_Graph_point_temp之X座標
                    OpenGL_Graph_point_temp.point_Y = (((double)ADC_Data / 4096)) * 2 - 1;
                    //  指定OpenGL_Graph_point_temp之Y座標
                }                                                               //  結束else敘述
                loop_num = loop_num + 1;                                        //  遞增loop_num變數
            }                                                                   //  結束foreach敘述
            Gsensor_GL.Flush();                                                 //  強制更新繪圖
        }                                                                       //  結束Data_Graphic_Queue_OpenGL副程式
        private void timer1_Tick(object sender, EventArgs e)                    //  宣告timer1_Tick副程式，即時更新現在時間
        {                                                                       //  進入timer1_Tick副程式
            DateTime localDate = DateTime.Now;                                  //  更新現在時間
        }                                                                       //  結束timer1_Tick副程式
    }                                                                           //  結束Form1類別
    //-----例外狀況處理-----
    public class IOException : SystemException
    {
        
    }
}
