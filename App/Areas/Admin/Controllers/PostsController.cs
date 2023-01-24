using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Indigo.DAL;
using Indigo.Entities.Concrets;
using Indigo.Entities.DTOs.PostDtos;
using AutoMapper;
using Indigo.Business.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Indigo.Utilities.Validation.Fluent.Post;

namespace Indigo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _env;
        private readonly PostPostDtoValidation _validator;

        public PostsController(AppDbContext context, IMapper mapper, IImageService imageService, IWebHostEnvironment env, PostPostDtoValidation validator)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            _env = env;
            _validator = validator;
        }

        public async Task<IActionResult> Index()
        {
            List<PostGetDto> posts = _mapper.Map<List<PostGetDto>>(await _context.Posts.ToListAsync());
            return View(posts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostPostDto postDto)
        {
                var result = _validator.Validate(postDto);
                if (!result.IsValid)
                {
                    ModelState.AddModelError("",result.Errors.FirstOrDefault().ErrorMessage);
                    return View(postDto);
                }
                if(postDto.FormFile is null)
                {
                    ModelState.AddModelError("FormFile","Image cannot be null");
                    return View(postDto);
                }
                try
                {
                    _imageService.CheckImage(postDto.FormFile);
                    _imageService.CheckSize(postDto.FormFile);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("FormFile", ex.Message);
                    return View(postDto);
                }
                string ImageName= await _imageService.CreateImageAsync(_env.WebRootPath,"assets/images/post-images/",postDto.FormFile);
                Post NewPost = new()
                {
                    Name = postDto.Name,
                    Description = postDto.Description,
                    Image= ImageName
                };
                _context.Add(NewPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            PostGetDto post = _mapper.Map<PostGetDto>(await _context.Posts.FindAsync(id));
            if (post == null)
            {
                return NotFound();
            }
            PostUpdateDto updateDto = new() { getDto = post };
            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostUpdateDto updateDto)
        {
            if (ModelState.IsValid)
            {
                var result = _validator.Validate(updateDto.postDto);
                if (!result.IsValid)
                {
                    ModelState.AddModelError("", result.Errors.FirstOrDefault().ErrorMessage);
                    return View(updateDto);
                }
                try
                {
                    Post post = _mapper.Map<Post>(await _context.Posts.FindAsync(updateDto.getDto.Id));
                    if (updateDto.postDto.FormFile is not null)
                    {
                        try
                        {
                            _imageService.CheckImage(updateDto.postDto.FormFile);
                            _imageService.CheckSize(updateDto.postDto.FormFile);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("FormFile", ex.Message);
                            return View(updateDto);
                        }
                        string ImageName= await _imageService.CreateImageAsync(_env.WebRootPath,"assets/images/post-images/",updateDto.postDto.FormFile);
                        post.Image=ImageName;
                    }
                    post.Name = updateDto.postDto.Name;
                    post.Description = updateDto.postDto.Description;
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(updateDto.getDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updateDto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'AppDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                post.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
