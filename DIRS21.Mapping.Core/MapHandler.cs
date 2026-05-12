//The main entry point — the only class the outside world needs to talk to.

namespace DIRS21.Mapping.Core
{
    public class MapHandler
    {
        private readonly MapperRegistry _registry;

        public MapHandler(MapperRegistry registry)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public object Map(object data, string sourceType, string targetType)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrWhiteSpace(sourceType))
                throw new ArgumentNullException(nameof(sourceType));

            if (string.IsNullOrWhiteSpace(targetType))
                throw new ArgumentNullException(nameof(targetType));

            var mapper = _registry.Resolve(sourceType, targetType);

            return mapper.Map(data);
        }
    }
}