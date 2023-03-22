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
    public partial class VW_HUMIFO_MST
    {
        public int HHI_HUM_SN { get; set; }
        [Required]
        [StringLength(50)]
        public string HHI_HUM_NAM { get; set; }
        public int HHI_HUM_SEX_SN { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string HHI_HUM_TEL { get; set; }
        public int HHI_HUM_SRC_SN { get; set; }
        [StringLength(50)]
        public string HHI_HUM_SRC { get; set; }
        public int HHI_HUM_STS_SN { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime HHI_HUM_ADD_DAT { get; set; }
        [StringLength(50)]
        public string HHI_HUM_ADD_USR { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime HHI_HUM_CHG_DAT { get; set; }
        [StringLength(50)]
        public string HHI_HUM_CHG_USR { get; set; }
        [Required]
        [StringLength(25)]
        [Unicode(false)]
        public string HHI_PPL_USR { get; set; }
        [StringLength(50)]
        public string HHI_PPL_USR_NAM { get; set; }
        public bool HHI_HUM_BLK { get; set; }
    }
}