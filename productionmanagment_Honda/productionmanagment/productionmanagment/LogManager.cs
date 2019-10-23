using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace productionmanagment
{
    public static class LogManager
    {
        public static void WriteErrors(string className, string methodName,Exception ex)
        {
            try
            {
                var directoryPath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                if (System.IO.Directory.Exists(directoryPath) == false)
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }

                var fileName = "Logs" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                var filePath = directoryPath + "\\" + fileName;

                string fileData = DateTime.Now.ToShortTimeString() + "  " + className + "  " + methodName + ex.Message + Environment.NewLine
                        + ex.ToString();

                if (System.IO.File.Exists(filePath) == false)
                {                                
                    System.IO.File.WriteAllText(filePath, fileData);
                }
                else
                {                    
                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        sw.WriteLine(Environment.NewLine);
                        sw.WriteLine(fileData);                        
                    }	
                }

            }
            catch(Exception)
            {

            }
        }
    }
}
