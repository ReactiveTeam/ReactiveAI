namespace ReactiveAI.Intelligence.Evaluators.Interfaces
{
    public interface IEvaluator
    {
        Pointf PtA { get; }

        Pointf PtB { get; }

        float MinX { get; }
        float MaxX { get; }
        float MinY { get; }
        float MaxY { get; }
        
        Interval<float> YInterval { get; }
        Interval<float> XInterval { get; }

        bool isInverted { get; set; }
        float Evaluate(float x);
    }
}
