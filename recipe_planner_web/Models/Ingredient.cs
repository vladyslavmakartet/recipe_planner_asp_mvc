using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace recipe_planner_web.Models
{
    public class Ingredient
    {
        public string Name { get; set; }
        public float Quantity { get; set; }
        public string Unit { get; set; }
        public Ingredient() { }
        public Ingredient(string newName, float newQuantity, string newUnit)
        {
            Name = newName;
            Quantity = newQuantity;
            Unit = newUnit;
        }

    }
}
