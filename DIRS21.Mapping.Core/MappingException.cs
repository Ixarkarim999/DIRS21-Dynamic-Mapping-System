//Custom Error Class

namespace DIRS21.Mapping.Core

{
	public class MappingException : Exception
	{
		public string SourceType { get; }
		public string TargetType { get; }

		public MappingException(string message) : base(message) { }

		public MappingException(string sourceType, string targetType) : base($"No mapper found for '{sourceType}' -> '{targetType}'")
		{
			SourceType = sourceType;
			TargetType = targetType;
		}

	}
}