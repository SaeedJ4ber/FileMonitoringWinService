# 📁 FileMonitoringService – README

---

## 📝 Overview
FileMonitoringService is a Windows Service that monitors a specified folder for new files.  
When a file is detected, it is renamed using a unique GUID, moved to a destination folder, and logged for tracking.  
The service supports both **Windows Service mode** and **Console mode** for debugging.

---

## ⚙️ Configuration (App.config)

<appSettings>
  <add key="SourceFolder" value="E:\FileMonitoring\Source" />
  <add key="DestinationFolder" value="E:\FileMonitoring\Destination" />
  <add key="LogFolder" value="E:\FileMonitoring\Logs" />
</appSettings>

SourceFolder: Folder to monitor for new files.  
DestinationFolder: Folder where renamed files are moved.  
LogFolder: Folder where log files are stored.

---

## 🚀 How to Run

### 🧩 Console Mode (Debugging)
1. Open the project in **Visual Studio**.  
2. Set **Output type** to *Console Application* in project properties.  
3. Press **F5** to run in Console mode.  
4. Add a file to the **SourceFolder** and observe logs in the console.  
5. Press **Enter** to stop the service.

---

### 🖥️ Windows Service Mode
1. Build the project to generate the `.exe` file.  
2. Open **Command Prompt** as Administrator.  
3. Install the service:
InstallUtil FileMonitoringService.exe

4. Start the service from **Services.msc**.  

5. (Optional) Uninstall the service:
InstallUtil /u FileMonitoringService.exe

---

## 🧪 Testing Scenarios
- Drop a file into the **SourceFolder** → it should be renamed and moved.  
- Check the **DestinationFolder** for the renamed file.  
- Verify **log entries** in the **LogFolder**.  
- Delete folders to test **auto-creation**.

---

## 📄 Sample Log Output

[2025-10-26 06:45:00] Service Started.  
[2025-10-26 06:46:12] New file detected: E:\FileMonitoring\Source\report.pdf  
[2025-10-26 06:46:13] File renamed to: 3f9c2a1e-7d2f-4c9e-bf3a-9e2d1f1c2b3a.pdf  
[2025-10-26 06:46:14] File moved to: E:\FileMonitoring\Destination\3f9c2a1e-7d2f-4c9e-bf3a-9e2d1f1c2b3a.pdf  
[2025-10-26 06:50:00] Service Stopped.

---

## 🧱 Requirements & Dependencies
- **.NET Framework:** 4.8 or later  
- **Language:** C#  
- **IDE:** Visual Studio 2019 or newer  
- **Permissions:** Requires Administrator privileges when running as a Windows Service  
- **Tools:** InstallUtil.exe for service installation

---

## 🧩 Project Structure
FileMonitoringService/
│
├── FileMonitoringService.csproj  
├── Program.cs  
├── FileMonitorService.cs  
├── Logger.cs  
├── App.config  
└── README.md

---

## 🧰 Key Features
- Automatic file detection and renaming using GUIDs  
- Move and track files between source and destination  
- Log all service activities with timestamps  
- Configurable folders via App.config  
- Dual mode operation: Console or Windows Service  
- Auto-creation of folders if not found

---

## 💡 Note
This project is my implementation of the **File Monitoring Service** idea proposed in **Course 24** on [ProgrammingAdvices.com](https://programmingadvices.com).
