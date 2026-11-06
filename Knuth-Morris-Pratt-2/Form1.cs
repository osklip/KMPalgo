using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

namespace Knuth_Morris_Pratt_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<int> matches = new();
        private int currentMatch = -1;
        private int patternLength;
        private sealed record Measurement(int TextLength, int PatternLength, int Matches, int Repeats, double TimeMs);

        private readonly List<Measurement> measurements = new();

        private void buttonInputFile_Click(object sender, EventArgs e)
        {
            //Methods.OpenFile();
            OpenFile();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Methods.RunSearch(wzorzec, tekst);
            RunSearch();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Methods.GoToNext();
            GoToNext();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string? text = PromptForText(
        "Wprowadź tekst",
        "Wpisz lub wklej tekst do przeszukania:",
        richTextBoxContent.Text);

            if (text == null)
                return; // użytkownik anulował

            richTextBoxContent.Text = text;
            ClearResults();
            labelSummary.Text = $"Wprowadzono tekst ({text.Length} znaków).";
        }

        private void OpenFile()
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Pliki tekstowe (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                richTextBoxContent.Text = File.ReadAllText(dialog.FileName);
                ClearResults();
                labelSummary.Text = $"Wczytano: {dialog.FileName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Błąd odczytu pliku",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RunSearch()
        {
            ClearResults();

            string pattern = textBoxPattern.Text;
            if (pattern.Length == 0)
            {
                labelSummary.Text = "Wpisz wzorzec.";
                return;
            }

            string text = richTextBoxContent.Text;

            if (checkBoxIgnoreCase.Checked)
            {
                text = text.ToLowerInvariant();
                pattern = pattern.ToLowerInvariant();
            }

            Measurement m = MeasureSearch(text, pattern, out matches);
            measurements.Add(m);
            patternLength = pattern.Length;

            // Podświetlanie (poza pomiarem)
            foreach (int pos in matches)
            {
                richTextBoxContent.Select(pos, patternLength);
                richTextBoxContent.SelectionBackColor = Color.FromArgb(255, 236, 153);
            }
            richTextBoxContent.Select(0, 0);

            if (matches.Count == 0)
            {
                labelSummary.Text = $"Wzorzec nie występuje w tekście. Czas: {m.TimeMs:F4} ms ({m.TextLength} znaków)";
                return;
            }

            labelSummary.Text = $"Liczba wystąpień: {matches.Count} | Czas: {m.TimeMs:F4} ms ({m.TextLength} znaków)";
            GoToNext();
        }

        private void GoToNext()
        {
            if (matches.Count == 0)
                return;

            currentMatch = (currentMatch + 1) % matches.Count;
            int pos = matches[currentMatch];

            richTextBoxContent.Select(pos, patternLength);
            richTextBoxContent.ScrollToCaret();
            richTextBoxContent.Focus();

            labelSummary.Text = $"Wystąpienie {currentMatch + 1} z {matches.Count} (pozycja {pos + 1})";
        }

        private void ClearResults()
        {
            matches = new List<int>();
            currentMatch = -1;

            richTextBoxContent.SelectAll();
            richTextBoxContent.SelectionBackColor = richTextBoxContent.BackColor;
            richTextBoxContent.Select(0, 0);
        }

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

        private string? PromptForText(string title, string label, string initialText = "")
        {
            using var dlg = new Form
            {
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.Sizable,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false,
                ClientSize = new Size(520, 360),
                MinimumSize = new Size(360, 240),
                Padding = new Padding(12),
                Font = Font
            };

            var box = new TextBox
            {
                Multiline = true,
                AcceptsReturn = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 11f),
                Text = initialText
            };

            var lbl = new Label
            {
                Text = label,
                Dock = DockStyle.Top,
                Height = 28,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 48,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 8, 0, 0)
            };

            var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, Width = 90, Height = 32 };
            var cancel = new Button { Text = "Anuluj", DialogResult = DialogResult.Cancel, Width = 90, Height = 32 };
            buttons.Controls.Add(ok);
            buttons.Controls.Add(cancel);

            dlg.Controls.Add(box);
            dlg.Controls.Add(lbl);
            dlg.Controls.Add(buttons);

            dlg.CancelButton = cancel;

            dlg.Shown += (s, e) =>
            {
                box.Focus();
                box.SelectionStart = box.TextLength;
            };

            return dlg.ShowDialog(this) == DialogResult.OK ? box.Text : null;
        }

        private Measurement MeasureSearch(string text, string pattern, out List<int> found)
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
