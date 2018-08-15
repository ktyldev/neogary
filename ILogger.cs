using Discord.WebSocket;

public interface ILogger
{
    void Log(string message, string source = "");
    void Log(SocketUserMessage message);
}
