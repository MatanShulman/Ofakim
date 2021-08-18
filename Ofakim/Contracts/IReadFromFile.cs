using Ofakim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofakim.Contracts
{
    public interface IReadFromFile
    {
        List<BlockModel> ReadFromFile();
    }
}
