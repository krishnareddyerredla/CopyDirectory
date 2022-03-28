using System;
using System.IO;
using System.Threading.Tasks;

namespace CopyDirectory
{
    public class CopyFunctions : ICopyFunctions
    {
        public async Task<CopyResult> CopyDirectory(
            string source,
            string destination,
            Action<string> progress)
        {
            try
            {
                var sourceDirectory = new DirectoryInfo(source);
                var targetDirectory = new DirectoryInfo(destination);

                if (sourceDirectory.FullName.ToLower() == targetDirectory.FullName.ToLower())
                {
                    return CopyResult.SameDirectory;
                }

                if (Directory.Exists(targetDirectory.FullName) == false)
                {
                    Directory.CreateDirectory(targetDirectory.FullName);
                }

                foreach (FileInfo fi in sourceDirectory.GetFiles())
                {
                    progress($"Copying {targetDirectory.FullName} {fi.Name}");
                    fi.CopyTo(Path.Combine(targetDirectory.ToString(), fi.Name), true);
                    progress($"Copied {targetDirectory.FullName} {fi.Name} successfully.");
                }

                foreach (DirectoryInfo sourceSubDir in sourceDirectory.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                        targetDirectory.CreateSubdirectory(sourceSubDir.Name);
                    await CopyDirectory(sourceSubDir.FullName, nextTargetSubDir.FullName, progress);
                }

                return CopyResult.Success;
            }
            catch (Exception ex)
            {
                progress($"Failed to copy with the exception:{ ex.Message}.");
                return CopyResult.Failed;
            }
        }
    }
}
