using MHN.Sync.Entity.MongoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.MongoInterface
{
    public interface IMemberRepository
    {
        Task<MemberMeta> GetById(string _id);
        Task<List<MemberMeta>> GetAllByField(string fieldName, string fieldValue);
        Task<List<MemberMeta>> GetAll();
        Task<List<MemberMeta>> GetAll(int currentPage, int pageSize);
        int GetAllCount();
        string Save(IEntity record);
        string SaveMany(IEnumerable<MemberMeta> records);
        bool? Delete(IEntity entity);

        //bool Drop();
        //bool DropAllData();
        //bool DropDataById(string _id);
    }
}
