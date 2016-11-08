using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Jet.Framework.Utility
{
    public partial class SharpZipHelper
    {
        private static void GetDirAndFileList(DirectoryInfo dir, List<FileInfo> fileList)
        {
            DirectoryInfo[] dirs = dir.GetDirectories();
            fileList.AddRange(dir.GetFiles());

            foreach (var dirTemp in dirs)
            {
                GetDirAndFileList(dirTemp, fileList);
            }
        }

        public static void CompressDirectory(string dirFullPath, string zipFileFullPath)
        {
            if (!Directory.Exists(dirFullPath))
                throw new ApplicationException("文件不存在或者已经被删除!");
            DirectoryInfo dir = new DirectoryInfo(dirFullPath);
            List<FileInfo> fileList = new List<FileInfo>();

            GetDirAndFileList(dir, fileList);

            using (ZipOutputStream zipedStream = new ZipOutputStream(File.Create(zipFileFullPath)))
            {
                foreach (var file in fileList)
                {
                    string entryName = string.Format("{0}{1}", dir.Name, file.FullName.Replace(dirFullPath, ""));
                    ZipEntry entry = new ZipEntry(entryName);
                    using (FileStream fs = file.OpenRead())
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        zipedStream.PutNextEntry(entry);
                        zipedStream.Write(buffer, 0, buffer.Length);
                    }
                }
                zipedStream.Finish();
            }
        }
    }
}
