namespace OsuApiHelper.Math
{
    public static class Mathf
    {
        public static float Round(float a, int b = 0) => (float) System.Math.Round(a, b);
        
        public static float Min(float a, float b) => (float) System.Math.Min(a, b);

        public static float Max(float a, float b) => (float) System.Math.Max(a, b);

        public static float Ceil(float a) => (float) System.Math.Ceiling(a);

        public static float Floor(float a) => (float) System.Math.Floor(a);

        public static float Pow(float a, float b) => (float) System.Math.Pow(a, b);
        
        public static float Abs(float a) => (float) System.Math.Abs(a);

        public static float Log10(float a) => (float) System.Math.Log10(a);

        public static string FormatNumber(this float value) => ((double) value).FormatNumber();
        
        public enum TemperatureType
        {
            Kelvin,
            Celsius,
            Fahrenheit
        }
        
        public static float ConvertTemperature(float value, TemperatureType from, TemperatureType to)
        {
            switch (from)
            {
                case TemperatureType.Kelvin:
                    switch (to)
                    {
                        case TemperatureType.Kelvin:
                            break;
                        case TemperatureType.Celsius:
                            value = value - 273.15f;
                            break;
                        case  TemperatureType.Fahrenheit:
                            value = (value - 273.15f) * 9f / 5f + 32f;
                            break;
                    }
                    break;
                case TemperatureType.Celsius:
                    switch (to)
                    {
                        case TemperatureType.Kelvin:
                            value = value + 273.15f;
                            break;
                        case TemperatureType.Celsius:
                            break;
                        case  TemperatureType.Fahrenheit:
                            value = value * 9f / 5f + 32;
                            break;
                    }
                    break;
                case TemperatureType.Fahrenheit:
                    switch (to)
                    {
                        case TemperatureType.Kelvin:
                            value = (value - 32f) * 5f / 9f + 273.15f;
                            break;
                        case TemperatureType.Celsius:
                            value = (value - 32f) * 5f / 9f;
                            break;
                        case  TemperatureType.Fahrenheit:
                            break;
                    }
                    break;
            }
            
            return value;
        }
    }
}