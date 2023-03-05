using RegexSchemaLib.Enums;

namespace RegexSchemaLib.Structs
{
    public readonly struct ErrorModel
    {
        public ErrorModel(ErrorType errorType, string message)
        {
            Type = errorType;
            Message = message;
        }
        public ErrorType Type { get; init; }
        public string Message { get; init; }
    }
}
