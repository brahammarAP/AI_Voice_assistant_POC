using Azure;
using Azure.AI.OpenAI;
using SpeakBot.Models;

namespace SpeakBot.Services;

public interface IAzureAiChatServices
{
    Task<Message> GetResponseAsync(List<Message> messagechain, string systemMessage);
}

public class AzureAiChatServices : IAzureAiChatServices
{
    private readonly IConfiguration _configuration;

    public AzureAiChatServices(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    //Funktion som tar in en lista av meddelande - tar alla meddelande inklusive SystemMessage och skickar den till modellen och får ett svar.
    //Svaret blir till ett enda meddelande som visas till användaren.
    public async Task<Message> GetResponseAsync(List<Message> messagechain, string systemMessage)
    {
        systemMessage ??= "You are an AI assistant that helps people find information.";

        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            var timeout = TimeSpan.FromSeconds(10);
            cancellationTokenSource.CancelAfter(timeout);

            string response = "";

            OpenAIClient client = new OpenAIClient(
                new Uri(_configuration["Azure:OpenAIUrl"]),
                //new Uri(_settings.OpenAIUrl),
                new AzureKeyCredential(_configuration["Azure:OpenAIKey"]));

            //För att kalla på API:n behöver vi options som ska innehålla information vilket krävs för att få ett svar från modellen.
            //SystemMessage sätts in via ChatRole.Systems - den kommer skickas med varje meddelande som skickas till modellen.
            //Därefter en look som kollar om det är användaren eller assistenten som skickar meddelande.
            ChatCompletionsOptions options = new ChatCompletionsOptions();
            options.Temperature = (float)0.7;
            options.MaxTokens = 800;
            options.NucleusSamplingFactor = (float)0.95;
            options.FrequencyPenalty = 0;
            options.PresencePenalty = 0;
            options.Messages.Add(new ChatMessage(ChatRole.System, systemMessage));
            foreach (var msg in messagechain)
            {
                if (msg.IsPrompt)
                {
                    options.Messages.Add(new ChatMessage(ChatRole.User, msg.Body));
                }
                else
                {
                    options.Messages.Add(new ChatMessage(ChatRole.Assistant, msg.Body));
                }
            }

            try
            {
                Response<ChatCompletions> resp = await client.GetChatCompletionsAsync(
                    _configuration.GetSection("Azure")["OpenAIDeploymentModel"]!,
                    options,
                    cancellationTokenSource.Token);

                ChatCompletions completions = resp.Value;
                string response1 = completions.Choices[0].Message.Content;
                Message responseMessage = new Message(response1, false);
                return responseMessage;
            }
            catch (TaskCanceledException)
            {
                // Hanterar time-out
                return new Message("API call timed out", false);
            }
            catch (RequestFailedException)
            {
                // Hanterar andra Azure API request-fel här
                return new Message("API request failed", false);
            }
            catch (Exception ex)
            {
                // Hanterar connection-fel här
                return new Message("Connection error", false);
            }
        }
    }
}

