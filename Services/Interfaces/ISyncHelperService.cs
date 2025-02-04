using Models.DTO;
using Models.Resps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISyncHelperService
    {
        Task<DTOBase?> GetByIdAsync(int id);

        Task<int> CreateLocalAsync(DTOBase entity);

        Task<int> CreateApiAsync(DTOBase entity);

        Task<int> UpdateLocalAsync(DTOBase entity);

        Task<List<DTOBase>?> GetByLastUpdateAsync(DateTime lastUpdate, int page);

        SubCategoryDTO DeserializeObj(string content);

        Task<SubCategoryDTO?> GetByLocalIdAsync(int uid, int localId);

        Task<ServResp> UpdateApiAsync(DTOBase entity);

        Task ApiToLocalAsync(int id, DateTime lastUpdate);

        Task LocalToApiAsync();
    }
}
