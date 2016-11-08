using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace Jet.Framework.Utility
{
    public abstract class HttpUploadHandlerBase : IHttpHandler
    {
        public virtual bool IsReusable
        {
            get 
            { 
                return false; 
            }
        }

        protected string FileName {get; set;}
        protected bool FirstChunk { get; private set; }
        protected bool LastChunk {get;private set;}
        protected long StartByte {get;private set;}
        protected string Parameters {get;private set;}
        protected string TempFilePath {get;private set;}
        protected string TargetFilePath {get;private set;}

        protected HttpContext HttpContext { get; private set; }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                this.HttpContext = context;

                if (context.Request.InputStream.Length == 0)
                    throw new ArgumentException("No file input");

                GetQueryStringParameters();

                TempFilePath = this.GetTempFilePath();
                TargetFilePath = this.GetTargetFilePath();



                this.OnProcessRequest();

                using (FileStream fs = File.Open(TempFilePath, FileMode.Append))
                {
                    SaveFile(context.Request.InputStream, fs);
                }

                if (LastChunk)
                    this.OnFinishedFileUpload();
            }
            catch(Exception ex)
            {
                this.HttpContext.Response.ContentEncoding = Encoding.UTF8;
                this.HttpContext.Response.Write(ex.ToString());
            }
        }

        protected virtual void OnProcessRequest()
        {

        }

        protected virtual string GetTempFilePath()
        {
            throw new NotImplementedException();
        }

        protected virtual string GetTargetFilePath()
        {
            throw new NotImplementedException();
        }

        protected virtual void GetQueryStringParameters()
        {
            string strbase64 = HttpContext.Request.QueryString["file"];
            byte[] datas = Convert.FromBase64String(strbase64);
            this.FileName = Encoding.Unicode.GetString(datas);
            Parameters = HttpContext.Request.QueryString["param"];
            LastChunk = string.IsNullOrEmpty(HttpContext.Request.QueryString["last"]) ? true : bool.Parse(HttpContext.Request.QueryString["last"]);
            FirstChunk = string.IsNullOrEmpty(HttpContext.Request.QueryString["first"]) ? true : bool.Parse(HttpContext.Request.QueryString["first"]);
            StartByte = string.IsNullOrEmpty(HttpContext.Request.QueryString["offset"]) ? 0 : long.Parse(HttpContext.Request.QueryString["offset"]); ;
        }

        private void SaveFile(Stream stream, FileStream fs)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
        }

        protected virtual void OnFinishedFileUpload()
        {
        }
    }
}
