/*
    NanoBadge. An endpoint to create badges with MAUI.
    Copyright (C) 2022  Erick Fernando Mora Ramirez

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.

    mailto:erickfernandomoraramirez@gmail.com
*/

using Microsoft.Maui.Graphics;
using NanoBadges;

int port = 8090;
if(args.Length > 0 && int.TryParse(args[0], out port)) { }

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();
Dictionary<string, Color> colors = new Dictionary<string, Color>
{
    {"black", Colors.Black },
    {"orange", new Color(255, 102, 0) },
    {"orange-red", new Color(255, 51, 0) },
    {"red", new Color(255, 0, 0) },
    {"green", new Color(0, 128, 0) },
    {"blue", new Color(0, 51, 204) },
    {"mustard", new Color(204, 153, 0) },
    {"gray", new Color(30, 30, 30) }
};

Dictionary<string, List<DateTime?>> requests = new();

app.MapGet("/nano-badge/{text?}/{title?}/{color?}/{titleColor?}/{font?}", (HttpRequest req, string? text, string? title, string? color, string? titleColor, string? font) =>
{
    string clientAddress = req.HttpContext.Connection.RemoteIpAddress?.ToString() ?? String.Empty;
    string message = $"[{DateTime.Now:yyyy-MM-dd hh:mm:ss}] Request from {clientAddress}";
    if (!requests.ContainsKey(clientAddress))
    {
        requests[clientAddress] = new List<DateTime?>();
    }
    TimeSpan sinceLastRequest = DateTime.Now - (requests[clientAddress].LastOrDefault() ?? DateTime.MinValue);
    requests[clientAddress].Add(DateTime.Now);
    if (sinceLastRequest.TotalSeconds < 5)
    {
        app.Logger.LogError(message);
        return Results.BadRequest("Too many requests");
    }
    if (sinceLastRequest.TotalMinutes < 10)
    {
        app.Logger.LogWarning(message);
    }
    else
    {
        app.Logger.LogInformation(message);
    }


    color = color ?? "blue";
    titleColor = titleColor ?? "gray";
    font = (font ?? "candara").Replace("_", " ");
    text = (text ?? "|").Replace("_", " ");
    title = (title ?? "|").Replace("_", " ");
    if(!colors.ContainsKey(color) || !colors.ContainsKey(titleColor))
    {
        return Results.BadRequest("Invalid color");
    }
    if(text.Length > 256 || title.Length > 256)
    {
        return Results.BadRequest("Text too long");
    }
    Font badgeFont = new Font(font, 800);
    BadgeBuilder badge = new BadgeBuilder(badgeFont, colors[titleColor], colors[color]);
    return Results.File(badge.TitledBadge(title ?? string.Empty, text ?? string.Empty) ?? new byte[0], "image/png");
});

app.MapGet("/colors", () => colors.Keys.ToList());
string url = $"http://localhost:{port}";
app.Run(url);