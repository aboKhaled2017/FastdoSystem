using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys
{
    public class PagedList<T>:List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious
        {
            get
            {
                return CurrentPage > 1;
            }
        }
        public bool HasNext
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }
        public PagedList(List<T>items,int count,ResourceParameters _params)
        {
            TotalCount = count;
            PageSize =_params.PageSize;
            CurrentPage = _params.PageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)_params.PageSize);
            AddRange(items);
        }
        public static async Task<PagedList<T>>CreateAsync(IQueryable<T> source,ResourceParameters _params)
        {
            int count = source.Count();
            var items =await source.Skip(_params.PageSize * (_params.PageNumber - 1))
                .Take(_params.PageSize).ToListAsync();
            return new PagedList<T>(
                items,count, _params
            );
        }
        public static async Task<PagedList<Y>> CreateAsync<Y>(IQueryable<T> source, ResourceParameters _params,Func<T,Y>mappItem)
        {
            int count = source.Count();
            var items = await source.Skip(_params.PageSize * (_params.PageNumber - 1))
                .Take(_params.PageSize).ToListAsync();
            var mappedItems = new List<Y>();
            items.ForEach(item =>
            {
                mappedItems.Add(mappItem(item));
            });
            return new PagedList<Y>(
                mappedItems, count, _params
            );
        }
    }
}
