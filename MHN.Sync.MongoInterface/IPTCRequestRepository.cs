using MHN.Sync.Entity.MongoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.MongoInterface
{
    public interface IPTCRequestRepository
    {
        Task<PTCRequest> GetById(string _id);
        Task<List<PTCRequest>> GetAllByField(string fieldName, string fieldValue);
        Task<List<PTCRequest>> GetAll();
        Task<List<PTCRequest>> GetAll(int currentPage, int pageSize);
        int GetAllCount();
        string Save(IEntity record);
        string SaveMany(IEnumerable<PTCRequest> records);
        bool? Delete(IEntity entity);

        //bool Drop();
        //bool DropAllData();
        //bool DropDataById(string _id);
    }
}
