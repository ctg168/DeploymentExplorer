//using System.Windows;
//using System.ServiceModel.Channels;
//using System.ServiceModel.Dispatcher;
//using System.ServiceModel;
//using System.Threading;
//using Jet.AppShell.Base;

//namespace Jet.AppShell.Base
//{
//    public class ClientMessageInspector : IClientMessageInspector
//    {
//        public ClientMessageInspector()
//        {

//        }

//        public bool ShowBusyUI { get; set; }

//        public void AfterReceiveReply(ref Message reply, object correlationState)
//        {
//            var HeaderIndex = reply.Headers.FindHeader("ClientId", "");
//            if (HeaderIndex == -1) return;
//            var ClientId = reply.Headers.GetHeader<string>(HeaderIndex);

//            HeaderIndex = reply.Headers.FindHeader("LocalClientId", "");
//            if (HeaderIndex == -1) return;
//            var LocalClientId = reply.Headers.GetHeader<string>(HeaderIndex);

//            HeaderIndex = reply.Headers.FindHeader("IpAddress", "");
//            if (HeaderIndex == -1) return;
//            var IpAddress = reply.Headers.GetHeader<string>(HeaderIndex);

//            //检查多账号登录问题------------------------------------------------------------------------------------------
//            if (!AppShellService.Instance.AllowSingleAccountMultiLogin)
//            {
//                if (ClientId != AppShellService.Instance.ClientId)
//                {
//                    Deployment.Current.Dispatcher.BeginInvoke(() => AppShellService.Instance.SingleAccountMultiLoginError(IpAddress));
//                    return;
//                }
//            }

//            if(!AppShellService.Instance.AllowSingleClientMultiLogin)
//            {
//                if(LocalClientId != AppShellService.Instance.ClientId)
//                {
//                    Deployment.Current.Dispatcher.BeginInvoke(() => AppShellService.Instance.SingleClientMultiLoginError());
//                    return;
//                }
//            }
//        }

//        public object BeforeSendRequest(ref Message request, IClientChannel channel)
//        {

//            if (ShowBusyUI)
//            {
//                Deployment.Current.Dispatcher.BeginInvoke(() =>
//                {
//                    BusyIndicatorService.Instance.IsBusy = true;
//                });
//            }

//            var emp = AppShellService.Instance.CurrentEmployee;
//            if (emp != null)
//            {
//                request.Headers.Add(MessageHeader.CreateHeader("UserName", "", emp.ObjectName, false, ""));
//                request.Headers.Add(MessageHeader.CreateHeader("DepartmentName", "", emp.DeptFullPath, false, ""));
//                request.Headers.Add(MessageHeader.CreateHeader("MacAddress", "", ActivexService.Instance.GetMacAddress(), false, ""));
//                request.Headers.Add(MessageHeader.CreateHeader("UserId", "", emp.Id.ToString(), false, ""));
//                request.Headers.Add(MessageHeader.CreateHeader("ClientId", "", AppShellService.Instance.ClientId, false, ""));
//            }
//            return null;
//        }
//    }
//}
