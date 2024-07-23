using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using NArchitecture.Core.Localization.Abstraction;
using System.Collections.Immutable;

namespace NArchitecture.Core.Localization.WebApi;

public class LocalizationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    public async Task Invoke(HttpContext context, ILocalizationService localizationService)
    {
        IList<StringWithQualityHeaderValue> acceptLanguages = context.Request.GetTypedHeaders().AcceptLanguage;
        if (acceptLanguages.Count > 0)
            localizationService.AcceptLocales = acceptLanguages
                .OrderByDescending(x => x.Quality ?? 1)
                .Select(x => x.Value.ToString())
                .ToImmutableArray();

        await _next(context);
    }
}
