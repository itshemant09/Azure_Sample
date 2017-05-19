using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            //creating stoarge acount on azure

            CloudStorageAccount csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            //creating blob container

            CloudBlobClient blobclient = csa.CreateCloudBlobClient();

            //creating blob container

            CloudBlobContainer blobcontainer = blobclient.GetContainerReference("blobcontainer");

            //checking if blob exist or not. if not, then it will create

            blobcontainer.CreateIfNotExists();

            //assigning public access to to blob container

            blobcontainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            //Upload

            //checking the length and uri of all the files store in the blob

            foreach (IListBlobItem item in blobcontainer.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob cbb = (CloudBlockBlob)item;
                    Console.WriteLine("block blob length {0} {1}", cbb.Properties.Length, cbb.Uri);
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob cpb = (CloudPageBlob)item;
                    Console.WriteLine("page blob length {0} {1}", cpb.Properties.Length, cpb.Uri);
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory cbd = (CloudBlobDirectory)item;
                    Console.WriteLine("page blob length  {1}", cbd.Uri);
                }
            }

            //uploading file and creating a block blob

            CloudBlockBlob blockblob = blobcontainer.GetBlockBlobReference("Myblob");
            using (var filestream = System.IO.File.OpenRead(@"C:\AzureSample\demo.txt"))
            {
                blockblob.UploadFromStream(filestream);
            }


            //Download

            CloudBlockBlob blockblob2 = blobcontainer.GetBlockBlobReference("Myblob");
            using (var filestream = System.IO.File.OpenWrite(@"C:\AzureSample\demo.txt"))
            {
                blockblob.DownloadToStream(filestream);
            }

            //create append blob

            CloudAppendBlob cab = blobcontainer.GetAppendBlobReference("Ap_blob");
            cab.CreateOrReplace();


            //delete blob

            blockblob.Delete();


        }
    }
}
