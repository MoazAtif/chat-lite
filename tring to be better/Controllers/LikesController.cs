using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tring_to_be_better.Model;

namespace tring_to_be_better.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LikesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbcontext _appDbcontext;
        public LikesController(AppDbcontext appDbcontext, IConfiguration configuration)
        {
            _appDbcontext = appDbcontext;
            _configuration = configuration;

        }
        [Authorize]
        [HttpPost("{postid}")]
        public IActionResult Addlike(int postid)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var alreadyliked = _appDbcontext.like.FirstOrDefault(a => a.UserId == userId && a.PostId == postid);
            if (alreadyliked != null)
                return BadRequest("you are already liked");
            var like = new Like { PostId = postid, UserId = userId };
            _appDbcontext.Add(like);
            _appDbcontext.SaveChanges();
            return Ok("Liked");



        }
        [Authorize]
        [HttpDelete("{postid}")]
        public IActionResult RemoveLike(int postid)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var like = _appDbcontext.like.FirstOrDefault(a => a.UserId == userId && a.PostId == postid);
            if (like == null)
                return BadRequest("Like not found");
            _appDbcontext.like.Remove(like);
            _appDbcontext.SaveChanges();
            return Ok("unliked");



        }
        [HttpGet("{postid}")]
        public IActionResult countlikes(int postid)
        {
            var res = _appDbcontext.like.Where(a => a.PostId == postid).Count();
            return Ok(res);
        }
    }
}
