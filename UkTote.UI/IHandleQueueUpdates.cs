using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UkTote.UI
{
    public interface IHandleQueueUpdates
    {
        void Log(string message);
        void FileBeingProcessed(string path);
        void BetResults(string path, List<Model.FileBet> bets, IList<BetReply> results);
        void FileFinishedProcessing(string path);
    }
}
