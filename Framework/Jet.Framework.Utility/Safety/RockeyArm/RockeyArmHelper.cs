using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Jet.Framework.Utility
{
    /************************************************************************/
    /*                              结构                                    */
    /************************************************************************/
    //RSA公钥格式(兼容1024,2048)
    [StructLayout(LayoutKind.Sequential)]
    public struct RSA_PUBLIC_KEY
    {
        public uint bits;                   // length in bits of modulus        	
        public uint modulus;				  // modulus
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] exponent;       // public exponent
    }
    //RSA私钥格式(兼容1024,2048)
    [StructLayout(LayoutKind.Sequential)]
    public struct RSA_PRIVATE_KEY
    {
        public uint bits;                   // length in bits of modulus        	
        public uint modulus;				  // modulus  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] publicExponent;       // public exponent
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] exponent;       // public exponent
    }
    //外部ECCSM2公钥格式 ECC(支持bits为192或256)和SM2的(bits为固定值0x8100)公钥格式
    [StructLayout(LayoutKind.Sequential)]
    public struct ECCSM2_PUBLIC_KEY
    {
        public uint bits;                   // length in bits of modulus        	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] XCoordinate;       // 曲线上点的X坐标
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] YCoordinate;       // 曲线上点的Y坐标
    }
    //外部ECCSM2私钥格式 ECC(支持bits为192或256)和SM2的(bits为固定值0x8100)私钥格式  
    [StructLayout(LayoutKind.Sequential)]
    public struct ECCSM2_PRIVATE_KEY
    {
        public uint bits;                   // length in bits of modulus        	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] PrivateKey;           // 私钥
    }

    //加密锁信息
    [StructLayout(LayoutKind.Sequential)]
    public struct DONGLE_INFO
    {
        public ushort m_Ver;               //COS版本,比如:0x0201,表示2.01版             	
        public ushort m_Type;              //产品类型: 0xFF表示标准版, 0x00为时钟锁,0x01为带时钟的U盘锁,0x02为标准U盘锁  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] m_BirthDay;       //出厂日期 
        public uint m_Agent;             //代理商编号,比如:默认的0xFFFFFFFF
        public uint m_PID;               //产品ID
        public uint m_UserID;            //用户ID
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] m_HID;            //8字节的硬件ID
        public uint m_IsMother;          //母锁标志: 0x01表示是母锁, 0x00表示不是母锁
        public uint m_DevType;           //设备类型(PROTOCOL_HID或者PROTOCOL_CCID)
    }
    /*************************文件授权结构***********************************/
    //数据文件授权结构
    [StructLayout(LayoutKind.Sequential)]
    public struct DATA_LIC
    {
        public ushort m_Read_Priv;     //读权限: 0为最小匿名权限，1为最小用户权限，2为最小开发商权限            	
        public ushort m_Write_Priv;    //写权限: 0为最小匿名权限，1为最小用户权限，2为最小开发商权限
    }

    //私钥文件授权结构
    [StructLayout(LayoutKind.Sequential)]
    public struct PRIKEY_LIC
    {
        public uint m_Count;        //可调次数: 0xFFFFFFFF表示不限制, 递减到0表示已不可调用
        public byte m_Priv;         //调用权限: 0为最小匿名权限，1为最小用户权限，2为最小开发商权限
        public byte m_IsDecOnRAM;   //是否是在内存中递减: 1为在内存中递减，0为在FLASH中递减
        public byte m_IsReset;      //用户态调用后是否自动回到匿名态: TRUE为调后回到匿名态 (开发商态不受此限制)
        public byte m_Reserve;      //保留,用于4字节对齐
    }

    //对称加密算法(SM4/TDES)密钥文件授权结构
    [StructLayout(LayoutKind.Sequential)]
    public struct KEY_LIC
    {
        public uint m_Priv_Enc;   //加密时的调用权限: 0为最小匿名权限，1为最小用户权限，2为最小开发商权限
    }


    //可执行文件授权结构
    [StructLayout(LayoutKind.Sequential)]
    public struct EXE_LIC
    {
        public ushort m_Priv_Exe;   //运行的权限: 0为最小匿名权限，1为最小用户权限，2为最小开发商权限
    }

    /****************************文件属性结构********************************/
    //数据文件属性数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct DATA_FILE_ATTR
    {
        public uint m_Size;      //数据文件长度，该值最大为4096
        public DATA_LIC m_Lic;       //授权
    }

    //ECCSM2/RSA私钥文件属性数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct PRIKEY_FILE_ATTR
    {
        public ushort m_Type;       //数据类型:ECCSM2私钥 或 RSA私钥
        public ushort m_Size;       //数据长度:RSA该值为1024或2048, ECC该值为192或256, SM2该值为0x8100
        public PRIKEY_LIC m_Lic;        //授权
    }

    //对称加密算法(SM4/TDES)密钥文件属性数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct KEY_FILE_ATTR
    {
        public uint m_Size;       //密钥数据长度=16
        public KEY_LIC m_Lic;        //授权
    }

    //可执行文件属性数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct EXE_FILE_ATTR
    {
        public EXE_LIC m_Lic;        //授权	
        public ushort m_Len;        //文件长度
    }
    /*************************文件列表结构***********************************/
    //获取私钥文件列表时返回的数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct PRIKEY_FILE_LIST
    {
        public ushort m_FILEID;  //文件ID
        public ushort m_Reserve; //保留,用于4字节对齐
        public PRIKEY_FILE_ATTR m_attr;    //文件属性
    }

    //获取SM4及TDES密钥文件列表时返回的数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct KEY_FILE_LIST
    {
        public ushort m_FILEID;  //文件ID
        public ushort m_Reserve; //保留,用于4字节对齐
        public KEY_FILE_ATTR m_attr;    //文件属性
    }

    //获取数据文件列表时返回的数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct DATA_FILE_LIST
    {
        public ushort m_FILEID;  //文件ID
        public ushort m_Reserve; //保留,用于4字节对齐
        public DATA_FILE_ATTR m_attr;    //文件属性
    }

    //获取可执行文件列表时返回的数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct EXE_FILE_LIST
    {
        public ushort m_FILEID;    //文件ID
        public EXE_FILE_ATTR m_attr;
        public ushort m_Reserve;  //保留,用于4字节对齐
    }

    //下载和列可执行文件时填充的数据结构
    [StructLayout(LayoutKind.Sequential)]
    public struct EXE_FILE_INFO
    {
        public ushort m_dwSize;           //可执行文件大小
        public ushort m_wFileID;          //可执行文件ID
        public byte m_Priv;             //调用权限: 0为最小匿名权限，1为最小用户权限，2为最小开发商权限

        public byte[] m_pData;            //可执行文件数据
    }


    //需要发给空锁的初始化数据
    [StructLayout(LayoutKind.Sequential)]
    public struct SON_DATA
    {
        public int m_SeedLen;                 //种子码长度
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string m_SeedForPID;	       //产生产品ID和开发商密码的种子码 (最长250个字节)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
        public string m_UserPIN;         //用户密码(16个字符的0终止字符串)
        public sbyte m_UserTryCount;            //用户密码允许的最大错误重试次数
        public sbyte m_AdminTryCount;           //开发商密码允许的最大错误重试次数
        //RSA_PRIVATE_KEY m_UpdatePriKey;   //远程升级私钥
        public int m_UserID_Start;            //起始用户ID
    }

    //母锁数据
    [StructLayout(LayoutKind.Sequential)]
    public struct MOTHER_DATA
    {
        public SON_DATA m_Son;                  //子锁初始化数据
        public int m_Count;                //可产生子锁初始化数据的次数 (-1表示不限制次数, 递减到0时会受限)
    }

    public class RockeyArmHelper
    {
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_Enum(IntPtr pDongleInfo, out ushort pCount);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_Open(ref uint phDongle, int nIndex);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_Close(uint hDongle);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_VerifyPIN(uint hDongle, uint nFlags, byte[] pPIN, out int pRemainCount);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_CreateFile(uint hDongle, uint nFileType, ushort wFileID, uint pFileAttr);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_WriteFile(uint hDongle, uint nFileType, ushort wFileID, short wOffset, byte[] buffer, int nDataLen);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_ReadFile(uint hDongle, short wFileID, short wOffset, byte[] buffer, int nDataLen);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_ListFile(uint hDongle, uint nFileType, DATA_FILE_LIST[] pFileList, ref int pDataLen);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_DeleteFile(uint hDongle, uint nFileType, short wFileID);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_DownloadExeFile(uint hDongle, EXE_FILE_INFO[] pExeFileInfo, int nCount);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_RunExeFile(uint hDongle, short wFileID, byte[] pInOutData, short wInOutDataLen, ref int nMainRet);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_WriteShareMemory(uint hDongle, byte[] pData, int nDataLen);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_ReadShareMemory(uint hDongle, byte[] pData);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_WriteData(uint hDongle, int nOffset, byte[] pData, int nDataLen);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_ReadData(uint hDongle, int nOffset, byte[] pData, int nDataLen);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_LEDControl(uint hDongle, uint nFlag);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_SwitchProtocol(uint hDongle, uint nFlag);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_GetUTCTime(uint hDongle, ref uint pdwUTCTime);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_SetDeadline(uint hDongle, uint dwTime);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_GenUniqueKey(uint hDongle, int nSeedLen, byte[] pSeed, byte[] pPIDstr, byte[] pAdminPINstr);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_ResetState(uint hDongle);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_ChangePIN(uint hDongle, uint nFlags, byte[] pOldPIN, byte[] pNewPIN, int nTryCount);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_RFS(uint hDongle);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_SetUserID(uint hDongle, uint dwUserID);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_ResetUserPIN(uint hDongle, byte[] pAdminPIN);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_RsaGenPubPriKey(uint hDongle, ushort wPriFileID, ref RSA_PUBLIC_KEY pPubBakup, ref RSA_PRIVATE_KEY pPriBakup);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_RsaPri(uint hDongle, ushort wPriFileID, uint nFlag, byte[] pInData, uint nInDataLen, byte[] pOutData, ref uint pOutDataLen);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_RsaPub(uint hDongle, uint nFlag, ref RSA_PUBLIC_KEY pPubKey, byte[] pInData, uint nInDataLen, byte[] pOutData, ref uint pOutDataLen);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_TDES(uint hDongle, ushort wKeyFileID, uint nFlag, byte[] pInData, byte[] pOutData, uint nDataLen);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_SM4(uint hDongle, ushort wKeyFileID, uint nFlag, byte[] pInData, byte[] pOutData, uint nDataLen);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_DeleteFile(uint hDongle, uint nFileType, ushort wFileID);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_HASH(uint hDongle, uint nFlag, byte[] pInData, uint nDataLen, byte[] pHash);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_LimitSeedCount(uint hDongle, int nCount);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_Seed(uint hDongle, byte[] pSeed, uint nSeedLen, byte[] pOutData);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_EccGenPubPriKey(uint hDongle, ushort wPriFileID, ref ECCSM2_PUBLIC_KEY vPubBakup, ref ECCSM2_PRIVATE_KEY vPriBakup);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_EccSign(uint hDongle, ushort wPriFileID, byte[] pHashData, uint nHashDataLen, byte[] pOutData);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_EccVerify(uint hDongle, ref ECCSM2_PUBLIC_KEY pPubKey, byte[] pHashData, uint nHashDataLen, byte[] pSign);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_SM2GenPubPriKey(uint hDongle, ushort wPriFileID, ref ECCSM2_PUBLIC_KEY pPubBakup, ref ECCSM2_PRIVATE_KEY pPriBakup);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_SM2Sign(uint hDongle, ushort wPriFileID, byte[] pHashData, uint nHashDataLen, byte[] pOutData);
        [DllImport("Dongle_d.dll")]
        static extern uint Dongle_SM2Verify(uint hDongle, ref ECCSM2_PUBLIC_KEY pPubKey, byte[] pHashData, uint nHashDataLen, byte[] pSign);
        [DllImport("kernel32.dll ")]
        static extern bool QueryPerformanceCounter(ref   long lpPerformanceCount);
        [DllImport("kernel32")]
        static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);

        public static string GetErrorInfo(uint error)
        {
            switch (error)
            {
                case DongleDef.DONGLE_SUCCESS: return "操作成功!";
                case DongleDef.DONGLE_NOT_FOUND: return "未找到加密锁!";
                case DongleDef.DONGLE_INVALID_HANDLE: return "无效的句柄!";
                case DongleDef.DONGLE_INVALID_PARAMETER: return "参数错误!";
                case DongleDef.DONGLE_COMM_ERROR: return "通讯错误!";
                case DongleDef.DONGLE_INSUFFICIENT_BUFFER: return "缓冲区空间不足!";
                case DongleDef.DONGLE_NOT_INITIALIZED: return "产品尚未初始化 (即没设置PID)!";
                case DongleDef.DONGLE_ALREADY_INITIALIZED: return "产品已经初始化 (即已设置PID)!";
                case DongleDef.DONGLE_ADMINPIN_NOT_CHECK: return "开发商密码没有验证!";
                case DongleDef.DONGLE_USERPIN_NOT_CHECK: return "用户密码没有验证!";
                case DongleDef.DONGLE_INCORRECT_PIN: return "密码不正确 (后2位指示剩余次数)!";
                case DongleDef.DONGLE_PIN_BLOCKED: return "PIN码已锁死!";
                case DongleDef.DONGLE_ACCESS_DENIED: return "访问被拒绝!";
                case DongleDef.DONGLE_FILE_EXIST: return "文件已存在!";
                case DongleDef.DONGLE_FILE_NOT_FOUND: return "未找到指定的文件!";
                case DongleDef.DONGLE_READ_ERROR: return "读取数据错误!";
                case DongleDef.DONGLE_WRITE_ERROR: return "写入数据错误!";
                case DongleDef.DONGLE_FILE_CREATE_ERROR: return "创建文件或文件夹错误!";
                case DongleDef.DONGLE_FILE_READ_ERROR: return "读取文件错误!";
                case DongleDef.DONGLE_FILE_WRITE_ERROR: return "写入文件错误!";
                case DongleDef.DONGLE_FILE_DEL_ERROR: return "删除文件或文件夹错误!";
                case DongleDef.DONGLE_FAILED: return "操作失败!";
                case DongleDef.DONGLE_CLOCK_EXPIRE: return "加密锁时钟到期!";
                case DongleDef.DONGLE_ERROR_UNKNOWN: return "未知的错误!";

                default:
                    return "未知错误!";
            }
        }

        public static IntPtr EnumRockeyArm(out ushort pCount)
        {
            IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DONGLE_INFO)) * 64);
            uint ret = Dongle_Enum(pt, out pCount);
            if (ret != 0)
                throw new Exception(RockeyArmHelper.GetErrorInfo(ret));

            return pt;
        }

        public static void OpenRockey(ref uint hDongle, int index)
        {
            uint ret = RockeyArmHelper.Dongle_Open(ref hDongle, index);
            if (ret != 0)
                throw new Exception(RockeyArmHelper.GetErrorInfo(ret));
        }

        public static void CloseRockey(uint hDongle)
        {
            uint ret = RockeyArmHelper.Dongle_Close(hDongle);
            if (ret != 0)
                throw new Exception(RockeyArmHelper.GetErrorInfo(ret));
        }

        public static void RFS(uint hDongle)
        {
            uint ret = RockeyArmHelper.Dongle_RFS(hDongle);
            if (ret != 0)
                throw new Exception(RockeyArmHelper.GetErrorInfo(ret));
        }

        public static void VerifyPIN(uint hDongle, byte[] sPin)
        {
            int uiRemainCount = 0;
            uint ret = RockeyArmHelper.Dongle_VerifyPIN(hDongle, DongleDef.FLAG_ADMINPIN, sPin, out uiRemainCount);
            if (ret != 0)
                throw new Exception(RockeyArmHelper.GetErrorInfo(ret));
        }

        public static byte[] ReadData(uint hDongle, int index, int length)
        {
            //写数据区，匿名和用户权限可写前4k(0~4095),开发商有所有8k的写权限
            byte[] datas = new byte[length];//数据区总大小8k
            uint ret = RockeyArmHelper.Dongle_ReadData(hDongle, 4096 + index, datas, length);
            if (ret != 0)
                throw new Exception(RockeyArmHelper.GetErrorInfo(ret));

            return datas;
        }

        public static byte[] ReadData(uint hDongle, int index)
        {
            //写数据区，匿名和用户权限可写前4k(0~4095),开发商有所有8k的写权限
            byte[] dataLengthBytes = new byte[4];//数据区总大小8k
            uint ret1 = RockeyArmHelper.Dongle_ReadData(hDongle, 4096 + index, dataLengthBytes, 4);
            if (ret1 != 0)
                throw new Exception(RockeyArmHelper.GetErrorInfo(ret1));
            int dataLength = BitConverter.ToInt32(dataLengthBytes, 0);

            byte[] datas = new byte[dataLength];//数据区总大小8k
            uint ret2 = RockeyArmHelper.Dongle_ReadData(hDongle, 4100 + index, datas, datas.Length);
            if (ret2 != 0)
                throw new Exception(RockeyArmHelper.GetErrorInfo(ret2));

            return datas;
        }

        public static void WriteData(uint hDongle, byte[] datas)
        {
            if (datas.Length > 4096)
                throw new Exception("写入的内容字节长度不能超过4096!");          

            uint ret = RockeyArmHelper.Dongle_WriteData(hDongle, 4096, datas, datas.Length);
            if (ret != 0)
                throw new Exception(RockeyArmHelper.GetErrorInfo(ret));
        }


        public static void GenUniqueKey(uint hDongle, byte[] seed)
        {
            byte[] cPid = new byte[9];
            byte[] cAdminPin = new byte[17];
            var ret = RockeyArmHelper.Dongle_GenUniqueKey(hDongle, seed.Length, seed, cPid, cAdminPin);

            if (ret != 0)
                throw new Exception(RockeyArmHelper.GetErrorInfo(ret));
        }

        public static string GetHIdStr(DONGLE_INFO pDongleInfo)
        {
            string hIdStr = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                hIdStr += pDongleInfo.m_HID[i].ToString("X");
            }
            return hIdStr;
        }
    }
}
