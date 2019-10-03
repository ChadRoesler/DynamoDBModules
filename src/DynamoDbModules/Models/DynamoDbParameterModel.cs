using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DynamoDbModules.Constants;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynamoDbModules.Models
{
    public class DynamoDbParameterModel
    {

        private Dictionary<string, AttributeValue> parameterDictionary = new Dictionary<string, AttributeValue>();

        public int ParamterCount
        {
            get
            {
                if(parameterDictionary == null)
                {
                    return 0;
                }
                else
                {
                    return parameterDictionary.Count;
                }
            }
        }

        public Dictionary<string, AttributeValue> ParameterDictionary
        {
            get { return parameterDictionary; }
        }

        public DynamoDbParameterModel()
        {
        }

        public DynamoDbParameterModel(string parameterName, DynamoDBEntryType dynamoDBEntryType, string parameterValue)
        {
            switch (dynamoDBEntryType)
            {
                case DynamoDBEntryType.Numeric:
                    parameterDictionary.Add(parameterName, new AttributeValue() { N = parameterValue });
                    break;
                case DynamoDBEntryType.Binary:
                    parameterDictionary.Add(parameterName, new AttributeValue() { B = new MemoryStream(Encoding.UTF8.GetBytes(parameterValue ?? "")) });
                    break;
                case DynamoDBEntryType.String:
                    parameterDictionary.Add(parameterName, new AttributeValue() { S = parameterValue });
                    break;
                default:
                    parameterDictionary.Add(parameterName, new AttributeValue() { S = parameterValue });
                    break;
            }
        }

        public DynamoDbParameterModel(string parameterName, string dynamoDBEntryType, string parameterValue)
        {
            switch (dynamoDBEntryType.ToLower())
            {
                case "numeric":
                case "n":
                    parameterDictionary.Add(parameterName, new AttributeValue() { N = parameterValue });
                    break;
                case "binary":
                case "b":
                    parameterDictionary.Add(parameterName, new AttributeValue() { B = new MemoryStream(Encoding.UTF8.GetBytes(parameterValue ?? "")) });
                    break;
                case "string":
                case "s":
                    parameterDictionary.Add(parameterName, new AttributeValue() { S = parameterValue });
                    break;
                default:
                    parameterDictionary.Add(parameterName, new AttributeValue() { S = parameterValue });
                    break;
            }
        }

        public void AddParameter(string parameterName, DynamoDBEntryType dynamoDBEntryType, string parameterValue)
        {
            if (!parameterName.StartsWith(":"))
            {
                throw (new ArgumentException(ErrorStrings.ParameterMissingColon));
            }
            if (!Regex.IsMatch(parameterName.TrimStart(':'), ResourceStrings.RegexParameterValidation))
            {
                throw (new ArgumentException(ErrorStrings.ParameterNonAlphaNumeric));
            }
            switch (dynamoDBEntryType)
            {
                case DynamoDBEntryType.Numeric:
                    parameterDictionary.Add(parameterName, new AttributeValue() { N = parameterValue });
                    break;
                case DynamoDBEntryType.Binary:
                    parameterDictionary.Add(parameterName, new AttributeValue() { B = new MemoryStream(Encoding.UTF8.GetBytes(parameterValue ?? "")) });
                    break;
                case DynamoDBEntryType.String:
                    parameterDictionary.Add(parameterName, new AttributeValue() { S = parameterValue });
                    break;
                default:
                    parameterDictionary.Add(parameterName, new AttributeValue() { S = parameterValue });
                    break;
            }
        }

        public void AddParameter(string parameterName, string dynamoDBEntryType, string parameterValue)
        {
            if (!parameterName.StartsWith(":"))
            {
                throw(new ArgumentException(ErrorStrings.ParameterMissingColon));
            }
            if (!Regex.IsMatch(parameterName.TrimStart(':'), ResourceStrings.RegexParameterValidation))
            {
                throw (new ArgumentException(ErrorStrings.ParameterNonAlphaNumeric));
            }
            switch (dynamoDBEntryType.ToLower())
            {
                case "numeric":
                case "n":
                    parameterDictionary.Add(parameterName, new AttributeValue() { N = parameterValue });
                    break;
                case "binary":
                case "b":
                    parameterDictionary.Add(parameterName, new AttributeValue() { B = new MemoryStream(Encoding.UTF8.GetBytes(parameterValue ?? "")) });
                    break;
                case "string":
                case "s":
                    parameterDictionary.Add(parameterName, new AttributeValue() { S = parameterValue });
                    break;
                default:
                    parameterDictionary.Add(parameterName, new AttributeValue() { S = parameterValue });
                    break;
            }
        }

        public void ClearParameters()
        {
            parameterDictionary = new Dictionary<string, AttributeValue>();
        }

        public void RemoveParameter(string parameterName)
        {
            parameterDictionary.Remove(parameterName);
        }

        public void Merge(DynamoDbParameterModel modelToMerge)
        {
            foreach(var parameter in modelToMerge.ParameterDictionary)
            {
                parameterDictionary.Add(parameter.Key, parameter.Value);
            }
        }

    }
}
