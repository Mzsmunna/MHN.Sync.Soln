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
    public class ProspectMemberRepository : RepositoryBase, IProspectMemberRepository
    {
        private static IMongoCollection<ProspectMember> collection { get; set; }
        private MongoDBCore<ProspectMember> core;
        static ProspectMemberRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(ProspectMember)))
            {
                BsonClassMap.RegisterClassMap<ProspectMember>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    map.MapProperty(x => x.Id).SetElementName("_id");
                    map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));
                    map.GetMemberMap(x => x.FName).SetSerializer(new StringSerializer(BsonType.String));

                    map.GetMemberMap(x => x.LName).SetSerializer(new EncryptedStringSerializer());
                    map.GetMemberMap(x => x.DOB).SetSerializer(new EncryptedStringSerializer());

                    if (!BsonClassMap.IsClassMapRegistered(typeof(Address)))
                    {
                        BsonClassMap.RegisterClassMap<Address>(child =>
                        {
                            child.AutoMap();
                            child.SetIgnoreExtraElements(true);

                            //child.GetMemberMap(x => x.zip).SetSerializer(new EncryptedStringSerializer());

                            if (!BsonClassMap.IsClassMapRegistered(typeof(ResidentialAddress)))
                            {
                                BsonClassMap.RegisterClassMap<ResidentialAddress>(child1 =>
                                {
                                    child1.AutoMap();
                                    child1.SetIgnoreExtraElements(true);

                                    child1.GetMemberMap(x => x.Zip).SetSerializer(new EncryptedStringSerializer());
                                });
                            }

                            if (!BsonClassMap.IsClassMapRegistered(typeof(MailingAddress)))
                            {
                                BsonClassMap.RegisterClassMap<MailingAddress>(child1 =>
                                {
                                    child1.AutoMap();
                                    child1.SetIgnoreExtraElements(true);

                                });
                            }

                        });
                    }

                });
            }
        }

        public ProspectMemberRepository(IDatabaseContext dbContext)
            : base(dbContext)
        {
            try
            {
                if (collection == null)
                {
                    collection = _database.GetCollection<ProspectMember>("ProspectMember");
                }
                core = new MongoDBCore<ProspectMember>(collection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private FilterDefinition<ProspectMember> BuildFilter(string _id)
        {
            var filter = Builders<ProspectMember>.Filter.Empty;

            if (!string.IsNullOrEmpty(_id) && _id.ToLower() != "undefined")
            {
                filter = filter & Builders<ProspectMember>.Filter.Eq("_id", _id);
            }

            return filter;
        }

        public async Task<ProspectMember> GetById(string _id)
        {
            var filter = BuildFilter(_id);
            return await collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);

        }

        public async Task<List<ProspectMember>> GetAllByField(string fieldName, string fieldValue)
        {
            var filter = Builders<ProspectMember>.Filter.Eq(fieldName, fieldValue);
            var result = await collection.Find(filter).ToListAsync().ConfigureAwait(false);
            //var result = await Collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<List<ProspectMember>> GetAll()
        {
            var filter = BuildFilter(null);
            return await collection.Find(filter).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<ProspectMember>> GetAll(int currentPage, int pageSize)
        {
            var filter = BuildFilter(null);
            return await collection.Find(filter).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync().ConfigureAwait(false);
        }

        public string Save(IEntity entity)
        {
            var returnVal = string.Empty;

            try
            {
                MongoDbOperationResult result = new MongoDBHelper<ProspectMember>(collection).Save(entity).Result;
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
                _database.DropCollection("ProspectMember");
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
                //var collection = _database.GetCollection<ProspectMember>("ProspectMember");

                var filter = Builders<ProspectMember>.Filter.Empty;

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

        //public ProspectMember GetById(string Id)
        //{
        //    MongoParam param = new MongoParam();
        //    param.AddParameter("_id", Id, true);
        //    return core.Get(param).Result;
        //}

        //public List<ProspectMember> GetAllProspectMember()
        //{
        //    List<ProspectMember> returnVal = new List<ProspectMember>();
        //    returnVal = collection.Find(_ => true).ToList();
        //    return returnVal;
        //}

        //public List<ProspectMember> GetAllProspectMember(int currentPage, int pageSize)
        //{
        //    List<ProspectMember> returnVal = new List<ProspectMember>();
        //    var filter = Builders<ProspectMember>.Filter.Empty;
        //    returnVal = collection.Find(filter).Skip(currentPage * pageSize).Limit(pageSize).ToList();
        //    return returnVal;
        //}

        public string SaveMany(IEnumerable<ProspectMember> records)
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
                ProspectMember _actualEntity = entity as ProspectMember;
                //_actualEntity.IsDeleted = true;
                //_actualEntity.ModifiedOn = DateTime.Now;

                MongoDbOperationResult result = new MongoDBHelper<ProspectMember>(collection).Save(_actualEntity).Result;

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
