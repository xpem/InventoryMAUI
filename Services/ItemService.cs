using ApiRepos.Interfaces;
using Models;
using Models.Item;
using Models.Item.Files;
using Models.Resps;
using Services.Handlers;
using Services.Handlers.Exceptions;
using Services.Interfaces;
using System.Text.Json.Nodes;

namespace Services
{
    public class ItemService(IItemApiRepo itemApiRepo) : IItemService
    {
        public async Task<List<Item>> GetItemsAllAsync()
        {
            ApiResp totalsResp = await itemApiRepo.GetTotalItensAsync();
            List<Item> items = [];

            var itemTotalsBLLResponse = ApiRespHandler.Handler<ItemTotals>(totalsResp);

            if (itemTotalsBLLResponse.Success)
            {
                ItemTotals? itemTotals = itemTotalsBLLResponse.Content as ItemTotals;

                for (int i = 1; i <= itemTotals?.TotalPages; i++)
                {
                    ApiResp resp = await itemApiRepo.GetPaginatedItemsAsync(i);
                    var paginatedItemsBLLResponse = ApiRespHandler.Handler<List<Item>>(resp);

                    if (paginatedItemsBLLResponse.Success)
                        if (paginatedItemsBLLResponse.Content is List<Item> pageItems)
                            items.AddRange(pageItems);
                }

                return items;
            }
            else
            {
                throw new ServerOffException("totalsResp success false, error:" + itemTotalsBLLResponse.Error);
            }

        }

        public async Task<ServResp> GetItemByIdAsync(string id)
        {
            ApiResp resp = await itemApiRepo.GetItemByIdAsync(id);
            return ApiRespHandler.Handler<Item>(resp);
        }

        public async Task<ServResp> AddItemAsync(Item item)
        {
            ApiResp? resp = await itemApiRepo.AddItemAsync(item);

            if (resp is not null && resp.Success && resp.Content is not null and string)
            {
                return ApiRespHandler.Handler<Item>(resp);
            }

            return new ServResp() { Success = false, Content = null };
        }

        public async Task<ServResp> AltItemAsync(Item item)
        {
            ApiResp? resp = await itemApiRepo.AltItemAsync(item);

            if (resp is not null && resp.Success && resp.Content is not null and string)
            {
                JsonNode? jResp = JsonNode.Parse(resp.Content as string);

                if (jResp is not null)
                    return new ServResp() { Success = resp.Success, Content = null };
                else return new ServResp() { Success = false, Content = resp.Content };
            }

            return new ServResp() { Success = false, Content = null };
        }

        public async Task<ServResp> DelItemAsync(int id)
        {
            ApiResp? resp = await itemApiRepo.DelItemAsync(id);

            if (resp is not null && !resp.Success && !string.IsNullOrEmpty(resp.Content as string))
                return new ServResp() { Success = false, Content = resp.Content.ToString() };

            return new ServResp() { Success = true, Content = null };
        }

        public async Task<ServResp> DelItemImageAsync(int id, string filename)
        {
            ApiResp resp = await itemApiRepo.DelItemImageAsync(id, filename);

            if (resp is not null && !resp.Success && !string.IsNullOrEmpty(resp.Content as string))
            {
                return new ServResp() { Success = false, Content = resp.Content.ToString() };
            }

            //BLLResponse itemResp = ApiResponseHandler.Handler<Item>(resp);
            return new ServResp() { Success = true, Content = null };
        }

        public async Task<ItemFilesToUpload> GetItemImages(int itemId, string itemImage1, string itemImage2)
        {
            ItemFilesToUpload itemFilesToUpload = new();

            if (itemImage1 != null)
            {
                var resItemImage = await GetImageItemAsync(itemId, 1, itemImage1, FilePaths.ImagesPath);

                if (resItemImage is not null)
                    itemFilesToUpload.Image1 = resItemImage;
            }

            if (itemImage2 != null)
            {
                var resItemImage = await GetImageItemAsync(itemId, 2, itemImage2, FilePaths.ImagesPath);

                if (resItemImage is not null)
                    itemFilesToUpload.Image2 = resItemImage;
            }

            return itemFilesToUpload;
        }

        public async Task<ServResp> AddItemImageAsync(int id, ItemFilesToUpload itemFilesToUpload)
        {
            ApiResp resp = await itemApiRepo.AddItemImage(id, itemFilesToUpload);

            if (resp != null && resp.Content is not null)
            {
                var respBllResp = ApiRespHandler.Handler<ItemFileNames>(resp);

                if (respBllResp is not null && respBllResp.Success)
                {
                    var itemFileNames = respBllResp.Content as ItemFileNames;
                    if (itemFileNames is not null)
                    {
                        if (itemFileNames.Image1 is not null)
                        {
                            var newPath = Path.Combine(FilePaths.ImagesPath, itemFileNames.Image1);
                            System.IO.File.Move(itemFilesToUpload.Image1.ImageFilePath, newPath);

                            itemFilesToUpload.Image1.ImageFilePath = Path.Combine(FilePaths.ImagesPath, itemFileNames.Image1);
                        }

                        if (itemFileNames.Image2 is not null)
                        {
                            var newPath = Path.Combine(FilePaths.ImagesPath, itemFileNames.Image2);
                            System.IO.File.Move(itemFilesToUpload.Image2.ImageFilePath, newPath);

                            itemFilesToUpload.Image2.ImageFilePath = Path.Combine(FilePaths.ImagesPath, itemFileNames.Image2);
                        }

                        return new ServResp() { Success = true };
                    }
                }
            }

            return new ServResp() { Success = false };

        }

        private async Task<ImageFile?> GetImageItemAsync(int id, int idx, string fileName, string filePath)
        {
            bool exists = System.IO.Directory.Exists(filePath);

            if (!exists)
                System.IO.Directory.CreateDirectory(filePath);

            string filePathAndName = Path.Combine(filePath, fileName);
            ImageFile imageFile;

            if (File.Exists(filePathAndName))
            {
                using var fs = new FileStream(filePathAndName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                using MemoryStream memoryStream = new();
                fs.CopyTo(memoryStream);
                imageFile = new() { FileName = fileName, FileId = idx, ImageFilePath = filePathAndName };

                return imageFile;
            }

            ApiResp resp = await itemApiRepo.GetItemImageAsync(id, fileName);

            if (resp is not null && resp.Content is not null and Stream)
            {
                using var fs = new FileStream(filePathAndName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                ((Stream)resp.Content).CopyTo(fs);

                imageFile = new() { FileName = fs.Name, FileId = idx, ImageFilePath = filePathAndName };

                await ((Stream)resp.Content).DisposeAsync();

                return imageFile;
            }

            return null;
        }

    }
}
