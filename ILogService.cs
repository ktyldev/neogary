using Discord.WebSocket;

public interface ILogService
{
    void Log(string message, string source = "");
    void Log(SocketUserMessage message);
}
