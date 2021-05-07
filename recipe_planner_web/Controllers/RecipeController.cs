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
        Recipe newRecipe = new Recipe();
        static List<Ingredient> ingredientsToAdd = new List<Ingredient>();

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddIngredient()
        {
            ViewData["Ingredients"] = ingredientsToAdd;
            return View();
        }
        [HttpPost]
        public Recipe CreateRecipe()
        {

            Recipe newRecipe = new Recipe();
            newRecipe.recipe_name = Request.Form["Name"].ToString();
            newRecipe.recipe_description = Request.Form["Description"].ToString();
            //newRecipe.ingredients = new List<Ingredient>();
            Ingredient newIngredient = TempData["IngredientToAdd"] as Ingredient;
            newRecipe.ingredientsList.Add(newIngredient);


            return newRecipe;
        }
        
        public IActionResult CreateIngredient()
        {
            Ingredient newIngredient = new Ingredient(Request.Form["ingredientName"].ToString(), Convert.ToSingle(Request.Form["ingredientQuantity"]), Request.Form["ingredientUnit"].ToString());
            //TempData["IngredientToAdd"] = newIngredient;
            if (!ingredientsToAdd.Any())
            {
                ingredientsToAdd.Add(newIngredient);
            }
            else
            {
                bool sameItem = false;
                foreach (var item in ingredientsToAdd.ToList())
                {
                    if (item.Name == newIngredient.Name && item.Unit == newIngredient.Unit)
                    {
                        sameItem = true;
                    }
                }
                if (!sameItem)
                {
                    ingredientsToAdd.Add(newIngredient);
                }
            }



            /*            if (!ingredientsToAdd.Contains(newIngredient))
                        {
                            ingredientsToAdd.Add(newIngredient);
                        }*/
            //return View("Views/Recipe/AddIngredient.cshtml");//RedirectToAction("AddIngredientToList", "Recipe");
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
            return View("Views/Recipe/Index.cshtml");
        }


    }
}
