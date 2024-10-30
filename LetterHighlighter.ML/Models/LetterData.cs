using Microsoft.ML.Data;

namespace ClassLibrary1LetterHighlighter.ML.Models
{
    public class LetterData
    {
        [LoadColumn(0)]
        public string Text { get; set; }
        
        [LoadColumn(1)]
        public int Position { get; set; }
        
        [LoadColumn(2)]
        public bool ShouldHighlight { get; set; }
    }
}