<%@ WebHandler Language="C#" Class="HttpUploadHandler" %>

using System;
using System.Web;
using System.IO;
using System.Web.Hosting;
using System.Diagnostics;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

public class HttpUploadHandler : IHttpHandler {

    private HttpContext _httpContext;
    private string _tempExtension = "_temp";
    private string _fileName;
    private string _parameters;
    private bool _lastChunk;
    private bool _firstChunk;
    private long _startByte;

    StreamWriter _debugFileStreamWriter;
    TextWriterTraceListener _debugListener;
       
    /// <summary>
    /// Start method
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        _httpContext = context;

        if (context.Request.InputStream.Length == 0)
        {
            //Use ?test=test for Write/Delete test!
            if (_httpContext.Request.QueryString["test"] == "test")
                RunWriteDeleteTest();
            else
                throw new ArgumentException("No file input");
        }
        else
        {
            try
            {
                GetQueryStringParameters();

                string uploadFolder = GetUploadFolder();
                string tempFileName = _fileName + _tempExtension;

                string tempPath = GetTempFilePath(tempFileName);
                string targetPath = GetTargetFilePath(_fileName);

                //Is it the first chunk? Prepare by deleting any existing files with the same name
                if (_firstChunk)
                {
                    Debug.WriteLine("First chunk arrived at webservice");

                    //Delete temp file               
                    if (File.Exists(tempPath))
                        File.Delete(tempPath);

                    //Delete target file                
                    if (File.Exists(targetPath))
                        File.Delete(targetPath);

                }

                //Write the file
                Debug.WriteLine(string.Format("Write data to disk FOLDER: {0}", uploadFolder));

                using (FileStream fs = File.Open(tempPath, FileMode.Append))
                {
                    SaveFile(context.Request.InputStream, fs);
                    fs.Close();
                }

                Debug.WriteLine("Write data to disk SUCCESS");

                //Is it the last chunk? Then finish up...
                if (_lastChunk)
                {
                    Debug.WriteLine("Last chunk arrived");

                    //Rename file to original file
                    File.Move(tempPath, targetPath);

                    //Finish stuff....
                    FinishedFileUpload(_fileName, _parameters);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());

                throw;
            }
            finally
            {
            }
        }

    }

    /// <summary>
    /// Get the querystring parameters
    /// </summary>
    private void GetQueryStringParameters()
    {
        _fileName = _httpContext.Request.QueryString["file"];
        _parameters = _httpContext.Request.QueryString["param"];
        _lastChunk = string.IsNullOrEmpty(_httpContext.Request.QueryString["last"]) ? true : bool.Parse(_httpContext.Request.QueryString["last"]);
        _firstChunk = string.IsNullOrEmpty(_httpContext.Request.QueryString["first"]) ? true : bool.Parse(_httpContext.Request.QueryString["first"]);
        _startByte = string.IsNullOrEmpty(_httpContext.Request.QueryString["offset"]) ? 0 : long.Parse(_httpContext.Request.QueryString["offset"]); ;
    }

    /// <summary>
    /// Save the contents of the Stream to a file
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="fs"></param>
    private void SaveFile(Stream stream, FileStream fs)
    {
        byte[] buffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            fs.Write(buffer, 0, bytesRead);
        }
    }

    /// <summary>
    /// Do your own stuff here when the file is finished uploading
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="parameters"></param>
    protected virtual void FinishedFileUpload(string fileName, string parameters)
    {
    }

    
    protected virtual string GetUploadFolder()
    {
        string folder = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"];
        if (string.IsNullOrEmpty(folder))
            folder = "Upload";

        return folder;
    }

    protected string GetTempFilePath(string fileName)
    {
        return Path.Combine(@HostingEnvironment.ApplicationPhysicalPath, Path.Combine(GetUploadFolder(), fileName));
    }

    protected string GetTargetFilePath(string fileName)
    {
        return Path.Combine(@HostingEnvironment.ApplicationPhysicalPath, Path.Combine(GetUploadFolder(), fileName));
    }


   
     /// <summary>
    /// Test method to test writing and deleting of files to temp and target directory
    /// </summary>
    private void RunWriteDeleteTest()
    {
      string tempPath = GetTempFilePath("test.test");
      string targetPath = GetTargetFilePath("test.test");

      //Cleanup
      TestDelete(tempPath);
      TestDelete(targetPath);

      //Write file to TempFilePath
      TestWrite(tempPath);

      //Delete file from TempFilePath
      TestDelete(tempPath);

      //Write file to TargetFilePath
      TestWrite(targetPath);

      //Delete file from TargetFilePath
      TestDelete(targetPath);

      _httpContext.Response.Write("Test success");
    }

    /// <summary>
    /// Test writing to a file
    /// </summary>
    /// <param name="tempPath"></param>
    private static void TestWrite(string path)
    {
      try
      {
        using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
        {
          string line1 = "test file";
          sw.WriteLine(line1);
          sw.Close();
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Test write failed: {0}", path), ex);
      }
    }

    /// <summary>
    /// Test deleting of a file
    /// </summary>
    /// <param name="tempPath"></param>
    private static void TestDelete(string path)
    {
      try
      {
        if (File.Exists(path))
          File.Delete(path);
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Test delete failed: {0}", path), ex);
      }
    }

    
    public bool IsReusable {
        get {
            return false;
        }
    }

}