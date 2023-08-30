using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UkTote.UI
{
    public class FileProcessQueue : CancellableQueueWorker<string>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(FileProcessQueue));
        private readonly Dictionary<string, DateTime> _log = new Dictionary<string, DateTime>();   // use to de-dupe fsw events
        private readonly IHandleQueueUpdates _updates;
        private readonly IToteGateway _toteGateway;
        private readonly int _dupeBetWindowSeconds;

        public FileProcessQueue(IHandleQueueUpdates updates, IToteGateway gateway) : base(0, 1)
        {
            _updates = updates;
            _toteGateway = gateway;
            _dupeBetWindowSeconds = Properties.Settings.Default.DupeBetWindowSeconds;
        }
        protected override void OnItemQueued(string t)
        {
         //   _updates.Log($"{t} queued");
        }

        protected override bool OnStart()
        {
            _updates.Log("File process queue started");
            return true;
        }

        protected override bool OnStop()
        {
            _updates.Log("File process queue stopped");
            return true;
        }

        private List<Model.FileBet> ProcessBetFile(string path)
        {
            _logger.InfoFormat("Processing bet file: {0}", path);
            var ret = new List<Model.FileBet>();
            var lines = File.ReadAllLines(path);
            int lineCounter = 0;
            foreach (var line in lines)
            {
                ++lineCounter;
                if (!string.IsNullOrEmpty(line.Trim()))
                {
                    try
                    {
                        _logger.InfoFormat("Parsing bet - line: {0} - {1}", lineCounter, line);
                        var bet = Model.FileBet.Parse(line);
                        ret.Add(bet);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        _updates.Log($"Invalid bet found in file line: {lineCounter} - {line}");
                    }
                }
            }
            return ret;
        }

        protected override void Process(string filePath)
        {
            // a value of -1 for _dupeBetWindowSeconds implies NEVER reprocess a file
            if (_log.ContainsKey(filePath) &&
                (_dupeBetWindowSeconds == -1 || (DateTime.UtcNow - _log[filePath]).TotalSeconds < _dupeBetWindowSeconds))
            {
                return;
            }

            try
            {
                _updates.FileBeingProcessed(filePath);
                _log[filePath] = DateTime.UtcNow;

                var bets = ProcessBetFile(filePath);

                var batch = bets
                    .Where(b => b.Request != null && b.IsValid)
                    .Select(b => b.Request)
                    .ToList();
                var results = _toteGateway.SellBatch(batch).Result;
                _updates.BetResults(filePath, bets, results);
            }
            catch (Exception ex)
            {
                _updates.Log($"Error processing: {ex.Message}");
            }
            finally
            {
                _updates.FileFinishedProcessing(filePath);
            }
        }
    }
}
