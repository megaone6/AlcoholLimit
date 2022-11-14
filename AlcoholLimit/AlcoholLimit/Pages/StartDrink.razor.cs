namespace AlcoholLimit.Pages
{
    public partial class StartDrink
    {
        private const string DEFAULT_TIME = "00:00:00";

        private bool hideList = true;
        private bool isDrinking = false;
        private string drinkingStatus = "You have not started drinking yet.";
        private string buttonLabel = "Start drinking session";
        private double bloodAlcoholLevel = 0;
        private string elapsedTime = DEFAULT_TIME;
        private System.Timers.Timer timer = new System.Timers.Timer(1);
        private DateTime startTime = DateTime.Now;
        private DateTime currentTime;

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            InvokeAsync(() =>
            {
                currentTime = e.SignalTime;
                elapsedTime = $"{currentTime.Subtract(startTime)}".Substring(0, 8);
                StateHasChanged();
            });
        }

        private void startTimer()
        {
            startTime = DateTime.Now;
            timer = new System.Timers.Timer(1);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void stopTimer()
        {
            timer.Enabled = false;
            elapsedTime = DEFAULT_TIME;
        }

        private void changeStatus()
        {
            if (isDrinking)
            {
                isDrinking = false;
                hideList = true;
                drinkingStatus = "You have not started drinking yet.";
                buttonLabel = "Start drinking session";
                stopTimer();
            }
            else
            {
                isDrinking = true;
                hideList = false;
                drinkingStatus = "You have started drinking.";
                buttonLabel = "Stop drinking session";
                startTimer();
            }
        }

        private void addFoodOrDrink()
        {

        }

        private void changeBloodAlcLevel()
        {
            bloodAlcoholLevel = 0.1;
        }
    }
}
