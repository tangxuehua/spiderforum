using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;

namespace System.Web.Core
{
    public class StaticFilesHttpHandler : IHttpHandler
    {
        private const bool DO_GZIP = true;
        private readonly static TimeSpan CACHE_DURATION = TimeSpan.FromDays(30);

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                HttpRequest request = context.Request;

                // Read setName, contentType and version. All are required. They are
                // used as cache key
                string setName = request["s"] ?? string.Empty;
                string contentType = request["t"] ?? string.Empty;
                string version = request["v"] ?? string.Empty;

                // Decide if browser supports compressed response
                bool isCompressed = DO_GZIP && this.CanGZip(context.Request);

                // Response is written as UTF8 encoding. If you are using languages like
                // Arabic, you should change this to proper encoding 
                //UTF8Encoding encoding = new (false);

                // If the set has already been cached, write the response directly from
                // cache. Otherwise generate the response and cache it
                if (!this.WriteFromCache(context, setName, version, isCompressed, contentType))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        string[] fileNames = null;
                        // Decide regular stream or GZipStream based on whether the response
                        // can be cached or not
                        using (Stream writer = isCompressed ?
                            (Stream)(new GZipStream(memoryStream, CompressionMode.Compress)) :
                            memoryStream)
                        {

                            // Load the files defined in <appSettings> and process each file
                            string setDefinition =
                                System.Configuration.ConfigurationManager.AppSettings[setName] ?? "";
                            fileNames = setDefinition.Split(new char[] { ',' },
                                StringSplitOptions.RemoveEmptyEntries);

                            foreach (string fileName in fileNames)
                            {
                                byte[] fileBytes = this.GetFileBytes(context, fileName.Trim(), Encoding.UTF8);
                                writer.Write(fileBytes, 0, fileBytes.Length);
                            }

                            writer.Close();
                        }

                        // Cache the combined response so that it can be directly written
                        // in subsequent calls 
                        byte[] responseBytes = memoryStream.ToArray();
                        if (fileNames != null && fileNames.Length > 0 && responseBytes != null && responseBytes.Length > 0)
                        {
                            string[] localFileList = GetLocalFiles(context, fileNames);
                            CacheDependency dependency = localFileList != null && localFileList.Length > 0 ? new CacheDependency(localFileList) : null;
                            CacheManager.Insert(GetCacheKey(setName, version, isCompressed), responseBytes, dependency, CACHE_DURATION.TotalSeconds);
                            // Generate the response
                            this.WriteBytes(responseBytes, context, isCompressed, contentType);
                        }
                    }
                }
            }
            catch { }
        }

        private byte[] GetFileBytes(HttpContext context, string virtualPath, Encoding encoding)
        {
            if (virtualPath.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                using (WebClient client = new WebClient())
                {
                    return client.DownloadData(virtualPath);
                }
            }
            else
            {
                string physicalPath = Globals.MapPath(virtualPath);
                byte[] bytes = File.ReadAllBytes(physicalPath);
                // TODO: Convert unicode files to specified encoding. For now, assuming
                // files are either ASCII or UTF8
                return bytes;
            }
        }

        private string[] GetLocalFiles(HttpContext context, string[] virtualPathList)
        {
            List<string> localFileList = new List<string>();
            foreach (string virtualPath in virtualPathList)
            {
                if (!virtualPath.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                {
                    localFileList.Add(Globals.MapPath(virtualPath));
                }
            }
            return localFileList.ToArray();
        }

        private bool WriteFromCache(HttpContext context, string setName, string version, bool isCompressed, string contentType)
        {
            byte[] responseBytes = CacheManager.Get(GetCacheKey(setName, version, isCompressed)) as byte[];

            if (null == responseBytes || 0 == responseBytes.Length) return false;

            this.WriteBytes(responseBytes, context, isCompressed, contentType);
            return true;
        }

        private void WriteBytes(byte[] bytes, HttpContext context, bool isCompressed, string contentType)
        {
            HttpResponse response = context.Response;

            response.AppendHeader("Content-Length", bytes.Length.ToString());
            response.ContentType = contentType;
            if (isCompressed)
                response.AppendHeader("Content-Encoding", "gzip");

            Globals.SetClientCache(context.Response, CACHE_DURATION);

            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.Flush();
        }

        private bool CanGZip(HttpRequest request)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(acceptEncoding) &&
                 (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate")))
                return true;
            return false;
        }

        private string GetCacheKey(string setName, string version, bool isCompressed)
        {
            return "StaticFilesHttpHandler." + setName + "." + version + "." + isCompressed;
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}