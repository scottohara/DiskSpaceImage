using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace ScoWare
{
    public partial class DiskSpaceImageService : ServiceBase
    {
        private EventLog log;
        private DiskSpaceImageWriter writer;
        private Timer t;

        public DiskSpaceImageService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Setup logging
            this.log = new EventLog("Application");
            if (!EventLog.SourceExists("DiskSpaceImage"))
            {
                EventLog.CreateEventSource("DiskSpaceImage", "Application");
            }
            this.log.Source = "DiskSpaceImage";

            // Log the parameters used
            this.log.WriteEntry("Image size is " + ConfigurationManager.AppSettings["ImageWidth"] + "px by " + ConfigurationManager.AppSettings["ImageHeight"] + "px", EventLogEntryType.Information);
            this.log.WriteEntry("Image style is " + ConfigurationManager.AppSettings["FontName"] + " " + ConfigurationManager.AppSettings["FontSize"] + "em " + ConfigurationManager.AppSettings["FontColour"], EventLogEntryType.Information);
            this.log.WriteEntry("Image path is " + ConfigurationManager.AppSettings["ImageName"], EventLogEntryType.Information);

            // Initialise the writer object
            this.writer = new DiskSpaceImageWriter(
                   Convert.ToInt16(ConfigurationManager.AppSettings["ImageWidth"]),
                   Convert.ToInt16(ConfigurationManager.AppSettings["ImageHeight"]),
                   ConfigurationManager.AppSettings["FontName"],
                   float.Parse(ConfigurationManager.AppSettings["FontSize"]),
                   ConfigurationManager.AppSettings["FontColour"],
                   ConfigurationManager.AppSettings["ImageName"],
                   Convert.ToBoolean(ConfigurationManager.AppSettings["ShowVolumeLabel"]));

            // Write out the image
            this.OnTimedEvent(null, null);

            // Setup a timer to fire every X milliseconds
            this.t = new Timer((Convert.ToDouble(ConfigurationManager.AppSettings["TimerInterval"])));
            this.log.WriteEntry("Refresh interval set to " + this.t.Interval + "ms", EventLogEntryType.Information);
            this.t.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            this.t.AutoReset = true;
            this.t.Start();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            this.writer.RenderImage();
            this.writer.SaveImage();
        }
    }

    [RunInstallerAttribute(true)]
    public class DiskSpaceImageInstaller: Installer
	{
		private ServiceInstaller serviceInstaller;
		private ServiceProcessInstaller processInstaller;

        public DiskSpaceImageInstaller()
		{
			// Instantiate installers for process and services.
			processInstaller = new ServiceProcessInstaller();
			serviceInstaller = new ServiceInstaller();

			// The services run under the system account.
			processInstaller.Account = ServiceAccount.LocalSystem;

			// The services are started manually.
			serviceInstaller.StartType = ServiceStartMode.Manual;

			// ServiceName must equal those on ServiceBase derived classes.            
            serviceInstaller.ServiceName = "DiskSpaceImage";
			serviceInstaller.DisplayName = "Media Center Start Page Disk Space Monitor";

			// Add installers to collection. Order is not important.
			Installers.Add(serviceInstaller);
			Installers.Add(processInstaller);
		}
	}
}
