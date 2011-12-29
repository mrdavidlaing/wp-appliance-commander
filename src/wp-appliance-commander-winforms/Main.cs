using System;
using System.Linq;
using System.Windows.Forms;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace wp_appliance_commander_winforms
{
    public partial class Main : Form
    {
        private string host = "wordpress-appliance-master";
        private string password = "wordpress";
        private SshClient sshClient;
        private string username = "wordpress";

        public Main()
        {
            InitializeComponent();
        }

        private void dependanciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Dependencies().ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {
        }

        private void ConnectionInfoPasswordExpired(object sender, AuthenticationPasswordChangeEventArgs e)
        {
            Log("New password required");
        }

        private void ConnectionInfoAuthenticationBanner(object sender, AuthenticationBannerEventArgs e)
        {
            Log(e.Username);
            Log(e.Language);
            Log(e.BannerMessage);
        }

        private void SshClientErrorOccurred(object sender, ExceptionEventArgs e)
        {
            Log("An ssh error occured:");
            Log(e.Exception.ToString());
        }

        private void Log(string message)
        {
            StatusListBox.Items.Add(message);
        }

        private void LocalVmToolStripMenuItemClick(object sender, EventArgs e)
        {
            var connectionInfo = new PasswordConnectionInfo(host, 22, username, password);
            connectionInfo.AuthenticationBanner += ConnectionInfoAuthenticationBanner;
            connectionInfo.PasswordExpired += ConnectionInfoPasswordExpired;
            sshClient = new SshClient(connectionInfo);
            sshClient.ErrorOccurred += SshClientErrorOccurred;
            Log(string.Format("Connecting to {0}:{1} as {2}", connectionInfo.Host, connectionInfo.Port, connectionInfo.Username));

            try
            {
                sshClient.Connect();
                var tunnel = sshClient.AddForwardedPort<ForwardedPortLocal>("localhost", 20080, "www.google.com", 80);
                tunnel.Start();

            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
            finally
            {
                sshClient.Dispose();
            }
            Log("Connected");
            sshClient.ForwardedPorts.ToList().ForEach(p => Log(string.Format("SSH tunnel: {0}:{1} --> {2}:{3}", p.BoundHost, p.BoundPort, p.Host, p.Port) ));
        }

        private void MainFormClosing(object sender, FormClosingEventArgs e)
        {
            sshClient.Disconnect();
            sshClient.Dispose();
        }
    }
}