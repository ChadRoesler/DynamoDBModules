using System;
using System.Management.Automation;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynamoDbModules.Constants;

namespace DynamoDbModules.Cmdlets
{

    /// <summary>
    /// <para type="synopsis">Creates a connection to DynamoDb.</para>
    /// <para type="description">Creatse a connection to DynamoDb based on information passed.</para>
    /// </summary>
    [Cmdlet("New", "DynamoDbClient")]
    [OutputType(typeof(AmazonDynamoDBClient))]
    //$storedCredentials = New-DynamoDbClient -AccessKeyId "[AccessKeyHere]" -SecretAccessKey "[SecretAccessKeyHere]" -RegionEndPoint "[RegionHere(us-east-1)]"
    public class NewDynamoDbClient : PSCmdlet
    {
        /// <summary>
        /// <para type="description">IAM access key with Dynamo Access.</para>
        /// </summary>
        [Parameter(Position = 0)]
        [Alias("key")]
        [ValidateNotNullOrEmpty]
        public string AccessKeyId { get; set; }

        /// <summary>
        /// <para type="description">IAM secret key with Dynamo Access.</para>
        /// </summary>
        [Parameter(Position = 1)]
        [Alias("secret")]
        [ValidateNotNullOrEmpty]
        public string SecretAccessKey { get; set; }

        /// <summary>
        /// <para type="description">Region you are accessing.</para>
        /// </summary>
        [Parameter(Position = 2)]
        [Alias("region")]
        [ValidateNotNullOrEmpty]
        public string RegionEndPoint { get; set; }


        private AmazonDynamoDBClient DynamoDBClient = new AmazonDynamoDBClient();
        private RegionEndpoint Region;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Region = RegionEndpoint.GetBySystemName(RegionEndPoint);
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            DynamoDBClient = new AmazonDynamoDBClient(AccessKeyId, SecretAccessKey, Region);
            try
            {
                var validation = Table.LoadTable(DynamoDBClient, ResourceStrings.ClientValidation);
            }
            catch(Exception ex)
            {

                if (ex.GetType() != typeof(ResourceNotFoundException))
                {
                    var errorExecuting = new ErrorRecord(ex, ErrorStrings.ConnectionError, ErrorCategory.ConnectionError, ex.Source);
                    ThrowTerminatingError(errorExecuting);
                }
              
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            WriteObject(DynamoDBClient);
        }
    }
}
