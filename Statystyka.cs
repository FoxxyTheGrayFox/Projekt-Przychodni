using ScottPlot;
using System.Globalization;

namespace PrzychodniaApp;

public static class Statystyka
{
    public static void GenerujWykres(
        IEnumerable<Wizyta> wizyty,
        string Plik)
    {
        var dane = wizyty
            .GroupBy(w => new { w.DataWizyty.Year, w.DataWizyty.Month })
            .OrderBy(g => g.Key.Year)
            .ThenBy(g => g.Key.Month)
            .Select(g => new
            {
                Miesiac = new DateTime(g.Key.Year, g.Key.Month, 1),
                Ilosc = g.Count()
            })
            .ToList();

        if (!dane.Any())
            return;

        double[] values = dane.Select(d => (double)d.Ilosc).ToArray();
        string[] labels = dane
            .Select(d => d.Miesiac.ToString("MM/yyyy", CultureInfo.InvariantCulture))
            .ToArray();

        var plot = new Plot();

        var bars = plot.Add.Bars(values);

        plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
            Enumerable.Range(0, labels.Length).Select(i => (double)i).ToArray(),
            labels);

        plot.Title("Liczba wizyt na miesiąc");
        plot.YLabel("Ilość wizyt");
        plot.XLabel("Miesiąc");

        var image = plot.GetImage(800, 450);
        image.Save(Plik);
    }
}
