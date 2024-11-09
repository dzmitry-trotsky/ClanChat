namespace GroupChat.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public string SenderName { get; set; }

        public DateTime SendDate { get; set; }

        public string Text { get; set; }

        public Guid ClanId { get; set; }
    }
}
