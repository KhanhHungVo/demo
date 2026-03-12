using Microsoft.AspNetCore.Http;
using Todo.Application.Common.Interfaces;

namespace Todo.Infrastructure.Services;

public class ClientInfoService(IHttpContextAccessor accessor) : IClientInfoService
{
    public string? RemoteIpAddress =>
        accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
}
