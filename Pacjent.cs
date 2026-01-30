namespace PrzychodniaApp
{
    public class Pacjent
    {
        public required string Imie { get; init; }
        public required string Nazwisko { get; init; }
        public required DateOnly DataUrodzenia { get; init; } // potrzebna tylko data bez godziny
        public required string Adres { get; init; }
        public required string NumerTelefonu { get; init; }
        public required string PESEL { get; init; }

        public Pacjent() { }
    }
}
