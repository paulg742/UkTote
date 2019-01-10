using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UkTote.UI
{
    public partial class MainForm : Form
    {
        private readonly ToteGateway _gateway = new ToteGateway(5000);
        private bool _connected;
        private bool _loggedIn;

        public MainForm()
        {
            InitializeComponent();
            txtHostIpAddress.Text = Properties.Settings.Default.HostIpAddress;
            numHostPort.Value = Properties.Settings.Default.HostPort;
            txtUsername.Text = Properties.Settings.Default.Username;
            txtPassword.Text = Properties.Settings.Default.Password;

            _gateway.OnConnected += _gateway_OnConnected;
            _gateway.OnDisconnected += _gateway_OnDisconnected;
            _gateway.OnRuOk += _gateway_OnRuOk;
            _gateway.OnRawPacketReceived += _gateway_OnRawPacketReceived;
            _gateway.OnRawPacketSent += _gateway_OnRawPacketSent;
        }

        private void _gateway_OnRawPacketSent(byte[] buffer)
        {
            Log($"[TX] {string.Join(" ", Array.ConvertAll(buffer, b => b.ToString("X2")))}");
        }

        private void _gateway_OnRawPacketReceived(byte[] buffer)
        {
            Log($"[RX] {string.Join(" ", Array.ConvertAll(buffer, b => b.ToString("X2")))}");
        }

        private void _gateway_OnRuOk()
        {
            Log("RUOK received, reply sent");
        }

        private void _gateway_OnDisconnected(string obj)
        {
            Log($"Gateway disconnected: {obj}");
            _connected = false;
            btnConnect.Enabled = true;
        }

        private void _gateway_OnConnected()
        {
            Log($"Gateway connected");
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            if (!_gateway.IsConnected)
            {
                try
                {
                    _connected = await Task.Factory.StartNew(() => _gateway.Connect(txtHostIpAddress.Text, (int)numHostPort.Value));
                    if (!_connected)
                    {
                        Log($"Could not connect to {txtHostIpAddress.Text}");

                    }
                    _loggedIn = await _gateway.Login(txtUsername.Text, txtPassword.Text);
                    if (!_loggedIn)
                    {
                        Log($"Login failed with {txtUsername.Text}");
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                }
                finally
                {
                    if (!_connected && !_loggedIn)
                    {
                        _gateway.Disconnect();
                    }
                    if (!_connected || !_loggedIn)
                    {
                        btnConnect.Enabled = true;
                    }
                    if (_loggedIn)
                    {
                        Log("Login successful");
                    }
                }
            }
        }

        private void Log(string text)
        {
            if (listBoxLog.InvokeRequired)
            {
                Invoke(new Action(() => Log(text)));
            }
            else
            {
                while (listBoxLog.Items.Count > 10000)
                {
                    listBoxLog.Items.RemoveAt(listBoxLog.Items.Count - 1);
                }
                listBoxLog.Items.Insert(0, text);
            }
        }

        private void UpdateStatus(string text)
        {
            if (statusStrip1.InvokeRequired)
            {
                Invoke(new Action(() => UpdateStatus(text)));
            }
            else
            {
                statusLabel.Text = text;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_gateway.IsConnected)
            {
                try
                {
                    _gateway.Disconnect();
                }
                catch (Exception)
                {

                }
            }
        }

        private void btnCopyLog_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            foreach (var item in listBoxLog.Items)
            {
                text += item.ToString() + "\r\n";
            }
            Clipboard.SetText(text);
            UpdateStatus("Copied to clipboard!");
        }
    }
}
