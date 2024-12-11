/**
    Date: 12/05/24
    Description: Creates an injury statistic that will be used to create pie chart slices for each injury type. 
    Bugs: None that we know of.
    Reflection: This class didn't take too much time, but it was a little difficult to determine how to go about changing
    the set values after they have all been displayed in the pie chart. 
**/

public class InjuryStatistic
{
    // stores colors for specific injury types (case-insensitive)
    private static readonly Dictionary<string, string> InjuredAreaColors = new(StringComparer.OrdinalIgnoreCase);

    // a list of colors to go off of when generating new colors for the pie chart
    private static readonly List<string> BaseColors = new()
    {
        "#FF0000", // Red
        "#FF8300", // Orange
        "#FFE400", // Yellow
        "#00C503", // Green
        "#0000FF", // Blue
        "#4B0082", // Indigo
        "#EE82EE"  // Violet
    };
    private static int CurrentColorIndex = 0; // tracks the next base color
    private static int BrightnessModifier = 0; // tracks brightness adjustments
    public string InjuryType { get; set; }
    public float Percentage { get; set; }
    public string ColorHex => GetColorHex();

    /// <summary>
    /// Gets the color for the specific injury type
    /// </summary>
    /// <returns>A color that corresponds to an injury type.</returns>
    private string GetColorHex()
    {
        if (string.IsNullOrWhiteSpace(InjuryType))
            return "#000000"; // default to black if input is invalid

        // check for the injured area (case-insensitive)
        if (!InjuredAreaColors.TryGetValue(InjuryType, out var color))
        {
            // generate a random color and add it to the dictionary
            color = GenerateNextColor();
            InjuredAreaColors[InjuryType] = color;
        }
        return color;
    }

    /// <summary>
    /// Generates the next color to be used for an injury type.
    /// </summary>
    /// <returns>The next color that corresponds to an injury type.</returns>
    private static string GenerateNextColor()
    {
        string baseColor = BaseColors[CurrentColorIndex]; // get the current base color

        string modifiedColor = AdjustBrightness(baseColor, BrightnessModifier); // adjust the brightness of the base color

        // update indices for the next color
        CurrentColorIndex = (CurrentColorIndex + 1) % BaseColors.Count;
        if (CurrentColorIndex == 0) // if we've cycled through all base colors, increase the brightness
        {
            BrightnessModifier = (BrightnessModifier + 20) % 100; 
        }
        return modifiedColor;
    }

    /// <summary>
    /// Adjusts the color brightness depending on if the current was already displayed.
    /// </summary>
    /// <param name="hexColor">The hex value of the color.</param>
    /// <param name="percentage">The percentage value to change the color.</param>
    /// <returns>The new modified color.</returns>
    private static string AdjustBrightness(string hexColor, int percentage)
    {
        // gets the red, green, and blue values of a color
        int r = Convert.ToInt32(hexColor.Substring(1, 2), 16);
        int g = Convert.ToInt32(hexColor.Substring(3, 2), 16);
        int b = Convert.ToInt32(hexColor.Substring(5, 2), 16);

        // changes the brightness of the red, green, and blue values
        r = Math.Min(255, r + percentage);
        g = Math.Min(255, g + percentage);
        b = Math.Min(255, b + percentage);

        // returns the modified color
        return $"#{r:X2}{g:X2}{b:X2}";
    }
}
