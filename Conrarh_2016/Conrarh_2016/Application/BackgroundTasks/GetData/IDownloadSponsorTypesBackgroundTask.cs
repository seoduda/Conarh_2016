using System.Collections.Generic;
using Conarh_2016.Application.Domain;

namespace Conarh_2016.Application.BackgroundTasks
{
    public interface IDownloadSponsorTypesBackgroundTask
    {
        List<SponsorType> Execute();
    }
}