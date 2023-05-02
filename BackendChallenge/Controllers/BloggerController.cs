using BackendChallenge.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//namespace BackendChallenge.Controllers
//{
//    public class BloggerController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}
[Route("api/[controller]")]
[ApiController]
public class BloggerController : ControllerBase
{
    private readonly List<BackendChallenge.Model.Blogger> _bloggers;

    public BloggerController()
    {
        _bloggers = new List<Blogger>();
    }

    [HttpPost]
    public IActionResult Create([FromBody] Blogger blogger)
    {
        blogger.Id = _bloggers.Count + 1;
        _bloggers.Add(blogger);
        return CreatedAtAction(nameof(GetById), new { id = blogger.Id }, blogger);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var basicBloggers = _bloggers.Select(b => new {
            b.Id,
            b.Name,
            b.Website,
            b.Picture
        });
        return Ok(basicBloggers);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var blogger = _bloggers.FirstOrDefault(b => b.Id == id);
        if (blogger == null)
        {
            return NotFound();
        }
        return Ok(blogger);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Blogger blogger)
    {
        var existingBlogger = _bloggers.FirstOrDefault(b => b.Id == id);
        if (existingBlogger == null)
        {
            return NotFound();
        }
        existingBlogger.Name = blogger.Name;
        existingBlogger.Email = blogger.Email;
        existingBlogger.Website = blogger.Website;
        existingBlogger.Picture = blogger.Picture;
        existingBlogger.FriendIds = blogger.FriendIds;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var blogger = _bloggers.FirstOrDefault(b => b.Id == id);
        if (blogger == null)
        {
            return NotFound();
        }
        _bloggers.Remove(blogger);
        return NoContent();
    }

    [HttpPost("{id}/friends/{friendId}")]
    public IActionResult AddFriend(int id, int friendId)
    {
        var blogger = _bloggers.FirstOrDefault(b => b.Id == id);
        var friend = _bloggers.FirstOrDefault(b => b.Id == friendId);
        if (blogger == null || friend == null)
        {
            return NotFound();
        }
        if (blogger.FriendIds.Contains(friendId))
        {
            return BadRequest("Blogger is already a friend.");
        }
        blogger.FriendIds.Add(friendId);
        
        return NoContent();
    }

    [HttpGet("search")]
    public IActionResult Search(string email = null, string website = null)
    {
        var bloggers = _bloggers.Where(b =>
            (email == null || b.Email.ToLower().Contains(email.ToLower())) &&
            (website == null || b.Website.ToLower().Contains(website.ToLower()))
        ).Select(b => new {
            b.Picture,
            b.Name,
            b.Website
        });
        return Ok(bloggers);
    }

    [HttpGet("friends/{id}")]
    public IActionResult GetFriends(int id)
    {
        var blogger = _bloggers.FirstOrDefault(b => b.Id == id);
        if (blogger == null)
        {
            return NotFound();
        }
        var friends = _bloggers.Where(b => blogger.FriendIds.Contains(b.Id)).Select(b => new {
            b.Picture,
            b.Name,
            b.Website
        });
        return Ok(friends);
    }


}
