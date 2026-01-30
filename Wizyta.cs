namespace PrzychodniaApp
{
    public class Wizyta
    {
        public required string IDwizyty { get; init; }
        public required Pacjent Pacjent { get; init; }
        public required Lekarz Lekarz { get; init; }
        public required DateTime DataWizyty { get; init; }

        public string? Diagnoza { get; set; } // opcjonalne pole na diagnoze
        public string? Recepta { get; set; } // opcjonalne pole na recepte

        public Wizyta() { }
    }
}
