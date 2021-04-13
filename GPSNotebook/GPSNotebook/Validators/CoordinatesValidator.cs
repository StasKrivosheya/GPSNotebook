using System;

namespace GPSNotebook.Validators
{
    public class CoordinatesValidator
    {
        #region -- Private constants --

        private const double LATITUDE_RANGE = 90d;
        private const double LONGITUDE_RANGE = 180d;

        #endregion

        private double Range { get; }

        #region -- Public properties --

        public static CoordinatesValidator Latitude;
        public static CoordinatesValidator Longitude;

        #endregion

        #region -- Constructors --

        private CoordinatesValidator(double range)
        {
            Range = range;
        }

        static CoordinatesValidator()
        {
            Latitude = new CoordinatesValidator(LATITUDE_RANGE);
            Longitude = new CoordinatesValidator(LONGITUDE_RANGE);
        }

        #endregion

        public static bool Validate(string input, CoordinatesValidator type)
        {
            bool isInputValid = false;

            if (double.TryParse(input, out double result))
            {
                if (Math.Abs(result) <= type.Range)
                {
                    isInputValid = true;
                }
            }

            return isInputValid;
        }
    }
}
