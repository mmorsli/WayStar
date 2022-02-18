using FileMover;
using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace DataProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            // filemover --source srcDir --backup bckDir --search pattern --numberfiles number --maxdays xDays
            WriteLine("Parsing command line options");

            if (args.Length < 10 )
            {
                WriteLine("Missing command line options");
                WriteLine("use the following format : --source srcDir --backup bckDir --search pattern --numberfiles number --maxdays xDays");
                ReadKey();
                return;
            }

            // Command line validation omitted for brevity
            FileOperationInfo fileOpInfo = new FileOperationInfo();
            fileOpInfo.SourceDir = args[1];
            fileOpInfo.BackupDir = args[3];
            fileOpInfo.FileCriteria = args[5]; // "*.txt"
            fileOpInfo.NumberFilesToGet = int.Parse( args[7]);
            fileOpInfo.FileDate = DateTime.Now.AddDays(-int.Parse(args[9]));


            // validate the command options
            if (!IsCommandLineOptionsValid(fileOpInfo))
            {
                WriteLine("Invalid command line options");
                ReadLine();
                return;
            }
            
            // If source dir is not valid or does not exist: return
            if (!Directory.Exists(fileOpInfo.SourceDir))
            {
                WriteLine($"ERROR: Directory '{fileOpInfo.SourceDir}' cannot be found or is invalid");
                ReadLine();
                return;
            }

            // start processing 
            ProcessDirectory(fileOpInfo);

            WriteLine("Press enter to quit.");
            ReadLine();
        }


        private static void ProcessDirectory(FileOperationInfo options)
        {
            WriteLine($"Processing Directory {options.SourceDir} to copy files of type {options.FileCriteria} to destination directoty {options.BackupDir}");


            foreach (string dirPath in Directory.GetDirectories(options.SourceDir, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(options.SourceDir, options.BackupDir));

            var filesFound = Directory.GetFiles(options.SourceDir, options.FileCriteria, SearchOption.AllDirectories);
            var filesToProcess = new List<string>();

            foreach(var file in filesFound)
            {
                DateTime dtFile = File.GetLastWriteTime(file);
                if (dtFile <= options.FileDate)
                    filesToProcess.Add(file);
            }            

            foreach (string newPath in filesToProcess)
                File.Copy(newPath, newPath.Replace(options.SourceDir, options.BackupDir), true);
        }


        private static bool IsCommandLineOptionsValid(FileOperationInfo options)
        {
            if (string.IsNullOrWhiteSpace(options.SourceDir))
                return false;

            if (string.IsNullOrWhiteSpace(options.BackupDir))
                return false;

            if (string.IsNullOrWhiteSpace(options.FileCriteria))
                return false;

            return true;
        }
    }
}
