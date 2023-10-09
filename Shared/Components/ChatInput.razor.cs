using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using SpeakBot.Models;
using SpeakBot.Repository.IRepository;
using SpeakBot.Services;

namespace SpeakBot.Shared.Components;

public partial class ChatInput
{
    [Inject] IAzureAiChatServices ChatService { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }
    [Inject] IUserSession Session { get; set; }
    [Inject] IChatHistoryRepository ChatHistory { get; set; }
    [Inject] IMessageService MessageService { get; set; }
    [Inject] HttpClient Http { get; set; }

    private ModalDialog ModalDialog { get; set; }
    private IJSObjectReference textToSpeechInterop { get; set; }

    private List<Message> chatMessages = new List<Message>();
    private ChatHistory currentChat;
    private bool awaitReply;
    private bool isText, isDisabled;
    private string? inputText = string.Empty;

    private void btnState(string isText) => this.isText = (string.IsNullOrEmpty(isText)) ? false : true;

    protected override async Task OnInitializedAsync()
    {
        MessageService.OnChatUpdate += LoadMessages;
        navManager.LocationChanged += StopSpeech;
        LoadMessages();
    }

    async void LoadMessages()
    {
        chatMessages = Session.CurrentChat.Prompts;
        currentChat = Session.CurrentChat;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                textToSpeechInterop = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/AzureCognitiveServicesHelper.js");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading JavaScript module: " + ex.Message);
            }
        }
    }

    void ModalUpdater()
    {
        awaitReply = true;
        StateHasChanged();
    }

    private async Task StartRecording()
    {
        isDisabled = true;
        ModalDialog.Open();

        if (textToSpeechInterop != null)
        {
            try
            {
                await JSRuntime.InvokeAsync<object>("Interop.StopPlay");

                var result = await JSRuntime.InvokeAsync<string>("Interop.SpeechToTextSDK", Session.Key, Session.Reigion);

                ModalUpdater();

                if (result == null)
                {
                    SaveChat(new Message("Oops, I forgot to say something!", true));
                    ModalDialog.Close();
                    isDisabled = false;
                    return;
                }

                await HandleAudioData(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating Speech SDK object from blazor: " + ex.Message);
                isDisabled = false;
            }
        }
        else
        {
            Console.WriteLine("speechInterop reference is not loaded.");
        }
    }

    private async Task HandleAudioData(string response)
    {
        if (await ExtensionMethods.StartsWithQuestionWord(Http, response))
            response = response.Substring(0, response.Length - 1) + "?";

        await SaveChat(new Message(response, true));

        await GetAIResponse();
    }

    private async Task HandleTextInput()
    {
        inputText = inputText.Trim();

        if (string.IsNullOrEmpty(inputText)) return;

        await SaveChat(new Message(inputText, true));

        await GetAIResponse();

        inputText = string.Empty;
    }

    private async Task GetAIResponse()
    {
        var response = await ChatService.GetResponseAsync(chatMessages, currentChat.SystemMessage);

        response ??= new Message("Please state your question again!", false);

        await SaveChat(response);

        await GetAiVoice(response);

        var storedHistory = await ChatHistory.GetAllAsync() ?? new List<ChatHistory>();

        if (storedHistory.Count() > 5) await ChatHistory.RemoveItemWhenMoreThanFive(storedHistory.ToList());

    }

    private async Task GetAiVoice(Message request)
    {
        isText = awaitReply = false;

        ModalDialog.Close();

        if (textToSpeechInterop != null)
        {
            try
            {
                await JSRuntime.InvokeAsync<object>("Interop.StopPlay");
                var aiSpeechResponse = await JSRuntime.InvokeAsync<object>("Interop.TextToSpeechSDK", request.Body, "sv-SE-SofieNeural", Session.Key, Session.Reigion);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating Text to Speech SDK object: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Speech Module is not loaded.");
        }

        isDisabled = false;
    }

    async Task SaveChat(Message messageData)
    {
        isDisabled = true;

        chatMessages.Add(messageData);

        var storedHistory = await ChatHistory.GetAllAsync() ?? new List<ChatHistory>();

        var currentHistory = storedHistory.FirstOrDefault(x => x.Id == currentChat.Id);

        if (currentHistory == null)
        {
            var newHistory = new ChatHistory
            {
                Id = currentChat.Id,
                Heading = ExtensionMethods.StringByWordCount(chatMessages.FirstOrDefault(x => x.IsPrompt == true).Body, 5),
                UserId = Session.User.Id,
                Prompts = chatMessages,
                IsLocked = false,
                CardHeading = Session.PromptCard.CardHeading,
                CardImage = Session.PromptCard.CardImage,
                SystemMessage = Session.PromptCard.SystemText
            };

            storedHistory.Add(newHistory);
        }
        else
        {
            currentHistory.Prompts = chatMessages;
        }

        await ChatHistory.CreateAsync(storedHistory);

        MessageService.MessageUpdate(chatMessages);

        if (chatMessages.Count < 2)
            MessageService.NewChat();
    }

    private async Task Resize(ChangeEventArgs e)
    {
        btnState(e.Value.ToString());

        var result = await JSRuntime.InvokeAsync<object>("blazorExtensions.AdjustInputField");

        Double.TryParse(result.ToString(), out var MyHeight);
    }

    private void StopSpeech(object sender, LocationChangedEventArgs e)
    {
        try
        {
            JSRuntime.InvokeAsync<object>("Interop.StopPlay");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error stopping playback: " + ex.Message);
        }
    }

    public void Dispose()
    {
        MessageService.OnChatUpdate -= LoadMessages;
        navManager.LocationChanged -= StopSpeech;
    }
}
