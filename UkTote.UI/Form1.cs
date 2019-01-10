using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace UkTote.UI
{
    public partial class MainForm : Form
    {
        private readonly ToteGateway _gateway = new ToteGateway(5000);
        private bool _connected;
        private bool _loggedIn;
        private Message.RacecardReply _racecard;
        private FileSystemWatcher _watcher;
        private Dictionary<string, DateTime> _watcherLog = new Dictionary<string, DateTime>();   // use to de-dupe fsw events

        public MainForm()
        {
            InitializeComponent();
            txtHostIpAddress.Text = Properties.Settings.Default.HostIpAddress;
            numHostPort.Value = Properties.Settings.Default.HostPort;
            txtUsername.Text = Properties.Settings.Default.Username;
            txtPassword.Text = Properties.Settings.Default.Password;
            txtBetFolder.Text = Properties.Settings.Default.BetFolder;

            _gateway.OnConnected += _gateway_OnConnected;
            _gateway.OnDisconnected += _gateway_OnDisconnected;
            _gateway.OnRuOk += _gateway_OnRuOk;
            _gateway.OnRawPacketReceived += _gateway_OnRawPacketReceived;
            _gateway.OnRawPacketSent += _gateway_OnRawPacketSent;

            UpdateButtons();
        }

        private void _gateway_OnRawPacketSent(byte[] buffer)
        {
            if (!checkBoxHideRawComms.Checked)
            {
                Log($"[TX] {string.Join(" ", Array.ConvertAll(buffer, b => b.ToString("X2")))}");
            }
        }

        private void _gateway_OnRawPacketReceived(byte[] buffer)
        {
            if (!checkBoxHideRawComms.Checked)
            {
                Log($"[RX] {string.Join(" ", Array.ConvertAll(buffer, b => b.ToString("X2")))}");
            }
        }

        private void _gateway_OnRuOk()
        {
            Log("RUOK received, reply sent");
        }

        private void _gateway_OnDisconnected(string obj)
        {
            Log($"Gateway disconnected: {obj}");
            _connected = false;
            StopWatchingFolder();
            UpdateButtons();
        }

        private void _gateway_OnConnected()
        {
            Log($"Gateway connected");
            StartWatchingFolder();
        }

        void UpdateButtons()
        {
            btnConnect.Enabled = !_connected;
            btnGetRacecard.Enabled = _connected && _racecard == null;
            btnExportRacecard.Enabled = _racecard != null;
            btnGetBalance.Enabled = _connected;
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
                    else
                    {
                        btnGetRacecard.Enabled = true;
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
                    UpdateButtons();
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

        private async void btnGetRacecard_Click(object sender, EventArgs e)
        {
            btnGetRacecard.Enabled = false;
            try
            {
                Log("Requesting racecard");
                _racecard = await _gateway.GetRacecardFast(DateTime.UtcNow.Date, true);
                Log($"Racecard complete {_racecard.NumMeetings} meetings downloaded");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
            UpdateButtons();
            DisplayRacecardTree();
        }

        void DisplayRacecardTree()
        {
            if (_racecard == null) return;
            foreach (var meeting in _racecard.Meetings)
            {
                var meetingNode = racecardTreeView.Nodes.Add($"{meeting.Key} - {meeting.Value.MeetingName} ({meeting.Value.NumberOfRaces} races)");
                foreach (var race in meeting.Value.Races)
                {
                    var raceNode = meetingNode.Nodes.Add($"R{race.Key} - {race.Value.RaceName} {race.Value.DistanceMeters}m");
                    if (race.Value.Runners != null)
                    {
                        var runnersNode = raceNode.Nodes.Add("Runners");

                        foreach (var runner in race.Value.Runners)
                        {
                            runnersNode.Nodes.Add($"{runner.Key}. {runner.Value.RunnerName}");
                        }
                    }
                    if (race.Value.RacePools != null)
                    {
                        var poolsNode = raceNode.Nodes.Add("Pools");
                        foreach (var pool in race.Value.RacePools)
                        {
                            poolsNode.Nodes.Add($"{pool.Key} - {pool.Value.PoolName}");
                        }
                    }
                }
            }
        }

        private void btnExportRacecard_Click(object sender, EventArgs e)
        {
            if (_racecard == null) return;
            var text = Newtonsoft.Json.JsonConvert.SerializeObject(_racecard, Newtonsoft.Json.Formatting.Indented)
                .Replace("\\u0000", string.Empty);
            Clipboard.SetText(text);
            UpdateStatus("Copied racecard to clipboard!");
        }

        void DisplayBalance(Message.CurrentBalanceReply currentBalance)
        {
            balanceLabel.Text = $"Remaining: {((double)currentBalance.RemainingBalance) / 100:N2} Stake Limit: {((double)currentBalance.StakeLimit) / 100:N2}";
        }

        private async void btnGetBalance_Click(object sender, EventArgs e)
        {
            btnGetBalance.Enabled = false;
            try
            {
                Log("Requesting balance");
                var currentBalance = await _gateway.GetCurrentBalance();
                if (currentBalance != null)
                {
                    Log(JsonConvert.SerializeObject(currentBalance));
                    DisplayBalance(currentBalance);
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
            UpdateButtons();
        }

        void StopWatchingFolder()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
            }
        }

        void StartWatchingFolder()
        {
            StopWatchingFolder();    

            _watcher = new FileSystemWatcher
            {
                Path = txtBetFolder.Text,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.bet"
            };
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.EnableRaisingEvents = true;
            Log($"Watching folder {txtBetFolder.Text} for .bet files");
        }

        List<Model.FileBet> ProcessBetFile(string path)
        {
            var ret = new List<Model.FileBet>();
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                ret.Add(Model.FileBet.Parse(line));
            }
            return ret;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (_watcherLog.ContainsKey(e.FullPath) && (DateTime.UtcNow - _watcherLog[e.FullPath]).TotalSeconds < 1)
            {
                // ignore this dupe event
                return;
            }
            _watcherLog[e.FullPath] = DateTime.UtcNow;
            Log($"{e.FullPath} was found, processing");
            var bets = ProcessBetFile(e.FullPath);
            Log($"{bets.Count} bets processed");
        }

        private void btnChangeBetFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtBetFolder.Text = dlg.SelectedPath;
                StartWatchingFolder();
            }
        }
    }
}
