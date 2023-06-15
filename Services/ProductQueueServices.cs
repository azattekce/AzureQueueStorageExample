using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using AzureQueueStorageExample.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AzureQueueStorageExample.Services
{
    public class ProductQueueServices : IProductQueueServices
    {
        public QueueClient _queueClient;
       public ProductQueueServices()
        {
       
            _queueClient = new QueueClient("DefaultEndpointsProtocol=https;AccountName=azattekcetest;AccountKey=H4LnBl+QUD+b4yQigTIoF549tTD6pQ5TJ/nPOM063zJCJGjCz1XQT82LHkQ/UV1Ungpm+/w4MNw8+AStvfLcBg==;EndpointSuffix=core.windows.net","myproducts");
            _queueClient.CreateIfNotExists();
        }

     
        public async Task DeleteMessageAsync()
        {


            if (await _queueClient.ExistsAsync())
                await _queueClient.DeleteAsync();
            else
                throw new Exception("Kuyruk silinirken beklenmeyen bir hatayla karşılaşıldı.");
        }

        public async Task DeleteQueueAsync()
        {

            if (await _queueClient.ExistsAsync())
            {
                QueueMessage[] queueMessages = await _queueClient.ReceiveMessagesAsync();
                await _queueClient.DeleteMessageAsync(queueMessages[0].MessageId, queueMessages[0].PopReceipt);
            }
            else
                throw new Exception("Mesaj silinirken beklenmeyen bir hatayla karşılaşıldı.");
        }

        public async Task<PeekedMessage> PeekMessageAsync()
        {
            if (await _queueClient.ExistsAsync())
            {
                Response<QueueProperties> response = await _queueClient.GetPropertiesAsync();
                //ApproximateMessagesCount : Önbelleğe alınan yaklaşık mesaj sayısını döner.
                if (response.Value.ApproximateMessagesCount > 0)
                {
                    PeekedMessage[] peekedMessages = await _queueClient.PeekMessagesAsync();
                    if (peekedMessages.Any())
                        return peekedMessages[0];
                }
                return null;
            }
            else
                throw new Exception("Mesaj okunurken(Peek) beklenmeyen bir hatayla karşılaşıldı.");
        }

        public async Task<QueueMessage> RetrieveNextMessageAsync()
        {
            if (await _queueClient.ExistsAsync())
            {
                Response<QueueProperties> response = await _queueClient.GetPropertiesAsync();
                //ApproximateMessagesCount : Önbelleğe alınan yaklaşık mesaj sayısını döner.
                if (response.Value.ApproximateMessagesCount > 0)
                {
                    QueueMessage[] queueMessages = await _queueClient.ReceiveMessagesAsync();
                    if (queueMessages.Any())
                        return queueMessages[0];
                }
                return null;
            }
            else
                throw new Exception("Mesaj okunurken(Retrieve) beklenmeyen bir hatayla karşılaşıldı.");
        }

        public async Task SendMessageAsync(string message)
        {
            if (await _queueClient.ExistsAsync())
                await _queueClient.SendMessageAsync(message);
            else
                throw new Exception("Mesaj gönderilmeye çalışılırken beklenmeyen bir hatayla karşılaşıldı.");
        }

        public async Task UpdateMessageAsync(string newMessage)
        {
            if (await _queueClient.ExistsAsync())
            {
                QueueMessage[] queueMessages = await _queueClient.ReceiveMessagesAsync();
                //TimeSpan.FromSeconds(60) ile 60 saniye daha visiblty özelliğini genişleterek ilgili mesajı görünmez yapıyoruz.
                await _queueClient.UpdateMessageAsync(queueMessages[0].MessageId, queueMessages[0].PopReceipt, newMessage, TimeSpan.FromSeconds(60));
            }
            else
                throw new Exception("Mesaj güncellenirken beklenmeyen bir hatayla karşılaşıldı.");
        }


    }


}
