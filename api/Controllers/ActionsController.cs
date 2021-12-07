using Mint.API.DTOs;
using Mint.API.Services;
using Microsoft.AspNetCore.Mvc;
using Action = Mint.API.Models.Action;

namespace Mint.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActionsController : ControllerBase
{

    [HttpGet]
    public List<Action> GetAllActions()
    {
        List<Action> vActions = new List<Action>();

        try
        {
            vActions = ActionService.GetAll();
            Response.StatusCode = 200;
        }
        catch
        {
            Response.StatusCode = 500;
        }

        return vActions;
    }

    [HttpPost]
    public Action? InsertAction(ActionDTO pAction)
    {
        Action? vAction = null;

        try
        {
            vAction = ActionService.Insert(
                pAction.Title,
                pAction.Link,
                out bool vResult
            );
            Response.StatusCode = (vAction == null || vResult == false) ? 500 : 200;
        }
        catch
        {
            Response.StatusCode = 500;
        }

        return vAction;
    }

}
