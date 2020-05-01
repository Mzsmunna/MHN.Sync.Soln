using MHN.Sync.Entity.MongoEntity;
using MHN.Sync.MongoInterface;
using MHN.Sync.MongoInterface.BASE;
using MHN.Sync.MongoRepository.DAL;
using MHN.Sync.MongoRepository.Serializar;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.MongoRepository
{
    public class MemberRepository : RepositoryBase, IMemberRepository
    {
        private static IMongoCollection<MemberMeta> collection { get; set; }
        private MongoDBCore<MemberMeta> core;
        static MemberRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(MemberMeta)))
            {
                BsonClassMap.RegisterClassMap<MemberMeta>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    map.MapProperty(x => x.Id).SetElementName("_id");
                    map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));

                });
            }
        }

        public MemberRepository(IDatabaseContext dbContext)
            : base(dbContext)
        {
            try
            {
                if (collection == null)
                {
                    collection = _database.GetCollection<MemberMeta>("MemberMeta");
                }
                core = new MongoDBCore<MemberMeta>(collection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private FilterDefinition<MemberMeta> BuildFilter(string _id)
        {
            var filter = Builders<MemberMeta>.Filter.Empty;

            if (!string.IsNullOrEmpty(_id) && _id.ToLower() != "undefined")
            {
                filter = filter & Builders<MemberMeta>.Filter.Eq("_id", _id);
            }

            return filter;
        }

        public async Task<MemberMeta> GetById(string _id)
        {
            var filter = BuildFilter(_id);
            return await collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);

        }

        public async Task<List<MemberMeta>> GetAllByField(string fieldName, string fieldValue)
        {
            var filter = Builders<MemberMeta>.Filter.Eq(fieldName, fieldValue);
            var result = await collection.Find(filter).ToListAsync().ConfigureAwait(false);
            //var result = await Collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<List<MemberMeta>> GetAll()
        {
            var filter = BuildFilter(null);
            return await collection.Find(filter).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<MemberMeta>> GetAll(int currentPage, int pageSize)
        {
            var filter = BuildFilter(null);
            return await collection.Find(filter).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync().ConfigureAwait(false);
        }

        public string Save(IEntity entity)
        {
            var returnVal = string.Empty;

            try
            {
                MongoDbOperationResult result = new MongoDBHelper<MemberMeta>(collection).Save(entity).Result;
                returnVal = result.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnVal;
        }

        public bool Drop()
        {
            try
            {
                _database.DropCollection("Member");
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DropAllData()
        {
            try
            {
                //var collection = _database.GetCollection<Member>("Member");

                var filter = Builders<MemberMeta>.Filter.Empty;

                collection.DeleteMany(filter);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DropDataById(string _id)
        {
            try
            {
                var filter = BuildFilter(_id);

                collection.DeleteMany(filter);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string SaveMany(IEnumerable<MemberMeta> records)
        {
            var returnVal = string.Empty;

            try
            {
                collection.InsertMany(records);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnVal;
        }

        public bool? Delete(IEntity entity)
        {
            bool? returnVal = null;

            try
            {
                MemberMeta _actualEntity = entity as MemberMeta;
                //_actualEntity.IsDeleted = true;
                //_actualEntity.ModifiedOn = DateTime.Now;

                MongoDbOperationResult result = new MongoDBHelper<MemberMeta>(collection).Save(_actualEntity).Result;

                returnVal = string.IsNullOrEmpty(result.Id) ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Do Nothing
            }

            return returnVal;
        }

        public int GetAllCount()
        {
            int count = 0;
            try
            {
                var filter = BuildFilter(null);
                count = Convert.ToInt32(collection.Find(filter).Count());
            }
            catch (Exception ex)
            {
                //new ExceptionWrapper(ex).Handle();
            }
            return count;
        }
    }
}
