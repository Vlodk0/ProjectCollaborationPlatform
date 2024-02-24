using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{

    [Route("api/[contoller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] BoardDTO board)
        {
            try
            {
                if (board == null)
                {
                    return BadRequest();
                }

                var brd = await _boardService.GetBoardById(board.Id);

                if (brd == null)
                {
                    ModelState.AddModelError("id", "Project id already in use");
                    return BadRequest(ModelState);
                }
                if (await _boardService.CreateBoard(board))
                {
                    var createdProject = await _boardService.GetBoardByName(board.Name);
                    return CreatedAtAction(nameof(GetBoardByName), new { title = createdProject.Name }, createdProject);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
        "Error retrieving data from the database");
                }


            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error retrieving data from the database");
            }
        }

        [HttpGet("{name}:string")]
        public async Task<IActionResult> GetBoardByName([FromRoute] string name)
        {
            var board = await _boardService.GetBoardByName(name);
            if (board == null)
            {
                return NotFound("Board with such name doesn't exist");
            }
            return Ok(board);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateBoard(BoardDTO boardDTO)
        {
            try
            {
                if (boardDTO.Id != boardDTO.Id)
                {
                    return BadRequest("Project ID mismatch");
                }

                var boardToUpdate = await _boardService.GetBoardById(boardDTO.Id);

                if (boardToUpdate == null)
                {
                    return NotFound($"Project with ID = {boardDTO.Id} not found");
                }

                if (await _boardService.UpdateBoard(boardDTO))
                {
                    return Ok("Project updated succesfully");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error updating data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Error updating data");
            }
        }

        [HttpDelete("{name:string}")]
        public async Task<ActionResult<Board>> DeleteBoardByName(BoardDTO board)
        {
            try
            {
                var boardToDelete = await _boardService.GetBoardByName(board.Name);

                if (boardToDelete == null)
                {
                    return NotFound($"Project with name = {board.Name} not found");
                }

                if (await _boardService.DeleteBoard(board.Id))
                {
                    return Ok("Project deleted succesfully");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error deleting data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}
