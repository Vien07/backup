using Admin.Sample;
using Admin.Sample.Models;
using AutoMapper;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.LocalizeManagement.Services;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.Sample.Controllers
{


    public partial class SampleController : Controller
    {
        public ISampleService _rep;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        IFileHelper _fileHelper;

        public string CreateUser = "admin";
        private readonly IRepositoryConfig<Database.SampleConfig> _repConfig;
        private readonly IRepository<Database.SampleDetail> _repoSampleDetail;
        private readonly IRepository<Database.Sample_Files> _repoSampleFile;
        private readonly IMapper _mapper;
        Dictionary<string, string> _CONFIG;
        private IContentLocalizationService _localize;
        public PageModel _pageModel = new PageModel();
        public SampleController(ISampleService rep,
            IViewRendererHelper viewRender,
            ILoggerHelper logger,
            IRepository<Database.SampleDetail> repoSampleDetail,
            IRepository<Database.Sample_Files> repoSampleFile,
            IRepositoryConfig<Database.SampleConfig> repConfig,
            IContentLocalizationService localize,
                  IFileHelper fileHelper)
        {
            _viewRender = viewRender;
            _rep = rep;
            _repConfig = repConfig;
            _repoSampleDetail = repoSampleDetail;
            _repoSampleFile = repoSampleFile;
            _logger = logger;
            _CONFIG = _repConfig.GetAllConfigs();
            _fileHelper = fileHelper;
            _localize = localize;
            _mapper = AutoMapperConfig.Initialize();

        }
    }

}
