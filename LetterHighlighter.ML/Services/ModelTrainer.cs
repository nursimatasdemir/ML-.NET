// Services/ModelTrainer.cs

using ClassLibrary1LetterHighlighter.ML.Models;
using Microsoft.ML;

namespace ClassLibrary1LetterHighlighter.ML.Services
{
    public class ModelTrainer
    {
        private readonly MLContext _mlContext;
        private ITransformer _trainedModel;

        public ModelTrainer()
        {
            _mlContext = new MLContext(seed: 0);
        }

        public void TrainModel()
        {
            var trainingData = GenerateTrainingData();
            IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", "Text")
                .Append(_mlContext.Transforms.Concatenate("Features", "Features", "Position"))
                .Append(_mlContext.BinaryClassification.Trainers.FastTree(numberOfLeaves: 2, numberOfTrees: 50));

            _trainedModel = pipeline.Fit(dataView);
            SaveModel();
        }

        private List<LetterData> GenerateTrainingData()
        {
            var trainingData = new List<LetterData>();
            string[] sampleTexts = {
                "merhaba dünya",
                "ankara",
                "istanbul",
                "izmir",
                "adana",
                "antalya",
                "konya"
            };

            foreach (string text in sampleTexts)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    trainingData.Add(new LetterData
                    {
                        Text = text,
                        Position = i,
                        ShouldHighlight = text[i] == 'a'
                    });
                }
            }

            return trainingData;
        }

        public void SaveModel()
        {
            _mlContext.Model.Save(_trainedModel, null, "LetterHighlighter.zip");
        }

        public LetterPrediction PredictLetter(string text, int position)
        {
            var predictor = _mlContext.Model.CreatePredictionEngine<LetterData, LetterPrediction>(_trainedModel);
            return predictor.Predict(new LetterData 
            { 
                Text = text, 
                Position = position 
            });
        }
    }
}