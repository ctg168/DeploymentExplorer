using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Jet.Framework.Utility
{
    public static class FileIconHelper
    {
        #region DLLIMPORT
        // Retrieves information about an object in the file system,
        // such as a file, a folder, a directory, or a drive root.
        [DllImport("shell32",
          EntryPoint = "SHGetFileInfo",
          ExactSpelling = false,
          CharSet = CharSet.Auto,
          SetLastError = true)]
        private static extern IntPtr SHGetFileInfo(
         string pszPath,      //指定的文件名
         FILE_ATTRIBUTE dwFileAttributes, //文件属性
         ref SHFILEINFO sfi,     //返回获得的文件信息,是一个记录类型
         int cbFileInfo,      //文件的类型名
         SHGFI uFlags);
        #endregion

        #region STRUCTS
        // Contains information about a file object
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;    //文件的图标句柄
            public IntPtr iIcon;    //图标的系统索引号
            public uint dwAttributes;   //文件的属性值
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;  //文件的显示名
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;   //文件的类型名
        };
        #endregion

        #region Enums
        // Flags that specify the file information to retrieve with SHGetFileInfo
        [Flags]
        public enum SHGFI : uint
        {
            ADDOVERLAYS = 0x20,
            ATTR_SPECIFIED = 0x20000,
            ATTRIBUTES = 0x800,   //获得属性
            DISPLAYNAME = 0x200,  //获得显示名
            EXETYPE = 0x2000,
            ICON = 0x100,    //获得图标
            ICONLOCATION = 0x1000,
            LARGEICON = 0,    //获得大图标
            LINKOVERLAY = 0x8000,
            OPENICON = 2,
            OVERLAYINDEX = 0x40,
            PIDL = 8,
            SELECTED = 0x10000,
            SHELLICONSIZE = 4,
            SMALLICON = 1,    //获得小图标
            SYSICONINDEX = 0x4000,
            TYPENAME = 0x400,   //获得类型名
            USEFILEATTRIBUTES = 0x10
        }
        // Flags that specify the file information to retrieve with SHGetFileInfo
        [Flags]
        public enum FILE_ATTRIBUTE
        {
            READONLY = 0x00000001,
            HIDDEN = 0x00000002,
            SYSTEM = 0x00000004,
            DIRECTORY = 0x00000010,
            ARCHIVE = 0x00000020,
            DEVICE = 0x00000040,
            NORMAL = 0x00000080,
            TEMPORARY = 0x00000100,
            SPARSE_FILE = 0x00000200,
            REPARSE_POINT = 0x00000400,
            COMPRESSED = 0x00000800,
            OFFLINE = 0x00001000,
            NOT_CONTENT_INDEXED = 0x00002000,
            ENCRYPTED = 0x00004000
        }
        #endregion

        /// <summary>
        /// 获取系统图标
        /// </summary>
        /// <param name="path">文件名</param>
        /// <param name="dwAttr">文件信息</param>
        /// <param name="dwFlag">获取信息控制字</param>
        /// <returns></returns>
        private static Icon GetIcon(string path, FILE_ATTRIBUTE dwAttr, SHGFI dwFlag)
        {
            SHFILEINFO fi = new SHFILEINFO();
            Icon ic = null;
            int iTotal = (int)SHGetFileInfo(path, dwAttr, ref fi, 0, dwFlag);
            ic = Icon.FromHandle(fi.hIcon);
            return ic;
        }
        /// <summary>
        /// 获得文件的图标        
        /// </summary>        
        public static Icon GetFileIcon(string fileName)
        {
            return GetIcon(fileName, FILE_ATTRIBUTE.NORMAL, SHGFI.USEFILEATTRIBUTES | SHGFI.ICON);
        }

        public static byte[] GetFileIconBytes(string fileName)
        {
            Icon icon = GetFileIcon(fileName);
            Bitmap bmp = icon.ToBitmap();
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
