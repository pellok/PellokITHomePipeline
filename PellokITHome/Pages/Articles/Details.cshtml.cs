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
    public class DetailsModel : PageModel
    {
        private readonly PellokITHome.Data.PellokITHomeContext _context;

        public DetailsModel(PellokITHome.Data.PellokITHomeContext context)
        {
            _context = context;
        }

        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Articles.FirstOrDefaultAsync(m => m.ID == id);

            if (Article == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
