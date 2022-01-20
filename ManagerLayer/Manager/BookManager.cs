using ManagerLayer.Interface;
using Microsoft.AspNetCore.Http;
using ModelLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Manager
{
    public class BookManager : IBookManager
    {
        private readonly IBookRepository repo;
        public BookManager(IBookRepository repo)
        {
            this.repo = repo;
        }
        public async Task<BooksModel> AddBook(BooksModel addbook)
        {
            try
            {
                return await this.repo.AddBook(addbook);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BooksModel> UpdateBook(BooksModel editbook)
        {
            try
            {
                return await this.repo.UpdateBook(editbook);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BooksModel> BookImg(string bookId, IFormFile img)
        {
            try
            {
                return await this.repo.BookImg(bookId, img);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteBook(BooksModel delbook)
        {
            try
            {
                return await this.repo.DeleteBook(delbook);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<BooksModel> GetAllBook()
        {
            try
            {
                return this.repo.GetAllBook();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public BooksModel GetByBookId(string id)
        {
            try
            {
                return this.repo.GetByBookId(id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
