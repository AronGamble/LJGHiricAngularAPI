﻿using LJGHistoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJGHistoryService.Repositories
{
    public interface IAuthenticationRepository
    {

        User GetJWTToken(User user);

    }
}
