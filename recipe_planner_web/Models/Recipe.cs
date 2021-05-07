using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace recipe_planner_web.Models
{
    public class Recipe
    {
        public string recipe_name { get; set; }
        public string recipe_description { get; set; }

        public List<Ingredient> ingredientsList = new List<Ingredient>();

        public List<Ingredient> GetIngredientList()
        {
            return ingredientsList;
        }

        public Recipe() { }
        public Recipe(string name, string description, List<Ingredient> newIngredients)
        {
            recipe_name = name;
            recipe_description = description;
            ingredientsList = newIngredients;
        }
        


    }
}
