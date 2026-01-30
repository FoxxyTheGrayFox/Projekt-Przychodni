using System.Globalization;

namespace PrzychodniaApp;

public static class ConsoleHelpers
{
    // Konwersja daty bez czasu z walidacją formatu
    public static DateOnly ConvertDateOnly(string komunikat)
    {
        Console.Write(komunikat);
        DateOnly data;
        while (!DateOnly.TryParseExact(
            Console.ReadLine(),
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out data))
        {
            Console.Write("Błędny format, proszę użyć (dd/mm/yyyy): ");
        }

        return data;
    }
    // Konwersja daty z walidacją formatu
    public static DateTime ConvertDateTime(string komunikat)
    {
        Console.Write(komunikat);
        DateTime data;
        while (!DateTime.TryParseExact(
            Console.ReadLine(),
            "dd/MM/yyyy HH:mm",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out data))
        {
            Console.Write("Błędny format, proszę użyć (dd/mm/yyyy hh:mm): ");
        }

        return data;
    }
    // Wymuszenie aby pole nie było puste
     public static string ReadRequired(string komunikat)
    {
        string? input;
        do
        {
            Console.Write(komunikat);
            input = Console.ReadLine();
        }
        while (string.IsNullOrWhiteSpace(input)); // blokuje puste pola oraz spacje

        return input;
    }
}
