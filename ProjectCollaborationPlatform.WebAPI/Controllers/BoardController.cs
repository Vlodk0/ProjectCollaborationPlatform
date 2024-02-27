using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{

    [Route("api/[controller]")]
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
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                var brd = await _boardService.GetBoardById(board.Id);

                if (brd == null)
                {
                    ModelState.AddModelError("id", "Project id already in use");
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }
                if (await _boardService.CreateBoard(board))
                {
                    var createdProject = await _boardService.GetBoardByName(board.Name);
                    return CreatedAtAction(nameof(GetBoardByName), new { title = createdProject.Name }, createdProject);
                }

            }
            catch { }

            return StatusCode(StatusCodes.Status500InternalServerError,
        "Error retrieving data from the database");
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetBoardByName([FromRoute] string name)
        {
            var board = await _boardService.GetBoardByName(name);
            if (board == null)
            {
                return StatusCode(StatusCodes.Status200OK, "Board with such name doesn't exist");
            }
            return StatusCode(StatusCodes.Status400BadRequest, board);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBoard([FromBody] BoardDTO boardDTO)
        {
            try
            {
                if (boardDTO.Id != boardDTO.Id)
                {
                    return StatusCode(StatusCodes.Status200OK, "Project ID mismatch");
                }

                var boardToUpdate = await _boardService.GetBoardById(boardDTO.Id);

                if (boardToUpdate == null)
                {
                    return NotFound($"Project with ID = {boardDTO.Id} not found");
                }

                if (await _boardService.UpdateBoard(boardDTO))
                {
                    return StatusCode(StatusCodes.Status200OK, "Project updated succesfully");
                }
            }
            catch { }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult<Board>> DeleteBoardByName([FromRoute] string name)
        {
            try
            {
                var boardToDelete = await _boardService.GetBoardByName(name);

                if (boardToDelete == null)
                {
                    return NotFound($"Board with name = {name} not found");
                }

                if (await _boardService.DeleteBoard(boardToDelete.Id))
                {
                    return StatusCode(StatusCodes.Status200OK, "Board deleted succesfully");
                }
            }
            catch { }

            return StatusCode(StatusCodes.Status500InternalServerError,
                       "Error deleting data");
        }
    }
}
