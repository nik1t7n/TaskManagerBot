using System;
using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Entities
{
    public class Challenge
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime Deadline { get; set; }
    }
}
