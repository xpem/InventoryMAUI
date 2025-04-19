using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalRepos.Interface;

public interface IOperationQueueRepo
{
    Task<bool> CheckIfHasPendingOperation();
    Task<bool> CheckIfHasPendingOperationWithObjectId(string objectId);
    Task<List<ApiOperationDTO>> GetPendingOperationsByStatusAsync(ApiOperationStatus operationStatus);
    Task InsertOperationInQueueAsync(ApiOperationDTO apiOperation);
    Task UpdateOperationStatusAsync(ApiOperationStatus operationStatus, int operationId);
}
