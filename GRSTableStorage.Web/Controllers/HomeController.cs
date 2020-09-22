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
using GRSTableStorage.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace GRSTableStorage.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Authors()
        {
            Author.LocationMode = LocationMode.PrimaryOnly;
            if (Request.Query.ContainsKey("location"))
            {
                if (Request.Query["location"] == "secondary")
                {
                    Author.LocationMode = LocationMode.SecondaryOnly;
                }
            }
            if (Author.Configuration == null) Author.Configuration = _configuration;
            List<Author> authors = Author.List();
            return View(authors);
        }

        public IActionResult Publishers()
        {
            Publisher.LocationMode = LocationMode.PrimaryOnly;
            if (Request.Query.ContainsKey("location"))
            {
                if (Request.Query["location"] == "secondary")
                {
                    Publisher.LocationMode = LocationMode.SecondaryOnly;
                }
            }
            if (Publisher.Configuration == null) Publisher.Configuration = _configuration;
            List<Publisher> publishers = Publisher.List();
            return View(publishers);
        }

        public IActionResult Titles()
        {
            Title.LocationMode = LocationMode.PrimaryOnly;
            if (Request.Query.ContainsKey("location"))
            {
                if (Request.Query["location"] == "secondary")
                {
                    Title.LocationMode = LocationMode.SecondaryOnly;
                }
            }
            if (Title.Configuration == null) Title.Configuration = _configuration;
            List<Title> titles = Title.List();
            return View(titles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
