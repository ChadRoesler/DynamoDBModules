using System;
using System.Collections.Generic;
using System.Management.Automation;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDbModules.Constants;
using DynamoDbModules.Helpers;

namespace DynamoDbModules.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Updates the passed table based on the documents in the document array.</para>
    /// <para type="description">Updates the passed table based on the documents in the document arra.</para>
    /// </summary>
    [Cmdlet("Update", "DynamoDbTable")]
    //Does not support adding a column if the document does not have it.
    //I DONT UNDERSTAND THIS MADNESS  THIS IS GARBAGE
    public class UpdateDynamoDbTable : PSCmdlet
    {
        /// <summary>
        /// <para type="description">Name of the DynamoDb Table.</para>
        /// </summary>
        [Parameter(Position = 0)]
        [Alias("t")]
        [ValidateNotNullOrEmpty]
        public string TableName { get; set; }

        /// <summary>
        /// <para type="description">Documents to update.</para>
        /// </summary>
        [Parameter(Position = 0)]
        [Alias("d")]
        [ValidateNotNullOrEmpty]
        public List<Document> DocumentList { get; set; }

        /// <summary>
        /// <para type="description">Your Aws DynamoDb Client Object.</para>
        /// </summary>
        [Parameter(Position = 3)]
        [Alias("c")]
        [ValidateNotNullOrEmpty]
        public AmazonDynamoDBClient Client { get; set; }

        /// <summary>
        /// <para type="description">Verbosity.</para>
        /// </summary>
        [Parameter(Position = 4)]
        [Alias("p")]
        public SwitchParameter PrintInfo
        {
            get
            {
                return OutputPrint;
            }
            set
            {
                OutputPrint = value;
            }
        }

        public Table Table;
        private string InfoText = string.Empty;
        private bool OutputPrint;
        private string KeyText = string.Empty;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            try
            {
                Table = DynamoDbHelper.GetTable(Client, TableName);
            }
            catch (Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, ErrorStrings.TableDoesNotExistError, ErrorCategory.InvalidData, ex.Source);
                ThrowTerminatingError(errorExecuting);
            }
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            try
            {
                var tableKeys = DynamoDbHelper.GetTableKeys(Table);
                foreach (var document in DocumentList)
                {
                    InfoText += ResourceStrings.UpdatingItem;
                    KeyText = string.Empty;
                    foreach (var key in tableKeys)
                    {
                        KeyText += string.Format(ResourceStrings.KeyValuePairString, key.Key, document[key.Key].AsString());
                    }
                    InfoText += KeyText;
                    Table.UpdateItem(document);
                }
            }
            catch(Exception ex)
            {
                var errorExecuting = new ErrorRecord(ex, string.Format(ErrorStrings.ErrorUpdatingDocument, KeyText), ErrorCategory.InvalidData, ex.Source);
                WriteError(errorExecuting);
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            if (OutputPrint)
            {
                WriteObject(InfoText);
            }
        }
    }
}
