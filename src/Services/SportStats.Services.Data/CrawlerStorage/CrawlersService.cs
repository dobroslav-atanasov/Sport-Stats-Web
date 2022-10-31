﻿namespace SportStats.Services.Data.CrawlerStorage;

using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SportStats.Data.Contexts;
using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Services.Data.CrawlerStorage.Interfaces;

public class CrawlersService : BaseCrawlerStorageService, ICrawlersService
{
    public CrawlersService(CrawlerStorageDbContext context)
        : base(context)
    {
    }

    public async Task AddCrawler(string crawlerName)
    {
        var crawler = new Crawler { Name = crawlerName };
        await this.Context.AddAsync(crawler);
        await this.Context.SaveChangesAsync();
    }

    public async Task<int> GetCrawlerIdAsync(string crawlerName)
    {
        var crawler = await this.Context
            .Crawlers
            .FirstOrDefaultAsync(x => x.Name == crawlerName);

        if (crawler == null)
        {
            await this.AddCrawler(crawlerName);
            return await this.GetCrawlerIdAsync(crawlerName);
        }

        return crawler.Id;
    }
}