using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using recipe_planner_web.Models;

namespace recipe_planner_web.Controllers
{
    public class RecipeController : Controller
    {

        static Recipe newRecipe = new Recipe();
        static List<Ingredient> ingredientsToAdd = new List<Ingredient>();
        static List<Recipe> RecipesList = new List<Recipe>();

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Ingredients"] = ingredientsToAdd;
            if (!String.IsNullOrEmpty(newRecipe.recipe_name))
                ViewData["RecipeName"] = newRecipe.recipe_name;
            if (!String.IsNullOrEmpty(newRecipe.recipe_description))
                ViewData["RecipeDescription"] = newRecipe.recipe_description;
            return View();
        }
        [HttpGet]
        public IActionResult AddIngredient()
        {
            ViewData["Ingredients"] = ingredientsToAdd;
            return View();
        }
        [HttpPost]
        public IActionResult CreateRecipe()
        {
            newRecipe.recipe_name = Request.Form["Name"].ToString();
            newRecipe.recipe_description = Request.Form["Description"].ToString();
            return RedirectToAction("Create", "Recipe");
        }
        
        public IActionResult CreateIngredient()
        {
            Ingredient newIngredient = new Ingredient(Request.Form["ingredientName"].ToString(), Convert.ToSingle(Request.Form["ingredientQuantity"]), Request.Form["ingredientUnit"].ToString());
            if (!ingredientsToAdd.Any())
                ingredientsToAdd.Add(newIngredient);
            else
            {
                bool sameItem = false;
                foreach (var item in ingredientsToAdd.ToList())
                    if (item.Name == newIngredient.Name && item.Unit == newIngredient.Unit)
                        sameItem = true;

                if (!sameItem)
                    ingredientsToAdd.Add(newIngredient);
            }
           return RedirectToAction("AddIngredient", "Recipe");

        }

        public IActionResult DeleteIngredientFromList()
        {
            ingredientsToAdd.RemoveAt(Convert.ToInt32(Request.Form["valueToDelete"]));
            return RedirectToAction("AddIngredient", "Recipe");
        }

        public IActionResult BackToList()
        {
            ingredientsToAdd.Clear();
            newRecipe.recipe_name = null;
            newRecipe.recipe_description = null;
            return View("Views/Recipe/Index.cshtml");
        }

        public IActionResult Summary()
        {
            ViewData["Ingredients"] = ingredientsToAdd;
            if (!String.IsNullOrEmpty(newRecipe.recipe_name))
                ViewData["RecipeName"] = newRecipe.recipe_name;
            if (!String.IsNullOrEmpty(newRecipe.recipe_description))
                ViewData["RecipeDescription"] = newRecipe.recipe_description;

            return View();
        }

        public IActionResult AddRecipeToList()
        {
            RecipesList.Add(newRecipe);
            return RedirectToAction("BackToList", "Recipe");
        }
    }
}
