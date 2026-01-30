namespace PrzychodniaApp;

public static class Widoki
{
    public static void WyswietlPacjenta(Pacjent pacjent)
    {
        Console.WriteLine($"""
        {pacjent.Imie} {pacjent.Nazwisko}
        Data urodzenia: {pacjent.DataUrodzenia:dd/MM/yyyy}
        Adres: {pacjent.Adres}
        Telefon: {pacjent.NumerTelefonu}
        """);
    }

    public static void WyswietlWizyty(IEnumerable<Wizyta> wizyty)
    {
        Console.WriteLine("\nWIZYTY");
        foreach (var w in wizyty)
        {
            Console.WriteLine(
                $"{w.DataWizyty:dd/MM/yyyy HH:mm} | " +
                $"{w.Lekarz.Imie} {w.Lekarz.Nazwisko} ({w.Lekarz.Specjalizacja})");
        }
    }
}
