using HotelApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApi
{
    public class HotelRepository: IDisposable
    {
        HotelDbContext ctx = new HotelDbContext();

        public HotelRepository()
        {
            ctx.Database.Log = (sql) =>
            {
                Debug.WriteLine("\t> " + sql);
            };
        }

        public Hotel FindById(int id)
        {
            return ctx
                    .Hotel
                    .Include(h => h.HotelBuchung)
                    .Where(h => h.HotelId == id)
                    .FirstOrDefault();

        }

        public IQueryable<Hotel> FindAll()
        {
            return ctx.Hotel.Where(h => h.RegionId == 1);
        }

        public List<Hotel> FindByRegion(int regionId)
        {
            return ctx
                    .Hotel
                    .Where(h => h.RegionId == regionId)
                    .ToList();
        }

        public void SaveHotel(Hotel hotel)
        {
            if (hotel.HotelId == 0) {
                ctx.Hotel.Add(hotel);
            }
            else {
                ctx.Hotel.Attach(hotel);
                ctx.Entry(hotel).State = EntityState.Modified;
                
                foreach(var entry in ctx.ChangeTracker.Entries())
                {
                    // entry.State
                    var entity = entry.Entity as IEntity;
                    if (entity != null)
                    {
                        entry.State = entity.State;
                    }
                }

            }

            ctx.SaveChanges();
        }

        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
