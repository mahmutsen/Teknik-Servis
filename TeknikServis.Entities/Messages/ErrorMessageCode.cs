using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Entities.Messages
{
    public enum ErrorMessageCode
    {
        DeviceAlreadyExist=101,
        WorkerIsNotActive = 151,
        EmailOrPassWrong = 152,
        ServiceIdWrong = 153,
        WorkerNotFound=201,
        EmailAlreadyExist=202,
        ProfileCouldNotUpdated=203,
        UserNameAlreadyExist=204,
        DeviceCouldNotInserted = 301,
        DeviceCouldNotFound=170,
        SessionCouldNotFound=171,
        FeeCouldNotInserted=181,
        FeeDoesNotExist=182,
        CargoCouldNotInserted = 191,
        CargoDoesNotExist = 192,

        WorkerAlreadyActive = 154,
        ActivateIdDoesNotExists = 155,
        WorkerCouldNotRemove = 158,
        WorkerCouldNotFind = 159,
        WorkerCouldNotInserted = 160,
        WorkerCouldNotUpdated = 161,
        
    }
}
