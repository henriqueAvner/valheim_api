using api_valheim.Repository;
using api_valheim.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

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
    public IActionResult AddItem([FromBody] Item item)
    {
        var tokenClaims = HttpContext.User.Identity as ClaimsIdentity;

        var name = tokenClaims!.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        var email = tokenClaims!.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        _repository.AddItem(item);

        return Created("", new { item, name, email });
    }

    [HttpGet]
    public IActionResult GetItems()
    {
        return Ok(_repository.GetItems());
    }

}