using ModelLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IFeedbackRepository
    {
        Task<FeedbackModel> AddFeedback(FeedbackModel feed);
        IEnumerable<FeedbackModel> GetFeedback();
    }
}
