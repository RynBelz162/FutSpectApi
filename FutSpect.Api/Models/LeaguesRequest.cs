using FutSpect.DAL.Interfaces;

namespace FutSpect.Api.Models;

public class LeaguesRequest : IPageable
{
    ///<inheritdoc/>
    public int Page { get; set; } = 1;

    ///<inheritdoc/>
    public int PageSize { get; set; } = 10;
}