using System.Globalization;
using PrzychodniaApp;
using ScottPlot;

internal class Program
{
    static void Main()
    {
        var przychodnia = new Przychodnia();
        przychodnia.Wczytaj();

        Statystyka.GenerujWykres(
        przychodnia.Wizyty,"wizyty.png");

        while (true)
        {
            Console.Clear();
            Console.WriteLine("PRZYCHODNIA");
            Console.WriteLine("1. Dodaj pacjenta");
            Console.WriteLine("2. Dodaj lekarza");
            Console.WriteLine("3. Zarejestruj wizytę");
            Console.WriteLine("4. Wyświetl dane pacjenta");
            Console.WriteLine("5. Wyświetl wizyty pacjenta");
            Console.WriteLine("0. Zakończ");
            Console.Write("Wybór: ");

            switch (Console.ReadLine())
            {
                case "1":
                    DodajPacjenta(przychodnia);
                    break;
                case "2":
                    DodajLekarza(przychodnia);
                    break;
                case "3":
                    ZarejestrujWizyte(przychodnia);
                    break;
                case "4":
                    WyswietlDanePacjenta(przychodnia);
                    break;
                case "5":
                    WyswietlWizytyPacjenta(przychodnia);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Niepoprawny wybór.");
                    break;
            }

            Console.WriteLine("\nNaciśnij dowolny klawisz...");
            Console.ReadKey();
        }
    }

    static void DodajPacjenta(Przychodnia przychodnia)
    {
        var imie = ConsoleHelpers.ReadRequired("Imię: ");
        var nazwisko = ConsoleHelpers.ReadRequired("Nazwisko: ");
        var dataUrodzenia = ConsoleHelpers.ConvertDateOnly("Data urodzenia (dd/MM/yyyy): ");
        var adres = ConsoleHelpers.ReadRequired("Adres: ");
        var telefon = ConsoleHelpers.ReadRequired("Numer telefonu: ");
        var pesel = ConsoleHelpers.ReadRequired("PESEL: ");

        try
        {
            przychodnia.DodajPacjenta(new Pacjent
            {
                Imie = imie,
                Nazwisko = nazwisko,
                DataUrodzenia = dataUrodzenia,
                Adres = adres,
                NumerTelefonu = telefon,
                PESEL = pesel
            });

            Console.WriteLine("Pacjent dodany.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void DodajLekarza(Przychodnia przychodnia)
    {
        var imie = ConsoleHelpers.ReadRequired("Imię: ");
        var nazwisko = ConsoleHelpers.ReadRequired("Nazwisko: ");
        var specjalizacja = ConsoleHelpers.ReadRequired("Specializacja: ");
        var telefon = ConsoleHelpers.ReadRequired("Numer telefonu: ");

        przychodnia.DodajLekarza(imie, nazwisko, specjalizacja, telefon);
        Console.WriteLine("Lekarz dodany.");
    }

    static void ZarejestrujWizyte(Przychodnia przychodnia)
    {
        var pesel = ConsoleHelpers.ReadRequired("PESEL pacjenta: ");
        var idLekarza = ConsoleHelpers.ReadRequired("ID lekarza: ");

        var dataWizyty = ConsoleHelpers.ConvertDateTime("Data wizyty (dd/MM/yyyy HH:mm): ");

        try
        {
            przychodnia.ZarejestrujWizyte(pesel, idLekarza, dataWizyty);
            Console.WriteLine("Wizyta zarejestrowana.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void WyswietlDanePacjenta(Przychodnia przychodnia)
    {
        var pesel = ConsoleHelpers.ReadRequired("PESEL pacjenta: ");

        var pacjent = przychodnia.ZnajdzPacjenta(pesel);
        if (pacjent == null)
        {
            Console.WriteLine("Pacjent nie istnieje.");
            return;
        }

        Widoki.WyswietlPacjenta(pacjent);

    }

    static void WyswietlWizytyPacjenta(Przychodnia przychodnia)
    {
        var pesel = ConsoleHelpers.ReadRequired("PESEL pacjenta: ");

        try
        {
            var wizyty = przychodnia.PobierzWizytyPacjenta(pesel);

            if (!wizyty.Any())
            {
                Console.WriteLine("Brak wizyt.");
                return;
            }

            Widoki.WyswietlWizyty(wizyty);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
