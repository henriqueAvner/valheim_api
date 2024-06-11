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
    public IActionResult AddPlayer([FromBody] Player player)
    {
        return Created("", _repository.AddPlayer(player));
    }

    [HttpGet]
    public IActionResult GetPlayers()
    {
        return Ok(_repository.GetPlayers());
    }

    [HttpDelete("{PlayerId}")]
    public IActionResult DeletePlayer(int PlayerId)
    {
        try
        {
            _repository.DeletePlayer(PlayerId);
            return NoContent();
        }
        catch (Exception ex)
        {

            return BadRequest(new { message = ex.Message });
        }

    }

}