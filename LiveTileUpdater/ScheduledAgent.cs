using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using TotallySwankWP.DataServices;
using Microsoft.Phone.Shell;
using System.Linq;

namespace LiveTileUpdater
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override async void OnInvoke(ScheduledTask task)
        {
            var activeTiles = ShellTile.ActiveTiles;
            var activeTile = activeTiles.FirstOrDefault();
            //no network or no live tile, we shouldnt even bother
            if (Microsoft.Phone.Net.NetworkInformation.DeviceNetworkInformation.IsNetworkAvailable && activeTile != null)
            {
                var wotdService = new WOTDDataService();
                var entries = await wotdService.GetEntries();
                var entry = entries.FirstOrDefault();
                if(entry != null)
                {
                    var tileData = new IconicTileData
                    {
                        WideContent1 = entry.Name,
                        WideContent2 = entry.Definition,
                        WideContent3 = entry.Example,
                        Title = "Totally Swank WOTD"
                    };
                    activeTile.Update(tileData);
                }
            }
            NotifyComplete();
        }
    }
}