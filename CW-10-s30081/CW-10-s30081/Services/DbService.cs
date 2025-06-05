using Microsoft.EntityFrameworkCore;

using CW_10_s30081.Data;
using CW_10_s30081.DTOs;
using System;
using System.Collections.Generic;
using CW_10_s30081.Exceptions;
using CW_10_s30081.Models;

namespace CW_10_s30081.Services {
    
    public interface IDbService {
        public Task<TripPagedGetDTO> GetTripsWithClientsAndCountriesOrderedByDate(int page,int pageSize);
        public Task RemoveClient(int id);

        public Task AssignClientToTrip(int id, TripReservationPostDTO tripReservationPostDTO);
    }
    
    public class DbService(MsdbContext data) : IDbService {
        public async Task AssignClientToTrip(int id, TripReservationPostDTO tripReservationPostDTO) {
            var client = await data.Clients.Where(x => x.Pesel.Equals(tripReservationPostDTO.Pesel)).FirstAsync();
            var trip = await data.Trips.Where(x => x.IdTrip == id).Include(t => t.ClientTrips).FirstAsync();

            if (client == null)
                throw new NotFoundException($"Client with PESEL: {tripReservationPostDTO.Pesel} doesn't exist");

            if (trip==null)
                throw new NotFoundException($"Trip {trip.IdTrip} doesn't exist");

            if (trip.ClientTrips.Any(x => x.IdClient == client.IdClient))
                throw new NotFoundException($"Client {client.IdClient} is already assigned to this trip");
            
            if(trip.DateFrom < DateTime.Now)
                throw new NotFoundException($"Trip {trip.IdTrip} has already began");


            data.ClientTrips.Add(new ClientTrip {
                IdClient = client.IdClient,
                IdTrip = trip.IdTrip,
                PaymentDate = tripReservationPostDTO.PaymentDate.HasValue ? tripReservationPostDTO.PaymentDate.Value : null,
                RegisteredAt = DateTime.Now,
            });
            await data.SaveChangesAsync();

        }

        public async Task<TripPagedGetDTO> GetTripsWithClientsAndCountriesOrderedByDate(int page, int pageSize) {
            var count = await data.Trips.CountAsync();
            if (pageSize < 0)
                pageSize = count;

            int index = (page - 1) * pageSize;
            var result = await data.Trips.OrderBy(x=>x.DateFrom).Skip(index).Take(pageSize).ToListAsync();
            
            return new TripPagedGetDTO {
                AllPages = count / pageSize,
                PageNumber = page,
                PageSize = pageSize,
                TripsInPage = result.Select(x => new TripWithClientsAndCountiresGetDTO {
                    IdTrip = x.IdTrip,
                    Name = x.Name,
                    Description = x.Description,
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    MaxPeople = x.MaxPeople,
                    
                    Clients = data.Clients
                    .Include(x=>x.ClientTrips)
                    .Select(x=>new ShortClientGetDTO {
                        FirstName = x.FirstName,
                        LastName = x.LastName
                    }).ToList(),
                    Countries = data.Countries.Where(z=>z.Trips.All(z=>z.IdTrip==x.IdTrip)).Select(z=>z.Name).ToList()

                }).ToList()
            };
        }

        public async Task RemoveClient(int id) {
            if(await data.Trips.Select(x=>x.ClientTrips.Where(z=>z.IdClient==id)).CountAsync() > 0) 
                throw new NotFoundException($"Klient id {id} należy do choć jednej wycieczki");

            var client = await data.Clients.FindAsync(id);
            if (client != null) {
                data.Clients.Remove(client);
                await data.SaveChangesAsync();
            }

        }
    }
}
