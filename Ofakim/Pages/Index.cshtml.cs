using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Ofakim.Contracts;
using Ofakim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ofakim.Pages
{
    public class IndexModel : PageModel
    {
        public static object xmlFileLock = new object();
        public List<BlockModel> ModelList = new List<BlockModel>();
        private readonly ILogger<IndexModel> _logger;
        private readonly IReadFromFile _readToFile;

        public IndexModel(ILogger<IndexModel> logger, IReadFromFile readToFile)
        {
            _logger = logger;
            _readToFile = readToFile;
        }

        public void OnGet()
        {
            lock (xmlFileLock)
            {
                try
                {
                    ModelList = _readToFile.ReadFromFile();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    ModelList = new List<BlockModel>();
                }
            }
        }
    }
}
