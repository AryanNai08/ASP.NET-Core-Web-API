using AutoMapper;
using CollegeApi.Data;
using CollegeApi.Data.Repository;
using CollegeApi.Models;
using CollegeApi.MyLogging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApi.Controllers
{
    [Route("api/[controller]")] //Defines the base URL route
    [ApiController]//Marks the class as a Web API controller
    [Authorize(AuthenticationSchemes = "LoginForLocalUsers", Roles = "Superadmin,Admin")]
    //[EnableCors(PolicyName = "AllowOnlyLocalhost")]
    public class StudentController : ControllerBase
    {

        private readonly ILogger<StudentController> _Logger;
        
        private readonly IMapper _mapper;

        //private readonly ICollegeRepository<Student> _studentRepository;

        private readonly IStudentRepository _studentRepository;
        public StudentController(ILogger<StudentController> logger, IMapper mapper, IStudentRepository studentRepository)
        {
            
            _Logger = logger;
            _mapper = mapper;
            _studentRepository = studentRepository;

        }

       

        //first end point
        //request method id endpoint
        [HttpGet]
        //api/student/All
        [Route("All", Name = "GetAllStudents")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //[AllowAnonymous]
        //Get all student details
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudent()
        {
            //without linq

            //var students = new List<StudentDTO>();
            //foreach(var item in _dbContext.Students)
            //{
            //    StudentDTO obj = new StudentDTO()
            //    {
            //        Id = item.Id,
            //        Studentname=item.Studentname,
            //        Address=item.Address,
            //        Email=item.Email
            //    };
            //    students.Add(obj);
            //}

            _Logger.LogInformation("GetStudents method started ");
            //with linq


            var students = await _studentRepository.GetAllAsync();

            var studentDTOData = _mapper.Map<List<StudentDTO>>(students);
            //ok-200-success
            return Ok(studentDTOData);

        }



        //Get  single student details by id
        [HttpGet]
        [Route("{id:int}", Name = "GetStudentByid")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDTO>> GetStudentByIdAsync(int id)
        {

            if (id <= 0)
            {
                _Logger.LogWarning("Bad request");
                //bad request-400-client side error
                return BadRequest();
            }

            var Student = await _studentRepository.GetAsync(student => student.Id == id);

            if (Student == null)
            {
                _Logger.LogError("Student not found with given id");
                return NotFound("Student not found");
            }

            var studentDTO = _mapper.Map<StudentDTO>(Student);

            return Ok(studentDTO);



        }


        //Get  single student details by Name
        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentByNameAsync(string name)
        {




            if (string.IsNullOrEmpty(name))
            {
                //bad request-400-client side error
                return BadRequest();
            }

            var Student = await _studentRepository.GetAsync(student => student.Studentname.ToLower().Contains(name.ToLower()));

            if (Student == null)
            {
                return NotFound("Student not found");
            }
            var studentDTO = _mapper.Map<StudentDTO>(Student);

            return Ok(studentDTO);


        }

        //create student record
        [HttpPost]
        [Route("Create")]
        //api/student/create
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody]StudentDTO dto)
        {
            if (dto == null) 
            {
                return BadRequest();
            }



            Student student = _mapper.Map<Student>(dto);

            var studentAfterCreation=await _studentRepository.CreateAsync(student);

            dto.Id = studentAfterCreation.Id;
            //link/location of newly created data
            //status code=201
            return CreatedAtRoute("GetStudentByid", new {id=dto.Id},dto);
            

        }


        //update student api
        [HttpPut]
        [Route("Update")]
        //api/student/update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO dto)
        {
            if(dto == null || dto.Id <= 0)
            {
                return BadRequest();
            }

            var existingStudent = await _studentRepository.GetAsync(student => student.Id == dto.Id, true);

            if (existingStudent == null)
            {
                return NotFound();
            }

            //using asnotracking
            var newrecord = _mapper.Map<Student>(dto);

            await _studentRepository.UpdateAsync(newrecord);

            return NoContent();
        }





        //updatepartial student api
        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/student/id/updatepartial
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentpartial(int id,[FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
            {
                return BadRequest();
            }

            var existingStudent = await _studentRepository.GetAsync(student => student.Id == id, true);

            if (existingStudent == null)
            {
                return NotFound();
            }

            var studentDTO = _mapper.Map<StudentDTO>(existingStudent);
            patchDocument.ApplyTo(studentDTO,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

             existingStudent=_mapper.Map<Student>(studentDTO);
            await _studentRepository.UpdateAsync(existingStudent);

            return NoContent();
        }




        //delete  single student details by id
        [HttpDelete("{id}", Name = "DeleteStudentById")]

        //disable cors for this particular metho
        [DisableCors]
        //api/student/delete/id
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteStudentByIdAsync(int id)
        {
            

            if (id <= 0)
            {
                //bad request-400-client side error
                return BadRequest();
            }

            var Student = await _studentRepository.GetAsync(student => student.Id == id);

            if (Student == null)
            {
                return NotFound("Student id not found");
            }
            else
            {
                await _studentRepository.DeleteAsync(Student);
                return Ok(true);
            }

        }
    }
}
