using AutoMapper;

namespace API.Utils;

public static class AutoMapperUtils
{
    public static TD BasicAutoMapper<TS, TD>(TS source)
    {
        MapperConfiguration config = new(cfg =>
        {
            cfg.CreateMap<TS, TD>();
        });
        Mapper mapper = new(config);
        TD result = mapper.Map<TD>(source);
        return result;
    }

    public static List<TD> BasicAutoMapper<TS, TD>(List<TS> source) 
    {
        MapperConfiguration config = new(cfg => cfg.CreateMap<TS, TD>());
        Mapper mapper = new(config);
        var result = new List<TD>(mapper.Map<List<TD>>(source));
        return result;
    }
    
    public static TD BasicNastedAutoMapper<TS, TD, TS2, TD2>(TS source)
    {
        MapperConfiguration config = new(cfg =>
        {
                    cfg.CreateMap<TS, TD>();
                    cfg.CreateMap<TS2, TD2>();
        });
        Mapper mapper = new(config);
        TD result = mapper.Map<TD>(source);
        return result;
    }
    
    public static List<TD> BasicNestedAutoMapper<TS, TD, TS2, TD2>(List<TS> source) 
    {
        MapperConfiguration config = new(cfg =>
        {
            cfg.CreateMap<TS, TD>();
            cfg.CreateMap<TS2, TD2>();
        });
        Mapper mapper = new(config);
        var result = new List<TD>(mapper.Map<List<TD>>(source));
        return result;
    }
    
    public static TD BasicNastedAutoMapper<TS, TD, TS2, TD2, TS3, TD3>(TS source)
    {
        MapperConfiguration config = new(cfg =>
        {
            cfg.CreateMap<TS, TD>();
            cfg.CreateMap<TS2, TD2>();
            cfg.CreateMap<TS3, TD3>();
        });
        Mapper mapper = new(config);
        TD result = mapper.Map<TD>(source);
        return result;
    }

    public static List<TD> BasicNestedAutoMapper<TS, TD, TS2, TD2, TS3, TD3>(List<TS> source)
    {
        MapperConfiguration config = new(cfg =>
        {
            cfg.CreateMap<TS, TD>();
            cfg.CreateMap<TS2, TD2>();
            cfg.CreateMap<TS3, TD3>();
        });
        Mapper mapper = new(config);
        var result = new List<TD>(mapper.Map<List<TD>>(source));
        return result;
    }
    
    public static TD TupleAutoMapper<TS, TD>(TS source, List<(Type SourceType, Type Destination)> typesTuples)
    {
        MapperConfiguration config = new(cfg =>
        {
            cfg.CreateMap<TS, TD>();
            foreach ((Type? sourceType, Type? destination) in typesTuples)
            {
                cfg.CreateMap(sourceType, destination);
            }
           
        });
        Mapper mapper = new(config);
        var result = mapper.Map<TD>(source);
        return result;
    }
    
    public static List<TD> TupleAutoMapper<TS, TD>(List<TS> source, List<(Type SourceType, Type Destination)> typesTuples)
    {
        MapperConfiguration config = new(cfg =>
        {
            cfg.CreateMap<TS, TD>();
            foreach ((Type? sourceType, Type? destination) in typesTuples)
            {
                cfg.CreateMap(sourceType, destination);
            }
           
        });
        Mapper mapper = new(config);
        var result = new List<TD>(mapper.Map<List<TD>>(source));
        return result;
    }
}