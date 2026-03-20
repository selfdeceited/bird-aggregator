using BirdAggregator.Application.Configuration;
using BirdAggregator.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BirdAggregator.Host.Controllers;

[Route("api/[controller]")]
public class LinksController(
    AppSettings appSettings,
    IPictureHostingService pictureHostingService) : Controller
{
    [HttpGet("github")]
    public IActionResult Github()
    {
        return Ok($"https://github.com/{appSettings.Github}");
    }

    [HttpGet("user")]
    public new IActionResult User()
    {
        return Ok(appSettings.UserName);
    }

    [HttpGet("hosting")]
    public IActionResult Hosting()
    {
        return Ok(pictureHostingService.GetUserLink());
    }
}
