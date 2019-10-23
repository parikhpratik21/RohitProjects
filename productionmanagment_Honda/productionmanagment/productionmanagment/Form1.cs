using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using DBCON;
using System.Net.Sockets;
using System.Net;
using DBCON;

namespace productionmanagment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int timeout = 0;
     
        string[] shift_time = new string[4];
        string[] Lunch_time = new string[4];
        string[] break_time = new string[8];
        DataTable settings = new DataTable();
        public DataTable csvtodatatable(string path)
        {
            DataTable dt = new DataTable();

            try
            {
                string CSVFilePathName = path;
                string[] Lines = File.ReadAllLines(CSVFilePathName);
                string[] Fields;
                Fields = Lines[0].Split(new char[] { ',' });
                int Cols = Fields.GetLength(0);
                //1st row must be column names; force lower case to ensure matching later on.
                for (int i = 0; i < Cols; i++)
                    dt.Columns.Add(Fields[i].ToString());
                DataRow Row;
                for (int i = 1; i < Lines.GetLength(0); i++)
                {
                    Fields = Lines[i].Split(new char[] { ',' });
                    Row = dt.NewRow();
                    for (int f = 0; f < Cols; f++)
                    {
                        try
                        {
                            Row[f] = Fields[f];
                        }
                        catch { }
                    }
                    dt.Rows.Add(Row);
                }

            }
            catch { }
            return dt;

        }
        DataTable devicetable = new DataTable();
        DataTable Holidays = new DataTable();

        int hours = 0;
        int mints = 0;
        public bool isHolday = false;
        public string shift = "Shift-1";
        public int time_compare(string t1)
        {


            DateTime today = DateTime.Now;
            long tcnt1 = 0;
            long tcnt2 = 0;
            string t2 = String.Format("{0:HH:mm}", today);
            try
            {
                tcnt1 = Convert.ToInt16(t1.Split(':')[0].ToString()) * 60 + Convert.ToInt16(t1.Split(':')[1].ToString());
                tcnt2 = Convert.ToInt16(t2.Split(':')[0].ToString()) * 60 + Convert.ToInt16(t2.Split(':')[1].ToString());
            }
            catch { }
            if (tcnt1 > tcnt2)
            {
                return -1;
            }
            else if (tcnt1 < tcnt2)
            {
                return 1;
            }
            else
            {
                return 0;
            }




        }
        DateTime DUMMY_DATE_TODAY;
        public int time_compare_DUMMY(string t1)
        {


            DateTime today = DUMMY_DATE_TODAY;
            long tcnt1 = 0;
            long tcnt2 = 0;
            string t2 = String.Format("{0:HH:mm}", today);
            try
            {
                tcnt1 = Convert.ToInt16(t1.Split(':')[0].ToString()) * 60 + Convert.ToInt16(t1.Split(':')[1].ToString());
                tcnt2 = Convert.ToInt16(t2.Split(':')[0].ToString()) * 60 + Convert.ToInt16(t2.Split(':')[1].ToString());
            }
            catch { }
            if (tcnt1 > tcnt2)
            {
                return -1;
            }
            else if (tcnt1 < tcnt2)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public bool shifttime = false;

        public bool isworkingday = true;
        int plantimer = 0;

        TcpClient tcpclnt = new TcpClient();
        TcpClient client = new TcpClient();
        string status_reply = "";

        public string addzerows(string number, int retlen)
        {
            string retstr = number;
            for (int k = 0; k < retlen - number.Length; k++)
            {
                retstr = "0" + retstr;
            }

            return retstr;
        }
        public bool check_cmd_respoce(int responce)
        {
           
            if (data_received)
            {
                data_received = false;
             
                    if (status_reply.StartsWith("COMMAND_DONE1") && responce == 1)
                    {
                      //  timer1value = 0;
                       // rectangleShape1.FillColor = Color.Lime;
                      //  this.Refresh();
                        return true;
                    }
                    
                    else if (status_reply.StartsWith("COMMAND_DONE2")&& responce == 2)
                    {
                      //  timer2value = 0;
                        //rectangleShape2.FillColor = Color.Lime;
                       // this.Refresh();
                        return true;
                    }
                    else if (status_reply.StartsWith("COMMAND_DONE3") && responce == 3)
                    {
                     //   timer3value = 0;
                       // rectangleShape3.FillColor = Color.Lime;
                       // this.Refresh();
                        return true; 
                    }
                    else if (status_reply.StartsWith("COMMAND_DONE4") && responce == 4)
                    {
                      //  timer4value = 0;
                       // rectangleShape4.FillColor = Color.Lime;
                       // this.Refresh();
                        return true;
                    }
                    if (status_reply.StartsWith("COMMAND_DONE5") &&  responce == 5)
                    {
                      //  timer5value = 0;
                      //  rectangleShape5.FillColor = Color.Lime;
                      //  this.Refresh();
                        return true;
                    }
                    else
                    {
                    return false;
                    }
                  }
                else
                {
                return false;
                }

            
        
        }
        public bool check_status_respoce(int responce)
        {
            if (data_received)
            {
                data_received = false;
                
            
                        if (status_reply.StartsWith("RESPONSE_DONE1") && responce == 1)
                        {
                         //   timer1value = 0;
                  //          ovalShape1.FillColor = Color.Lime;
                   //         this.Refresh();
                            return true;
                        }
                        else if (status_reply.StartsWith("RESPONSE_DONE2") && responce == 2)
                        {
                         //   timer2value = 0;
                    //        ovalShape2.FillColor = Color.Lime;
                     //       this.Refresh();
                            return true;
                        }
                        else if (status_reply.StartsWith("RESPONSE_DONE3") && responce == 3)
                        {
                         //   timer3value = 0;
                      //      ovalShape3.FillColor = Color.Lime;
                      //      this.Refresh();
                            return true;
                        }
                        else if (status_reply.StartsWith("RESPONSE_DONE4") && responce == 4)
                        {
                          //  timer4value = 0;
                        //    ovalShape4.FillColor = Color.Lime;
                        //    this.Refresh();
                            return true;
                        }
                        else if (status_reply.StartsWith("RESPONSE_DONE5") && responce == 5)
                        {
                          //  timer5value = 0;
                        //    ovalShape5.FillColor = Color.Lime;
                         //   this.Refresh();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else 
                    {
                        return false;
                    }

             
        }
        
        public void Com_commands(string S)
        {
            try
            {               
                if (!setting_open)
                {                   
                    if (S == "1")
                    {
                        try
                        {
                            timer1value = timeout;
                            data_received = false;
                            data = "";
                            cmnd_sent = true; TcpClient tcpclnt = new TcpClient();
                            byte[] bb = new byte[500];
                            byte[] ba = new byte[500];                                                     
                            tcpclnt.Connect(clienttcpip, Convert.ToInt32(clientport));
                            Stream stm = tcpclnt.GetStream();
                            ASCIIEncoding asen = new ASCIIEncoding();
                            string sTcpText = "NKMLINE1:" + System.DateTime.Now.ToString("ddMMyyHHmm") + "\r\n";
                            ba = asen.GetBytes(sTcpText);
                            stm.WriteTimeout = 2000;
                            stm.Write(ba, 0, ba.Length);
                            tcpclnt.Close();
                            timer1value = timeout;                                                      
                         }
                          catch 
                         {
                             cmnd_sent = false;                        
                         }                     
                        //this.Refresh();
                    }
                                       
                    if (S == "2")
                    {
                        try
                        {
                            timer1value = timeout;
                            data_received = false;
                            data = "";
                            cmnd_sent = true;
                            TcpClient tcpclnt = new TcpClient();
                            //byte[] bb = new byte[500];
                            byte[] ba = new byte[500];                            
                            tcpclnt.Connect(clienttcpip, Convert.ToInt32(clientport));
                            Stream stm = tcpclnt.GetStream();
                            ASCIIEncoding asen = new ASCIIEncoding();

                            string sMessage = string.Empty;
                            sMessage = TARGETLABLE.Text.PadLeft(4, '0') + PLANLABLE.Text.PadLeft(4, '0') + ACTUALLABLE.Text.PadLeft(4, '0');
                            ba = asen.GetBytes("NKMLINE2:" + sMessage + "\r\n");

                            stm.WriteTimeout = 2000;
                            stm.Write(ba, 0, ba.Length);
                            tcpclnt.Close();
                            timer1value = timeout;                          
                        }
                        catch
                        {
                            cmnd_sent = false;
                            // timer1value = 0;
                        }
                    }

                   
                }
            }
            catch (Exception ex)
            {
               
              //  this.Refresh();
            //    tcpclnt.Close();
                cmnd_sent = false;

            }
            finally
            {
            //    tcpclnt.Close();
                //cmd1_send = true;
            }


        }
        public string cmdnumber = "";

        //public void Com_status(string S)
        //{
        //    try
        //    {

        //        if (!setting_open)
        //        {

                    
        //            if (S == "1")
        //            {
        //                try
        //                {
        //                    responce_sent = true;
        //                    timer1value = timeout;
        //                    data_received = false;
        //                    data = "";
                          
        //                    TcpClient tcpclnt = new TcpClient();
        //                    byte[] bb = new byte[500];
        //                    byte[] ba = new byte[500];                            
        //                    //ovalShape1.FillColor = Color.Red;
        //                    tcpclnt.Connect(TCP_IP.Text, Convert.ToInt32(portno.Text));
        //                    Stream stm = tcpclnt.GetStream();
        //                    ASCIIEncoding asen = new ASCIIEncoding();
        //                    ba = asen.GetBytes("CHECK_RESPONSE" + S + "\r\n");
        //                    stm.WriteTimeout = 500;
        //                    stm.Write(ba, 0, ba.Length);
        //                    tcpclnt.Close();
        //                    timer1value = timeout;

        //                    //while (timer1value > 0)
        //                    //{
        //                    //    //Thread.Sleep(1);
        //                    //    if (check_status_respoce(1) == true)
        //                    //    {
        //                    //        ovalShape1.FillColor = Color.LimeGreen;
        //                    //       // this.Refresh();
        //                    //        responce_sent = false;

        //                    //        break;
        //                    //    }
        //                    //    // timer1value--;
        //                    //}
        //                }
        //                catch 
        //                {
        //               //     timer1value = 0;
        //                    responce_sent = false;
                        
        //                }
        //                //if (timer1value == 0)
        //                //{
        //                //    ovalShape1.FillColor = Color.Red;
        //                //    responce_sent = false;
        //                //   // this.Refresh();
        //                //}
                       
                        

        //            }
        //            if (S == "2")
        //            {
        //                try
        //                {
        //                    responce_sent = true;
        //                    timer1value = timeout;
        //                    data_received = false;
        //                    data = "";
        //                    responce_sent = true;
        //                    TcpClient tcpclnt = new TcpClient();
        //                    byte[] bb = new byte[500];
        //                    byte[] ba = new byte[500];                            
        //                    //ovalShape2.FillColor = Color.Red;
        //                    tcpclnt.Connect(TCP_IP.Text, Convert.ToInt32(portno.Text));
        //                    Stream stm = tcpclnt.GetStream();
        //                    ASCIIEncoding asen = new ASCIIEncoding();
        //                    ba = asen.GetBytes("CHECK_RESPONSE" + S + "\r\n");
        //                    stm.WriteTimeout = 500;
        //                    stm.Write(ba, 0, ba.Length);
        //                    tcpclnt.Close();
        //                    timer1value = timeout;

        //                    //while (timer1value > 0)
        //                    //{
        //                    //    //  Thread.Sleep(1);
        //                    //    if (check_status_respoce(2) == true)
        //                    //    {
        //                    //        responce_sent = false;
        //                    //        ovalShape2.FillColor = Color.LimeGreen;
        //                    //        break;
        //                    //    }
        //                    //}
        //                }
        //                catch 
        //                {
        //                    //timer1value = 0;
        //                    responce_sent = false;
                        
        //                }
        //                //if (timer1value == 0)
        //                //{
        //                //    ovalShape2.FillColor = Color.Red;
        //                //}
        //                //timer1value--;
        //            }
        //            if (S == "3")
        //            {
        //                try
        //                {
        //                    responce_sent = true;
        //                    timer1value = timeout;
        //                    data_received = false;
        //                    data = "";
        //                    responce_sent = true;
        //                    TcpClient tcpclnt = new TcpClient();
        //                    byte[] bb = new byte[500];
        //                    byte[] ba = new byte[500];                            
        //                   // ovalShape3.FillColor = Color.Red;
        //                    tcpclnt.Connect(TCP_IP.Text, Convert.ToInt32(portno.Text));
        //                    Stream stm = tcpclnt.GetStream();
        //                    ASCIIEncoding asen = new ASCIIEncoding();
        //                    ba = asen.GetBytes("CHECK_RESPONSE" + S + "\r\n");
        //                    stm.WriteTimeout = 500;
        //                    stm.Write(ba, 0, ba.Length);
        //                    tcpclnt.Close();
        //                    timer1value = timeout;

        //                    //while (timer1value > 0)
        //                    //{
        //                    //    //Thread.Sleep(3);
        //                    //    if (check_status_respoce(3) == true)
        //                    //    {
        //                    //        responce_sent = false;
        //                    //        ovalShape3.FillColor = Color.LimeGreen;
                                   
        //                    //        responce_sent = false;

        //                    //        break;
        //                    //    }
        //                    //    timer1value--;
        //                    //}
        //                }
        //                catch
        //                {
        //                  //  timer1value = 0;
        //                    responce_sent = false;
                         
        //                }

        //                //if (timer1value == 0)
        //                //{
        //                //    ovalShape3.FillColor = Color.Red;
        //                //    responce_sent = false;
        //                //    this.Refresh();

        //                //}
        //            }
        //            if (S == "4")
        //            {
        //                try
        //                {

        //                    responce_sent = true;
        //                    timer1value = timeout;
        //                    data_received = false;
        //                    data = ""; TcpClient tcpclnt = new TcpClient();
        //                    byte[] bb = new byte[500];
        //                    byte[] ba = new byte[500];
        //                    //ovalShape4.FillColor = Color.Red;
        //                    tcpclnt.Connect(TCP_IP.Text, Convert.ToInt32(portno.Text));
        //                    Stream stm = tcpclnt.GetStream();
        //                    ASCIIEncoding asen = new ASCIIEncoding();
        //                    ba = asen.GetBytes("CHECK_RESPONSE" + S + "\r\n");
        //                    stm.WriteTimeout = 500;
        //                    stm.Write(ba, 0, ba.Length);
        //                    tcpclnt.Close();
        //                    timer1value = timeout;

        //                    //while (timer1value > 0)
        //                    //{
        //                    //    // Thread.Sleep(1);
        //                    //    if (check_status_respoce(1) == true)
        //                    //    {
        //                    //        ovalShape4.FillColor = Color.LimeGreen;
                          
        //                    //        responce_sent = false;

        //                    //        break;
        //                    //    }
        //                    //    //  timer1value--;
        //                    //}
        //                }
        //                catch 
        //                {
        //                 //   timer1value = 0;
        //                    responce_sent = false;
                        
        //                }
        //                //if (timer1value == 0)
        //                //{
        //                //    ovalShape4.FillColor = Color.Red;
        //                //    responce_sent = false;
        //                //    this.Refresh();
        //                // }
        //            }
        //            if (S == "5")
        //            {
        //                try
        //                {
        //                    responce_sent = true;
        //                    timer1value = timeout;
        //                    data_received = false;
        //                    data = "";
        //                    TcpClient tcpclnt = new TcpClient();
        //                    byte[] bb = new byte[500];
        //                    byte[] ba = new byte[500];
        //                    //ovalShape5.FillColor = Color.Red;
        //                    tcpclnt.Connect(TCP_IP.Text, Convert.ToInt32(portno.Text));
        //                    Stream stm = tcpclnt.GetStream();
        //                    ASCIIEncoding asen = new ASCIIEncoding();
        //                    ba = asen.GetBytes("CHECK_RESPONSE" + S + "\r\n");
        //                    stm.WriteTimeout = 500;
        //                    stm.Write(ba, 0, ba.Length);
        //                    tcpclnt.Close();
        //                    timer1value = timeout;

        //                    //while (timer1value > 0)
        //                    //{
        //                    //    // Thread.Sleep(1);
        //                    //    if (check_status_respoce(5) == true)
        //                    //    {
        //                    //        ovalShape5.FillColor = Color.LimeGreen;
                                  
        //                    //        responce_sent = false;

        //                    //        break;
        //                    //    }
        //                    //    //timer1value--;
        //                    //}
        //                }
        //                catch 
        //                {
        //                  //  timer1value = 0;
        //                    responce_sent = false;
                        
        //                }
        //                //if (timer1value == 0)
        //                //{
        //                //    ovalShape5.FillColor = Color.Red;
        //                //    responce_sent = false;
        //                //    this.Refresh();

        //                //}
        //            }
                    
        //        }
        //    }
        //    catch
        //    {
        //        /*if (S == "1")
        //        {
        //            ovalShape1.FillColor = Color.Red;
        //        }
        //        if (S == "2")
        //        {
        //            ovalShape2.FillColor = Color.Red;
        //        }
        //        if (S == "3")
        //        {
        //            ovalShape3.FillColor = Color.Red;
        //        }
        //        if (S == "4")
        //        {
        //            ovalShape4.FillColor = Color.Red;
        //        }
        //        if (S == "5")
        //        {
        //            ovalShape5.FillColor = Color.Red;
        //        }*/
        //     //   this.Refresh();
        //       // tcpclnt.Close();


        //    }
        //    finally 
        //    {
        //       // tcpclnt.Close();
        //       // responce_sent = false;
        //    }
        //}
        public string filepath = "";
        int status_count = 1;
        int comm_count = 1;
        string second_str = "";

        string clienttcpip = "";
        string clientport = "";
        string displayscroll = "";
        string displayspeed = "";       

        void dummy_clock(DateTime dummydate)
        {
            DateTime today = dummydate;
            TIMELABLE.Text = String.Format("{0:HH:mm}", today);
                
            string h_date = today.Date.Day.ToString() + "-" + today.Date.Month.ToString() + "-" + today.Date.Year.ToString();            
            DATELABLE.Text = String.Format("{0:dd/MM/yy}", today).ToUpper();
            string weekday = String.Format("{0:ddd}", today).ToUpper();
            if ((second_str != today.Second.ToString()))
            {
                second_str = today.Second.ToString();
                if (isworkingday == true)
                {
                    if (plantimer > 0)
                    {
                        plantimer--;
                    }
                    if (plantimer == 0 && shifttime == true)
                    {
                        
                        plantimer = Convert.ToInt16(PLANNINGTIME.Text);
                        if (Convert.ToInt64(PLANLABLE.Text) < Convert.ToInt64(TARGETLABLE.Text))
                        {
                            PLANLABLE.Text = (Convert.ToInt16(PLANLABLE.Text) + 1).ToString();
                            cmd2_send = true;
                        }                                                                            

                    }
                }
            }
                       
            isworkingday = true;
                    
            if (today.Month != lastupdate.Month)
            {
                lastupdate = today;                             
            }
            
            
            if (today.Minute != lastupdate.Minute)
            {
                lastupdate = today;
                try
                {
                    if (Mon.Checked && weekday == "MON")
                    {
                        isworkingday = false;
                        status_string = "Week OFF";
                    }
                    if (Tue.Checked && weekday == "TUE")
                    {
                        isworkingday = false;
                        status_string = "Week OFF";
                    }
                    if (Wed.Checked && weekday == "WED")
                    {
                        isworkingday = false;
                        status_string = "Week OFF";
                    }
                    if (Thu.Checked && weekday == "THU")
                    {
                        isworkingday = false;
                        status_string = "Week OFF";
                    }
                    if (Fri.Checked && weekday == "FRI")
                    {
                        isworkingday = false;
                        status_string = "Week OFF";
                    }

                    if (Sat.Checked && weekday == "SAT")
                    {
                        isworkingday = false;
                        status_string = "Week OFF";
                    }
                    if (Sun.Checked && weekday == "SUN")
                    {
                        isworkingday = false;
                        status_string = "Week OFF";
                    }
                }
                catch { }
                foreach (string Hdate in Holiday_list)
                {
                    if (h_date == Hdate)
                    {
                        isworkingday = false;
                        status_string = "Today is Holiday";

                    }
                    
                    


                }
                if (isworkingday == true)
                {


                    if (time_compare_DUMMY(shift_time[0]) == 0 || time_compare_DUMMY(shift_time[2]) == 0)
                    {
                        PLANLABLE.Text = "0000";
                        ACTUALLABLE.Text = "0000";                        
                    }
                    /*if (time_compare(shift_time[1]) == 0 || time_compare(shift_time[3]) == 0)
                    {
                     //   PLANTOTALLABLE.Text =   (Convert.ToInt16(PLANLABLE.Text) + Convert.ToInt16(PLANTOTALLABLE.Text)).ToString();
                        ATCUALTOTALLABLE.Text = (Convert.ToInt16(ATCUALTOTALLABLE.Text) + Convert.ToInt16(ACTUALLABLE.Text)).ToString();

                    }*/


                    if (time_compare_DUMMY(shift_time[0]) >= 0 && time_compare_DUMMY(shift_time[1]) <= 0)
                    {                        
                        status_string = "SHIFTA";
                        shifttime = true;

                    }
                    else if (time_compare_DUMMY(shift_time[2]) >= 0 && time_compare_DUMMY(shift_time[3]) <= 0)
                    {
                        shifttime = true;                        
                        status_string = "SHIFTB";
                    }
                    else
                    {
                        shifttime = false;
                    }
                    if ((time_compare_DUMMY(break_time[0]) >= 0 && time_compare_DUMMY(break_time[1]) <= 0) || (time_compare_DUMMY(break_time[4]) >= 0 && time_compare_DUMMY(break_time[5]) <= 0))
                    {
                        status_string = "Tea Break-1";
                        shifttime = false;

                    }
                    if ((time_compare_DUMMY(break_time[2]) >= 0 && time_compare_DUMMY(break_time[3]) <= 0) || (time_compare_DUMMY(break_time[6]) >= 0 && time_compare_DUMMY(break_time[7]) <= 0))
                    {
                        status_string = "Tea Break-2";
                        shifttime = false;
                    }
                    if ((time_compare_DUMMY(Lunch_time[0]) >= 0 && time_compare_DUMMY(Lunch_time[1]) <= 0) || (time_compare_DUMMY(Lunch_time[2]) >= 0 && time_compare_DUMMY(Lunch_time[3]) <= 0))
                    {
                        status_string = "Lunch/Dinner Break";
                        shifttime = false;
                    }

                }


            }
        
        }
        void readfile(string f)
        {
            filepath = Actualfilepath.Text;
            try
            {
                File.Copy(filepath, Application.StartupPath + "\\data1.txt", true);
            }
            catch
            {

            }
            StreamReader file = new StreamReader(Application.StartupPath + "\\data1.txt");
            try
            {
                string newvalue = file.ReadLine().Split('|')[3].ToString();
                Int64 diff = Convert.ToInt64(newvalue) - Convert.ToInt64(ACTUALLABLE.Text);
                UpdateActualTextValue(newvalue.ToString());
                if(diff != 0)
                {
                //    ATCUALTOTALLABLE.Text = (Convert.ToInt64(ATCUALTOTALLABLE.Text) + diff).ToString();
                    cmd2_send = true;      
                }
            }
            catch
            {
                file.Close();
            }
            finally
            {
                file.Close();
            }
        }

        private delegate void updateActualTextUI(string pValue);
        private void UpdateActualTextValue(string pValue)
        {            
            if(ACTUALLABLE.InvokeRequired == true)
            {
                updateActualTextUI updateUI = new updateActualTextUI(UpdateActualTextValue);

                object[] args = new object[1];
                args[0] = pValue;
                ACTUALLABLE.Invoke(updateUI, args);
            } 
            else
            {
                ACTUALLABLE.Text = pValue;
            }       
        }

        int file_read_counter = 0;
        bool software_started=true;
        bool cmd1_send = true;
        bool cmd2_send = true;   
        private void timer2_Tick(object sender, EventArgs e)
        {
            //timer2.Enabled = false;

            DateTime today = DateTime.Now;

            if (software_started)
            {
                TIMELABLE.Text = String.Format("{0:HH:mm}", today);
                Thread m_serverThread = new Thread(() => Com_commands("1"));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
                m_serverThread.Start();
                cmd1_send = true;
                cmd2_send = true;               
                software_started = false;
            }
            else
            {
                file_read_counter++;
                if (file_read_counter > 5)
                {
                    file_read_counter = 0;
                    if (fileavile)
                    {

                       Thread m_serverThread = new Thread(() => readfile("3"));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
                        m_serverThread.Start();
                       
                    }

                }
                string h_date = today.Date.Day.ToString() + "-" + today.Date.Month.ToString() + "-" + today.Date.Year.ToString();
                DATELABLE.Text = String.Format("{0:dd/MM/yy}", today).ToUpper();
                string weekday = String.Format("{0:ddd}", today).ToUpper();
                if ((second_str != today.Second.ToString()))
                {
                  
                    second_str = today.Second.ToString();
                    if (isworkingday == true)
                    {
                        if (plantimer > 0)
                        {
                            plantimer--;
                        }

                        if (plantimer == 0 && shifttime == true)
                        {


                            plantimer = Convert.ToInt16(PLANNINGTIME.Text);
                            if (Convert.ToInt16(PLANLABLE.Text) < Convert.ToInt16(TARGET.Text))
                            {
                                PLANLABLE.Text = (Convert.ToInt16(PLANLABLE.Text) + 1).ToString();
                               // PLANTOTALLABLE.Text = (Convert.ToInt64(PLANTOTALLABLE.Text) + 1).ToString();
                                //con.updateproduction(1, TARGETTOTALLABLE.Text, PLANLABLE.Text, PLANTOTALLABLE.Text, ACTUALLABLE.Text, ATCUALTOTALLABLE.Text);
                                cmd2_send = true;
                            }
                           
                            //cmd2_send = true;

                        }
                    }
                }
                if (Convert.ToBoolean(today.Second % 2) == false)
                {
                         //if (!cmnd_sent)
                          {                           
                              
                             if (cmd1_send)
                              {
                                cmd1_send = false;
                                Thread m_serverThread = new Thread(() => Com_commands("1"));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
                                m_serverThread.Start();
                               // if (cmd2_send == true)
                               //     System.Threading.Thread.Sleep(5000);
                              }

                             if (cmd2_send)
                             {
                                 cmd2_send = false;
                                 Thread m_serverThread = new Thread(() => Com_commands("2"));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
                                 m_serverThread.Start();

                             }
                             else
                             {
                                 if (!responce_sent)
                                 {
                                     //Thread m_serverThread = new Thread(() => Com_status(comm_count.ToString()));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
                                     //m_serverThread.Start();
                                     comm_count++;
                                 }
                             }
                        
                            //if (comm_count > 5)
                            //{
                            //    comm_count = 1;
                            //}
                        
                    }
                }

                if (Convert.ToBoolean(today.Second % 2) == true)
                {
                    TIMELABLE.Text = String.Format("{0:HH:mm}", today);
                    Wstatus.Text = status_string;
                }
                else
                {

                    TIMELABLE.Text = String.Format("{0:HH:mm}", today);
                    //  Wstatus.Text = "";
                }                

                //if (DateTime.Now.Month != lastupdate.Month)
                //{
                //    lastupdate = DateTime.Now;
                //    con.update("UPDATE Setting SET   lastupdated='" + lastupdate.ToString() + "' WHERE  (Setting.ID = 1)");
                //    TARGETTOTALLABLE.Text = "000000";
                //    ATCUALTOTALLABLE.Text = "000000";
                //    PLANTOTALLABLE.Text = "000000";

                //    con.updateproduction(1, TARGETTOTALLABLE.Text, PLANLABLE.Text, PLANTOTALLABLE.Text, ACTUALLABLE.Text, ATCUALTOTALLABLE.Text);
                //    //Com_commands("4");
                //    //Thread m_serverThread = new Thread(() => Com_commands("4"));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
                //    //m_serverThread.Start();
                //    cmd4_send = true;
                //}
                if (DateTime.Now.Minute != lastupdate.Minute)
                {
                    cmd1_send = true;

                    lastupdate = DateTime.Now;
                    isworkingday = true;

                    con.update("UPDATE Setting SET   lastupdated='" + lastupdate.ToString() + "' WHERE  (Setting.ID = 1)");
                    try
                    {
                        if (Mon.Checked && weekday == "MON")
                        {
                            isworkingday = false;
                            status_string = "Week OFF";
                        }
                        if (Tue.Checked && weekday == "TUE")
                        {
                            isworkingday = false;
                            status_string = "Week OFF";
                        }
                        if (Wed.Checked && weekday == "WED")
                        {
                            isworkingday = false;
                            status_string = "Week OFF";
                        }
                        if (Thu.Checked && weekday == "THU")
                        {
                            isworkingday = false;
                            status_string = "Week OFF";
                        }
                        if (Fri.Checked && weekday == "FRI")
                        {
                            isworkingday = false;
                            status_string = "Week OFF";
                        }

                        if (Sat.Checked && weekday == "SAT")
                        {
                            isworkingday = false;
                            status_string = "Week OFF";
                        }
                        if (Sun.Checked && weekday == "SUN")
                        {
                            isworkingday = false;
                            status_string = "Week OFF";
                        }
                    }
                    catch { }
                    foreach (string Hdate in Holiday_list)
                    {
                        if (h_date == Hdate)
                        {
                            isworkingday = false;
                            status_string = "Today is Holiday";

                        }


                    }
                    if (isworkingday == true)
                    {


                        if (time_compare(shift_time[0]) == 0 || time_compare(shift_time[2]) == 0)
                        {
                            PLANLABLE.Text = "0000";
                            ACTUALLABLE.Text = "0000";                          
                        }                       

                        if (time_compare(shift_time[0]) >= 0 && time_compare(shift_time[1]) <= 0)
                        {                            
                            status_string = "SHIFTA";
                            shifttime = true;
                        }
                        else if (time_compare(shift_time[2]) >= 0 && time_compare(shift_time[3]) <= 0)
                        {
                            shifttime = true;                            
                            status_string = "SHIFTB";
                        }
                        else
                        {
                            shifttime = false;
                        }
                        if ((time_compare(break_time[0]) >= 0 && time_compare(break_time[1]) <= 0) || (time_compare(break_time[4]) >= 0 && time_compare(break_time[5]) <= 0))
                        {
                            status_string = "Tea Break-1";
                            shifttime = false;

                        }
                        if ((time_compare(break_time[2]) >= 0 && time_compare(break_time[3]) <= 0) || (time_compare(break_time[6]) >= 0 && time_compare(break_time[7]) <= 0))
                        {
                            status_string = "Tea Break-2";
                            shifttime = false;
                        }
                        if ((time_compare(Lunch_time[0]) >= 0 && time_compare(Lunch_time[1]) <= 0) || (time_compare(Lunch_time[2]) >= 0 && time_compare(Lunch_time[3]) <= 0))
                        {
                            status_string = "Lunch/Dinner Break";
                            shifttime = false;
                        }

                    }


                }
            }

           // timer2.Enabled = true;
        }

        public string status_string = "";

        dbconnection con = new dbconnection();

        DataTable siteread = new DataTable();
        public static List<DateTime> list = new List<DateTime>();
        string[] Holiday_list = new string[1000];
        bool server_started = false;

        int timervalue = 0;
        bool cmnd_sent = false;
        bool responce_sent = false;
        int timer1value = 0;
        int timer2value = 0;
        int timer3value = 0;
        int timer4value = 0;
        int timer5value = 0;
        


        
        private void settingmenu_Click(object sender, EventArgs e)
        {
            PasswordBox oPassword = new PasswordBox();
            //oPassword.Parent = this;
            oPassword.StartPosition = FormStartPosition.CenterScreen;
            oPassword.ShowDialog();
            if (oPassword.Result == true)
            {
                read_setting();
                Shifts.SelectedIndex = 1;
                Shifts.SelectedIndex = 0;
                settingpanel.Show();
                settingpanel.Enabled = true;
                savebutton.Enabled = true;
                settingpanel.BringToFront();
                settingpanel.Dock = DockStyle.Fill;
                setting_open = true;
            }               
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LogManager.WriteErrors("Form1", "Test", new Exception("Test 1"));
                LogManager.WriteErrors("Form1", "Test", new Exception("Test 2"));
                string path = Application.StartupPath.ToString() + "\\timeout.txt";
                StreamReader a = new StreamReader(path);
                timeout = Convert.ToInt16(a.ReadLine());
                a.Close();
            }

            catch { }
            
            try
            {
                Dates.Value = DateTime.Now;
                //string[] ports = SerialPort.GetPortNames();
                //foreach (string port in ports)
                //{

                //    COMPORT.Items.Add(port.ToString());

                //}
            }

            catch { }
            //try
            //{
                
                
            //    string[] ports = SerialPort.GetPortNames();
            //    foreach (string port in ports)
            //    {

            //        COMPORT.Items.Add(port.ToString());

            //    }
            //}

            //catch { }
            
            try
            {
                read_setting();
               // con.updateproduction(1, TARGETTOTALLABLE.Text, PLANLABLE.Text, PLANTOTALLABLE.Text, ACTUALLABLE.Text, ATCUALTOTALLABLE.Text);
                con.update("UPDATE Setting SET   lastupdated='" + lastupdate.ToString() + "' WHERE  (Setting.ID = 1)");
                //con.updateproduction(1, TARGETTOTALLABLE.Text, PLANLABLE.Text, PLANTOTALLABLE.Text, ACTUALLABLE.Text, ATCUALTOTALLABLE.Text);
            }
            catch { }
            try
            {
                Shifts.SelectedIndex = 1;
                Shifts.SelectedIndex = 0;              
            }
            catch
            {
            }
           
                
           
        }
        TcpListener server = null;
        private void ServerThreadStart()
        {


            try
            {

                Int32 port = Convert.ToInt32(Serverport.Text);
                IPAddress localAddr = IPAddress.Parse(ServerIP.Text);
                server = new TcpListener(localAddr, port);
                server.Start();

                while (true)
                {
                    if (closing == true)
                    {
                        break;
                    }
                    client = server.AcceptTcpClient();
                    Thread CleintThread = new Thread(new ThreadStart(ClientThreadStart));
                    CleintThread.Start();
                }



            }
            catch (SocketException e)
            {
                //MessageBox.Show("SocketException:" + e.Message);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

        }

        string data = "";
        bool data_received = false;
        Byte[] bytes = new Byte[256];
        bool fisrttimeread = false;
        private void ClientThreadStart()
        {
            try
            {
                data = null;
                NetworkStream stream = client.GetStream();
                int i;
                stream.ReadTimeout = 4000;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    
                    data_received = true;
                    //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    client.Close();
                }
                client.Close();
            }
            catch
            {
                client.Close();
            }
        }
        bool fileavile = false;
        DateTime lastupdate;
        DataTable production = new DataTable();
        string connection_staus;
       
        public void read_setting()
        {

            try
            {
                Holidays = con.gettable("Select * from holidays");
                Holidaygrid.DataSource = Holidays;
                Holidaygrid.Columns[0].Visible = false;
                Holidaygrid.Columns[1].Width = 190;
                Holidaygrid.Columns[2].Width = 173;
                for (int row = 0; row < Holidays.Rows.Count; row++)
                {
                    Holiday_list[row] = Holidays.Rows[row][2].ToString();
                }
            }
            catch { }
            try
            {
                settings = con.gettable("Select * from setting");
                //COMPORT.Text = settings.Rows[0][1].ToString();
                //baudrate.Text = settings.Rows[0][2].ToString();
                TARGET.Text = settings.Rows[0][4].ToString();
                TARGETLABLE.Text = settings.Rows[0][4].ToString();
                PLANNINGTIME.Text = settings.Rows[0][3].ToString();
                SCROLLING.Text = settings.Rows[0][5].ToString();
                displayscroll = SCROLLING.Text;
                displayspeed = settings.Rows[0]["Scroll_speed"].ToString();
                displaytextBox.Text = settings.Rows[0][6].ToString();
                lastupdate = Convert.ToDateTime(settings.Rows[0][7].ToString());
                ServerIP.Text = settings.Rows[0]["myip"].ToString();
                Serverport.Text = settings.Rows[0]["myport"].ToString();
                Actualfilepath.Text = settings.Rows[0]["filepath"].ToString();
                TitleValue.Text = settings.Rows[0]["COMSPEED"].ToString();

                this.Text = TitleValue.Text;

                if (File.Exists(Actualfilepath.Text))
                {

                    fileavile = true;
                }
                else
                {
                   // MessageBox.Show(" Actual value File not Avilable");
                }




            }
            catch { }
            try
            {
                production = con.gettable("Select * from production");
                //TARGETTOTALLABLE.Text = production.Rows[0][1].ToString();
                PLANLABLE.Text = production.Rows[0][2].ToString();
               // PLANTOTALLABLE.Text = production.Rows[0][3].ToString();
               // ATCUALTOTALLABLE.Text = production.Rows[0][5].ToString();
               // targettotal.Text = TARGETTOTALLABLE.Text;
                Planvalue.Text = PLANLABLE.Text;
               // Plannigtotalvalue.Text = PLANTOTALLABLE.Text;
               // actualtotal.Text = ATCUALTOTALLABLE.Text;
                // SCROLLING.Text = settings.Rows[0][5].ToString();
                // displaytextBox.Text = settings.Rows[0][6].ToString();
                // lastupdate = Convert.ToDateTime(settings.Rows[0][7].ToString());

            }
            catch { }
            try
            {
                for (int j = 0; j < 4; j++)
                {
                    shift_time[j] = settings.Rows[0][j + 8].ToString();
                }
                for (int j = 0; j < 8; j++)
                {
                    break_time[j] = settings.Rows[0][j + 12].ToString();
                }
            }
            catch { }
            try
            {
                Lunch_time[0] = settings.Rows[0]["shift1lunchstart"].ToString();
                Lunch_time[1] = settings.Rows[0]["shift1lunchend"].ToString();
                Lunch_time[2] = settings.Rows[0]["shift2lunchstart"].ToString();
                Lunch_time[3] = settings.Rows[0]["shift2lunchend"].ToString();

            }
            catch { }

            try
            {
                string offdays = settings.Rows[0]["Weekoffs"].ToString();
                string[] days;
                days = offdays.Split(',');
                Mon.Checked = false;
                Tue.Checked = false;
                Wed.Checked = false;
                Thu.Checked = false;
                Fri.Checked = false;
                Sat.Checked = false;
                Sun.Checked = false;
                foreach (string d in days)
                {
                    if (d == "Mon")
                    {
                        Mon.Checked = true;
                    }
                    if (d == "Tue")
                    {
                        Tue.Checked = true;
                    }
                    if (d == "Wed")
                    {
                        Wed.Checked = true;
                    }
                    if (d == "Thu")
                    {
                        Thu.Checked = true;
                    }
                    if (d == "Fri")
                    {
                        Fri.Checked = true;
                    }
                    if (d == "Sat")
                    {
                        Sat.Checked = true;
                    }
                    if (d == "Sun")
                    {
                        Sun.Checked = true;
                    }
                }
            }
            catch { }
            try
            {

                //COMMODE.Text = settings.Rows[0]["COMMODE"].ToString();
                TCP_IP.Text = settings.Rows[0]["TCPIP"].ToString();
                portno.Text = settings.Rows[0]["TCPPORT"].ToString();
                clienttcpip = TCP_IP.Text;
                clientport = portno.Text;

            }
            catch { }
            try
            {
                DISPLAYLABLE.Text = displaytextBox.Text;
                ScrollSpeed.Text = settings.Rows[0]["Scroll_speed"].ToString();
            }
            catch
            {

            }
           
            try
            {
                if (server_started == false)
                {
                    Thread m_serverThread = new Thread(new ThreadStart(ServerThreadStart));
                    m_serverThread.Start();
                    server_started = true;
                }
            }
            catch { }

            timer1.Start();
            timer2.Start();
            timer3.Start();  

        }

        //  SerialPort[] Modbus_port = new SerialPort[8];


        DataTable Devices = new DataTable();
        DataTable Modemslocal = new DataTable();
        DataTable Modemsremote = new DataTable();
        DataTable modems = new DataTable();

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog directchoosedlg = new OpenFileDialog();

            if (directchoosedlg.ShowDialog() == DialogResult.OK)
            {
                Actualfilepath.Text = directchoosedlg.FileName;
            }
        }
        bool setting_open = false;
        private void buttonclose_Click(object sender, EventArgs e)
        {
            settingpanel.Hide();
            settingpanel.SendToBack();
            panel1.BringToFront();
             panel1.Dock= DockStyle.Fill;
             setting_open = false;
             cmd1_send = true;                       
           // Thread serverThread = new Thread(() => Com_commands("5"));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
           //  serverThread.Start();
            
        }

        public string checkweekoff()
        {
            string weekoffs = "";
            try
            {
                if (Mon.Checked)
                {
                    weekoffs = weekoffs + "Mon,";
                }
                if (Tue.Checked)
                {
                    weekoffs = weekoffs + "Tue,";
                }
                if (Wed.Checked)
                {
                    weekoffs = weekoffs + "Wed,";
                }
                if (Thu.Checked)
                {

                    weekoffs = weekoffs + "Thu,";
                }
                if (Fri.Checked)
                {
                    weekoffs = weekoffs + "Fri,";
                }

                if (Sat.Checked)
                {
                    weekoffs = weekoffs + "Sat,";
                }
                if (Sun.Checked)
                {
                    weekoffs = weekoffs + "Sun,";
                }
            }
            catch
            {

            }
            return weekoffs;
        }
        private void savebutton_Click(object sender, EventArgs e)
        {
            if(SCROLLING.Text == "Display")
            {
                if(displaytextBox.Text.Length > 12)
                {
                    MessageBox.Show("Display message can't be greter than 12 character");
                    return;
                }
            }
            else
            {
                if (displaytextBox.Text.Length > 50)
                {
                    MessageBox.Show("Display message can't be greter than 50 character");
                    return;
                }
            }
            con.updateproduction(1, Planvalue.Text, ACTUALLABLE.Text);
            con.updatesetting(Shifts.SelectedIndex, PLANNINGTIME.Text, TARGET.Text, SCROLLING.Text, displaytextBox.Text, Shift_start.Text, Shift_end.Text, brk1_start.Text, brk1_end.Text, brk2_start.Text, brk2_end.Text, checkweekoff(), TCP_IP.Text, portno.Text, Lunchbreakstart.Text, Lunchbreakend.Text, ScrollSpeed.Text, ServerIP.Text, Serverport.Text, Actualfilepath.Text,TitleValue.Text);

            MessageBox.Show("Setting updated successfully.");

            Form1_Load(sender, e);
            read_setting();
           //   cmd1_send = true;
           //   cmd2_send = true;
           //   cmd3_send = true;
            //  cmd4_send = true;
             // cmd5_send = true;

           // Thread serverThread = new Thread(() => Com_commands("5"));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
            //serverThread.Start();
            

        }

        private void Shifts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Shifts.SelectedIndex == 0)
            {
                Shift_start.Text = shift_time[0];
                Shift_end.Text = shift_time[1];
                brk1_start.Text = break_time[0];
                brk1_end.Text = break_time[1];
                brk2_start.Text = break_time[2];
                brk2_end.Text = break_time[3];
                Lunchbreakstart.Text = Lunch_time[0];
                Lunchbreakend.Text = Lunch_time[1];
            }
            else
            {
                Shift_start.Text = shift_time[2];
                Shift_end.Text = shift_time[3];
                brk1_start.Text = break_time[4];
                brk1_end.Text = break_time[5];
                brk2_start.Text = break_time[6];
                brk2_end.Text = break_time[7];
                Lunchbreakstart.Text = Lunch_time[2];
                Lunchbreakend.Text = Lunch_time[3];
            }
        }

        private void Add_holidays_Click(object sender, EventArgs e)
        {
            try
            {

                string holiday_date = Dates.Value.Day.ToString() + "-" + Dates.Value.Month.ToString() + "-" + Dates.Value.Year.ToString();
                string cmd = "INSERT INTO Holidays     (Name, Holiday_date)  VALUES        ('" + name_holidays.Text + "', '" + holiday_date + "')";
                con.insert(cmd);
            }
            catch { }
            read_setting();
        }

        private void Remove_holidays_Click(object sender, EventArgs e)
        {
            try
            {
                string cmd = "Delete from Holidays   where Holidays.SRNO=" + Holidaygrid.SelectedCells[0].Value.ToString() + "  ";
                con.delete(cmd);
            }
            catch { }

            read_setting();
        }

        private void Holidaygrid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                name_holidays.Text = Holidaygrid.SelectedCells[1].Value.ToString();
            }
            catch
            {
            }
        }       

        private void Form1_Shown(object sender, EventArgs e)
        {

            timer3.Interval = 2000;
            timer3.Enabled =true;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
             
                timer3.Enabled = false;
                if (fisrttimeread == false)
                {
                    fisrttimeread = true;
                    string lastupdated_on = con.gettable("Select * from Setting  WHERE  (Setting.ID = 1)").Rows[0]["lastupdated"].ToString();
                    DUMMY_DATE_TODAY = Convert.ToDateTime(lastupdated_on);
                    Double span = ((DateTime.Now - DUMMY_DATE_TODAY).TotalSeconds);
                    Double count = 0;
                    // MessageBox.Show("Updating please wait\r\n last updated:-"+lastupdated_on+"\r\n It may Take while!");
                    while (DUMMY_DATE_TODAY < DateTime.Now)
                    {


                        DUMMY_DATE_TODAY = DUMMY_DATE_TODAY.AddSeconds(1);
                        dummy_clock(DUMMY_DATE_TODAY);
                        count++;
                        int value = (Convert.ToInt16(((count) / (span)) * 100));
                        if (value < 0)
                        {
                            value = 0;
                        }
                        if (value > 100)
                        {
                            value = 100;
                        }
                        //progressBar1.Value = value;
                        if (count % 3600 == 0)
                        {
                          
                            this.Refresh();
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {
               // con.updateproduction(1, TARGETTOTALLABLE.Text, PLANLABLE.Text, PLANTOTALLABLE.Text, ACTUALLABLE.Text, ATCUALTOTALLABLE.Text);
                con.update("UPDATE Setting SET   lastupdated='" + lastupdate.ToString() + "' WHERE  (Setting.ID = 1)");
                /*
                Thread serverThread = new Thread(() => Com_commands("1"));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
                serverThread.Start();
                Thread.Sleep(5000);
                this.Refresh();
                serverThread = new Thread(() => Com_commands("2"));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
                serverThread.Start();
                Thread.Sleep(5000);
                this.Refresh();
                serverThread = new Thread(() => Com_commands("3"));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
                serverThread.Start();
                Thread.Sleep(5000);
                this.Refresh();
                serverThread = new Thread(() => Com_commands("4"));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
                serverThread.Start();
                Thread.Sleep(5000);
                this.Refresh();
                serverThread = new Thread(() => Com_commands("5"));// new Thread(new  ParameterizedThreadStart(voicecallstart(a)));//soundpipe.voicesoundwrite(modems.Rows[a]["voiceport"].ToString(), Convert.ToInt32(modems.Rows[a]["voicePortSpeed"].ToString()), voicebuffer[a],a);
                serverThread.Start();
                this.Refresh();
                Thread.Sleep(5000);
            */
                timer2.Enabled = true;
                timer1.Enabled = true;
                timer3.Enabled = false;
               // progressBar1.Hide();
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        bool closing;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (setting_open == true)
            {
                e.Cancel = true;                     
                settingpanel.Hide();
                settingpanel.SendToBack();
                panel1.BringToFront();
                panel1.Dock = DockStyle.Fill;
                setting_open = false;
                cmd1_send = true;
            }
            else
            {
                closing = true;
                Environment.Exit(1);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
                byte[] bb = new byte[500];
                byte[] ba = new byte[500];
                tcpclnt.Connect(clienttcpip, Convert.ToInt32(clientport));
                Stream stm = tcpclnt.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                if (SCROLLING.Text == "Display")
                {
                    string sDisplayMessage = displaytextBox.Text;
                    if (displaytextBox.Text.Length > 12)
                    {
                        sDisplayMessage = displaytextBox.Text.Substring(0, 12);
                    }
                    ba = asen.GetBytes("NKMLINE3_DISP:" + sDisplayMessage + "\r\n");
                }
                else if (SCROLLING.Text == "Scrolling")
                {
                    string sDisplayMessage = displaytextBox.Text;
                    if (displaytextBox.Text.Length > 50)
                    {
                        sDisplayMessage = sDisplayMessage.Substring(0, 50);
                    }
                    ba = asen.GetBytes("NKMLINE3_SCRL" + ScrollSpeed.Text + ":" + sDisplayMessage + "\r\n");
                }
                stm.WriteTimeout = 500;
                stm.Write(ba, 0, ba.Length);
                tcpclnt.Close();
            }
            catch(Exception)
            {

            }
        }       

        private void pnlLine_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            Pen myPen = new Pen(Color.Black);
            myPen.Width = 2;
            g.DrawLine(myPen, 0, 5, pnlLine.Width, 5);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            Pen myPen = new Pen(Color.Black);
            myPen.Width = 2;
            g.DrawLine(myPen, 0, 5, pnlLine.Width, 5);
        }

        private void btnBrightnessSend_Click(object sender, EventArgs e)
        {
            try
            {
                timer1value = timeout;
                data_received = false;
                data = "";
                cmnd_sent = true;
                TcpClient tcpclnt = new TcpClient();
                byte[] bb = new byte[500];
                byte[] ba = new byte[500];
                //UpdateRectangleShapeColor(5, true);                        
                tcpclnt.Connect(clienttcpip, Convert.ToInt32(clientport));
                Stream stm = tcpclnt.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();

                ba = asen.GetBytes("NKM_BRTS:" + tbBrightness.Value.ToString() + "\r\n");

                stm.WriteTimeout = 500;
                stm.Write(ba, 0, ba.Length);
                tcpclnt.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }               
      
    }
}
