using System;
using System.Diagnostics;
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
            Console.WriteLine($"Аргументов { args.Length }");
            if (args.Length == 0)
            {
                string input;

                Settings.Default.lispFilePath = Settings.Default.lispFilePath;
                Settings.Default.templateFilePath = Settings.Default.templateFilePath;
                Settings.Default.deltaX = Settings.Default.deltaX;

                

                Console.Write("lispFilePath - ");
                input = Console.ReadLine();
                if (input != "")
                {
                    Settings.Default.lispFilePath = input;
                }

                Console.Write("templateFilePath - ");
                input = Console.ReadLine();
                if (input != "")
                {
                    Settings.Default.templateFilePath = input;
                }


                Console.Write("deltaX - ");
                input = Console.ReadLine();
                if (input != "")
                {
                    Int32.TryParse(input, out int deltaX);
                    Settings.Default.deltaX = deltaX;
                }

                Settings.Default.Save();
            }
            else
            {
                CopyDWGTemplate(args[0]);
                CreateLispCode(args);
                CreateLispFile();

                Process process = Process.Start(mergedFile);
                //var Acad = new AutoCAD.AcadApplication();
                //Acad.ActiveDocument.SendCommand("ii");
            }

        }

        static void CreateLispFile()
        {
            try
            {
                string lispFilePath = Settings.Default.lispFilePath;

                using (StreamWriter sw = new StreamWriter(lispFilePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(lispCode);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                Console.ReadKey();
            }
        }

        static void CopyDWGTemplate(string fPath)
        {
            try
            {                               
                string templateFilePath = Settings.Default.templateFilePath;
                
                string mergedFilePath = Path.GetDirectoryName(fPath);
                string mergedFileName = Path.GetFileName(fPath).Substring(0, 15) + ".dwg";
                mergedFile = Path.Combine(mergedFilePath, mergedFileName);

                // Use the Path.Combine method to safely append the file name to the path.
                // Will overwrite if the destination file already exists.
                File.Copy(templateFilePath, mergedFile, true);
            }

            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
                Console.ReadKey();
            }
        }

        static void CreateLispCode(string[] filePaths)
        {
            try
            {
                lispCode.Append("(defun c:ii() ");

                string fileName;
                int xPos = 0;

                // Сдвиг по оси x для следующего чертежа
                int deltaX = Settings.Default.deltaX;

                foreach (var filePath in filePaths)
                {
                    fileName = Path.GetFileName(filePath);

                    lispCode.Append("(command\"_insert\"\"" + fileName + "\"\"МАСШТАБ\"\"1\"\"ПОВОРОТ\"\"0\"\"" + xPos + ",0\")");

                    xPos += deltaX;
                }

                lispCode.Append(")");
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                Console.ReadKey();
            }
        }
    }
}
