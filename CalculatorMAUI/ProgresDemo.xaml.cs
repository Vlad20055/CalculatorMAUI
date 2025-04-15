using CalculationLibrary;
using System.Threading;

namespace CalculatorMAUI
{
    public partial class ProgresDemo : ContentPage
    {
        private IntegralSolver _solver = new IntegralSolver();
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private double _a = 0;
        private double _b = 1;
        private double _step = 0.000001;

        public ProgresDemo()
        {
            InitializeComponent();
            SetupSolverEvents();
        }

        private void SetupSolverEvents()
        {
            _solver.ProgressChanged += progress =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    progressBar.Progress = progress / 100;
                    progressLabel.Text = $"{progress:0.00}%";
                });
            };

            _solver.SolvingCompleted += (time) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    startButton.IsEnabled = true;
                    cancelButton.IsEnabled = false;
                    headerLabel.Text = "Завершено успешно";
                });
            };

            _solver.SolvingCanceled += () =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    headerLabel.Text = "Задание отменено";
                    startButton.IsEnabled = true;
                    cancelButton.IsEnabled = false;
                });
            };
        }

        private async void startButton_Clicked(object sender, EventArgs e)
        {
            this.headerLabel.Text = "Вычисление...";
            startButton.IsEnabled = false;
            cancelButton.IsEnabled = true;

            _cts = new CancellationTokenSource();

            try
            {
                double result = await _solver.SolveIntegralAsync(
                    x => Math.Sin(x),
                    _a,
                    _b,
                    _cts.Token,
                    _step
                    );
            }
            catch (OperationCanceledException)
            {
                _solver.CancelSolving();
            }
            finally
            {
                _cts.Dispose();
            }
        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            _cts?.Cancel();
        }
    }
}