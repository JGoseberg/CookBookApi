public class MeasurementUnit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Abbreviation { get; set; }


    public ICollection<Ingredient> Ingredients { get; set; }
}
