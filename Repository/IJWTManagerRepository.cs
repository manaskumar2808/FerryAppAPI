using FerryAppApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryAppApi.Repository
{
   public interface IJWTManagerRepository
    {
        Tokens Authenticate(ManasUser manasUser); 
    }
}