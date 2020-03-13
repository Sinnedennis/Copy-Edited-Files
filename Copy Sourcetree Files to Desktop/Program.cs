using System;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace CopyFilesToClipboard
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                return;
            }

            //Get filepaths into list, removing first arg which is always the project path
            var filePaths = args.Where(x => x != null && !String.IsNullOrEmpty(x))
                .Select(x => x.Replace("/", @"\")).ToList();
            filePaths.RemoveAt(0);

            var repoPath = args.First();
            var repoName = new DirectoryInfo(repoPath).Name;
            var writePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), repoName);

            Console.WriteLine("_____________________________________________________________________________________");
            Console.WriteLine("Starting file copy. {0} {1} found", filePaths.Count(), filePaths.Count() == 1 ? "file" : "files");

            //Delete any existing files
            if (Directory.Exists(writePath))
            {
                Directory.Delete(writePath, true);
            }

            Directory.CreateDirectory(writePath);

            foreach (var filePath in filePaths)
            {
                try
                {
                    var fileName = GetFileName(filePath);
                    var filePathFull = Path.Combine(repoPath, filePath);
                    var relativePath = GetRelativeFilePath(filePath);
                    var copyPathFull = Path.Combine(writePath, relativePath);
                    var copyPathDir = GetTargetDirectory(relativePath, writePath);

                    Console.WriteLine("Copying file {0} from: {1}", fileName, Path.Combine(repoPath, filePath).Replace(fileName, ""));
                    Console.WriteLine("To: {0}", Path.Combine(writePath, relativePath));
                    Console.WriteLine("_____________________________________________________________________________________");

                    if (!Directory.Exists(copyPathDir))
                    {
                        Directory.CreateDirectory(copyPathDir);
                    }

                    if (File.Exists(filePathFull))
                    {
                        File.Copy(filePathFull, copyPathFull, true);
                    }
                    else
                    {
                        Console.WriteLine("Can't find file: {0}", filePathFull);
                        Console.WriteLine("Skipping");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("_____ERROR_____");
                    Console.WriteLine("Issue copying {0}", filePath);
                    Console.WriteLine(ex.ToString());
                }
            }

            ReadKeyIfDebugging();

            return;
        }

        [Conditional("DEBUG")]
        private static void ReadKeyIfDebugging()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static string GetFileName (string filePath)
        {
            return filePath.Split('\\').Last();
        }

        private static string GetRelativeFilePath (string filePath)
        {
            if (filePath.Contains(""))
            {
                return filePath;
            }
            else
            {
                return filePath.Replace(filePath.Split('\\').First(), "");
            }
        }

        private static string GetTargetDirectory(string relativeFilePath, string writePath)
        {
            var dirArr = relativeFilePath.Split('\\').ToList();
            dirArr.RemoveAt(dirArr.Count() - 1);
            var dirPath = String.Join("\\", dirArr);
            dirPath = Path.Combine(writePath, dirPath.TrimEnd('\\'));
            return dirPath;
        }
    }
}








//if (filePaths != null && filePaths.Any())
//{
//    Clipboard.Clear();

//    List<string> paths = new List<string>();

//    foreach (string filePath in filePaths)
//    {
//        Console.WriteLine(filePath);
//        if (File.Exists(filePath))
//        {
//            paths.Add(filePath);
//        }
//    }

//    var dataObj = new DataObject();
//    dataObj.SetData(DataFormats.FileDrop, paths, true);
//    Clipboard.SetDataObject(dataObj, true);
//}