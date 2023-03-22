﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HR.Models.Entity
{
    [Table("HR_VACMST", Schema = "dbo")]
    public partial class HR_VACMST
    {
        public HR_VACMST()
        {
            HR_VACANCY_HIS = new HashSet<HR_VACANCY_HIS>();
            HR_VACTYP_RELATE = new HashSet<HR_VACTYP_RELATE>();
        }

        [Key]
        public int HVM_VAC_SN { get; set; }
        [Required]
        [StringLength(50)]
        public string HVM_VAC_NAM { get; set; }
        public bool HVM_VAC_STS { get; set; }
        public int HVM_CMP_SN { get; set; }
        [Required]
        [StringLength(1)]
        [Unicode(false)]
        public string HVM_ACT_TYP { get; set; }
        public int HVM_DAY_IDT { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? HVM_CLS_DAT { get; set; }
        [StringLength(255)]
        public string HVM_CLS_RSN { get; set; }

        [InverseProperty("HVH_HVM_VAC_SNNavigation")]
        public virtual ICollection<HR_VACANCY_HIS> HR_VACANCY_HIS { get; set; }
        [InverseProperty("HVT_HVM_VAC_SNNavigation")]
        public virtual ICollection<HR_VACTYP_RELATE> HR_VACTYP_RELATE { get; set; }
    }
}