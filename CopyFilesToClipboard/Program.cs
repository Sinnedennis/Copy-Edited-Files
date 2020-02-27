using System;
using System.Linq;
using System.IO;

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
            Console.WriteLine("Starting file copy. {0} Files found", filePaths.Count());

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
                    var filePathFull = Path.Combine(repoPath, filePath);
                    var relativePath = GetRelativeFilePath(filePath);
                    var copyPathFull = writePath + relativePath;
                    var copyPathDir = GetTargetDirectory(relativePath, writePath);

                    Console.WriteLine("Copying file from: {0}", Path.Combine(repoPath, filePath));
                    Console.WriteLine("To: {0}", Path.Combine(writePath, relativePath));
                    Console.WriteLine("_____________________________________________________________________________________");

                    if (!Directory.Exists(copyPathDir))
                    {
                        Directory.CreateDirectory(copyPathDir);
                    }

                    File.Copy(filePathFull, copyPathFull, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("_____ERROR_____");
                    Console.WriteLine("Issue copying {0}", filePath);
                    Console.WriteLine(ex.ToString());
                }
            }
            ////Console.ReadKey();
            return;
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
            dirPath = writePath + dirPath.TrimEnd('\\');
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