using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;

namespace PINChat.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
// [Authorize]
public class ImageController : ControllerBase
{
    private IImageData _imageData;

    public ImageController(IImageData imageData)
    {
        _imageData = imageData;
    }

    [HttpGet("GetImage/{userId}")]
    public IActionResult Get(string userId)
    {
        return File(_imageData.GetUserImage(userId), "image/png");
    }
}