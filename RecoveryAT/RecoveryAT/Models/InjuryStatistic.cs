using System;
using System.Collections.Generic;

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

    // method to get the color for the injury type
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

    // Generate the next color in sequence with brightness adjustment
    private static string GenerateNextColor()
    {
        // Get the current base color
        string baseColor = BaseColors[CurrentColorIndex];

        // Adjust the brightness
        string modifiedColor = AdjustBrightness(baseColor, BrightnessModifier);

        // update indices for the next color
        CurrentColorIndex = (CurrentColorIndex + 1) % BaseColors.Count;
        if (CurrentColorIndex == 0) // if we've cycled through all base colors, increase the brightness
        {
            BrightnessModifier = (BrightnessModifier + 20) % 100; 
        }

        return modifiedColor;
    }

    // adjusts the brightness of a color in hex format
    private static string AdjustBrightness(string hexColor, int percentage)
    {
        // Parse the color
        int r = Convert.ToInt32(hexColor.Substring(1, 2), 16);
        int g = Convert.ToInt32(hexColor.Substring(3, 2), 16);
        int b = Convert.ToInt32(hexColor.Substring(5, 2), 16);

        // Modify brightness
        r = Math.Min(255, r + percentage);
        g = Math.Min(255, g + percentage);
        b = Math.Min(255, b + percentage);

        // Return the modified color
        return $"#{r:X2}{g:X2}{b:X2}";
    }
}
