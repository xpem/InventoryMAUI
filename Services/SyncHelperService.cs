using LocalRepos.Interface;
using Models.DTO;
using Models.Resps;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class SyncHelperService
    {
        private const int PAGEMAX = 50;

        public static async Task ApiToLocalAsync<T>(T requester, int uid, DateTime lastUpdate) where T : ISyncHelperService
        {
            int page = 1;

            try
            {
                while (true)
                {
                    var apiRespList = await requester.GetByLastUpdateAsync(lastUpdate, page);

                    if (apiRespList is null) break;

                    foreach (var apiRespObj in apiRespList)
                    {
                        if (apiRespObj is null) throw new ArgumentNullException(nameof(apiRespObj));

                        apiRespObj.UserId = uid;

                        DTOBase? localObj = null;

                        if (apiRespObj.Id is not null)
                        {
                            localObj = await requester.GetByIdAsync(apiRespObj.Id.Value);

                            apiRespObj.LocalId = localObj?.LocalId ?? 0;
                        }
                        else
                            throw new ArgumentNullException(nameof(apiRespObj.Id));

                        if (localObj == null && !apiRespObj.Inactive)
                            await requester.CreateLocalAsync(apiRespObj);
                        else if (apiRespObj.UpdatedAt > localObj?.UpdatedAt)
                            await requester.UpdateLocalAsync(apiRespObj);
                    }

                    if (apiRespList.Count < PAGEMAX)
                        break;

                    page++;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task LocalToApiAsync<T>(T requester, IOperationQueueRepo operationQueueRepo) where T : ISyncHelperService
        {
            List<ApiOperationDTO> pendingOperations = await operationQueueRepo.GetPendingOperationsByStatusAsync(ApiOperationStatus.Pending);

            foreach (var pendingOperation in pendingOperations)
            {
                await operationQueueRepo.UpdateOperationStatusAsync(ApiOperationStatus.Processing, pendingOperation.Id);

                DTOBase? localObj = requester.DeserializeObj(pendingOperation.Content);
                ServResp servResp;
                switch (pendingOperation.ExecutionType)
                {
                    case ExecutionType.Insert:

                        int objId = await requester.CreateApiAsync(localObj);

                        localObj.Id = Convert.ToInt32(objId);
                        await requester.UpdateLocalAsync(localObj);

                        break;
                    case ExecutionType.Update:

                        if (localObj.Id is null)
                        {
                            DTOBase? insertedObj = await requester.GetByLocalIdAsync(localObj.UserId, localObj.LocalId.Value);

                            if (insertedObj is not null)
                                servResp = await requester.UpdateApiAsync(insertedObj);
                            else throw new NullReferenceException("Livro inserido não encontrado " + localObj.LocalId);
                        }
                        else
                            servResp = await requester.UpdateApiAsync(localObj);

                        if (!servResp.Success) throw new Exception($"Não foi possivel sincronizar o livro {pendingOperation.ObjectId}, res: {servResp.Error}");

                        break;
                }

                await operationQueueRepo.UpdateOperationStatusAsync(ApiOperationStatus.Success, pendingOperation.Id);
            }
        }
    }

}
