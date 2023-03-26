using Microsoft.EntityFrameworkCore;
using NLayerApp.DAL.EF;
using NLayerApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.DAL.Repositories
{
        public class RoleRepository
        {
            private PitchforkContext db;

            public RoleRepository(PitchforkContext context)
            {
                this.db = context;
            }
            public IEnumerable<Role> GetAll()
            {
                return db.Roles;
            }
            public Role Get(int id)
            {
                return db.Roles.Find(id);
            }

            public void Create(Role user)
            {
                db.Roles.Add(user);
            }

            public void Update(Role user)
            {
                db.Entry(user).State = EntityState.Modified;
            }

            public IEnumerable<Role> Find(Func<Role, Boolean> predicate)
            {
                return db.Roles.Where(predicate).ToList();
            }

            public void Delete(int id)
            {
                Role role = db.Roles.Find(id);
                if (role != null)
                    db.Roles.Remove(role);
            }

        }
    }
