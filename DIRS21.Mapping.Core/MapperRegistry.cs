//A registry that stores all mappers and looks them up when needed.

namespace DIRS21.Mapping.Core
{
    public class MapperRegistry
    {
        private readonly Dictionary<(string, string), IMapper> _mappers = new();

        //Adds a mapper into the dictionary.
        public void Register(IMapper mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            var key = (mapper.SourceType, mapper.TargetType);

            _mappers[key] = mapper;
        }

        //Looks up the dictionary and returns the right mapper

        public IMapper Resolve(string sourceType, string targetType)
        {
            if (string.IsNullOrWhiteSpace(sourceType))
                throw new ArgumentNullException(nameof(sourceType));

            if (string.IsNullOrWhiteSpace(targetType))
                throw new ArgumentNullException(nameof(targetType));

            var key = (sourceType, targetType);

            if (!_mappers.TryGetValue(key, out var mapper))
                throw new MappingException(sourceType, targetType);

            return mapper;
        }
    }
}