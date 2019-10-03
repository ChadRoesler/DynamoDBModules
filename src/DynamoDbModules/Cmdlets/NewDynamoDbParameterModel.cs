using System;
using System.Management.Automation;
using System.Text.RegularExpressions;
using DynamoDbModules.Constants;
using DynamoDbModules.Models;

namespace DynamoDbModules.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Creates a DynamoDbParameter Model.</para>
    /// <para type="description">Creats a DynamoDbParameter Module for use when filtering in Get-DynamoDbClient.</para>
    /// </summary>
    [Cmdlet("New", "DynamoParameterModel")]
    [OutputType(typeof(DynamoDbParameterModel))]
    //New-DynamoParameterModel -VariableName ":variableName" -VariableValue "SomeValue" -VariableType "string/numeric/binary"
    public class CreateDynamoDbParameterModel : PSCmdlet
    {
        /// <summary>
        /// <para type="description">Name of the Variable, must start with ":" followed by alpha-numeric characters.</para>
        /// </summary>
        [Parameter(Position = 0)]
        [Alias("name")]
        [ValidateNotNullOrEmpty]
        public string VariableName { get; set; }

        /// <summary>
        /// <para type="description">Type of Variable, N=Numeric, B=Binary, S=STring, if not used will default to S.</para>
        /// </summary>
        [Parameter(Position = 1)]
        [Alias("type")]
        [ValidateNotNullOrEmpty]
        public string VariableType { get; set; }

        /// <summary>
        /// <para type="description">Value of the Variable.</para>
        /// </summary>
        [Parameter(Position = 2)]
        [Alias("value")]
        [ValidateNotNullOrEmpty]
        public string VariableValue { get; set; }

        private DynamoDbParameterModel parameterModel = new DynamoDbParameterModel();

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if(!VariableName.StartsWith(":"))
            {
                var exceptionToThrow = new ArgumentException(ErrorStrings.ParameterMissingColon);
                var errorExecuting = new ErrorRecord(exceptionToThrow, ErrorStrings.ParameterMissingColon, ErrorCategory.InvalidData, exceptionToThrow.Source);
                ThrowTerminatingError(errorExecuting);
            }
            if(!Regex.IsMatch(VariableName.TrimStart(':'), ResourceStrings.RegexParameterValidation))
            {
                var exceptionToThrow = new ArgumentException(ErrorStrings.ParameterNonAlphaNumeric);
                var errorExecuting = new ErrorRecord(exceptionToThrow, ErrorStrings.ParameterNonAlphaNumeric, ErrorCategory.InvalidData, exceptionToThrow.Source);
                ThrowTerminatingError(errorExecuting);
            }
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            try
            {
                parameterModel.AddParameter(VariableName, VariableType, VariableValue);
            }
            catch(Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.ParameterLoadError, ErrorCategory.InvalidData, ex.Source);
                WriteError(errorExecuting);
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            WriteObject(parameterModel);
        }
    }
}
