using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;

internal class TelegramBotBase
{
    private readonly string _token;
    
    protected User _me = new User();
    private TelegramBotClient _bot;
    
    public TelegramBotBase(string token)
    {
        _token = token;
    }
    
    protected ITelegramBotClient Bot => _bot;

    public virtual async Task Run(CancellationToken ct)
    {
        _bot = new TelegramBotClient(_token);
        var me = await _bot.GetMeAsync(cancellationToken: ct);
        
        _bot.StartReceiving(OnUpdate, OnError, cancellationToken: ct);
        Console.WriteLine($"Start listening for @{me.Username}");
    }
    
    public virtual async Task<string> GetInvoiceLink(
        string title,
        string description,
        string payload,
        string providerToken,
        string currency,
        IEnumerable<LabeledPrice> prices,
        int? maxTipAmount = default,
        IEnumerable<int>? suggestedTipAmounts = default,
        string? providerData = default,
        string? photoUrl = default,
        int? photoSize = default,
        int? photoWidth = default,
        int? photoHeight = default,
        bool? needName = default,
        bool? needPhoneNumber = default,
        bool? needEmail = default,
        bool? needShippingAddress = default,
        bool? sendPhoneNumberToProvider = default,
        bool? sendEmailToProvider = default,
        bool? isFlexible = default,
        CancellationToken ct = default
    )
    {
        return _bot == default
            ? await Task.Run(async () =>
            {
                Console.WriteLine($"Bot is not initialized call {nameof(Run)}() first");
                return String.Empty;
            }, ct)
            : await _bot.CreateInvoiceLinkAsync(title, description, payload, providerToken, currency, prices,
                maxTipAmount, suggestedTipAmounts, providerData, photoUrl, photoSize, photoWidth, photoHeight,
                needName, needPhoneNumber, needEmail, needShippingAddress, sendPhoneNumberToProvider,
                sendEmailToProvider, isFlexible, ct);
    }

    protected virtual async Task OnError(ITelegramBotClient bot, Exception ex, CancellationToken ct) => 
        Console.WriteLine($"occured: {ex}");

    private async Task OnUpdate(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        try
        {
            await (update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(bot, update.Message!, ct),
                UpdateType.EditedMessage => BotOnMessageReceived(bot, update.Message!, ct),
                UpdateType.PreCheckoutQuery => BotOnPreCheckoutQueryReceived(bot, update.PreCheckoutQuery!, ct),
                _ => OnUnhandledUpdate(update)
            });
        }
#pragma warning disable CA1031
        catch (Exception ex)
        {
            Console.WriteLine($"Exception while handling {update.Type}: {ex}");
        }
#pragma warning restore CA1031
            

    }
    protected virtual async Task OnUnhandledUpdate(Update update) => 
        Console.WriteLine($"Received unhandled update {update.Type}");

    protected virtual async Task BotOnMessageReceived(ITelegramBotClient bot, Message msg, CancellationToken ct) =>
        await (msg.Type switch
        {
            MessageType.Text => BotOnTextMessageReceived(bot, msg, ct),
            MessageType.SuccessfulPayment => SuccessfulPayment(bot, msg.SuccessfulPayment!, ct),
            _ => OnUnhandledMessage(bot, msg, ct)
        });

    protected virtual async Task BotOnTextMessageReceived(ITelegramBotClient bot, Message msg, CancellationToken ct)
    {
        if (msg.Text is not { } text)
            Console.WriteLine($"Received a message of type {msg.Type}");
        else if (text.StartsWith('/'))
            await OnCommand(bot, msg, ct);
        else
            await OnTextMessage(bot, msg, ct);
    }

    protected virtual async Task OnUnhandledMessage(ITelegramBotClient bot, Message msg, CancellationToken ct) => 
        Console.WriteLine($"Received a message of type {msg.Type}");

    // received a text message that is not a command
    protected virtual async Task OnTextMessage(ITelegramBotClient bot, Message msg, CancellationToken ct) =>
        Console.WriteLine($"Received text '{msg.Text}' in {msg.Chat}");

    protected virtual async Task OnCommand(ITelegramBotClient bot, Message msg, CancellationToken ct) => 
        Console.WriteLine($"Received command: {msg.Text}");

    protected virtual async Task BotOnPreCheckoutQueryReceived(
        ITelegramBotClient bot, PreCheckoutQuery updatePreCheckoutQuery, CancellationToken ct)
    {
        // means access to the product is granted
        await bot.AnswerPreCheckoutQueryAsync(updatePreCheckoutQuery.Id, ct);
        //otherwise just add errorMessage :
        //await bot.AnswerPreCheckoutQueryAsync(updatePreCheckoutQuery.Id, errorMessage:"Unfortunately we can't sell the merchandise", ct);
        Console.WriteLine( $"Received a pre-checkout query from [{updatePreCheckoutQuery.From}] for {updatePreCheckoutQuery.TotalAmount} {updatePreCheckoutQuery.Currency}");
    }
    
    protected virtual async Task SuccessfulPayment(ITelegramBotClient bot, SuccessfulPayment payment, CancellationToken ct) => 
        Console.WriteLine($"Successful Payment: {payment.InvoicePayload}, {payment.Currency}, {payment.TotalAmount}, {payment.ProviderPaymentChargeId}");
    
}