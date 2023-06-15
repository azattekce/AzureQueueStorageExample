using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Queues.Models;
using AzureQueueStorageExample.IServices;
using AzureQueueStorageExample.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureQueueStorageExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        readonly IConsumerQueueServices _consumerQueueServices;
        public ConsumerController(IConsumerQueueServices consumerQueueServices)
        {
          
            _consumerQueueServices = consumerQueueServices;
            
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> RetrieveMessageAsync()
        {
            try
            {
                QueueMessage queueMessage = await _consumerQueueServices.RetrieveNextMessageAsync();
                return Ok(queueMessage?.MessageText);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> PeekMessageAsync()
        {
            try
            {
                PeekedMessage peekedMessage = await _consumerQueueServices.PeekMessageAsync();
                return Ok(peekedMessage?.MessageText);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }




        [HttpGet("[action]/{messageText}")]
        public async Task<IActionResult> SendMessageAsync(string messageText)
        {
            try
            {
                await _consumerQueueServices.SendMessageAsync(messageText);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpGet("[action]/{newMessageText}")]
        public async Task<IActionResult> UpdateMessageAsync(string newMessageText)
        {
            try
            {
                await _consumerQueueServices.UpdateMessageAsync(newMessageText);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> DeleteMessageAsync()
        {
            try
            {
                await _consumerQueueServices.DeleteMessageAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> DeleteQueueAsync()
        {
            try
            {
                await _consumerQueueServices.DeleteQueueAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


    }
}
