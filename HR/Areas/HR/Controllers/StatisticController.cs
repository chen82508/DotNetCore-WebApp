using HR.Areas.HR.Service;
using HR.DTOs.HR;
using HR.Interface;
using HR.Utils.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HR.Areas.HR.Controllers
{
    [Area("HR")]
    public class StatisticController : AreaBaseController
    {
        private readonly IEntityService<StatisticData> _StatService;
        public StatisticController(IEntityService<StatisticData> service)
        {
            _StatService = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/api/Statistic/OverView")]
        public IEnumerable<StatisticData> GetOverViews()
        {
            return _StatService.GetData();
        }

        [HttpGet]
        [Route("/api/Statistic/Vacancy")]
        public IEnumerable<VacancyStatistic> GetVacancyStatistic()
        {
            return (_StatService as StatisticDataService).GetVacancyStat();
        }
    }
}
