using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aliyun.OSS;
using System.Text;
using System.IO;
using System;
using Aliyun.OSS.Common;

public class OSSConnect {
    public static OSSConnect Instance;
    private static string accessKeyId = "LTAI2KOximaB9uJf";
    private static string accessKeySecret = "B4mR66LuJs3kHriM0nmmRG5yr8e3pJ";
    private static string endPoint = "oss-cn-shanghai.aliyuncs.com";
    private static string bucketName = "rehabsys";

    //objectName 在OSS中的名字；filePath 在本地中的文件路径
    public static void UploadFile(string objectName,string filePath)
    {
        try
        {
            OssClient ossClient = new OssClient(endPoint, accessKeyId, accessKeySecret);
            ossClient.PutObject(bucketName, objectName, filePath);
            Debug.Log("Upload succeeded:"+objectName);
        }
        catch (OssException ex)
        {
            Debug.Log("Upload failed:" + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.Log("Upload failed:" + ex.Message);
        }
    }

    public static void DownLoadFile(string objectName,string filePath)
    {
        try
        {
            OssClient ossClient = new OssClient(endPoint, accessKeyId, accessKeySecret);
            OssObject obj = ossClient.GetObject(bucketName, objectName);
            using (var requestStream = obj.Content)
            {
                byte[] buf = new byte[1024];
                var fs = File.Open(filePath, FileMode.OpenOrCreate);
                var len = 0;
                while ((len = requestStream.Read(buf, 0, 1024)) != 0)
                {
                    fs.Write(buf, 0, len);
                }
                fs.Close();
            }
            Debug.Log("Download succeeded:" + objectName);
        }
        catch (OssException ex)
        {
            Debug.Log("Download failed:" + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.Log("Download failed:" + ex.Message);
        }
    }

    public static void DeleteFile(string objectName)
    {
        try
        {
            OssClient ossClient = new OssClient(endPoint, accessKeyId, accessKeySecret);
            ossClient.DeleteObject(bucketName, objectName);
            Debug.Log("Delete succeeded:" + objectName);
        }
        catch (OssException ex)
        {
            Debug.Log("Delete failed:" + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.Log("Delete failed:" + ex.Message);
        }
    }
}
