using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRH.Firmware {
    public interface IFirmware {
        List<AdministratorLogin> GetPasswords(byte[] data);
    }
}
