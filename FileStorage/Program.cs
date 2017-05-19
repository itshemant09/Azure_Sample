using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;

namespace FileStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            //creating stoarge acount on azure

            CloudStorageAccount csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

        }
    }
}
