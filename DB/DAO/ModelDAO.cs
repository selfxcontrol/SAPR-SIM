using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DAO
{
    public class ModelDAO : DAO<Model>
    {

        public void save(Model model)
        {
            using (var ctx = new SaprSimDbContext())
            {
                ctx.models.Add(model);
                ctx.SaveChanges();
            }
        }

        public void saveAll(ICollection<Model> models)
        {
            using (var ctx = new SaprSimDbContext())
            {
                foreach (Model m in models)
                {
                    ctx.models.Add(m);
                }

                ctx.SaveChanges();
            }
        }

        public Model findById(int id)
        {
            using (var ctx = new SaprSimDbContext())
            {
                return ctx.models
                                  .Include(s => s.entities).Include(s => s.entities.Select(i => i.input)).Include(s => s.entities.Select(i => i.output))
                                  .Include(s => s.projects)
                                  .Include(s => s.resources).Where(s => s.id == id).FirstOrDefault();
            }
        }

        public ICollection<Model> findAll()
        {
            IList<Model> mdls;

            using (var ctx = new SaprSimDbContext())
            {
                mdls = ctx.models.ToList();
            }

            return mdls;
        }

        public void removeById(int id)
        {
            using (var ctx = new SaprSimDbContext())
            {
                Model model = ctx.models.Where(s => s.id == id).FirstOrDefault();
                ctx.models.Remove(model);
            }
        }

        public void removeAll()
        {
            using (var ctx = new SaprSimDbContext())
            {
                ctx.models.RemoveRange(ctx.models);
            }
        }

        public DbContext getContext()
        {
            return new SaprSimDbContext();
        }
    }
}
