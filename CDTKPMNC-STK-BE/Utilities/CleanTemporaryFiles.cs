using Quartz;
using System.IO;

namespace CDTKPMNC_STK_BE.Utilities
{
    public class CleanTemporaryFiles : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            string uploadDirectory = Environment.GetEnvironmentVariable("UPLOAD_DIRECTORY")!;
            string tempDirectory = Path.Combine(uploadDirectory, "TempImages");
            if (!Directory.Exists(tempDirectory)) return Task.CompletedTask;
            string[] files = Directory.GetFiles(tempDirectory);
            foreach (string file in files)
            {
                DateTime creationTime = File.GetCreationTime(file);
                if (creationTime.AddHours(1) < DateTime.Now)
                {
                    try {
                        File.Delete(file);
                        Console.WriteLine($"Deleted: {file}");
                    }
                    catch (Exception){}
                }
            }
            return Task.CompletedTask;
        }
    }
}
