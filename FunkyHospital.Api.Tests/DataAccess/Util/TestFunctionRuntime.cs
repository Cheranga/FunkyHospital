using System;
using System.Diagnostics;
using System.IO;

namespace FunkyHospital.Api.Tests.DataAccess.Util
{
    public class TestFunctionRuntime : IDisposable
    {
        private readonly Process _funcHostProcess;

        public int Port { get; } = 7071;

        public TestFunctionRuntime()
        {
            var dotnetExePath = Environment.ExpandEnvironmentVariables(ConfigurationHelper.Settings.DotNetExecutablePath);
            var functionHostPath = Environment.ExpandEnvironmentVariables(ConfigurationHelper.Settings.FunctionHostPath);
            var functionAppFolder = Path.GetRelativePath(Directory.GetCurrentDirectory(), ConfigurationHelper.Settings.FunctionApplicationPath);

            _funcHostProcess = new Process
            {
                StartInfo =
                {
                    FileName = dotnetExePath,
                    Arguments = $"\"{functionHostPath}\" start -p {Port}",
                    WorkingDirectory = functionAppFolder
                }
            };
            var success = _funcHostProcess.Start();
            if (!success)
            {
                throw new InvalidOperationException("Could not start Azure Functions host.");
            }

            //
            // Starting the storage explorer if it hasn't started.
            //
            //var storageExplorerProcess = new Process
            //{
            //    StartInfo =
            //    {
            //        FileName = "cmd.exe",
            //        Arguments = @"cd C:\\Program Files (x86)\\Microsoft SDKs\\Azure\\Storage Emulator \n cd c:\n AzureStorageEmulator.exe start",
            //        WorkingDirectory = functionAppFolder
            //    }
            //};

            //var command = "StorageEmulator.bat";
            //var processInfo = new ProcessStartInfo("cmd.exe", command);
            //var a = Process.Start(processInfo);

            //var testProcess = Process.Start( "StorageEmulator.bat");


            var startInfo = new ProcessStartInfo
            {
                Arguments = "start",
                FileName = @"C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe"
            };
            using (var proc = new Process { StartInfo = startInfo })
            {
                proc.Start();
                proc.WaitForExit();
                var exitCode = proc.ExitCode;
            }
        }

        public void Dispose()
        {
            if (!_funcHostProcess.HasExited)
            {
                _funcHostProcess.Kill();
            }

            _funcHostProcess.Dispose();
        }
    }
}
