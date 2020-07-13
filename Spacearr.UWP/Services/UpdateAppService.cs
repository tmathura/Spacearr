using ICSharpCode.SharpZipLib.Zip;
using Spacearr.Core.Xamarin.Services.Interfaces;
using Spacearr.UWP.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Xamarin.Forms;

[assembly: Dependency(typeof(UpdateAppService))]
namespace Spacearr.UWP.Services
{
    public class UpdateAppService : IUpdateAppService
    {
        public async Task Update(string versionNumber)
        {
            var localFolderList = await ApplicationData.Current.LocalFolder.GetFoldersAsync();
            foreach (var storageFolder in localFolderList)
            {
                await storageFolder.DeleteAsync();
            }

            var localFolderFileList = await ApplicationData.Current.LocalFolder.GetFilesAsync();
            foreach (var storageFile in localFolderFileList)
            {
                if (storageFile.Name == "Spacearr.Core.Xamarin.SQLite.db3") { } 
                else if (storageFile.Name == $"Spacearr.UWP.v{versionNumber}.zip") { }
                else
                {
                    await storageFile.DeleteAsync();
                }
            }
            var uwpPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, $"Spacearr.UWP.v{versionNumber}.zip");
            var unzipped = await UnzipFileAsync(uwpPath, ApplicationData.Current.LocalFolder.Path);
            if (unzipped)
            {
                var ps1Folder = Path.Combine(ApplicationData.Current.LocalFolder.Path, $"Spacearr.UWP_{versionNumber}_Test");
                var ps1File = Path.Combine(ps1Folder, "Add-AppDevPackage.ps1");

                var storageFile = await StorageFile.GetFileFromPathAsync(ps1File);
                var success = await Launcher.LaunchFileAsync(storageFile);
                if (!success)
                {
                    var storageFolder = await StorageFolder.GetFolderFromPathAsync(ps1Folder);
                    await Launcher.LaunchFolderAsync(storageFolder);
                }
            }
        }

        private static async Task<bool> UnzipFileAsync(string zipFilePath, string unzipFolderPath)
        {
            try
            {
                var entry = new ZipEntry(Path.GetFileNameWithoutExtension(zipFilePath));
                var fileStreamIn = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read);
                var zipInStream = new ZipInputStream(fileStreamIn);
                entry = zipInStream.GetNextEntry();
                while (entry != null && entry.CanDecompress)
                {
                    var outputFile = unzipFolderPath + @"/" + entry.Name;
                    var outputDirectory = Path.GetDirectoryName(outputFile);
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    if (entry.IsFile)
                    {
                        var fileStreamOut = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                        int size;
                        byte[] buffer = new byte[4096];
                        do
                        {
                            size = await zipInStream.ReadAsync(buffer, 0, buffer.Length);
                            await fileStreamOut.WriteAsync(buffer, 0, size);
                        } while (size > 0);
                        fileStreamOut.Close();
                    }

                    entry = zipInStream.GetNextEntry();
                }
                zipInStream.Close();
                fileStreamIn.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}