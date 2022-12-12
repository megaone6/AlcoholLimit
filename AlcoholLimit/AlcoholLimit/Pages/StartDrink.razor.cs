using AlcoholLimit.Data;
using Microsoft.AspNetCore.Components;
using Plugin.LocalNotification;
using System.Diagnostics;

namespace AlcoholLimit.Pages
{
    public partial class StartDrink
    {
        private const string DEFAULT_TIME = "00:00:00";

        private bool recalBac = false;
        private bool isAddItemMode = false;
        private double displayBac;
        private string drinkingStatus = "You have not started drinking yet.";
        private string elapsedTime = DEFAULT_TIME;
        private System.Timers.Timer timer;
        private System.Timers.Timer timer2;
        private DateTime currentTime;
        private double elapsedsomething = 0;
        private TimeSpan elapsedSpan;
        private List<DrinkItem> drinks = new List<DrinkItem>();
        private int selectedID;
        private double sumAlcoholGrams = 0;
        private List<ConsumedDrinkItem> consumedDrinkItems = new List<ConsumedDrinkItem>();

        private void startTimer(bool calledFromInit = false)
        {
            if (!calledFromInit)
            {
                AppState.startTime = DateTime.Now;
            }
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
            if (AppState.isDrinking)
            {
                AppState.isDrinking = false;
                AppState.consumedDrinks.Clear();
                drinkingStatus = "You have not started drinking yet.";
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
                AppState.isDrinking = true;
                drinkingStatus = "You have started drinking.";
                AppState.consumedDrinks = new List<DrinkItem>();
                startTimer();
            }
        }

        private async void addSelectedDrink()
        {
            DrinkItem selectedDrink = await DrinkDatabase.GetItemAsync(selectedID);

            addDrink(selectedDrink);
        }

        private async void addDrink(DrinkItem drinkItem)
        {
            sumAlcoholGrams += drinkItem.PureAlcGram;
            AppState.consumedDrinks.Add(drinkItem);
            ConsumedDrinkItem consumed = new ConsumedDrinkItem();
            consumed.DrinkItemID = drinkItem.ID;
            consumed.Date = DateTime.Now.Date.ToString("yyyy/MM/dd");
            consumedDrinkItems.Add(consumed);

            displayBac = Math.Round(currentBloodAlcohol(AppState.profile.Sex, AppState.profile.Weight, elapsedSpan.TotalHours, sumAlcoholGrams), 4);

            if (OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
                notifyOnHighBloodAlcohol(AppState.profile.bloodThreshold);
            StateHasChanged();
        }

        private void ToggleAddItem()
        {
            isAddItemMode = !isAddItemMode;
        }

        private async Task AddItem(DrinkItem item)
        {
            await DrinkDatabase.SaveItemAsync(item);
            addDrink(item);
            drinks.Add(item);
            isAddItemMode = false;
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
            sumAlcoholGrams -= drink.PureAlcGram;
            displayBac = Math.Max(0, Math.Round(currentBloodAlcohol(AppState.profile.Sex, AppState.profile.Weight, elapsedSpan.TotalHours, sumAlcoholGrams), 4));
            consumedDrinkItems.RemoveAt(AppState.consumedDrinks.IndexOf(drink));
            AppState.consumedDrinks.Remove(drink);
        }

        protected override async void OnInitialized()
        {
            if (AppState.isDrinking)
            {
                recalBac = true;
                startTimer(true);

                sumAlcoholGrams = 0;
                foreach (var di in AppState.consumedDrinks)
                {
                    sumAlcoholGrams += di.PureAlcGram;
                }
            }

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
                elapsedTime = $"{currentTime.Subtract(AppState.startTime)}".Substring(0, 8);
                long elapsedTicks = currentTime.Ticks - AppState.startTime.Ticks;
                elapsedSpan = new TimeSpan(elapsedTicks);
                elapsedsomething = elapsedSpan.TotalMinutes;

                if (recalBac)
                {
                    displayBac = Math.Max(0, Math.Round(currentBloodAlcohol(AppState.profile.Sex, AppState.profile.Weight, elapsedSpan.TotalHours, sumAlcoholGrams), 4));
                    recalBac = false;
                }

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