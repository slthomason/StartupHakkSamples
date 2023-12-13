namespace WebApplication19
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Country> getData()
        {
            var country = new List<Country>
        {
            new()
            {
                Id = 1,
                Name = "United States of America"
            },
            new()
            {
                Id=2,
                Name="Canada"
            },
            new()
            {
                Id=3,
                Name="Australia"
            }
        };
            return country;
        }
    }
}
