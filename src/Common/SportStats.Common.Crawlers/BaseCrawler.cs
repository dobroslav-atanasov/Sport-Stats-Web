﻿namespace SportStats.Common.Crawlers;

using Microsoft.Extensions.Logging;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Http;
using SportStats.Services.Data.CrawlerStorage.Interfaces;
using SportStats.Services.Interfaces;

public abstract class BaseCrawler
{
    private readonly ICrawlersService crawlersService;
    private readonly IGroupsService groupsService;
    private readonly Lazy<int> crawlerId;

    public BaseCrawler(ILogger<BaseCrawler> logger, IHttpService httpService, ICrawlersService crawlersService, IGroupsService groupsService)
    {
        this.Logger = logger;
        this.HttpService = httpService;
        this.crawlersService = crawlersService;
        this.groupsService = groupsService;
        this.crawlerId = new Lazy<int>(() => this.crawlersService.GetCrawlerIdAsync(this.GetType().FullName).GetAwaiter().GetResult());
    }

    protected ILogger<BaseCrawler> Logger { get; }

    protected IHttpService HttpService { get; }

    public abstract Task StartAsync();

    protected async Task ProcessGroupAsync(HttpModel httpModel)
    {
        var name = this.GetNameFromUrl(httpModel);
        var document = this.CreateDocument(httpModel);

        var group = new Group
        {
            Name = name,
            CrawlerId = this.crawlerId.Value,
            Documents = new List<Document> { document }
        };

        await this.groupsService.AddOrUpdateGroupAsync(group);
    }

    protected async Task ProcessGroupAsync(HttpModel httpModel, ICollection<Document> documents)
    {
        var name = this.GetNameFromUrl(httpModel);

        var group = new Group
        {
            Name = name,
            CrawlerId = this.crawlerId.Value,
            Documents = documents
        };

        await this.groupsService.AddOrUpdateGroupAsync(group);
    }

    protected Document CreateDocument(HttpModel httpModel)
    {
        var name = this.GetNameFromUrl(httpModel);
        var document = new Document
        {
            Name = $"{name}.html".ToLower(),
            Format = httpModel.MimeType,
            Url = httpModel.Url,
            Content = httpModel.Bytes,
            Encoding = httpModel.Encoding.BodyName,
            Order = 1
        };

        return document;
    }

    protected virtual string GetNameFromUrl(HttpModel httpModel)
    {
        var uri = httpModel.Uri;
        var nameParts = uri.AbsolutePath.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        var name = string.Join("_", nameParts);
        name = name.Replace(".html", string.Empty);

        return name;
    }

    protected string CreateUrl(string link, string mainUrl)
    {
        var mainUri = new Uri(mainUrl);
        var uri = new Uri(mainUri, link);

        return uri.AbsoluteUri;
    }
}