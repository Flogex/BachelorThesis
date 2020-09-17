using GraphQL;

namespace Flogex.Thesis.NeverNote.GraphQL.Helper
{
    internal static class Errors
    {
        public static T IdParsingError<T>(string prefix)
            => throw new ExecutionError("A valid id must be provided. " +
                $"Ids must be prefixed with '{prefix}', followed by an integer value.");

        public static T ArgumentMissingError<T>(string name)
            => throw new ExecutionError($"Argument '{name}' must be provided.");

        public static T AuthenticationRequiredError<T>()
            => throw new ExecutionError("Authentication required.");

        public static T InvalidUsernameError<T>()
            => throw new ExecutionError("Invalid username.");

        public static T NotFound<T>(string entity, string id)
            => throw new ExecutionError($"{entity} with id '{id}' could not be found.");

        public static T InvalidArgumentException<T>(string argument, string message)
            => throw new ExecutionError($"Invalid argument {argument}: {message}.");
    }
}
