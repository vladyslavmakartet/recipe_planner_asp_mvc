using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace recipe_planner_web.Models
{
    public static class RecipeList
    {
        public static List<Recipe> RecipesList = new List<Recipe>();
        public static List<Recipe> GetRecipeList()
        {
            return RecipesList;
        }
    }
}
