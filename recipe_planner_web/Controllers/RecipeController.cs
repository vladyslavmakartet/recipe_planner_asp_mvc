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
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["RecipesList"] = RecipesList;
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
        [HttpPost]
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
        [HttpPost]
        public IActionResult DeleteIngredientFromList()
        {
            ingredientsToAdd.RemoveAt(Convert.ToInt32(Request.Form["valueToDelete"]));
            return RedirectToAction("AddIngredient", "Recipe");
        }

        public IActionResult BackToList()
        {
            ViewData["RecipesList"] = RecipesList;
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
            List<Ingredient> newIngredientsListToAdd = new List<Ingredient>();
            newIngredientsListToAdd.AddRange(ingredientsToAdd);
            Recipe newRecipeToAdd = new Recipe(newRecipe.recipe_name, newRecipe.recipe_description, newIngredientsListToAdd);
            if (!RecipesList.Contains(newRecipeToAdd))
                RecipesList.Add(newRecipeToAdd);
            return RedirectToAction("BackToList", "Recipe");
        }
        [HttpPost]
        public IActionResult ActionOnRecipe()
        {
            var valueToView = Request.Form["valueToView"];
            var valueToEdit = Request.Form["valueToEdit"];
            var valueToDelete = Request.Form["valueToBeDeleted"];
            
            if (valueToView.Count != 0)
            {
                ViewData["RecipeToView"] = Convert.ToInt32(valueToView);
                ViewData["RecipesList"] = RecipesList;

                return View("Views/Recipe/ViewRecipe.cshtml");
            }
            else if (valueToDelete.Count != 0)
            {
                RecipesList.RemoveAt(Convert.ToInt32(valueToDelete));
                return RedirectToAction("Index", "Recipe");
            }
            else if (valueToEdit.Count != 0) 
            {
                ViewData["RecipeToEdit"] = Convert.ToInt32(valueToEdit);
                ViewData["RecipesList"] = RecipesList;
                return View("Views/Recipe/Edit.cshtml");
            }
            return RedirectToAction("Index", "Recipe");
        }
        public IActionResult Edit() 
        {
            ViewData["RecipesList"] = RecipesList;
            return View();
        }
        public IActionResult EditNameAndDescription()
        {
            RecipesList[Convert.ToInt32(ViewData["RecipeToEdit"])].recipe_name = Request.Form["Name"].ToString();
            RecipesList[Convert.ToInt32(ViewData["RecipeToEdit"])].recipe_description = Request.Form["Description"].ToString();
            return RedirectToAction("Edit", "Recipe");
        }
        public IActionResult EditIngredients() 
        {
            ViewData["IngredientsToEdit"] = RecipesList[Convert.ToInt32(ViewData["RecipeToEdit"])].ingredientsList;
            return View();
        }
        public IActionResult EditDeleteIngredientFromList()
        {
            RecipesList[Convert.ToInt32(ViewData["RecipeToEdit"])].ingredientsList.RemoveAt(Convert.ToInt32(Request.Form["valueToDelete"]));
            return RedirectToAction("EditIngredients", "Recipe");
        }
        public IActionResult EditIngredient()
        {
            Ingredient newIngredient = new Ingredient(Request.Form["ingredientName"].ToString(), Convert.ToSingle(Request.Form["ingredientQuantity"]), Request.Form["ingredientUnit"].ToString());
            if (!RecipesList[Convert.ToInt32(ViewData["RecipeToEdit"])].ingredientsList.Any())
                RecipesList[Convert.ToInt32(ViewData["RecipeToEdit"])].ingredientsList.Add(newIngredient);
            else
            {
                bool sameItem = false;
                foreach (var item in RecipesList[Convert.ToInt32(ViewData["RecipeToEdit"])].ingredientsList.ToList())
                    if (item.Name == newIngredient.Name && item.Unit == newIngredient.Unit)
                        sameItem = true;

                if (!sameItem)
                    RecipesList[Convert.ToInt32(ViewData["RecipeToEdit"])].ingredientsList.Add(newIngredient);
            }
            return RedirectToAction("EditIngredients", "Recipe");

        }
    }
}
