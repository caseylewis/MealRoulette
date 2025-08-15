namespace MealRoulette.Models;

public class UnitOfMeasurement
{
    public string FullName { get; set; }
    public string ShortName { get; set; }

    public UnitOfMeasurement(string fullName, string shortName)
    {
        FullName = fullName;
        ShortName = shortName;
    }

    // Optional: parameterless constructor for serialization/deserialization
    public UnitOfMeasurement() { }

    // Common cooking units
    public static List<UnitOfMeasurement> AllUnits => new()
    {
        // Imperial Units
        new UnitOfMeasurement("Cup", "c"),
        new UnitOfMeasurement("Tablespoon", "tbsp"),
        new UnitOfMeasurement("Teaspoon", "tsp"),
        new UnitOfMeasurement("Ounce", "oz"),
        new UnitOfMeasurement("Pound", "lb"),
        new UnitOfMeasurement("Quart", "qt"),
        new UnitOfMeasurement("Pint", "pt"),
        new UnitOfMeasurement("Gallon", "gal"),
        new UnitOfMeasurement("Fluid Ounce", "fl oz"),

        // Metric Units
        new UnitOfMeasurement("Gram", "g"),
        new UnitOfMeasurement("Kilogram", "kg"),
        new UnitOfMeasurement("Milliliter", "ml"),
        new UnitOfMeasurement("Liter", "l"),

        // Culinary Units
        new UnitOfMeasurement("Pinch", "pinch"),
        new UnitOfMeasurement("Piece", "pc"),
        new UnitOfMeasurement("Dash", "dash"),
        new UnitOfMeasurement("Smidgen", "smidgen"),
        new UnitOfMeasurement("To Taste", "to taste"),
        new UnitOfMeasurement("Handful", "handful"),
        new UnitOfMeasurement("Knob", "knob"),
        new UnitOfMeasurement("Stick", "stick"),
        new UnitOfMeasurement("Slice", "slice"),
        new UnitOfMeasurement("Count", "count"),
    };
}
