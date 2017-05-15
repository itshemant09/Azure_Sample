using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Azure_Learning_sample_code
{
    class Program
    {
        static void Main(string[] args)
        {
            CloudStorageAccount csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobclient = csa.CreateCloudBlobClient();
            CloudBlobContainer blobcontainer = blobclient.GetContainerReference("blobcontainer");
            blobcontainer.CreateIfNotExists();
            blobcontainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            //Download
            CloudBlockBlob blockblob = blobcontainer.GetBlockBlobReference("DCIPL_Tech_SI_Hemant_Bharadwaj.docx");
            
            CloudAppendBlob cab = blobcontainer.GetAppendBlobReference("Ap_blob");
            cab.CreateOrReplace();

            //using (var filestream = System.IO.File.OpenWrite(@"C:\AzureSample\demo.txt"))
            //{
            //    blockblob.DownloadToStream(filestream);
            //}
            //delete blob
         //   blockblob.Delete();

            //Upload


            //foreach(IListBlobItem item in blobcontainer.ListBlobs(null,false))
            //{
            //    if(item.GetType()==typeof(CloudBlockBlob))
            //    {
            //        CloudBlockBlob cbb = (CloudBlockBlob)item;
            //        Console.WriteLine("block blob length {0} {1}", cbb.Properties.Length, cbb.Uri);
            //    }
            //    else if(item.GetType()==typeof(CloudPageBlob))
            //    {
            //        CloudPageBlob cpb = (CloudPageBlob)item;
            //        Console.WriteLine("page blob length {0} {1}", cpb.Properties.Length, cpb.Uri);
            //    }
            //    else if (item.GetType() == typeof(CloudBlobDirectory))
            //    {
            //        CloudBlobDirectory cbd = (CloudBlobDirectory)item;
            //        Console.WriteLine("page blob length  {1}", cbd.Uri);
            //    }
            //}
            //CloudBlockBlob blockblob = blobcontainer.GetBlockBlobReference("Myblob");
            // using (var filestream = System.IO.File.OpenRead(@"C:\AzureSample\demo.txt"))
            //{
            //    blockblob.UploadFromStream(filestream);
            //}
        }
    }
}
