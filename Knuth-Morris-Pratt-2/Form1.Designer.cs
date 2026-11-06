namespace Knuth_Morris_Pratt_2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonInputFile = new Button();
            buttonSearch = new Button();
            buttonOccurence = new Button();
            richTextBoxContent = new RichTextBox();
            textBoxPattern = new TextBox();
            label1 = new Label();
            checkBoxIgnoreCase = new CheckBox();
            labelSummary = new Label();
            label3 = new Label();
            buttonInputText = new Button();
            SuspendLayout();
            // 
            // buttonInputFile
            // 
            buttonInputFile.Font = new Font("Segoe UI", 10F);
            buttonInputFile.Location = new Point(648, 12);
            buttonInputFile.Name = "buttonInputFile";
            buttonInputFile.Size = new Size(140, 41);
            buttonInputFile.TabIndex = 0;
            buttonInputFile.Text = "Wczytaj plik .txt";
            buttonInputFile.UseVisualStyleBackColor = true;
            buttonInputFile.Click += buttonInputFile_Click;
            // 
            // buttonSearch
            // 
            buttonSearch.Font = new Font("Segoe UI", 10F);
            buttonSearch.Location = new Point(270, 74);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new Size(142, 41);
            buttonSearch.TabIndex = 1;
            buttonSearch.Text = "Szukaj";
            buttonSearch.UseVisualStyleBackColor = true;
            buttonSearch.Click += buttonSearch_Click;
            // 
            // buttonOccurence
            // 
            buttonOccurence.Font = new Font("Segoe UI", 10F);
            buttonOccurence.Location = new Point(98, 74);
            buttonOccurence.Name = "buttonOccurence";
            buttonOccurence.Size = new Size(140, 41);
            buttonOccurence.TabIndex = 2;
            buttonOccurence.Text = "Następne";
            buttonOccurence.UseVisualStyleBackColor = true;
            buttonOccurence.Click += buttonOccurence_Click;
            // 
            // richTextBoxContent
            // 
            richTextBoxContent.Location = new Point(12, 121);
            richTextBoxContent.Name = "richTextBoxContent";
            richTextBoxContent.Size = new Size(776, 302);
            richTextBoxContent.TabIndex = 3;
            richTextBoxContent.Text = "";
            // 
            // textBoxPattern
            // 
            textBoxPattern.Location = new Point(98, 19);
            textBoxPattern.Name = "textBoxPattern";
            textBoxPattern.Size = new Size(314, 23);
            textBoxPattern.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(17, 19);
            label1.Name = "label1";
            label1.Size = new Size(63, 19);
            label1.TabIndex = 5;
            label1.Text = "Wzorzec:";
            // 
            // checkBoxIgnoreCase
            // 
            checkBoxIgnoreCase.AutoSize = true;
            checkBoxIgnoreCase.Font = new Font("Segoe UI", 10F);
            checkBoxIgnoreCase.Location = new Point(450, 19);
            checkBoxIgnoreCase.Name = "checkBoxIgnoreCase";
            checkBoxIgnoreCase.Size = new Size(153, 23);
            checkBoxIgnoreCase.TabIndex = 6;
            checkBoxIgnoreCase.Text = "Ignoruj wielkość liter";
            checkBoxIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // labelSummary
            // 
            labelSummary.AutoSize = true;
            labelSummary.Font = new Font("Segoe UI", 10F);
            labelSummary.Location = new Point(68, 426);
            labelSummary.Name = "labelSummary";
            labelSummary.Size = new Size(12, 19);
            labelSummary.TabIndex = 7;
            labelSummary.Text = ".";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(12, 426);
            label3.Name = "label3";
            label3.Size = new Size(50, 19);
            label3.TabIndex = 8;
            label3.Text = "Wynik:";
            // 
            // buttonInputText
            // 
            buttonInputText.Font = new Font("Segoe UI", 10F);
            buttonInputText.Location = new Point(648, 74);
            buttonInputText.Name = "buttonInputText";
            buttonInputText.RightToLeft = RightToLeft.No;
            buttonInputText.Size = new Size(140, 41);
            buttonInputText.TabIndex = 9;
            buttonInputText.Text = "Wpisz własny tekst";
            buttonInputText.UseVisualStyleBackColor = true;
            buttonInputText.Click += buttonInputText_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonInputText);
            Controls.Add(label3);
            Controls.Add(labelSummary);
            Controls.Add(checkBoxIgnoreCase);
            Controls.Add(label1);
            Controls.Add(textBoxPattern);
            Controls.Add(richTextBoxContent);
            Controls.Add(buttonOccurence);
            Controls.Add(buttonSearch);
            Controls.Add(buttonInputFile);
            Name = "Form1";
            Text = "Knuth-Morris-Pratt";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonInputFile;
        private Button buttonSearch;
        private Button buttonOccurence;
        private RichTextBox richTextBoxContent;
        private TextBox textBoxPattern;
        private Label label1;
        private CheckBox checkBoxIgnoreCase;
        private Label labelSummary;
        private Label label3;
        private Button buttonInputText;
    }
}
