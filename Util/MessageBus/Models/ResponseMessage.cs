﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.MessageBus.Models
{
    public class ResponseMessage
    {
        public string SerializableResponse { get; set; }
        public ResponseMessage()
        {
        
        }
    }
}
