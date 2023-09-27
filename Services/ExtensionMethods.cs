using Microsoft.Extensions.Logging;

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
}
