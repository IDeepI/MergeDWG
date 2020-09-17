using System;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Text;
//using AutoCAD;

namespace MergeDWG
{
    class Program
    {
        static StringBuilder lispCode = new StringBuilder("");
        static string mergedFile;
        static void Main(string[] args)
        {
           /* 
            string[] tmpargs = new string[4];
            tmpargs[0] = "d:\\Work\\DWG\\ПП-825_Каховская\\ПП825\\ВИЛЕ.674791.000ПЭ3__1.dwg";
            tmpargs[1] = "d:\\Work\\DWG\\ПП-825_Каховская\\ПП825\\ВИЛЕ.674791.000Э3__1.dwg";
            tmpargs[2] = "d:\\Work\\DWG\\ПП-825_Каховская\\ПП825\\ВИЛЕ.674791.000Э3__2.dwg";
            tmpargs[3] = "d:\\Work\\DWG\\ПП-825_Каховская\\ПП825\\ВИЛЕ.674791.000Э3__3.dwg";
           */
            
            CopyDWGTemplate(args[0]);
            CreateLispCode(args);
            CreateLispFile();

            Process process =  Process.Start(mergedFile);
            //var Acad = new AutoCAD.AcadApplication();
            //Acad.ActiveDocument.SendCommand("ii");
        }

        static void CreateLispFile()
        {
            string lispFilePath = ConfigurationManager.AppSettings["lispFilePath"];

            try
            {
                using (StreamWriter sw = new StreamWriter(lispFilePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(lispCode);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        static void CopyDWGTemplate(string fPath)
        {
            string templateFilePath = ConfigurationManager.AppSettings["templateFilePath"];
            string mergedFilePath = Path.GetDirectoryName(fPath);
            string mergedFileName = Path.GetFileName(fPath).Substring(0, 15) + ".dwg";
            mergedFile = Path.Combine(mergedFilePath, mergedFileName);

            try
            {
                // Use the Path.Combine method to safely append the file name to the path.
                // Will overwrite if the destination file already exists.
                File.Copy(templateFilePath, mergedFile, true);
            }

            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }
        }

        static void CreateLispCode(string[] filePaths)
        {
            lispCode.Append("(defun c:ii() ");

            string fileName;
            int xPos = 0;
            int deltaX = 0;
            // Сдвиг по оси x для следующего чертежа
            int.TryParse(ConfigurationManager.AppSettings["deltaX"], out deltaX);

            foreach (var filePath in filePaths)
            {
                fileName = Path.GetFileName(filePath);

                lispCode.Append("(command\"_insert\"\"" + fileName + "\"\"МАСШТАБ\"\"1\"\"ПОВОРОТ\"\"0\"\"" + xPos + ",0\")");
                
                xPos += deltaX; 
            }

            lispCode.Append(")");
        }
    }

}
