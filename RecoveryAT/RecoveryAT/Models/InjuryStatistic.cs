using System;

public class InjuryStatistic
{
    public string InjuryType { get; set; }
    public float Percentage { get; set; }

    // Automatically generates a random color hex string
    public string ColorHex { get; } = GenerateRandomColorHex();

    // Static method to generate a random color in hex format
    private static string GenerateRandomColorHex()
    {
        Random random = new Random();
        return $"#{random.Next(0x1000000):X6}";
    }
}
