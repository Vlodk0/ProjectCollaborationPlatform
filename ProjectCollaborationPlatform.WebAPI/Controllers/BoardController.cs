using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [Authorize(Policy = "ProjectOwner")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpPost("{id:Guid}")]
        public async Task<IActionResult> CreateBoard([FromBody] BoardDTO board, [FromRoute] Guid id, CancellationToken token)
        {
            if (board == null)
            {
                throw new CustomApiException()//exceptions in controller are not OK. simply return BadRequest
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Bad request",
                    Detail = "Can't create a board"
                };
            }

            var brd = await _boardService.GetBoardByName(board.Name, token);

            if (brd != null)//why can't we have 2 boards with the same name but different guids? questionable but maybe it is important for your BL
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Error creating the board",
                    Detail = "Board is already in use"
                };
            }
            if (await _boardService.CreateBoard(id, board))//you could simply return the whole object after context.SaveChanges(). there is no need to call context twice 
                                                           //https://stackoverflow.com/questions/5212751/how-can-i-retrieve-id-of-inserted-entity-using-entity-framework
            {
                var createdProject = await _boardService.GetBoardByName(board.Name, token);
                return Created("api/board", createdProject);
            }
            else//this else statement and exception are not needed. there is no reason to return 500 at the end of the method
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = "Error occured while server running"
                };
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetBoardByName([FromRoute] string name, CancellationToken token)
        {
            var board = await _boardService.GetBoardByName(name, token);
            if (board != null)
            {
                return StatusCode(StatusCodes.Status200OK, board);//why not return Ok(board);?
            }
            else
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Board not found",
                    Detail = "Board with such name doesn't exist"
                };
            }
        }

        [HttpPut("Board/{id:Guid}")]
        public async Task<IActionResult> UpdateBoard([FromBody] BoardDTO boardDTO, [FromRoute] Guid id)//better name it projectId instead of id. not clear naming
        {
            if (await _boardService.UpdateBoard(id, boardDTO.Name))
            {
                return StatusCode(StatusCodes.Status200OK, "Board updated succesfully");
            }
            else
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = "Error occured while server running"
                };
            }
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult<Board>> DeleteBoardByName([FromRoute] string name, CancellationToken token)//why ActionResult<Board> but not IActionResult?
        {
            var boardToDelete = await _boardService.GetBoardByName(name, token);

            if (boardToDelete == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Board not found",
                    Detail = "Board with such name doesn't exist"
                };
            }

            if (await _boardService.DeleteBoard(boardToDelete.Name, token))
            {
                return StatusCode(StatusCodes.Status200OK, "Board deleted succesfully");
            }
            else
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = "Error occured while server running"
                };
            }
        }
    }
}
