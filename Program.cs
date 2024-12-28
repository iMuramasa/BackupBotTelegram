using System.IO.Compression;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BACKUP_BOT
{
    class Program
    {
        private static TelegramBotClient? _botClient;
        private static Config? _config;
        private static readonly string ConfigPath = "config.json";

        static async Task Main(string[] args)
        {
            await LoadConfigAsync();

            if (_config == null || string.IsNullOrEmpty(_config.Token))
            {
                Console.WriteLine("Invalid configuration.");
                return;
            }

            _botClient = new TelegramBotClient(_config.Token);
            await _botClient.GetMe();

            Console.WriteLine("Available commands:");
            Console.WriteLine("/backup - Initiate backup for all tasks");
            Console.WriteLine("/backup <name> - Initiate backup for specific task");
            Console.WriteLine("/reload - Reload configuration");

            while (true)
            {
                string? input = Console.ReadLine()?.Trim().ToLower();
                if (string.IsNullOrEmpty(input)) continue;

                input = input.Trim().ToLower();

                switch (input)
                {
                    case string s when s.StartsWith("/backup "):
                        var taskName = s.Substring("/backup ".Length).Trim();
                        await PerformBackupAsync(taskName);
                        break;
                    case "/backup":
                        await PerformBackupAsync();
                        break;
                    case "/reload":
                        await ReloadConfigAsync();
                        break;
                }
            }
        }

        private static async Task LoadConfigAsync()
        {
            if (!System.IO.File.Exists(ConfigPath))
            {
                _config = null;
                return;
            }

            try
            {
                string configContent = await System.IO.File.ReadAllTextAsync(ConfigPath);
                _config = JsonSerializer.Deserialize<Config>(configContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                _config = null;
            }
        }

        private static async Task ReloadConfigAsync()
        {
            await LoadConfigAsync();
        }

        private static async Task PerformBackupAsync(string taskName = "")
        {
            if (_config == null || _config.Backups == null)
                return;

            var backups = string.IsNullOrEmpty(taskName)
                  ? _config.Backups
                  : _config.Backups.Where(b => b.Name!.Equals(taskName, StringComparison.OrdinalIgnoreCase));

            if (!backups.Any())
                return;

            foreach (var backup in backups)
            {
                try
                {
                    try
                    {
                        await _botClient!.GetChat(backup.ChatId);
                    }
                    catch
                    {
                        continue;
                    }

                    if (!Directory.Exists(backup.Path))
                        continue;

                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string archiveName = $"{backup.Name}_{timestamp}.zip";
                    string tempPath = Path.Combine(Path.GetTempPath(), archiveName);

                    ZipFile.CreateFromDirectory(backup.Path, tempPath, CompressionLevel.Optimal, false);

                    using (var stream = System.IO.File.OpenRead(tempPath))
                    {
                        await _botClient!.SendDocument(
                            chatId: backup.ChatId,
                            document: InputFile.FromStream(stream, archiveName),
                            caption: $"Backup: {backup.Name}\nPath: {backup.Path}\nDate: {timestamp}"
                        );
                    }

                    System.IO.File.Delete(tempPath);
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}