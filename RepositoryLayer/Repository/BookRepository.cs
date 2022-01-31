using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ModelLayer;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly IMongoCollection<BooksModel> Books;
        private readonly IConfiguration configuration;

        public BookRepository(IDBSetting db, IConfiguration configuration)
        {
            this.configuration = configuration;
            var client = new MongoClient(db.ConnectionString);
            var database = client.GetDatabase(db.DatabaseName);
            Books = database.GetCollection<BooksModel>("Books");
        }
        public async Task<BooksModel> AddBook(BooksModel addbook)
        {
            try
            {
                var ifExists = await this.Books.Find(x => x.bookID == addbook.bookID).SingleOrDefaultAsync();
                if(ifExists == null)
                {
                    await this.Books.InsertOneAsync(addbook);
                    return addbook;
                }
                return null;

            }
            catch(ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BooksModel> UpdateBook(BooksModel editbook)
        {
            try
            {
                var ifExists = await this.Books.Find(x => x.bookID == editbook.bookID).FirstOrDefaultAsync();
                if (ifExists != null)
                {
                    //// replace cannot be used as it replaces whole doc not a specific field, other field becomes null
                    // await this.Books.ReplaceOneAsync<BooksModel>(x => x.bookID == editbook.bookID, editbook);
                    await this.Books.UpdateOneAsync(x => x.bookID == editbook.bookID,
                        Builders<BooksModel>.Update.Set(x=>x.bookName, editbook.bookName)
                        .Set(x => x.description, editbook.description)
                        .Set(x => x.authorName, editbook.authorName)
                        .Set(x => x.rating, editbook.rating)
                        .Set(x => x.totalRating, editbook.totalRating)
                        .Set(x => x.discountPrice, editbook.discountPrice));
                    return ifExists;
                }
                else
                {
                    await this.Books.InsertOneAsync(editbook);
                    return editbook;
                }
            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteBook(BooksModel delbook)
        {
            try
            {
                var ifExists = await this.Books.FindOneAndDeleteAsync(x => x.bookID == delbook.bookID);
                return true;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BooksModel> BookImg(string bookId, IFormFile img)
        {
            try
            {
                Account account = new Account(this.configuration["CloudinaryAccount:Name"], this.configuration["CloudinaryAccount:ApiKey"], this.configuration["CloudinaryAccount:ApiSecret"]);
                var cloudinary = new Cloudinary(account);
                var uploadparams = new ImageUploadParams()
                {
                    File = new FileDescription(img.FileName, img.OpenReadStream()),
                };
                var uploadResult = cloudinary.Upload(uploadparams);
                string imagePath = uploadResult.Url.ToString();
                var ifExists = await this.Books.AsQueryable().Where(x => x.bookID == bookId).SingleOrDefaultAsync();
                if (ifExists != null)
                {
                    ifExists.bookImage = imagePath;
                    await this.Books.UpdateOneAsync(x => x.bookID == bookId,
                        Builders<BooksModel>.Update.Set(x => x.bookImage, ifExists.bookImage));
                    return ifExists;
                }

                return null;
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<BooksModel> GetAllBook()
        {
            return Books.Find(FilterDefinition<BooksModel>.Empty).ToList();
        }

        public BooksModel GetByBookId(string id)
        {
            return Books.Find(x => x.bookID == id).FirstOrDefault();
        }
    }
}
