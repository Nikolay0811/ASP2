﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ASPProject1.Data;

namespace ASPProject1.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)            
        {

        }
        public DbSet<Trophy> Trophys { get; set; }
        public DbSet<Repertoire> Repertories { get; set; }
        public DbSet<News> Newes { get; set; }
        public DbSet<Messages> Messages2 { get; set; }
        public DbSet<NewsImages> NewsImages { get; set; }
        public DbSet<TrophyImages> TrophyImages { get; set; }
    }  
}

