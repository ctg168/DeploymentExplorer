using System;
using System.Net;
using System.Windows;
using System.ComponentModel;
using System.Windows.Forms;

namespace Jet.Framework.Utility
{
    public static class WcfHelper
    {
        public static string GetUserMessage(this Exception ex)
        {
            if (ex.IsCommunicationException())
                return "网络传输故障，请联系系统管理员!";
            else
                return ex.Message;
        }

        private static void ShowError(AsyncCompletedEventArgs e, Action errorMethod, bool showErrorMsg)
        {
            if (showErrorMsg)            
                MessageBox.Show(e.Error.Message);           
            
            if (errorMethod != null)
                errorMethod();
        }

        public static void AsyncComplete(this AsyncCompletedEventArgs e, Action resultMethod = null, Action errorMethod = null, bool showErrorMsg = true)
        {
            if (e.Error == null)
            {
                //Deployment.Current.Dispatcher.BeginInvoke(() =>
                //{
                //    Deployment.Current.Dispatcher.BeginInvoke(() =>
                //    {
                //        BusyIndicatorService.Instance.IsBusy = false;
                //    });
                //});

                if (resultMethod != null)
                    resultMethod();
            }
            else
            {
                //BusyIndicatorService.Instance.IsBusy = false;
                WcfHelper.ShowError(e, errorMethod, showErrorMsg);
            }
        }

        public static void AsyncCompleteNoBusy(this AsyncCompletedEventArgs e, Action resultMethod = null, Action errorMethod = null, bool showErrorMsg = true)
        {
            if (e.Error == null)
            {
                if (resultMethod != null)
                    resultMethod();
            }
            else
            {
                WcfHelper.ShowError(e, errorMethod, showErrorMsg);
            }
        }
    }
}
