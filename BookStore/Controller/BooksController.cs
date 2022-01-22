using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    { 
        private readonly IBookManager manager;

        public BooksController(IBookManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("addbook")]
        public async Task<IActionResult> AddBook(BooksModel addbook)
        {
            try
            {
                var ifExists = await this.manager.AddBook(addbook);
                if(ifExists !=null)
                {
                    return this.Ok(new ResponseModel<BooksModel> { Status = true, Message = "Book Added Successfully", Data = ifExists });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Book not Added" });
                }

            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpPut]
        [Route("updatebook")]
        public async Task<IActionResult> UpdateBook(BooksModel editbook)
        {
            try
            {
                var ifExists = await this.manager.UpdateBook(editbook);
                if (ifExists != null)
                {
                    return this.Ok(new ResponseModel<BooksModel> { Status = true, Message = "Book Updated Successfully", Data = ifExists });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Book not Updated" });
                }

            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpPut]
        [Route("uploadimage")]
        public async Task<IActionResult> BookImg(string bookId, IFormFile img)
        {
            try
            {
                var ifExists = await this.manager.BookImg(bookId, img);
                if (ifExists != null)
                {
                    return this.Ok(new ResponseModel<BooksModel> { Status = true, Message = "Image Uploaded Successfully", Data = ifExists });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Image not Uploaded" });
                }

            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpDelete]
        [Route("deletebook")]
        public async Task<IActionResult> DeleteBook(BooksModel delbook)
        {
            try
            {
                var ifExists = await this.manager.DeleteBook(delbook);
                if (ifExists == true)
                {
                    return this.Ok(new { Status = true, Message = "Book Deleted Successfully", Data = ifExists });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Book not Deleted" });
                }

            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpGet]
        [Route("getallbooks")]
        public IActionResult GetAllBook()
        {
            try
            {
                IEnumerable<BooksModel> ifExists = this.manager.GetAllBook();
                if (ifExists != null)
                {
                    return this.Ok(new { Status = true, Message = "Books Retrived Successfully", Data = ifExists });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Books not Found" });
                }

            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        [HttpGet]
        [Route("getbooksbyid")]
        public IActionResult GetByBookId(string id)
        {
            try
            {
                var ifExists = this.manager.GetByBookId(id);
                if (ifExists != null)
                {
                    return this.Ok(new { Status = true, Message = "Book Found Successfully", Data = ifExists });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Book not Found" });
                }

            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }
    }
}
