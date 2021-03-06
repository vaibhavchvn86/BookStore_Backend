using Microsoft.AspNetCore.Http;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IBookRepository
    {
        Task<BooksModel> AddBook(BooksModel addbook);
        Task<BooksModel> UpdateBook(BooksModel editbook);
        Task<bool> DeleteBook(BooksModel delbook);
        Task<BooksModel> BookImg(string bookId, IFormFile img);
        IEnumerable<BooksModel> GetAllBook();
        BooksModel GetByBookId(string id);
    }
}
