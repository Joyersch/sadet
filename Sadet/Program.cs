﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Sadet;
using Sadet.Steam.DataObjects;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((services) =>
    {
        services.AddSingleton<Library>();
        services.AddSingleton<ArgumentParser>();
        services.AddSingleton(Console.Out);
    })
    .Build();

var app = host.Services.GetRequiredService<ArgumentParser>();
var actions = await app.ParseAsync(args);

foreach (var action in actions)
    await action.ExecuteAsync();