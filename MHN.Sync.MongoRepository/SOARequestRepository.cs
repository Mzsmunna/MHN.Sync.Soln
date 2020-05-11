﻿using MHN.Sync.Entity.MongoEntity;
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
    public class SOARequestRepository : RepositoryBase //, ISOARequestRepository
    {
        private static IMongoCollection<SOARequest> Collection { get; set; }
        private MongoDBCore<SOARequest> core;
        static SOARequestRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(SOARequest)))
            {
                BsonClassMap.RegisterClassMap<SOARequest>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    map.MapProperty(x => x.Id).SetElementName("_id");
                    map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));

                });
            }
        }

        public SOARequestRepository(IDatabaseContext dbContext)
            : base(dbContext)
        {
            try
            {
                if (Collection == null)
                {
                    Collection = _database.GetCollection<SOARequest>("SOARequest");
                }
                core = new MongoDBCore<SOARequest>(Collection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private FilterDefinition<SOARequest> BuildFilter(string _id)
        {
            var filter = Builders<SOARequest>.Filter.Empty;

            if (!string.IsNullOrEmpty(_id) && _id.ToLower() != "undefined")
            {
                filter = filter & Builders<SOARequest>.Filter.Eq("_id", _id);
            }

            return filter;
        }

        public async Task<SOARequest> GetById(string _id)
        {
            var filter = BuildFilter(_id);
            return await Collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);

        }

        public async Task<List<SOARequest>> GetAllByField(string fieldName, string fieldValue)
        {
            var filter = Builders<SOARequest>.Filter.Eq(fieldName, fieldValue);
            var result = await Collection.Find(filter).ToListAsync().ConfigureAwait(false);
            //var result = await Collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<List<SOARequest>> GetAll()
        {
            var filter = BuildFilter(null);
            return await Collection.Find(filter).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<SOARequest>> GetAll(int currentPage, int pageSize)
        {
            var filter = BuildFilter(null);
            return await Collection.Find(filter).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync().ConfigureAwait(false);
        }

        public int GetAllCount()
        {
            int count = 0;
            try
            {
                var filter = BuildFilter(null);
                count = Convert.ToInt32(Collection.Find(filter).Count());
            }
            catch (Exception ex)
            {
                //new ExceptionWrapper(ex).Handle();
            }
            return count;
        }

        public SOARequest GetPtcRequestDetailsById(string prosMemberRef)
        {
            try
            {
                var filter = Builders<SOARequest>.Filter.Eq("ProspectMemberDataRef", prosMemberRef);
                return Collection.Find(filter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string Save(IEntity entity)
        {
            var returnVal = string.Empty;
            try
            {
                MongoDbOperationResult result = new MongoDBHelper<SOARequest>(Collection).Save(entity).Result;
                returnVal = result.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnVal;
        }

        public string SaveMany(IEnumerable<SOARequest> records)
        {
            var returnVal = string.Empty;

            try
            {
                Collection.InsertMany(records);
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
                SOARequest _actualEntity = entity as SOARequest;
                //_actualEntity.IsDeleted = true;
                //_actualEntity.ModifiedOn = DateTime.Now;

                MongoDbOperationResult result = new MongoDBHelper<SOARequest>(Collection).Save(_actualEntity).Result;

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
    }
}
