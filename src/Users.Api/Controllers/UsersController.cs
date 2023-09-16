using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Users.Api.Constants;
using Users.Api.Contracts.Requests;
using Users.Api.Contracts.Responses;
using Users.Api.Mappings;
using Users.Application.Providers;
using Users.Application.Services;

namespace Users.Api.Controllers
{
    [ApiVersion(1.0)]
    [Consumes("application/json"), Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UsersController(IUserService userService, IDateTimeProvider dateTimeProvider)
        {
            _userService = userService;
            _dateTimeProvider = dateTimeProvider;
        }


        [HttpGet(ApiEndpoints.Users.Get)]
        [SwaggerOperation("Gets a user by id")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(UserResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var response = user.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Users.GetAll)]
        [SwaggerOperation("Gets all users")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(UsersResponse))]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var users = await _userService.GetAllAsync();
            var response = users.MapToResponse();
            return Ok(response);
        }

        [HttpPost(ApiEndpoints.Users.Create)]
        [SwaggerOperation("Creates a user")]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(UserResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            var user = request.MapToUser(_dateTimeProvider);
            await _userService.CreateAsync(user);

            var response = user.MapToResponse();
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, response);
        }

        [HttpDelete(ApiEndpoints.Users.Delete)]
        [SwaggerOperation("Deletes a user")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Delete([FromBody] DeleteUserRequest request, CancellationToken cancellationToken = default)
        {
            var deleted = await _userService.DeleteByIdAsync(request.Id);

            if (deleted == false)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
