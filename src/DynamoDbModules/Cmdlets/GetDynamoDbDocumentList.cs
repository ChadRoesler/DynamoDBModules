using System;
using System.Collections.Generic;
using System.Management.Automation;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynamoDbModules.Constants;
using DynamoDbModules.Helpers;
using DynamoDbModules.Models;

namespace DynamoDbModules.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Gathers a Dynamo Db Document Table based on filtering and parameters passed.</para>
    /// <para type="description">Outputs a document array from the chosen table based on what you are filtering for.</para>
    /// </summary>
    [Cmdlet("Get", "DynamoDbDocumentList")]
    [OutputType(typeof(IEnumerable<Document>))]
    //$documentList = Get-DynamoDbDocumentList -TableName "SomeTable" -Client $storedCredentials -FilterExpression "ColumnName = :variableName OR/AND ColumnName2 = :variableName2" -Parameters $parameterModel
    public class GetDynamoDbDocumentList : PSCmdlet
    {
        /// <summary>
        /// <para type="description">Name of the DynamoDb Table.</para>
        /// </summary>
        [Parameter(Position = 0)]
        [Alias("t")]
        [ValidateNotNullOrEmpty]
        public string TableName { get; set; }

        /// <summary>
        /// <para type="description">The filter Expression used. (Ex: SomeDynamoColumn = :SomeVariableName)</para>
        /// </summary>
        [Parameter(Position = 1)]
        [Alias("f")]
        public string FilterExpression { get; set; }

        /// <summary>
        /// <para type="description">The parameter module used for filtering.</para>
        /// </summary>
        [Parameter(Position = 2)]
        [Alias("p")]
        public DynamoDbParameterModel Parameters { get; set; }

        /// <summary>
        /// <para type="description">Your Aws DynamoDb Client Object.</para>
        /// </summary>
        [Parameter(Position = 3)]
        [Alias("c")]
        [ValidateNotNullOrEmpty]
        public AmazonDynamoDBClient Client { get; set; }

        public Table Table;
        private List<Document> DocumentList = new List<Document>();
        private ScanRequest Request = new ScanRequest(); 

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            try
            {
                if (!string.IsNullOrWhiteSpace(FilterExpression) && Parameters == null)
                {
                    throw new Exception(ErrorStrings.FilterExistsNoParameters);
                }
            }
            catch (Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.TableDoesNotExistError, ErrorCategory.InvalidData, ex.Source);
                ThrowTerminatingError(errorExecuting);
            }
            try
            {
                Table = DynamoDbHelper.GetTable(Client, TableName);
            }
            catch (Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.TableDoesNotExistError, ErrorCategory.InvalidData, ex.Source);
                ThrowTerminatingError(errorExecuting);
            }
            try
            {
                if (!string.IsNullOrWhiteSpace(FilterExpression) && (Parameters != null || Parameters.ParamterCount > 0))
                {
                    Request = DynamoDbHelper.CreateScanRequest(TableName, FilterExpression, Parameters.ParameterDictionary);
                }
                else if(!string.IsNullOrWhiteSpace(FilterExpression) && (Parameters == null) || Parameters.ParamterCount == 0)
                {
                    Request = DynamoDbHelper.CreateScanRequest(TableName, FilterExpression);
                }
                else
                {
                    Request = DynamoDbHelper.CreateScanRequest(TableName);
                }
            }
            catch (Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.ScanRequestCreationError, ErrorCategory.InvalidData, ex.Source);
                ThrowTerminatingError(errorExecuting);
            }
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            try
            {
                DocumentList = DynamoDbHelper.GetDocumentListFromScan(Client, Request);
            }
            catch(Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.GatherDocumentListError, ErrorCategory.InvalidData, ex.Source);
                ThrowTerminatingError(errorExecuting);
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            WriteObject(DocumentList);
        }

    }
}
