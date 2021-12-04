using Mint.API.DTOs;
using Mint.API.Models;
using Mint.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mint.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{

    [HttpGet]
    public List<Message> GetAllCurrentMessages()
    {
        List<Message> vCurrentMessages = new List<Message>();

        try
        {
            vCurrentMessages = MessageService.GetAllCurrent();
            Response.StatusCode = 200;
        }
        catch
        {
            Response.StatusCode = 500;
        }

        return vCurrentMessages;
    }

    [HttpPost]
    public List<Message> GetAllMessages(MessageFiltersDTO pMessageFilters)
    {
        List<Message> vMessages = new List<Message>();

        try
        {
            vMessages = MessageService.GetAll(
                pMessageFilters.Categories ?? new List<int>(),
                pMessageFilters.IsCurrent,
                pMessageFilters.IsDraft
            );
            Response.StatusCode = 200;
        }
        catch
        {
            Response.StatusCode = 500;
        }

        return vMessages;
    }

    [HttpPut]
    public Message? UpdateOrInsertMessage(MessageDTO pMessage)
    {
        Message? vMessage = null;

        try
        {
            vMessage = MessageService.UpdateOrInsert(
                pMessage.Id,
                pMessage.Title,
                pMessage.IsDraft,
                pMessage.Header,
                pMessage.Body,
                pMessage.Background,
                pMessage.StartDate,
                pMessage.EndDate,
                pMessage.Icon,
                pMessage.Action,
                pMessage.Categories,
                out bool vResult
            );
            Response.StatusCode = (vMessage == null || vResult == false) ? 500 : 200;
        }
        catch
        {
            Response.StatusCode = 500;
        }

        return vMessage;
    }

    [HttpGet("{pMessageId}")]
    public Message? GetMessageById(int pMessageId)
    {
        Message? vMessage = null;

        try
        {
            vMessage = MessageService.GetById(pMessageId);
            Response.StatusCode = (vMessage == null) ? 500 : 200;
        }
        catch
        {
            Response.StatusCode = 500;
        }

        return vMessage;
    }

}
