using api_valheim.Repository;
using api_valheim.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace api_valheim.controllers;

[ApiController]
[Route("players")]
public class PlayerController : Controller
{
    protected readonly IPlayerRepository _repository;

    public PlayerController(IPlayerRepository repository)
    {
        _repository = repository;
    }


    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(policy: "levelB")]
    public IActionResult AddPlayer([FromBody] Player player)
    {
        return Created("", _repository.AddPlayer(player));
    }

    [HttpGet]
    public IActionResult GetPlayers()
    {
        return Ok(_repository.GetPlayers());
    }

}