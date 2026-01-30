namespace PrzychodniaApp
{
    public class Lekarz
    {
        public required string IDlekarza { get; init; }
        public required string Imie { get; init; }
        public required string Nazwisko { get; init; }
        public required string Specjalizacja { get; init; }
        public required string NumerTelefonu { get; init; }

        public Lekarz() { }
    }
}
