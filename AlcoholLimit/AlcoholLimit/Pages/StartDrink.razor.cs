using Plugin.LocalNotification;

namespace AlcoholLimit.Pages
{
    public partial class StartDrink
    {
        private const string DEFAULT_TIME = "00:00:00";

        private bool hideList = true;
        private bool isDrinking = false;
        private string drinkingStatus = "You have not started drinking yet.";
        private string buttonLabel = "Start drinking session";
        private string elapsedTime = DEFAULT_TIME;
        private System.Timers.Timer timer;
        private System.Timers.Timer timer2;
        private DateTime startTime = DateTime.Now;
        private DateTime currentTime;
        private double elapsedsomething = 0;
        TimeSpan elapsedSpan;

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            InvokeAsync(() =>
            {
                currentTime = e.SignalTime;  //.Addminutes(minute) for testing
                elapsedTime = $"{currentTime.Subtract(startTime)}".Substring(0, 8);
                long elapsedTicks = currentTime.Ticks - startTime.Ticks;
                elapsedSpan = new TimeSpan(elapsedTicks);
                elapsedsomething = elapsedSpan.TotalMinutes;

                StateHasChanged();
            });
            
        }
        private void OnTimedEvent2(Object source, System.Timers.ElapsedEventArgs e)
        {
            InvokeAsync(() =>
            {
                displayBac = currentBloodAlcohol(true, 80, elapsedSpan.TotalHours, 0.014, 2);

                StateHasChanged();
            });
            notifyOnHighBloodAlcohol(0.05); // only here for testing purposes at the moment
        }
        private void startTimer()
        {
            startTime = DateTime.Now;
            timer = new System.Timers.Timer(1);
            timer2 = new System.Timers.Timer(1800000);
            timer.Elapsed += OnTimedEvent;
            timer2.Elapsed += OnTimedEvent2;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer2.AutoReset = true;
            timer2.Enabled = true;
            displayBac = currentBloodAlcohol(true, 80, elapsedSpan.TotalHours, 0.014, 2);
            timer2.Start();
        }

        private void stopTimer()
        {
            timer.Enabled = false;
            timer2.Enabled = false;
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
            // TODO: Implement adding drink to current session and notification when reaching the threshold
        }

        private void notifyOnHighBloodAlcohol(double threshold)
        {
            if (displayBac >= threshold)
            {
                var request = new NotificationRequest
                {
                    NotificationId = 0,
                    Title = "Warning!",
                    Subtitle = "Threshold reached!",
                    Description = "You have reached your blood alcohol threshold. Maybe you should take a break?",
                    CategoryType = NotificationCategoryType.Alarm
                };
                LocalNotificationCenter.Current.Show(request);
            }
        }

        public double displayBac;
        public double currentBloodAlcohol(bool sex, int weight, double time, double alcoholInGrams, int numberOfDrinks)
        {
            double alcoholMetabolization;
            double r; //gender constant
            if (sex == true)
            {
                r = 0.68;
                alcoholMetabolization = 0.015;
            }
            else
            {
                r = 0.55;
                alcoholMetabolization = 0.017;
            }

            double bloodAlcoholLevel = (numberOfDrinks * alcoholInGrams / (r * weight)) * 100 - (alcoholMetabolization * time);
            return bloodAlcoholLevel;
        }
    }
}