using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;

namespace FileMonitoringWinService
{
    public partial class FileMonitoringWinService : ServiceBase
    {
        private FileSystemWatcher _watcher;
        private string _sourceFolder;
        private string _destinationFolder;
        private string _logFolder;

        public FileMonitoringWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LoadConfiguration();
            EnsureFoldersExist();
            InitializeWatcher();
            LogInfo("Service started successfully.");
        }

        protected override void OnStop()
        {
            StopWatcher();
            LogInfo("Service stopped.");
        }

        public void StartInConsole()
        {
            OnStart(null);
        }

        public void StopInConsole()
        {
            OnStop();
        }

        // -------------------------------
        // 🔹 Core Functions
        // -------------------------------

        private void LoadConfiguration()
        {
            _sourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
            _destinationFolder = ConfigurationManager.AppSettings["DestinationFolder"];
            _logFolder = ConfigurationManager.AppSettings["LogFolder"];

            if (string.IsNullOrWhiteSpace(_sourceFolder) ||
                string.IsNullOrWhiteSpace(_destinationFolder) ||
                string.IsNullOrWhiteSpace(_logFolder))
            {
                throw new InvalidOperationException("One or more folders are not configured in App.config.");
            }
        }

        private void EnsureFoldersExist()
        {
            if (!Directory.Exists(_sourceFolder))
                Directory.CreateDirectory(_sourceFolder);

            if (!Directory.Exists(_destinationFolder))
                Directory.CreateDirectory(_destinationFolder);

            if (!Directory.Exists(_logFolder))
                Directory.CreateDirectory(_logFolder);
        }

        private void InitializeWatcher()
        {
            _watcher = new FileSystemWatcher
            {
                Path = _sourceFolder,
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.LastWrite,
                Filter = "*.*"
            };

            _watcher.Created += OnNewFileDetected;
            _watcher.Error += OnWatcherError;
            _watcher.EnableRaisingEvents = true;
        }

        private void StopWatcher()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Created -= OnNewFileDetected;
                _watcher.Error -= OnWatcherError;
                _watcher.Dispose();
                _watcher = null;
            }
        }

        private void OnNewFileDetected(object sender, FileSystemEventArgs e)
        {
            try
            {
                LogInfo($"New file detected: {e.FullPath}");

                // Step 1: Rename with GUID
                string renamedFile = RenameFileWithGuid(e.FullPath);

                // Step 2: Move to destination
                MoveFile(renamedFile, _destinationFolder);

                // Step 3: Delete original (if still exists)
                DeleteFile(e.FullPath);
            }
            catch (Exception ex)
            {
                LogError("Error processing file.", ex);
            }
        }

        private string RenameFileWithGuid(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            string newFileName = Guid.NewGuid().ToString() + extension;
            string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName);

            File.Move(filePath, newFilePath);
            LogInfo($"File renamed to: {newFileName}");
            return newFilePath;
        }

        private void MoveFile(string sourcePath, string destinationFolder)
        {
            string fileName = Path.GetFileName(sourcePath);
            string destPath = Path.Combine(destinationFolder, fileName);

            File.Move(sourcePath, destPath);
            LogInfo($"File moved to: {destPath}");
        }

        private void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                LogInfo($"Original file deleted: {filePath}");
            }
        }

        private void OnWatcherError(object sender, ErrorEventArgs e)
        {
            LogError("FileSystemWatcher error.", e.GetException());
        }

        // -------------------------------
        // 🔹 Logging
        // -------------------------------

        private void LogInfo(string message)
        {
            string logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [INFO] {message}";
            Console.WriteLine(logLine); 
            WriteLog("INFO", message);  
        }

        private void LogError(string message, Exception ex)
        {
            string logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [ERROR] {message} | {ex.Message}";
            Console.WriteLine(logLine);
            WriteLog("ERROR", $"{message} | {ex.Message}");
        }

        private void WriteLog(string level, string message)
        {
            string logFile = Path.Combine(_logFolder, $"Log_{DateTime.Now:yyyyMMdd}.txt");
            using (StreamWriter writer = new StreamWriter(logFile, true))
            {
                writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}");
            }
        }




    }
}
