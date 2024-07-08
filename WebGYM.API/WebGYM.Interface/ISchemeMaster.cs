using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGYM.Models;

namespace WebGYM.Interface
{
    public interface ISchemeMaster
    {
        Task<bool> AddSchemeMaster(SchemeMaster schemeMaster);
        Task<List<SchemeMaster>> GetSchemeMasterList();
        Task<SchemeMaster> GetSchemeMasterbyId(int schemeId);
        Task<bool> CheckSchemeNameExists(string schemeName);
        Task<bool> UpdateSchemeMaster(SchemeMaster schemeMaster);
        Task<bool> DeleteScheme(int schemeId);
        Task<List<SchemeMaster>> GetActiveSchemeMasterList();
    }
}
