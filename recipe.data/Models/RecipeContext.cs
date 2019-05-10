using System;
using System.Collections.Generic;
using System.Text;
using recipe.data.Models;

namespace recipe.data.Models
{
    public class RecipeContext
    {
        public static List<Recipe> Recipes = new List<Recipe>();
        public RecipeContext()
        {
            Recipes.Add(new Recipe() { Name = "test1"});
            Recipes.Add(new Recipe() { Name = "test7"});
            Recipes.Add(new Recipe() { Name = "test5"});
            Recipes.Add(new Recipe() { Name = "test4"});
            Recipes.Add(new Recipe() { Name = "test2"});
            Recipes.Add(new Recipe() { Name = "test3"});
        }

        public List<Recipe> GetRecipes { get => Recipes; set => Recipes = value; }
    }
}
