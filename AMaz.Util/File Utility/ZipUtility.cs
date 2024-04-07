using System.IO.Compression;

namespace AMaz.Util
{ 
    public static class ZipUtility
    {
        // Create a zip file with multiple folders
        // The key is the directory path and the value is the path of the file
        public static Byte[] CreateZipFileWithMultipleFolders(Dictionary<string, List<string>> map)
        {
            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {
                    foreach (var contribution in map)
                    {
                        var folderName = DateTime.Now.ToString("yyyy-MM-dd") + "_" + contribution.Key;
                        foreach (var filePath in contribution.Value)
                        {
                            var fileInArchive = archive.CreateEntry($"{folderName}/{Path.GetFileName(filePath)}"); //Create a folder in the archive

                            using (var entryStream = fileInArchive.Open())
                            using (var fileStream = File.OpenRead(filePath)) //Open the file to be compressed
                            {
                                fileStream.CopyTo(entryStream); //Copy the file to the archive
                            }
                        }
                    }
                }

                return outStream.ToArray();
            }
        }

        public static void ExtractZipFile(string sourceDirectory, string destinationDirectory)
        {
            // Extract a zip file
        }
    }
}
