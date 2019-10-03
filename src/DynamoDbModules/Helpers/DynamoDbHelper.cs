using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;

namespace DynamoDbModules.Helpers
{
    internal static class DynamoDbHelper
    {

        internal static Table GetTable (AmazonDynamoDBClient client, string tableName)
        {
            var table = Table.LoadTable(client, tableName);
            return table;
        }

        internal static Dictionary<string, DynamoDBEntryType> GetTableKeys (Table table)
        {
            var keyDictionary = new Dictionary<string, DynamoDBEntryType>();
            foreach(var keyPair in table.Keys)
            {
                keyDictionary.Add(keyPair.Key, keyPair.Value.Type);
            }
            return keyDictionary;
        }

        internal static ScanRequest CreateScanRequest(string tableName, string filterExpression, Dictionary<string, AttributeValue> parameters)
        {
            var scanRequest = new ScanRequest
            {
                TableName = tableName,
                ExpressionAttributeValues = parameters,
                FilterExpression = filterExpression
            };
            return scanRequest;
        }

        internal static ScanRequest CreateScanRequest(string tableName, string filterExpression)
        {
            var scanRequest = new ScanRequest
            {
                TableName = tableName,
                FilterExpression = filterExpression
            };
            return scanRequest;
        }

        internal static ScanRequest CreateScanRequest(string tableName)
        {
            var scanRequest = new ScanRequest
            {
                TableName = tableName,
            };
            return scanRequest;
        }

        internal static List<Document> GetDocumentListFromScan(AmazonDynamoDBClient client, ScanRequest scanRequest)
        {
            var table = GetTable(client, scanRequest.TableName);
            var tableKeys = GetTableKeys(table);

            var scanResults = client.Scan(scanRequest);
            var scanResultsList = scanResults.Items;
            while (scanResults.LastEvaluatedKey != null && scanResults.LastEvaluatedKey.Count > 0 )
            {
                scanRequest.ExclusiveStartKey = scanResults.LastEvaluatedKey;
                scanResults = client.Scan(scanRequest);
                scanResultsList.AddRange(scanResults.Items);
            }

            var tableBatchGet = table.CreateBatchGet();

            foreach(var item in scanResultsList)
            {
                var keyDictionary = new Dictionary<string, DynamoDBEntry>();
                foreach(var key in tableKeys)
                {
                    switch(key.Value)
                    {
                        case DynamoDBEntryType.String:
                            keyDictionary.Add(key.Key, item[key.Key].S);
                            break;
                        case DynamoDBEntryType.Numeric:
                            keyDictionary.Add(key.Key, item[key.Key].N);
                            break;
                        case DynamoDBEntryType.Binary:
                            keyDictionary.Add(key.Key, item[key.Key].B);
                            break;
                        default:
                            keyDictionary.Add(key.Key, item[key.Key].S);
                            break;
                    }
                }
                tableBatchGet.AddKey(keyDictionary);
            }
            tableBatchGet.Execute();

            return tableBatchGet.Results;
        }
    }
}
