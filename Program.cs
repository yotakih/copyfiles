using System;
using System.Collections.Generic;
using System.IO;

namespace CopyFiles
{
    class Program
    {
        const string DLM = "\t";
        Dictionary<string, string> DicContext = new Dictionary<string, string>();
        string RootDir = "";
        string TargetListFilePath = "";
        string OutputFilePath = "";
        string DstDir = "";
        StreamWriter SwOutput = null;
        static void Main(string[] args)
        {
            Console.WriteLine($"Compute Hash Start!! {DateTime.Now}");
            new Program().Proc(args, CopyFiles.CopyFilesClass.CopyFilesFunc);
            Console.WriteLine($"Compute Hash End!! {DateTime.Now}");
        }
        void Proc(string[] args, Func<string, Dictionary<string, string>, string> func)
        {
            try
            {
                if (!this.AnalizeArgs(args))
                    return;
                if (this.OutputFilePath.Trim() != "")
                    this.SwOutput = new StreamWriter(this.OutputFilePath, false);
                else
                    this.SwOutput = new StreamWriter(Stream.Null);
                using (var sr = new StreamReader(this.TargetListFilePath))
                {
                    string ln = sr.ReadLine();
                    while (!(ln is null))
                    {
                        string[] splt = ln.Split(DLM);
                        if (splt.Length == 2){
                            var interdir = splt[0];
                            var fulldir = Path.Join(this.RootDir, interdir);
                            Console.WriteLine(fulldir);
                            var ext = splt[1];
                            if (!Directory.Exists(fulldir)) return;
                            foreach (string filepath in Directory.GetFiles(fulldir))
                            {
                                if (Path.GetExtension(filepath).ToLower().Equals(ext))
                                {
                                    this.SwOutput.WriteLine(
                                        String.Format(
                                            "{1}{0}{2}{0}{3}",
                                             DLM,
                                             filepath.Substring(this.RootDir.Length),
                                             filepath,
                                             func(filepath, this.DicContext)
                                        ));
                                }
                            }
                        }
                        ln = sr.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"error: {e.Message}");
            }
            finally
            {
                if (!(this.SwOutput is null))
                    this.SwOutput.Close();
            }
        }
        bool AnalizeArgs(string[] args)
        {
            bool ret = true;
            switch (args.Length)
            {
                case 3:
                    this.RootDir = args[0];
                    this.TargetListFilePath = args[1];
                    this.DstDir = args[2];
                    ret = true;
                    break;
                default:
                    Usage();
                    ret = false;
                    break;
            }
            this.DicContext["RootDir"] = this.RootDir;
            this.DicContext["TargetListFilePath"] = this.TargetListFilePath;
            this.DicContext["DstDir"] = this.DstDir;
            return ret;
        }
        void Usage()
        {
            Console.WriteLine(
                $"Usage:\n" +
                $" arg1: select Root Dir\n" +
                $" arg2: target extension list file path\n" +
                $" arg3: destination directory path"
            );
        }
    }
}
