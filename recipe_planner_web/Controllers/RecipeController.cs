using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using recipe_planner_web.Models;


namespace recipe_planner_web.Controllers
{
    public class RecipeController : Controller
    {
        static Recipe newRecipe = new Recipe();
        static List<Ingredient> ingredientsToAdd = new List<Ingredient>();
        static List<Recipe> RecipesList = new List<Recipe>();
        static List<Recipe> RecipesShoppingList = new List<Recipe>();
        static int index = 0;
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
            if (!String.IsNullOrEmpty(newRecipe.Name))
                ViewData["RecipeName"] = newRecipe.Name;
            if (!String.IsNullOrEmpty(newRecipe.Description))
                ViewData["RecipeDescription"] = newRecipe.Description;
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
            newRecipe.Name = Request.Form["Name"].ToString();
            newRecipe.Description = Request.Form["Description"].ToString();
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
            RecipesShoppingList.Clear();
            newRecipe.Name = null;
            newRecipe.Description = null;
            index = 0;
            return View("Views/Recipe/Index.cshtml");
        }

        public IActionResult Summary()
        {
            ViewData["Ingredients"] = ingredientsToAdd;
            if (!String.IsNullOrEmpty(newRecipe.Name))
                ViewData["RecipeName"] = newRecipe.Name;
            if (!String.IsNullOrEmpty(newRecipe.Description))
                ViewData["RecipeDescription"] = newRecipe.Description;

            return View();
        }

        public IActionResult AddRecipeToList()
        {
            List<Ingredient> newIngredientsListToAdd = new List<Ingredient>();
            newIngredientsListToAdd.AddRange(ingredientsToAdd);
            Recipe newRecipeToAdd = new Recipe(newRecipe.Name, newRecipe.Description, newIngredientsListToAdd);
            if (!RecipesList.Contains(newRecipeToAdd))
                RecipesList.Add(newRecipeToAdd);
            return RedirectToAction("BackToList", "Recipe");
        }
        [HttpPost]
        public IActionResult ActionOnRecipe(int id, string action)
        {
            var valueToView = Request.Form["valueToView"];
            var valueToEdit = Request.Form["valueToEdit"];
            var valueToDelete = Request.Form["valueToBeDeleted"];

            if (valueToView.Count != 0)
            {
                ViewData["RecipeToView"] = id;//Convert.ToInt32(valueToView);
                ViewData["RecipesList"] = RecipesList;

                return View("Views/Recipe/ViewRecipe.cshtml");
            }
            else if (valueToDelete.Count != 0)
            {
                RecipesList.RemoveAt(id);//Convert.ToInt32(valueToDelete));
                return RedirectToAction("Index", "Recipe");
            }
            else if (valueToEdit.Count != 0)
            {
                ViewData["RecipeToEdit"] = id;//Convert.ToInt32(valueToEdit);
                ViewData["RecipesList"] = RecipesList;
                return View("Views/Recipe/Edit.cshtml");
            }
            return RedirectToAction("Index", "Recipe");
        }
        public IActionResult Edit(int id) 
        {
            ViewData["RecipeToEdit"] = id;
            ViewData["RecipesList"] = RecipesList;
            return View();
        }
        public IActionResult EditNameAndDescription(int id)
        {
            RecipesList[id].Name = Request.Form["Name"].ToString();
            RecipesList[id].Description = Request.Form["Description"].ToString();
            return RedirectToAction("Edit", "Recipe", new { Id = id });
        }
        public IActionResult EditIngredients(int id) 
        {
            ViewData["IngredientsToEdit"] = RecipesList[id].Ingredients;
            ViewData["ID"] = id;
            return View();
        }
        public IActionResult EditDeleteIngredientFromList(int id)
        {
            RecipesList[id].Ingredients.RemoveAt(Convert.ToInt32(Request.Form["valueToDelete"])); 
            return RedirectToAction("EditIngredients", "Recipe", new { Id = id });
        }
        public IActionResult EditIngredient(int id)
        {
            Ingredient newIngredient = new Ingredient(Request.Form["ingredientName"].ToString(), Convert.ToSingle(Request.Form["ingredientQuantity"]), Request.Form["ingredientUnit"].ToString());
            if (!RecipesList[id].Ingredients.Any())
                RecipesList[id].Ingredients.Add(newIngredient);
            else
            {
                bool sameItem = false;
                foreach (var item in RecipesList[id].Ingredients.ToList())
                    if (item.Name == newIngredient.Name && item.Unit == newIngredient.Unit)
                        sameItem = true;

                if (!sameItem)
                    RecipesList[id].Ingredients.Add(newIngredient);
            }
            return RedirectToAction("EditIngredients", "Recipe", new { Id = id });

        }
        public IActionResult ShoppingList()
        {
            ViewData["RecipesList"] = RecipesList;
            ViewData["AddedRecipes"] = RecipesShoppingList;
            ViewData["ingredientsToAdd"] = ingredientsToAdd;
            return View();
        }

        public IActionResult AddRecipeToShoppingList(int id)
        {
            RecipesShoppingList.Add(new Recipe(RecipesList[id].Name,
                               RecipesList[id].Description,
                               RecipesList[id].Ingredients));
            if (ingredientsToAdd.Count == 0)
            {
                for (int j = 0; j < RecipesShoppingList[index].Ingredients.Count; j++)
                {
                    ingredientsToAdd.Add(new Ingredient(RecipesShoppingList[index].Ingredients[j].Name,
                                                        RecipesShoppingList[index].Ingredients[j].Quantity,
                                                        RecipesShoppingList[index].Ingredients[j].Unit));
                }
            }
            else
            {
                    for (int j = 0; j < RecipesShoppingList[index].Ingredients.Count; j++)
                    {
                        if(ingredientsToAdd.Any(x => x.Name == RecipesShoppingList[index].Ingredients[j].Name && x.Unit == RecipesShoppingList[index].Ingredients[j].Unit))
                        {
                            int indexValue = ingredientsToAdd.FindIndex(x => x.Name == RecipesShoppingList[index].Ingredients[j].Name && x.Unit == RecipesShoppingList[index].Ingredients[j].Unit);
                            if(indexValue != (-1))
                                ingredientsToAdd[indexValue].Quantity += RecipesShoppingList[index].Ingredients[j].Quantity;
                        }
                        else
                        {
                            ingredientsToAdd.Add(new Ingredient(RecipesShoppingList[index].Ingredients[j].Name,
                                RecipesShoppingList[index].Ingredients[j].Quantity,
                                RecipesShoppingList[index].Ingredients[j].Unit));
                        }
                    }
            }
            index++;
            ingredientsToAdd.Sort((x,y) => string.Compare(x.Name,y.Name));
            return RedirectToAction("ShoppingList", "Recipe");
        }
        public IActionResult RemoveRecipeToShoppingList(int id)
        {
            if(ingredientsToAdd.Count != 0)
            {
                for (int j = 0; j < RecipesShoppingList[id].Ingredients.Count; j++)
                {
                    if (ingredientsToAdd.Any(x => x.Name == RecipesShoppingList[id].Ingredients[j].Name && x.Unit == RecipesShoppingList[id].Ingredients[j].Unit))
                    {
                        int indexValue = ingredientsToAdd.FindIndex(x => x.Name == RecipesShoppingList[id].Ingredients[j].Name && x.Unit == RecipesShoppingList[id].Ingredients[j].Unit);
                        if (indexValue != (-1))
                        {
                            ingredientsToAdd[indexValue].Quantity -= RecipesShoppingList[id].Ingredients[j].Quantity;
                            if (ingredientsToAdd[indexValue].Quantity <= 0)
                                ingredientsToAdd.RemoveAt(indexValue);
                        }
                    }
                }
            }
            if(RecipesShoppingList.Count != 0)
                RecipesShoppingList.RemoveAt(id);
            index--;
            return RedirectToAction("ShoppingList", "Recipe");
        }
        public IActionResult Save(int id)
        {
            if (id == 0)
            {
                var json = JsonConvert.SerializeObject(RecipesList, Formatting.Indented);
                System.IO.File.WriteAllText("Recipes.json", json);
            }
            return RedirectToAction("Index", "Recipe");
        }

        public IActionResult Load(int id)
        {
            if (id == 0)
            {
                if(System.IO.File.Exists("Recipes.json"))
                    RecipesList = JsonConvert.DeserializeObject<List<Recipe>>(System.IO.File.ReadAllText("Recipes.json"));
            }
            return RedirectToAction("Index", "Recipe");
        }
    }
}
