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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(policy: "levelB")]
    public IActionResult AddCharacter([FromBody] Character character)
    {
        return Created("", _repository.AddCharacter(character));
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(policy: "levelB")]
    public IActionResult GetCharacters()
    {
        return Ok(_repository.GetCharacters());
    }

}