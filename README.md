# Telegram Backup Bot

Telegram Backup Bot is a simple and efficient tool for backing up files to a Telegram channel. It allows you to quickly send files to your designated channels with just a bot token and channel IDs. The bot supports backup commands and dynamic configuration reloads.

---

## Features
- **Easy File Backup:**
  Quickly back up files by simply typing a command in the console.

- **Channel-Specific Backups:**
  Use custom names from `config.json` for targeted backups.

- **Dynamic Configuration Reload:**
  Update configuration without restarting the bot.

---

## Getting Started
### Requirements
1. A Telegram Bot token. Create one via [BotFather](https://core.telegram.org/bots#botfather).
2. The Telegram Channel IDs where files will be sent.

---

### Installation
1. Clone this repository:
   ```bash
   git clone <repository_url>
   ```
2. Create and configure `config.json`:
   ```json
   {
       "Token": "YOUR_TELEGRAM_BOT_TOKEN",
       "Backups": [
           {
               "Name": "site",
               "Path": "C:/xampp/htdocs",
               "ChatId": here
           },
           {
               "Name": "projects",
               "Path": "D:/Data/Projects",
               "ChatId": here
           }
       ]
   }
   ```

---

### Usage
#### Back Up All Files
To back up all files:
```bash
/backup
```

#### Back Up to a Specific Channel
To back up files to a specific channel:
```bash
/backup <name>
```

#### Reload Configuration
To reload the bot's configuration without restarting:
```bash
/reload
```

---

## Example `config.json`
```json
{
    "Token": "YOUR_TELEGRAM_BOT_TOKEN",
    "Backups": [
        {
            "Name": "test",
            "Path": "C:/xampp/htdocs",
            "ChatId": 1231213224
        },
        {
            "Name": "test123",
            "Path": "D:/Data/Projects",
            "ChatId": 4234234222
        }
    ]
}
```

---

## Contribution
Feel free to submit issues or pull requests to improve this project. Contributions are always welcome!

---

## Contact
For questions or support, reach out via the Discord: https://discord.gg/jmZqsdhsV3.

