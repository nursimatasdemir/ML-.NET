using Microsoft.ML.Data;

namespace ClassLibrary1LetterHighlighter.ML.Models
{
    public class LetterPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool ShouldHighlight { get; set; }

        public float Probability { get; set; }
    }
}