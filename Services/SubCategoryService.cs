using ApiRepos.Interfaces;
using LocalRepos.Interface;
using Models.DTO;
using Models.Resps;
using Services.Handlers;
using Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Services
{
    public class SubCategoryService(ISubCategoryApiRepo subCategoryApiRepo, ISubCategoryRepo subCategoryRepo, IOperationService operationService, IOperationQueueRepo operationQueueRepo) : ISubCategoryService
    {
        public async Task<ServResp> GetSubCategoriesByCategoryId(int categoryId)
        {
            ApiResp resp = await subCategoryApiRepo.GetSubCategoriesByCategoryId(categoryId.ToString());

            return ApiRespHandler.Handler<List<SubCategoryDTO>>(resp);
        }

        public async Task<List<SubCategoryDTO>> GetByCategoryIdAsync(int uid, int page, int categoryId)
        {
            return await subCategoryRepo.GetByCategoryIdAsync(uid, page, categoryId);
        }

        public async Task<ServResp> CreateApiAsync(SubCategoryDTO subCategory)
        {
            ApiResp? resp = await subCategoryApiRepo.CreateAsync(subCategory);

            if (resp is not null && resp.Success && resp.Content is not null and string)
            {
                JsonNode? jResp = JsonNode.Parse((string)resp.Content);
                if (jResp is not null)
                {
                    SubCategoryDTO subCategoryResp = new()
                    {
                        Id = jResp["Id"]?.GetValue<int>() ?? 0,
                        Name = jResp["Name"]?.GetValue<string>(),
                        IconName = jResp["IconName"]?.GetValue<string>(),
                        SystemDefault = jResp["SystemDefault"]?.GetValue<bool>()
                    };

                    return new ServResp() { Success = resp.Success, Content = subCategoryResp };
                }
                else return new ServResp() { Success = false, Content = resp.Content };
            }

            return new ServResp() { Success = false, Content = null };
        }

        public async Task<ServResp> CreateAsync(int uid, bool isON, SubCategoryDTO subCategoryDTO)
        {
            subCategoryDTO.CreatedAt = DateTime.Now;
            subCategoryDTO.UserId = uid;

            bool isValid = !await subCategoryRepo.CheckIfExistsByCategoryIdAndName(uid, subCategoryDTO.CategoryId, subCategoryDTO.Name, null);

            if (isValid)
            {
                await subCategoryRepo.CreateAsync(subCategoryDTO);

                if (isON)
                {
                    ServResp resp = await CreateApiAsync(subCategoryDTO);

                    if (resp is not null && resp.Success && resp.Content is not null)
                    {
                        subCategoryDTO.Id = (resp.Content as SubCategoryDTO).Id;
                        await subCategoryRepo.UpdateAsync(subCategoryDTO);
                    }
                    else
                    {
                        return resp?.Content is not null and string
                            ? new ServResp() { Success = false, Content = resp.Content }
                            : new ServResp() { Success = false, Content = null };
                    }
                }
                else
                {
                    await operationService.InsertOperationAsync(JsonSerializer.Serialize(subCategoryDTO), subCategoryDTO.LocalId.ToString(), ExecutionType.Insert, ObjectType.SubCategory);
                }

                return new ServResp() { Success = true, Content = subCategoryDTO };
            }
            else return new ServResp() { Success = false, Content = "SubCategoria já existe" };

        }

        public async Task<ServResp> UpdateApiAsync(DTOBase subCategory)
        {
            ApiResp? resp = await subCategoryApiRepo.UpdateApiAsync(subCategory as SubCategoryDTO);

            if (resp is not null && resp.Content is not null and string)
            {
                if (resp.Success)
                {
                    JsonNode? jResp = JsonNode.Parse(resp.Content as string);
                    if (jResp is not null)
                    {
                        SubCategoryDTO subCategoryResp = new()
                        {
                            Id = jResp["Id"]?.GetValue<int>() ?? 0,
                            Name = jResp["Name"]?.GetValue<string>(),
                            IconName = jResp["IconName"]?.GetValue<string>(),
                            SystemDefault = jResp["SystemDefault"]?.GetValue<bool>()
                        };

                        return new ServResp() { Success = resp.Success, Content = subCategoryResp };
                    }
                }
                else return new ServResp() { Success = false, Content = resp.Content };
            }

            return new ServResp() { Success = false, Content = null };
        }

        public async Task<ServResp> UpdateAsync(int uid, bool isOn, SubCategoryDTO subCategoryDTO)
        {
            bool isValid = !await subCategoryRepo.CheckIfExistsByCategoryIdAndName(uid, subCategoryDTO.CategoryId, subCategoryDTO.Name, subCategoryDTO.LocalId);

            if (isValid)
            {
                subCategoryDTO.UpdatedAt = DateTime.Now;
                subCategoryDTO.UserId = uid;

                await subCategoryRepo.UpdateAsync(subCategoryDTO);

                if (isOn)
                {
                    ApiResp resp = await subCategoryApiRepo.UpdateApiAsync(subCategoryDTO);
                    if (!resp.Success) throw new Exception($"Could not be possible update obj, id: {subCategoryDTO.Id}, Erro: {resp.Content?.ToString()} ");
                }
                else await operationService.InsertOperationAsync(JsonSerializer.Serialize(subCategoryDTO), subCategoryDTO.LocalId.ToString() ?? throw new ArgumentNullException(), ExecutionType.Update, ObjectType.SubCategory);


                return new ServResp() { Success = true, Content = subCategoryDTO };
            }
            else return new ServResp() { Success = false, Content = "SubCategoria já existe" };
        }

        public async Task<ServResp> DelSubCategory(int id)
        {
            ApiResp? resp = await subCategoryApiRepo.DelSubCategory(id);

            return resp is not null && resp.Content is not null
                ? new ServResp() { Success = resp.Success, Content = resp.Content }
                : new ServResp() { Success = false, Content = null };
        }

        public async Task LocalToApiAsync() => await SyncHelperService.LocalToApiAsync(this, operationQueueRepo);

        public async Task ApiToLocalAsync(int uid, DateTime lastUpdate) => await SyncHelperService.ApiToLocalAsync(this, uid, lastUpdate);

        public SubCategoryDTO DeserializeObj(string content)
        {
            SubCategoryDTO? subCategory = JsonSerializer.Deserialize<SubCategoryDTO>(content);

            return subCategory is null ? throw new ArgumentNullException(nameof(subCategory)) : subCategory;
        }

        public async Task<int> CreateApiAsync(DTOBase modelBase)
        {
            ServResp? apiResp = await CreateApiAsync(modelBase as SubCategoryDTO);

            if (apiResp is not null)
            {
                SubCategoryDTO subCategoryApiResp = apiResp.Content as SubCategoryDTO;

                return subCategoryApiResp.Id.Value;
            }
            else throw new Exception("Error creating SubCategory");
        }

        public Task<int> CreateLocalAsync(DTOBase entity) => subCategoryRepo.CreateAsync(entity as SubCategoryDTO);

        public Task<int> UpdateLocalAsync(DTOBase entity) => subCategoryRepo.UpdateAsync(entity as SubCategoryDTO);

        public async Task<List<DTOBase>?> GetByLastUpdateAsync(DateTime lastUpdate, int page)
        {
            ApiResp apiResp = await subCategoryApiRepo.GetByLastUpdateAsync(lastUpdate, page);

            ServResp? resp = ApiRespHandler.Handler<List<SubCategoryDTO>>(apiResp);

            if (resp is not null && resp.Success && resp.Content is not null)
            {
                List<SubCategoryDTO>? listResp = resp.Content as List<Models.DTO.SubCategoryDTO>;

                return listResp?.Select(x => x as DTOBase).ToList();
            }
            else
            {
                if (resp?.Content is not null and string)
                    throw new Exception($"Error getting SubCategories from API: {resp.Content}");
                else
                    throw new Exception("Error getting SubCategories from API");
            }
        }

        public async Task<SubCategoryDTO?> GetByLocalIdAsync(int uid, int localId)
        {
            return await subCategoryRepo.GetByLocalIdAsync(uid, localId);
        }

        public async Task<DTOBase?> GetByIdAsync(int id)
        {
            return (await subCategoryRepo.GetByIdAsync(id)) as DTOBase;
        }
    }
}
