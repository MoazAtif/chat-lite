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
    public class CommentController : ControllerBase
    {

        public AppDbcontext dbcontext;
        public CommentController(AppDbcontext _appdb)
        {
           dbcontext = _appdb;
        }
        [HttpGet]
        [Authorize]

        public async Task< IActionResult> GetAll()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);

            var res=await dbcontext.comments.Include(c=>c.User).Include(c=>c.Post).Select(c=>new commentdto{
            content = c.Content,
            UserName=c.User.UserName,
            PostId=c.PostId,
            
            
            }).ToListAsync();

            return Ok(res);
        }
        [HttpPost]
        public IActionResult Add(commentdto commentdto) {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    PostId=commentdto.PostId,
                    Content =commentdto.content,
                    UserId=commentdto.UserId
                };
                dbcontext.comments.Add(comment);
                dbcontext.SaveChanges();
                return Ok();
            }
            return BadRequest(ModelState);
        
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);

            var res = dbcontext.comments.FirstOrDefault(a => a.Id == id);
            if (res == null)
                return NotFound();
            dbcontext.comments.Remove(res);
            dbcontext.SaveChanges();
            return Ok();

        }
        [HttpGet("post/{postid}")]
        public IActionResult Getbypost(int postid)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var res = dbcontext.comments.Where(c => c.PostId == postid).Include(c=>User).Include(c=>c.Post).Select(c=>new commentdto{
            content=c.Content,
            UserName=c.User.UserName,
            postcontent=c.Post.Content}).ToList();
            return Ok(res);
        }


    }
}
