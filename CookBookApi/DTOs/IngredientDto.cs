namespace CookBookApi.DTOs
{
    public class IngredientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string UnitName { get; set; } // This will hold the name of the unit
        public CuisineDto Cuisine { get; set; }
        public List<RestrictionDto> Restrictions { get; set; }
    }
}
