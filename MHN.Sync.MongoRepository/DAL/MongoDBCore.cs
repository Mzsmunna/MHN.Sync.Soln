using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.MongoRepository.DAL
{
    public class MongoDBCore<T> where T : class
    {
        public string IdentityKey;

        private bool IsSuccess;

        private readonly IMongoCollection<T> collection;

        public MongoDBCore(IMongoCollection<T> baseCollection)
        {
            try
            {
                this.collection = baseCollection;
            }
            catch(Exception ex)
            {
             
            }
            finally
            {
                //Do Nothing
            }
        }
        public async Task<bool> Save(T entity)
        {
            try
            {
                //WriteConcernResult result = this.collection.Save(entity);
                await this.collection.InsertOneAsync(entity).ConfigureAwait(false);
                IsSuccess = true;
                //IsSuccess = result.Ok;
                //if (result.Ok && result.DocumentsAffected != 0 && !result.UpdatedExisting)
                //    IdentityKey = result.Upserted.AsBsonValue.ToString();

            }
            catch(Exception ex)
            {
                IsSuccess = false;
                
            }  
            finally
            {
                //Do Nothing
            }

            return IsSuccess;            
        }

        public async Task< T> Get(MongoParam query)
        {
            try
            {
                var result = await this.collection.Find(query.Parameters).FirstOrDefaultAsync().ConfigureAwait(false); 


                return result == null ? null : result as T;                
            }
            catch (Exception ex)
            {
              
            }  
            finally
            {
                //Do Nothing
            }

            return null;
        }

        public async Task<T> SortAndGet(MongoParam query, string sortingFieldName)
        {
            try
            {
                var sort = Builders<T>.Sort.Descending(sortingFieldName);

                var result = await this.collection.Find(query.Parameters).Sort(sort).FirstOrDefaultAsync().ConfigureAwait(false);


                return result == null ? null : result as T;
            }
            catch (Exception ex)
            {
              
            }
            finally
            {
                //Do Nothing
            }

            return null;
        }

        public async Task<List<T>> GetAll(MongoParam query)
        {
            try
            {
                var result = await this.collection.Find(query.Parameters).ToListAsync().ConfigureAwait(false);

                return result == null ? null :  result;     
               
            }
            catch (Exception ex)
            {
                
            } 
            finally
            {
                //Do Nothing
            }

            return null;
        }
    }
}
