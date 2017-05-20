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

            CloudStorageAccount csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("samplecodeazure"));

            //creatig cloud file client
            CloudFileClient fileclient = csa.CreateCloudFileClient();

            //creating FileShare
            CloudFileShare fileshare = fileclient.GetShareReference("logs");

            //Checking if fileshare exist or not
            fileshare.CreateIfNotExists();

            //if exist , performing check on sub directories
            if(fileshare.Exists())
            {

                // checking fileshare size
                Microsoft.WindowsAzure.Storage.File.Protocol.ShareStats stats = fileshare.GetStats();
                Console.WriteLine("{0}", stats.Usage.ToString());

                //incressing file share size by 1 gb
                fileshare.Properties.Quota = 1 + stats.Usage;
                fileshare.SetProperties();

                fileshare.FetchAttributes();
                Console.WriteLine("Updated size {0}", fileshare.Properties.Quota);


                //checking for directory
                CloudFileDirectory dir = fileshare.GetRootDirectoryReference();

                //checking for sub directory
                CloudFileDirectory subdir = dir.GetDirectoryReference("CustomLogs");

                //checking if sub dir exist
                if (subdir.Exists())
                {
                    //creating file
                    CloudFile file = subdir.GetFileReference("Log1.txt");

                    if(file.Exists())
                    {
                        Console.WriteLine(file.DownloadTextAsync().Result);

                        //creating duplicate copy of existing file
                        CloudFile file2 = subdir.GetFileReference("Log1copy.txt");
                        file2.StartCopy(file);
                        Console.WriteLine(file2.DownloadText().ToString());
                    }

                }
            }
            else
            {
                Console.WriteLine("File Share does not exist");
            }

            //Delete File Share
            fileshare.Delete();
            Console.ReadLine();
        }
    }
}
