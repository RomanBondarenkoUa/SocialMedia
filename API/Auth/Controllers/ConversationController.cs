using System;
using System.Collections.Generic;
using API.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain.Users.Conversations;

namespace SocialMedia.Users.Actions
{
    [Route("api/conversation")]
    [ApiController]
    public class ConversationController : ControllerBase
    {

        /// <summary>
        /// TODO:
        ///     if page = -1:
        ///         returns ALL Conversations for given user;
        ///     if page_id AND itemsPerPage specified:
        ///         returns conversations with numbers from (page_id * itemsPerPage) to (page_id * itemsPerPage + itemsPerPage); 
        ///     if itemsPerPage is not specified:
        ///         returns conversations with numbers from (page_id * 30) to (page_id * 30 + 30);
        ///     if userId does not exist:
        ///         return HTTP 400;
        /// </summary>
        //[HttpGet]
        //public Result<IEnumerable<Conversation>> GetConversations(
        //    [FromBody] int userId,
        //    [FromQuery] int page = 0,
        //    [FromQuery] int itemsPerPage = 30)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
