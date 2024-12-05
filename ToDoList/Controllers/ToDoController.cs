using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Database;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly DbHelper _database;

        public ToDoController(Context database)
        {
            _database = new DbHelper(database);        
        }

        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            ResponseType responseType = ResponseType.Success;

            try
            {
                IEnumerable<ToDoModel> data = _database.GetToDos();

                if (!data.Any())
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

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            ResponseType responseType = ResponseType.Success;

            try
            {
                ToDoModel data = _database.GetToDoById(id);

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

        [HttpGet("GetByCategory")]
        public IActionResult GetByCategory([FromBody] TODoFilterModel filter)
        {
            ResponseType responseType = ResponseType.Success;

            try
            {
                ToDoModel data = _database.GetToDoByCategory(filter.UserId, filter.Category);

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

        [HttpPost("SaveToDo")]
        public IActionResult Post([FromBody] ToDoModel toDoModel)
        {
            try
            {
                ResponseType responseType = ResponseType.Success;

                _database.SaveToDo(toDoModel);

                return Ok(ResponseHandler.GetAppResponse(responseType, toDoModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }

        [HttpPost("UpdateToDo")]
        public IActionResult Put([FromBody] ToDoModel toDoModel)
        {
            try
            {
                ResponseType responseType = ResponseType.Success;

                _database.SaveToDo(toDoModel);

                return Ok(ResponseHandler.GetAppResponse(responseType, toDoModel));
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

                _database.DeleteToDo(id);

                return Ok(ResponseHandler.GetAppResponse(responseType, "Deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }
    }
}