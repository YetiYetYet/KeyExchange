using AutoMapper;

namespace API.Utils;

public static class AutoMapperUtils
{
    public static TD BasicAutoMapper<TS, TD>(TS source)
    {
        MapperConfiguration config = new(cfg => cfg.CreateMap<TS, TD>());
        Mapper mapper = new(config);
        TD result = mapper.Map<TD>(source);
        return result;
    }
    
    public static List<TD> BasicAutoMapper<TS, TD>(List<TS> source) 
    {
        MapperConfiguration config = new(cfg => cfg.CreateMap<TS, TD>());
        Mapper mapper = new(config);
        var result = mapper.Map<List<TS>, List<TD>>(source);
        return result;
    }
}