using AutoMapper;
using Indigo.DAL;
using Indigo.Entities.DTOs.PostDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Indigo.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        public HomeController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<PostGetDto> posts=_mapper.Map<List<PostGetDto>>(await _db.Posts.Take(3).ToListAsync());
            return View(posts);
        }
    }
}
