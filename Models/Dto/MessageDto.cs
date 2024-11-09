namespace GroupChat.Models.Dto
{
    public class MessageDto
    {
        public Guid Id { get; set; }

        public string SenderName { get; set; }

        public DateTime SendDate { get; set; }

        public string Message { get; set; }

        public string ClanName { get; set; }
    }
}
