namespace PanicEngine.Physix
{
    public sealed class PhysixSettings
    {
        public int SolverIterations { get; set; } = 2;
        public float PositionCorrectionPercent { get; set; } = 0.8f;
        public float PositionCorrectionSlop { get; set; } = 0.01f;
        public float MaxSpeed { get; set; } = 50f;
    }
}