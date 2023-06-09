﻿using Microsoft.EntityFrameworkCore;
using NLayerApp.DAL.EF;
using NLayerApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.DAL.Repositories
{
    public class UserRepository
    {
        private PitchforkContext db;

        public UserRepository(PitchforkContext context)
        {
            this.db = context;
        }
        public IEnumerable<User> GetAll()
        {
            return db.Users;
        }
        public User Get(int id)
        {
            return db.Users.Find(id);
        }

        public void Create(User user)
        {
            db.Users.Add(user);
        }

        public void Update(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public IEnumerable<User> Find(Func<User, Boolean> predicate)
        {
            return db.Users.Include(u => u.Role).Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            User user = db.Users.Find(id);
            if (user != null)
                db.Users.Remove(user);
        }

    }
}
