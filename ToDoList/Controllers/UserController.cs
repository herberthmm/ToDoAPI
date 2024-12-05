using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Database;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DbHelper _database;

        public UserController(Context database)
        {
            _database = new DbHelper(database);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            ResponseType responseType = ResponseType.Success;

            try
            {
                bool correct = _database.VerifyLogin(login);
                if (correct)
                {
                    return Ok(ResponseHandler.GetAppResponse(responseType, correct));
                }
                else
                {
                    responseType = ResponseType.Failure;
                    return BadRequest("Incorrect username or password.");
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }

        [HttpGet("GetUser/{id}")]
        public IActionResult GetById(int id)
        {
            ResponseType responseType = ResponseType.Success;

            try
            {
                UserModel data = _database.GetUserById(id);

                if (data.Id == 0)
                {
                    responseType = ResponseType.NotFound;
                }

                return Ok(ResponseHandler.GetAppResponse(responseType, data));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }

        [HttpPost("SaveUser")]
        public IActionResult Post([FromBody] UserModel userModel)
        {
            try
            {
                ResponseType responseType = ResponseType.Success;

                _database.SaveUser(userModel);

                return Ok(ResponseHandler.GetAppResponse(responseType, userModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }

        [HttpPost("UpdateUser")]
        public IActionResult Put([FromBody] UserModel userModel)
        {
            try
            {
                ResponseType responseType = ResponseType.Success;

                _database.SaveUser(userModel);

                return Ok(ResponseHandler.GetAppResponse(responseType, userModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }

        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                ResponseType responseType = ResponseType.Success;

                _database.DeleteUser(id);

                return Ok(ResponseHandler.GetAppResponse(responseType, "Deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }
    }
}
