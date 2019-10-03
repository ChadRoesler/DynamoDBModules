namespace DynamoDbModules.Constants
{
    internal class ResourceStrings
    {
        internal readonly static string KeyValuePairString = $"    Key {{0}}: Value {{1}}{ System.Environment.NewLine }";
        internal readonly static string RemovingItem = $"Deleting Document:{System.Environment.NewLine}";
        internal readonly static string UpdatingItem = $"Updating Document:{System.Environment.NewLine}";
        internal readonly static string RegexParameterValidation = @"^[a-zA-Z0-9]+$";
        internal const string ClientValidation = "ValidationTableThatShouldInNoWayShapeOrFormExist";
    }
}
