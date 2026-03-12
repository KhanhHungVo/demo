namespace Todo.Application.Common.Interfaces;

public interface IClientInfoService
{
    string? RemoteIpAddress { get; }
}
