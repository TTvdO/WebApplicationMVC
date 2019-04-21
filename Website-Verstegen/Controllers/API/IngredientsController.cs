using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Website_Verstegen.Models.web;

namespace Website_Verstegen.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public IngredientsController(DatabaseContext context)
        {
            _context = context;
        }

        // POST: api/Ingredients
        [HttpPost]
        public async Task<IActionResult> PostIngredient([FromBody] Ingredient ingredient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngredient", new { id = ingredient.Id }, ingredient);
        }

        // DELETE: api/Ingredients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isIngredientInUse = _context.RecipeIngredients.Where(ri => ri.RecipeId == id).Any();

            if (isIngredientInUse)
            {
                //Don't delete ingredient if its in use
                return BadRequest(new BadRequestMessenger { error = "inUse", message="The ingredient is already in use in recipes and can therefore not be removed" });
            }

            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return Ok(ingredient);
        }
    }
}