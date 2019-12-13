using AutoMapper;
using Domain.Users.Posts;
using Domain.Users.SocialActivities;
using Repository.Dapper.DataModels;
using Repository.Dapper.Mappers.Interfaces;
using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Dapper.Mappers
{
    public class PostMapper : IPostMapper
    {
        IMapper mapper;

        public PostMapper()
        {
            this.mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreatePostModel, DataModels.Post>();
            })
            .CreateMapper();
        }

        public DataModels.Post Map(CreatePostModel createPostModel)
        {
            return this.mapper.Map<DataModels.Post>(createPostModel);
        }

        public Domain.Users.SocialActivities.Post Map(Repository.Dapper.DataModels.Post post, string publisherName, string publisherEmail, IEnumerable<Comment> comments)
        {
            var commentsCount = comments.Count();

            var positiveTotal = comments.AggregatePositiveCommentsRate();
            var negativeTotal = comments.AggregateNegativeCommentsRate();

            return new Domain.Users.SocialActivities.Post()
            {
                PostUrl = new Uri($"/post/{post.Id}", UriKind.Relative),
                Text = post.Text,
                Title = post.Title,
                AttachmentUrl = post.AttachmentUrl,
                CreatedAt = post.CreatedAt,
                OwnerName = publisherName,
                OwnerProfileLink = new Uri($"/user/{publisherEmail}", UriKind.Relative),
                Comments = comments.Where(c => c.Text != null),
                TotalPositiveRate = positiveTotal,
                TotalNegativeRate = negativeTotal
            };
        }

        public Comment Map(Reaction reaction, string reactionAuthorName)
        {
            return new Comment()
            {
                CreatorName = reactionAuthorName,
                IsPositiveComment = reaction.IsPositive,
                Text = reaction.Text
            };
        }
    }
}
