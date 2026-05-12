//Interface every mapper must follow
//Without it the registry would need to know about every specific mapper.

namespace DIRS21.Mapping.Core
{
	public interface IMapper 
	{ 
		string SourceType { get; }
		string TargetType { get; }
		Object Map(object Data);
	}
}


