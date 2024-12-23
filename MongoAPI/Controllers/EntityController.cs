using Microsoft.AspNetCore.Mvc;
using MongoAPI.Domain;
using MongoAPI.Dto;
using MongoAPI.Services;

namespace MongoAPI.Controllers
{
    [ApiController]
    [Route("entities")]
    public class EntityController(IEntityService entityService): ControllerBase
    {
        [HttpGet("{key}")]
        public async Task<IActionResult> GetId(string key)
        {
            try
            {
                var id = await entityService.GetEntityIdByKeyAsync(key);

                return id == null
                    ? NotFound() 
                    : Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntityAndReturnId(Entity entity)
        {
            try
            {
                var id = await entityService.CreateOrReturnEntityIdAsync(entity);

                return Ok(id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEntityByKey(DeleteEntityDto deleteEntityDto)
        {
            try
            {
                var deletedEntityId = await entityService.DeleteEntityByKeyAsync(deleteEntityDto.Key);

                return deletedEntityId == null
                    ? NotFound()
                    : Ok(deletedEntityId);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
