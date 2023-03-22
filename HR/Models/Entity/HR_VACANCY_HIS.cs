﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HR.Models.Entity
{
    [Table("HR_VACANCY_HIS", Schema = "dbo")]
    public partial class HR_VACANCY_HIS
    {
        [Key]
        public int HVH_HIS_SN { get; set; }
        [Key]
        public int HVH_HVM_VAC_SN { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime HVH_OPN_DAT { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime HVH_CLS_DAT { get; set; }

        [ForeignKey("HVH_HVM_VAC_SN")]
        [InverseProperty("HR_VACANCY_HIS")]
        public virtual HR_VACMST HVH_HVM_VAC_SNNavigation { get; set; }
    }
}