using Azure.Identity;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Replace with your connection string thatlooks like mine below  and container name

        string connectionString = "DefaultEndpointsProtocol=https;AccountName=;AccountKey=;EndpointSuffix=core.windows.net";
        /*
        You can get an Azure Blob Storage connection string from the Azure portal by navigating to your storage account, going to the "Access keys" section, and copying the "Connection string" from either the primary or secondary key. Alternatively, you can use the Azure CLI with the az storage account show-connection-string command or Azure PowerShell to retrieve it programmatically 
         */
        string containerName = "yourtextfilescontainername";
        string blobName = "theblobtextfilename.txt";//this will be the name of your file in the storage, container and not the name of your local text file but the content will be the content of the local text file.
        string filePath = "C:\\temp\\yourblocaltextfilename.txt"; // Create a local text file for testing note yourlocale file path

        try
        {
            //1.set your Uri serviceUri = new Uri($"https://{storageAccountName}.blob.core.windows.net"); using your connectionstring from azure

            //2.when creating the blobServiceClient using the below aproach do not worry about new DefaultAzureCredential()
            //BlobServiceClient blobServiceClient = new BlobServiceClient(accountUri, new DefaultAzureCredential());
            /*
             DefaultAzureCredential.
        // This will authenticate automatically using your developer tools locally,
        // or a managed identity when deployed to Azure.
        
            */

            // Create a BlobServiceClient object
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Get a BlobContainerClient object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Create the container if it doesn't exist
            await containerClient.CreateIfNotExistsAsync();

            // Get a BlobClient object
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            Console.WriteLine($"Uploading to Blob storage as blob: {blobClient.Uri}");

            // Upload the local file
            using FileStream uploadFileStream = File.OpenRead(filePath);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();

            Console.WriteLine("Upload complete.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
    }
}