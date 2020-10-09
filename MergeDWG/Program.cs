using System;
using System.Diagnostics;
using System.IO;
using System.Text;
//using AutoCAD;

namespace MergeDWG
{
    class Program
    {
        // Variable fo lisp file code
        static StringBuilder lispCode = new StringBuilder("");
        // Merged file path
        static string mergedFile;
        static void Main(string[] args)
        {
            Console.WriteLine($"Аргументов { args.Length }");
            if (args.Length == 0)
            {
                // input from console
                string input;
                // Initialize settings
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
        /// <summary>
        /// Generate lisp file for AutoCade (got to be APPLOAD by default in AC + switch SECURELOAD to 0)
        /// </summary>
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
        /// <summary>
        /// Make copy of template AutoCade file
        /// </summary>
        /// <param name="fPath"> Path to AC template </param>
        static void CopyDWGTemplate(string fPath)
        {
            try
            {                               
                string templateFilePath = Settings.Default.templateFilePath;
                
                string mergedFilePath = Path.GetDirectoryName(fPath);
                string mergedFileName = Path.GetFileNameWithoutExtension(fPath);
                if (mergedFileName.Length > 6)
                {               
                    mergedFileName = mergedFileName.Substring(0, mergedFileName.IndexOf("__")) + ".dwg";
                }
               
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
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        ///  Generate lisp code for AutoCade
        /// </summary>
        /// <param name="filePaths">File paths which we got to merge</param>
        static void CreateLispCode(string[] filePaths)
        {
            try
            {
                // "ii" - name for autocad function
                lispCode.Append("(defun c:ii() ");

                string fileName;
                int xPos = 0;

                // Offset by X for new sheet placement
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
