using Azure.Storage.Queues.Models;
using System.Threading.Tasks;

namespace AzureQueueStorageExample.IServices
{
    public interface IBaseQueueServices
    {
        Task SendMessageAsync(string message);

        Task<QueueMessage> RetrieveNextMessageAsync();

        Task<PeekedMessage> PeekMessageAsync();

        Task UpdateMessageAsync(string newMessage);
        Task DeleteMessageAsync();

        Task DeleteQueueAsync();
    }
}
