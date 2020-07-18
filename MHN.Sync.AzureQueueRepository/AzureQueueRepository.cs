using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHN.Sync.Telemetry;
using Microsoft.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

namespace MHN.Sync.AzureQueueRepository
{
    public class AzureQueueRepository
    {
        private string _connnectionString { get; set; }
        private CloudQueueClient _cloudQueueClient
        {
            get => CloudStorageAccount.Parse(this._connnectionString).CreateCloudQueueClient();
        }

        public AzureQueueRepository(string storageConnectionString)
        {
            _connnectionString = storageConnectionString;
        }

        public string CreateQueue(string name)
        {
            try
            {
                CloudQueue queue = this._cloudQueueClient.GetQueueReference(name);
                bool result = queue.CreateIfNotExists();
                return result ? name : null;
            }
            catch (Exception ex)
            {
                TelemetryLogger.LogException(ex);
            }
            return null;
        }

        public bool PushMessageToQueue(string queueName, string message)
        {
            try
            {
                CloudQueue queue = this._cloudQueueClient.GetQueueReference(queueName);
                CloudQueueMessage cloudQueueMessage = new CloudQueueMessage(message);
                queue.AddMessage(cloudQueueMessage);
                return true;
            }
            catch(Exception ex)
            {
                TelemetryLogger.LogException(ex);
            }
            return false;
        }

        public string PeekMessageFromQueue(string queueName)
        {
            try
            {
                CloudQueue queue = this._cloudQueueClient.GetQueueReference(queueName);
                CloudQueueMessage peekedMessage = queue.PeekMessage();
                return peekedMessage.AsString;
            }
            catch(Exception ex)
            {
                TelemetryLogger.LogException(ex);
            }
            return null;
        }
    }
}
