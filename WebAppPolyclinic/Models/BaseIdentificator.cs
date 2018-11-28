﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public abstract class BaseIdentificator
    {
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ID { get; set; }
    }
}