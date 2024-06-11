using api_valheim.Repository;
using api_valheim.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

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
    [Authorize(Policy = "levelA")]
    public IActionResult AddPlayer([FromBody] Player player)
    {
        var tokenClaims = HttpContext.User.Identity as ClaimsIdentity;

        var name = tokenClaims!.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

        var email = tokenClaims!.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

        try
        {
            var newPlayer = _repository.AddPlayer(player);
            return Created("", new { name, email, newPlayer });
        }
        catch (Exception ex)
        {

            return BadRequest(new { message = ex.Message });
        }

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