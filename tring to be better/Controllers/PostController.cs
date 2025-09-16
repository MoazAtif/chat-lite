using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tring_to_be_better.DTOS;
using tring_to_be_better.Model;

namespace tring_to_be_better.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private AppDbcontext dbcontext;
        public PostController(AppDbcontext a)
        {
            dbcontext = a;
            
        }
        [HttpPost]
        public IActionResult Create(postcreateDTO postdto)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var post = new Post
            {
                Title = postdto.title,
                Content = postdto.content,

                UserId = postdto.userId,
                
            };

            dbcontext.posts.Add(post);
            dbcontext.SaveChanges();
            return Ok(post);
        
        }
        [HttpGet]
        public async Task< IActionResult> Get()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var res =await dbcontext.posts
                .Include(c=>c.User)
                .ThenInclude(c=>c.Comments)
                .Select(c=>new Postdto{
                Title = c.Title,
                content = c.Content,
             
                UserName=c.User.UserName,
                comments=c.Comments.Select(a=>new commentdto{

                    Id=a.Id,
                    content = a.Content,
                    UserName = a.User.UserName,
                    postcontent=a.Post.Content
                    

                }).ToList()
            
            
            
            }).ToListAsync();
            return Ok(res);
        }
        [HttpGet("gitone({id})")]
        public async Task< IActionResult> Get(int id)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var res = dbcontext.posts
                .Include(c=>c.User)
                .Include(c=>c.Comments)
                .Select(c=>new Postdto
                {
                    Id = c.Id,
                    Title = c.Title,
                    content = c.Content,

                    UserName = c.User.UserName,
                    comments = c.Comments.Select(a => new commentdto
                    {

                        Id = a.Id,
                        content = a.Content,
                        UserName = a.User.UserName,
                        postcontent = a.Post.Content

                    }).ToList()
                }).FirstOrDefault(a=>a.Id == id);
            if (res == null)
                return BadRequest(ModelState);
            return Ok(res);
        }
         [HttpPut]
          public async Task< IActionResult> Edit(postputdto post)
          {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var res=await dbcontext.posts.Select(c=>new postputdto{
        Id = c.Id,
        content = c.Content,
        Title = c.Title,
        }).FirstOrDefaultAsync(b => b.Id == post.Id);
            if (res == null)
                return BadRequest(ModelState);
            res.Title = post.Title;
            res.content = post.content;
            dbcontext.SaveChanges();
            return Ok(res);

        
        
          }
            [HttpDelete("{id}")]
            public IActionResult Delete(int id)
            {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var res= dbcontext.posts.FirstOrDefault(a=>a.Id == id);
            if(res == null)
                return BadRequest(ModelState);
            dbcontext.Remove(res);
            dbcontext.SaveChanges();
            return Ok();
        
        
            }
    }
}
