﻿using System;

namespace api_dating_app.DTOs
{
    public class MessageForCreationDto
    {
        public int SenderId { get; set; }

        public int RecipientId { get; set; }

        public DateTime MessageSentTime { get; set; }

        public string Content { get; set; }

        public MessageForCreationDto()
        {
            MessageSentTime = DateTime.Now;
        }
    }
}