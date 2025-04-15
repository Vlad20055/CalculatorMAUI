using System.Diagnostics;

namespace CalculationLibrary
{
    public class IntegralSolver
    {
        private Stopwatch _stopwatch = new Stopwatch();
        public delegate double Function(double x);
        public event Action<double>? ProgressChanged;
        public event Action<double>? SolvingCompleted;
        public event Action? SolvingCanceled;

        public void CancelSolving()
        {
            SolvingCanceled?.Invoke();
            return;
        }

        public async Task<double> SolveIntegralAsync(Function f, double a, double b,
                                                    CancellationToken token, double step = 0.000001)
        {
            int updateInterval = 1000;
            double result = 0.0;
            double curPoint = a;
            long totalSteps = (long)((b - a) / step);
            long stepsCompleted = 0;

            _stopwatch.Restart();

            while (curPoint <= b)
            {
                token.ThrowIfCancellationRequested();

                result += f(curPoint) * step;
                curPoint += step;
                stepsCompleted++;

                if (stepsCompleted % updateInterval == 0)
                {
                    double progress = ((curPoint - a) / (b - a)) * 100;
                    ProgressChanged?.Invoke(progress);
                    await Task.Delay(10, token);
                }
            }

            _stopwatch.Stop();
            SolvingCompleted?.Invoke(_stopwatch.Elapsed.TotalSeconds);
            return result;
        }
    }
}
