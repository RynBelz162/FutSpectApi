namespace FutSpect.Scraper.Services.Scraping;

public class PlayerService : IPlayerService
{
    public (string? FirstName, string LastName) GetName(string value)
    {
        string? firstName = null;
        string lastName;

        var names = value.Split(" ");

        if (names.Length > 1)
        {
            firstName = names[0];
            lastName = names[1];
        }
        else
        {
            lastName = names[1];
        }

        return (firstName, lastName);
    }

    public int GetPositionId(string value)
    {
        throw new NotImplementedException();
    }
}