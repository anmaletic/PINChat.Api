﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;

namespace PINChat.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupController : ControllerBase
{
    private IGroupData _groupData;

    public GroupController(IGroupData groupData)
    {
        _groupData = groupData;
    }

    [HttpGet]
    [Route("GetAll")]
    public List<GroupModel> GetAll()
    {
        var groups = _groupData.GetAllGroups();

        return groups;
    }
}