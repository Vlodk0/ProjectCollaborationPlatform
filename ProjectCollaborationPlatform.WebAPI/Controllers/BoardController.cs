using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.DAL.Data.Models;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Helpers;

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
        public async Task<IActionResult> CreateBoard([FromBody] BoardDTO board, CancellationToken token)
        {
            if (board == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Bad request",
                    Detail = "Can't create a board"
                };
            }

            var brd = await _boardService.GetBoardById(board.Id, token);

            if (brd != null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Error creating the board",
                    Detail = "Board is already in use"
                };
            }
            if (await _boardService.CreateBoard(board))
            {
                var createdProject = await _boardService.GetBoardByName(board.Name, token);
                return Created("api/board", createdProject);
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

        [HttpGet("{name}")]
        public async Task<IActionResult> GetBoardByName([FromRoute] string name, CancellationToken token)
        {
            var board = await _boardService.GetBoardByName(name, token);
            if (board != null)
            {
                return StatusCode(StatusCodes.Status200OK, board);
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

        [HttpPut]
        public async Task<IActionResult> UpdateBoard([FromBody] BoardDTO boardDTO, CancellationToken token)
        {
            var boardToUpdate = await _boardService.GetBoardById(boardDTO.Id, token);

            if (boardToUpdate == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Board not found",
                    Detail = $"Board with {boardDTO.Id} id doesn't exist"
                };
            }

            if (await _boardService.UpdateBoard(boardDTO))
            {
                return StatusCode(StatusCodes.Status200OK, "Project updated succesfully");
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
        public async Task<ActionResult<Board>> DeleteBoardByName([FromRoute] string name, CancellationToken token)
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

            if (await _boardService.DeleteBoard(boardToDelete.Id))
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
