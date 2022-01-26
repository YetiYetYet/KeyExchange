using System.Net;
using API.Exception;

namespace DN.WebApi.Application.Common.Exceptions;

public class EntityCannotBeDeleted : CustomExeption.CustomException
{
    public EntityCannotBeDeleted(string message)
    : base(message, null, HttpStatusCode.Conflict)
    {
    }
}