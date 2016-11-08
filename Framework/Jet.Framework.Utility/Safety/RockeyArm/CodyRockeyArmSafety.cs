using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Jet.Framework.Utility
{
    public class CodyCertRockeyArmSafety : CodyRockeyArmSafetyBase
    {
        public static readonly CodyCertRockeyArmSafety Instance = new CodyCertRockeyArmSafety();
        private CodyCertRockeyArmSafety() { }


        public CertCustomerInfo ReadCustomerInfo(out string error)
        {
            var result = this.ReadCustomerInfo((int)CodySystemTypeEnum.Cert, out error);
            if (!string.IsNullOrEmpty(error)) return null;

            var rockeyArm = SmartSerializeHelper.DeserializeObject<CodyCertRockeyArm>(result.ObjDatas, CodyCertRockeyArm.LoadObj);

            return new CertCustomerInfo()
            {
                CustomerKey = rockeyArm.CustomerKey,
                CustomerName = rockeyArm.CustomerName,
                CodySystemType = (int)CodySystemTypeEnum.Cert,
                EmpowerDate = rockeyArm.EmpowerDate,
                HId = RockeyArmHelper.GetHIdStr(result.PDongleInfo),
                OperatorLimit = rockeyArm.OperatorLimit
            };
        }

        public string WriteCustomerInfo(CertCustomerInfo info, out string error)
        {
            CodyCertRockeyArm rockeyArm = new CodyCertRockeyArm()
            {
                CustomerKey = info.CustomerKey,
                CustomerName = info.CustomerName,
                EmpowerDate = info.EmpowerDate,
                OperatorLimit = info.OperatorLimit
            };
            byte[] bytes = SmartSerializeHelper.SerializeObject(rockeyArm, CodyCertRockeyArm.LoadBytes);
            info.HId = this.WriteCustomerInfo((int)CodySystemTypeEnum.Cert, bytes, out error);
            return info.HId;
        }
    }

    public class CodyPQRockeyArmSafety : CodyRockeyArmSafetyBase
    {
        public static readonly CodyPQRockeyArmSafety Instance = new CodyPQRockeyArmSafety();
        private CodyPQRockeyArmSafety() { }

        public PQCustomerInfo ReadCustomerInfo(out string error)
        {
            var result = this.ReadCustomerInfo((int)CodySystemTypeEnum.PQ, out error);
            CodyPQRockeyArm codyPQRockeyArm = SmartSerializeHelper.DeserializeObject<CodyPQRockeyArm>(result.ObjDatas, CodyPQRockeyArm.LoadObj);
            return new PQCustomerInfo()
            {
                CustomerKey = codyPQRockeyArm.CustomerKey,
                CustomerName = codyPQRockeyArm.CustomerName,
                CodySystemType = (int)CodySystemTypeEnum.PQ,
                EmpowerDate = codyPQRockeyArm.EmpowerDate,
                HId = RockeyArmHelper.GetHIdStr(result.PDongleInfo)
            };
        }

        public string WriteCustomerInfo(PQCustomerInfo info, out string error)
        {
            CodyPQRockeyArm rockeyArm = new CodyPQRockeyArm()
            {
                CustomerKey = info.CustomerKey,
                CustomerName = info.CustomerName,
                EmpowerDate = info.EmpowerDate,
            };
            byte[] bytes = SmartSerializeHelper.SerializeObject(rockeyArm, CodyPQRockeyArm.LoadBytes);
            info.HId = this.WriteCustomerInfo((int)CodySystemTypeEnum.PQ, bytes, out error);
            return info.HId;
        }
    }

    public class CodyPQEXRockeyArmSafety : CodyRockeyArmSafetyBase
    {
        public static readonly CodyPQEXRockeyArmSafety Instance = new CodyPQEXRockeyArmSafety();
        private CodyPQEXRockeyArmSafety() { }

        public PQEXCustomerInfo ReadCustomerInfo(out string error)
        {
            var result = this.ReadCustomerInfo((int)CodySystemTypeEnum.PQEX, out error);
            CodyPQEXRockeyArm codyPQEXRockeyArm = SmartSerializeHelper.DeserializeObject<CodyPQEXRockeyArm>(result.ObjDatas, CodyPQEXRockeyArm.LoadObj);
            return new PQEXCustomerInfo()
            {
                CustomerKey = codyPQEXRockeyArm.CustomerKey,
                CustomerName = codyPQEXRockeyArm.CustomerName,
                CodySystemType = (int)CodySystemTypeEnum.PQEX,
                EmpowerDate = codyPQEXRockeyArm.EmpowerDate,
                HId = RockeyArmHelper.GetHIdStr(result.PDongleInfo)
            };
        }

        public string WriteCustomerInfo(PQEXCustomerInfo info, out string error)
        {
            CodyPQEXRockeyArm rockeyArm = new CodyPQEXRockeyArm()
            {
                CustomerKey = info.CustomerKey,
                CustomerName = info.CustomerName,
                EmpowerDate = info.EmpowerDate,
            };
            byte[] bytes = SmartSerializeHelper.SerializeObject(rockeyArm, CodyPQEXRockeyArm.LoadBytes);
            info.HId = this.WriteCustomerInfo((int)CodySystemTypeEnum.PQEX, bytes, out error);
            return info.HId;
        }
    }

    public class CodyRockeyArmSafetyBase
    {
        private const uint DefaultPid = 4294967295;
        private const uint CodyMasterPid = 4069330466;
        private byte[] DefaultAdminPin = new byte[16] { 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70 }; //默认的开发商密码
        private byte[] CodyMasterPin = new byte[] { 50, 66, 70, 48, 53, 70, 48, 56, 49, 57, 67, 51, 49, 56, 48, 65, 0 };//科迪Pin   

        public class ReadCustomerInfoResult
        {
            public DONGLE_INFO PDongleInfo { get; set; }
            public byte[] ObjDatas { get; set; }
        }

        public static string ReadCustomerInfoStr(out string error)
        {
            try
            {
                ushort pCount = 0;
                var pt = RockeyArmHelper.EnumRockeyArm(out pCount);

                string strInfo = string.Format("共找到{0}个加密锁.", pCount);
                for (int i = 0; i < pCount; i++)
                {
                    DONGLE_INFO pDongleInfo = (DONGLE_INFO)Marshal.PtrToStructure((IntPtr)((UInt32)pt + i * Marshal.SizeOf(typeof(DONGLE_INFO))), typeof(DONGLE_INFO));

                    if (pDongleInfo.m_PID == CodyMasterPid)//只读取CodyMaster初始化过的加密锁
                    {
                        uint hDongle = 0;
                        RockeyArmHelper.OpenRockey(ref hDongle, i);
                        byte[] datas = RockeyArmHelper.ReadData(hDongle, 0, 4);
                        int systemType = BitConverter.ToInt32(datas, 0);

                        byte[] objDatas = RockeyArmHelper.ReadData(hDongle, 4);

                        if (systemType == (int)CodySystemTypeEnum.PQ)
                        {
                            var rockeyArm = SmartSerializeHelper.DeserializeObject<CodyPQRockeyArm>(objDatas, CodyPQRockeyArm.LoadObj);
                            strInfo += string.Format("\r\n----------第{0}个----------\r\n{1}", i + 1, rockeyArm.GetInfo());
                        }
                        else if (systemType == (int)CodySystemTypeEnum.Cert)
                        {
                            var rockeyArm = SmartSerializeHelper.DeserializeObject<CodyCertRockeyArm>(objDatas, CodyCertRockeyArm.LoadObj);
                            strInfo += string.Format("\r\n----------第{0}个----------\r\n{1}", i + 1, rockeyArm.GetInfo());
                        }

                        RockeyArmHelper.CloseRockey(hDongle);
                    }
                }

                error = string.Empty;
                return strInfo;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 读取指定Cody系统类型的加密锁
        /// </summary>
        internal ReadCustomerInfoResult ReadCustomerInfo(int codySystemType, out string error)
        {
            try
            {
                ushort pCount = 0;
                var pt = RockeyArmHelper.EnumRockeyArm(out pCount);

                for (int i = 0; i < pCount; i++)
                {
                    DONGLE_INFO pDongleInfo = (DONGLE_INFO)Marshal.PtrToStructure((IntPtr)((UInt32)pt + i * Marshal.SizeOf(typeof(DONGLE_INFO))), typeof(DONGLE_INFO));

                    if (pDongleInfo.m_PID == CodyMasterPid)//只读取CodyMaster初始化过的加密锁
                    {
                        uint hDongle = 0;
                        RockeyArmHelper.OpenRockey(ref hDongle, i);
                        byte[] datas = RockeyArmHelper.ReadData(hDongle, 0, 4);
                        int systemType = BitConverter.ToInt32(datas, 0);

                        if (systemType == codySystemType)
                        {
                            byte[] objDatas = RockeyArmHelper.ReadData(hDongle, 4);

                            error = string.Empty;
                            RockeyArmHelper.CloseRockey(hDongle);
                            return new ReadCustomerInfoResult()
                            {
                                PDongleInfo = pDongleInfo,
                                ObjDatas = objDatas
                            };
                        }
                        else
                        {
                            RockeyArmHelper.CloseRockey(hDongle);
                        }
                    }
                }

                error = "没有找到针对此系统的加密锁!";
                return null;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        private void Init()
        {
            byte[] codyMasterSeed = Encoding.Unicode.GetBytes("CodyMasterRockeySeed");

            ushort pCount = 0;
            var pt = RockeyArmHelper.EnumRockeyArm(out pCount);

            if (pCount > 1)
            {
                throw new Exception("加密锁不止一个!");
            }
            else if (pCount == 0)
            {
                throw new Exception("没有找到加密锁!");
            }
            else
            {
                DONGLE_INFO pDongleInfo = (DONGLE_INFO)Marshal.PtrToStructure((IntPtr)(UInt32)pt, typeof(DONGLE_INFO));
                if (pDongleInfo.m_PID != DefaultPid)
                    throw new Exception("加密锁已被初始化!");
            }

            uint hDongle = 0;
            RockeyArmHelper.OpenRockey(ref hDongle, 0);
            RockeyArmHelper.VerifyPIN(hDongle, DefaultAdminPin);
            RockeyArmHelper.GenUniqueKey(hDongle, codyMasterSeed);

            RockeyArmHelper.CloseRockey(hDongle);
        }

        private DONGLE_INFO WriteData(byte[] datas)
        {
            DONGLE_INFO pDongleInfo;

            ushort pCount = 0;
            IntPtr pt = RockeyArmHelper.EnumRockeyArm(out pCount);

            if (pCount > 1)
            {
                throw new Exception("加密锁不止一个!");
            }
            else if (pCount == 0)
            {
                throw new Exception("没有找到加密锁!");
            }
            else
            {
                pDongleInfo = (DONGLE_INFO)Marshal.PtrToStructure((IntPtr)(UInt32)pt, typeof(DONGLE_INFO));
                if (pDongleInfo.m_PID != CodyMasterPid)
                    this.Init();
            }

            uint hDongle = 0;
            RockeyArmHelper.OpenRockey(ref hDongle, 0);
            RockeyArmHelper.VerifyPIN(hDongle, CodyMasterPin);
            RockeyArmHelper.WriteData(hDongle, datas);

            RockeyArmHelper.CloseRockey(hDongle);
            return pDongleInfo;
        }

        public string WriteCustomerInfo(int codySystemType, byte[] infoBytes, out string error)
        {
            //byte[] datas = SmartSerializeHelper.SerializeObject(obj, CodyPQRockeyArm.LoadBytes, false);
            List<byte> byteList = new List<byte>();
            byteList.AddRange(BitConverter.GetBytes(codySystemType));
            byteList.AddRange(BitConverter.GetBytes(infoBytes.Length));
            byteList.AddRange(infoBytes);

            try
            {
                DONGLE_INFO pDongleInfo = this.WriteData(byteList.ToArray());
                string hId = RockeyArmHelper.GetHIdStr(pDongleInfo);//回写设备Id

                error = string.Empty;
                return hId;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return string.Empty;
            }
        }
    }

    //public class CodyRockeyArmSafety : CodySafetyBase<PQCustomerInfo>
    //{




    //    public override PQCustomerInfo ReadCustomerInfo(out string error)
    //    {
    //        try
    //        {
    //            ushort pCount = 0;
    //            var pt = RockeyArmHelper.EnumRockeyArm(out pCount);

    //            if (pCount > 1)
    //            {
    //                error = "加密锁不止一个!";
    //                return null; ;
    //            }
    //            else if (pCount == 0)
    //            {
    //                error = "没有找到加密锁!";
    //                return null; ;
    //            }

    //            DONGLE_INFO pDongleInfo = (DONGLE_INFO)Marshal.PtrToStructure((IntPtr)((UInt32)pt), typeof(DONGLE_INFO));

    //            if (pDongleInfo.m_PID == CodyMasterPid)//只读取CodyMaster初始化过的加密锁
    //            {
    //                uint hDongle = 0;
    //                RockeyArmHelper.OpenRockey(ref hDongle, 0);
    //                byte[] datas = RockeyArmHelper.ReadData(hDongle, 0, 4);
    //                int systemType = BitConverter.ToInt32(datas, 0);

    //                byte[] pqDatas = RockeyArmHelper.ReadData(hDongle, 4);
    //                var codyPQRockeyArm = SmartSerializeHelper.DeserializeObject<CodyPQRockeyArm>(pqDatas, CodyPQRockeyArm.LoadObj);

    //                RockeyArmHelper.CloseRockey(hDongle);

    //                error = string.Empty;
    //                return new PQCustomerInfo()
    //                {
    //                    CustomerKey = codyPQRockeyArm.CustomerKey,
    //                    CustomerName = codyPQRockeyArm.CustomerName,
    //                    CodySystemType = systemType,
    //                    EmpowerDate = codyPQRockeyArm.EmpowerDate,
    //                    HId = RockeyArmHelper.GetHIdStr(pDongleInfo)
    //                };
    //            }
    //            else
    //            {
    //                error = "加密锁没有初始化!";
    //                return null;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            error = ex.Message;
    //            return null;
    //        }
    //    }

    //    private void Init()
    //    {
    //        byte[] codyMasterSeed = Encoding.Unicode.GetBytes("CodyMasterRockeySeed");

    //        ushort pCount = 0;
    //        var pt = RockeyArmHelper.EnumRockeyArm(out pCount);

    //        if (pCount > 1)
    //        {
    //            throw new Exception("加密锁不止一个!");
    //        }
    //        else if (pCount == 0)
    //        {
    //            throw new Exception("没有找到加密锁!");
    //        }
    //        else
    //        {
    //            DONGLE_INFO pDongleInfo = (DONGLE_INFO)Marshal.PtrToStructure((IntPtr)(UInt32)pt, typeof(DONGLE_INFO));
    //            if (pDongleInfo.m_PID != DefaultPid)
    //                throw new Exception("加密锁已被初始化!");
    //        }

    //        uint hDongle = 0;
    //        RockeyArmHelper.OpenRockey(ref hDongle, 0);
    //        RockeyArmHelper.VerifyPIN(hDongle, DefaultAdminPin);
    //        RockeyArmHelper.GenUniqueKey(hDongle, codyMasterSeed);

    //        RockeyArmHelper.CloseRockey(hDongle);
    //    }

    //    private DONGLE_INFO WriteData(byte[] datas)
    //    {
    //        DONGLE_INFO pDongleInfo;

    //        ushort pCount = 0;
    //        IntPtr pt = RockeyArmHelper.EnumRockeyArm(out pCount);

    //        if (pCount > 1)
    //        {
    //            throw new Exception("加密锁不止一个!");
    //        }
    //        else if (pCount == 0)
    //        {
    //            throw new Exception("没有找到加密锁!");
    //        }
    //        else
    //        {
    //            pDongleInfo = (DONGLE_INFO)Marshal.PtrToStructure((IntPtr)(UInt32)pt, typeof(DONGLE_INFO));
    //            if (pDongleInfo.m_PID != CodyMasterPid)
    //                this.Init();
    //        }

    //        uint hDongle = 0;
    //        RockeyArmHelper.OpenRockey(ref hDongle, 0);
    //        RockeyArmHelper.VerifyPIN(hDongle, CodyMasterPin);
    //        RockeyArmHelper.WriteData(hDongle, datas);

    //        RockeyArmHelper.CloseRockey(hDongle);
    //        return pDongleInfo;
    //    }

    //    public override void WriteCustomerInfo(PQCustomerInfo info, out string error)
    //    {
    //        CodyPQRockeyArm obj = new CodyPQRockeyArm()
    //        {
    //            CustomerKey = info.CustomerKey,
    //            CustomerName = info.CustomerName,
    //            EmpowerDate = info.EmpowerDate,
    //        };
    //        byte[] datas = SmartSerializeHelper.SerializeObject(obj, CodyPQRockeyArm.LoadBytes, false);
    //        List<byte> byteList = new List<byte>();
    //        byteList.AddRange(BitConverter.GetBytes(info.CodySystemType));
    //        byteList.AddRange(BitConverter.GetBytes(datas.Length));
    //        byteList.AddRange(datas);

    //        try
    //        {
    //            DONGLE_INFO pDongleInfo = this.WriteData(byteList.ToArray());
    //            info.HId = RockeyArmHelper.GetHIdStr(pDongleInfo);//回写设备Id

    //            error = string.Empty;
    //            return;
    //        }
    //        catch (Exception ex)
    //        {
    //            error = ex.Message;
    //            return;
    //        }
    //    }
    //}
}
