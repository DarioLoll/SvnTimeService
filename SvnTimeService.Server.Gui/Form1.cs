using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SvnTimeService.Server.Core;

namespace SvnTimeService.Server.Gui
{
    public partial class Form1 : Form
    {
        private TcpService _server;

        private GuiLogger _logger;

        private delegate void LogMethod(string logMessage);
        
        public Form1()
        {
            InitializeComponent();
            _logger = new GuiLogger();
            _logger.SystemInfoLogged += OnSystemInfoLogged;
            _logger.RequestInfoLogged += OnRequestInfoLogged;
            _server = new TcpService(_logger);
        }

        private void OnRequestInfoLogged(object sender, string logMessage)
        {
            Invoke(new LogMethod(LogSystemInfo), logMessage);
        }

        private void OnSystemInfoLogged(object sender, string logMessage)
        {
            Invoke(new LogMethod(LogRequestInfo), logMessage);
        }

        private void LogSystemInfo(string logMessage)
        {
            lstSystemLogs.Items.Add(logMessage);
        }

        private void LogRequestInfo(string logMessage)
        {
            lstRequestLogs.Items.Add(logMessage);
        }

        
        private void btnStart_Click(object sender, EventArgs e)
        {
            _server.Start(IPAddress.Loopback, 4004);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _server.Stop();
        }

        
        
    }
}