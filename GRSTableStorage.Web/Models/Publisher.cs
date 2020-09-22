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
using System.Collections.Generic;
using System.Linq;

namespace GRSTableStorage.Web.Models
{
    [Serializable]
    public class Publisher : EntityBase
    {
        #region Declarations

        private const string _tableName = "Publisher";

        #endregion

        #region Properties

        public string PublisherId {
            get
            {
                return this.RowKey;
            }
            set
            {
                this.RowKey = value;
            }
        }
        public string Name { get; set; }
        public string City { get; set; }
        public string State {
            get
            {
                return this.PartitionKey;
            }
            set
            {
                this.PartitionKey = value;
            }
        }
        public string Country { get; set; }

        #endregion

        #region Constructors

        public Publisher()
        {
            this.EntityType = "Publisher";
            this.Version = "1.0";
        }

        public Publisher(IConfiguration configuration): this()
        {
            if (Configuration == null)
            {
                Configuration = configuration;
            }
        }

        #endregion

        #region Methods

        public static List<Publisher> List()
        {
            CloudTable table = GetTable(_tableName);

            TableQuery<Publisher> listPublishers = new TableQuery<Publisher>();
            List<Publisher> publishers = table.ExecuteQuery(listPublishers).ToList();
            if (publishers == null) publishers = new List<Publisher>();

            return publishers;
        }

        public static Publisher Get(string publisherId)
        {
            CloudTable table = GetTable(_tableName);

            TableQuery<Publisher> getPublisher = new TableQuery<Publisher>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, publisherId));
            Publisher publisher = table.ExecuteQuery(getPublisher).FirstOrDefault();

            return publisher;
        }

        public static void Create(Publisher publisher)
        {
            CloudTable table = GetTable(_tableName);

            TableOperation createPublisher = TableOperation.Insert(publisher);
            table.Execute(createPublisher);

        }

        public static void Update(Publisher publisher)
        {
            CloudTable table = GetTable(_tableName);

            TableOperation updatePublisher = TableOperation.Replace(publisher);
            table.Execute(updatePublisher);
        }

        public static void Delete(Publisher publisher)
        {
            CloudTable table = GetTable(_tableName);

            TableOperation deletePublisher = TableOperation.Delete(publisher);
            table.Execute(deletePublisher);
        }

        #endregion
    }
}