using System;

namespace EnsembleFX.Core.Exceptions
{
    /// <summary>
    /// Exception which describes an unhandled enum in a switch statement.
    /// </summary>
    public class EnumTranslationException : Exception
    {
        /// <summary>
        /// Create new instance of EnumSwitchException using enum value and base exception message.
        /// </summary>
        /// <param name="value">The value of unhandled enum.</param>
        public EnumTranslationException(Enum value)
            : base(String.Format("Unhandled enum value in switch statement: {0}", value.ToString()))
        {

        }
    }
}
