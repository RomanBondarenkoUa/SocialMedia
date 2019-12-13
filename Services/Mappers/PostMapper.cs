using AutoMapper;
using Domain.Users.Posts;
using System.Collections.Generic;
using System.Linq;

namespace Services.Mappers
{
    public class PostMapper : Interfaces.IPostMapper
    {
        IMapper mapper;

        public PostMapper()
        {
            this.mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreatePostByUserModel, CreatePostModel>();
            })
            .CreateMapper();
        }

        public CreatePostModel Map(CreatePostByUserModel createPostByUserModel, string email)
        {
            var post = this.mapper.Map<CreatePostModel>(createPostByUserModel);
            post.PublisherEmail = email;

            return post;
        }

        public IEnumerable<PostPreviewDto> Map(IEnumerable<Domain.Users.SocialActivities.Post> posts)
        {
            return posts.Select(post =>
                new PostPreviewDto()
                {
                    publisherName = post.OwnerName,
                    publisheProfileLink = post.OwnerProfileLink,
                    Title = post.Title,
                    FullPostUrl = post.PostUrl,
                    PreviewText = post.Text.Substring(0, post.Text.Length < 150 ? post.Text.Length : 150) + "...",
                    TotalRate = post.TotalPositiveRate - post.TotalNegativeRate
                });
        }
    }
}
