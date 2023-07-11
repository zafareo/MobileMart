using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Pagination<T>
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevPage => Page > 1;
        public bool HasNextPage => TotalPages > Page;

        public List<T> Items { get; set;} = new List<T>();    
        public Pagination(int page, int count, int pageSize, List<T> items)
        {
            Page = page;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items.AddRange(items);
        }

        public static async Task<Pagination<T>> CreateAsync(IQueryable<T> source, int page, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((page-1) * pageSize).Take(pageSize).ToListAsync();
            return new Pagination<T>(page, count, pageSize, items);
        }
    }
}
