namespace DynamoDbModules.Constants
{
    internal class ErrorStrings
    {
        internal const string ParameterLoadError = "Error loading parameters into DynamoDbParamter Model.";
        internal const string ConnectionError = "Unable to Connect to DyanmoDb.";
        internal const string TableDoesNotExistError = "The specified table does not exist.";
        internal const string ScanRequestCreationError = "Unable to create scan request.";
        internal const string GatherDocumentListError = "Unable to gather document list.";
        internal const string FilterExistsNoParameters = "A filter was passed with no Parameters.";
        internal const string ParametersExistNoFilter = "A ParameterModel was passed with no Filter.";
        internal const string ParameterMissingColon = "ParameterName must start with ':'.";
        internal const string ParameterNonAlphaNumeric = "ParameterName must start with ':' followed by alpha numeric characters.";

        internal readonly static string ErrorRemovingDocument = $"Error removing document with Keys:{System.Environment.NewLine}{{0}}.";
        internal readonly static string ErrorUpdatingDocument = $"Error updating document with Keys:{System.Environment.NewLine}{{0}}.";

    }
}
