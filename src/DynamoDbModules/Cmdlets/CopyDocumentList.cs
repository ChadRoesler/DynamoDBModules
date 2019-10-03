using System.Collections.Generic;
using System.Management.Automation;
using Amazon.DynamoDBv2.DocumentModel;

namespace DynamoDbModules.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Duplicates the Document list passed for modification purposes.</para>
    /// <para type="description">Duplicates the Document List passed for modification, this is needed due to how Dynamo handles drilling down into documents.</para>
    /// </summary>
    [Cmdlet("Copy", "DocumentList")]
    [OutputType(typeof(IEnumerable<Document>))]
    //$copiedDocumentList = Copy-DocumentList -DocumentList $originalDocumentList
    public class CopyDocumentList : PSCmdlet
    {
        /// <summary>
        /// <para type="description">Document List to copy.</para>
        /// </summary>
        [Parameter(Position = 0)]
        [Alias("d")]
        [ValidateNotNullOrEmpty]
        public List<Document> DocumentList { get; set; }

        private List<Document> CopiedDocumentList = new List<Document>();
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
                foreach (var document in DocumentList)
                {
                    var clonedDocument = (Document)document.Clone();
                    CopiedDocumentList.Add(clonedDocument);
                }

        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            WriteObject(CopiedDocumentList);
        }
    }
}
