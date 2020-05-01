using MHN.Sync.Entity.MongoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.MongoInterface
{
    public interface IProspectMemberRepository
    {
        Task<ProspectMember> GetById(string _id);
        //ProspectMember GetById(string Id);

        Task<List<ProspectMember>> GetAllByField(string fieldName, string fieldValue);

        Task<List<ProspectMember>> GetAll();
        //List<ProspectMember> GetAllProspectMember();

        Task<List<ProspectMember>> GetAll(int currentPage, int pageSize);
        //List<ProspectMember> GetAllProspectMember(int currentPage, int pageSize);

        int GetAllCount();
        string Save(IEntity record);
        string SaveMany(IEnumerable<ProspectMember> records);
        bool? Delete(IEntity entity);

        //bool Drop();
        //bool DropAllData();
        //bool DropDataById(string _id);
    }
}
