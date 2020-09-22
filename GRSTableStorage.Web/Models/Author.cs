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
    public class Author : EntityBase
    {
        #region Declarations

        private const string _tableName = "Author";

        #endregion

        #region Properties

        public string AuthorId {
            get
            {
                return this.RowKey;
            }
            set
            {
                this.RowKey = value;
            }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
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
        public string PostalCode { get; set; }
        public bool HasContract { get; set; }

        #endregion

        #region Constructors

        public Author()
        {
            this.EntityType = "Author";
            this.Version = "1.0";
        }

        public Author(IConfiguration configuration): this()
        {
            if (Configuration == null)
            {
                Configuration = configuration;
            }
        }

        #endregion

        #region Methods

        public static List<Author> List()
        {
            CloudTable table = GetTable(_tableName);

            TableQuery<Author> listAuthors = new TableQuery<Author>();
            List<Author> authors = table.ExecuteQuery(listAuthors).ToList();
            if (authors == null) authors = new List<Author>();

            return authors;
        }

        public static Author Get(string authorId)
        {
            CloudTable table = GetTable(_tableName);

            TableQuery<Author> getAuthor = new TableQuery<Author>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, authorId));
            Author author = table.ExecuteQuery(getAuthor).FirstOrDefault();

            return author;
        }

        public static void Create(Author author)
        {
            CloudTable table = GetTable(_tableName);

            TableOperation createAuthor = TableOperation.Insert(author);
            table.Execute(createAuthor);
        }

        public static void Update(Author author)
        {
            CloudTable table = GetTable(_tableName);

            TableOperation updateAuthor = TableOperation.Replace(author);
            table.Execute(updateAuthor);
        }

        public static void Delete(Author author)
        {
            CloudTable table = GetTable(_tableName);

            TableOperation deleteAuthor = TableOperation.Delete(author);
            table.Execute(deleteAuthor);
        }

        #endregion
    }
}