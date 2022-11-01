namespace AlcoholLimit.Pages
{
    public partial class StartDrink
    {
        private const string STARTING_TIME = "00:00:00";

        private bool hideList = true;
        private string drinkingStatus = "You have not started drinking yet.";
        private string buttonLabel = "Start drinking session";
        private double bloodAlcoholLevel = 0;
        private string elapsedTime = STARTING_TIME;

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {

        }

        protected override void OnInitialized()
        {
            startTimer();
        }

        private void startTimer()
        {

        }

        private void changeStatus()
        {
            if (drinkingStatus == "You have started drinking.")
            {
                hideList = true;
                drinkingStatus = "You have not started drinking yet.";
                buttonLabel = "Start drinking session";
            }
            else
            {
                hideList = false;
                drinkingStatus = "You have started drinking.";
                buttonLabel = "Stop drinking session";
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