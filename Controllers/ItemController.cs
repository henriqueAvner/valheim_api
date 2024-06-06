using api_valheim.Repository;
using api_valheim.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace api_valheim.controllers;

[ApiController]
[Route("items")]
public class ItemController : Controller
{
    protected readonly IItemRepository _repository;

    public ItemController(IItemRepository repository)
    {
        _repository = repository;
    }


    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(policy: "levelA")]
    public IActionResult AddItem([FromBody] Item item)
    {
        return Created("", _repository.AddItem(item));
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(policy: "levelB")]
    public IActionResult GetItems()
    {
        return Ok(_repository.GetItems());
    }

}