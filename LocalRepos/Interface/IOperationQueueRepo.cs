using Models.DTO;

namespace LocalRepos.Interface;

public interface IOperationQueueRepo
{
    Task<bool> CheckIfHasPendingOperation();
    Task<bool> CheckIfHasPendingOperationWithObjectId(string objectId);
    Task<List<ApiOperationDTO>> GetPendingOperationsByStatusAsync(ApiOperationStatus operationStatus);
    Task InsertOperationInQueueAsync(ApiOperationDTO apiOperation);
    Task UpdateOperationStatusAsync(ApiOperationStatus operationStatus, int operationId);
}
