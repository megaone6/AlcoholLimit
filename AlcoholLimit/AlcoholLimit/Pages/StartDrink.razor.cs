using AlcoholLimit.Data;
using Microsoft.AspNetCore.Components;
using Plugin.LocalNotification;
using System.Diagnostics;

namespace AlcoholLimit.Pages
{
    public partial class StartDrink
    {
        private const string DEFAULT_TIME = "00:00:00";

        private double displayBac;
        private bool hideComponents = true;
        private bool isDrinking = false;
        private string drinkingStatus = "You have not started drinking yet.";
        private string buttonLabel = "Start drinking session";
        private string elapsedTime = DEFAULT_TIME;
        private System.Timers.Timer timer;
        private System.Timers.Timer timer2;
        private DateTime startTime = DateTime.Now;
        private DateTime currentTime;
        private double elapsedsomething = 0;
        private TimeSpan elapsedSpan;
        private List<DrinkItem> drinks = new List<DrinkItem>();
        private DrinkItem selectedDrink = new DrinkItem();
        private int selectedID;
        private double sumAlcoholGrams = 0;
        private List<DrinkItem> consumedDrinks = new List<DrinkItem>();
        private List<ConsumedDrinkItem> consumedDrinkItems = new List<ConsumedDrinkItem>();

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
            displayBac = 0;
            timer2.Start();
        }

        private void stopTimer()
        {
            timer.Enabled = false;
            timer2.Enabled = false;
            elapsedTime = DEFAULT_TIME;
        }

        private async void changeStatus()
        {
            if (isDrinking)
            {
                isDrinking = false;
                hideComponents = true;
                drinkingStatus = "You have not started drinking yet.";
                buttonLabel = "Start drinking session";
                displayBac = 0;
                sumAlcoholGrams = 0;
                foreach (ConsumedDrinkItem consumed in consumedDrinkItems)
                {
                    await ConsumedDrinkDatabase.SaveItemAsync(consumed);
                    StateHasChanged();
                }
                stopTimer();
            }
            else
            {
                isDrinking = true;
                hideComponents = false;
                drinkingStatus = "You have started drinking.";
                buttonLabel = "Stop drinking session";
                consumedDrinks = new List<DrinkItem>();
                startTimer();
            }
        }

        private async void addDrink()
        {
            selectedDrink = await DrinkDatabase.GetItemAsync(selectedID);
            sumAlcoholGrams += selectedDrink.PureAlcGram;
            consumedDrinks.Add(selectedDrink);
            ConsumedDrinkItem consumed = new ConsumedDrinkItem();
            consumed.DrinkItemID = selectedID;
            consumed.Date = DateTime.Now.Date.ToString("yyyy/MM/dd");
            consumedDrinkItems.Add(consumed);

            displayBac = Math.Round(currentBloodAlcohol(AppState.profile.Sex, AppState.profile.Weight, elapsedSpan.TotalHours, sumAlcoholGrams), 4);

            if (OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
                notifyOnHighBloodAlcohol(AppState.profile.bloodThreshold);
            StateHasChanged();
        }

        private void notifyOnHighBloodAlcohol(float threshold)
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

        private double currentBloodAlcohol(sex enumerator, int weight, double time, double alcoholInGrams)
        {
            double alcoholMetabolization;
            double r; //gender constant
            if (enumerator == sex.MAN)
            {
                r = 0.68;
                alcoholMetabolization = 0.015;
            }
            else
            {
                r = 0.55;
                alcoholMetabolization = 0.017;
            }

            double bloodAlcoholLevel = ((alcoholInGrams / 1000) / (r * weight)) * 100 - (alcoholMetabolization * time);
            return bloodAlcoholLevel;
        }

        private void setID(ChangeEventArgs e)
        {
            selectedID = int.Parse((string)e.Value);
        }

        private void deleteItem(DrinkItem drink)
        {
            sumAlcoholGrams -= selectedDrink.PureAlcGram;
            displayBac = Math.Max(0, Math.Round(currentBloodAlcohol(AppState.profile.Sex, AppState.profile.Weight, elapsedSpan.TotalHours, sumAlcoholGrams), 4));
            consumedDrinkItems.RemoveAt(consumedDrinks.IndexOf(drink));
            consumedDrinks.Remove(drink);
        }

        protected override async void OnInitialized()
        {
            drinks = await DrinkDatabase.GetItemsAsync();
            if(drinks.Count > 0)
            {
            selectedID = drinks[0].ID;
            }
            StateHasChanged();
        }

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
                displayBac = Math.Round(currentBloodAlcohol(AppState.profile.Sex, AppState.profile.Weight, elapsedSpan.TotalHours, sumAlcoholGrams), 4);

                StateHasChanged();
            });
        }
    }
}