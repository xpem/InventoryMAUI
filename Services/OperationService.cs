using LocalRepos.Interface;
using Models.DTO;

namespace Services;

public interface IOperationService
{
    Task InsertOperationAsync(string jsonContent, string objectId, ExecutionType executionType, ObjectType objectType);
}

public class OperationService(IOperationQueueRepo operationQueueRepo) : IOperationService
{
    public async Task InsertOperationAsync(string jsonContent, string objectId, ExecutionType executionType, ObjectType objectType)
    {
        DateTime dateTimeNow = DateTime.Now;

        ApiOperationDTO apiOperation = new()
        {
            CreatedAt = dateTimeNow,
            ObjectType = objectType,
            Status = ApiOperationStatus.Pending,
            UpdatedAt = dateTimeNow,
            Content = jsonContent,
            ObjectId = objectId,
            ExecutionType = executionType
        };

        await operationQueueRepo.InsertOperationInQueueAsync(apiOperation);
    }
}
