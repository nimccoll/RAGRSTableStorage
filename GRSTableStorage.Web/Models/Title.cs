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
    public class Title : EntityBase
    {
        #region Declarations

        private const string _tableName = "Title";

        #endregion

        #region Properties

        public string TitleId
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
        public string BookTitle { get; set; }
        public string Type { get; set; }
        public string PublisherId
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
        public List<string> AuthorIds { get; set; }

        private string _price = "0.00";
        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                decimal val;
                if (decimal.TryParse(value, out val))
                {
                    _price = val.ToString("N2");
                }
                else
                {
                    throw new InvalidOperationException("Price must be a decimal number");
                }
            }
        }

        private string _advance = "0.00";
        public string Advance {
            get
            {
                return _advance;
            }
            set
            {
                decimal val;
                if (decimal.TryParse(value, out val))
                {
                    _advance = val.ToString("N2");
                }
                else
                {
                    throw new InvalidOperationException("Advance must be a decimal number");
                }
            }
        }
        public int Royalty { get; set; }
        public int YearToDateSales { get; set; }
        public string Notes { get; set; }
        public DateTime PublishDate { get; set; }

        #endregion

        #region Constructors

        public Title()
        {
            this.EntityType = "Title";
            this.Version = "1.0";
        }

        public Title(IConfiguration configuration): this()
        {
            if (Configuration == null)
            {
                Configuration = configuration;
            }
        }

        #endregion

        #region Methods

        public decimal GetPrice()
        {
            return decimal.Parse(this.Price);
        }

        public decimal GetAdvance()
        {
            return decimal.Parse(this.Advance);
        }

        public static List<Title> List()
        {
            CloudTable table = GetTable(_tableName);

            TableQuery<Title> listTitles = new TableQuery<Title>();
            List<Title> titles = table.ExecuteQuery(listTitles).ToList();
            if (titles == null) titles = new List<Title>();

            return titles;
        }

        public static List<Title> ListTitlesByAuthor(string authorId)
        {
            List<Title> titles = new List<Title>();
            List<AuthorTitle> authorTitles = AuthorTitle.ListTitlesByAuthor(authorId);

            if (authorTitles.Count > 0)
            {
                foreach (AuthorTitle authorTitle in authorTitles)
                {
                    Title title = Get(authorTitle.TitleId);
                    if (title != null)
                    {
                        titles.Add(title);
                    }
                }
            }

            return titles;
        }

        public static List<Title> ListTitlesByPublisher(string publisherId)
        {
            CloudTable table = GetTable(_tableName);

            TableQuery<Title> listTitles = new TableQuery<Title>().Where(TableQuery.GenerateFilterCondition("PublisherId", QueryComparisons.Equal, publisherId));
            List<Title> titles = table.ExecuteQuery(listTitles).ToList();
            if (titles == null) titles = new List<Title>();

            return titles;
        }

        public static Title Get(string titleId)
        {
            CloudTable table = GetTable(_tableName);

            TableQuery<Title> getTitle = new TableQuery<Title>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, titleId));
            Title title = table.ExecuteQuery(getTitle).FirstOrDefault();
            if (title != null)
            {
                title.AuthorIds = new List<string>();
                List<AuthorTitle> authors = AuthorTitle.ListAuthorsByTitle(titleId);
                foreach(AuthorTitle author in authors)
                {
                    title.AuthorIds.Add(author.AuthorId);
                }
            }

            return title;
        }

        public static void Create(Title title)
        {
            CloudTable table = GetTable(_tableName);

            TableOperation createTitle = TableOperation.Insert(title);
            table.Execute(createTitle);
            List<AuthorTitle> authors = new List<AuthorTitle>();
            foreach (string authorId in title.AuthorIds)
            {
                authors.Add(new AuthorTitle() { AuthorId = authorId, TitleId = title.TitleId });
            }
            AuthorTitle.Create(authors);
        }

        public static void Update(Title title)
        {
            CloudTable table = GetTable(_tableName);

            TableOperation updateTitle = TableOperation.Replace(title);
            table.Execute(updateTitle);
            List<AuthorTitle> authors = new List<AuthorTitle>();
            foreach (string authorId in title.AuthorIds)
            {
                authors.Add(new AuthorTitle() { AuthorId = authorId, TitleId = title.TitleId });
            }
            AuthorTitle.Create(authors);
        }

        public static void Delete(Title title)
        {
            CloudTable table = GetTable(_tableName);

            TableOperation deleteTitle = TableOperation.Delete(title);
            table.Execute(deleteTitle);
            List<AuthorTitle> authors = new List<AuthorTitle>();
            foreach (string authorId in title.AuthorIds)
            {
                authors.Add(new AuthorTitle() { AuthorId = authorId, TitleId = title.TitleId, ETag = "*" });
            }
            AuthorTitle.Delete(authors);
        }

        #endregion
    }
}