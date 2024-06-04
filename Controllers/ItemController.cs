using api_valheim.Repository;
using api_valheim.models;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult AddCharacter([FromBody] Item item)
    {
        return Created("", _repository.AddItem(item));
    }

    [HttpGet]
    public IActionResult GetCharacters()
    {
        return Ok(_repository.GetItems());
    }

}