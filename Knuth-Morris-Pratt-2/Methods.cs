using System.Diagnostics;

namespace Knuth_Morris_Pratt_2
{
    // Wynik pojedynczego pomiaru czasu wyszukiwania.
    internal sealed record Measurement(int TextLength, int PatternLength, int Matches, int Repeats, double TimeMs);

    // Logika aplikacji bez odwołań do interfejsu użytkownika.
    internal static class Methods
    {
        // pi[i] = długość najdłuższego właściwego prefikso-sufiksu pattern[0..i]
        public static int[] BuildPrefixTable(string pattern)
        {
            var pi = new int[pattern.Length];
            int k = 0;

            for (int i = 1; i < pattern.Length; i++)
            {
                while (k > 0 && pattern[i] != pattern[k])
                    k = pi[k - 1];

                if (pattern[i] == pattern[k])
                    k++;

                pi[i] = k;
            }

            return pi;
        }

        // Indeksy (od 0) wszystkich wystąpień wzorca w tekście, także nakładających się.
        public static List<int> Search(string text, string pattern)
        {
            var result = new List<int>();
            if (pattern.Length == 0 || pattern.Length > text.Length)
                return result;

            int[] pi = BuildPrefixTable(pattern);
            int j = 0;

            for (int i = 0; i < text.Length; i++)
            {
                while (j > 0 && text[i] != pattern[j])
                    j = pi[j - 1];

                if (text[i] == pattern[j])
                    j++;

                if (j == pattern.Length)
                {
                    result.Add(i - j + 1);
                    j = pi[j - 1];
                }
            }

            return result;
        }

        // Wyszukuje wzorzec i mierzy średni czas pojedynczego wywołania Search.
        public static Measurement MeasureSearch(string text, string pattern, out List<int> found)
        {
            // Pierwsze wywołanie: wynik i "rozgrzewka" (kompilacja JIT nie zafałszuje pomiaru)
            found = Search(text, pattern);

            int repeats = 0;
            long start = Stopwatch.GetTimestamp();
            TimeSpan elapsed;

            do
            {
                Search(text, pattern);
                repeats++;
                elapsed = Stopwatch.GetElapsedTime(start);
            }
            while (elapsed.TotalMilliseconds < 200);

            double avgMs = elapsed.TotalMilliseconds / repeats;
            return new Measurement(text.Length, pattern.Length, found.Count, repeats, avgMs);
        }

        // Indeks następnego wystąpienia (cyklicznie); -1, jeśli nie ma wystąpień.
        public static int NextMatchIndex(int current, int count)
        {
            return count == 0 ? -1 : (current + 1) % count;
        }
    }
}
