using System.Net.Http.Json;

namespace SpeakBot.Services;

public static class ExtensionMethods
{
    public static string StringByWordCount(string text, int wordCount)
    {
        var words = text.Split(' ');

        if (text.Length <= 30 && words.Length < wordCount)
        {
            return text;
        }

        string newString = string.Join(" ", words.Take(wordCount));

        if (newString.Length > 30)
        {
            newString = newString.Substring(0, 30);
        }

        return $"{newString}...";
    }

    public static async Task<bool> StartsWithQuestionWord(HttpClient httpClient, string text)
    {
        var data = await httpClient.GetFromJsonAsync<Dictionary<string, List<string>>>("data/systemSettings.json");

        var starters = data["QuestionStarterWords"].Select(word => word.ToLower()).ToList();

        var firstWord = text.Split(' ').FirstOrDefault()?.ToLower();

        return firstWord != null && starters.Contains(firstWord);
    }

}
