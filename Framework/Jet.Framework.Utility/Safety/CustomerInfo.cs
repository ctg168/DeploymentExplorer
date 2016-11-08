using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public enum CodySystemTypeEnum
    {
        [AttachData("科迪题卷单机版")]
        PQ = 1,

        [AttachData("科迪持证系统单机版")]
        Cert = 2,

        [AttachData("科迪题卷高级单机版")]
        PQEX = 3,
    }

    public abstract class CustomerInfoBase
    {
        public CustomerInfoBase()
        {

        }

        public int CustomerKey { get; set; }
        public string CustomerName { get; set; }
        public int CodySystemType { get; set; }
        public string HId { get; set; }
        public DateTime? EmpowerDate { get; set; }

        public virtual string GetCustomerInfoStr()
        {
            return string.Format("系统：{0}\r\n设备Id：{1}\r\n客户编号：{2}\r\n客户名称：{3}\r\n过期时间：{4}",
                                ((CodySystemTypeEnum)this.CodySystemType).GetText(),
                                this.HId,
                                this.CustomerKey,
                                this.CustomerName,
                                this.EmpowerDate.HasValue ? this.EmpowerDate.Value.ToString("yyyy-MM-dd") : "永久");
        }

        public override string ToString()
        {
            return string.Format("系统：{0}，设备Id：{1}，客户编号：{2}，客户名称：{3}，过期时间：{4}",
                               this.CodySystemType,
                               this.HId,
                               this.CustomerKey,
                               this.CustomerName,
                               this.EmpowerDate.HasValue ? this.EmpowerDate.Value.ToString("yyyy-MM-dd") : "永久");
        }
    }

    public class PQCustomerInfo : CustomerInfoBase
    {

    }

    public class PQEXCustomerInfo : CustomerInfoBase
    {

    }

    public class CertCustomerInfo : CustomerInfoBase
    {
        public int OperatorLimit { get; set; }

        public override string GetCustomerInfoStr()
        {
            return string.Format("{0}\r\n最大培训员数量：{1}", base.GetCustomerInfoStr(), this.OperatorLimit);
        }
    }
}
