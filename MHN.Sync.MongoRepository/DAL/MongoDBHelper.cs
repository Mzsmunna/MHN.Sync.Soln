using MHN.Sync.Entity.MongoEntity;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MHN.Sync.MongoRepository.DAL
{
    internal class MongoDbOperationResult
    {
        internal string Id { get; set; }
        internal bool IsCompleted { get; set; }
    }
    internal class MongoDBHelper<T> where T : class
    {
        private IMongoCollection<T> collection { get; set; }
        public MongoDBHelper(IMongoCollection<T> collection)
        {
            this.collection = collection;
        }
        internal async Task<MongoDbOperationResult> Save(IEntity entity)
        {
            var _entity = entity as T;
            var _id = _entity.GetType().GetProperty("Id").GetValue(_entity, null);

            if (_id !=null && !string.IsNullOrEmpty(_id.ToString()))
            {
                //upadte
                BsonDocument query = new BsonDocument {
                    { "_id" , ObjectId.Parse(_id.ToString()) }
                };

                if(_entity.GetType().GetProperty("ModifiedOn") != null)
                    _entity.GetType().GetProperty("ModifiedOn").SetValue(_entity, DateTime.UtcNow);

                var result =  await collection.ReplaceOneAsync(query, _entity).ConfigureAwait(false);
                return new MongoDbOperationResult() { Id = _id.ToString(), IsCompleted = result.IsAcknowledged }; 
                
            }
            else
            {
                //create
                var _generatedId = ObjectId.GenerateNewId().ToString();


                if (_entity.GetType().GetProperty("CreatedOn") != null && _entity.GetType().GetProperty("CreatedOn").GetValue(_entity) != null)
                {
                    DateTime date = (DateTime)_entity.GetType().GetProperty("CreatedOn").GetValue(_entity);

                    if (date == DateTime.MinValue)
                    {
                        _entity.GetType().GetProperty("CreatedOn").SetValue(_entity, DateTime.UtcNow);
                    }
                }
                else if (_entity.GetType().GetProperty("CreatedOn") != null)
                {
                    _entity.GetType().GetProperty("CreatedOn").SetValue(_entity, DateTime.UtcNow);
                }
                

                if (_entity.GetType().GetProperty("ModifiedOn") != null)
                    _entity.GetType().GetProperty("ModifiedOn").SetValue(_entity, DateTime.UtcNow);

                _entity.GetType().GetProperty("Id").SetValue(_entity, _generatedId);

                await collection.InsertOneAsync(_entity).ConfigureAwait(false);

                // have to return tru for the moment, due to lack of return type support.
                return new MongoDbOperationResult() { Id = _generatedId, IsCompleted = true };               
            }  
        }
    }
}
