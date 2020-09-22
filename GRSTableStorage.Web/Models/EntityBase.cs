//===============================================================================
// Microsoft FastTrack for Azure
// Azure Read Access Geo-Redundant Table Storage Samples
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;

namespace GRSTableStorage.Web.Models
{
    [Serializable]
    public abstract class EntityBase : TableEntity
    {
        public static IConfiguration Configuration { get; set; }
        public static LocationMode LocationMode { get; set; }
        public string EntityType { get; set; }
        public string Version { get; set; }

        public static CloudTable GetTable(string tableName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Configuration.GetValue<string>("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.LocationMode = LocationMode;
            CloudTable table = tableClient.GetTableReference(tableName);
            return table;
        }
    }
}