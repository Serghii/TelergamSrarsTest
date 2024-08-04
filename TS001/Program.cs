using Telegram.Bot.Types.Payments;


class Program
{
    //in test environment you have to add "/test" to the end of your token
    private const string tokenTest = "*****/test";
    private const string token = "*****";
    
    static async Task Main()
    {
        var cts = new CancellationTokenSource();
        
        TelegramBotBase telegramBotBase = new TelegramBotBase(tokenTest);
        telegramBotBase.Run(cts.Token);
        
        //this url have to send to the client side for payment
        var url = await telegramBotBase.GetInvoiceLink(title: "TestTitle", description: "TestDescription",
            providerToken:"providerToken" ,payload: "Test", currency: "XTR", prices: new LabeledPrice[]
        {
            new LabeledPrice("Label", 1),
        });
        
        Console.WriteLine($"InvoiceLink: {url}");
        while (!cts.IsCancellationRequested)
        {
            await Task.Delay(1000, cts.Token);
        }
        Console.WriteLine("BotStopped");
    }
}