using api_valheim.Repository;
using api_valheim.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace api_valheim.controllers;

[ApiController]
[Route("characters")]
public class CharacterController : Controller
{
    protected readonly ICharacterRepository _repository;

    public CharacterController(ICharacterRepository repository)
    {
        _repository = repository;
    }


    [HttpPost]
    public IActionResult AddCharacter([FromBody] Character character)
    {
        try
        {
            return Created("", _repository.AddCharacter(character));
        }
        catch (Exception ex)
        {

            return BadRequest(new { message = ex.Message });
        }

    }

    [HttpGet]
    public IActionResult GetCharacters()
    {
        return Ok(_repository.GetCharacters());
    }

    [HttpDelete("{CharacterId}")]
    public IActionResult DeleteCharacter(int CharacterId)
    {
        try
        {
            _repository.DeleteCharacter(CharacterId);
            return NoContent();
        }
        catch (Exception ex)
        {

            return NotFound(new { message = ex.Message });
        }

    }

}