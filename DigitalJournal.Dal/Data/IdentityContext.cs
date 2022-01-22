// dotnet ef --startup-project ../DigitalJournal/ migrations add init --context IdentityContext

namespace DigitalJournal.Dal.Data;

public class IdentityContext : IdentityDbContext<User, Role, string>
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    { }
}

