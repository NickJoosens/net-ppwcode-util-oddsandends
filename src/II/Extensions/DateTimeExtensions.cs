﻿// Copyright 2014 by PeopleWare n.v..
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PPWCode.Util.OddsAndEnds.II.Extensions
{
    public static class DateTimeExtensions
    {
        [Pure]
        public static DateTime StripMilliseconds(this DateTime dt)
        {
            Contract.Ensures(Contract.Result<DateTime>() == new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second));

            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        [Pure]
        public static DateTime StripSeconds(this DateTime dt)
        {
            Contract.Ensures(Contract.Result<DateTime>() == new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0));

            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        }

        [Pure]
        public static DateTime StripHours(this DateTime dt)
        {
            Contract.Ensures(Contract.Result<DateTime>() == new DateTime(dt.Year, dt.Month, dt.Day));

            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        [Pure]
        public static bool IsLegalSqlDate(this DateTime dt)
        {
            Contract.Ensures(Contract.Result<bool>() == (SqlDateTime.MinValue.Value <= dt && dt <= SqlDateTime.MaxValue.Value));

            return SqlDateTime.MinValue.Value <= dt && dt <= SqlDateTime.MaxValue.Value;
        }

        [Pure]
        public static DateTime AddQuarters(this DateTime dt, int quarters)
        {
            Contract.Ensures(Contract.Result<DateTime>() == dt.AddMonths(3 * quarters));

            return dt.AddMonths(3 * quarters);
        }

        [Pure]
        public static DateTime? StripMilliseconds(this DateTime? dt)
        {
            Contract.Ensures(Contract.Result<DateTime?>() == (!dt.HasValue ? dt : dt.StripMilliseconds()));

            return !dt.HasValue ? dt : dt.Value.StripMilliseconds();
        }

        [Pure]
        public static DateTime? StripSeconds(this DateTime? dt)
        {
            Contract.Ensures(Contract.Result<DateTime?>() == (!dt.HasValue ? dt : dt.StripSeconds()));

            return !dt.HasValue ? dt : dt.Value.StripSeconds();
        }

        [Pure]
        public static DateTime? StripHours(this DateTime? dt)
        {
            Contract.Ensures(Contract.Result<DateTime?>() == (!dt.HasValue ? dt : dt.StripHours()));

            return !dt.HasValue ? dt : dt.Value.StripHours();
        }

        [Pure]
        public static bool IsLegalSqlDate(this DateTime? dt)
        {
            Contract.Ensures(Contract.Result<bool>() == (dt.HasValue ? dt.Value.IsLegalSqlDate() : true));

            return dt.HasValue ? dt.Value.IsLegalSqlDate() : true;
        }

        [Pure]
        public static DateTime? AddMonths(this DateTime? dt, int months)
        {
            Contract.Ensures(Contract.Result<DateTime?>() == (!dt.HasValue ? dt : dt.Value.AddMonths(months)));

            return !dt.HasValue ? dt : dt.Value.AddMonths(months);
        }

        [Pure]
        public static DateTime? AddQuarters(this DateTime? dt, int quarters)
        {
            Contract.Ensures(Contract.Result<DateTime?>() == dt.AddMonths(3 * quarters));

            return dt.AddMonths(3 * quarters);
        }

        [Pure]
        public static DateTime FirstDayOfQuarter(this string yearQuarter)
        {
            string trimmedQuarter = yearQuarter.Trim();
            int size = trimmedQuarter.Length;
            int quarter = int.Parse(trimmedQuarter.Substring(size - 1));
            int year = int.Parse(trimmedQuarter.Substring(0, size - 1));
            DateTime result = new DateTime(year, ((quarter - 1) * 3) + 1, 1);
            return result;
        }

        [Pure]
        public static DateTime FirstDayOfQuarter(this int yearQuarter)
        {
            int quarter = yearQuarter % 10;
            int year = yearQuarter / 10;
            DateTime result = new DateTime(year, ((quarter - 1) * 3) + 1, 1);
            return result;
        }

        [Pure]
        public static DateTime FirstDayOfQuarter(this DateTime dt)
        {
            int months = (((dt.Month - 1) / 3) * 3) + 1;
            DateTime result = new DateTime(dt.Year + (months / 12), months % 12, 1);
            return result;
        }

        [Pure]
        public static bool IsFirstDayOfQuarter(this DateTime dt)
        {
            return (dt.Day == 1) && (((dt.Month - 1) % 3) == 0);
        }

        /// <summary>
        ///     ImmediateFirstOfQuarter returns given date if the given date is first of quarter or else first of next quarter.
        /// </summary>
        /// <param name="dt">The given date.</param>
        /// <returns>A date computed on the given date.</returns>
        [Pure]
        public static DateTime ImmediateFirstOfQuarter(this DateTime dt)
        {
            return IsFirstDayOfQuarter(dt) ? dt : FirstDayOfNextQuarter(dt);
        }

        [Pure]
        public static DateTime FirstDayOfNextQuarter(this DateTime dt)
        {
            return dt.FirstDayOfQuarter().AddQuarters(1);
        }

        [Pure]
        public static DateTime FirstDayOfPreviousQuarter(this DateTime dt)
        {
            return dt.FirstDayOfQuarter().AddQuarters(-1);
        }

        [Pure]
        public static DateTime FirstDayOfNextNextQuarter(this DateTime dt)
        {
            return dt.FirstDayOfQuarter().AddQuarters(2);
        }

        [Pure]
        public static DateTime LastDayOfCurrentQuarter(this DateTime dt)
        {
            return dt.FirstDayOfQuarter().AddQuarters(1).AddDays(-1);
        }

        [Pure]
        public static DateTime LastDayOfNextQuarter(this DateTime dt)
        {
            return dt.FirstDayOfQuarter().AddQuarters(2).AddDays(-1);
        }

        [Pure]
        public static bool IsFirstDayOfMonth(this DateTime dt)
        {
            return dt.Day == 1;
        }

        [Pure]
        public static bool? IsFirstDayOfMonth(this DateTime? dt)
        {
            return dt.HasValue ? dt.Value.Day == 1 : (bool?)null;
        }

        /// <summary>
        ///     ImmediateFirstOfMonth returns given date if the given date is first of month or else first of next month.
        /// </summary>
        /// <param name="dt">The given date.</param>
        /// <returns>A date computed on the given date.</returns>
        [Pure]
        public static DateTime ImmediateFirstOfMonth(this DateTime dt)
        {
            return IsFirstDayOfMonth(dt) ? dt : new DateTime(dt.Year, dt.Month, 1).AddMonths(1);
        }

        [Pure]
        public static DateTime FirstDayOfNextMonth(this DateTime dt)
        {
            DateTime ndt = dt.AddMonths(1);
            return new DateTime(ndt.Year, ndt.Month, 1);
        }

        /// <summary>
        /// Computes the number of days between the given dates.    
        /// </summary>
        /// <remarks>Returns 0 when start and end date is on the same day.</remarks>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A number representing the number of days between the given <paramref name="startDate"/> and <paramref name="endDate"/>.</returns>
        [Pure]
        public static int DaysBetween(this DateTime startDate, DateTime endDate)
        {
            // ReSharper disable SimplifyConditionalTernaryExpression
            Contract.Ensures(startDate.StripHours() < endDate.StripHours()
                                 ? Contract.Result<int>() > 0
                                 : true);
            Contract.Ensures(startDate.StripHours() == endDate.StripHours()
                                 ? Contract.Result<int>() == 0
                                 : true);
            Contract.Ensures(startDate.StripHours() > endDate.StripHours()
                                 ? Contract.Result<int>() < 0
                                 : true);
            // ReSharper restore SimplifyConditionalTernaryExpression
            Contract.Ensures((endDate - startDate).Days == Contract.Result<int>());

            TimeSpan ts = endDate - startDate;
            return ts.Days;
        }

        /// <summary>
        /// Computes the number of months between the given dates.    
        /// </summary>
        /// <remarks>Returns 0 when start and end date is in the same month.</remarks>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A number representing the number of months between the given <paramref name="startDate"/> and <paramref name="endDate"/>.</returns>
        [Pure]
        public static int MonthsBetween(this DateTime startDate, DateTime endDate)
        {
            int monthsApart = (12 * (startDate.Year - endDate.Year)) + (startDate.Month - endDate.Month);
            return Math.Abs(monthsApart);
        }

        /// <summary>
        /// Computes the number of quarters between the given dates.    
        /// </summary>
        /// <remarks>Returns 0 when start and end date is in the same quarter.</remarks>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A number representing the number of quarters between the given <paramref name="startDate"/> and <paramref name="endDate"/>.</returns>
        [Pure]
        public static int QuartersBetween(this DateTime startDate, DateTime endDate)
        {
            return MonthsBetween(startDate, endDate) / 3;
        }

        /// <summary>
        ///     ImmediateFirstOfYear returns given date if the given date is first of year or else first of next year.
        /// </summary>
        /// <param name="dt">The given date.</param>
        /// <returns>A computed date based on the given date.</returns>
        [Pure]
        public static DateTime ImmediateFirstOfYear(this DateTime dt)
        {
            return dt.DayOfYear == 1 ? dt : new DateTime(dt.Year + 1, 1, 1);
        }

        [Pure]
        public static DateTime FirstDayOfNextYear(this DateTime dt)
        {
            return new DateTime(dt.Year + 1, 1, 1);
        }

        [Pure]
        public static DateTime FirstDayOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        [Pure]
        public static DateTime LastDayOfMonth(this DateTime dt)
        {
            DateTime result = dt.AddMonths(1);
            result = new DateTime(result.Year, result.Month, 1);
            return result.AddDays(-1);
        }

        /// <summary>
        ///     Computes the number of days in a year, taking into account leap years.
        /// </summary>
        /// <param name="date">The given date.</param>
        /// <returns>A number representing the number of days in the year of the given <paramref name="date"/>.</returns>
        [Pure]
        public static int NumberOfDaysInYear(this DateTime date)
        {
            DateTime lastDayOfYear = new DateTime(date.Year, 12, 31);
            return lastDayOfYear.DayOfYear;
        }

        [Pure]
        public static int AgeInYears(this DateTime birth, DateTime dt)
        {
            Contract.Ensures(Contract.Result<int>() ==
                             dt.Year - birth.Year - (dt.Month < birth.Month || (dt.Month == birth.Month && dt.Day < birth.Day) ? 1 : 0));

            /* in a previous version, we used DayOfYear to see whether
               the person already had his birthday in the year of dt or not;
               that doesn't work however in leap years;
               we need to test months and days of months separately */
            int result = dt.Year - birth.Year;
            if (dt.Month < birth.Month
                || (dt.Month == birth.Month
                    && dt.Day < birth.Day))
            {
                result--;
            }

            return result;
        }

        [Pure]
        public static bool IsConsecutiveSequence<T>(
            this IEnumerable<T> lst,
            Func<T, DateTime?> extractDateBegin,
            Func<T, DateTime?> extractDateEnd)
            where T : class
        {
            Contract.Requires(extractDateBegin != null);
            Contract.Requires(extractDateEnd != null);

            if (lst == null || lst.Count() == 0)
            {
                return true;
            }

            var sortedList = lst
                .Select(o => new
                             {
                                 StartDate = extractDateBegin(o) ?? DateTime.MinValue,
                                 EndDate = extractDateEnd(o) ?? DateTime.MaxValue
                             })
                .OrderBy(o => o.StartDate)
                .ToList();

            var previousItem = sortedList[0];
            if (previousItem.StartDate > previousItem.EndDate)
            {
                return false;
            }

            for (int i = 1; i < sortedList.Count; i++)
            {
                var item = sortedList[i];
                if (item.StartDate > item.EndDate
                    || item.StartDate != previousItem.EndDate)
                {
                    return false;
                }

                previousItem = item;
            }

            return true;
        }
    }
}