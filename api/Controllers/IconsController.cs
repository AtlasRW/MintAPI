using Mint.API.DTOs;
using Mint.API.Models;
using Mint.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mint.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IconsController : ControllerBase
{

    [HttpGet]
    public List<Icon> GetAllIcons()
    {
        List<Icon> vIcons = new List<Icon>();

        try
        {
            vIcons = IconService.GetAll();
            Response.StatusCode = 200;
        }
        catch
        {
            Response.StatusCode = 500;
        }

        return vIcons;
    }

    [HttpPost]
    public Icon? InsertIcon(IconDTO pIcon)
    {
        Icon? vIcon = null;

        try
        {
            vIcon = IconService.Insert(
                Convert.FromBase64String(pIcon.Data),
                out bool vResult
            );
            Response.StatusCode = (vIcon == null || vResult == false) ? 500 : 200;
        }
        catch
        {
            Response.StatusCode = 500;
        }

        return vIcon;
    }

}
