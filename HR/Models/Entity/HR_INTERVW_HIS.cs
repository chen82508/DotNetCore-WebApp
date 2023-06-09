﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HR.Models.Entity
{
    [Table("HR_INTERVW_HIS", Schema = "dbo")]
    public partial class HR_INTERVW_HIS
    {
        [Key]
        public int HIH_ITV_HIS_SN { get; set; }
        [Key]
        public int HIH_HHI_HUM_SN { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime HIH_ITV_DAT { get; set; }
        public string HIH_ITV_TXT { get; set; }
        [Required]
        [StringLength(25)]
        [Unicode(false)]
        public string HIH_ADD_USR { get; set; }

        [ForeignKey("HIH_HHI_HUM_SN")]
        [InverseProperty("HR_INTERVW_HIS")]
        public virtual HR_HUMIFO_MST HIH_HHI_HUM_SNNavigation { get; set; }
    }
}