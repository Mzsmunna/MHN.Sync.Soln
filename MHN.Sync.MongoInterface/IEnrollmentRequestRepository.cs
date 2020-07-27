using MHN.Sync.Entity.MongoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.MongoInterface
{
    public interface IEnrollmentRequestRepository
    {
        Task<EnrollmentRequest> GetById(string _id);
        Task<List<EnrollmentRequest>> GetAllByField(string fieldName, string fieldValue);
        Task<List<EnrollmentRequest>> GetAll();
        Task<List<EnrollmentRequest>> GetAll(int currentPage, int pageSize);
        int GetAllCount();
        string Save(IEntity record);
        string SaveMany(IEnumerable<EnrollmentRequest> records);
        bool? Delete(IEntity entity);

        //bool Drop();
        //bool DropAllData();
        //bool DropDataById(string _id);
    }
}
