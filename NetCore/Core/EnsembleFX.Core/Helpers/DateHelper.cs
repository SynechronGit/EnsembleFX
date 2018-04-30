using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnsembleFX.Core.Helpers
{
    /// <summary>
    /// Represents helper members pertaining to DateTime object specific functionality.
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Gets a new DateTime object that represents the same time as DateTime.Today with DateTimeKind.Unspecified 
        /// </summary>
        public static DateTime UnspecifiedToday
        {
            get
            {
                return DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Unspecified);
            }
        }
        /// <summary>
        /// Gets a new DateTime object that represents the same time as DateTime.Now with DateTimeKind.Unspecified 
        /// </summary>
        public static DateTime UnspecifiedNow
        {
            get
            {
                return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            }
        }

        /// <summary>
        /// Gets a new DateTime object that represents the same time as a new DateTime with DateTimeKind.Unspecified 
        /// </summary>
        public static DateTime UnspecifiedNew
        {
            get
            {
                return DateTime.SpecifyKind(new DateTime(), DateTimeKind.Unspecified);
            }
        }

    }
}
