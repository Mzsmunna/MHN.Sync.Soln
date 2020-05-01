using MHN.Sync.MongoInterface;
using MHN.Sync.MongoRepository.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MHN.Sync.Entity;
using MHN.Sync.MongoInterface.BASE;

namespace MHN.Sync.MongoRepository.DAL
{
    public class MongoDbContext : IDatabaseContextContainer<IMongoDatabase>, IDatabaseContext
    { 
        private static IMongoDatabase database;
        private static int retryCount = 3;


        private static bool IsTransient(Exception ex)
        {          
            var webException = ex as WebException;
            if (webException != null)
            {                
                return new[] {WebExceptionStatus.ConnectionClosed, 
                  WebExceptionStatus.Timeout, 
                  WebExceptionStatus.RequestCanceled }.
                        Contains(webException.Status);
            }

            return false;
        }

        private static IMongoDatabase Database
        {
            get
            {
                int currentRetry = 0;
                

                if (database == null)
                {
                    if (!String.IsNullOrEmpty(ConfigServiceForWizard.DatabaseConnectionString) && !String.IsNullOrEmpty(ConfigServiceForWizard.DatabaseName))
                    {
                        var client = new MongoClient(ConfigServiceForWizard.DatabaseConnectionString);
                        database = client.GetDatabase(ConfigServiceForWizard.DatabaseName);
                    }
                    else if (ConfigService.DatabaseConnectionString != null && ConfigService.DatabaseName != null)
                    {
                        var client = new MongoClient(ConfigService.DatabaseConnectionString);
                        database = client.GetDatabase(ConfigService.DatabaseName);    
                    }                    
                }
                return database;
            }
        }

        public IMongoDatabase GetInstance()
        {
            return Database;
        }


        private static string GetLocalConnectionString(string env)
        {
            switch (env)
            {
                case "Dev":
                    return ApplicationConstants.DevConnectionString;
                case "Live":
                    return ApplicationConstants.LiveConnectionString;
            }
            return "";
        }
        private static string GetLocalDatabaseName(string env)
        {
            switch (env)
            {
                case "Dev":
                    return ApplicationConstants.DevDatabaseName;
                case "Live":
                    return ApplicationConstants.LiveDatabaseName;
            }
            return "";
        }

        
    }
}
