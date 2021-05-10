using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace recipe_planner_web.Models
{
    public class Recipe
    {

        public string Name { get; set; }
        public string Description { get; set; }

        public List<Ingredient> Ingredients = new List<Ingredient>();

        public List<Ingredient> GetIngredientList()
        {
            return Ingredients;
        }

        public Recipe() { }
        public Recipe(string name, string description, List<Ingredient> newIngredients)
        {
            Name = name;
            Description = description;

            for (int i = 0; i < newIngredients.Count; i++)
            {
                Ingredients.Add(new Ingredient(newIngredients[i].Name, newIngredients[i].Quantity, newIngredients[i].Unit));
            }
        }
        


    }
}
