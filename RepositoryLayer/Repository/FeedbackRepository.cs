using ModelLayer;
using MongoDB.Driver;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly IMongoCollection<FeedbackModel> Feedback;

        public FeedbackRepository(IDBSetting db)
        {
            var client = new MongoClient(db.ConnectionString);
            var database = client.GetDatabase(db.DatabaseName);
            Feedback = database.GetCollection<FeedbackModel>("Feedback");
        }
        public async Task<FeedbackModel> AddFeedback(FeedbackModel feed)
        {
            try
            {
                var check = await this.Feedback.Find(x => x.feedbackID == feed.feedbackID).SingleOrDefaultAsync();
                if (check == null)
                {
                    await this.Feedback.InsertOneAsync(feed);
                    return feed;

                }
                return null;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<FeedbackModel> GetFeedback()
        {
            return Feedback.Find(FilterDefinition<FeedbackModel>.Empty).ToList();
        }
    }
}
