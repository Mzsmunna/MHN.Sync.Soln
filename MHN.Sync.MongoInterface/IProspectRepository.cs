using MHN.Sync.Entity.MongoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.MongoInterface
{
    public interface IProspectRepository
    {
        Task<ProspectMeta> GetById(string _id);
        Task<List<ProspectMeta>> GetAllByField(string fieldName, string fieldValue);
        Task<List<ProspectMeta>> GetAll();
        Task<List<ProspectMeta>> GetAll(int currentPage, int pageSize);
        int GetAllCount();
        string Save(IEntity record);
        string SaveMany(IEnumerable<ProspectMeta> records);
        bool? Delete(IEntity entity);

        //bool Drop();
        //bool DropAllData();
        //bool DropDataById(string _id);
    }
}
