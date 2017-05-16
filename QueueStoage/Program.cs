using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace QueueStoage
{
    class Program
    {
        static void Main(string[] args)
        {
            //conneting to storage accunt
            CloudStorageAccount csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("samplecodeazure"));

            //creating queue client
            CloudQueueClient clientqueue = csa.CreateCloudQueueClient();

            //creatng queue
            CloudQueue queue = clientqueue.GetQueueReference("myqueue");

            //create queue if not exist
            queue.CreateIfNotExists();

            //insert message into queue
            CloudQueueMessage cqm = new CloudQueueMessage("Hi, There! testing");
            queue.AddMessage(cqm);

            //Peek message
            CloudQueueMessage cqm1 = queue.PeekMessage();
            Console.WriteLine(cqm1.AsString);

            //Update Queue message
            CloudQueueMessage cqm2 = queue.GetMessage();
            cqm2.SetMessageContent("Updated message");
            queue.UpdateMessage(cqm2, TimeSpan.FromSeconds(60.0), MessageUpdateFields.Content | MessageUpdateFields.Visibility);

            //Delete message
           // CloudQueueMessage cqm3 = queue.GetMessage();
           // queue.DeleteMessage(cqm3);

            //take all message from queue
            foreach (CloudQueueMessage msg in queue.GetMessages(20, TimeSpan.FromMinutes(5)))
            {
                Console.WriteLine(msg.AsString);
            }

            //get length
            queue.FetchAttributes();
            int? contentmsg = queue.ApproximateMessageCount;
            Console.WriteLine(contentmsg);

            //Delete queue
            queue.Delete();
        }
    }
}
