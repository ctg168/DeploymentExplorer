//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Jet.Framework.Utility
//{
//    public class CodyRockey2Safety : CodySafetyBase
//    {
//        private string GetErrorInfo(uint error)
//        {
//            switch (error)
//            {
//                case 0xA0100001:
//                    return "没有找到给定要求的设备(参数错误)";
//                case 0xA0100002:
//                    return "在调用此功能前需要先调用 RY2_Open 打开设备(操作错误)";
//                case 0xA0100003:
//                    return "给出的 UID 错误(参数错误)";
//                case 0xA0100004:
//                    return "读写操作给出的块索引错误(参数错误)";
//                case 0xA0100005:
//                    return "调用 GenUID 功能的时候，给出的 seed 字符串长度超过了 64 个字节(参数错误)";
//                case 0xA0100006:
//                    return "试图改写已经写保护的硬件(操作错误)";
//                case 0xA0100007:
//                    return "打开设备错(Windows 错误)";
//                case 0xA0100008:
//                    return "读记录错(Windows 错误)";
//                case 0xA0100009:
//                    return " 写记录错(Windows 错误)";
//                case 0xA010000A:
//                    return "内部错误(Windows 错误)";
//                case 0xA010000B:
//                    return "内部错误(Windows 错误)";
//                case 0xA010000C:
//                    return "内部错误(Windows 错误)";
//                case 0xA010000D:
//                    return "内部错误(Windows 错误)";
//                case 0xA010000E:
//                    return "内部错误(Windows 错误)";
//                case 0xA010000F:
//                    return "内部错误(Windows 错误)";
//                case 0xA0100010:
//                    return "内部错误(Windows 错误)";
//                case 0xA0100011:
//                    return "内部错误(Windows 错误)";
//                case 0xA0100012:
//                    return "内部错误(Windows 错误)";
//                case 0xA0100013:
//                    return "内部错误";
//                case 0xA0100020:
//                    return "未知的设备(硬件错误)";
//                case 0xA0100021:
//                    return "操作验证错误(硬件错误)";
//                case 0xA010FFFF:
//                    return "未知错误(硬件错误)";
//                default:
//                    return "未知错误";
//            }
//        }

//        /// <summary>
//        /// 获得加密锁内的密钥
//        /// </summary>
//        /// <returns></returns>
//        private string GetRockey(out string error)
//        {
//            int handle = -1;
//            error = string.Empty;
//            try
//            {
//                int result = Rockey2Helper.Find();

//                if (result < 0)
//                {
//                    //error = this.GetErrorInfo((uint)result);
//                    //find()方法返回有问题，所有错误返回暂时都为未找到加密锁
//                    error = "未能找到加密锁设备,请插入加密锁后重试!";
//                    return string.Empty;
//                }
//                else if (result == 0)
//                {
//                    error = "未能找到加密锁设备,请插入加密锁后重试!";
//                    return string.Empty;
//                }
//                else
//                {
//                    uint uid = 0;
//                    uint hid = 0;
//                    handle = Rockey2Helper.Open(Rockey2Helper.AUTO_MODE, uid, ref hid);
//                    if (handle < 0) { throw new Exception(this.GetErrorInfo((uint)handle)); }
//                    StringBuilder builder = new StringBuilder("", 512);
//                    Rockey2Helper.Read(handle, 0, builder);
//                    return builder.ToString();
//                }
//            }
//            finally
//            {
//                if (handle >= 0)
//                {
//                    Rockey2Helper.Close(handle);
//                }
//            }
//        }

//        /// <summary>
//        /// 将Key写入Rockey
//        /// </summary>
//        /// <param name="key"></param>
//        private void WriteRockey(string content, out string error)
//        {
//            int handle = -1;
//            error = string.Empty;
//            try
//            {
//                int result = Rockey2Helper.Find();
//                if (result < 0)
//                {
//                    error = this.GetErrorInfo((uint)result);
//                    return;
//                }
//                else if (result == 0)
//                {
//                    error = "未能找到加密锁设备,请插入加密锁后重试!";
//                    return;
//                }
//                else
//                {
//                    uint uid = 0;
//                    uint hid = 0;
//                    handle = Rockey2Helper.Open(Rockey2Helper.AUTO_MODE, uid, ref hid);
//                    if (handle < 0)
//                    {
//                        error = this.GetErrorInfo((uint)handle);
//                        return;
//                    }
//                    Rockey2Helper.Write(handle, 0, content);
//                }
//            }
//            catch (Exception ee)
//            {
//                throw ee;
//            }
//            finally
//            {
//                if (handle >= 0)
//                {
//                    Rockey2Helper.Close(handle);
//                }
//            }
//        }

//        public override CustomerInfo ReadCustomerInfo(out string error)
//        {
//            error = string.Empty;
//            string rockeyStr = this.GetRockey(out error);
//            if (!string.IsNullOrEmpty(error))
//                return null;
//            var strs = rockeyStr.Split(',');
//            if (strs.Length != 2)
//            {
//                error = string.Format("加密锁内的信息异常!");
//                return null;
//            }

//            CustomerInfo info = new CustomerInfo()
//            {
//                CustomerKey = int.Parse(strs[0]),
//                CustomerName = strs[1]
//            };
//            return info;
//        }

//        public override void WriteCustomerInfo(CustomerInfo info, out string error)
//        {
//            error = string.Empty;
//            string rockeyStr = info.CustomerKey + "," + info.CustomerName;
//            this.WriteRockey(rockeyStr, out error);
//            if (!string.IsNullOrEmpty(error))
//                return;
//        }

//        /// <summary>
//        /// 暂无实现
//        /// </summary>
//        public override CustomerInfo ReadCustomerInfo(int codySystemType, out string error)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
