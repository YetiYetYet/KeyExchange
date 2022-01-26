using System.Net;
using API.Exception;

namespace DN.WebApi.Application.Common.Exceptions;

public class NothingToUpdateException : CustomExeption.CustomException
{
    public NothingToUpdateException()
    : base("There are no new changes to update for this Entity.", null, HttpStatusCode.NotAcceptable)
    {
    }
}