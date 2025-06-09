using ElasticSearch.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.Controllers
{
    [ApiController]
    [Route("api/text")]
    public class TextController(ElasticService elastic): ControllerBase
    {
        [HttpGet("{textId}")]
        public async Task<IActionResult> GetWordsByTextId(string textId)
        {
            try
            {
                var words = await elastic.GetWordsByTextIdAsync(textId);
                return Ok(words);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddText([FromBody] string text)
        {
            try
            {
                return Ok($"TextId: {await elastic.AddTextAsync(text)}");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("${textId}/word")]
        public async Task<IActionResult> AddWord(string textId, [FromBody] AddWordRequest request)
        {
            try
            {
                var result = await elastic.AddWordToTextAsync(textId, request.Value, request.Position);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string pattern)
        {
            try
            {
                var result = await elastic.SearchByWordOrSyllableAsync(pattern);
                return Ok(result.Select(r => new
                {
                    r.Match,
                    r.Word
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{textId}/word)")]
        public async Task<IActionResult> UpdateWord(string textId, [FromBody] UpdateWordRequest request)
        {
            try
            {
                await elastic.UpdateWordInTextAsync(textId, request.Position, request.NewValue);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{textId}/word)")]
        public async Task<IActionResult> DeleteWord(string textId, [FromBody] int position)
        {
            try
            {
                await elastic.DeleteWordFromTextAsync(textId, position);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
