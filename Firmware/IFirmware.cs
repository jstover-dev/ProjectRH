using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRH {
    public interface IFirmware {
        List<AdministratorPassword> GetPasswords(byte[] data);
    }
}
