﻿using BlogApplication.UI.Models;
using BlogApplication.UI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogApplication.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;

        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository)
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
        }

        public async Task<IActionResult> Index()
        {
            var blogPost = await blogPostRepository.GetAllAsync();

            return View(blogPost);
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