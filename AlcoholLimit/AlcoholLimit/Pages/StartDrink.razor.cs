namespace AlcoholLimit.Pages
{
    public partial class StartDrink
    {
        private const string STARTING_TIME = "00:00:00";

        private bool hideList = true;
        private bool isDrinking = false;
        private string drinkingStatus = "You have not started drinking yet.";
        private string buttonLabel = "Start drinking session";
        private double bloodAlcoholLevel = 0;
        private string elapsedTime = STARTING_TIME;

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {

        }

        protected override void OnInitialized()
        {
            startTimer(); //timer increment felorankent
        }

        private void startTimer()
        {

        }

        private void changeStatus()
        {
            if (isDrinking)
            {
                isDrinking = false;
                hideList = true;
                drinkingStatus = "You have not started drinking yet.";
                buttonLabel = "Start drinking session";
            }
            else
            {
                isDrinking = true;
                hideList = false;
                drinkingStatus = "You have started drinking.";
                buttonLabel = "Stop drinking session";
            }
        }

        private void addFoodOrDrink()
        {
            //
        }

        private void changeBloodAlcLevel()
        {
            bloodAlcoholLevel = 0.1;
        }
    }
}
