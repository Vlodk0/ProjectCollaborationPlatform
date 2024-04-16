using Microsoft.AspNetCore.Mvc;
using ProjectCollaborationPlatform.BL.Interfaces;
using ProjectCollaborationPlatform.BL.Services;
using ProjectCollaborationPlatform.Domain.DTOs;
using ProjectCollaborationPlatform.Domain.Enums;
using ProjectCollaborationPlatform.Domain.Helpers;

namespace ProjectCollaborationPlatform.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FunctionalityBlockController : ControllerBase
    {
        private readonly IFunctionalityBlockService _functionalityBlockService;

        public FunctionalityBlockController(IFunctionalityBlockService functionalityBlockService)
        {
            _functionalityBlockService = functionalityBlockService;
        }

        [HttpPost("{boardId:Guid}")]
        public async Task<IActionResult> CreateFunctionalityBlock([FromBody] FunctionalityBlockDTO functionalityBlockDTO,
            [FromRoute] Guid boardId, CancellationToken token)
        {
            if (functionalityBlockDTO == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Bad request",
                    Detail = "Can't create a FunctionalityBlock"
                };
            }

            var funcBlock = await _functionalityBlockService.GetFunctionalityBlockById(functionalityBlockDTO.Id,
                token);

            if (funcBlock != null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Title = "Error creating FunctionalityBlock",
                    Detail = "FunctionalityBlock is already exist"
                };
            }

            if (await _functionalityBlockService.CreateFunctionalityBlock(functionalityBlockDTO, boardId))
            {
                var createdFuncBlock = await _functionalityBlockService
                    .GetFunctionalityBlockById(functionalityBlockDTO.Id, token);

                return Created("api/functionalityBlock", createdFuncBlock);
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

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateFunctionalityBlock([FromBody] FunctionalityBlockDTO functionalityBlockDTO,
            [FromRoute] Guid id, CancellationToken token)
        {
            var funcBlockToUpdate = await _functionalityBlockService.GetFunctionalityBlockById(id, token);

            if (funcBlockToUpdate == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "FunctionalityBlock not found",
                    Detail = "FunctionalityBlock with such id not found"
                };
            }

            if (await _functionalityBlockService.UpdateFunctionalityBlock(id, functionalityBlockDTO.Task))
            {
                return Ok("FunctionalityBlock updated successfully");
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

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteFunctionalityBlock([FromRoute] Guid id, CancellationToken token)
        {
            var funcBlockToDelete = await _functionalityBlockService.GetFunctionalityBlockById(id, token);

            if (funcBlockToDelete == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Projects not found",
                    Detail = "Project with such name not found"
                };
            }

            if(await _functionalityBlockService.DeleteFunctionalityBlock(id))
            {
                return Ok("FunctionalityBlock deleted successfully");
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

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetFunctionalityBlockById([FromRoute] Guid id, CancellationToken token)
        {
            var funcBlock = await _functionalityBlockService.GetFunctionalityBlocksByBoardId(id, token);
            if (funcBlock == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Project not found",
                    Detail = "Project with such id doesn't exist"
                };
            }
            return Ok(funcBlock);
        }

        [HttpPatch("{id:Guid}")]
        public async Task<IActionResult> UpdateFunctionalityBlockByStatus([FromRoute] Guid id, [FromBody] StatusEnum status, 
            CancellationToken token)
        {
            var funcBlock = await _functionalityBlockService.GetFunctionalityBlockById(id, token);

            if (funcBlock == null)
            {
                throw new CustomApiException()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Title = "Project not found",
                    Detail = "Project with such id doesn't exist"
                };
            }

            if (await _functionalityBlockService.UpdateFunctionalityBlockStatus(id, status))
            {
                return Ok("Status updated successfully");
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
