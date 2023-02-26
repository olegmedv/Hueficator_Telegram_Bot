using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using Telegram.Bot;
using Telegram.Bot.Types;

internal class Program
{
    private static string token { get; set; } = "";
    private static void Main(string[] args)
    {
        var client = new TelegramBotClient(token);
        client.StartReceiving(Update, Error);
        Console.ReadLine();
        //var inputStr = "Telegram";
        //inputStr = RemovePunctuations(inputStr);
        //var inputArray = inputStr.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

        //for (int i = 0; i < inputArray.Length; i++)
        //{
        //    inputArray[i] = huefy(inputArray[i]);
        //}

        //Console.WriteLine(string.Join(" ", inputArray));
        //Console.ReadKey();

    }

    private async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken arg3)
    {
        var message = update.Message;
        if (message == null || message.Text == null)
        {
            return;
        }
        var messageString = RemovePunctuations(message.Text);
        if (messageString == null || messageString == String.Empty)
        {
            return;
        }
        var messageArray = messageString.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < messageArray.Length; i++)
        {
            try
            {
                messageArray[i] = huefy(messageArray[i]);
            }
            catch (Exception)
            {

                messageArray[i] = messageArray[i];
            }
        }
        messageString = string.Join(" ", messageArray);
        await botClient.SendTextMessageAsync(message.Chat.Id, messageString);
    }

    private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }

    private static string RemovePunctuations(string input)
    {
        return Regex.Replace(input, "\\p{P}", string.Empty);
    }

    private static string huefy(string src)
    {
        char[] G = new[] { 'а', 'у', 'о', 'ы', 'и', 'э', 'я', 'ю', 'ё', 'е' };
        char[] S = new[] { 'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ' };
        Dictionary<char, char> Y = new Dictionary<char, char>() {
            {'а','я'},
            {'у','ю'},
            {'о','е'},
            {'ы','и'},
            {'и','и'},
            {'э','е'},
            {'я','я'},
            {'ю','ю'},
            {'ё','е'},
            {'е','е'}
        };

        if (src.Length <= 4) return src;

        var ending = src.ToLower();

        // Пропускаем начальные согласные (для коротких слов)
        var match = src.IndexOfAny(G);
        if (match != -1)
            ending = ending.Substring(match);

        // Берем окончание
        ending = ending.Substring(ending.Length < 5 ? ending.Length - 3 : ending.Length - 5);

        //// Сокращаем окончание до первой согласной
        match = ending.IndexOfAny(S);
        if (match != -1)
            ending = ending.Substring(match);

        var base1 = src.Substring(0, src.Length - ending.Length);
        var gIndex = base1.IndexOfAny(G);
        if (gIndex == -1)
        {
            return src;
        }

        ending = src.Substring(gIndex + 1);

        var x = base1[gIndex];
        var y = Y[x];

        var returnString = $"{src}-ху{y}{ending}";

        return returnString;
    }
}