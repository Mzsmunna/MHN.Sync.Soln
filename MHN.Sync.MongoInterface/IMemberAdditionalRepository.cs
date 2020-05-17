using MHN.Sync.Entity.MongoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.MongoInterface
{
    public interface IMemberAdditionalRepository
    {
        Task<MemberAdditional> GetById(string _id);
        Task<List<MemberAdditional>> GetAllByField(string fieldName, string fieldValue);
        Task<List<MemberAdditional>> GetAll();
        Task<List<MemberAdditional>> GetAll(int currentPage, int pageSize);
        int GetAllCount();
        string Save(IEntity record);
        string SaveMany(IEnumerable<MemberAdditional> records);
        bool? Delete(IEntity entity);

        //bool Drop();
        //bool DropAllData();
        //bool DropDataById(string _id);
    }
}
