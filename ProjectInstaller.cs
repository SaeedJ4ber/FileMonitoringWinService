using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace FileMonitoringWinService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller: System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public ProjectInstaller()
        {
            InitializeComponent();

            processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };

            serviceInstaller = new ServiceInstaller
            {
                ServiceName = "FileMonitoringWinService",
                DisplayName = "File Monitoring Service",
                Description = "A Windows service that monitoring folders and files",
                StartType = ServiceStartMode.Manual
            };

            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);

        }
    }
}
