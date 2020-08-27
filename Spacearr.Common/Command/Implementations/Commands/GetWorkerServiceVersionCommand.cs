using Newtonsoft.Json;
using Spacearr.Common.Command.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Common.Services.Interfaces;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Spacearr.Common.Command.Implementations.Commands
{
    public class GetWorkerServiceVersionCommand : ICommand
    {
        private const string AppName = "Spacearr.WorkerService.Windows";
        private readonly IFileService _fileService;

        public GetWorkerServiceVersionCommand(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Returns the Worker Service version.
        /// </summary>
        /// <returns>Returns a WorkerServiceVersionModel</returns>
        public async Task<string> Execute()
        {
            var parentDirectory = Directory.GetParent(_fileService.GetUpdateAppStorageFolderPath());
            var currentAppPath = Path.GetFullPath(parentDirectory?.FullName ?? string.Empty);
            var currentApp = Path.Combine(currentAppPath, $"{AppName}.dll");
            var assemblyName = AssemblyName.GetAssemblyName(currentApp);
            var workerServiceVersion = new WorkerServiceVersionModel { Version = assemblyName.Version};

            return await Task.FromResult(JsonConvert.SerializeObject(workerServiceVersion));
        }
    }
}