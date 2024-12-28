using System.Text.Json.Serialization;

namespace BACKUP_BOT
{
    public class Config
    {
        public string ?Token { get; set; }

        [JsonPropertyName("Backups")]
        public BackupTask[] ?Backups { get; set; }
    }

    public class BackupTask
    {
        public string ?Name { get; set; }
        public string ?Path { get; set; }
        public long ChatId { get; set; }
    }
} 