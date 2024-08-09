using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

var modelId = "llama3.1:70b";
var endpoint = "http://localhost:11434";
var apiKey = "api-key-not-needed";

// create custom http client
var httpClient = new HttpClient
{
    BaseAddress = new Uri(endpoint)
};

var builder = Kernel.CreateBuilder();

builder.AddOpenAIChatCompletion(modelId, apiKey, httpClient: httpClient);

var kernel = builder.Build();

var chatService = kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();
history.AddSystemMessage("You are help full assistant that will help you with your questions.");

while (true)
{
    Console.Write("You: ");
    var userMessage = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userMessage))
    {
        break;
    }

    history.AddUserMessage(userMessage);

    var response = await chatService.GetChatMessageContentAsync(history);

    Console.WriteLine($"Bot: {response.InnerContent}");

    history.AddMessage(response.Role, response.Content ?? string.Empty);
}