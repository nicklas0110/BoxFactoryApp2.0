
using Application.DTOs;
using Application.Interfaces;
using BoxFactoryApp;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class BoxController : ControllerBase
{
    private IBoxService _boxService;

    public BoxController(IBoxService boxService)
    {
        _boxService = boxService;
    }

    [HttpGet]
    [Route("")]
    public ActionResult<List<Box>> GetAllBoxes()
    {
        return _boxService.GetAllNBoxes();
    }

    [HttpPost]  
    [Route("")]
    public ActionResult<Box> CreateNewBox(BoxDTOs dto)
    {
        try
        {
            var result = _boxService.CreateNewBox(dto);
            return Created("", result);
        }
        catch (ValidationException v)
        {
            return BadRequest(v.Message);
        }
        catch (System.Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("{id}")] //localhost:5001/box/42
    public ObjectResult GetBoxById(int id)
    {
        try
        {
            return new ObjectResult(_boxService.GetBoxById(id));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound("No product found at ID " + id);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.ToString());
        }
    }


    [HttpGet]
    [Route("RebuildDB")]
    public void RebuildDB()
    {
        _boxService.RebuildDB();
    }

    [HttpPut]
    [Route("{id}")] //localhost:5111/box/8732648732
    public ActionResult<Box> UpdateBox([FromRoute] int id, [FromBody] Box box)
    {
        try
        {
            return Ok(_boxService.UpdateBox(id, box));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound("No product found at ID " + id);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.ToString());
        }
    }



    [HttpDelete]
    [Route("{id}")]
    public ActionResult<Box> DeleteBox(int id)
    {
        try
        {
            return Ok(_boxService.DeleteBox(id));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound("No product found at ID " + id);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.ToString());
        }
    }
}