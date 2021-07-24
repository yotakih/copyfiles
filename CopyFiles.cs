using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CopyFiles
{
    class CopyFilesClass
    {
        public static string CopyFilesFunc(string filepath, Dictionary<string, string> dicContext)
        {
            var srcDir = dicContext["RootDir"];
            var dstDir = dicContext["DstDir"];
            var dstfilepath = Path.Join(dstDir, filepath.Substring(srcDir.Length));
            CreateDirAndCopyFile(filepath, dstfilepath);

            return "";
        }

        static void CreateDirAndCopyFile(string sourceFullPath, string distFullPath)
        {
            string distDir = Path.GetDirectoryName(distFullPath);
            if(!Directory.Exists(distDir)){
                Directory.CreateDirectory(distDir);
            }

            File.Copy(sourceFullPath, distFullPath, true);
            // using(var srcStrm = new FileStream(sourceFullPath, FileMode.Open, FileAccess.Read))
            // {
            //     using(var dstStrm = new FileStream(distFullPath, FileMode.CreateNew, FileAccess.Write))
            //     {
            //         srcStrm.CopyToAsync(dstStrm);
            //     }
            // }
        }
    }
}
