using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using Newtonsoft.Json;
using UkTote.Message;

namespace UkTote.UI
{
    public partial class MainForm : Form
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(MainForm));
        private readonly ToteGateway _gateway = new ToteGateway(30000);
        private bool _connected;
        private bool _loggedIn;
        private Message.RacecardReply _racecard;
        private FileSystemWatcher _watcher;
        private readonly Dictionary<string, DateTime> _watcherLog = new Dictionary<string, DateTime>();   // use to de-dupe fsw events
        private readonly Dictionary<Guid?, ListViewItem> _itemMap = new Dictionary<Guid?, ListViewItem>();
        private readonly HttpClient _httpClient;

        public MainForm()
        {
            InitializeComponent();
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://hooks.slack.com")
            };
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
            _gateway.OnRaceUpdate += _gateway_OnRaceUpdate;
            _gateway.OnRacePoolUpdate += _gateway_OnRacePoolUpdate;
            _gateway.OnMeetingPoolUpdate += _gateway_OnMeetingPoolUpdate;
            _gateway.OnPoolSubstituteUpdate += _gateway_OnPoolSubstituteUpdate;
            _gateway.OnRaceSalesUpdate += _gateway_OnRaceSalesUpdate;
            _gateway.OnMeetingPoolSalesUpdate += _gateway_OnMeetingPoolSalesUpdate;
            _gateway.OnResultUpdate += _gateway_OnResultUpdate;
            _gateway.OnSubstituteUpdate += _gateway_OnSubstituteUpdate;
            _gateway.OnWeighedInUpdate += _gateway_OnWeighedInUpdate;
            _gateway.OnEndOfRacingUpdate += _gateway_OnEndOfRacingUpdate;
            _gateway.OnRacePoolSalesUpdate += _gateway_OnRacePoolSalesUpdate;
            _gateway.OnRacePoolDividendUpdate += _gateway_OnRacePoolDividendUpdate;
            _gateway.OnMeetingPoolDividendUpdate += _gateway_OnMeetingPoolDividendUpdate;
            _gateway.OnSuperComplexPoolDividendUpdate += _gateway_OnSuperComplexPoolDividendUpdate;
            _gateway.OnMatrixPoolDividendUpdate += _gateway_OnMatrixPoolDividendUpdate;
            _gateway.OnComplexRacePoolDividendUpdate += _gateway_OnComplexRacePoolDividendUpdate;
            _gateway.OnRunnerUpdate += _gateway_OnRunnerUpdate;

            // pay update messages
            _gateway.OnMeetingPayUpdate += _gateway_OnMeetingPayUpdate;
            _gateway.OnRacePayUpdate += _gateway_OnRacePayUpdate;
            _gateway.OnMeetingPoolPayUpdate += _gateway_OnMeetingPoolPayUpdate;
            _gateway.OnRacePoolPayUpdate += _gateway_OnRacePoolPayUpdate;

            // will pay update messages
            _gateway.OnMeetingPoolWillPayUpdate += _gateway_OnMeetingPoolWillPayUpdate;
            _gateway.OnRaceWillPayUpdate += _gateway_OnRaceWillPayUpdate;
            _gateway.OnRaceExtendedWillPayUpdate += _gateway_OnRaceExtendedWillPayUpdate;
            _gateway.OnLegBreakdownUpdate += _gateway_OnLegBreakdownUpdate;
            _gateway.OnMeetingPoolTotalUpdate += _gateway_OnMeetingPoolTotalUpdate;
            _gateway.OnComplexRacePoolTotalUpdate += _gateway_OnComplexRacePoolTotalUpdate;

            _gateway.OnBatchProgress += _gateway_OnBatchProgress;

            UpdateButtons();
        }

        private void _gateway_OnRaceExtendedWillPayUpdate(RaceExtendedWillPayUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("RaceExtendedWillPayUpdate", obj);
        }

        private void _gateway_OnComplexRacePoolTotalUpdate(ComplexRacePoolTotalUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("ComplexRacePoolTotalUpdate", obj);
        }

        private void _gateway_OnMeetingPoolTotalUpdate(MeetingPoolTotalUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("MeetingPoolTotalUpdate", obj);
        }

        private void _gateway_OnLegBreakdownUpdate(LegBreakdownUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("LegBreakdownUpdate", obj);
        }

        private void _gateway_OnMeetingPoolWillPayUpdate(MeetingPoolWillPayUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("MeetingPoolWillPayUpdate", obj);
        }

        private void _gateway_OnRacePoolPayUpdate(RacePoolPayUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("RacePoolPayUpdate", obj);
        }

        private void _gateway_OnMeetingPoolPayUpdate(MeetingPoolPayUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("MeetingPoolPayUpdate", obj);
        }

        private void _gateway_OnRacePayUpdate(RacePayUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("RacePayUpdate", obj);
        }

        private void _gateway_OnMeetingPayUpdate(MeetingPayUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("MeetingPayUpdate", obj);
        }

        private void _gateway_OnComplexRacePoolDividendUpdate(ComplexRacePoolDividendUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("ComplexRacePoolDividendUpdate", obj);
        }

        private void _gateway_OnMatrixPoolDividendUpdate(MatrixPoolDividendUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("MatrixPoolDividendUpdate", obj);
        }

        private void _gateway_OnSuperComplexPoolDividendUpdate(SuperComplexPoolDividendUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("SuperComplexPoolDividendUpdate", obj);
        }

        private void _gateway_OnEndOfRacingUpdate(EndOfRacingUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("EndOfRacingUpdate", obj);
        }

        private void _gateway_OnWeighedInUpdate(WeighedInUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("WeighedInUpdate", obj);
        }

        private void _gateway_OnSubstituteUpdate(SubstituteUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("SubstituteUpdate", obj);
        }

        private void _gateway_OnResultUpdate(ResultUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("ResultUpdate", obj);
        }

        private void _gateway_OnMeetingPoolSalesUpdate(MeetingPoolSalesUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("MeetingPoolSalesUpdate", obj);
        }

        private void _gateway_OnPoolSubstituteUpdate(PoolSubstituteUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("PoolSubstituteUpdate", obj);
        }

        private void _gateway_OnMeetingPoolUpdate(MeetingPoolUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("MeetingPoolUpdate", obj);
        }

        private void _gateway_OnBatchProgress(int complete, int total)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => _gateway_OnBatchProgress(complete, total)));
            }
            else
            {
                if (batchProgressBar.Maximum != total) batchProgressBar.Maximum = total;
                batchProgressBar.Value = complete;
                var percentComplete = ((double)complete) / total;
                statusLabel.Text = $"{complete}/{total} {percentComplete:P0}";
            }
        }

        private void _gateway_OnMeetingPoolDividendUpdate(MeetingPoolDividendUpdate obj)
        {
            Log(JsonConvert.SerializeObject(obj));
            LogFeed("MeetingPoolDividendUpdate", obj);
        }

        private void LogFeed<T>(string type, T obj) where T : MessageBase
        {
            try
            {
                var str = JsonConvert.SerializeObject(obj, Formatting.Indented).Replace("\\u0000", string.Empty);
                var fileName = $"{txtFeedFolder.Text}\\{type}.{DateTime.UtcNow:yyyyMMddTHHmmssfff}.json";
                if (obj is IRacePoolUpdate)
                {
                    var update = (IRacePoolUpdate)obj;
                    if (_racecard?.Meetings.ContainsKey(update.MeetingNumber) ?? false)
                    {
                        var meeting = _racecard?.Meetings[update.MeetingNumber];
                        var racePool = meeting.Races[update.RaceNumber].RacePools[update.PoolNumber];
                        //var betCode = (Enums.BetCode)update.PoolNumber;
                        fileName = $"{txtFeedFolder.Text}\\{meeting.MeetingName.Trim('\0')}-R{update.RaceNumber}-{racePool.PoolName.Trim('\0')}.{type}.{DateTime.UtcNow:yyyyMMddTHHmmssfff}.json";
                    }
                }
                else if (obj is IPoolUpdate)
                {
                    var update = (IPoolUpdate)obj;
                    if (_racecard?.Meetings.ContainsKey(update.MeetingNumber) ?? false)
                    {
                        var meeting = _racecard?.Meetings[update.MeetingNumber];
                        var meetingPool = meeting.MeetingPools[update.PoolNumber];
                        //var betCode = (Enums.BetCode)update.PoolNumber;
                        fileName = $"{txtFeedFolder.Text}\\{meeting.MeetingName.Trim('\0')}-{meetingPool.PoolName.Trim('\0')}.{type}.{DateTime.UtcNow:yyyyMMddTHHmmssfff}.json";
                    }
                }
                else if (obj is IRaceUpdate)
                {
                    var update = (IRaceUpdate)obj;
                    if (_racecard?.Meetings.ContainsKey(update.MeetingNumber) ?? false)
                    {
                        var meeting = _racecard?.Meetings[update.MeetingNumber];
                        fileName = $"{txtFeedFolder.Text}\\{meeting.MeetingName.Trim('\0')}-R{update.RaceNumber}.{type}.{DateTime.UtcNow:yyyyMMddTHHmmssfff}.json";
                    }
                }
                else if (obj is IUpdate)
                {
                    var update = (IUpdate)obj;
                    if (_racecard?.Meetings.ContainsKey(update.MeetingNumber) ?? false)
                    {
                        var meeting = _racecard?.Meetings[update.MeetingNumber];
                        fileName = $"{txtFeedFolder.Text}\\{meeting.MeetingName.Trim('\0')}.{type}.{DateTime.UtcNow:yyyyMMddTHHmmssfff}.json";
                    }
                }
                _logger.DebugFormat("Logging to: {0}", fileName);
                File.WriteAllText(fileName, str);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void ArchiveFeed()
        {
            if (btnArchiveFeed.InvokeRequired)
            {
                Invoke(new Action(() => btnArchiveFeed.Enabled = false));
            }
            
            try
            {
                var files = Directory.GetFiles(txtFeedFolder.Text);
                Log($"Running archive task in {txtFeedFolder.Text}");

                if (files.Length > 0) Log($"Archiving {files.Length} files...");
                foreach (var file in files)
                {
                    var match = Regex.Match(file, ".*\\.(.*?)\\.json");
                    if (match.Success)
                    {
                        var dateStr = match.Groups[1].Value.Substring(0, 8);
                        var archiveFolder = Path.Combine(txtFeedFolder.Text, dateStr);
                        if (!Directory.Exists(archiveFolder))
                        {
                            Directory.CreateDirectory(archiveFolder);
                        }
                        var destFile = Path.Combine(archiveFolder, Path.GetFileName(file));
                        File.Move(file, destFile);
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Error archiving files, check log");
                _logger.Error(ex);
            }
            finally
            {
                if (btnArchiveFeed.InvokeRequired)
                {
                    Invoke(new Action(() => btnArchiveFeed.Enabled = true));
                }
                Log("Archive task completed");
            }
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
                    for (var i = 0; i < obj.NonRunnerMap.Count; ++i)
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

        private void UpdateButtons()
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
                    var message = new Model.SlackMessage()
                    {
                        Text = $"{txtUsername.Text} connecting to {txtHostIpAddress.Text}:{(int)numHostPort.Value} - Version: {ApplicationVersion}"
                    };
                    var request = new HttpRequestMessage(HttpMethod.Post, "/services/T03UJ822ELU/B03UCR5GY3X/1OQ00QQ7JGAzLjaBYt9X5qyt")
                    {
                        Content = new StringContent(message.ToString(), 
                            Encoding.UTF8,
                            "application/json")
                    };
                    
                    var resp = await _httpClient.SendAsync(request);

                    if (!resp.IsSuccessStatusCode)
                    {
                        Log("Audit failure - please contact support");
                    }
                    else
                    {
                        _connected = await Task.Factory.StartNew(() => _gateway.Connect(txtHostIpAddress.Text, (int)numHostPort.Value));
                        if (!_connected)
                        {
                            Log($"Could not connect to {txtHostIpAddress.Text}");
                        }
                        else
                        {
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
            _logger.Debug(text);
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

            if (_racecard == null) return;
            LogFeed("RaceCardReply", _racecard);
        }

        private void DisplayRacecardTree()
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

        private void UpdateRacecardTree(RunnerReply runner)
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

        private void DisplayBalance(Message.CurrentBalanceReply currentBalance)
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
                LogFeed("CurrentBalanceReply", currentBalance);
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

        private void StopWatchingFolder()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
            }
        }

        private void StartWatchingFolder()
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

        private List<Model.FileBet> ProcessBetFile(string path)
        {
            var ret = new List<Model.FileBet>();
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line.Trim()))
                {
                    ret.Add(Model.FileBet.Parse(line));
                }
            }
            return ret;
        }

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
                try
                {
                    Log($"{e.FullPath} was found, processing");
                    var bets = ProcessBetFile(e.FullPath);
                    listView1.Items.Clear();
                    _itemMap.Clear();
                    _watcherLog[e.FullPath] = DateTime.UtcNow;
                    
                    var betMap = new Dictionary<Guid?, Model.FileBet>();
                    listView1.BeginUpdate();
                    foreach (var bet in bets)
                    {
                        var item = listView1.Items.Add(new ListViewItem(new string[]
                        {
                            bet.Raw,
                            bet.Request == null ? string.Empty : bet.Request.ForDate.ToShortDateString(),
                            bet.Request == null ? string.Empty : bet.Request.MeetingNumber.ToString(),
                            bet.Request == null ? string.Empty : bet.Request.Selections[0].RaceNumber.ToString(),
                            bet.Request == null ? string.Empty : $"{((decimal)bet.Request.UnitStake)/100:N2}",
                            bet.Request == null ? string.Empty : $"{((decimal)bet.Request.TotalStake)/100:N2}",
                            bet.Request == null ? string.Empty : bet.Request.BetCode.ToString(),
                            bet.Request == null ? string.Empty : bet.Request.BetOption.ToString(),
                            bet.Request == null ? string.Empty : string.Join(",", bet.Request?.Selections.Select(s => s.IsBanker > 0 ? s.HorseNumber + 900 : s.HorseNumber)),
                            !bet.IsValid ? bet.Error : string.Empty,
                            string.Empty, // BetId
                            string.Empty, // TSN
                            string.Empty // pay enquiry result
                        }));
                        if (bet?.Request?.Ref != null)
                        {
                            item.Tag = bet.Request?.Ref;
                            _itemMap[bet.Request?.Ref] = item;
                            betMap[bet.Request?.Ref] = bet;
                        }
                    }
                    listView1.EndUpdate();
                    var batch = bets
                        .Where(b => b.Request != null && b.IsValid)
                        .Select(b => b.Request)
                        .ToList();
                    var results = await _gateway.SellBatch(batch);

                    listView1.BeginUpdate();
                    foreach (var result in results)
                    {
                        var item = _itemMap[result.Ref];
                        item.SubItems[9].Text = result.ErrorCode.ToString();

                        if (result.ErrorCode == Message.Enums.ErrorCode.Success)
                        {
                            item.SubItems[10].Text = result.BetId.ToString();
                            item.SubItems[11].Text = result.Tsn;
                        }

                        var bet = betMap[result.Ref];
                        bet.Result = result;
                    }
                    listView1.EndUpdate();

                    var lastBetId = bets.Max(b => b.Result?.BetId);
                    if ((lastBetId ?? 0) > 0)
                    {
                        numLastBetId.Value = lastBetId.Value;
                    }
                    DumpBetsOutput(e.FullPath, bets);
                    Log($"{bets.Count} bets processed");
                }
                catch (Exception ex)
                {
                    Log($"Error processing: {ex.Message}");
                }
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

        private void DumpBetsOutput(string filePath, List<Model.FileBet> betsWithResults)
        {
            var filename = Path.GetFileName(filePath);
            var outputFilePath = GetOutputFilePath(filePath);
            var outputTxt = string.Empty;
            foreach (var x in betsWithResults)
            {
                var outputLine = $"{x.Raw} > {x.Result?.BetId},{x.Result?.Tsn},{x.Result?.ErrorCode},{x.Result?.ErrorText},{x.Request?.Ref}\n".Replace("\0", string.Empty);
                //var outputLine = $"{x.BetId},{x.TSN},{x.ErrorCode},{x.ErrorText}\r\n".Replace("\0", string.Empty);
                outputTxt += outputLine;
            }
            File.AppendAllText($"{outputFilePath}", outputTxt);
            File.Delete(filePath);
        }

        private void btnChangeBetFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog()
            {
                SelectedPath = txtBetFolder.Text
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtBetFolder.Text = dlg.SelectedPath;
                StartWatchingFolder();
                SaveSettings();
            }
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.BetFolder = txtBetFolder.Text;
            Properties.Settings.Default.BetOutputFolder = txtBetOutputFolder.Text;
            Properties.Settings.Default.FeedFolder = txtFeedFolder.Text;
            Properties.Settings.Default.Save();
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
                    Properties.Settings.Default.LastBetId = lastBetId;
                    Properties.Settings.Default.Save();
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

        private async void MainForm_Load(object sender, EventArgs e)
        {
#if EIGHT_BYTE_MONEY
            this.Text += " (8 byte Money)";
#endif
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
            if (Properties.Settings.Default.LastRunTime.Date < DateTime.UtcNow.Date)
            {
                // reset the bet ID
                Properties.Settings.Default.LastBetId = 0;
            }
            numLastBetId.Value = Properties.Settings.Default.LastBetId;
            Properties.Settings.Default.LastRunTime = DateTime.UtcNow;
            Properties.Settings.Default.Save();

            versionLabel.Text = ApplicationVersion;
            versionLabel.Alignment = ToolStripItemAlignment.Right;

            await Task.Run(() => ArchiveFeed());
        }

        private string ApplicationVersion
        {
            get
            {
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return $"Version: {version.Major}.{version.Minor}";
            }
        }

        private void btnChangeFeedFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog()
            {
                SelectedPath = txtFeedFolder.Text
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtFeedFolder.Text = dlg.SelectedPath;
                SaveSettings();
            }
        }

        private async void btnMsnRequest_Click(object sender, EventArgs e)
        {
            btnMsnRequest.Enabled = false;
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
                    InitialDirectory = txtBetOutputFolder.Text,
                    Filter = "out files|*.out"
                };
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    btnPayEnquiry.Enabled = false;
                    var lines = File.ReadAllLines(dlg.FileName);
                    var tsnList = new List<string>();
                    var betMap = new Dictionary<string, (string raw, long betId, Guid betRef)>();
                    foreach (var line in lines)
                    {
                        var fields = line.Split('>');
                        if (fields.Length == 2)
                        {
                            var raw = fields[0];
                            var res = fields[1];

                            fields = res.Split(',');
                            try
                            {
                                if (!long.TryParse(fields[0], out long betId)) continue;
                                var tsn = fields[1].Trim();
                                if (!Guid.TryParse(fields.Last(), out Guid betRef)) continue;
                                if (!string.IsNullOrEmpty(tsn))
                                {
                                    tsnList.Add(tsn);
                                    betMap[tsn] = (raw, betId, betRef);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    if (tsnList.Count > 0)
                    {
                        var results = await _gateway.PayEnquiryBatch(tsnList);
                        var outputTxt = string.Empty;
                        //listView1.Clear();
                        listView1.BeginUpdate();
                        foreach (var result in results.OrderBy(r => betMap[r.Tsn].betId))
                        {
                            var txt = "";
                            if (result == null)
                            {
                                txt = "No reply";
                            }
                            else if (result.ErrorCode == Enums.ErrorCode.Success)
                            {
                                txt = $"Paid:{((double)result.PayoutAmount) / 100:N2} Void:{((double)result.VoidAmount) / 100:N2} (RAW:{result.PayoutAmount} {result.VoidAmount})";
                            }
                            else
                            {
                                txt = $"{result.ErrorCode.ToString()}: {result.ErrorText}";
                            }
                            outputTxt += $"{betMap[result.Tsn].raw} > {betMap[result.Tsn].betId},{result.Tsn.Replace("\0", "")},{txt.Replace("\0", "")}\r\n";

                            if (_itemMap.ContainsKey(betMap[result.Tsn].betRef))
                            {
                                _itemMap[betMap[result.Tsn].betRef].SubItems[12].Text = txt;
                            }
                        }
                        listView1.EndUpdate();
                        File.AppendAllText($"{dlg.FileName}.pay", outputTxt);
                    }
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
            var dlg = new FolderBrowserDialog()
            {
                SelectedPath = txtBetOutputFolder.Text
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtBetOutputFolder.Text = dlg.SelectedPath;
                SaveSettings();
            }
        }

        private async void btnArchiveFeed_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ArchiveFeed());
        }

        private void btnOpenBetInputFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", txtBetFolder.Text);
        }

        private void btnOpenBetOutputFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", txtBetOutputFolder.Text);
        }

        private void btnOpenFeedOutputFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", txtFeedFolder.Text);
        }
    }
}
