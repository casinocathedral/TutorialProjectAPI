using Microsoft.AspNetCore.Mvc;
using TutorialProjectAPI.Contexts;
using TutorialProjectAPI.Models;
using TutorialProjectAPI.Repositories;

namespace TutorialProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IdentifiableRepository<PostDB> _postRepository;
        private readonly IdentifiableRepository<ReplyDB> _replyRepository;
        private readonly MainContext _context;

        public PostController(
            IdentifiableRepository<PostDB> postRepository,
            IdentifiableRepository<ReplyDB> replyRepository,
            MainContext context)
        {
            _postRepository = postRepository;
            _replyRepository = replyRepository;
            _context = context;
        }

        [HttpPost]
        public IActionResult CreatePosts([FromBody] List<PostWithRepliesDTO> posts)
        {
            if (posts == null || posts.Count == 0)
                return BadRequest("No posts provided.");

            var createdPosts = new List<PostDB>();

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                foreach (var postDto in posts)
                {
                    // Validate UserId
                    if (!_context.Users.Any(u => u.Id == postDto.UserId))
                        return BadRequest($"User with ID {postDto.UserId} does not exist.");

                    Guid? imageId = null;

                    // Handle image creation or reference
                    if (!string.IsNullOrWhiteSpace(postDto.ImageBase64))
                    {
                        if (postDto.ImageBase64.Length > 136000)
                            return BadRequest("Image exceeds 100kb base64 size limit.");

                        var image = new ImageDB
                        {
                            Id = Guid.NewGuid(),
                            Base64Image = postDto.ImageBase64
                        };
                        _context.Images.Add(image);
                        _context.SaveChanges();
                        imageId = image.Id;
                    }
                    else if (postDto.ImageId.HasValue)
                    {
                        // Check if the referenced image exists
                        if (!_context.Images.Any(i => i.Id == postDto.ImageId.Value))
                            return BadRequest($"Image with ID {postDto.ImageId.Value} does not exist.");
                        imageId = postDto.ImageId.Value;
                    }

                    var post = new PostDB
                    {
                        Id = Guid.NewGuid(),
                        Body = postDto.Body,
                        UserId = postDto.UserId,
                        ImageId = imageId,
                        Replies = new List<ReplyDB>()
                    };

                    _postRepository.Insert(post);
                    _context.SaveChanges(); // Save post to generate PostId

                    if (postDto.Replies != null)
                    {
                        foreach (var replyDto in postDto.Replies)
                        {
                            // Validate UserId for replies
                            if (!_context.Users.Any(u => u.Id == replyDto.UserId))
                                return BadRequest($"User with ID {replyDto.UserId} does not exist.");

                            var reply = new ReplyDB
                            {
                                Id = Guid.NewGuid(),
                                Body = replyDto.Body,
                                UserId = replyDto.UserId,
                                PostId = post.Id
                            };
                            _replyRepository.Insert(reply);
                        }
                    }

                    _context.SaveChanges(); // Save replies
                    createdPosts.Add(post);
                }

                transaction.Commit();
                return Ok(createdPosts);
            }
            catch
            {
                transaction.Rollback();
                return StatusCode(500, "An error occurred while creating posts.");
            }
        }

        // ... other CRUD endpoints ...
    }
}