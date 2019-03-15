using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace WindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            serviceProcessInstaller.Account = ServiceAccount.LocalService;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
        }

        private void ServiceProcessInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
        }

        private void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceInstaller.ServiceName))
                {
                    sc.Start();
                }
            }
            catch(Exception)
            {
            }
        }
    }
}
