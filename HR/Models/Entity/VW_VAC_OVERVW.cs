﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HR.Models.Entity
{
    [Keyless]
    public partial class VW_VAC_OVERVW
    {
        public int HVM_VAC_SN { get; set; }
        [Required]
        [StringLength(50)]
        public string HVM_VAC_NAM { get; set; }
        public int ITVW_CNT { get; set; }
        public int ADM_CNT { get; set; }
        public int CHKIN_CNT { get; set; }
        public bool HVM_VAC_STS { get; set; }
    }
}