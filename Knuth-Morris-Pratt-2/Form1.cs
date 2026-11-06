namespace Knuth_Morris_Pratt_2
{
    // Obsługa interfejsu: zdarzenia kontrolek i wyświetlanie wyników.
    // Cała logika wyszukiwania znajduje się w klasie Methods.
    public partial class Form1 : Form
    {
        private List<int> matches = new();
        private int currentMatch = -1;
        private int patternLength;
        private readonly List<Measurement> measurements = new();
        private Measurement? lastMeasurement;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // ---------- Zdarzenia ----------

        private void buttonInputFile_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            RunSearch();
        }

        private void buttonOccurence_Click(object sender, EventArgs e)
        {
            GoToNext();
        }

        private void buttonInputText_Click(object sender, EventArgs e)
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

        // ---------- Operacje na interfejsie ----------

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

            Measurement m = Methods.MeasureSearch(text, pattern, out matches);
            measurements.Add(m);
            lastMeasurement = m;
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
            int next = Methods.NextMatchIndex(currentMatch, matches.Count);
            if (next < 0)
                return;

            currentMatch = next;
            int pos = matches[currentMatch];

            richTextBoxContent.Select(pos, patternLength);
            richTextBoxContent.ScrollToCaret();
            richTextBoxContent.Focus();

            string time = lastMeasurement is null
                ? ""
                : $" | Czas: {lastMeasurement.TimeMs:F4} ms ({lastMeasurement.TextLength} znaków)";

            labelSummary.Text = $"Wystąpienie {currentMatch + 1} z {matches.Count} (pozycja {pos + 1}){time}";
        }

        private void ClearResults()
        {
            matches = new List<int>();
            currentMatch = -1;
            lastMeasurement = null;

            richTextBoxContent.SelectAll();
            richTextBoxContent.SelectionBackColor = richTextBoxContent.BackColor;
            richTextBoxContent.Select(0, 0);
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
    }
}
