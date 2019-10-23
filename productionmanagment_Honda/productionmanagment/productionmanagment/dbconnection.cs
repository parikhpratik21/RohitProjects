using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.Windows.Forms;
using productionmanagment;

namespace DBCON
{
    class dbconnection
    {
    public NotifyIcon a = new NotifyIcon();
        
        public OleDbConnection con  = new OleDbConnection();
        public OleDbCommand cmd = new OleDbCommand();
        public OleDbDataAdapter adp = new OleDbDataAdapter();
       
        public bool notclick = false;
        DataSet ds = new DataSet();
        string StrError;
        public void sendnotification(string tital, string msg)
        {
            a.Visible = true;
            a.Icon = new Icon("download.ico");
            a.ShowBalloonTip(5000,tital,msg, ToolTipIcon.Info);
        
        }
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

            }
        }
        public void localnotification(string tital, string msg1)
        {
            a.Visible = true;
            a.Icon = new Icon(Application.StartupPath+ "\\download.ico");
            a.ShowBalloonTip(500, tital, msg1, ToolTipIcon.Info);
            //  Form.ActiveForm.Show();

        }
        public dbconnection()
        {
            try
            {
                this.a.Click += new EventHandler(a_Click);
                // con.ConnectionString = "Data Source=PC-3;Initial Catalog=eskool_V_2.0;Persist Security Info=True;User ID=sa;Password=ska";
                // con.ConnectionString = "Data Source=SKAADMINSERVER;Initial Catalog=eSchool;Persist Security Info=True;User ID=sa;Password=ska";
                // con.ConnectionString = "Data Source=SKA-HAPPY;Initial Catalog=KT;User ID=sa;Password=ska";
                string databaspath = Application.StartupPath.ToString() ;
                con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+databaspath+"\\database\\database.accdb;Jet OLEDB:Database Password=Admin@123";
               // con.ConnectionString = "Provider=Microsoft.jet.OLEDB.4.0;Data Source="+databaspath+"\\database\\database.accdb;Jet OLEDB:Database Password=Admin@123";
              
                //con.ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\bin\\Debug\\DataBase\\GSPC.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
               // con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBTEST"].ConnectionString);
            }
            catch (OleDbException sqlex)
            {
                StrError = sqlex.Message;
                LogManager.WriteErrors("DBConnection", "DBConnection", sqlex);
            }
            catch (Exception ex)
            {
                StrError = ex.Message;
                LogManager.WriteErrors("DBConnection", "DBConnection", ex);
            }
        }
        private void a_Click(object sender, EventArgs e)
        {
            notclick = true;
        }
        public void Open_Connection()
        {
            try
            {
                con.Open();
            }
            catch (OleDbException sqlex)
            {
                StrError = sqlex.Message;
                LogManager.WriteErrors("DBConnection", "Open_Connection", sqlex);
            }
            catch (Exception ex)
            {
                StrError = ex.Message;
                LogManager.WriteErrors("DBConnection", "Open_Connection", ex);
            }
        }
        public void Close_Connection()
        {
            con.Close();
        }
        bool READING=false;
        public DataTable gettable(string query)
        {
            while (READING)
            { }
            READING = true;
            DataTable dt = new DataTable();
            try
            {
                OleDbDataAdapter adp = new OleDbDataAdapter(query, con);
               
                adp.Fill(dt);
            }
            catch { READING = false; }
            READING = false;
            return dt;
        }
        
        public void insert(string query)
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.CommandType = CommandType.Text;
                string state = con.State.ToString();
                if (state == "Closed")
                {
                    con.Open();
                }
                
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (OleDbException ex)
            {
                con.Close();
                LogManager.WriteErrors("DBConnection", "insert", ex);
            }
        }
        public void delete(string query)
        {
            try
            {
                string state = con.State.ToString();
                if (state == "Closed")
                {
                    con.Open();
                }
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException sqlex)
            {
                LogManager.WriteErrors("DBConnection", "delete", sqlex);
                con.Close();
            }
        }
        public void update(string query)
        {
            try
            {
                while (READING)
                { }
                READING = true;
                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }

                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch
                {
                    READING = false;
                }
          
            }
            catch (OleDbException ex)
            {
                READING = false;
                LogManager.WriteErrors("DBConnection", "update", ex);
          
            }
            READING = false;
          
        }
        
        public string updatemodem(int id, string modemname, string smscomport, string smsspeed, bool smsact, string voicecomport, string voicespeed, bool voiceact, string billdatesms, string billdatecall, long smslimit, long voicelimit, string smsreapet, string callreapet, bool activated, string calltime)
        {
            string status =" ";
            try
            {
                while (READING)
                { }
                READING = true;
          
                string cmnd="UPDATE    Modem SET   ModemName ='"+modemname.ToString()+"', SmsPort = '"+smscomport+"', SmsPortSpeed = '"+smsspeed+"', ActivateSmsPort = "+smsact.ToString()+", VoicePort = '"+voicecomport+"', VoicePortSpeed = '"+voicespeed+"', VoiceActivate = "+voiceact.ToString()+",billdatesms = '"+billdatesms.ToString()+"',billdatecall = '"+billdatecall.ToString()+"', SmsLimit = "+smslimit.ToString()+",calltimer = '"+calltime.ToString()+"', VoiceLimit = "+voicelimit.ToString()+", smsReapet = '"+smsreapet.ToString()+"', callReapet = '"+callreapet.ToString()+"' ,Activated="+activated.ToString()+" WHERE (Modem.ModemID ="+id.ToString()+")";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                
                cmd.ExecuteNonQuery();
                con.Close();
                status = "Sucsess";
                }
                catch(Exception ex)
                {
                    status = ex.ToString();
                    LogManager.WriteErrors("DBConnection", "updatemodem", ex);
                }
             }
            catch (OleDbException ex)
            {
                READING = false;
                status = ex.ToString();
                LogManager.WriteErrors("DBConnection", "updatemodem", ex);
            }
            READING = false;
          
            return status;
        }
        public void insertcall(int ch, string to,string FROM,DateTime datereceived,string status)
        {
            try
            {
                while (READING)
                { }
                READING = true;
          
                string DATECALL = datereceived.Date.Day.ToString() + ":" + datereceived.Date.Month.ToString() + ":" + datereceived.Date.Year.ToString() + ":" + datereceived.TimeOfDay.Hours.ToString() + ":" + datereceived.TimeOfDay.Minutes.ToString();
                string cmnd = "INSERT INTO INcomingCall ([Received channel], [Received To], [From], [Datetime], Status) VALUES     ("+ch.ToString()+", '"+to+"', '"+FROM+"', #"+datereceived+"#, '"+status+"')";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state=con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                cmd.ExecuteNonQuery();
             
                }
                catch { }
            }
            catch (OleDbException ex)
            {
                READING = false;
                LogManager.WriteErrors("DBConnection", "insertcall", ex);
            }
            READING = false;
       }
        public void Update_sim_remarks_byid(int id, string sim, string mo_no, string remark)
        {
            try
            {
                string cmnd ="UPDATE       Device SET MobileNo = '"+sim+"', Modem = '"+mo_no+"', DeviceNo = '"+remark+"' WHERE        (Device.DeviceID = "+id.ToString()+")";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();

                }
                catch { }
            }
            catch (OleDbException ex)
            {
                LogManager.WriteErrors("DBConnection", "Update_sim_remark_byid", ex);
            }
        }
        public void Update_sim_remarks_bysim(string sim, string mo_no, string remark)
        {
            try
            {
                string cmnd ="UPDATE       Device SET MobileNo = '"+mo_no+"', Modem = '"+remark+"', DeviceNo = '"+sim+"' WHERE        (Device.DeviceNo = '"+sim+"')";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();

                }
                catch(Exception ex) {
                    LogManager.WriteErrors("DBConnection", "Update_sim_remarks_bysim", ex);
                }
            }
            catch (OleDbException ex)
            {
                LogManager.WriteErrors("DBConnection", "Update_sim_remarks_bysim", ex);
            }
        }
        public void insert_sim_remarks(string sim, string mo_no, string remark)
        {
            try
            {

                string cmnd ="INSERT INTO Device (DeviceNo, MobileNo, Modem)VALUES        ('"+sim+"', '"+mo_no+"', '"+remark+"')";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();

                }
                catch { }
            }
            catch (OleDbException ex)
            {
                LogManager.WriteErrors("DBConnection", "insert_sim_remarks", ex);
            }
        }
        public DataTable updatesmscounter(int ch,Int64 smsremain)
        {
            try
            {
                string cmnd ="UPDATE    Modem SET [Sms Remainig] ='" + smsremain.ToString() + "' WHERE     (ModemID = " + (ch + 1).ToString() + ")";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex) {
                    LogManager.WriteErrors("DBConnection", "updatesmscounter", ex);
                }
                
            }
            catch (OleDbException ex)
            {
                LogManager.WriteErrors("DBConnection", "updatesmscounter", ex);
            }
            return gettable("select * from modem");
        }
        public DataTable updatecallcounter(int ch, Int64 callremain)
        {
            try
            {
               string cmnd = "UPDATE    Modem SET [Call Remaining] ='" + callremain.ToString() + "' WHERE     (ModemID = " + (ch + 1).ToString() + ")";
               OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                }
                catch { }
               
            }
            catch (OleDbException)
            {
            }
            return gettable("select * from modem");
        }
        public void insertsms(int ch, string to,string FROM,string smstext,DateTime datereceived)
        {
            try
            {
                while (READING)
                { }
                READING = true;
                string cmnd = "        INSERT INTO IncomingSms ([Received channel], [Received To], [From], [SMS Text], [Received Date], [Reading Date])VALUES     (" + (ch+1).ToString() + ",'" + to + "', '" + FROM + "', '" + smstext + "', #"+datereceived.ToString()+"#, #"+DateTime.Now.ToShortDateString()+"#)";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state=con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                cmd.ExecuteNonQuery();
                }
                catch { READING = false; }

           }
            catch (OleDbException )
            {
                READING = false;
            }
            READING = false;
 
       }
        public void updateOutcall(string callid,int ch ,string simno, DateTime datesent, string status, string sendstatus)
        {
            try
            {
                while (READING)
                { }
                READING = true;
                string DATECALL = datesent.Date.Day.ToString()+":"+datesent.Date.Month.ToString()+":"+datesent.Date.Year.ToString()+":"+datesent.TimeOfDay.Hours.ToString()+":"+datesent.TimeOfDay.Minutes.ToString();

              //  INSERT INTO OutGoingcalls ([Calling channel], [Call to], [Sound Source],[Reason], [Call Date], Status)VALUES     (" + ch.ToString() + ", '" + to + "', '" + soundsourece + "','" + reasonforsend + "', #" + datereceived.ToString() + "#, '" + status + "')";
                string cmnd = "UPDATE    OutGoingcalls SET [Calling channel] = '" + ch + "',[Calling SIM] = '" + simno + "', [call Date] = '" + DATECALL.ToString() + "', Status = '" + status.ToString() + "', [call Status] = '" + sendstatus + "' WHERE     (OutGoingcalls.[call ID] = " + callid.ToString() + ")";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                }
                catch { }
            }
            catch (OleDbException)
            {
                READING = false;
            }
            READING = false;
        }
        public void updatelicence(string lic,string pckey)
        {
            try
            {
                //  INSERT INTO OutGoingcalls ([Calling channel], [Call to], [Sound Source],[Reason], [Call Date], Status)VALUES     (" + ch.ToString() + ", '" + to + "', '" + soundsourece + "','" + reasonforsend + "', #" + datereceived.ToString() + "#, '" + status + "')";
                string cmnd = "UPDATE    licence SET [pckey] = '" + pckey + "',[licence] = '" + lic + "' WHERE     (licence.[ID] =1)";

                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                    //con.Close();

                }
                catch { }
            }
            catch (OleDbException)
            {
            }
        }
        public void updateOutsms(int ch,Int64 smsid,string simno,DateTime datesent,string status,string sendstatus)
        {
        try
            {
                while (READING)
                { }
                READING = true;
          
                string cmnd = "UPDATE    OutGoingSms SET [Sending channel] = " + (ch+1).ToString() + ",[Sending SIM] = '  ', [Sent Date] = #" + datesent.ToString() + "#, Status = '" + status.ToString() + "', [Send Status] = '" + sendstatus + "' WHERE     (OutGoingSms.[Sms ID] = " + smsid.ToString() + ")";
        
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state=con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                cmd.ExecuteNonQuery();
                //con.Close();
                }
                catch { READING = false; }
                
            }
            catch (OleDbException )
            {
                READING = false;
            }
        READING = false;
       }
        public void insertOutcall(int ch, string to, string soundsourece, string reasonforsend, DateTime datereceived, string status)
        {
            try
            {
                while (READING)
                { }
                READING = true;
          
                string DATECALL = datereceived.Date.Day.ToString() + ":" + datereceived.Date.Month.ToString() + ":" + datereceived.Date.Year.ToString() + ":" + datereceived.TimeOfDay.Hours.ToString() + ":" + datereceived.TimeOfDay.Minutes.ToString();
                string cmnd = "INSERT INTO OutGoingcalls ([Calling channel], [Call to], [Sound Source],[Reason], [Call Date], Status)VALUES     (" + ch.ToString() + ", '" + to + "', '" + soundsourece + "','" + reasonforsend + "', '" + DATECALL.ToString() + "', '" + status + "')";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                    //con.Close();

                }
                catch { }
               }
            catch (OleDbException)
            {
                READING = false;
            }
            READING = false;
        }
        public void insertblockednumber(string number, string detail)
        {
            try
            {
                string cmnd = " INSERT INTO BlockedCantacts  ([Mo No], Reson, [Date created]) VALUES     ('" + number + "','" + detail + "','" + DateTime.Now.ToString() + "')";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                    //con.Close();

                }
                catch { }

            }
            catch (OleDbException)
            {
            }
        }
        public void Deleteblockednumber(string id)
        {
            try
            {
                string cmnd = "DELETE FROM BlockedCantacts WHERE     (BlockedCantacts.ID = "+id.ToString()+")";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                    //con.Close();

                }
                catch { }
             }
            catch (OleDbException)
            {
            }
        }
        public void updateresponce(string smsresponce,bool smsactive,string callrespnce,string callactivesound,string callreject)
        {
            try
            {
                string cmnd = "UPDATE    Responder SET Respond = '"+smsresponce+"', Active = "+smsactive+" WHERE     (Responder.Responder = 1)";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                }
                catch { }
                cmnd = "UPDATE    Responder SET Respond = '" + callrespnce + "', type = '" + callreject + "',sound = '" + callactivesound + "' WHERE     (Responder.Responder = 2)";
                 cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                    //con.Close();

                }
                catch { }

            }
            catch (OleDbException)
            {
            }
        }
         public void insertOutsms(int ch, string to,string smstext,string reasonforsend,DateTime datereceived,string status)
        {
            try
            {
                while (READING)
                { 
                }
                READING = true;
                string cmnd = "INSERT INTO OutGoingSms ([Sending channel], [Sent To], [SMS Text],[Reason], [Sent Date], Status)VALUES     (" + (ch+1).ToString() + ", '" + to + "', '" + smstext + "','" + reasonforsend + "', #" + datereceived.ToString() + "#, '" + status + "')";
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state=con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                cmd.ExecuteNonQuery();
                //con.Close();
 
                }
                catch { }

                           }
            catch (OleDbException )
            {
                READING = false;

            }
            READING = false;

         }
         public void insertSchedual(string Schedualname, string type, string Contactfile, string smstext, string Soundfile, string schdate, string schtime, string repeat, string datecreted,bool active)
         {
             try
             {
                 string cmnd = "INSERT INTO Schduled ([Schedual name], [Type], [Contact file], [SMS Text], [Sound File], [Schedual Date], [Schedual Time], Reapet, [Date Created],Active) VALUES     ('"+Schedualname+"', '"+type+"', '"+Contactfile+"', '"+smstext+"', '"+Soundfile+"', '"+schdate+"', '"+schtime+"', '"+repeat+"','"+DateTime.Now.ToString()+"',"+active.ToString()+")";
                // = "INSERT INTO OutGoingSms ([Sending channel], [Sent To], [SMS Text],[Reason], [Sent Date], Status)VALUES     (" + ch.ToString() + ", '" + to + "', '" + smstext + "','" + reasonforsend + "', #" + datereceived.ToString() + "#, '" + status + "')";
                 OleDbCommand cmd = new OleDbCommand(cmnd, con);
                 cmd.CommandType = CommandType.Text;
                 try
                 {
                     string state = con.State.ToString();
                     if (state == "Closed")
                     {
                         con.Open();
                     }
                     cmd.ExecuteNonQuery();
                     //con.Close();

                 }
                 catch { }

             }
             catch (OleDbException)
             {
             }
         }
         public void updateSchedualfull(string Scid,string Schedualname, string type, string Contactfile, string smstext, string Soundfile, string schdate, string schtime, string repeat, string datecreted,bool active)
         {
             try
             {
                 string cmnd = "UPDATE    Schduled SET [Schedual name] = '" + Schedualname + "', Type = '" + type + "', [Contact file] = '" + Contactfile + "', [SMS Text] = '" + smstext + "', [Sound File] = '" + Soundfile + "', [Schedual Date] = '" + schdate + "',  [Schedual Time] = '" + schtime + "', Reapet = '" + repeat + "',Active=" + active.ToString()+" WHERE     (Schduled.SRNO =" + Scid + " )";
                 // "INSERT INTO Schduled ([Schedual name], [Type], [Contact file], [SMS Text], [Sound File], [Schedual Date], [Schedual Time], Reapet, [Date Created]) VALUES     ('" + Schedualname + "', '" + type + "', '" + Contactfile + "', '" + smstext + "', '" + Soundfile + "', '" + schdate + "', '" + schtime + "', '" + repeat + "'," + DateTime.Now.ToString() + ")";
                 // = "INSERT INTO OutGoingSms ([Sending channel], [Sent To], [SMS Text],[Reason], [Sent Date], Status)VALUES     (" + ch.ToString() + ", '" + to + "', '" + smstext + "','" + reasonforsend + "', #" + datereceived.ToString() + "#, '" + status + "')";
                 OleDbCommand cmd = new OleDbCommand(cmnd, con);
                 cmd.CommandType = CommandType.Text;
                 try
                 {
                     string state = con.State.ToString();
                     if (state == "Closed")
                     {
                         con.Open();
                     }
                     cmd.ExecuteNonQuery();
                     //con.Close();

                 }
                 catch { }

             }
             catch (OleDbException)
             {
             }
         }
         public void updatenextSchedual(int schid,string schdate, string schtime, string repeat)
         {
             try
             {
                 string cmnd = "UPDATE    Schduled SET [Schedual Date] = '" + schdate + "',  [Schedual Time] = '" + schtime + "', Reapet = '" + repeat + "' WHERE     (Schduled.SRNO =" + schid + " )";
                 // = "INSERT INTO OutGoingSms ([Sending channel], [Sent To], [SMS Text],[Reason], [Sent Date], Status)VALUES     (" + ch.ToString() + ", '" + to + "', '" + smstext + "','" + reasonforsend + "', #" + datereceived.ToString() + "#, '" + status + "')";
                 OleDbCommand cmd = new OleDbCommand(cmnd, con);
                 cmd.CommandType = CommandType.Text;
                 try
                 {
                     string state = con.State.ToString();
                     if (state == "Closed")
                     {
                         con.Open();
                     }
                     cmd.ExecuteNonQuery();
                     //con.Close();

                 }
                 catch { }

             }
             catch (OleDbException)
             {
             }
         }

         public void updatesetting(int id, string plannigtime, string target, string display, string displaytext, string s1start, string s1end, string s1b1start, string s1b1end, string s1b2start, string s1b2end,string weekoff,string ip,string port,string lunchstart,string lunchend,string scrollspeed,string myip,string myport,string file, string TitleValue)
        {
            try
            {
                string cmnd = "";
                if (id == 0)
                {
                    cmnd = "UPDATE       Setting SET  COMPORT = '0', COMSPEED = '" + TitleValue + "', PLANNINGTIME = '" + plannigtime + "', TARGET = '" + target + "', SCROLLING ='" + display + "', SCROLLINGTEXT = '" + displaytext + "',  SHIFT1START = '" + s1start + "',  SHIFT1END = '" + s1end + "',  SHIFT1BREAK1START = '" + s1b1start + "', SHIFT1BREAK1END = '" + s1b1end + "', SHIFT1BREAK2START = '" + s1b2start + "',  SHIFT1BREAK2END = '" + s1b2end + "',WEEKOFFS = '" + weekoff + "',COMMODE = '0',TCPIP = '" + ip + "',TCPPORT = '" + port + "',shift1lunchstart='" + lunchstart + "', shift1lunchend='" + lunchend + "', scroll_speed='" + scrollspeed + "',myip='" + myip + "',myport='" + myport + "',filepath='" + file + "' WHERE  (Setting.ID = 1)";
                 }
                else 
                {
                    cmnd = "UPDATE       Setting SET  COMPORT = '0', COMSPEED = '" + TitleValue + "', PLANNINGTIME = '" + plannigtime + "', TARGET = '" + target + "', SCROLLING ='" + display + "', SCROLLINGTEXT = '" + displaytext + "',  SHIFT2START = '" + s1start + "',  SHIFT2END = '" + s1end + "',  SHIFT2BREAK1START = '" + s1b1start + "', SHIFT2BREAK1END = '" + s1b1end + "', SHIFT2BREAK2START = '" + s1b2start + "',  SHIFT2BREAK2END = '" + s1b2end + "',WEEKOFFS = '" + weekoff + "',COMMODE = '0',TCPIP = '" + ip + "',TCPPORT = '" + port + "',shift2lunchstart='" + lunchstart + "', shift2lunchend='" + lunchend + "', scroll_speed='" + scrollspeed + "',myip='" + myip + "',myport='" + myport + "',filepath='" + file + "' WHERE  (Setting.ID = 1)";
                }
                OleDbCommand cmd = new OleDbCommand(cmnd, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    string state = con.State.ToString();
                    if (state == "Closed")
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                    //con.Close();

                }
                catch(Exception ex) {
                    LogManager.WriteErrors("DBConnection", "updatesetting", ex);
                }
               

            }
            catch (OleDbException ex)
            {
                LogManager.WriteErrors("DBConnection", "updatesetting", ex);
            }
        }
         public void updateproduction(int id, string plan, string actual)
         {
             try
             {
                 string cmnd = "";
                 cmnd = "UPDATE       PRODUCTION SET  TARGETTOTAL = '0', PLANNING = '" + plan + "', PLANNINGTOTAL = '0', ACTUAL = '" + actual + "', ACTUALTOTAL ='0'WHERE  (PRODUCTION.ID = 1)";
                 
                 OleDbCommand cmd = new OleDbCommand(cmnd, con);
                 cmd.CommandType = CommandType.Text;
                 try
                 {
                     string state = con.State.ToString();
                     if (state == "Closed")
                     {
                         con.Open();
                     }
                     cmd.ExecuteNonQuery();
                     //con.Close();

                 }
                 catch(Exception ex) {
                     LogManager.WriteErrors("DBConnection", "updateProduction", ex);
                 }


             }
             catch (OleDbException ex)
             {
                 LogManager.WriteErrors("DBConnection", "updateProduction", ex);
             }
         }
         
        public void deletedv(string id, string dvno, string mono, string comport, string name)
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand("delete FROM Device WHERE  (DeviceID = " + id + ")", con);//+ "update device set DeviceID=" + id + ", DeviceNo='" + dvno + "',MobileNo='" + mono + "',ModemID=" + comport + ",ComponyName='" + name + "',Createddate=#" + DateTime.Now.ToString() + "# where (DeviceID=" + id + ")", con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            catch (OleDbException )
            {
            }
        }
        public void deletemodem(string id, string modemname, string speed, string comport)
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand("DELETE FROM Modem WHERE (ModemID = "+id+")", con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            catch (OleDbException )
            {
            }
        }
        public void REDINGINSERT(string id, string cv, string cvt, string cvy,string date)
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand("INSERT INTO Reading (DeviceId, Corrected_Volume, CvToday, CvYesterday, [Datetime], CreatedDate) VALUES(" + id + ", '" + cv + "', '" + cvt + "', '" + cvy + "', #"+date+"#, #"+DateTime.Now.ToString()+"#)", con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            catch (OleDbException )
            {
            }
        }
        public void savedv(string id, string dvno, string mono, string comport, string name)
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand("INSERT INTO Device  (DeviceNo, MobileNo, Modem , ComponyName , CreatedDate)values('" + dvno + "','" + mono + "','" + comport + "','" + name + "',#" + DateTime.Now.ToString() + "#)", con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            catch (OleDbException )
            {
            }
        }
        public void backup()
        {
            try
            {

                OleDbCommand cmd = new System.Data.OleDb.OleDbCommand("backup database |DataDirectory|\\database\\database.accdb to disk='c:/db.bak'", con);
               // cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            catch (OleDbException )
           {
            
            }
        }
        
        public void Savemodem(string id, string modemname, string speed, string comport)
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand("INSERT INTO Modem (ModemName, Speed, Comport)VALUES     ('"+modemname+"', '"+speed+"', '"+comport+"')", con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            catch (OleDbException )
            {
            }
        }
    }
}
