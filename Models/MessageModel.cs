using System;

namespace api_dating_app.models
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public class MessageModel
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public UserModel Sender { get; set; }

        public int RecipientId { get; set; }

        public UserModel Recipient { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime? MessageReadTime { get; set; }

        public DateTime MessageSentTime { get; set; }

        public bool IsSenderDeleted { get; set; }

        public bool IsRecipientDeleted { get; set; }
    }
}