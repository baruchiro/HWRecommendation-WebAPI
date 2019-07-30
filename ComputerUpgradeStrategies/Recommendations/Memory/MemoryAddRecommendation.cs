namespace ComputerUpgradeStrategies.Recommendations.Memory
{
    class MemoryAddRecommendation : IUpgradeRecommendation
    {
        public MemoryAddRecommendation(string explanation)
        {
            Explanation = explanation;
        }

        public string Explanation { get; }
    }
}
