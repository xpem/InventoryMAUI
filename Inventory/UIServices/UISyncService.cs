using Models;
using Models.DTO;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;

namespace Inventory.UIServices
{
    public class UISyncService(IUserService userService, ISubCategoryService subCategoryService) : ISyncService
    {
        public static SyncStatus Synchronizing { get; set; }

        public bool ThreadIsRunning { get; set; } = false;

        public Timer Timer { get; set; }

        //40 secs
        readonly int Interval = 30000;

        public void StartThread()
        {
            if (!ThreadIsRunning)
            {
                Synchronizing = SyncStatus.Sleeping;

                Thread thread = new(SetTimer) { IsBackground = true };
                thread.Start();
            }
        }

        private void SetTimer()
        {
            if (!ThreadIsRunning)
            {
                ThreadIsRunning = true;
                SyncLocalDb(null);

                Timer = new Timer(SyncLocalDb, null, Interval, Timeout.Infinite);
            }
        }

        public async void SyncLocalDb(object state) => await ExecSyncAsync();

        public async Task ExecSyncAsync()
        {
            try
            {
                UserDTO user = userService.GetAsync().Result;

                if (user != null && Synchronizing != SyncStatus.Processing)
                {
                    Synchronizing = SyncStatus.Processing;

                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        await subCategoryService.ApiToLocalAsync(user.Id, user.LastUpdate);

                        await subCategoryService.LocalToApiAsync();

                        //await booksSyncBLL.LocalToApiSync(user.Id, user.LastUpdate);

                        //await booksSyncBLL.ApiToLocalSync(user.Id, user.LastUpdate);

                        //await bookHistoricSyncBLL.ApiToLocalSync(user.Id, user.LastUpdate);

                        userService.UpdateLastUpdate(user.Id);
                    }

                    Synchronizing = SyncStatus.Sleeping;
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("No connection could be made because the target machine actively refused it."))
                    Synchronizing = SyncStatus.ServerOff;
                else throw ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                Synchronizing = SyncStatus.Unauthorized;
            }
            catch
            {
                throw;
            }
            finally
            {
                Timer?.Change(Interval, Timeout.Infinite);

                if (Synchronizing != SyncStatus.Unauthorized)
                    Synchronizing = SyncStatus.Sleeping;
            }
        }
    }
}
