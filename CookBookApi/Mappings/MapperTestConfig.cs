using AutoMapper;

namespace CookBookApi.Mappings
{
    public class MapperTestConfig
    {
        public static IMapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                {
                    cfg.AddProfile<MappingProfile>();
                }
            });

            return config.CreateMapper();
        }
    }
}
