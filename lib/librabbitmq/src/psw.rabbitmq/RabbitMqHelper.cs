using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PSW.RabbitMq
{
    //TODO: Should be in Database
    public struct MessageQueues
    {
        public const string AUTHQueue = "AUTHQueue";
        public const string EDIQueue = "EDIQueue";
        public const string SubscriptionQueue = "SubscriptionQueue";
        public const string UPSQueue = "UPSQueue";
        public const string ANSQueue = "ANSQueue";
    }

    //TODO: Should be in config file
    // public struct ConnectionParams
    // {
    //     public const string HostName = "172.17.0.3";
    //     public const string UserName = "guest";
    //     public const string Password = "guest";
    //     public const int Port = 5672;
    //     public const string VirtualHost = "/";
    // }

    public static class RabbitMqHelper
    {
        public static byte[] ObjectToByteArray(Object obj)
        {
            if(obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object) binForm.Deserialize(memStream);
        
            return obj;
        }
    }
}