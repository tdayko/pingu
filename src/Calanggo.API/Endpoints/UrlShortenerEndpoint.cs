using Calanggo.Application.Common.Results;
using Calanggo.Application.Interfaces;
using Calanggo.Application.UseCases.ShortenUrl;
using Calanggo.Domain.Entities;
using Microsoft.AspNetCore.Http.Extensions;

namespace Calanggo.API.Endpoints;

public static class UrlShortenerEndpoint
{
    private const string _path = "/calango/api";
    public static IEndpointRouteBuilder AddUrlShortenerEndpoint(this IEndpointRouteBuilder endpoinBuilder)
    {
        RouteGroupBuilder urlShortenerEndpoint = endpoinBuilder.MapGroup(_path);

        // POST /url-shortener/shorten
        urlShortenerEndpoint.MapPost("/shorten", HandleShortenUrl)
            .WithDescription("The endpoint to shorten a URL")
            .Produces(StatusCodes.Status201Created);

        // GET /url-shortener/{shortCode}
        urlShortenerEndpoint.MapGet("/shorten/{shortCode}", HandleGetShortenedUrl)
            .WithDescription("The endpoint to get the original URL from a shortened URL");

        return urlShortenerEndpoint;
    }

    #region [Endpoints Handlers]

    // POST /url-shortener/shorten
    private static async Task<IResult> HandleShortenUrl(HttpContext context, IUrlShortenerService urlShortenerService, ShortenUrlRequest request)
    {
        Result<ShortenedUrl> result = await urlShortenerService.CreateShortenedUrl(request.OriginalUrl, request.ExpiresAt);
        if (result.IsSuccess == false)
        {
            return Results.Problem(
                title: "An error occurred while shortening the URL",
                statusCode: result.Error!.Code,
                detail: result.Error!.Message);
        }

        string shortenedUrl = context.Request.GetDisplayUrl().Replace(context.Request.Path, $"{_path}/shorten/{result.Value!.ShortCode}");
        ShortenUrlResponse response = new(result.Value.OriginalUrl, shortenedUrl, result.Value.ShortCode, result.Value.ExpiresAt);
        return Results.Created(shortenedUrl, response);
    }

    // GET /url-shortener/{shortCode}
    private static async Task<IResult> HandleGetShortenedUrl(HttpContext context, IUrlShortenerService urlShortenerService, string shortCode)
    {
        Result<ShortenedUrl> result = await urlShortenerService.GetShortenedUrl(shortCode);
        return result.IsSuccess == false
            ? Results.Problem(
                title: "An error occurred while getting the original URL",
                statusCode: result.Error!.Code,
                detail: result.Error!.Message)
            : Results.Redirect(result.Value!.OriginalUrl, true);
    }

    #endregion
}