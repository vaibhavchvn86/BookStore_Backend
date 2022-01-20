using ModelLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interface
{
    public interface IAddressManager
    {
        Task<AddressModel> AddAddress(AddressModel add);
        Task<AddressModel> UpdateAddress(AddressModel edit);
        Task<bool> DeleteAddress(AddressModel del);
        IEnumerable<AddressModel> GetallAddress();
        Task<AddressModel> GetByAddressType(string addtypeId);
    }
}
