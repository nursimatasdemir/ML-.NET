// Services/VstoHighlighter.cs

using Microsoft.Office.Interop.Word;
using ClassLibrary1LetterHighlighter.ML.Services;
using Microsoft.ML.Data;

namespace LetterHighlighter.Core.Services
{
    public class VstoHighlighter
    {
        private readonly ModelTrainer _modelTrainer;
        private readonly Application _wordApp;

        public VstoHighlighter(Application wordApp)
        {
            _modelTrainer = new ModelTrainer();
            _wordApp = wordApp;
        }

        public void TrainNewModel()
        {
            _modelTrainer.TrainModel();
        }

        public void HighlightText(Microsoft.Office.Interop.Word.Range range)
        {
            if (range == null || string.IsNullOrEmpty(range.Text))
                return;

            string text = range.Text;
            
            for (int i = 0; i < text.Length; i++)
            {
                var prediction = _modelTrainer.PredictLetter(text, i);
                
                if (prediction.ShouldHighlight && prediction.Probability > 0.5)
                {
                    Microsoft.Office.Interop.Word.Range letterRange = range.Characters[i + 1];
                    letterRange.Font.Color = WdColor.wdColorRed;
                }
            }
        }

        public void ProcessDocument()
        {
            Document doc = _wordApp.ActiveDocument;
            HighlightText(doc.Content);
        }
    }
}