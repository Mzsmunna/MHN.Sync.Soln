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
    public class MemberAdditionalRepository : RepositoryBase, IMemberAdditionalRepository
    {
        private static IMongoCollection<MemberAdditional> collection { get; set; }
        private MongoDBCore<MemberAdditional> core;
        static MemberAdditionalRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(MemberAdditional)))
            {
                BsonClassMap.RegisterClassMap<MemberAdditional>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    map.MapProperty(x => x.Id).SetElementName("_id");
                    map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));

                    if (!BsonClassMap.IsClassMapRegistered(typeof(Supplementary)))
                    {
                        BsonClassMap.RegisterClassMap<Supplementary>(child =>
                        {
                            child.AutoMap();
                            child.SetIgnoreExtraElements(true);
                        });
                    }

                    if (!BsonClassMap.IsClassMapRegistered(typeof(CareManagement)))
                    {
                        BsonClassMap.RegisterClassMap<CareManagement>(child =>
                        {
                            child.AutoMap();
                            child.SetIgnoreExtraElements(true);
                        });
                    }

                    if (!BsonClassMap.IsClassMapRegistered(typeof(ContactRelated)))
                    {
                        BsonClassMap.RegisterClassMap<ContactRelated>(child =>
                        {
                            child.AutoMap();
                            child.SetIgnoreExtraElements(true);

                            //child.GetMemberMap(x => x.zip).SetSerializer(new EncryptedStringSerializer());

                            if (!BsonClassMap.IsClassMapRegistered(typeof(Emergency)))
                            {
                                BsonClassMap.RegisterClassMap<Emergency>(child1 =>
                                {
                                    child1.AutoMap();
                                    child1.SetIgnoreExtraElements(true);
                                });
                            }
                        });
                    }

                    if (!BsonClassMap.IsClassMapRegistered(typeof(RiskActivity)))
                    {
                        BsonClassMap.RegisterClassMap<RiskActivity>(child =>
                        {
                            child.AutoMap();
                            child.SetIgnoreExtraElements(true);
                        });
                    }

                    if (!BsonClassMap.IsClassMapRegistered(typeof(InteractionHistory)))
                    {
                        BsonClassMap.RegisterClassMap<InteractionHistory>(child =>
                        {
                            child.AutoMap();
                            child.SetIgnoreExtraElements(true);
                        });
                    }

                    if (!BsonClassMap.IsClassMapRegistered(typeof(BenefitsActivity)))
                    {
                        BsonClassMap.RegisterClassMap<BenefitsActivity>(child =>
                        {
                            child.AutoMap();
                            child.SetIgnoreExtraElements(true);

                            if (!BsonClassMap.IsClassMapRegistered(typeof(OTC)))
                            {
                                BsonClassMap.RegisterClassMap<OTC>(child1 =>
                                {
                                    child1.AutoMap();
                                    child1.SetIgnoreExtraElements(true);
                                });
                            }
                        });
                    }

                    if (!BsonClassMap.IsClassMapRegistered(typeof(Medication)))
                    {
                        BsonClassMap.RegisterClassMap<Medication>(child =>
                        {
                            child.AutoMap();
                            child.SetIgnoreExtraElements(true);
                        });
                    }

                    if (!BsonClassMap.IsClassMapRegistered(typeof(AddressChanged)))
                    {
                        BsonClassMap.RegisterClassMap<AddressChanged>(child =>
                        {
                            child.AutoMap();
                            child.SetIgnoreExtraElements(true);

                            child.GetMemberMap(x => x.ZipCode).SetSerializer(new EncryptedStringSerializer());
                        });
                    }

                });
            }
        }

        public MemberAdditionalRepository(IDatabaseContext dbContext)
            : base(dbContext)
        {
            try
            {
                if (collection == null)
                {
                    collection = _database.GetCollection<MemberAdditional>("MemberAdditional");
                }
                core = new MongoDBCore<MemberAdditional>(collection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private FilterDefinition<MemberAdditional> BuildFilter(string _id)
        {
            var filter = Builders<MemberAdditional>.Filter.Empty;

            if (!string.IsNullOrEmpty(_id) && _id.ToLower() != "undefined")
            {
                filter = filter & Builders<MemberAdditional>.Filter.Eq("_id", _id);
            }

            return filter;
        }

        public async Task<MemberAdditional> GetById(string _id)
        {
            var filter = BuildFilter(_id);
            return await collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);

        }

        public async Task<List<MemberAdditional>> GetAllByField(string fieldName, string fieldValue)
        {
            var filter = Builders<MemberAdditional>.Filter.Eq(fieldName, fieldValue);
            var result = await collection.Find(filter).ToListAsync().ConfigureAwait(false);
            //var result = await Collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<List<MemberAdditional>> GetAll()
        {
            var filter = BuildFilter(null);
            return await collection.Find(filter).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<MemberAdditional>> GetAll(int currentPage, int pageSize)
        {
            var filter = BuildFilter(null);
            return await collection.Find(filter).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync().ConfigureAwait(false);
        }

        public string Save(IEntity entity)
        {
            var returnVal = string.Empty;

            try
            {
                MongoDbOperationResult result = new MongoDBHelper<MemberAdditional>(collection).Save(entity).Result;
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
                _database.DropCollection("MemberAdditional");
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
                //var collection = _database.GetCollection<MemberAdditional>("MemberAdditional");

                var filter = Builders<MemberAdditional>.Filter.Empty;

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

        public string SaveMany(IEnumerable<MemberAdditional> records)
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
                MemberAdditional _actualEntity = entity as MemberAdditional;
                //_actualEntity.IsDeleted = true;
                //_actualEntity.ModifiedOn = DateTime.Now;

                MongoDbOperationResult result = new MongoDBHelper<MemberAdditional>(collection).Save(_actualEntity).Result;

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
