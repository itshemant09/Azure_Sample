using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace TableStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            //Connet to storage account

            CloudStorageAccount csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("samplecodeazure"));

            // creating table storage

            CloudTableClient ctc = csa.CreateCloudTableClient();

            // creating cloud table storage

            CloudTable cloudtbl = ctc.GetTableReference("mytable");

            //check if table is exist or not

            cloudtbl.CreateIfNotExists();

            // creating entity 1

            CustomerEntity cust = new CustomerEntity("Bharadwaj", "Hemanth");
            //cust.Email = "abc@xy.com";
            //cust.PhoneNumber = "+91-8123899837";


            // performing insert into table

            TableOperation tops = TableOperation.Insert(cust);

            // execute operation

            cloudtbl.Execute(tops);

            // creating entity 2

            CustomerEntity cust2 = new CustomerEntity("Bharadwaj", "Nitish");
            cust2.Email = "abc@xy.com";
            //cust2.PhoneNumber = "+91-8123899837";


            // creating entity 3

            CustomerEntity cust3 = new CustomerEntity("Bharadwaj", "Kishor");
            cust3.Email = "abc@xyz.com";
            cust3.PhoneNumber = "+91-8123899837";

            // performing table Batchoperation into table

            TableBatchOperation tbo = new TableBatchOperation();

            //Insert into table 

            tbo.Insert(cust2);
            tbo.Insert(cust3);

            //execte Batchoperation 

            cloudtbl.ExecuteBatch(tbo);

            //Execute table query operation

            TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Bharadwaj"));

            //fetching each customer

            foreach (CustomerEntity ce in cloudtbl.ExecuteQuery(query))
            {
                Console.WriteLine("{0},{1},{2},{3}", ce.PartitionKey, ce.RowKey, ce.Email, ce.PhoneNumber);

            }

            //Execute table query operation

            TableQuery<CustomerEntity> query2 = new TableQuery<CustomerEntity>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Bharadwaj"), TableOperators.And, TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "Hemanth")));

            //fetching each customer

            foreach (CustomerEntity ce1 in cloudtbl.ExecuteQuery(query2))
            {
                Console.WriteLine("{0},{1},{2},{3}", ce1.PartitionKey, ce1.RowKey, ce1.Email, ce1.PhoneNumber);

            }

            //Executing only single entity

            TableOperation tr2 = TableOperation.Retrieve<CustomerEntity>("Bharadwaj", "Kishor");

            //using table result
            TableResult result = cloudtbl.Execute(tr2);

            //checking if exist
            if (result.Result != null)
            {
                Console.WriteLine(((CustomerEntity)result.Result).PhoneNumber);
            }
            else
            {
                Console.WriteLine("This person doesn't have phone number");
            }

            //update entity

            CustomerEntity update = (CustomerEntity)result.Result;

            //cheking and updating

            if (update != null)
            {
                update.PhoneNumber = "+91-7799578900";

                TableOperation update1 = TableOperation.Replace(update);

                cloudtbl.Execute(update1);

                Console.WriteLine("Updated");
            }
            else
            {
                Console.WriteLine("This person doesn't have phone number");
            }

            //Insert or Replace operation
            TableOperation tr8 = TableOperation.Retrieve<CustomerEntity>("Bharadwaj", "Tanish");

            //using table result
            TableResult result8 = cloudtbl.Execute(tr2);
            CustomerEntity InsertorReplace = (CustomerEntity)result8.Result;
            TableOperation tr4 = TableOperation.InsertOrReplace(InsertorReplace);
            cloudtbl.Execute(tr4);

            //Query subset of table (projection)
            TableQuery<DynamicTableEntity> dte = new TableQuery<DynamicTableEntity>().Select(new string[] { "Email" });
            EntityResolver<string> resolver = (pk, rk, ts, props, etag) => props.ContainsKey("Email") ? props["Email"].StringValue : null;
            foreach (string projectedemail in cloudtbl.ExecuteQuery(dte, resolver, null, null))
            {
                Console.WriteLine(projectedemail);
            }

            //delete entity
                        
            //if(InsertorReplace != null)
            //{
            //    TableOperation tr5 = TableOperation.Delete(InsertorReplace);
            //    cloudtbl.Execute(tr5);
            //    Console.WriteLine("Deleted");
            //}
            //else
            //{
            //    Console.WriteLine("Not Exist");
            //}


            //delete table

          //  cloudtbl.DeleteIfExists();

            Console.ReadLine();




        }
    }
}
