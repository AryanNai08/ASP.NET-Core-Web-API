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

        public RoleController(IMapper mapper,ICollegeRepository<Role> roleRepository) 
        {
            _mapper= mapper;
            _roleRepository= roleRepository;
            _apiResponse = new();

        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<APIResponse>> CreateRole(RoleDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            Role role=_mapper.Map<Role>(dto);
            role.IsDeleted = false;
            role.CreatedDate=DateTime.Now;
            role.ModifiedDate=DateTime.Now;

         var result= await  _roleRepository.CreateAsync(role);

            dto.Id = result.Id;
            _apiResponse.Data = dto; 
            _apiResponse.Status = true;
            _apiResponse.StatusCode=HttpStatusCode.OK;

            return Ok(_apiResponse);
            //return CreatedAtRoute("GetRoleById", new {id=dto.Id});
        }
    }
}
