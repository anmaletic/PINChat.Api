using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;

namespace PINChat.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SettingController
{
    private readonly ISettingData _settingData;

    public SettingController(ISettingData settingData)
    {
        _settingData = settingData;
    }
    
    [HttpPost]
    [Route("GetByKey")]
    public SettingModel GetByKey(SettingModel model)
    {
            var messages = _settingData.GetValueByKey(model.Key!);
            return messages;
    }
}