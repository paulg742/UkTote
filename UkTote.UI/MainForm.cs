using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using UkTote.Message;

namespace UkTote.UI
{
    public partial class MainForm : Form
    {
        const string LastBetIdFile = "lastbet.id";
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
            txtFeedFolder.Text = Properties.Settings.Default.FeedFolder;
            txtBetOutputFolder.Text = Properties.Settings.Default.BetOutputFolder;

            _gateway.OnConnected += _gateway_OnConnected;
            _gateway.OnDisconnected += _gateway_OnDisconnected;
            _gateway.OnRuOk += _gateway_OnRuOk;
            _gateway.OnRawPacketReceived += _gateway_OnRawPacketReceived;
            _gateway.OnRawPacketSent += _gateway_OnRawPacketSent;

            // update messages
            _gateway.OnMeetingUpdate += _gateway_OnMeetingUpdate;
            _gateway.OnMeetingSalesUpdate += _gateway_OnMeetingSalesUpdate;
            _gateway.OnRacePoolDividendUpdate += _gateway_OnRacePoolDividendUpdate;
            _gateway.OnRacePoolSalesUpdate += _gateway_OnRacePoolSalesUpdate;
            _gateway.OnRacePoolUpdate += _gateway_OnRacePoolUpdate;
            _gateway.OnRaceSalesUpdate += _gateway_OnRaceSalesUpdate;
            _gateway.OnRaceUpdate += _gateway_OnRaceUpdate;
            _gateway.OnRaceWillPayUpdate += _gateway_OnRaceWillPayUpdate;
            _gateway.OnRunnerUpdate += _gateway_OnRunnerUpdate;
            _gateway.OnMeetingPoolDividendUpdate += _gateway_OnMeetingPoolDividendUpdate;

            UpdateButtons();
        }

        private void _gateway_OnMeetingPoolDividendUpdate(MeetingPoolDividendUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("MeetingPoolDividendUpdate", obj);
        }

        private void LogFeed<T>(string type, T obj) where T: MessageBase
        {
            var str = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText($"{txtFeedFolder.Text}\\{type}.{DateTime.UtcNow:yyyyMMddTHHmmssfff}.json", str);
        }

        private void _gateway_OnMeetingUpdate(Message.MeetingUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("MeetingUpdate", obj);
        }

        private void _gateway_OnRunnerUpdate(Message.RunnerUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("RunnerUpdate", obj);
            if (_racecard != null && _racecard.Meetings.ContainsKey(obj.MeetingNumber))
            {
                var meeting = _racecard.Meetings[obj.MeetingNumber];
                if (meeting.Races.ContainsKey(obj.RaceNumber))
                {
                    var race = meeting.Races[obj.RaceNumber];
                    for (var i=0; i < obj.NonRunnerMap.Count; ++i)
                    {
                        if (obj.NonRunnerMap[i] == 1 && race.Runners.ContainsKey(i + 1))
                        {
                            if (!race.Runners[i + 1].IsScratched)
                            {
                                Log($"{race.MeetingNumber} R{race.RaceNumber}-{i + 1} SCR");
                                race.Runners[i + 1].IsScratched = true;
                            }
                            
                            UpdateRacecardTree(race.Runners[i + 1]);
                        }
                    }
                }
            }
        }

        private void _gateway_OnRaceWillPayUpdate(Message.RaceWillPayUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("RaceWillPayUpdate", obj);
        }

        private void _gateway_OnRaceUpdate(Message.RaceUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("RaceUpdate", obj);
        }

        private void _gateway_OnRaceSalesUpdate(Message.RaceSalesUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("RaceSalesUpdate", obj);
        }

        private void _gateway_OnRacePoolUpdate(Message.RacePoolUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("RacePoolUpdate", obj);
        }

        private void _gateway_OnRacePoolSalesUpdate(Message.RacePoolSalesUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("RacePoolSalesUpdate", obj);
        }

        private void _gateway_OnRacePoolDividendUpdate(Message.RacePoolDividendUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("RacePoolDividendUpdate", obj);
        }

        private void _gateway_OnMeetingSalesUpdate(Message.MeetingSalesUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("MeetingSalesUpdate", obj);
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
            if (btnConnect.InvokeRequired)
            {
                Invoke(new Action(() => UpdateButtons()));
            }
            else
            {
                btnConnect.Enabled = !_connected;
                btnGetRacecard.Enabled = _connected && _racecard == null;
                btnExportRacecard.Enabled = _racecard != null;
                btnGetBalance.Enabled = _connected;
                btnMsnRequest.Enabled = _connected;
                btnPayEnquiry.Enabled = _connected;
            }
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
                        numLastBetId.Enabled = true;
                        if (numLastBetId.Value > 0)
                        {
                            numLastBetId_ValueChanged(this, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                }
                finally
                {
                    if (!_connected || !_loggedIn)
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
            if (racecardTreeView.InvokeRequired)
            {
                Invoke(new Action(() => DisplayRacecardTree()));
                return;
            }
            racecardTreeView.Nodes.Clear();
            foreach (var meeting in _racecard.Meetings)
            {
                var meetingNode = racecardTreeView.Nodes.Add($"{meeting.Key} - {meeting.Value.MeetingName} ({meeting.Value.NumberOfRaces} races)");
                meetingNode.Tag = meeting.Value;
                foreach (var race in meeting.Value.Races)
                {
                    var raceNode = meetingNode.Nodes.Add($"R{race.Key} - {race.Value.RaceName} {race.Value.DistanceMeters}m");
                    raceNode.Tag = race.Value;
                    if (race.Value.Runners != null)
                    {
                        var runnersNode = raceNode.Nodes.Add("Runners");
                        foreach (var runner in race.Value.Runners)
                        {
                            var txt = $"{runner.Key}. {runner.Value.RunnerName.Replace("\0", string.Empty)}";
                            if (runner.Value.IsScratched)
                            {
                                txt += " [SCR]";
                            }
                            var runnerNode = runnersNode.Nodes.Add(txt);
                            runnerNode.Tag = runner.Value;
                        }
                    }
                    if (race.Value.RacePools != null)
                    {
                        var poolsNode = raceNode.Nodes.Add("Pools");
                        foreach (var pool in race.Value.RacePools)
                        {
                            poolsNode.Nodes.Add($"P{pool.Key} - {pool.Value.PoolName}");
                        }
                        if (meeting.Value.MeetingPools != null)
                        {
                            foreach (var meetingPool in meeting.Value.MeetingPools)
                            {
                                if (meetingPool.Value.Races.Contains(race.Value.RaceNumber))
                                {
                                    poolsNode.Nodes.Add($"MP{meetingPool.Key}.{Array.IndexOf(meetingPool.Value.Races, race.Value.RaceNumber) + 1} - {meetingPool.Value.PoolName}");
                                }
                            }
                        }
                    }
                }
                if (meeting.Value.MeetingPools != null)
                {
                    var multiLegRootNode = meetingNode.Nodes.Add($"Multi Leg Pools");
                    foreach (var meetingPool in meeting.Value.MeetingPools)
                    {
                        multiLegRootNode.Nodes.Add($"{meetingPool.Value.MeetingPoolNumber}: {meetingPool.Value.PoolName.Replace("\0", string.Empty)} {meetingPool.Value.GetRaces()}");
                    }
                }
            }
        }

        public bool IsTagged(TreeNode node, RunnerReply runner)
        {
            if (node.Tag == null) return false;
            if (!(node.Tag is RunnerReply tag)) return false;
            return tag.MeetingNumber == runner.MeetingNumber && tag.RaceNumber == runner.RaceNumber && tag.RunnerNumber == runner.RunnerNumber;
        }

        public TreeNode GetNode(RunnerReply tag, TreeNode rootNode)
        {
            foreach (TreeNode node in rootNode.Nodes)
            {
                if (IsTagged(node, tag)) return node;

                //recursion
                var next = GetNode(tag, node);
                if (next != null) return next;
            }
            return null;
        }

        public TreeNode GetNode(RunnerReply tag)
        {
            TreeNode itemNode = null;
            foreach (TreeNode node in racecardTreeView.Nodes)
            {
                if (IsTagged(node, tag)) return node;

                itemNode = GetNode(tag, node);
                if (itemNode != null) break;
            }
            return itemNode;
        }

        void UpdateRacecardTree(RunnerReply runner)
        {
            if (racecardTreeView.InvokeRequired)
            {
                Invoke(new Action(() => UpdateRacecardTree(runner)));
                return;
            }
            var node = GetNode(runner);
            if (node != null)
            {
                var txt = $"{runner.RunnerNumber}. {runner.RunnerName.Replace("\0", string.Empty)}";
                if (runner.IsScratched)
                {
                    txt += " [SCR]";
                }
                node.Text = txt;
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
            try
            {
                var lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrEmpty(line.Trim()))
                    {
                        ret.Add(Model.FileBet.Parse(line));
                    }
                }
            }
            catch (Exception)
            {
            }
            return ret;
        }

        //private string DisplaySelections(Selection[] selections)
        //{
        //    selections.GroupBy(s => s.RaceNumber)
        //}

        private async void OnChanged(object source, FileSystemEventArgs e)
        {
            if (_watcherLog.ContainsKey(e.FullPath) && (DateTime.UtcNow - _watcherLog[e.FullPath]).TotalSeconds < 30)
            {
                // ignore this dupe event
                return;
            }
            if (listView1.InvokeRequired)
            {
                Invoke(new Action(() => OnChanged(source, e)));
            }
            else
            {
                listView1.Items.Clear();
                _watcherLog[e.FullPath] = DateTime.UtcNow;
                Log($"{e.FullPath} was found, processing");
                var bets = ProcessBetFile(e.FullPath);
                foreach (var bet in bets)
                {
                    var item = listView1.Items.Add(new ListViewItem(new string[]
                    {
                            bet.Raw,
                            bet.Request == null ? string.Empty : bet.Request.ForDate.ToShortDateString(),
                            bet.Request == null ? string.Empty : bet.Request.MeetingNumber.ToString(),
                            bet.Request == null ? string.Empty : bet.Request.Selections[0].RaceNumber.ToString(),
                            bet.Request == null ? string.Empty : $"{bet.Request.UnitStake/100:N2}",
                            bet.Request == null ? string.Empty : $"{bet.Request.TotalStake/100:N2}",
                            bet.Request == null ? string.Empty : bet.Request.BetCode.ToString(),
                            bet.Request == null ? string.Empty : bet.Request.BetOption.ToString(),
                            bet.Request == null ? string.Empty : string.Join(",", bet.Request?.Selections.Select(s => s.IsBanker > 0 ? s.HorseNumber + 900 : s.HorseNumber)),
                            !bet.IsValid ? bet.Error : string.Empty,
                            string.Empty, // BetId
                            string.Empty, // TSN
                            string.Empty // pay enquiry result
                    }));
                    item.Tag = bet.Request?.Ref;
                }
                var batch = bets
                    .Where(b => b.Request != null && b.IsValid)
                    .Select(b => b.Request)
                    .ToList();
                var results = await _gateway.SellBatch(batch);
                foreach (var result in results)
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.Tag as Guid? == result.Ref)
                        {
                            item.SubItems[9].Text = result.ErrorCode.ToString();

                            if (result.ErrorCode == Message.Enums.ErrorCode.SUCCESS)
                            {
                                item.SubItems[10].Text = result.BetId.ToString();
                                item.SubItems[11].Text = result.TSN;
                            }
                        }
                    }
                    foreach (var bet in bets)
                    {
                        if (bet.Request?.Ref == result.Ref)
                        {
                            bet.Result = result;
                        }
                    }
                }
                var lastBetId = bets.Max(b => b.Result?.BetId);
                if ((lastBetId ?? 0) > 0)
                {
                    numLastBetId.Value = lastBetId.Value;
                }
                DumpBetsOutput(e.FullPath, bets);
                Log($"{bets.Count} bets processed");
            }
        }

        private string GetOutputFilePath(string inputFilePath)
        {
            var filename = Path.GetFileName(inputFilePath);
            var outputFilePath = $"{txtBetOutputFolder.Text}\\{filename}.out";
            var counter = 0;
            while (File.Exists(outputFilePath))
            {
                ++counter;
                outputFilePath = $"{txtBetOutputFolder.Text}\\{filename}.{counter}.out";
            }
            return outputFilePath;
        }

        void DumpBetsOutput(string filePath, List<Model.FileBet> betsWithResults)
        {
            var filename = Path.GetFileName(filePath);
            var outputFilePath = GetOutputFilePath(filePath);
            foreach (var x in betsWithResults)
            {
                var outputLine = $"{x.Raw} > {x.Result?.BetId},{x.Result?.TSN},{x.Result?.ErrorCode},{x.Result?.ErrorText}\n".Replace("\0", string.Empty);
                //var outputLine = $"{x.BetId},{x.TSN},{x.ErrorCode},{x.ErrorText}\r\n".Replace("\0", string.Empty);
                File.AppendAllText($"{outputFilePath}", outputLine);
            }
            File.Delete(filePath);
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

        private void numLastBetId_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_gateway != null && _gateway.IsConnected)
                {
                    _gateway.NextBetId = (int)numLastBetId.Value;
                    var lastBetId = (int)numLastBetId.Value;
                    Log($"Changed last bet id to: {lastBetId}");
                    File.WriteAllText(LastBetIdFile, lastBetId.ToString());
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        private void btnExportBets_Click(object sender, EventArgs e)
        {
            var sb = "RAW,ForDate,MeetingNumber,RaceNumber,UnitStake,TotalStake,BetCode,BetOption,Selections,Status,BetId,TSN\r\n";
            foreach (ListViewItem item in listView1.Items)
            {
                sb += $"\"{item.Text.Replace('\0', ' ')}\",";
                for (var i = 1; i < item.SubItems.Count; ++i)
                {
                    sb += $"\"{item.SubItems[i].Text.Replace('\0', ' ')}\",";
                }
                sb += "\r\n";
            }
            Clipboard.SetText(sb);

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // create folders if they dont exist
            if (!Directory.Exists(txtBetFolder.Text))
            {
                Directory.CreateDirectory(txtBetFolder.Text);
            }
            if (!Directory.Exists(txtFeedFolder.Text))
            {
                Directory.CreateDirectory(txtFeedFolder.Text);
            }
            if (!Directory.Exists(txtBetOutputFolder.Text))
            {
                Directory.CreateDirectory(txtBetOutputFolder.Text);
            }
            if (!File.Exists(LastBetIdFile))
            {
                File.WriteAllLines(LastBetIdFile, new[] { "0" });
            }
            else
            {
                var txt = File.ReadAllText(LastBetIdFile);
                numLastBetId.Value = int.Parse(txt);
            }
        }

        private void btnChangeFeedFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtFeedFolder.Text = dlg.SelectedPath;
            }
        }

        private async void btnMsnRequest_Click(object sender, EventArgs e)
        {
            btnGetBalance.Enabled = false;
            try
            {
                Log("MSN request sent");
                var reply = await _gateway.GetMsn(0);
                if (reply != null)
                {
                    Log(JsonConvert.SerializeObject(reply));
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        private async void btnPayEnquiry_Click(object sender, EventArgs e)
        {
            try
            {
                var dlg = new OpenFileDialog()
                {
                    DefaultExt = "*.out",
                    InitialDirectory = txtBetOutputFolder.Text
                };
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    btnPayEnquiry.Enabled = false;
                    var lines = File.ReadAllLines(dlg.FileName);
                    foreach (var line in lines)
                    {
                        var fields = line.Split('>');
                        if (fields.Length == 2)
                        {
                            var raw = fields[0];
                            var res = fields[1];

                            fields = res.Split(',');
                            var betId = fields[0];
                            var tsn = fields[1];
                            if (!string.IsNullOrEmpty(tsn))
                            {
                                var result = await _gateway.PayEnquiry(tsn);
                                var txt = "";
                                if (result == null)
                                {
                                    txt = "No reply";
                                }
                                else if (result.ErrorCode == Enums.ErrorCode.SUCCESS)
                                {
                                    txt = $"Paid:{((double)result.PayoutAmount) / 100:N2} Void:{((double)result.VoidAmount) / 100:N2} (RAW:{result.PayoutAmount} {result.VoidAmount})";
                                }
                                else
                                {
                                    txt = $"{result.ErrorCode.ToString()}: {result.ErrorText}";
                                }
                                File.AppendAllText($"{dlg.FileName}.pay", $"{raw} > {betId},{tsn.Replace("\0", "")},{txt.Replace("\0", "")}\r\n");
                            }
                        }
                    }
                    //foreach (ListViewItem item in listView1.Items)
                    //{
                    //    var tsn = item.SubItems[11].Text.Replace("\0", "");
                    //    if (!string.IsNullOrEmpty(tsn))
                    //    {
                    //        var result = await _gateway.PayEnquiry(tsn);
                    //        if (result == null)
                    //        {
                    //            item.SubItems[12].Text = "No reply";
                    //        }
                    //        else if (result.ErrorCode == Enums.ErrorCode.SUCCESS)
                    //        {
                    //            item.SubItems[12].Text = $"Paid:{result.PayoutAmount / 100:N2} Void:{result.VoidAmount / 100:N2}";
                    //        }
                    //        else
                    //        {
                    //            item.SubItems[12].Text = $"{result.ErrorCode.ToString()}: {result.ErrorText}";
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                UpdateButtons();
            }
        }

        private void btnChangeBetOutputFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtBetOutputFolder.Text = dlg.SelectedPath;
            }
        }
    }
}
