using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudDevelopment2020 {
    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            MyLabel.Text = "Hello World from Azure";
        }

		protected void GoButton_Click(object sender, EventArgs e) {
			if (MyFileUpload.HasFile) {
				var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageAccountConnectionString"].ConnectionString);
				var blobClient = storageAccount.CreateCloudBlobClient();
				var containerReference = blobClient.GetContainerReference("test"); // name of your container

				var blobReference = containerReference.GetBlockBlobReference(MyFileUpload.FileName);
				blobReference.UploadFromStream(MyFileUpload.FileContent);
			}
		}

        protected override void OnPreRender(EventArgs e) {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageAccountConnectionString"].ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var containerReference = blobClient.GetContainerReference("test");

            var blobs = containerReference.ListBlobs();
            FilesRepeater.DataSource = blobs;
            FilesRepeater.DataBind();
        }

        protected void FileLink_Command(object sender, CommandEventArgs e) {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageAccountConnectionString"].ConnectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();

            var blobReference = new CloudBlockBlob(new Uri((string)e.CommandArgument), blobClient);
            blobReference.DownloadToStream(Response.OutputStream);
        }
    }
}