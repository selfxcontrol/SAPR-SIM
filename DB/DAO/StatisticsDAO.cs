using Statistics.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DAO
{
    public class StatisticsDAO : DAO<AbstractStatiscticsBean>
    {

        public void save(AbstractStatiscticsBean value)
        {
            using (var ctx = new SaprSimDbContext())
            {
                ctx.statistics.Add(value);
                ctx.SaveChanges();
            }
        }

        public void saveAll(ICollection<AbstractStatiscticsBean> values)
        {
            using (var ctx = new SaprSimDbContext())
            {
                foreach (AbstractStatiscticsBean stat in values)
                {
                    ctx.statistics.Add(stat);
                }

                ctx.SaveChanges();
            }
        }

        public AbstractStatiscticsBean findById(int id)
        {
            using (var ctx = new SaprSimDbContext())
            {
                return ctx.statistics.Where(s => s.id == id).FirstOrDefault();
            }
        }

        public ICollection<AbstractStatiscticsBean> findAll()
        {
            IList<AbstractStatiscticsBean> stat;

            using (var ctx = new SaprSimDbContext())
            {
                stat = ctx.statistics.ToList();
            }

            return stat;
        }

        public void removeById(int id)
        {
            using (var ctx = new SaprSimDbContext())
            {
                AbstractStatiscticsBean stat = ctx.statistics.Where(s => s.id == id).FirstOrDefault();
                ctx.statistics.Remove(stat);
            }
        }

        public void removeAll()
        {
            using (var ctx = new SaprSimDbContext())
            {
                ctx.statistics.RemoveRange(ctx.statistics);
            }
        }

        public System.Data.Entity.DbContext getContext()
        {
            return new SaprSimDbContext();
        }
    }
}
