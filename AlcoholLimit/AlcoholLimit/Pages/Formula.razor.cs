using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using AlcoholLimit;
using AlcoholLimit.Shared;

namespace AlcoholLimit.Pages
{
    public partial class Formula
    {
        public double bloodAlcohol = 0; //Widmark formula anyad
        //private bool isMale = true;
        // BAC = [Alcohol consumed in grams / (Body weight in grams x r)] x 100.
        // 80 kg male drinking 2 drinks of 14 grams (0.014 kg) each, in two hours:
        // (2 * 0.014) / (0.68 * 80) * 100 - (0.015*2) ~ 0.021%
        void currentBloodAlcohol(bool sex, int weight, double time, double alcoholInGrams, int numberOfDrinks)
        {
            double bac;
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

            bac = (numberOfDrinks * alcoholInGrams / (r * weight)) * 100 - (alcoholMetabolization * time);
            bloodAlcohol = bac;
        }
    }
}