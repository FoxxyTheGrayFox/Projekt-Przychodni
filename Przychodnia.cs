using System.Text.Json;

namespace PrzychodniaApp;

public class Przychodnia
{
    private readonly List<Pacjent> _pacjenci = []; // =[] to samo co = new()
    private readonly List<Lekarz> _lekarze = [];
    private readonly List<Wizyta> _wizyty = [];

    public IReadOnlyCollection<Pacjent> Pacjenci => _pacjenci;
    public IReadOnlyCollection<Lekarz> Lekarze => _lekarze;
    public IReadOnlyCollection<Wizyta> Wizyty => _wizyty;

    private static readonly string Plik = "przychodnia.json";

    private int _ostatniIdLekarza;
    private int _ostatniIdWizyty;

    #region Zapis i Odczyt danych

    // Odczytaj dane z pliku json
    public void Wczytaj()
    {
        if (!File.Exists(Plik)) return;

        var json = File.ReadAllText(Plik);
        var data = JsonSerializer.Deserialize<PrzychodniaDto>(json);

        if (data == null) return;

        _pacjenci.AddRange(data.Pacjenci);
        _lekarze.AddRange(data.Lekarze);
        _wizyty.AddRange(data.Wizyty);

        _ostatniIdLekarza = _lekarze.Count != 0
            ? _lekarze.Max(l => int.Parse(l.IDlekarza)) + 1
            : 0;

        _ostatniIdWizyty = _wizyty.Count != 0
            ? _wizyty.Max(w => int.Parse(w.IDwizyty)) + 1
            : 0;

    }

    private static JsonSerializerOptions GetOptions()
    {
        return new() { WriteIndented = true };
    }

    // Zapisz dane do pliku json
    private void Zapisz(JsonSerializerOptions options)
    {

        var dto = new PrzychodniaDto
        {
            Pacjenci = _pacjenci,
            Lekarze = _lekarze,
            Wizyty = _wizyty
        };
        var json = JsonSerializer.Serialize(dto,
            options);

        File.WriteAllText(Plik, json);
    }

    #endregion

    #region Polecenia do menu

    // Funkcja dodająca nowego pacjenta do przychodnii
    public void DodajPacjenta(Pacjent pacjent)
    {
        if (!pacjent.PESEL.All(char.IsDigit))
            throw new InvalidOperationException("PESEL może zawierać tylko cyfry.");

        if (!pacjent.NumerTelefonu.All(char.IsDigit))
            throw new InvalidOperationException("Numer telefonu może zawierać tylko cyfry.");

        if (_pacjenci.Any(p => p.PESEL == pacjent.PESEL))
            throw new InvalidOperationException("Pacjent o podanym PESEL'u już istnieje.");
        
        // Sprawdzenie czy dane pacjenta nie są puste
        if (string.IsNullOrWhiteSpace(pacjent.Imie) ||
            string.IsNullOrWhiteSpace(pacjent.Nazwisko) ||
            string.IsNullOrWhiteSpace(pacjent.Adres) ||
            string.IsNullOrWhiteSpace(pacjent.NumerTelefonu) ||
            string.IsNullOrWhiteSpace(pacjent.PESEL))
        {
            throw new InvalidOperationException("Wszystkie pola pacjenta są wymagane.");
        }

        _pacjenci.Add(pacjent);
        Zapisz(GetOptions());
    }

    public Pacjent? ZnajdzPacjenta(string pesel)
        => _pacjenci.FirstOrDefault(p => p.PESEL == pesel);

    // Funkcja dodająca nowego lekarza do przychodnii
    public void DodajLekarza(string imie, string nazwisko, string specjalizacja, string telefon)
    {
        if (!telefon.All(char.IsDigit))
            throw new InvalidOperationException("Numer telefonu może zawierać tylko cyfry.");

        // Sprawdzenie czy dane lekarza nie są puste
        if (string.IsNullOrWhiteSpace(imie) ||
            string.IsNullOrWhiteSpace(nazwisko) ||
            string.IsNullOrWhiteSpace(specjalizacja) ||
            string.IsNullOrWhiteSpace(telefon))
        {
            throw new InvalidOperationException("Wszystkie pola lekarza są wymagane.");
        }

        var lekarz = new Lekarz
        {
            IDlekarza = (_ostatniIdLekarza++).ToString(),
            Imie = imie,
            Nazwisko = nazwisko,
            Specjalizacja = specjalizacja,
            NumerTelefonu = telefon
        };
        _lekarze.Add(lekarz);
        Zapisz(GetOptions());
    }

    public Lekarz? ZnajdzLekarza(string id)
        => _lekarze.FirstOrDefault(l => l.IDlekarza == id);

    // Funkcja do rejestracji wizyt
    public void ZarejestrujWizyte(string peselPacjenta, string idLekarza, DateTime dataWizyty)
    {
        var pacjent = ZnajdzPacjenta(peselPacjenta)
            ?? throw new InvalidOperationException("Pacjent o podanym PESEL'u nie istnieje.");

        var lekarz = ZnajdzLekarza(idLekarza)
                     ?? throw new InvalidOperationException("Lekarz o podanym ID nie istnieje.");

        // Lekarz może mieć tylko jedną wizytę o tej samej godzinie
        bool czyLekarzZajety = _wizyty.Any(w =>
            w.Lekarz.IDlekarza == idLekarza &&
            w.DataWizyty == dataWizyty);

        if (!czyLekarzZajety) // Jesli nie jest zajety dodaj wizyte
        {
            var wizyta = new Wizyta
            {
                IDwizyty = (_ostatniIdWizyty++).ToString(),
                Pacjent = pacjent,
                Lekarz = lekarz,
                DataWizyty = dataWizyty
            };

            _wizyty.Add(wizyta);
            Zapisz(GetOptions());
        }
        else
            throw new InvalidOperationException(
                        "Lekarz ma już wizytę o tej godzinie.");
    }
    // Lista wszystkich wizyt danego pacjenta
    public IEnumerable<Wizyta> PobierzWizytyPacjenta(string pesel)
    {
        if (!_pacjenci.Any(p => p.PESEL == pesel))
            throw new InvalidOperationException("Pacjent o podanym PESEL'u nie istnieje.");

        return _wizyty
            .Where(w => w.Pacjent.PESEL == pesel)
            .OrderBy(w => w.DataWizyty);
    }
    #endregion
    
}
// DTO tylko dla serializacji danych
internal class PrzychodniaDto
{
    public List<Pacjent> Pacjenci { get; set; } = [];
    public List<Lekarz> Lekarze { get; set; } = [];
    public List<Wizyta> Wizyty { get; set; } = [];
}
