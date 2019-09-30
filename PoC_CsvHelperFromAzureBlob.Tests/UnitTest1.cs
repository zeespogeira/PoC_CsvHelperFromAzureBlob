using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using NUnit.Framework;
using CsvHelper;
using System.IO;
using System.Linq;
using PoC_ParseMultipleCSVFiles;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var blockBlob = GetCloudBlockBlob();
            // Get the blob file as text
            var contents = await blockBlob.DownloadTextAsync();

            Assert.IsNotNull(contents);
        }

        [Test]
        public async Task DownloadAzureBlobFileToStream_v1()
        {
            var count = 0;

            var blockBlob = GetCloudBlockBlob();

            using(var ms = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(ms);

                ms.Seek(0, SeekOrigin.Begin);
                using(var sr = new StreamReader(ms))
                {
                    var csv = new CsvReader(sr);
                    csv.Configuration.RegisterClassMap(typeof(ServiceCovMap));
                    csv.Configuration.Delimiter = ",";
                    csv.Configuration.HasHeaderRecord = false;
                    var records = csv.GetRecords<ServiceCov>().ToList();
                    count = records.Count;
                }            
            }
            
            Assert.AreEqual(94266, count);
        }

        [Test]
        public async Task DownloadAzureBlobFileToStream_v2()
        {
            var count = 0;

            var blockBlob = GetCloudBlockBlob();

            using(MemoryStream memoryStream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(memoryStream);
                memoryStream.Position = 0;

                using(StreamReader sr = new StreamReader(memoryStream))
                {
                    var csv = new CsvReader(sr);
                    csv.Configuration.RegisterClassMap(typeof(ServiceCovMap));
                    csv.Configuration.Delimiter = ",";
                    csv.Configuration.HasHeaderRecord = false;
                    var records = csv.GetRecords<ServiceCov>().ToList();
                    count = records.Count;
                }
            }

            Assert.AreEqual(94266, count);
        }

        [Test]
        public async Task DownloadAzureBlobFileToStream_v3()
        {
            var count = 0;

            var blockBlob = GetCloudBlockBlob();
            using (var stream = blockBlob.OpenRead())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    //while (!reader.EndOfStream){ }
                    var csv = new CsvReader(sr);
                    csv.Configuration.RegisterClassMap(typeof(ServiceCovMap));
                    csv.Configuration.Delimiter = ",";
                    csv.Configuration.HasHeaderRecord = false;
                    var records = csv.GetRecords<ServiceCov>().ToList();
                    count = records.Count;
                }
            }

            Assert.AreEqual(94266, count);
        }


        private CloudBlockBlob GetCloudBlockBlob()
        {
            var connectionString = $"";
            var containerName = "";
            var fileName = "";
           
 
            // Setup the connection to the storage account
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            
            // Connect to the blob storage
            var serviceClient = storageAccount.CreateCloudBlobClient();
            // Connect to the blob container
            var container = serviceClient.GetContainerReference($"{containerName}");
            // Connect to the blob file
            return container.GetBlockBlobReference($"{fileName}");
        }
    }
}