using System;
using System.Collections.Generic;
using System.Linq;

namespace Bobasoft.Data
{
    public static class Moment
    {
        public class Descriptor
        {
            public Descriptor(double upperBound, Func<double, string> inPast, Func<double, string> inFuture = null)
            {
                UpperBound = upperBound;
                InPast = inPast;
                InFuture = inFuture ?? delegate
                                       {
                                           return "";
                                       };
            }

            public double UpperBound { get; private set; }

            /// <summary>
            /// In past description.
            /// <example>
            /// 1 minute ago
            /// </example>
            /// </summary>
            public Func<double, string> InPast { get; private set; }

            /// <summary>
            /// In future description.
            /// <example>
            /// 1 minute from now
            /// </example>
            /// </summary>
            public Func<double, string> InFuture { get; private set; } 
        }

        public static string ToMoment(this DateTime dateTime)
        {
            return ToMoment(dateTime, Default);
        }

        public static string ToMoment(this DateTime dateTime, IEnumerable<Descriptor> descriptors)
        {
            var diff = DateTime.UtcNow - dateTime;
            var totalMinutes = diff.TotalMinutes;

            var inFuture = false;
            if (totalMinutes < 0.0)
            {
                totalMinutes = Math.Abs(totalMinutes);
                inFuture = true;
            }

            var descriptor = descriptors.First(n => totalMinutes < n.UpperBound);
            return inFuture ? descriptor.InFuture(totalMinutes) : descriptor.InPast(totalMinutes);
        }

        //======================================================
        #region _Private, protected, internal fields_

        private static readonly IEnumerable<Descriptor> Default =
            new[]
            {
                new Descriptor(0.75, mins => "less than a minute ago", mins => "less than a minute from now"),
                new Descriptor(1.5, mins => "about a minute ago", mins => "about a minute from now"),
                new Descriptor(45,
                               mins => string.Format("{0} minutes ago", Math.Round(mins)),
                               mins => string.Format("{0} minutes from now", Math.Round(mins))),
                new Descriptor(90, mins => "about an hour ago", mins => "about an hour from now"),
                new Descriptor(60*24,
                               mins => string.Format("about {0} hours ago", Math.Round(Math.Abs(mins/60))),
                               mins => string.Format("about {0} hours from now", Math.Round(Math.Abs(mins/60)))),
                new Descriptor(60*48, mins => "a day ago", mins => "a day from now"),
                new Descriptor(60*24*30,
                               mins => string.Format("{0} days ago", Math.Floor(Math.Abs(mins/1440))),
                               mins => string.Format("{0} days from now", Math.Floor(Math.Abs(mins/1440)))),
                new Descriptor(60*24*60, mins => "about a month ago", mins => "about a month from now"),
                new Descriptor(60*24*365,
                               mins => string.Format("{0} months ago", Math.Floor(Math.Abs(mins/43200))),
                               mins => string.Format("{0} months from now", Math.Floor(Math.Abs(mins/43200)))),
                new Descriptor(60*24*365*2, mins => "about a year ago", mins => "about a year from now"),
                new Descriptor(double.MaxValue,
                               mins => string.Format("{0} years ago", Math.Floor(Math.Abs(mins/525600))),
                               mins => string.Format("{0} years from now", Math.Floor(Math.Abs(mins/525600)))),
            };

        #endregion
    }
}