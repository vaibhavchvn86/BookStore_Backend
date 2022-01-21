using ManagerLayer.Interface;
using ModelLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Manager
{
    public class FeedbackManager : IFeedbackManager
    {
        private readonly IFeedbackRepository repo;
        public FeedbackManager(IFeedbackRepository repo)
        {
            this.repo = repo;
        }
        public async Task<FeedbackModel> AddFeedback(FeedbackModel feed)
        {
            try
            {
                return await this.repo.AddFeedback(feed);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<FeedbackModel> GetFeedback()
        {
            try
            {
                return this.repo.GetFeedback();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
