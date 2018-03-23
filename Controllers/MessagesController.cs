using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using api_dating_app.Data;
using api_dating_app.DTOs;
using api_dating_app.Helpers;
using api_dating_app.models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_dating_app.Controllers
{
    [ServiceFilter(typeof(LogUserActivityHelper))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IDatingRepository _datingRepository;
        private readonly IMapper _mapperService;

        public MessagesController(IDatingRepository datingRepository, IMapper mapperService)
        {
            _datingRepository = datingRepository;
            _mapperService = mapperService;
        }

        [HttpGet("{messageId}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int messageId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var messageFromRepo = await _datingRepository.GetMessage(messageId);

            if (messageFromRepo == null)
            {
                return NotFound();
            }

            return Ok(messageFromRepo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId,
            [FromBody] MessageForCreationDto messageForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            messageForCreationDto.SenderId = userId;

            var recipient = await _datingRepository.GetUser(messageForCreationDto.RecipientId);
            var sender = await _datingRepository.GetUser(messageForCreationDto.SenderId);

            if (recipient == null)
            {
                return BadRequest("Could not find user!");
            }

            var message = _mapperService.Map<MessageModel>(messageForCreationDto);

            _datingRepository.Add(message);

            var messageToReturn = _mapperService.Map<MessageForCreationDto>(message);

            if (await _datingRepository.SaveAll())
            {
                return CreatedAtRoute("GetMessage", new {messageId = message.Id}, messageToReturn);
            }

            throw new Exception("Creating the message failed on save!");
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, MessageParamsHelper messageParamsHelper)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var messagesFromRepo = await _datingRepository.GetMessagesForUser(messageParamsHelper);
            var messages = _mapperService.Map<IEnumerable<MessageForReturnDto>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalCount,
                messagesFromRepo.TotalPages);

            return Ok(messages);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var messagesFromRepo = await _datingRepository.GetMessageThread(userId, recipientId);
            var messageThread = _mapperService.Map<IEnumerable<MessageForReturnDto>>(messagesFromRepo);

            return Ok(messageThread);
        }

        [HttpPost("{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var messageFromRepo = await _datingRepository.GetMessage(messageId);

            if (messageFromRepo.SenderId == userId)
            {
                messageFromRepo.IsSenderDeleted = true;
            }

            if (messageFromRepo.RecipientId == userId)
            {
                messageFromRepo.IsRecipientDeleted = true;
            }

            if (messageFromRepo.IsSenderDeleted && messageFromRepo.IsRecipientDeleted)
            {
                _datingRepository.Delete(messageFromRepo);
            }

            if (await _datingRepository.SaveAll())
            {
                return NoContent();
            }

            throw new Exception("An error has occured while deleting the message!");
        }

        [HttpPost("{messageId}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int messageId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var message = await _datingRepository.GetMessage(messageId);

            if (message.RecipientId != userId)
            {
                return BadRequest("Failed to mark message as read!");
            }

            message.IsRead = true;
            message.MessageReadTime = DateTime.Now;

            await _datingRepository.SaveAll();

            return NoContent();
        }
    }
}