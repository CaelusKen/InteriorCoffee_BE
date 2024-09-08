using InteriorCoffee.Application.Constants;
using InteriorCoffee.Application.DTOs.Role;
using InteriorCoffee.Application.Services.Implements;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InteriorCoffeeAPIs.Controllers
{
    [ApiController]
    public class RoleController : BaseController<RoleController>
    {
        private readonly IRoleService _roleService;

        public RoleController(ILogger<RoleController> logger, IRoleService roleService) : base(logger)
        {
            _roleService = roleService;
        }

        [HttpGet(ApiEndPointConstant.Role.RolesEndpoint)]
        [ProducesResponseType(typeof(List<Role>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRoles();
            return Ok(result);
        }

        [HttpGet(ApiEndPointConstant.Role.RoleEndpoint)]
        [ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a role by id")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var result = await _roleService.GetRoleById(id);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Role.RolesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create role")]
        public async Task<IActionResult> CreateRole(RoleDTO role)
        {
            await _roleService.CreateRole(role);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndPointConstant.Role.RoleEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update a role's data")]
        public async Task<IActionResult> UpdateRoles(string id, [FromBody] RoleDTO updateRole)
        {
            await _roleService.UpdateRole(id, updateRole);
            return Ok("Action success");
        }

        [HttpDelete(ApiEndPointConstant.Role.RoleEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a role")]
        public async Task<IActionResult> DeleteRoles(string id)
        {
            await _roleService.DeleteRole(id);
            return Ok("Action success");
        }
    }
}
