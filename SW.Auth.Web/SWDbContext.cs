using Microsoft.EntityFrameworkCore;
using SW.EfCoreExtensions;
using SW.PrimitiveTypes;

namespace SW.Auth.Web;

public class SwDbContext : DbContext
{
    public const string ConnectionString = "PasswordLessDb";
    public const string Schema = "main";
    private readonly RequestContext _requestContext;

    public SwDbContext(DbContextOptions<SwDbContext> options, RequestContext requestContext) :
        base(options)
    {
        _requestContext = requestContext;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SwDbContext).Assembly);
        modelBuilder.CommonProperties(b => { b.HasSoftDeletionQueryFilter(); });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.ApplyAuditValues(_requestContext.GetNameIdentifier());
        ChangeTracker.ApplySoftDeletion(_requestContext.GetNameIdentifier());

        var affectedRecords = await base.SaveChangesAsync(cancellationToken);
        return affectedRecords;
    }
}