using System.Reflection;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace Helpers.WebApi.Services;

/// <summary>
///     The customized Sieve processor used for filtering.
/// </summary>
public class ApplicationSieveProcessor : SieveProcessor
{
    public ApplicationSieveProcessor(IOptions<SieveOptions> options) : base(options)
    {
    }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        return mapper.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}