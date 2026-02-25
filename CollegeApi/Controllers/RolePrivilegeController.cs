using AutoMapper;
using CollegeApi.Data;
using CollegeApi.Data.Repository;
using CollegeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePrivilegeController : ControllerBase
    {


        private readonly IMapper _mapper;
        private readonly ICollegeRepository<RolePrivilege> _rolePrivilegeRepository;
        private APIResponse _apiResponse;

        public RolePrivilegeController(IMapper mapper, ICollegeRepository<RolePrivilege> rolePrivilegeRepository)
        {
            _mapper = mapper;
            _rolePrivilegeRepository = rolePrivilegeRepository;
            _apiResponse = new();

        }


        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<ActionResult<APIResponse>> CreateRolePrivilege(RolePrivilegeDTO dto)
        {

            try
            {
                if (dto == null)
                {
                    return BadRequest();
                }

                RolePrivilege rolePrivilege = _mapper.Map<RolePrivilege>(dto);
                rolePrivilege.IsDeleted = false;
                rolePrivilege.CreatedDate = DateTime.Now;
                rolePrivilege.ModifiedDate = DateTime.Now;

                var result = await _rolePrivilegeRepository.CreateAsync(rolePrivilege);

                dto.Id = result.Id;
                _apiResponse.Data = dto;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                //return Ok(_apiResponse);
                return CreatedAtRoute("GetRolePrivilegeById", new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Error.Add(ex.Message);
                return _apiResponse;
            }

        }

        [HttpGet]
        [Route("GetAllRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<ActionResult<APIResponse>> GetAllRolesPrivilegeAsync()
        {

            try
            {
                var rolePrivilege = await _rolePrivilegeRepository.GetAllAsync();

                _apiResponse.Data = _mapper.Map<List<RolePrivilegeDTO>>(rolePrivilege);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Error.Add(ex.Message);
                return _apiResponse;
            }

        }


        [HttpGet]
        [Route("{id:int}", Name = "GetRolePrivilegeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegeByIdAsync(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }


                var role = await _rolePrivilegeRepository.GetAsync(role => role.Id == id);

                if (role != null)
                {
                    _apiResponse.Data = _mapper.Map<RolePrivilegeDTO>(role);
                    _apiResponse.Status = true;
                    _apiResponse.StatusCode = HttpStatusCode.OK;

                    return Ok(_apiResponse);
                }
                else
                {
                    return NotFound($"Role not found with sepecifc id :{id}");
                }


            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Error.Add(ex.Message);
                return _apiResponse;
            }

        }



        [HttpGet]
        [Route("{Name:alpha}", Name = "GetRolePrivilegeByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegeByName(string Name)
        {

            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return BadRequest();
                }


                var rolePrivilege = await _rolePrivilegeRepository.GetAsync(role => role.RolePrivilegeName.Contains(Name));

                if (rolePrivilege != null)
                {
                    _apiResponse.Data = _mapper.Map<RolePrivilegeDTO>(rolePrivilege);
                    _apiResponse.Status = true;
                    _apiResponse.StatusCode = HttpStatusCode.OK;

                    return Ok(_apiResponse);
                }
                else
                {
                    return NotFound($"Role privileges not found with name:{Name}");
                }


            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Error.Add(ex.Message);
                return _apiResponse;
            }

        }



        [HttpGet]
        [Route("AllRolePrivilegesByRoleId", Name = "GetAllRolePrivilegesByRoleId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegesByRoleIdAsync(int roleId)
        {
            try
            {
                var rolePrivileges = await _rolePrivilegeRepository.GetAllByFilterAsync(rolePrivilege => rolePrivilege.RoleId == roleId);

                _apiResponse.Data = _mapper.Map<List<RolePrivilegeDTO>>(rolePrivileges);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            //catch (Exception ex)
            //{
            //    _apiResponse.Status = false;
            //    _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            //    _apiResponse.Error.Add(ex.Message);
            //    return _apiResponse;
            //}
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Error = new List<string> { ex.Message };

                return StatusCode(500, _apiResponse);
            }
        }



        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateRolePrivilege(RolePrivilegeDTO dto)
        {
            try
            {
                if (dto == null || dto.Id <= 0)
                {
                    return BadRequest();
                }

                var existingRolePrivilege = await _rolePrivilegeRepository.GetAsync(role => role.Id == dto.Id, true);

                if (existingRolePrivilege == null)
                {
                    return BadRequest($"RolePrivilege not found with id:{dto.Id}");
                }

                var newRolePrivilege = _mapper.Map<RolePrivilege>(dto);

                await _rolePrivilegeRepository.UpdateAsync(newRolePrivilege);
                _apiResponse.Data = newRolePrivilege;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Error.Add(ex.Message);
                return _apiResponse;
            }
        }




        [HttpDelete]
        [Route("Delete{id:int}", Name = "DeleteRolePrivilegeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteRole(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var Role = await _rolePrivilegeRepository.GetAsync(role => role.Id == id);

                if (Role == null)
                {
                    return BadRequest($"RolePrivilege not found with id:{id}");
                }


                await _rolePrivilegeRepository.DeleteAsync(Role);
                _apiResponse.Data = true;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Error.Add(ex.Message);
                return _apiResponse;
            }
        }
    }
}
