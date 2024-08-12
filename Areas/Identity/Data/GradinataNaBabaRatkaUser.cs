using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GradinataNaBabaRatka.Areas.Identity.Data;

// Add profile data for application users by adding properties to the GradinataNaBabaRatkaUser class
public class GradinataNaBabaRatkaUser : IdentityUser
{
    public string Ime { get; internal set; }
}

