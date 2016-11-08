using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Jet.Framework.Utility
{
    public static class FileHelper
    {
#if !SILVERLIGHT
        /// <summary>
        /// 获得文件夹大小
        /// </summary>
        public static long GetDirSize(string dirPath)
        {
            long totalSize = 0;
            string[] subFilePaths = Directory.GetFiles(dirPath);
            foreach (var subFilePath in subFilePaths)
            {
                FileInfo fileInfo = new FileInfo(subFilePath);
                totalSize += fileInfo.Length;
            }

            string[] subDirPaths = Directory.GetDirectories(dirPath);
            foreach (var subDirPath in subDirPaths)
            {
                totalSize += FileHelper.GetDirSize(subDirPath);//递归
            }
            return totalSize;
        }
#endif

        public const string FileNameInValidChars = "\\/:*?\"<>|";

        public static string ReplaceFileNameInValidChars(string fileName, string tostr = "")
        {
            string ret = fileName;
            for (int i = 0; i < FileNameInValidChars.Length; i++)
            {
                ret = ret.Replace(FileNameInValidChars[i].ToString(), tostr);
            }
            return ret;
        }

        public static bool IsValidFileName(string fileName)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(fileName))
                isValid = false;
            else
            {
                for (int i = 0; i < FileNameInValidChars.Length; i++)
                {
                    if (fileName.IndexOf(FileNameInValidChars[i]) != -1)
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// 根据指定内容，创建一个文本文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileContent"></param>
        public static void CreateTextFile(string fileName, string fileContent)
        {
            StreamWriter streamw = File.CreateText(fileName);
            streamw.Write(fileContent);
            streamw.Close();
        }

#if !SILVERLIGHT
        /// <summary>
        /// 文件和文件夹递归复制
        /// </summary>
        /// <param name="sourcePath">原路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="searchPattern">通配符</param>
        /// <param name="isOverWrite">是否覆盖原有文件</param>
        public static void CopyDirectory(string sourcePath, string targetPath, string searchPattern, bool isOverWrite)
        {
            string[] SourceFileList = Directory.GetFiles(sourcePath, searchPattern, SearchOption.AllDirectories);

            foreach (var fileItem in SourceFileList)
            {
                string NewFileName = fileItem.Replace(sourcePath, targetPath);
                if (!Directory.Exists(Path.GetDirectoryName(NewFileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(NewFileName));
                }
                File.Copy(fileItem, NewFileName, true);
            }
        }
#endif

    }
}
