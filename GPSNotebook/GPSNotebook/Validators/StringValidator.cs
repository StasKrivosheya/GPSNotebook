using System.Text.RegularExpressions;

namespace GPSNotebook.Validators
{
    public class StringValidator
    {
        #region --- Private Constants ---

        private const string MAIL_REGEX =
            @"^[\w\.]+@([\w-]+\.)+[\w-]{2,4}$";

        // accepts string with at least one lowercase, one uppercase and one digit,
        // total length is from 8 to 16
        private const string PASSWORD_REGEX =
            @"^(?=.*\d)(?=.*[a-zа-яё])(?=.*[A-ZА-ЯЁ]).{8,16}$";

        #endregion

        #region --- Properties ---

        private string Pattern { get; }

        public static StringValidator Mail { get; }
        public static StringValidator Password { get; }

        #endregion

        #region --- Constructors ---

        private StringValidator(string pattern)
        {
            Pattern = pattern;
        }

        static StringValidator()
        {
            Mail = new StringValidator(MAIL_REGEX);
            Password = new StringValidator(PASSWORD_REGEX);
        }

        #endregion

        #region --- Public Methods ---

        public static bool Validate(string input, StringValidator type)
        {
            return !string.IsNullOrEmpty(input) &&
                   Regex.IsMatch(input, type.Pattern);
        }

        #endregion
    }
}
