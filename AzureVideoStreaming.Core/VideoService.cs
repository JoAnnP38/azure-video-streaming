using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MediaServices.Client;

namespace AzureVideoStreaming.Core
{
    public class VideoService
    {
        private static readonly string _accountKey = ConfigurationManager.AppSettings["accountKey"];
        private static readonly string _accountName = ConfigurationManager.AppSettings["accountName"];

        // Field for service context.
        private static CloudMediaContext _context = null;

        public VideoService()
        {
            _context = new CloudMediaContext(_accountName, _accountKey);
        }

        public IAsset CreateAssetAndUploadSingleFile(string singleFilePath)
        {
            var assetName = "UploadSingleFile_" + DateTime.UtcNow.ToString();
            var asset = _context.Assets.Create(assetName, AssetCreationOptions.None);

            var fileName = Path.GetFileName(singleFilePath);

            var assetFile = asset.AssetFiles.Create(fileName);

            Trace.WriteLine("Created assetFile {0}", assetFile.Name);

            var accessPolicy = _context.AccessPolicies.Create(assetName, TimeSpan.FromDays(30),
                                                                AccessPermissions.Write | AccessPermissions.List);

            var locator = _context.Locators.CreateLocator(LocatorType.Sas, asset, accessPolicy);

            Trace.WriteLine("Upload {0}", assetFile.Name);

            assetFile.Upload(singleFilePath);
            Trace.WriteLine("Done uploading {0}", assetFile.Name);

            locator.Delete();
            accessPolicy.Delete();

            return asset;
        }

        public IJob CreateEncodingJob(IAsset asset, string inputMediaFilePath)
        {
            IJob job = _context.Jobs.Create("Job-" + Guid.NewGuid());
            // Get a media processor reference, and pass to it the name of the 
            // processor to use for the specific task.
            IMediaProcessor processor = GetLatestMediaProcessorByName("Windows Azure Media Encoder");

            // Create a task with the encoding details, using a string preset.
            ITask task = job.Tasks.AddNew("H264-" + Guid.NewGuid(),
                processor,
                "H264 Broadband 720p",
                Microsoft.WindowsAzure.MediaServices.Client.TaskOptions.ProtectedConfiguration);

            // Specify the input asset to be encoded.
            task.InputAssets.Add(asset);
            // Add an output asset to contain the results of the job. 
            // This output is specified as AssetCreationOptions.None, which 
            // means the output asset is not encrypted. 
            task.OutputAssets.AddNew("Output asset",
                AssetCreationOptions.None);

            const string configuration = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Thumbnail Size=""120,*"" Type=""Jpeg"" Filename=""{OriginalFilename}_{ThumbnailTime}.{DefaultExtension}"">
  <Time Value=""0:0:0""/>
  <Time Value=""0:0:3"" Step=""0:0:0.25"" Stop=""0:0:10""/>
</Thumbnail>";

            var thumbnailTask = job.Tasks.AddNew("Thumbnail",
                processor,
                configuration,
                TaskOptions.ProtectedConfiguration);

            // Specify the input asset to be encoded.
            thumbnailTask.InputAssets.Add(asset);
            // Add an output asset to contain the results of the job. 
            // This output is specified as AssetCreationOptions.None, which 
            // means the output asset is not encrypted. 
            thumbnailTask.OutputAssets.AddNew("Thumbnail output asset",
                AssetCreationOptions.None);

            // Launch the job.
            job.Submit();

            return job;
        }

        public JobState GetJobOutput(string jobId, out string mp4Url, out string vc1Url)
        {
            var job = GetJob(jobId);

            mp4Url = vc1Url = null;

            if (job.State == JobState.Finished)
            {
                // Get a reference to the output asset from the job.
                IAsset outputAsset = job.OutputMediaAssets[0];

                var streamingAssetId = outputAsset.Id;
                const int daysForWhichStreamingUrlIsActive = 365;
                var streamingAsset = _context.Assets.Where(a => a.Id == streamingAssetId).FirstOrDefault();
                var accessPolicy = _context.AccessPolicies.Create(streamingAsset.Name, TimeSpan.FromDays(daysForWhichStreamingUrlIsActive),
                                                         AccessPermissions.Read | AccessPermissions.List);

                var assetFiles = streamingAsset.AssetFiles.ToList();

                var streamingAssetFile = assetFiles.Where(f => f.Name.ToLower().EndsWith(".mp4")).FirstOrDefault();
                if (streamingAssetFile != null)
                {
                    var locator = _context.Locators.CreateLocator(LocatorType.Sas, streamingAsset, accessPolicy);
                    var uri = new UriBuilder(locator.Path);
                    uri.Path += "/" + streamingAssetFile.Name;
                    mp4Url = uri.ToString();
                }

                streamingAssetFile = assetFiles.Where(f => f.Name.ToLower().EndsWith(".wmv")).FirstOrDefault();
                if (streamingAssetFile != null)
                {
                    var locator = _context.Locators.CreateLocator(LocatorType.Sas, streamingAsset, accessPolicy);
                    var uri = new UriBuilder(locator.Path);
                    uri.Path += "/" + streamingAssetFile.Name;
                    vc1Url = uri.ToString();
                }


            }

            return job.State;

        }

        private static IMediaProcessor GetLatestMediaProcessorByName(string mediaProcessorName)
        {
            // The possible strings that can be passed into the 
            // method for the mediaProcessor parameter:
            //   Windows Azure Media Encoder
            //   Windows Azure Media Packager
            //   Windows Azure Media Encryptor
            //   Storage Decryption

            var processor = _context.MediaProcessors.Where(p => p.Name == mediaProcessorName).
                ToList().OrderBy(p => new Version(p.Version)).LastOrDefault();

            if (processor == null)
                throw new ArgumentException(string.Format("Unknown media processor", mediaProcessorName));

            return processor;
        }

        private IJob GetJob(string jobId)
        {
            // Use a Linq select query to get an updated 
            // reference by Id. 
            var jobInstance =
                from j in _context.Jobs
                where j.Id == jobId
                select j;
            // Return the job reference as an Ijob. 
            IJob job = jobInstance.FirstOrDefault();

            return job;
        }
    }
}
