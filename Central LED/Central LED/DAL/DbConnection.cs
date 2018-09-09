using Central_LED.Entity;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Central_LED.DAL
{
    public class DbConnection
    {

        public OleDbConnection con = new OleDbConnection();
        public OleDbCommand cmd = new OleDbCommand();
        public OleDbDataAdapter adp = new OleDbDataAdapter();

        public DbConnection()
        {
            try
            {
                string databaspath = System.IO.Directory.GetCurrentDirectory();
                con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+databaspath+"\\database\\database.accdb;Jet OLEDB:Database Password=Admin@123";
            }
            catch (OleDbException sqlex)
            {
                MessageBox.Show(sqlex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Open_Connection()
        {
            try
            {
                con.Open();
            }
            catch (OleDbException sqlex)
            {
                MessageBox.Show(sqlex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Close_Connection()
        {
            con.Close();
        }

        public bool CreateOrUpdateDisplay(Display displayData)
        {
            try
            {
                Open_Connection();

                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = con;
                string sqlQuerry;
                if (displayData.ID == 0)
                {
                    //Create display data
                    sqlQuerry = "insert into Display (DisplayName,IP,Port,Lines,DataColumns) Values('"
                    + displayData.DisplayName + "', '" + displayData.IP + "'," + displayData.Port + ","  + displayData.Lines.ToString() 
                    + ", " + displayData.DataColumns.ToString() + ")";
                    cmd.CommandText = sqlQuerry;
                    cmd.ExecuteNonQuery();

                    sqlQuerry = "SELECT @@IDENTITY";
                    cmd.CommandText = sqlQuerry;
                    var returnData = cmd.ExecuteScalar();
                    if (returnData.GetType() == typeof(int))
                    {
                        int displayDataId = (int)returnData;
                        displayData.ID = displayDataId;                       
                    }
                }
                else
                {
                    //update display data
                    sqlQuerry = "update display set DisplayName='" + displayData.DisplayName + "',IP='" + displayData.IP + "',Port=" + displayData.Port.ToString() +",Lines=" + displayData.Lines.ToString() +
                        ",DataColumns=" + displayData.DataColumns.ToString() +" where ID=" + displayData.ID.ToString();
                    cmd.CommandText = sqlQuerry;
                    cmd.ExecuteNonQuery();
                }

                string idList = string.Empty;
                //Update line data
                foreach (Line data in displayData.LineList)
                {                   
                    bool result = CreateorUpdateLine(data, displayData.ID);
                    if (result == false)
                    {
                        return false;
                    }

                    if (data.ID > 0)
                    {
                        idList = idList + data.ID + ",";
                    }
                }

                if (idList.Length > 1)
                {
                    idList = idList.Substring(0, idList.Length - 1);

                    //delete unnecessary static data address list
                    sqlQuerry = "delete from StaticLineDataColumns where LineId in (select Id from Line where DisplayControlId="
                    + displayData.ID.ToString() + " and id not in (" + idList + "))";
                    cmd.CommandText = sqlQuerry;
                    cmd.ExecuteNonQuery();

                    sqlQuerry = "delete from Line where DisplayControlId ="
                        + displayData.ID.ToString() + "  and Id not in (" + idList + ")";
                    cmd.CommandText = sqlQuerry;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Close_Connection();
            }           
            return true;
        }

        private bool CreateorUpdateLine(Line lineData, int displayControlId)
        {
            try
            {
                string sqlQuerry;
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = con;
                if (lineData.ID == 0)
                {
                    //Create display data
                    sqlQuerry = "insert into Line (DisplayControlId,LineName,Type,Parameter,ScrollingMessage,DataAddress,Unit,ScrollingType,Speed,CharacterType,BlinkingMessasge1,BlinkingMessage2,Blinking2DataAddress,ScrollingType2) Values("
                    + displayControlId.ToString() + ", '" + lineData.LineName + "', " + lineData.Type.ToString() + "," +
                    "'" + lineData.ParameterName + "','" + lineData.ScrollingMessage + "'," + lineData.DataAddress.ToString() +
                    ",'" + lineData.Unit +"'," + lineData.ScrollingType.ToString() + "," + lineData.Speed.ToString() + 
                    "," + lineData.CharacterType.ToString() + ",'" + lineData.BlinkingMessage1 + "','" + lineData.BlinkingMessage2 +"'," +
                    lineData.Blinking2DataAddress.ToString() + "," + lineData.ScrollingType2.ToString() +")";
                    cmd.CommandText = sqlQuerry;
                    cmd.ExecuteNonQuery();

                    sqlQuerry = "SELECT @@IDENTITY";
                    cmd.CommandText = sqlQuerry;
                    var returnData = cmd.ExecuteScalar();
                    if (returnData != null && returnData.GetType() == typeof(int))
                    {
                        int displayDataId = (int)returnData;
                        lineData.ID = displayDataId;
                    }

                    //If line is static also add static line address
                    if (lineData.Type == (int)Helper.LineType.Static)
                    {
                        if(lineData.StaticDataAddressList != null && lineData.StaticDataAddressList.Any())
                        {
                            foreach(var staticAddress in lineData.StaticDataAddressList)
                            {
                                CreateorUpdateStaticLineData(staticAddress, lineData.ID, displayControlId);
                            }
                        }
                    }
                }
                else
                {
                    //update display data
                    sqlQuerry = "update Line set LineName='" + lineData.LineName + "',Type=" + lineData.Type.ToString() +
                        ",Parameter='" + lineData.ParameterName + "',ScrollingMessage='" + lineData.ScrollingMessage + "'" +
                        ",DataAddress=" + lineData.DataAddress.ToString() + ",Unit='" + lineData.Unit + "',ScrollingType="
                    + lineData.ScrollingType.ToString() + ",Speed=" + lineData.Speed.ToString() + ",CharacterType="
                    + lineData.CharacterType.ToString() + ",BlinkingMessasge1='" + lineData.BlinkingMessage1 + "',BlinkingMessage2='"
                    + lineData.BlinkingMessage2 + "', Blinking2DataAddress = " + lineData.Blinking2DataAddress.ToString() + ",ScrollingType2 ="
                    + lineData.ScrollingType2.ToString() +" where ID=" + lineData.ID.ToString();
                    cmd.CommandText = sqlQuerry;
                    cmd.ExecuteNonQuery();

                    //If line is static also add static line address
                    if (lineData.Type == (int)Helper.LineType.Static)
                    {
                        if (lineData.StaticDataAddressList != null && lineData.StaticDataAddressList.Any())
                        {
                            string idList = string.Empty;
                            foreach (var staticAddress in lineData.StaticDataAddressList)
                            {
                                CreateorUpdateStaticLineData(staticAddress, lineData.ID, displayControlId);   
                                if(staticAddress.ID > 0)
                                {
                                    idList = idList + staticAddress.ID + ",";
                                }
                            }

                            if (idList.Length > 1)
                            {
                                idList = idList.Substring(0, idList.Length - 1);

                                //delete unnecessary static data address list
                                sqlQuerry = "delete from StaticLineDataColumns where LineId =" + lineData.ID.ToString() + " and DisplayControlId ="
                                    + lineData.DisplayControlID.ToString() + "  and Id not in (" + idList + ")";
                                cmd.CommandText = sqlQuerry;
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }          
        }

        private bool CreateorUpdateStaticLineData(StaticLineDataColumns staticAddress, int lineId, int displayControlId)
        {
            try
            {
                string sqlQuerry;
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = con;

                if (staticAddress.ID == 0)
                {
                    //insert data
                    sqlQuerry = "insert into StaticLineDataColumns(LineId,DataIndex,DataColumnName,DataAddress,DisplayControlId) Values("
                        + lineId.ToString() + "," + staticAddress.Index.ToString() + ",'"
                        + staticAddress.DataColumnName + "'," + staticAddress.DataAddress + ","
                    + displayControlId.ToString() + ")";
                    cmd.CommandText = sqlQuerry;
                    cmd.ExecuteNonQuery();

                    sqlQuerry = "SELECT @@IDENTITY";
                    cmd.CommandText = sqlQuerry;
                    var returnData = cmd.ExecuteScalar();
                    if (returnData != null && returnData.GetType() == typeof(int))
                    {
                        int displayDataId = (int)returnData;
                        staticAddress.ID = displayDataId;
                    }
                }
                else
                {
                    //updateData
                    sqlQuerry = "update StaticLineDataColumns set DataAddress=" + staticAddress.DataAddress.ToString() +
                        " where ID =" + staticAddress.ID.ToString();
                    cmd.CommandText = sqlQuerry;
                    cmd.ExecuteNonQuery();
                }                                            

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveDisplay(Display displayData)
        {
            try
            {
                Open_Connection();

                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = con;
                string sqlQuerry;
                sqlQuerry = "delete from Display where ID =" + displayData.ID.ToString();
                cmd.CommandText = sqlQuerry;
                cmd.ExecuteNonQuery();

                sqlQuerry = "delete from StaticLineDataColumns where DisplayControlId =" + displayData.ID.ToString();
                cmd.CommandText = sqlQuerry;
                cmd.ExecuteNonQuery();

                sqlQuerry = "delete from Line where DisplayControlId =" + displayData.ID.ToString();
                cmd.CommandText = sqlQuerry;
                cmd.ExecuteNonQuery();              
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Close_Connection();
            }
            return true;
        }

        public List<Entity.Display> GetDisplayList()
        {
            try
            {
                Open_Connection();
                List<Entity.Display> displayDataList = new List<Display>();

                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = con;
                string sqlQuerry;
                sqlQuerry = "select * from Display";
                cmd.CommandText = sqlQuerry;
                var reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Display data = new Display();
                        data.ID = (int)reader["ID"];
                        data.IP = reader["Ip"].ToString();
                        data.Port = (int)reader["Port"];
                        data.DisplayName = reader["DisplayName"].ToString();
                        data.Lines = (int)reader["Lines"];
                        data.DataColumns = (int)reader["DataColumns"];
                        data.LineList = new List<Line>();
                        displayDataList.Add(data);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    reader.Close();
                }

                sqlQuerry = "select * from Line";
                cmd.CommandText = sqlQuerry;
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Line data = new Line();
                        data.ID = (int)reader["ID"];
                        data.DisplayControlID = (int)reader["DisplayControlId"];
                        data.LineName = reader["LineName"].ToString();
                        data.Type = (int)reader["Type"];
                        data.ParameterName = reader["Parameter"].ToString();                  
                        data.ScrollingMessage = reader["ScrollingMessage"].ToString();
                        data.DataAddress = (int)reader["DataAddress"];
                        data.Blinking2DataAddress = (int)reader["Blinking2DataAddress"];
                        data.Unit = reader["Unit"].ToString();
                        data.ScrollingType = (int)reader["ScrollingType"];
                        data.Speed = (int)reader["Speed"];
                        data.CharacterType = (int)reader["CharacterType"];
                        data.BlinkingMessage1 = reader["BlinkingMessasge1"].ToString();
                        data.BlinkingMessage2 = reader["BlinkingMessage2"].ToString();                        
                        data.ScrollingType2 = (int)reader["ScrollingType2"];
                        var relatedDisplayData = displayDataList.FirstOrDefault(display => display.ID == data.DisplayControlID);
                        if (relatedDisplayData != null)
                        {                            
                            relatedDisplayData.LineList.Add(data);
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    reader.Close();
                }

                sqlQuerry = "select * from StaticLineDataColumns";
                cmd.CommandText = sqlQuerry;
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        StaticLineDataColumns data = new StaticLineDataColumns();
                        data.ID = (int)reader["ID"];
                        data.LineID = (int)reader["LineId"];
                        data.Index = (int)reader["DataIndex"];
                        data.DataColumnName = reader["DataColumnName"].ToString();
                        data.DataAddress = (int)reader["DataAddress"];
                        data.DisplayControlId = (int)reader["DisplayControlId"];

                        var relatedDisplayData = displayDataList.FirstOrDefault(display => display.ID == data.DisplayControlId);
                        if (relatedDisplayData != null && relatedDisplayData.LineList != null && relatedDisplayData.LineList.Any())
                        {
                            var relatedLine = relatedDisplayData.LineList.FirstOrDefault(line => line.ID == data.LineID);
                            if(relatedLine != null)
                            {
                                if(relatedLine.StaticDataAddressList == null)
                                {
                                    relatedLine.StaticDataAddressList = new List<StaticLineDataColumns>();
                                }
                                relatedLine.StaticDataAddressList.Add(data);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    reader.Close();
                }

                return displayDataList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Close_Connection();
            }
        }

        public Modbus GetModbusSetting()
        {
            try
            {
                Open_Connection();
                Modbus modbusData = new Modbus();

                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = con;
                string sqlQuerry;
                sqlQuerry = "select * from Modbus where ID=2";
                cmd.CommandText = sqlQuerry;
                var reader = cmd.ExecuteReader();
                try
                {
                    reader.Read();
                    modbusData.ID = (int)reader["Id"];
                    modbusData.Address = (int)reader["Address"];
                    modbusData.ComPort = reader["ComPort"].ToString();
                    modbusData.DataType = (int)reader["DataType"];
                    modbusData.DeviceId = (int)reader["DeviceId"];
                    modbusData.FunctionType = (int)reader["FunctionType"];
                    modbusData.Length = (int)reader["Length"];
                    modbusData.Type = (int)reader["Type"];
                    modbusData.UpdateTime = (int)reader["UpdateTime"];
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    reader.Close();
                }

                return modbusData;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Close_Connection();
            }
        }

        public bool SaveModbusSetting(Modbus modbusData)
        {
            try
            {
                Open_Connection();

                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = con;
                string sqlQuerry;
                sqlQuerry = "update Modbus set DeviceId=" + modbusData.DeviceId.ToString() + ",Address=" + modbusData.Address.ToString() + ",Length="
                + modbusData.Length.ToString() + ",UpdateTime=" + modbusData.UpdateTime.ToString() + ",FunctionType=" + modbusData.FunctionType.ToString() + ",ComPort='"
                + modbusData.ComPort + "',Type=" + modbusData.Type.ToString() + ",DataType="+ modbusData.DataType.ToString() +" where ID=2";
                cmd.CommandText = sqlQuerry;
                cmd.ExecuteNonQuery();               
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Close_Connection();
            }
            return true;
        }
    }
}
