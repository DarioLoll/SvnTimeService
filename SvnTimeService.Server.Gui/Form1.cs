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

        private delegate void Log(string logMessage);
        
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
            Invoke(new Log(LogSystemInfo), logMessage);
        }

        private void OnSystemInfoLogged(object sender, string logMessage)
        {
            Invoke(new Log(LogRequestInfo), logMessage);
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
            if (!string.IsNullOrEmpty(tbxPort.Text) && int.TryParse(tbxPort.Text, out int port))
            {
                if (!string.IsNullOrEmpty(tbxIp.Text))
                {
                    _server.Start(port, tbxIp.Text);
                }
                else
                {
                    _server.Start(port);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(tbxIp.Text))
                {
                    _server.Start(TcpService.DefaultPort, tbxIp.Text);
                    //_server.Start(4004, tbxIp.Text);
                }
                else
                {
                    _server.Start();
                }
            }
            RefreshButtons();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _server.Stop();
            RefreshButtons();
        }

        private void RefreshButtons()
        {
            if (_server.IsOnline)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
            else
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
        }
    }
}