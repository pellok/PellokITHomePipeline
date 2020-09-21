using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PellokITHome.Data;
using PellokITHome.Models;

namespace PellokITHome.Pages.Articles
{
    public class IndexModel : PageModel
    {
        private readonly PellokITHome.Data.PellokITHomeContext _context;

        public IndexModel(PellokITHome.Data.PellokITHomeContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public async Task OnGetAsync()
        {
            var articles = from a in _context.Articles select a;
            if (!string.IsNullOrEmpty(SearchString))
            {
                articles = articles.Where(s => s.Title.Contains(SearchString));
            }
            
            Article = await articles.ToListAsync();
        }
    }
}
