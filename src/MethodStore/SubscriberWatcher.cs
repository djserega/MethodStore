using System.IO;
using System.Threading;

namespace MethodStore
{
    internal class SubscriberWatcher
    {
        private CallUpdateListObjectMethodsEvents _callUpdate;

        internal SubscriberWatcher(CallUpdateListObjectMethodsEvents callUpdate)
        {
            _callUpdate = callUpdate;

            FileSystemWatcher watcher = new FileSystemWatcher(new DirFile().PathDataFiles)
            {
                EnableRaisingEvents = true
            };
            watcher.Changed += Watcher_Changed;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(1 * 1000);

            _callUpdate.EvokeUpdateList();
        }
    }
}
