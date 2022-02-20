using DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNP_main.Helpers
{
    public enum Prices { Breakfast = 49, Lunch = 72, Dinner = 59 };

    public static class Helpers
    {
        public static Menu.DaysInWeek GetCurrentDay()
        {
            int currentDayIndex = (int)DateTime.Now.DayOfWeek;
            int dayIndex = ((currentDayIndex - 1) % 7 + 7) % 7;

            return (Menu.DaysInWeek)dayIndex;
        }

        public static int CalculatePrice(bool hasBreakfast, bool hasLunch, bool hasDinner) 
            => (hasBreakfast    ? (int)Prices.Breakfast : 0)
             + (hasDinner       ? (int)Prices.Dinner    : 0)
             + (hasLunch        ? (int)Prices.Lunch     : 0);

        public static void SetString(this ISession session, string key, string value) 
            => session.Set(key, Encoding.UTF8.GetBytes(value));

        public static string GetString(this ISession session, string key)
            => session.TryGetValue(key, out var value) ? Encoding.UTF8.GetString(value) : null;
    }
}
