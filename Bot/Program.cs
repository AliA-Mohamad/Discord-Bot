using Discord.WebSocket;
using Discord;

class Program
{
    private static DiscordSocketClient _client;

    public static async Task Main(string[] args)
    {
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        _client = new DiscordSocketClient(config);

        _client.Log += LogAsync;
        _client.Ready += ReadyAsync;
        _client.MessageReceived += MessageReceivedAsyc;
        _client.InteractionCreated += InteractionCreatedAsync;

        await _client.LoginAsync(TokenType.Bot, "Token");

        await _client.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }

    public static Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log.ToString());
        return Task.CompletedTask;
    }

    private static Task ReadyAsync()
    {
        Console.WriteLine($"{_client.CurrentUser} is connected!");
        return Task.CompletedTask;
    }

    private static async Task MessageReceivedAsyc(SocketMessage message)
    {
        if (message.Author.Id == _client.CurrentUser.Id) return;

        await message.Channel.SendMessageAsync($"Vem cá baixinha~ {message.Author.Mention}");
    }

    private static async Task InteractionCreatedAsync(SocketInteraction interaction)
    {
        if (interaction is SocketMessageComponent component)
        {
            if (component.Data.CustomId == "unique-id")
                await interaction.RespondAsync("Thank you for clicking my button!");

            else
                Console.WriteLine("An ID has been received that has no handler!");
        }
    }
}