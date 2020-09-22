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
    internal class AuthorTitle : EntityBase
    {
        #region Declarations

        private const string _tableName = "AuthorTitle";

        #endregion

        #region Properties

        public string AuthorId
        {
            get
            {
                return this.RowKey;
            }
            set
            {
                this.RowKey = value;
            }
        }

        public string TitleId
        {
            get
            {
                return this.PartitionKey;
            }
            set
            {
                this.PartitionKey = value;
            }
        }

        #endregion

        #region Constructors
        
        public AuthorTitle()
        {
            this.EntityType = "AuthorTitle";
            this.Version = "1.0";
        }
        
        public AuthorTitle(IConfiguration configuration): this()
        {
            if (Configuration == null)
            {
                Configuration = configuration;
            }
        }
        #endregion

        #region Methods

        public static List<AuthorTitle> ListTitlesByAuthor(string authorId)
        {
            CloudTable table = GetTable(_tableName);

            TableQuery<AuthorTitle> listTitles = new TableQuery<AuthorTitle>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, authorId));
            List<AuthorTitle> titles = table.ExecuteQuery(listTitles).ToList();
            if (titles == null) titles = new List<AuthorTitle>();

            return titles;
        }

        public static List<AuthorTitle> ListAuthorsByTitle(string titleId)
        {
            CloudTable table = GetTable(_tableName);

            TableQuery<AuthorTitle> listAuthors = new TableQuery<AuthorTitle>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, titleId));
            List<AuthorTitle> authors = table.ExecuteQuery(listAuthors).ToList();
            if (authors == null) authors = new List<AuthorTitle>();

            return authors;
        }

        public static void Create(List<AuthorTitle> authors)
        {
            CloudTable table = GetTable(_tableName);

            TableBatchOperation create = new TableBatchOperation();
            foreach (AuthorTitle author in authors)
            {
                create.InsertOrReplace(author);
            }

            if (create.Count > 0)
            {
                table.ExecuteBatch(create);
            }
        }

        public static void Delete(List<AuthorTitle> authors)
        {
            CloudTable table = GetTable(_tableName);

            TableBatchOperation delete = new TableBatchOperation();
            foreach (AuthorTitle author in authors)
            {
                delete.Delete(author);
            }

            if (delete.Count > 0)
            {
                table.ExecuteBatch(delete);
            }
        }

        #endregion
    }
}