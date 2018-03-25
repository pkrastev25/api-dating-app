using System;

namespace api_dating_app.DTOs
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public class MessageForReturnDto
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public string SenderKnownAs { get; set; }

        public string SenderPhotoUrl { get; set; }

        public int RecipientId { get; set; }

        public string RecipientKnowAs { get; set; }

        public string RecipientPhotoUrl { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime? MessageReadTime { get; set; }

        public DateTime MessageSentTime { get; set; }
    }
}