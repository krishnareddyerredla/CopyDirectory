using CopyDirectory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace CopyDirectoryApp
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //setup DI
            var serviceProvider = RegisterServices();

            //Select source directory
            var sourceDirectory = SelectDirectoryPath("Please press enter to select source directory......");
            WriteMessage($"Source directory path:{sourceDirectory}");
            WriteMessage(Environment.NewLine);
            //Select target directory
            var targetDirectory = SelectDirectoryPath("Please press enter to select target directory......");
            
            WriteMessage($"Target directory path:{targetDirectory}");
            WriteMessage(Environment.NewLine);
            WriteMessage($"Copying files started.....");

            //do the actual work here
            var copyFunctions = serviceProvider.GetService<ICopyFunctions>();
            var copyResult = copyFunctions.CopyDirectory(sourceDirectory, targetDirectory, WriteMessage).Result;

            HandleCopyResult(copyResult);
            WriteMessage($"Press enter to close the application.");
            Console.ReadLine();
        }

        static void HandleCopyResult(CopyResult result)
        {
            if (result == CopyResult.Success || result == CopyResult.Failed)
            {
                WriteMessage($"Copying files: {result}");
            }
            else
            {
                WriteMessage($"Copying files failed. Both source and target directories are same.");
            }
        }

        static void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }

        static string SelectDirectoryPath(string userMessage)
        {
            string path = null;
            do
            {
                WriteMessage(userMessage);
                Console.ReadLine();

                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    path = fbd.SelectedPath;
                }
            } while (path == null);

            return path;
        }

        static IServiceProvider RegisterServices()
        {
            return new ServiceCollection()
                .AddSingleton<ICopyFunctions, CopyFunctions>()
                .BuildServiceProvider();
        }
    }
}
