using Mint.API.DTOs;
using Mint.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mint.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{

    [HttpGet]
    public List<Category> GetAllCategories()
    {
        List<Category> vCategories = new List<Category>();

        try
        {
            vCategories = Category.GetAll();
            Response.StatusCode = 200;
        }
        catch
        {
            Response.StatusCode = 500;
        }

        return vCategories;
    }

    [HttpPost]
    public Category? InsertCategory(CategoryDTO pCategory)
    {
        Category? vCategory = null;

        try
        {
            vCategory = Category.Insert(
                pCategory.Title,
                out bool vResult
            );
            Response.StatusCode = (vCategory == null || vResult == false) ? 500 : 200;
        }
        catch
        {
            Response.StatusCode = 500;
        }

        return vCategory;
    }

}
