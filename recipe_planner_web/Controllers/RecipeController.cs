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
            RecipesShoppingList.Clear();
            newRecipe.recipe_name = null;
            newRecipe.recipe_description = null;
            index = 0;
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
/*        public IActionResult ActionOnRecipe(int id, string action)
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
        }*/
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

            /*            RecipesList[Convert.ToInt32(ViewData["RecipeToEdit"])].recipe_name = Request.Form["Name"].ToString();/////////////////////*/
            RecipesList[id].recipe_name = Request.Form["Name"].ToString();/////////////////////
            /*            RecipesList[Convert.ToInt32(ViewData["RecipeToEdit"])].recipe_description = Request.Form["Description"].ToString();*/
            RecipesList[id].recipe_description = Request.Form["Description"].ToString();
            return RedirectToAction("Edit", "Recipe", new { Id = id });
        }
        public IActionResult EditIngredients(int id) 
        {
            ViewData["IngredientsToEdit"] = RecipesList[id].ingredientsList;
            ViewData["ID"] = id;
            return View();
        }
        public IActionResult EditDeleteIngredientFromList(int id)
        {
            RecipesList[id].ingredientsList.RemoveAt(Convert.ToInt32(Request.Form["valueToDelete"])); /////////////////////
            return RedirectToAction("EditIngredients", "Recipe", new { Id = id });
        }
 /*       public IActionResult EditIngredient(int id)
        {
            Ingredient newIngredient = new Ingredient(Request.Form["ingredientName"].ToString(), Convert.ToSingle(Request.Form["ingredientQuantity"]), Request.Form["ingredientUnit"].ToString());/////////////////////
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

        }*/
        public IActionResult EditIngredient(int id)
        {
            Ingredient newIngredient = new Ingredient(Request.Form["ingredientName"].ToString(), Convert.ToSingle(Request.Form["ingredientQuantity"]), Request.Form["ingredientUnit"].ToString());/////////////////////
            if (!RecipesList[id].ingredientsList.Any())
                RecipesList[id].ingredientsList.Add(newIngredient);
            else
            {
                bool sameItem = false;
                foreach (var item in RecipesList[id].ingredientsList.ToList())
                    if (item.Name == newIngredient.Name && item.Unit == newIngredient.Unit)
                        sameItem = true;

                if (!sameItem)
                    RecipesList[id].ingredientsList.Add(newIngredient);
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
        /*        public IActionResult AddRecipeToShoppingList()
                {

                    if (RecipesShoppingList.Count == 0)
                         RecipesShoppingList.Add(new Recipe(RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].recipe_name, 
                         RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].recipe_description, 
                         RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].ingredientsList));
                    else
                    {

                        for (int i = 0; i < RecipesShoppingList.Count; i++)
                        {
                            for (int j = 0; j < RecipesShoppingList[i].ingredientsList.Count; j++)
                            {
                                if (RecipesShoppingList[i].ingredientsList[j].Name == RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].ingredientsList[j].Name
                                && RecipesShoppingList[i].ingredientsList[j].Unit == RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].ingredientsList[j].Unit)
                                {
                                    //RecipesShoppingList[i].ingredientsList[j].Quantity = RecipesShoppingList[i].ingredientsList[j].Quantity + RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].ingredientsList[j].Quantity;
                                    RecipesShoppingList[i].ingredientsList[j].Quantity += RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].ingredientsList[j].Quantity;
                                }
                                else
                                {
                                    //RecipesShoppingList[i].ingredientsList.Add(RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].ingredientsList[j]);
                                    RecipesShoppingList[i].ingredientsList.Add(new Ingredient(RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].ingredientsList[j].Name,
                                        RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].ingredientsList[j].Quantity,
                                        RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].ingredientsList[j].Unit));
                                }
                            }
                        }
                    }

                    return RedirectToAction("ShoppingList", "Recipe");
                }*/

        /*        public IActionResult AddRecipeToShoppingList(int id)
                {

                    if (RecipesShoppingList.Count == 0)
                    {
                        RecipesShoppingList.Add(new Recipe(RecipesList[id].recipe_name, RecipesList[id].recipe_description, RecipesList[id].ingredientsList));
                        for (int j = 0; j < RecipesShoppingList[0].ingredientsList.Count; j++)
                        {
                            if (ingredientsToAdd.Count == 0)
                            {
                                ingredientsToAdd.Add(new Ingredient(RecipesShoppingList[0].ingredientsList[j].Name,
                                                                    RecipesShoppingList[0].ingredientsList[j].Quantity,
                                                                    RecipesShoppingList[0].ingredientsList[j].Unit));
                            }
                        }
                    }
                    *//*                RecipesShoppingList.Add(new Recipe(RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].recipe_name,
                                        RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].recipe_description,
                                        RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].ingredientsList));*//*
                    else
                    {
                        RecipesShoppingList.Add(new Recipe(RecipesList[id].recipe_name, RecipesList[id].recipe_description, RecipesList[id].ingredientsList));
                        for (int i = 0; i < RecipesShoppingList.Count; i++)
                        {
                            for (int j = 0; j < RecipesShoppingList[i].ingredientsList.Count; j++)
                            {
        *//*                        if (ingredientsToAdd.Count == 0)
                                {
                                    ingredientsToAdd.Add(new Ingredient(RecipesShoppingList[i].ingredientsList[j].Name,
                                                                        RecipesShoppingList[i].ingredientsList[j].Quantity,
                                                                        RecipesShoppingList[i].ingredientsList[j].Unit));
                                }
                                else
                                {*//*
                                    if (ingredientsToAdd[i].Name == RecipesShoppingList[i].ingredientsList[j].Name
                                        && ingredientsToAdd[i].Unit == RecipesShoppingList[i].ingredientsList[j].Unit)
                                    {
                                        ingredientsToAdd[i].Quantity += RecipesShoppingList[i].ingredientsList[j].Quantity;
                                    }
                                    else
                                    {
                                        ingredientsToAdd.Add(new Ingredient(RecipesShoppingList[i].ingredientsList[j].Name,
                                            RecipesShoppingList[i].ingredientsList[j].Quantity,
                                            RecipesShoppingList[i].ingredientsList[j].Unit));
                                    }
                               // }
                            }
                        }
                    }


                    return RedirectToAction("ShoppingList", "Recipe");
                }*/


        public IActionResult AddRecipeToShoppingList(int id)
        {


            //RecipesShoppingList.Add(RecipesList[id]);
            RecipesShoppingList.Add(new Recipe(RecipesList[id].recipe_name,
                               RecipesList[id].recipe_description,
                               RecipesList[id].ingredientsList));







            if (ingredientsToAdd.Count == 0)
            {
                for (int j = 0; j < RecipesShoppingList[index].ingredientsList.Count; j++)
                {
                    ingredientsToAdd.Add(new Ingredient(RecipesShoppingList[index].ingredientsList[j].Name,
                                                        RecipesShoppingList[index].ingredientsList[j].Quantity,
                                                        RecipesShoppingList[index].ingredientsList[j].Unit));
                }
            }
            else
            {
/*                for (int i = 0; i < RecipesShoppingList.Count; i++)
                {*/
                    for (int j = 0; j < RecipesShoppingList[index].ingredientsList.Count; j++)
                    {
                        if(ingredientsToAdd.Any(x => x.Name == RecipesShoppingList[index].ingredientsList[j].Name && x.Unit == RecipesShoppingList[index].ingredientsList[j].Unit))
/*                        if (ingredientsToAdd[index].Name == RecipesShoppingList[index].ingredientsList[j].Name
                            && ingredientsToAdd[index].Unit == RecipesShoppingList[index].ingredientsList[j].Unit)
                        {*/
                        {
                        //int indexValue = ingredientsToAdd.FindIndex(RecipesShoppingList[index].ingredientsList[j]);
                            int indexValue = ingredientsToAdd.FindIndex(x => x.Name == RecipesShoppingList[index].ingredientsList[j].Name && x.Unit == RecipesShoppingList[index].ingredientsList[j].Unit);
                            if(indexValue != (-1))
                                ingredientsToAdd[indexValue].Quantity += RecipesShoppingList[index].ingredientsList[j].Quantity;
                        }
                        else
                        {
                            ingredientsToAdd.Add(new Ingredient(RecipesShoppingList[index].ingredientsList[j].Name,
                                RecipesShoppingList[index].ingredientsList[j].Quantity,
                                RecipesShoppingList[index].ingredientsList[j].Unit));
                        }
                   // }
                    }
            }
            index++;
            /*                RecipesShoppingList.Add(new Recipe(RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].recipe_name,
                                RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].recipe_description,
                                RecipesList[Convert.ToInt32(Request.Form["valueToAddRecipe"])].ingredientsList));*//*

                RecipesShoppingList.Add(new Recipe(RecipesList[id].recipe_name, RecipesList[id].recipe_description, RecipesList[id].ingredientsList));
                for (int i = 0; i < RecipesShoppingList.Count; i++)
                {
                    for (int j = 0; j < RecipesShoppingList[i].ingredientsList.Count; j++)
                    {
                        *//*                        if (ingredientsToAdd.Count == 0)
                                                {
                                                    ingredientsToAdd.Add(new Ingredient(RecipesShoppingList[i].ingredientsList[j].Name,
                                                                                        RecipesShoppingList[i].ingredientsList[j].Quantity,
                                                                                        RecipesShoppingList[i].ingredientsList[j].Unit));
                                                }
                                                else
                                                {*//*
                        if (ingredientsToAdd[i].Name == RecipesShoppingList[i].ingredientsList[j].Name
                            && ingredientsToAdd[i].Unit == RecipesShoppingList[i].ingredientsList[j].Unit)
                        {
                            ingredientsToAdd[i].Quantity += RecipesShoppingList[i].ingredientsList[j].Quantity;
                        }
                        else
                        {
                            ingredientsToAdd.Add(new Ingredient(RecipesShoppingList[i].ingredientsList[j].Name,
                                RecipesShoppingList[i].ingredientsList[j].Quantity,
                                RecipesShoppingList[i].ingredientsList[j].Unit));
                        }
                        // }
                    }
                
            }*/


            return RedirectToAction("ShoppingList", "Recipe");
        }





        public IActionResult RemoveRecipeToShoppingList(int id)
        {
            if(ingredientsToAdd.Count != 0)
            {
                for (int j = 0; j < RecipesShoppingList[id].ingredientsList.Count; j++)
                {
                    if (ingredientsToAdd.Any(x => x.Name == RecipesShoppingList[id].ingredientsList[j].Name && x.Unit == RecipesShoppingList[id].ingredientsList[j].Unit))
                    {
                        int indexValue = ingredientsToAdd.FindIndex(x => x.Name == RecipesShoppingList[id].ingredientsList[j].Name && x.Unit == RecipesShoppingList[id].ingredientsList[j].Unit);
                        if (indexValue != (-1))
                        {
                            ingredientsToAdd[indexValue].Quantity -= RecipesShoppingList[id].ingredientsList[j].Quantity;
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


    }
}
