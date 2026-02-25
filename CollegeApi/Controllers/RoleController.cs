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
    public class RoleController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Role> _roleRepository;
        private APIResponse _apiResponse;

        public RoleController(IMapper mapper, ICollegeRepository<Role> roleRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _apiResponse = new();

        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateRole(RoleDTO dto)
        {

            try
            {
                if (dto == null)
                {
                    return BadRequest();
                }

                Role role = _mapper.Map<Role>(dto);
                role.IsDeleted = false;
                role.CreatedDate = DateTime.Now;
                role.ModifiedDate = DateTime.Now;

                var result = await _roleRepository.CreateAsync(role);

                dto.Id = result.Id;
                _apiResponse.Data = dto;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
                //return CreatedAtRoute("GetRoleById", new {id=dto.Id});
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
        public async Task<ActionResult<APIResponse>> GetAllRolesAsync()
        {

            try
            {
                var roles = await _roleRepository.GetAllAsync();

                _apiResponse.Data = _mapper.Map<List<RoleDTO>>(roles);
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
        [Route("{id:int}",Name ="GetRolById")]
        public async Task<ActionResult<APIResponse>> GetRolByIdAsync(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                
                var role = await _roleRepository.GetAsync(role=>role.Id==id);

                if (role != null)
                {
                    _apiResponse.Data = _mapper.Map<RoleDTO>(role);
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
        [Route("{Name:alpha}", Name = "GetRolByName")]
        public async Task<ActionResult<APIResponse>> GetRolByName(string Name)
        {

            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return BadRequest();
                }


                var role = await _roleRepository.GetAsync(role => role.RoleName == Name);

                if (role!=null)
                {
                    _apiResponse.Data = _mapper.Map<RoleDTO>(role);
                    _apiResponse.Status = true;
                    _apiResponse.StatusCode = HttpStatusCode.OK;

                    return Ok(_apiResponse);
                }
                else
                {
                    return NotFound($"Role not found with name:{Name}");
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
    }
}
