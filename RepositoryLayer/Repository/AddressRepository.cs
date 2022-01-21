using ModelLayer;
using MongoDB.Driver;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IMongoCollection<AddressModel> Address;

        public AddressRepository(IDBSetting db)
        {
            var client = new MongoClient(db.ConnectionString);
            var database = client.GetDatabase(db.DatabaseName);
            Address = database.GetCollection<AddressModel>("Address");
        }
        public async Task<AddressModel> AddAddress(AddressModel add)
        {
            try
            {
                var check = await this.Address.Find(x => x.addressID == add.addressID).SingleOrDefaultAsync();
                if (check == null)
                {
                    await this.Address.InsertOneAsync(add);
                    return add;

                }
                return null;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<AddressModel> UpdateAddress(AddressModel edit)
        {
            try
            {
                var check = await this.Address.Find(x => x.addressID == edit.addressID).FirstOrDefaultAsync();
                if (check != null)
                {
                    await this.Address.UpdateOneAsync(x => x.addressID == edit.addressID,
                        Builders<AddressModel>.Update.Set(x => x.fullAddress, edit.fullAddress)
                        .Set(x => x.city, edit.city)
                        .Set(x => x.state, edit.state)
                        .Set(x => x.pinCode, edit.pinCode));
                    return check;

                }
                return null;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteAddress(AddressModel del)
        {
            try
            {
                var check = await this.Address.FindOneAndDeleteAsync(x => x.addressID == del.addressID);
                return true;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<AddressModel> GetallAddress()
        {
            return this.Address.Find(FilterDefinition<AddressModel>.Empty).ToList();
        }

        public async Task<AddressModel> GetByAddressType(string addtypeId)
        {
            return await this.Address.Find(x => x.addTypeID == addtypeId).FirstOrDefaultAsync();
        }

    }
}
